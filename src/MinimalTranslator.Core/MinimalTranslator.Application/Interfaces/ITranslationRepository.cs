using MinimalTranslator.Domain;

namespace MinimalTranslator.Application.Interfaces;

public interface ITranslationRepository
{
    Task<Translation> Add(Translation translation);
    Task<Translation?> Get(Guid id, string language);
}
