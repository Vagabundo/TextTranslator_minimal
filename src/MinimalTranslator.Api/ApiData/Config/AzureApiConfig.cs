namespace MinimalTranslator.Api.Config;

public class AzureApiConfig 
{
    public AzureConfig TextAnalytics { get; set; }
    public AzureConfig TextTranslator { get; set; }
};
public class AzureConfig 
{
    public string Uri { get; set; }
    public string Key { get; set; }
};
