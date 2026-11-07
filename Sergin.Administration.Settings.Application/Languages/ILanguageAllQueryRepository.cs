using Sergin.Administration.Settings.Application.Languages.Commands.GetAudit;
using Sergin.Administration.Settings.Application.Languages.Commands.GetList;
using Sergin.Administration.Settings.Application.Languages.Commands.GetOne;

namespace Sergin.Administration.Settings.Application.Languages;

public interface ILanguageAllQueryRepository :
    IGetLanguageListQueryRepository,
    IGetLanguageQueryRepository,
    IGetLanguageAuditQueryRepository;
