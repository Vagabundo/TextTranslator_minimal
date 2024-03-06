using MinimalTranslator.Application.Interfaces;
using MinimalTranslator.Core.Domain;

namespace MinimalTranslator.Application.Services;

public class TranslationService : ITranslationService
{
    private readonly ITranslationRepository _translationRepository;

    public TranslationService (ITranslationRepository translationRepository)
    {
        _translationRepository = translationRepository;
    }

    public async Task Add(Translation translation)
    {
        await _translationRepository.Add(translation);
    }

    public async Task<Translation> Get(Guid id)
    {
        return await _translationRepository.Get(id);
    }
}
