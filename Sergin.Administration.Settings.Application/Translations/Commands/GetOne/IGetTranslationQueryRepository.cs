using Sergin.Administration.Settings.Application.Contracts.Translations.Commands.GetOne;
using Sergin.Administration.Settings.Domain.Translations;
using Sergin.SharedKernel.Application.Concurrency;

namespace Sergin.Administration.Settings.Application.Translations.Commands.GetOne;

public interface IGetTranslationQueryRepository
{
    Task<Versioned<TranslationQueryResponse>?> GetTranslationById(TranslationId id, CancellationToken cancellationToken = default);
}
