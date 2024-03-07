using Azure;
using Azure.AI.TextAnalytics;
using MinimalTranslator.Application.Interfaces;

namespace MinimalTranslator.Application.Services;

public class AzureTextAnalyticService : ITextAnalyticService
{
    private TextAnalyticsClient _azureTextAnalyticsClient;

    public AzureTextAnalyticService (IConfiguration configuration)
    {
        _azureTextAnalyticsClient = new TextAnalyticsClient(
        new Uri(configuration.GetValue<string>("Azure:API:TextAnalytics:Uri")),
        new AzureKeyCredential(configuration.GetValue<string>("Azure:API:TextAnalytics:Key")));
    }

    public async Task<string> GetLanguage(string text)
    {
        var response = await _azureTextAnalyticsClient.DetectLanguageAsync(text);        
        return response.Value.Iso6391Name;
    }
}
