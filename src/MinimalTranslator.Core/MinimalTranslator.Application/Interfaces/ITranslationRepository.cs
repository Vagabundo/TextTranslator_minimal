using MinimalTranslator.Domain;

namespace MinimalTranslator.Application.Interfaces;

public interface ITranslationRepository
{
    Task Add(Translation translation);
    Task<Translation?> Get(Guid id);
}
