using System.Data;
using Dapper;
using MinimalTranslator.Application.Abstractions.Data;
using MinimalTranslator.Application.Abstractions.Messaging;
using MinimalTranslator.Domain.Translations;
using MinimalTranslator.SharedKernel;

namespace MinimalTranslator.Application.Translations.Get;

internal sealed class GetTranslationByIdQueryHandler : IQueryHandler<GetTranslationQuery, TranslationResponse>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ICacheService _cacheService;

    public GetTranslationByIdQueryHandler(IDbConnectionFactory dbConnectionFactory, ICacheService cacheService)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _cacheService = cacheService;
    }

    public async Task<Result<TranslationResponse>> Handle(GetTranslationQuery request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.TranslationId, out var guid))
        {
            return Result.Failure<TranslationResponse>(TranslationErrors.InvalidId(request.TranslationId));
        }
        
        if (string.IsNullOrEmpty(request.Language))
        {
            return Result.Failure<TranslationResponse>(TranslationErrors.NoTargetLanguage);
        }

        var translation = await GetFromRedis(request, cancellationToken);
        //var translation = await GetFromSql(guid, request.Language);

        if (translation is null)
        {
            return Result.Failure<TranslationResponse>(TranslationErrors.NotFound(guid, request.Language));
        }

        return translation;
    }

    private async Task<TranslationResponse?> GetFromRedis(GetTranslationQuery request, CancellationToken cancellationToken)
    {
        return await _cacheService.GetAsync<TranslationResponse>($"translation/{request.TranslationId}/{request.Language}", cancellationToken);
    }

    private async Task<TranslationResponse?> GetFromSql(Guid guid, string language)
    {
        using IDbConnection connection = _dbConnectionFactory.CreateOpenConnection();

        const string sql =
            """
            SELECT t."OriginalText", t."LanguageFrom", t."TranslatedText", t."LanguageTo"
            FROM "Translations" t
            WHERE t."Id" = @Id AND t."LanguageTo" = @Language
            """;

        return await connection.QueryFirstOrDefaultAsync<TranslationResponse>(
            sql,
            new { Id = guid, Language = language });
    }
}