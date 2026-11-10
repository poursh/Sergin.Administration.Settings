using Sergin.Administration.Settings.Domain.Languages;
using Sergin.SharedKernel.Application.Aggregates;
using Sergin.SharedKernel.Application.Commands.Queries;

namespace Sergin.Administration.Settings.Application.Contracts.Languages.Commands.GetAudit;

/// <summary>A language's audit stamps — the detail view's information dialog. A language belongs to no scope.</summary>
public sealed record GetLanguageAuditQueryCommand(LanguageId Id) : IQuery<AuditInfo>, IAuditQuery<Language, LanguageId>;
