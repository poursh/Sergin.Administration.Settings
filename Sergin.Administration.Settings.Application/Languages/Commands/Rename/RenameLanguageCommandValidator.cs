using FluentValidation;
using Sergin.Administration.Settings.Domain.Languages;

namespace Sergin.Administration.Settings.Application.Languages.Commands.Rename;

internal sealed class RenameLanguageCommandValidator : AbstractValidator<RenameLanguageCommand>
{
    public RenameLanguageCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.DisplayName.Value)
            .NotEmpty()
            .MaximumLength(LanguageDisplayName.MaxLength)
            .OverridePropertyName(nameof(RenameLanguageCommand.DisplayName));
    }
}
