namespace MinimalTranslator.Application.Data.Azure;

// https://learn.microsoft.com/en-us/azure/ai-services/translator/reference/v3-0-detect#response-body
public class DetectedLanguageResponse : DetectedLanguage
{
    public IEnumerable<DetectedLanguage> Alternatives { get; set; }
}

public class DetectedLanguage : LanguageScore
{
    public bool IsTranslationSupported { get; set; }
    public bool IsTransliterationSupported { get; set; }
}

public class LanguageScore
{
    public string Language { get; set; }
    public float Score { get; set; }
}