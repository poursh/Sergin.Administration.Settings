using Sergin.SharedKernel.Application.Commands;

namespace Sergin.Platform.Application.Contracts.Languages.Commands.Unpublish;

public sealed record UnpublishLanguageCommand(Guid Id) : ICommand<UnpublishLanguageCommandResponse>;
