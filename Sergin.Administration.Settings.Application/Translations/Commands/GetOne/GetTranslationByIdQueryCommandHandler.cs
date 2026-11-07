using Sergin.Administration.Settings.Application.Contracts.Translations.Commands.GetOne;
using Sergin.SharedKernel.Application.Concurrency;

namespace Sergin.Administration.Settings.Application.Translations.Commands.GetOne;

// The version goes out beside the response, through the scope's ConcurrencyContext, never on the record.
internal sealed class GetTranslationByIdQueryCommandHandler(IGetTranslationQueryRepository repository, ConcurrencyContext concurrency)
    : VersionedQueryHandler<GetTranslationByIdQueryCommand, TranslationQueryResponse>(concurrency)
{
    public override async Task<ErrorOr<Versioned<TranslationQueryResponse>>> HandleVersioned(
        GetTranslationByIdQueryCommand request, CancellationToken cancellationToken)
    {
        Versioned<TranslationQueryResponse>? res = await repository.GetTranslationById(request.Id, cancellationToken);

        if (res is null)
        {
            return Error.NotFound();
        }

        return res;
    }
}
