using MinimalTranslator.Application.Abstractions.Services;
using MinimalTranslator.Application.Services.Azure.Data;
using MinimalTranslator.SharedKernel;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

namespace MinimalTranslator.Application.Services.Azure;

public class AzureHttpTextAnalyticService : AzureHttpServiceBase, ITextAnalyticService
{
    public AzureHttpTextAnalyticService (string uri, string region, string key, float languageRecognitionScoreThreshold)
    {
        _uri = uri;
        _region = region;
        _key = key;
        _languageRecognitionScoreThreshold = languageRecognitionScoreThreshold;
    }

    public async Task<Result<string>> GetLanguage(string text)
    {
        string route = "/detect?api-version=3.0";

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
                return Result.Failure<string>(DetectedLanguageResponseErrors.ExternalFailure);
            }

            var result = await response.Content.ReadFromJsonAsync<IReadOnlyList<DetectedLanguageResponse>>();

            if (result is null || !result.Any() || result.First().Score < _languageRecognitionScoreThreshold)
            {
                return Result.Failure<string>(DetectedLanguageResponseErrors.LanguageNotRecognized);
            }

            var language = result?.First().Language;
            if (string.IsNullOrEmpty(language))
            {
                return Result.Failure<string>(DetectedLanguageResponseErrors.EmptyLanguage);
            }

            return language;
        }
    }
}