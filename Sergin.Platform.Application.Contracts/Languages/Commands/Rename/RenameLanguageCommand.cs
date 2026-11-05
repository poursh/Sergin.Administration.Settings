using Sergin.Platform.Domain.Languages;
using Sergin.SharedKernel.Application.Commands;

namespace Sergin.Platform.Application.Contracts.Languages.Commands.Rename;

public sealed record RenameLanguageCommand(Guid Id, LanguageDisplayName DisplayName) : ICommand<RenameLanguageCommandResponse>;
