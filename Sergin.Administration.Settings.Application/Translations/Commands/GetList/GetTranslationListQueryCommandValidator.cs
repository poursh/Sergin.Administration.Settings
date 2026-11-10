using FluentValidation;
using Sergin.Administration.Settings.Application.Contracts.Translations.Commands.GetList;
using Sergin.SharedKernel.Application.Validations;

namespace Sergin.Administration.Settings.Application.Translations.Commands.GetList;

/// <summary>
/// Refuses any sort or filter (<see cref="TranslationListFields"/> declares none: the list's filters are its own
/// properties) and a status that is not one.
/// </summary>
internal sealed class GetTranslationListQueryCommandValidator : ListQueryValidator<GetTranslationListQueryCommand>
{
    public GetTranslationListQueryCommandValidator()
        : base(TranslationListFields.Set)
    {
        RuleFor(x => x.ParentId.Value).NotEmpty().OverridePropertyName(nameof(GetTranslationListQueryCommand.ParentId));

        RuleFor(x => x.Status).IsInEnum();
    }
}
