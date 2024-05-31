using MinimalTranslator.SharedKernel;

namespace MinimalTranslator.Domain.Translation;

public static class TranslationErrors
{
    public static readonly Error NoText = Error.Validation(
        "Translation.Validation", $"Text can't be empty");
    public static readonly Error NoTargetLanguage = Error.Validation(
        "Translation.Validation", $"TargetLanguage can't be empty");
    public static readonly Error NoTranslatedText = Error.Validation(
        "Translation.Validation", $"Translated text can't be empty");
    public static Error InvalidId(string id) => Error.Validation(
        "Translation.Validation", $"Provided translation id '{id}' is invalid");
    public static Error NotFound(Guid translationId, string language) => Error.NotFound(
        "Translation.NotFound", $"The translation with Id = '{translationId}' to language '{language}' was not found");
    public static Error TranslationNotUnique(Guid translationId, string language) => Error.Conflict(
        "Translation.Conflict", $"A translation with Id = '{translationId}' to language '{language}' already exists");
}