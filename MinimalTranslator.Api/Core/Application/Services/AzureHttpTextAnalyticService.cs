using System.Text;
using Newtonsoft.Json;
using MinimalTranslator.Application.Interfaces;
using MinimalTranslator.Api.Data;
using Microsoft.Extensions.Options;
using MinimalTranslator.Api.Data.Azure;

namespace MinimalTranslator.Application.Services;

public class AzureHttpTextAnalyticService : ITextAnalyticService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly AzureHttpConfig _settings;

    public AzureHttpTextAnalyticService (IHttpClientFactory clientFactory, IOptions<AzureHttpConfig> settings)
    {
        _clientFactory = clientFactory;
        _settings = settings.Value;
    }

    public async Task<string> GetLanguage(string text)
    {
        string route = "/detect?api-version=3.0";

        object[] body = new object[] { new { Text = text } };
        var requestBody = JsonConvert.SerializeObject(body);

        using (var client = _clientFactory.CreateClient())
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