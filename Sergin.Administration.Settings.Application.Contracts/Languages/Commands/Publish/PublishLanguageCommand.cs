using Sergin.SharedKernel.Application.Commands;

namespace Sergin.Administration.Settings.Application.Contracts.Languages.Commands.Publish;

public sealed record PublishLanguageCommand(Guid Id) : ICommand<PublishLanguageCommandResponse>;
