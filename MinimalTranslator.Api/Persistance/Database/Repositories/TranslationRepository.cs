using Microsoft.Extensions.Caching.Distributed;
using MinimalTranslator.Application.Interfaces;
using MinimalTranslator.Core.Domain;

namespace MinimalTranslator.Persistance.Database;

public class TranslationCacheRepository : ITranslationRepository
{
    private readonly IDistributedCache _cacheDb;

    public TranslationCacheRepository (IDistributedCache cacheDb)
    {
        _cacheDb = cacheDb;
    }

    public async Task Add(Translation translation)
    {
        if (!string.IsNullOrEmpty(translation.TranslatedText))
        {
            await _cacheDb.SetStringAsync(translation.Id.ToString(), translation.TranslatedText);
        }
    }

    public async Task<Translation> Get(Guid id)
    {
        return new () { Id = id, TranslatedText = await _cacheDb.GetStringAsync(id.ToString()) };
    }
}
