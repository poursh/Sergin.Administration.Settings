using Sergin.Administration.Settings.Application.Localizations;
using Sergin.Administration.Settings.Domain.Languages;
using Sergin.SharedKernel.Application.Commands;

namespace Sergin.Administration.Settings.Application.Languages.Commands.Add;

// The cache holds the language list from its first load on, so every language write refreshes it once the save has
// committed, through TranslationCacheRefresher so a failing refresh never turns the saved write into an error.
internal sealed class AddLanguageCommandHandler(
    ISettingsUnitOfWork unitOfWork,
    ILanguageRepository repository,
    TranslationCacheRefresher refresher) : ICommandHandler<AddLanguageCommand, AddLanguageCommandResponse>
{
    public async Task<ErrorOr<AddLanguageCommandResponse>> Handle(AddLanguageCommand request, CancellationToken cancellationToken)
    {
        ErrorOr<Language> language = Language.Create(request.Culture, request.DisplayName);

        if (language.IsError)
        {
            return language.Errors;
        }

        repository.Insert(language.Value);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        await refresher.LanguagesChangedAsync();

        return new AddLanguageCommandResponse(language.Value.Id.Value);
    }
}
