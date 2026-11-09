using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.Publish;
using Sergin.Administration.Settings.Domain.Languages;
using Sergin.SharedKernel.Application.Commands;

namespace Sergin.Administration.Settings.Application.Contracts.Languages.Commands.Unpublish;

public sealed record UnpublishLanguageCommand(LanguageId Id)
    : ICommand<UnpublishLanguageCommandResponse>, ITransitionCommand<Language, LanguageId>, IReverseOf<PublishLanguageCommand>;
