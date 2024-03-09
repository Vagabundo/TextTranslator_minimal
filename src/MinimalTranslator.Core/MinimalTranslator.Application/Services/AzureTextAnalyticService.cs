using Azure;
using Azure.AI.TextAnalytics;
using MinimalTranslator.Application.Interfaces;

namespace MinimalTranslator.Application.Services;

public class AzureTextAnalyticService : ITextAnalyticService
{
    private TextAnalyticsClient _azureTextAnalyticsClient;

    public AzureTextAnalyticService (string uri, string key)
    {
        _azureTextAnalyticsClient = new TextAnalyticsClient(
            new Uri(uri),
            new AzureKeyCredential(key)
        );
    }

    public async Task<string> GetLanguage(string text)
    {
        var response = await _azureTextAnalyticsClient.DetectLanguageAsync(text);        
        return response.Value.Iso6391Name;
    }
}
