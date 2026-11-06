using Sergin.Platform.Domain.Languages;
using Sergin.SharedKernel.Application.Aggregates;

namespace Sergin.Platform.Application.Languages.Commands.GetAudit;

public interface IGetLanguageAuditQueryRepository
{
    Task<AuditInfo?> GetLanguageAuditAsync(LanguageId id, CancellationToken cancellationToken = default);
}
