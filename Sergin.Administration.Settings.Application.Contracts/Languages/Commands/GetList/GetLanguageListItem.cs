using Sergin.Administration.Settings.Domain.Languages;

namespace Sergin.Administration.Settings.Application.Contracts.Languages.Commands.GetList;

public sealed record GetLanguageListItem(Guid Id, string Culture, string DisplayName, bool IsRightToLeft, LanguageStatus Status);
