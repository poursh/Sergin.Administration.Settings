using Sergin.Platform.Domain.Languages;
using Sergin.SharedKernel.Application.Commands;
using Sergin.SharedKernel.Application.Localizations;

namespace Sergin.Platform.Application.Languages.Commands.Add;

// The cache holds the language list from its first load on, so every language write refreshes it once the save has
// committed.
internal sealed class AddLanguageCommandHandler(
    IPlatformUnitOfWork unitOfWork,
    ILanguageRepository repository,
    ITranslationCache cache) : ICommandHandler<AddLanguageCommand, AddLanguageCommandResponse>
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
        await cache.RefreshLanguagesAsync(cancellationToken);

        return new AddLanguageCommandResponse(language.Value.Id.Value);
    }
}
