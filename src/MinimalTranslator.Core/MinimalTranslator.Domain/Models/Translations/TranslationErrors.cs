using MinimalTranslator.SharedKernel;

namespace MinimalTranslator.Domain.Translations;

public static class TranslationErrors
{
    public static readonly Error NoText = Error.Validation(
        "Translation.Validation", $"Text can't be empty");
    public static readonly Error NoTargetLanguage = Error.Validation(
        "Translation.Validation", $"TargetLanguage can't be empty");
    public static readonly Error NoTranslatedText = Error.Validation(
        "Translation.Validation", $"Translated text can't be empty");
    public static Error NotFound(Guid translationId, string language) => Error.NotFound(
        "Translation.NotFound", $"The translation with the Id = '{translationId}' to language {language} was not found");
    public static Error TranslationNotUnique(Guid translationId, string language) => Error.Conflict(
        "Translation.Conflict", $"A translation with the Id = '{translationId}' to language '{language}' already exists");
}