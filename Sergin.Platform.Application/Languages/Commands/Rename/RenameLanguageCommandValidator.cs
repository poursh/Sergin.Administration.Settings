using FluentValidation;
using Sergin.Platform.Domain.Languages;

namespace Sergin.Platform.Application.Languages.Commands.Rename;

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
