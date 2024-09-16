using MinimalTranslator.Domain.Translations;

namespace MinimalTranslator.Application.Abstractions.Events;

public abstract record TranslationEvent(Guid Id, Translation Translation) : ITranslationEvent;