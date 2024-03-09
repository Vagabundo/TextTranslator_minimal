using Microsoft.EntityFrameworkCore;
using MinimalTranslator.Application.Interfaces;
using MinimalTranslator.Domain;

namespace MinimalTranslator.Database.Repositories;

public class TranslationRepository : ITranslationRepository
{
    private readonly InMemoryContext _dbContext;

    public TranslationRepository(InMemoryContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task Add(Translation translation)
    {
        if (!string.IsNullOrEmpty(translation.TranslatedText) && (await Get(translation.Id)) is null)
        {
            await _dbContext.Translations.AddAsync(translation);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<Translation?> Get(Guid id)
    {
        return await _dbContext.Translations
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }
}
