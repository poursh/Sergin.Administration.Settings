using Sergin.Platform.Domain.Languages;

namespace Sergin.Platform.Application.Contracts.Languages.Commands.GetList;

public sealed record GetLanguageListItem(Guid Id, string Culture, string DisplayName, bool IsRightToLeft, LanguageStatus Status);
