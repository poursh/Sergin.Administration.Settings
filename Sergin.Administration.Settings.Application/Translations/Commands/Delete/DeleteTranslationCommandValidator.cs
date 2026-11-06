using FluentValidation;
using Sergin.Administration.Settings.Application.Configurations.Translations;
using Sergin.Administration.Settings.Application.Contracts.Translations.Commands.Delete;
using Sergin.Administration.Settings.Domain.Translations;
using Sergin.SharedKernel.Application.Localizations;
using Sergin.SharedKernel.Application.Localizations.Validation;

namespace Sergin.Administration.Settings.Application.Translations.Commands.Delete;

// Only an orphaned translation may go. An unknown id passes here: the handler answers not found.
internal sealed class DeleteTranslationCommandValidator : AbstractValidator<DeleteTranslationCommand>
{
    public DeleteTranslationCommandValidator(ITranslationRepository translations, TextCatalog catalog)
    {
        RuleFor(x => x.Id.Value).NotEmpty().OverridePropertyName(nameof(DeleteTranslationCommand.Id));

        RuleFor(x => x.Id)
            .MustAsync(async (id, cancellationToken) =>
                await translations.GetAsync(id, cancellationToken) is not { } translation || !catalog.TryGet(translation.Key.Value, out _))
            .WithMessage(_ => ValidationTexts.Template(TranslationTexts.NotOrphaned))
            .When(x => x.Id.Value != Guid.Empty);
    }
}
