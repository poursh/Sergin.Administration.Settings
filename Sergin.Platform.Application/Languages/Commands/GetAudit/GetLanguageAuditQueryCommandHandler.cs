using Sergin.Platform.Application.Contracts.Languages.Commands.GetAudit;
using Sergin.Platform.Domain.Languages;
using Sergin.SharedKernel.Application.Aggregates;
using Sergin.SharedKernel.Application.Commands.Queries;

namespace Sergin.Platform.Application.Languages.Commands.GetAudit;

internal sealed class GetLanguageAuditQueryCommandHandler(IGetLanguageAuditQueryRepository repository)
    : IQueryHandler<GetLanguageAuditQueryCommand, AuditInfo>
{
    public async Task<ErrorOr<AuditInfo>> Handle(GetLanguageAuditQueryCommand request, CancellationToken cancellationToken)
    {
        AuditInfo? audit = await repository.GetLanguageAuditAsync(new LanguageId(request.Id), cancellationToken);

        if (audit is null)
        {
            return Error.NotFound();
        }

        return audit;
    }
}
