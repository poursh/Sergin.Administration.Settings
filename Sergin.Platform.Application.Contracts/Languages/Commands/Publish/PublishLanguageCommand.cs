using Sergin.SharedKernel.Application.Commands;

namespace Sergin.Platform.Application.Contracts.Languages.Commands.Publish;

public sealed record PublishLanguageCommand(Guid Id) : ICommand<PublishLanguageCommandResponse>;
