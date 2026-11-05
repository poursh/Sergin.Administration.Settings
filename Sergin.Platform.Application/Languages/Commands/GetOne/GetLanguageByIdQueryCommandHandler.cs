using Sergin.Platform.Domain.Languages;
using Sergin.SharedKernel.Application.Concurrency;

namespace Sergin.Platform.Application.Languages.Commands.GetOne;

// The version goes out beside the response, through the scope's ConcurrencyContext, never on the record.
internal sealed class GetLanguageByIdQueryCommandHandler(IGetLanguageQueryRepository repository, ConcurrencyContext concurrency)
    : VersionedQueryHandler<GetLanguageByIdQueryCommand, LanguageQueryResponse>(concurrency)
{
    public override async Task<ErrorOr<Versioned<LanguageQueryResponse>>> HandleVersioned(
        GetLanguageByIdQueryCommand request, CancellationToken cancellationToken)
    {
        Versioned<LanguageQueryResponse>? res = await repository.GetLanguageById(new LanguageId(request.Id), cancellationToken);

        if (res is null)
        {
            return Error.NotFound();
        }

        return res;
    }
}
