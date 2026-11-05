using Sergin.Platform.Domain.Languages;
using Sergin.SharedKernel.Application.Commands;
using Sergin.SharedKernel.Application.Localizations;

namespace Sergin.Platform.Application.Languages.Commands.Publish;

internal sealed class PublishLanguageCommandHandler(
    IPlatformUnitOfWork unitOfWork,
    ILanguageRepository repository,
    ITranslationCache cache) : ICommandHandler<PublishLanguageCommand, PublishLanguageCommandResponse>
{
    public async Task<ErrorOr<PublishLanguageCommandResponse>> Handle(PublishLanguageCommand request, CancellationToken cancellationToken)
    {
        Language? language = await repository.GetAsync(new LanguageId(request.Id), cancellationToken);

        if (language is null)
        {
            return Error.NotFound();
        }

        language.Publish();

        await unitOfWork.SaveChangesAsync(cancellationToken);
        await cache.RefreshLanguagesAsync(cancellationToken);

        return new PublishLanguageCommandResponse(language.Id.Value);
    }
}
