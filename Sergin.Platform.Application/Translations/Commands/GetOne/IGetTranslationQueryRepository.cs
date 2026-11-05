using Sergin.Platform.Application.Contracts.Translations.Commands.GetOne;
using Sergin.Platform.Domain.Translations;
using Sergin.SharedKernel.Application.Concurrency;

namespace Sergin.Platform.Application.Translations.Commands.GetOne;

public interface IGetTranslationQueryRepository
{
    Task<Versioned<TranslationQueryResponse>?> GetTranslationById(TranslationId id, CancellationToken cancellationToken = default);
}
