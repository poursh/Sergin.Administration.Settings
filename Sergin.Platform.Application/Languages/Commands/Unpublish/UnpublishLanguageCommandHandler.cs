using Sergin.Platform.Application.Localizations;
using Sergin.Platform.Domain.Languages;
using Sergin.SharedKernel.Application.Commands;

namespace Sergin.Platform.Application.Languages.Commands.Unpublish;

internal sealed class UnpublishLanguageCommandHandler(
    IPlatformUnitOfWork unitOfWork,
    ILanguageRepository repository,
    TranslationCacheRefresher refresher) : ICommandHandler<UnpublishLanguageCommand, UnpublishLanguageCommandResponse>
{
    public async Task<ErrorOr<UnpublishLanguageCommandResponse>> Handle(UnpublishLanguageCommand request, CancellationToken cancellationToken)
    {
        Language? language = await repository.GetAsync(new LanguageId(request.Id), cancellationToken);

        if (language is null)
        {
            return Error.NotFound();
        }

        language.Unpublish();

        await unitOfWork.SaveChangesAsync(cancellationToken);
        await refresher.LanguagesChangedAsync();

        return new UnpublishLanguageCommandResponse(language.Id.Value);
    }
}
