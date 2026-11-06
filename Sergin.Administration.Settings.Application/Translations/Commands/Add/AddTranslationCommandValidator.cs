using FluentValidation;
using Sergin.Administration.Settings.Application.Configurations.Languages;
using Sergin.Administration.Settings.Application.Configurations.Translations;
using Sergin.Administration.Settings.Application.Contracts.Translations.Commands.Add;
using Sergin.Administration.Settings.Domain.Languages;
using Sergin.Administration.Settings.Domain.Translations;
using Sergin.SharedKernel.Application.Localizations;
using Sergin.SharedKernel.Application.Localizations.Validation;
using Sergin.SharedKernel.Application.Validations;

namespace Sergin.Administration.Settings.Application.Translations.Commands.Add;

// A key the catalog declares, not yet translated in the language (ix_translation_language_id_key is the guarantee under
// a race), and a text with the default's placeholders.
internal sealed class AddTranslationCommandValidator : AbstractValidator<AddTranslationCommand>
{
    public AddTranslationCommandValidator(ILanguageRepository languages, ITranslationRepository translations, TextCatalog catalog)
    {
        RuleFor(x => x.LanguageId).MustExistIn(languages, LanguageTexts.Singular);

        RuleFor(x => x.Key).NotEmpty();

        RuleFor(x => x.Key)
            .Must(key => catalog.TryGet(key, out _))
            .WithMessage(_ => ValidationTexts.Template(TranslationTexts.UnknownKey))
            .When(x => !string.IsNullOrEmpty(x.Key));

        RuleFor(x => x.Key)
            .MustAsync(async (command, key, cancellationToken) =>
                await translations.GetAsync(command.LanguageId, new TranslationKey(key), cancellationToken) is null)
            .WithMessage(_ => ValidationTexts.Template(TranslationTexts.AlreadyTranslated))
            .When(x => !string.IsNullOrEmpty(x.Key) && catalog.TryGet(x.Key, out _));

        RuleFor(x => x.Text).MustBeATranslationText();

        RuleFor(x => x.Text)
            .MustKeepThePlaceholdersOf((command, _) =>
                Task.FromResult(catalog.TryGet(command.Key ?? string.Empty, out TextDefinition? definition) ? definition : null))
            .When(x => !string.IsNullOrEmpty(x.Text));
    }
}
