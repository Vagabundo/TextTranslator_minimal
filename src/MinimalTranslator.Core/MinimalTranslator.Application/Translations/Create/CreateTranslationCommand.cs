using MinimalTranslator.Application.Abstractions.Messaging;

namespace MinimalTranslator.Application.Translations.Create;

public sealed record CreateTranslationCommand(string Text, string TargetLanguage)
    : ICommand<Guid>;