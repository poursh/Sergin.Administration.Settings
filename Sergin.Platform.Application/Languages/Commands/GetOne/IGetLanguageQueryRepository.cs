using Sergin.Platform.Domain.Languages;
using Sergin.SharedKernel.Application.Concurrency;

namespace Sergin.Platform.Application.Languages.Commands.GetOne;

public interface IGetLanguageQueryRepository
{
    Task<Versioned<LanguageQueryResponse>?> GetLanguageById(LanguageId id, CancellationToken cancellationToken = default);
}
