namespace MinimalTranslator.Api.Config;

public class AzureHttpConfig 
{
    public string Uri { get; set; }
    public string Region { get; set; }
    public string Key { get; set; }
    public float LanguageRecognitionScoreThreshold { get; set; }
};
