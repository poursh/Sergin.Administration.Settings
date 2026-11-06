using Sergin.Administration.Settings.Domain.Languages;
using Sergin.SharedKernel.Application.Concurrency;

namespace Sergin.Administration.Settings.Application.Languages.Commands.GetOne;

public interface IGetLanguageQueryRepository
{
    Task<Versioned<LanguageQueryResponse>?> GetLanguageById(LanguageId id, CancellationToken cancellationToken = default);
}
