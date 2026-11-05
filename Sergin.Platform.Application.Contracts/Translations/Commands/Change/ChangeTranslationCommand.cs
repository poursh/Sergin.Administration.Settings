using Sergin.Platform.Domain.Translations;
using Sergin.SharedKernel.Application.Commands;

namespace Sergin.Platform.Application.Contracts.Translations.Commands.Change;

public sealed record ChangeTranslationCommand(TranslationId Id, string Text) : ICommand<ChangeTranslationCommandResponse>;
