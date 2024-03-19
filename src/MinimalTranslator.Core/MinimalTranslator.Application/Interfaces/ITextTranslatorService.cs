using MinimalTranslator.SharedKernel;

namespace MinimalTranslator.Application.Interfaces;

public interface ITextTranslatorService
{
    Task<Result<string>> Translate (string text, string sourceLanguage, string targetLanguage);
}