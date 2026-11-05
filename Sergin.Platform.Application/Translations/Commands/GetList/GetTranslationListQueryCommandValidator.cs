using FluentValidation;
using Sergin.Platform.Application.Contracts.Translations.Commands.GetList;
using Sergin.SharedKernel.Application.Validations;

namespace Sergin.Platform.Application.Translations.Commands.GetList;

/// <summary>
/// Refuses any sort or filter (<see cref="TranslationListFields"/> declares none: the list's filters are its own
/// properties) and a status that is not one.
/// </summary>
internal sealed class GetTranslationListQueryCommandValidator : ListQueryValidator<GetTranslationListQueryCommand>
{
    public GetTranslationListQueryCommandValidator()
        : base(TranslationListFields.Set)
    {
        RuleFor(x => x.LanguageId.Value).NotEmpty().OverridePropertyName(nameof(GetTranslationListQueryCommand.LanguageId));

        RuleFor(x => x.Status).IsInEnum();
    }
}
