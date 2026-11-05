using Sergin.Platform.Application.Localizations;
using Sergin.Platform.Domain.Languages;
using Sergin.SharedKernel.Application.Commands;

namespace Sergin.Platform.Application.Languages.Commands.SetDirection;

internal sealed class SetLanguageDirectionCommandHandler(
    IPlatformUnitOfWork unitOfWork,
    ILanguageRepository repository,
    TranslationCacheRefresher refresher) : ICommandHandler<SetLanguageDirectionCommand, SetLanguageDirectionCommandResponse>
{
    public async Task<ErrorOr<SetLanguageDirectionCommandResponse>> Handle(SetLanguageDirectionCommand request, CancellationToken cancellationToken)
    {
        Language? language = await repository.GetAsync(new LanguageId(request.Id), cancellationToken);

        if (language is null)
        {
            return Error.NotFound();
        }

        language.SetDirection(request.Direction);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        await refresher.LanguagesChangedAsync();

        return new SetLanguageDirectionCommandResponse(language.Id.Value);
    }
}
