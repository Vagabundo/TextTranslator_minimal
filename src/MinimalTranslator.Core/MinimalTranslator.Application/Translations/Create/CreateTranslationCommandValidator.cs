using FluentValidation;
using MinimalTranslator.Domain.Translations;

namespace MinimalTranslator.Application.Translations.Create;

public class CreateTranslationCommandValidator : AbstractValidator<CreateTranslationCommand>
{
    public CreateTranslationCommandValidator()
    {
        RuleFor(c => c.Text).NotEmpty()
            .WithMessage(TranslationErrors.NoText.Description)
            .WithErrorCode(TranslationErrors.NoText.Code);
        RuleFor(c => c.TargetLanguage).NotEmpty()
            .WithMessage(TranslationErrors.NoTargetLanguage.Description)
            .WithErrorCode(TranslationErrors.NoTargetLanguage.Code);
    }
}