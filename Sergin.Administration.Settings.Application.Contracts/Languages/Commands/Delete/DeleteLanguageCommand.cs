using Sergin.SharedKernel.Application.Commands;

namespace Sergin.Administration.Settings.Application.Contracts.Languages.Commands.Delete;

public sealed record DeleteLanguageCommand(Guid Id) : ICommand<DeleteLanguageCommandResponse>;
