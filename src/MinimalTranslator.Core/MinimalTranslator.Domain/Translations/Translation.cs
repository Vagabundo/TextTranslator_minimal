namespace MinimalTranslator.Domain.Translation;

public sealed record Translation : Entity
{
    public string? OriginalText { set; get; }
    public string? LanguageFrom { set; get; }
    public string? TranslatedText { set; get; }
    public string? LanguageTo { set; get; }
}
