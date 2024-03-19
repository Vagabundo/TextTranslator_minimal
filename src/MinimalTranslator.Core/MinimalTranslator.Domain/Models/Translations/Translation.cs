using System.ComponentModel.DataAnnotations;

namespace MinimalTranslator.Domain;

public class Translation
{
    [Key]
    public Guid Id { set; get; }
    public string OriginalText { set; get; }
    public string LanguageFrom { set; get; }
    public string TranslatedText { set; get; }
    public string LanguageTo { set; get; }
}
