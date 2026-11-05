using Sergin.Platform.Application.Translations.Commands.GetOne;

namespace Sergin.Platform.Application.Translations;

public interface ITranslationAllQueryRepository :
    IGetTranslationQueryRepository,
    ITranslationRowQueryRepository;
