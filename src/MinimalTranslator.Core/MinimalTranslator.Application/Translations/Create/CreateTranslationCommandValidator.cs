using FluentValidation;

namespace MinimalTranslator.Application.Translations.Create;

public class CreateTranslationCommandValidator : AbstractValidator<CreateTranslationCommand>
{
    public CreateTranslationCommandValidator()
    {
        RuleFor(c => c.Text).NotEmpty();
        RuleFor(c => c.TargetLanguage).NotEmpty();
    }
}