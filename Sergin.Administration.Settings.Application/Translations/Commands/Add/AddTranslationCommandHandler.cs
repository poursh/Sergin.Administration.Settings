using Sergin.Administration.Settings.Application.Configurations.Translations;
using Sergin.Administration.Settings.Application.Contracts.Translations.Commands.Add;
using Sergin.Administration.Settings.Application.Localizations;
using Sergin.Administration.Settings.Domain.Languages;
using Sergin.Administration.Settings.Domain.Translations;
using Sergin.SharedKernel.Application.Commands;
using Sergin.SharedKernel.Application.Localizations;
using Sergin.SharedKernel.Domain.Localizations;

namespace Sergin.Administration.Settings.Application.Translations.Commands.Add;

// The source stored is the catalog's default at save time, so a later change to the default flags the row for review.
// The culture's cache entry is refreshed once the save has committed, so the next render shows the translation.
internal sealed class AddTranslationCommandHandler(
    ISettingsUnitOfWork unitOfWork,
    ITranslationRepository repository,
    ILanguageRepository languages,
    TextCatalog catalog,
    TranslationCacheRefresher refresher) : ICommandHandler<AddTranslationCommand, AddTranslationCommandResponse>
{
    public async Task<ErrorOr<AddTranslationCommandResponse>> Handle(AddTranslationCommand request, CancellationToken cancellationToken)
    {
        Language? language = await languages.GetAsync(request.LanguageId, cancellationToken);

        if (language is null)
        {
            return Error.NotFound();
        }

        // The validator refuses an unknown key; this is the guard for a caller that skips it.
        if (!catalog.TryGet(request.Key, out TextDefinition? definition))
        {
            return Error.Validation(
                nameof(AddTranslationCommand.Key), TranslationTexts.UnknownKey.Default, ErrorText.Of(TranslationTexts.UnknownKey.Key.Value));
        }

        var translation = Translation.Create(
            request.LanguageId, new TranslationKey(request.Key), new TranslationText(request.Text), new SourceText(definition.Default));

        repository.Insert(translation);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        await refresher.TranslationsChangedAsync(language.Culture.Value);

        return new AddTranslationCommandResponse(translation.Id.Value);
    }
}
