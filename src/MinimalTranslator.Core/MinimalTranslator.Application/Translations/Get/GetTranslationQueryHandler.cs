using System.Data;
using Dapper;
using MinimalTranslator.Application.Abstractions.Data;
using MinimalTranslator.Application.Abstractions.Messaging;
using MinimalTranslator.Domain.Translation;
using MinimalTranslator.SharedKernel;

namespace MinimalTranslator.Application.Translations.Get;

internal sealed class GetTranslationByIdQueryHandler : IQueryHandler<GetTranslationQuery, TranslationResponse>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public GetTranslationByIdQueryHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
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

        using IDbConnection connection = _dbConnectionFactory.CreateOpenConnection();

        const string sql =
            """
            SELECT t."OriginalText", t."LanguageFrom", t."TranslatedText", t."LanguageTo"
            FROM "Translations" t
            WHERE t."Id" = @Id AND t."LanguageTo" = @Language
            """;

        TranslationResponse? translation = await connection.QueryFirstOrDefaultAsync<TranslationResponse>(
            sql,
            new { Id = guid, request.Language });

        if (translation is null)
        {
            return Result.Failure<TranslationResponse>(TranslationErrors.NotFound(guid, request.Language));
        }

        return translation;
    }
}