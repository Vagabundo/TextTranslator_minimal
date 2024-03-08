using System.Net.Http.Json;
using System.Text;
using MinimalTranslator.Application.Config;
using MinimalTranslator.Application.Data.Azure;
using MinimalTranslator.Application.Interfaces;
using Newtonsoft.Json;

namespace MinimalTranslator.Application.Services;

public class AzureHttpTextTranslatorService : ITextTranslatorService
{
    private readonly AzureHttpConfig _settings;

    public AzureHttpTextTranslatorService (AzureHttpConfig settings)
    {
        _settings = settings;
    }

    public async Task<string> Translate(string text, string sourceLanguage, string targetLanguage)
    {
        string route = $"/translate?api-version=3.0&from={sourceLanguage}&to={targetLanguage}";

        object[] body = { new { Text = text } };
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

            var result = await response.Content.ReadFromJsonAsync<IReadOnlyList<TranslatedTextResponse>>();

            return result?.First().Translations.First().Text ?? "";
        }
    }
}