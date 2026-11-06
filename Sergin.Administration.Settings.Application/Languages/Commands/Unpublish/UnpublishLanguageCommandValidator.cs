using FluentValidation;

namespace Sergin.Administration.Settings.Application.Languages.Commands.Unpublish;

internal sealed class UnpublishLanguageCommandValidator : AbstractValidator<UnpublishLanguageCommand>
{
    public UnpublishLanguageCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
