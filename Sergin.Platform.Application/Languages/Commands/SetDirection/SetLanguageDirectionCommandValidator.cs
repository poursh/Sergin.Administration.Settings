using FluentValidation;

namespace Sergin.Platform.Application.Languages.Commands.SetDirection;

internal sealed class SetLanguageDirectionCommandValidator : AbstractValidator<SetLanguageDirectionCommand>
{
    public SetLanguageDirectionCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.Direction).IsInEnum();
    }
}
