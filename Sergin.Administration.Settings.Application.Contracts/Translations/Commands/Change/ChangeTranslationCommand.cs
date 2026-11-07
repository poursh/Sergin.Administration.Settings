using Sergin.Administration.Settings.Domain.Translations;
using Sergin.SharedKernel.Application.Commands;

namespace Sergin.Administration.Settings.Application.Contracts.Translations.Commands.Change;

public sealed record ChangeTranslationCommand(TranslationId Id, string Text) : ICommand<ChangeTranslationCommandResponse>;
