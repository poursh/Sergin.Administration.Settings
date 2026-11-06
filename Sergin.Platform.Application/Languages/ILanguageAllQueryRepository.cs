using Sergin.Platform.Application.Languages.Commands.GetAudit;
using Sergin.Platform.Application.Languages.Commands.GetList;
using Sergin.Platform.Application.Languages.Commands.GetOne;

namespace Sergin.Platform.Application.Languages;

public interface ILanguageAllQueryRepository :
    IGetLanguageListQueryRepository,
    IGetLanguageQueryRepository,
    IGetLanguageAuditQueryRepository;
