using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.GetAudit;
using Sergin.SharedKernel.Application.Aggregates;
using Sergin.SharedKernel.Application.Commands.Queries;

namespace Sergin.Administration.Settings.Application.Languages.Commands.GetAudit;

internal sealed class GetLanguageAuditQueryCommandHandler(IGetLanguageAuditQueryRepository repository)
    : IQueryHandler<GetLanguageAuditQueryCommand, AuditInfo>
{
    public async Task<ErrorOr<AuditInfo>> Handle(GetLanguageAuditQueryCommand request, CancellationToken cancellationToken)
    {
        AuditInfo? audit = await repository.GetLanguageAuditAsync(request.Id, cancellationToken);

        if (audit is null)
        {
            return Error.NotFound();
        }

        return audit;
    }
}
