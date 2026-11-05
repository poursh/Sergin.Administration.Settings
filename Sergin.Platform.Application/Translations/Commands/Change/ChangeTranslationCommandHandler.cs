using Sergin.Platform.Application.Configurations.Translations;
using Sergin.Platform.Application.Contracts.Translations.Commands.Change;
using Sergin.Platform.Application.Localizations;
using Sergin.Platform.Domain.Languages;
using Sergin.Platform.Domain.Translations;
using Sergin.SharedKernel.Application.Commands;
using Sergin.SharedKernel.Application.Localizations;
using Sergin.SharedKernel.Domain.Localizations;

namespace Sergin.Platform.Application.Translations.Commands.Change;

// Sent at the version the translator loaded (RequireExpectedVersion): of two translators editing one row, the second
// save is stale and the first edit survives. The source becomes the default as it reads now, clearing a review flag.
internal sealed class ChangeTranslationCommandHandler(
    IPlatformUnitOfWork unitOfWork,
    ITranslationRepository repository,
    ILanguageRepository languages,
    TextCatalog catalog,
    TranslationCacheRefresher refresher) : ICommandHandler<ChangeTranslationCommand, ChangeTranslationCommandResponse>
{
    public async Task<ErrorOr<ChangeTranslationCommandResponse>> Handle(ChangeTranslationCommand request, CancellationToken cancellationToken)
    {
        Translation? translation = await repository.GetAsync(request.Id, cancellationToken);

        if (translation is null)
        {
            return Error.NotFound();
        }

        // The validator refuses an orphaned row; this is the guard for a caller that skips it.
        if (!catalog.TryGet(translation.Key.Value, out TextDefinition? definition))
        {
            return Error.Validation(
                nameof(ChangeTranslationCommand.Id), TranslationTexts.UnknownKey.Default, ErrorText.Of(TranslationTexts.UnknownKey.Key.Value));
        }

        Language? language = await languages.GetAsync(translation.LanguageId, cancellationToken);

        translation.Change(new TranslationText(request.Text), new SourceText(definition.Default));

        await unitOfWork.SaveChangesAsync(cancellationToken);

        if (language is not null)
        {
            await refresher.TranslationsChangedAsync(language.Culture.Value);
        }

        return new ChangeTranslationCommandResponse(translation.Id.Value);
    }
}
