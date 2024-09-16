namespace MinimalTranslator.Application.Services.Azure.Data.Config;

public record AzureApiConfig 
{
    public AzureConfig? TextAnalytics { get; set; }
    public AzureConfig? TextTranslator { get; set; }
};
public record AzureConfig 
{
    public string? Uri { get; set; }
    public string? Key { get; set; }
};
