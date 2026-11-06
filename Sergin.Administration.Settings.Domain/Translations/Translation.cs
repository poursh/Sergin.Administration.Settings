using Sergin.Administration.Settings.Domain.Languages;
using Sergin.SharedKernel.Domain;
using Sergin.SharedKernel.Domain.Localizations;

namespace Sergin.Administration.Settings.Domain.Translations;

/// <summary>
/// One declared text in one language: the translated text and the English default it was translated from. A stored
/// source that differs from the catalog's current default marks the translation as needing review; a key the catalog
/// no longer knows makes it orphaned, kept and listed but never served. Unique per language and key. A separate
/// aggregate rather than a child of <see cref="Language"/>, since a language holds thousands of them.
/// </summary>
public class Translation : AggregateRoot<TranslationId>
{
    private Translation() { }

    public LanguageId LanguageId { get; private set; }

    public TranslationKey Key { get; private set; }

    public TranslationText Text { get; private set; }

    /// <summary>The catalog's default for <see cref="Key"/> when the text was last saved.</summary>
    public SourceText SourceText { get; private set; }

    public static Translation Create(LanguageId languageId, TranslationKey key, TranslationText text, SourceText sourceText) =>
        new()
        {
            Id = new TranslationId(Guid.CreateVersion7()),
            LanguageId = languageId,
            Key = key,
            Text = text,
            SourceText = sourceText,
        };

    /// <summary>A new text, translated from <paramref name="sourceText"/>, the default as it reads now.</summary>
    public void Change(TranslationText text, SourceText sourceText)
    {
        Text = text;
        SourceText = sourceText;
    }
}

public sealed record TranslationId(Guid Value);

/// <summary>The <see cref="TextKey"/> a translation is for, as stored: the catalog may no longer declare it.</summary>
public sealed record TranslationKey(string Value)
{
    public const int MaxLength = TextKey.MaxLength;
}

public sealed record TranslationText(string Value)
{
    public const int MaxLength = 4000;
}

/// <summary>The English default a translation was made from.</summary>
public sealed record SourceText(string Value);
