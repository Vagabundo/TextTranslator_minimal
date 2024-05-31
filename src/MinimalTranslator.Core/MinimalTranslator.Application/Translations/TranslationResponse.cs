namespace MinimalTranslator.Application.Translations;

public sealed record TranslationResponse
{
    public string? OriginalText { get; init; }
    public string? LanguageFrom { get; init; }
    public string? TranslatedText { get; init; }
    public string? LanguageTo { get; init; }
}