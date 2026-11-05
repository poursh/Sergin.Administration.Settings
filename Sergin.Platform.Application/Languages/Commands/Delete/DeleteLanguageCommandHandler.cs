using Sergin.Platform.Application.Localizations;
using Sergin.Platform.Domain.Languages;
using Sergin.SharedKernel.Application.Commands;

namespace Sergin.Platform.Application.Languages.Commands.Delete;

// A hard delete: Language is not soft-deletable, so its culture is free again at once. Its translations go in the same
// statement, through the cascading foreign key from pf.translation, and the culture's cache entry is refreshed (to
// nothing) with the language list.
internal sealed class DeleteLanguageCommandHandler(
    IPlatformUnitOfWork unitOfWork,
    ILanguageRepository repository,
    TranslationCacheRefresher refresher) : ICommandHandler<DeleteLanguageCommand, DeleteLanguageCommandResponse>
{
    public async Task<ErrorOr<DeleteLanguageCommandResponse>> Handle(DeleteLanguageCommand request, CancellationToken cancellationToken)
    {
        Language? language = await repository.GetAsync(new LanguageId(request.Id), cancellationToken);

        if (language is null)
        {
            return Error.NotFound();
        }

        repository.Remove(language);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        await refresher.LanguagesChangedAsync();
        await refresher.TranslationsChangedAsync(language.Culture.Value);

        return new DeleteLanguageCommandResponse(language.Id.Value);
    }
}
