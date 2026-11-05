using Sergin.Platform.Application.Localizations;
using Sergin.Platform.Domain.Languages;
using Sergin.SharedKernel.Application.Commands;

namespace Sergin.Platform.Application.Languages.Commands.Rename;

internal sealed class RenameLanguageCommandHandler(
    IPlatformUnitOfWork unitOfWork,
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
