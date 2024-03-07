using System.Text;
using Microsoft.Extensions.Options;
using MinimalTranslator.Api.Data;
using MinimalTranslator.Api.Data.Azure;
using MinimalTranslator.Application.Interfaces;
using Newtonsoft.Json;

namespace MinimalTranslator.Application.Services;

public class AzureHttpTextTranslatorService : ITextTranslatorService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly AzureHttpConfig _settings;

    public AzureHttpTextTranslatorService (IHttpClientFactory clientFactory, IOptions<AzureHttpConfig> settings)
    {
        _clientFactory = clientFactory;
        _settings = settings.Value;
    }

    public async Task<string> Translate(string text, string sourceLanguage, string targetLanguage)
    {
        string route = $"/translate?api-version=3.0&from={sourceLanguage}&to={targetLanguage}";

        object[] body = { new { Text = text } };
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

            var result = await response.Content.ReadFromJsonAsync<IReadOnlyList<TranslatedTextResponse>>();

            return result?.First().Translations.First().Text ?? "";
        }
    }
}