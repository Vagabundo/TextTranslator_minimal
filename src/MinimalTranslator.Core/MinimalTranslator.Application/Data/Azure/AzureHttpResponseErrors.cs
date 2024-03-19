using MinimalTranslator.SharedKernel;

namespace MinimalTranslator.Application.Data.Azure;

public static class DetectedLanguageResponseErrors
{
    public static readonly Error ExternalFailure = Error.Failure(
        "DetectedLanguageResponse.ExternalFailure", "External service failed to detect text language");
    public static readonly Error LanguageNotRecognized = Error.Validation(
        "DetectedLanguageResponse.LanguageNotRecognized", "The Text Analytic Service couldn't recognize the original language");
    public static readonly Error EmptyLanguage = Error.Validation(
        "DetectedLanguageResponse.EmptyLanguage", "The language is empty");
}

public static class TranslatedTextResponseErrors
{
    public static readonly Error ExternalFailure = Error.Failure(
        "TranslatedTextResponse.ExternalFailure", "External service failed to translate text");
    public static readonly Error LanguageNotRecognized = Error.Validation(
        "TranslatedTextResponse.LanguageNotRecognized", "The translator service couldn't recognize the original language");
    public static readonly Error NoTranslation = Error.Validation(
        "TranslatedTextResponse.NoTranslation", "The translator service couldn't translate the text");
    public static readonly Error EmptyTranslation = Error.Validation(
        "TranslatedTextResponse.EmptyTranslation", "The translation is empty");
}
