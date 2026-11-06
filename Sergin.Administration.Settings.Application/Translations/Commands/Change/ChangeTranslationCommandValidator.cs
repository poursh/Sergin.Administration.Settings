using FluentValidation;
using Sergin.Administration.Settings.Application.Configurations.Translations;
using Sergin.Administration.Settings.Application.Contracts.Translations.Commands.Change;
using Sergin.Administration.Settings.Domain.Translations;
using Sergin.SharedKernel.Application.Localizations;
using Sergin.SharedKernel.Application.Localizations.Validation;

namespace Sergin.Administration.Settings.Application.Translations.Commands.Change;

// The row's key must still be declared (an orphan is deleted, not edited), and the text keep its default's
// placeholders. An unknown id passes here: the handler answers not found. The row is loaded through the scope's
// context, so the second lookup and the handler's are served from its change tracker.
internal sealed class ChangeTranslationCommandValidator : AbstractValidator<ChangeTranslationCommand>
{
    public ChangeTranslationCommandValidator(ITranslationRepository translations, TextCatalog catalog)
    {
        RuleFor(x => x.Id.Value).NotEmpty().OverridePropertyName(nameof(ChangeTranslationCommand.Id));

        RuleFor(x => x.Id)
            .MustAsync(async (id, cancellationToken) =>
                await translations.GetAsync(id, cancellationToken) is not { } translation || catalog.TryGet(translation.Key.Value, out _))
            .WithMessage(_ => ValidationTexts.Template(TranslationTexts.UnknownKey))
            .When(x => x.Id.Value != Guid.Empty);

        RuleFor(x => x.Text).MustBeATranslationText();

        RuleFor(x => x.Text)
            .MustKeepThePlaceholdersOf(async (command, cancellationToken) =>
                await translations.GetAsync(command.Id, cancellationToken) is { } translation
                && catalog.TryGet(translation.Key.Value, out TextDefinition? definition)
                    ? definition
                    : null)
            .When(x => x.Id.Value != Guid.Empty && !string.IsNullOrEmpty(x.Text));
    }
}
