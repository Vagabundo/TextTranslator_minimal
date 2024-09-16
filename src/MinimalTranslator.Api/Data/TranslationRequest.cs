namespace MinimalTranslator.Api.Data;

public record TranslationRequest
{
    public string? Text { get; init; }
    public string? TargetLanguage { get; init; }
}