using Sergin.Administration.Settings.Domain.Languages;

namespace Sergin.Administration.Settings.Application.Contracts.Languages.Commands.GetOne;

/// <summary>One language; <see cref="IsRightToLeft"/> is the resolved direction, <see cref="Direction"/> the setting behind it.</summary>
public sealed record LanguageQueryResponse(
    Guid Id, string Culture, string DisplayName, LanguageDirection Direction, bool IsRightToLeft, LanguageStatus Status);
