using Sergin.SharedKernel.Application.Commands;

namespace Sergin.Administration.Settings.Application.Contracts.Languages.Commands.Unpublish;

public sealed record UnpublishLanguageCommand(Guid Id) : ICommand<UnpublishLanguageCommandResponse>;
