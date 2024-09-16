using MinimalTranslator.Domain.Abstractions;
using MinimalTranslator.Domain.Translations.Events;

namespace MinimalTranslator.Domain.Translations;

public sealed class Translation : Entity
{
    public Text? OriginalText { get; private set; }
    public Language? LanguageFrom { get; private set; }
    public Text? TranslatedText { get; private set; }
    public Language? LanguageTo { get; private set; }

    private Translation()
    {
    }

    private Translation(Guid id, Text originalText, Language languageFrom, Text translatedText, Language languageTo) : base(id)
    {
        OriginalText = originalText;
        LanguageFrom = languageFrom;
        TranslatedText = translatedText;
        LanguageTo = languageTo;
    }

    public static Translation Create(Guid id, Text originalText, Language languageFrom, Text translatedText, Language languageTo)
    {
        var translation = new Translation(id, originalText, languageFrom, translatedText, languageTo);
        translation.Raise(new TranslationCreatedDomainEvent(translation.Id));
        
        return translation;
    }
}
