using Sergin.Platform.Domain.Languages;
using Sergin.SharedKernel.Application.Commands;

namespace Sergin.Platform.Application.Contracts.Languages.Commands.SetDirection;

public sealed record SetLanguageDirectionCommand(Guid Id, LanguageDirection Direction) : ICommand<SetLanguageDirectionCommandResponse>;
