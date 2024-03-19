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
    
    public async Task<Translation> Add(Translation translation)
    {
        await _dbContext.Translations.AddAsync(translation);
        await _dbContext.SaveChangesAsync();

        return translation;
    }

    public async Task<Translation?> Get(Guid id, string language)
    {
        return await _dbContext.Translations
            .AsNoTracking()
            .Where(x => x.Id == id && x.LanguageTo == language)
            .FirstOrDefaultAsync();
    }
}
