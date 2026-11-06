using Sergin.Administration.Settings.Domain.Languages;
using Sergin.SharedKernel.Application.Commands;

namespace Sergin.Administration.Settings.Application.Contracts.Languages.Commands.Add;

public sealed record AddLanguageCommand(CultureName Culture, LanguageDisplayName DisplayName) : ICommand<AddLanguageCommandResponse>;
