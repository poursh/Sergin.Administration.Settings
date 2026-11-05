using Sergin.SharedKernel.Application.Commands.Queries;

namespace Sergin.Platform.Application.Languages.Commands.GetList;

public interface IGetLanguageListQueryRepository
{
    Task<ListQueryResponse<GetLanguageListItem>> GetListAsync(ListQuery query, CancellationToken cancellationToken = default);
}
