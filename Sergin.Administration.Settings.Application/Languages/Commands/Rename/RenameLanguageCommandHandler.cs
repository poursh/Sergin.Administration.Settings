using Sergin.Administration.Settings.Application.Localizations;
using Sergin.Administration.Settings.Domain.Languages;
using Sergin.SharedKernel.Application.Commands;

namespace Sergin.Administration.Settings.Application.Languages.Commands.Rename;

internal sealed class RenameLanguageCommandHandler(
    ISettingsUnitOfWork unitOfWork,
    ILanguageRepository repository,
    TranslationCacheRefresher refresher) : ICommandHandler<RenameLanguageCommand, RenameLanguageCommandResponse>
{
    public async Task<ErrorOr<RenameLanguageCommandResponse>> Handle(RenameLanguageCommand request, CancellationToken cancellationToken)
    {
        Language? language = await repository.GetAsync(new LanguageId(request.Id), cancellationToken);

        if (language is null)
        {
            return Error.NotFound();
        }

        language.Rename(request.DisplayName);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        await refresher.LanguagesChangedAsync();

        return new RenameLanguageCommandResponse(language.Id.Value);
    }
}
