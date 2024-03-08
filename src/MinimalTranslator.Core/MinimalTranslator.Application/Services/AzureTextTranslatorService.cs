using Azure;
using Azure.AI.Translation.Text;
using MinimalTranslator.Application.Config;
using MinimalTranslator.Application.Interfaces;

namespace MinimalTranslator.Application.Services;

public class AzureTextTranslatorService : ITextTranslatorService
{
    private TextTranslationClient _azureTextTranslatorClient;
    public AzureTextTranslatorService (AzureApiConfig config)
    {
        _azureTextTranslatorClient = new TextTranslationClient(
            new AzureKeyCredential(config.TextTranslator.Key),
            new Uri(config.TextTranslator.Uri)
        );
    }

    public async Task<string> Translate(string text, string sourceLanguage, string targetLanguage)
    {
        var response = await _azureTextTranslatorClient.TranslateAsync(targetLanguage, text, sourceLanguage);

        IReadOnlyList<TranslatedTextItem> translations = response.Value;

        return translations.FirstOrDefault()?.Translations?.FirstOrDefault()?.Text ?? "";
    }
}
