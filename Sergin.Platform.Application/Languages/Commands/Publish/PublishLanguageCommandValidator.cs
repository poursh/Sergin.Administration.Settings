using FluentValidation;

namespace Sergin.Platform.Application.Languages.Commands.Publish;

internal sealed class PublishLanguageCommandValidator : AbstractValidator<PublishLanguageCommand>
{
    public PublishLanguageCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
