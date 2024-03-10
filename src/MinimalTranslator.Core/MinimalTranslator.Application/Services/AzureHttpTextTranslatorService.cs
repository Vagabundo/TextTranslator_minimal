using System.Net.Http.Json;
using System.Text;
using MinimalTranslator.Application.Data.Azure;
using MinimalTranslator.Application.Interfaces;
using Newtonsoft.Json;

namespace MinimalTranslator.Application.Services;

public class AzureHttpTextTranslatorService : ITextTranslatorService
{
    private readonly string _uri;
    private readonly string _region;
    private readonly string _key;

    public AzureHttpTextTranslatorService (string uri, string region, string key)
    {
        _uri = uri;
        _region = region;
        _key = key;
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
            request.RequestUri = new Uri(_uri + route);
            request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            request.Headers.Add("Ocp-Apim-Subscription-Key", _key);
            request.Headers.Add("Ocp-Apim-Subscription-Region", _region);

            HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("External service failed to translate text");
            }

            var result = await response.Content.ReadFromJsonAsync<IReadOnlyList<TranslatedTextResponse>>();

            return result?.First().Translations.First().Text ?? "";
        }
    }
}