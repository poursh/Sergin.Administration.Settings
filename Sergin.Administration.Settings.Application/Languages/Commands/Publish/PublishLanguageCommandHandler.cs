using Sergin.Administration.Settings.Application.Localizations;
using Sergin.Administration.Settings.Domain.Languages;
using Sergin.SharedKernel.Application.Commands;

namespace Sergin.Administration.Settings.Application.Languages.Commands.Publish;

internal sealed class PublishLanguageCommandHandler(
    ISettingsUnitOfWork unitOfWork,
    ILanguageRepository repository,
    TranslationCacheRefresher refresher) : ICommandHandler<PublishLanguageCommand, PublishLanguageCommandResponse>
{
    public async Task<ErrorOr<PublishLanguageCommandResponse>> Handle(PublishLanguageCommand request, CancellationToken cancellationToken)
    {
        Language? language = await repository.GetAsync(request.Id, cancellationToken);

        if (language is null)
        {
            return Error.NotFound();
        }

        language.Publish();

        await unitOfWork.SaveChangesAsync(cancellationToken);
        await refresher.LanguagesChangedAsync();

        return new PublishLanguageCommandResponse(language.Id.Value);
    }
}
