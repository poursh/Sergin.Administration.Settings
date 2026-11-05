using Sergin.SharedKernel.Application.Commands.Queries;

namespace Sergin.Platform.Application.Languages.Commands.GetList;

internal sealed class GetLanguageListQueryCommandHandler(IGetLanguageListQueryRepository queryRepository)
    : IListQueryHandler<GetLanguageListQueryCommand, GetLanguageListItem>
{
    public async Task<ErrorOr<ListQueryResponse<GetLanguageListItem>>> Handle(
        GetLanguageListQueryCommand request, CancellationToken cancellationToken)
    {
        ListQueryResponse<GetLanguageListItem> res = await queryRepository.GetListAsync(request, cancellationToken);

        return res;
    }
}
