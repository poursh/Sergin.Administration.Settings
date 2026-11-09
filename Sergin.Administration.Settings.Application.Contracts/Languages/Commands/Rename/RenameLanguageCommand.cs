using Sergin.Administration.Settings.Domain.Languages;
using Sergin.SharedKernel.Application.Commands;

namespace Sergin.Administration.Settings.Application.Contracts.Languages.Commands.Rename;

public sealed record RenameLanguageCommand(LanguageId Id, LanguageDisplayName DisplayName)
    : ICommand<RenameLanguageCommandResponse>, ISetPropertyCommand<Language, LanguageId>;
