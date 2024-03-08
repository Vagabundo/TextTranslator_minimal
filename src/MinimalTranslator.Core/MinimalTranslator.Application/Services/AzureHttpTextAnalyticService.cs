using System.Text;
using Newtonsoft.Json;
using MinimalTranslator.Application.Interfaces;
using MinimalTranslator.Application.Config;
using System.Net.Http.Json;
using MinimalTranslator.Application.Data.Azure;

namespace MinimalTranslator.Application.Services;

public class AzureHttpTextAnalyticService : ITextAnalyticService
{
    private readonly AzureHttpConfig _settings;

    public AzureHttpTextAnalyticService (AzureHttpConfig settings)
    {
        _settings = settings;
    }

    public async Task<string> GetLanguage(string text)
    {
        string route = "/detect?api-version=3.0";

        object[] body = new object[] { new { Text = text } };
        var requestBody = JsonConvert.SerializeObject(body);

        using (var client = new HttpClient())
        using (var request = new HttpRequestMessage())
        {
            request.Method = HttpMethod.Post;
            request.RequestUri = new Uri(_settings.Uri + route);
            request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            request.Headers.Add("Ocp-Apim-Subscription-Key", _settings.Key);
            request.Headers.Add("Ocp-Apim-Subscription-Region", _settings.Region);

            HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode) return "";

            var result = await response.Content.ReadFromJsonAsync<IReadOnlyList<DetectedLanguageResponse>>();

            return result?.First().Language ?? "";
        }
    }
}