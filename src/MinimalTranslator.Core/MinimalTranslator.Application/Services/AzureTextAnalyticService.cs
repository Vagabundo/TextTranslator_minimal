using Azure;
using Azure.AI.TextAnalytics;
using MinimalTranslator.Application.Config;
using MinimalTranslator.Application.Interfaces;

namespace MinimalTranslator.Application.Services;

public class AzureTextAnalyticService : ITextAnalyticService
{
    private TextAnalyticsClient _azureTextAnalyticsClient;

    public AzureTextAnalyticService (AzureApiConfig config)
    {
        _azureTextAnalyticsClient = new TextAnalyticsClient(
            new Uri(config.TextAnalytics.Uri),
            new AzureKeyCredential(config.TextAnalytics.Key)
        );
    }

    public async Task<string> GetLanguage(string text)
    {
        var response = await _azureTextAnalyticsClient.DetectLanguageAsync(text);        
        return response.Value.Iso6391Name;
    }
}
