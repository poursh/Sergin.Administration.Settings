using Sergin.Administration.Settings.Domain.Languages;
using Sergin.SharedKernel.Application.Commands;

namespace Sergin.Administration.Settings.Application.Contracts.Languages.Commands.SetDirection;

public sealed record SetLanguageDirectionCommand(LanguageId Id, LanguageDirection Direction)
    : ICommand<SetLanguageDirectionCommandResponse>, ISetPropertyCommand<Language, LanguageId>;
