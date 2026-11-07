using Sergin.Administration.Settings.Domain.Languages;
using Sergin.SharedKernel.Application.Commands;

namespace Sergin.Administration.Settings.Application.Contracts.Translations.Commands.Add;

/// <summary>A first translation of <paramref name="Key"/> in a language; refused when the language already has one.</summary>
public sealed record AddTranslationCommand(LanguageId LanguageId, string Key, string Text) : ICommand<AddTranslationCommandResponse>;
