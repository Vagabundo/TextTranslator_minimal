using MinimalTranslator.Application.Abstractions.Messaging;

namespace MinimalTranslator.Application.Translations.Get;

public sealed record GetTranslationQuery(string TranslationId, string Language) : IQuery<TranslationResponse>;