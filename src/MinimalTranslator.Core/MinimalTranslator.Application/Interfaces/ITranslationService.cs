using MinimalTranslator.Domain;

namespace MinimalTranslator.Application.Interfaces;

public interface ITranslationService
{
    Task<Guid> Add(string text, string targetLanguage);
    Task<Translation> Add(Translation translation);
    Task<Translation?> Get(Translation translation);
    Task<Translation?> Get(Guid id, string language);
    Task<Guid> GetHashedId(string text);
}
