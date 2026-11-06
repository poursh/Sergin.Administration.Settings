using Sergin.Administration.Settings.Domain.Languages;
using Sergin.SharedKernel.Application.Commands;

namespace Sergin.Administration.Settings.Application.Contracts.Languages.Commands.SetDirection;

public sealed record SetLanguageDirectionCommand(Guid Id, LanguageDirection Direction) : ICommand<SetLanguageDirectionCommandResponse>;
