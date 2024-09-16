namespace MinimalTranslator.Application.Abstractions.Events;

public interface IEventBus
{
    Task PublishAsync<T>(T translationEvent, CancellationToken cancellationToken = default)
        where T : class, ITranslationEvent;
}