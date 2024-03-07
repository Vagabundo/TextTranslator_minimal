namespace MinimalTranslator.Application.Interfaces;

public interface ITextTranslatorService
{
    Task<string> Translate (string text, string sourceLanguage, string targetLanguage);
}