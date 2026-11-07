using Sergin.Administration.Settings.Domain.Languages;
using Sergin.SharedKernel.Application.Aggregates;

namespace Sergin.Administration.Settings.Application.Languages.Commands.GetAudit;

public interface IGetLanguageAuditQueryRepository
{
    Task<AuditInfo?> GetLanguageAuditAsync(LanguageId id, CancellationToken cancellationToken = default);
}
