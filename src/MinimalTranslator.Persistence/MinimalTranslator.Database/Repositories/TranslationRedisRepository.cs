using Microsoft.Extensions.Caching.Distributed;
using MinimalTranslator.Application.Interfaces;
using MinimalTranslator.Domain;
using Newtonsoft.Json;

namespace MinimalTranslator.Database;

public class TranslationRedisRepository : ITranslationRepository
{
    private readonly IDistributedCache _cacheDb;

    public TranslationRedisRepository (IDistributedCache cacheDb)
    {
        _cacheDb = cacheDb;
    }

    public async Task Add(Translation translation)
    {
        if (!string.IsNullOrEmpty(translation.TranslatedText))
        {
            await _cacheDb.SetStringAsync(translation.Id.ToString(), JsonConvert.SerializeObject(translation));
        }
    }

    public async Task<Translation?> Get(Guid id)
    {
        string? cachedValue = await _cacheDb.GetStringAsync(id.ToString());

        return cachedValue is not null ? JsonConvert.DeserializeObject<Translation>(cachedValue) : null;
    }
}
