using MinimalTranslator.Domain;
using MinimalTranslator.SharedKernel;

namespace MinimalTranslator.Application.Interfaces;

public interface ITranslationService
{
    Task<Result<Guid>> Add(string? text, string? targetLanguage);
    Task<Result<Translation>> Get(Guid id, string? language);
}
