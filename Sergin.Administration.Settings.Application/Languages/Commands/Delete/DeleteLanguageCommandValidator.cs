using FluentValidation;

namespace Sergin.Administration.Settings.Application.Languages.Commands.Delete;

internal sealed class DeleteLanguageCommandValidator : AbstractValidator<DeleteLanguageCommand>
{
    public DeleteLanguageCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
