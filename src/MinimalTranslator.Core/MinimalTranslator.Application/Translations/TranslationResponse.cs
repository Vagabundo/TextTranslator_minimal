namespace MinimalTranslator.Application.Translations;

public sealed record TranslationResponse
(
    string? OriginalText,
    string? LanguageFrom,
    string? TranslatedText,
    string? LanguageTo
);