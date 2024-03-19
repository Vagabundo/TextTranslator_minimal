using System.Net.Http.Json;
using System.Text;
using MinimalTranslator.Application.Data.Azure;
using MinimalTranslator.Application.Interfaces;
using MinimalTranslator.SharedKernel;
using Newtonsoft.Json;

namespace MinimalTranslator.Application.Services;

public class AzureHttpTextTranslatorService : AzureHttpServiceBase, ITextTranslatorService
{
    public AzureHttpTextTranslatorService (string uri, string region, string key)
    {
        _uri = uri;
        _region = region;
        _key = key;
    }

    public async Task<Result<string>> Translate(string text, string sourceLanguage, string targetLanguage)
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
            request.Headers.Add(KeyHeaderName, _key);
            request.Headers.Add(RegionHeaderName, _region);

            HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                return Result.Failure<string>(TranslatedTextResponseErrors.ExternalFailure);
            }

            var result = await response.Content.ReadFromJsonAsync<IReadOnlyList<TranslatedTextResponse>>();

            if (result is null || !result.Any())
            {
                return Result.Failure<string>(TranslatedTextResponseErrors.NoTranslation);
            }

            var translation = result?.First().Translations.First().Text;
            if (string.IsNullOrEmpty(translation))
            {
                return Result.Failure<string>(TranslatedTextResponseErrors.EmptyTranslation);
            }

            return translation;
        }
    }
}