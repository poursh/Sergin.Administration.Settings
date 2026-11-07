using Sergin.Administration.Settings.Application.Configurations.Translations;
using Sergin.Administration.Settings.Application.Contracts.Translations.Commands.Delete;
using Sergin.Administration.Settings.Application.Localizations;
using Sergin.Administration.Settings.Domain.Languages;
using Sergin.Administration.Settings.Domain.Translations;
using Sergin.SharedKernel.Application.Commands;
using Sergin.SharedKernel.Application.Localizations;
using Sergin.SharedKernel.Domain.Localizations;

namespace Sergin.Administration.Settings.Application.Translations.Commands.Delete;

// Only an orphan: a translation of a declared key is changed, never deleted, so a language cannot lose a text by
// accident. A hard delete; the culture's cache entry is refreshed after the save.
internal sealed class DeleteTranslationCommandHandler(
    ISettingsUnitOfWork unitOfWork,
    ITranslationRepository repository,
    ILanguageRepository languages,
    TextCatalog catalog,
    TranslationCacheRefresher refresher) : ICommandHandler<DeleteTranslationCommand, DeleteTranslationCommandResponse>
{
    public async Task<ErrorOr<DeleteTranslationCommandResponse>> Handle(DeleteTranslationCommand request, CancellationToken cancellationToken)
    {
        Translation? translation = await repository.GetAsync(request.Id, cancellationToken);

        if (translation is null)
        {
            return Error.NotFound();
        }

        // The validator refuses a declared key; this is the guard for a caller that skips it.
        if (catalog.TryGet(translation.Key.Value, out _))
        {
            return Error.Validation(
                nameof(DeleteTranslationCommand.Id), TranslationTexts.NotOrphaned.Default, ErrorText.Of(TranslationTexts.NotOrphaned.Key.Value));
        }

        Language? language = await languages.GetAsync(translation.LanguageId, cancellationToken);

        repository.Remove(translation);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        if (language is not null)
        {
            await refresher.TranslationsChangedAsync(language.Culture.Value);
        }

        return new DeleteTranslationCommandResponse(translation.Id.Value);
    }
}
