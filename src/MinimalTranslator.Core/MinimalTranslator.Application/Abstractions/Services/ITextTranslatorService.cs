using MinimalTranslator.SharedKernel;

namespace MinimalTranslator.Application.Abstractions.Services;

public interface ITextTranslatorService
{
    Task<Result<string>> Translate (string text, string sourceLanguage, string targetLanguage);
}