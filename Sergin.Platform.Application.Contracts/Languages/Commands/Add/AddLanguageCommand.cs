using Sergin.Platform.Domain.Languages;
using Sergin.SharedKernel.Application.Commands;

namespace Sergin.Platform.Application.Contracts.Languages.Commands.Add;

public sealed record AddLanguageCommand(CultureName Culture, LanguageDisplayName DisplayName) : ICommand<AddLanguageCommandResponse>;
