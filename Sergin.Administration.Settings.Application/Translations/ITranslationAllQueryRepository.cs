using Sergin.Administration.Settings.Application.Translations.Commands.GetOne;

namespace Sergin.Administration.Settings.Application.Translations;

public interface ITranslationAllQueryRepository :
    IGetTranslationQueryRepository,
    ITranslationRowQueryRepository;
