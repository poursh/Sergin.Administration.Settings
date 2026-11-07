using Sergin.SharedKernel.Application.Validations;

namespace Sergin.Administration.Settings.Application.Languages.Commands.GetList;

/// <summary>Refuses a sort or filter the language list does not declare in <see cref="LanguageListFields"/>.</summary>
internal sealed class GetLanguageListQueryCommandValidator() : ListQueryValidator<GetLanguageListQueryCommand>(LanguageListFields.Set);
