namespace MinimalTranslator.Application.Services.Azure.Data;

// https://learn.microsoft.com/en-us/azure/ai-services/translator/reference/v3-0-detect#response-body
public record DetectedLanguageResponse : DetectedLanguage
{
    public IEnumerable<DetectedLanguage>? Alternatives { get; set; }
}

public record DetectedLanguage : LanguageScore
{
    public bool IsTranslationSupported { get; set; }
    public bool IsTransliterationSupported { get; set; }
}

public record LanguageScore
{
    public string? Language { get; set; }
    public float Score { get; set; }
}