using MinimalTranslator.Domain.Translations;

namespace MinimalTranslator.Application.Abstractions.Events;

public interface ITranslationEvent
{
    Guid Id { get; init; }
    Translation Translation { get; init; }
}