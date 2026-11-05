using System.Globalization;
using FluentValidation;
using Sergin.Platform.Application.Configurations.Languages;
using Sergin.Platform.Domain.Languages;
using Sergin.SharedKernel.Application.Localizations.Validation;
using Sergin.SharedKernel.Application.Validations;

namespace Sergin.Platform.Application.Languages.Commands.Add;

// The culture rules mirror CultureName.Normalise, so a form shows them as the user types. Uniqueness compares the
// normalised culture (LanguageRepository.IsTakenAsync); ix_language_culture is the guarantee under a race.
internal sealed class AddLanguageCommandValidator : AbstractValidator<AddLanguageCommand>
{
    public AddLanguageCommandValidator(ILanguageRepository languages)
    {
        RuleFor(x => x.Culture.Value)
            .NotEmpty()
            .MaximumLength(CultureName.MaxLength)
            .OverridePropertyName(nameof(AddLanguageCommand.Culture));

        RuleFor(x => x.Culture.Value)
            .Must(value => CultureName.Resolve(value) is not null)
            .WithMessage(_ => ValidationTexts.Template(LanguageTexts.UnknownCulture))
            .OverridePropertyName(nameof(AddLanguageCommand.Culture))
            .When(x => !string.IsNullOrWhiteSpace(x.Culture.Value) && x.Culture.Value.Length <= CultureName.MaxLength);

        RuleFor(x => x.Culture.Value)
            .Must(value => !CultureName.IsEnglish(CultureName.Resolve(value)!))
            .WithMessage(_ => ValidationTexts.Template(LanguageTexts.BuiltInEnglish))
            .OverridePropertyName(nameof(AddLanguageCommand.Culture))
            .When(x => CultureName.Resolve(x.Culture.Value) is not null);

        RuleFor(x => x.Culture)
            .MustBeUniqueIn(languages)
            .When(x => CultureName.Resolve(x.Culture.Value) is CultureInfo culture && !CultureName.IsEnglish(culture));

        RuleFor(x => x.DisplayName.Value)
            .NotEmpty()
            .MaximumLength(LanguageDisplayName.MaxLength)
            .OverridePropertyName(nameof(AddLanguageCommand.DisplayName));
    }
}
