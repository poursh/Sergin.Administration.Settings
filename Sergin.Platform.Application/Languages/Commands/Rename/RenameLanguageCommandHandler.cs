using Sergin.Platform.Domain.Languages;
using Sergin.SharedKernel.Application.Commands;
using Sergin.SharedKernel.Application.Localizations;

namespace Sergin.Platform.Application.Languages.Commands.Rename;

internal sealed class RenameLanguageCommandHandler(
    IPlatformUnitOfWork unitOfWork,
    ILanguageRepository repository,
    ITranslationCache cache) : ICommandHandler<RenameLanguageCommand, RenameLanguageCommandResponse>
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
        await cache.RefreshLanguagesAsync(cancellationToken);

        return new RenameLanguageCommandResponse(language.Id.Value);
    }
}
