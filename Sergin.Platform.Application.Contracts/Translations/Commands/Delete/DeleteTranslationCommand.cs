using Sergin.Platform.Domain.Translations;
using Sergin.SharedKernel.Application.Commands;

namespace Sergin.Platform.Application.Contracts.Translations.Commands.Delete;

/// <summary>Deletes an orphaned translation, one whose key the catalog no longer declares; any other is refused.</summary>
public sealed record DeleteTranslationCommand(TranslationId Id) : ICommand<DeleteTranslationCommandResponse>;
