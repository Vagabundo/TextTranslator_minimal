using System.Text;
using Newtonsoft.Json;
using MinimalTranslator.Application.Interfaces;
using System.Net.Http.Json;
using MinimalTranslator.Application.Data.Azure;

namespace MinimalTranslator.Application.Services;

public class AzureHttpTextAnalyticService : ITextAnalyticService
{
    private readonly string _uri;
    private readonly string _region;
    private readonly string _key;

    public AzureHttpTextAnalyticService (string uri, string region, string key)
    {
        _uri = uri;
        _region = region;
        _key = key;
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
            request.RequestUri = new Uri(_uri + route);
            request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            request.Headers.Add("Ocp-Apim-Subscription-Key", _key);
            request.Headers.Add("Ocp-Apim-Subscription-Region", _region);

            HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode) return "";

            var result = await response.Content.ReadFromJsonAsync<IReadOnlyList<DetectedLanguageResponse>>();

            return result?.First().Language ?? "";
        }
    }
}