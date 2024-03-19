namespace MinimalTranslator.Application.Services;

public class AzureHttpServiceBase
{
    protected const string KeyHeaderName = "Ocp-Apim-Subscription-Key";
    protected const string RegionHeaderName = "Ocp-Apim-Subscription-Region";
    protected string? _uri;
    protected string? _region;
    protected string? _key;
    protected float _languageRecognitionScoreThreshold;
}