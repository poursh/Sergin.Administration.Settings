using System.Globalization;
using Sergin.SharedKernel.Domain;
using Sergin.SharedKernel.Domain.Localizations;

namespace Sergin.Administration.Settings.Domain.Languages;

/// <summary>
/// A language translations are written for: its culture, its name in its own script, its direction and whether it is
/// offered to users. English is not one: it is the code default, always present, never a row.
/// </summary>
public class Language : AggregateRoot<LanguageId>
{
    private Language() { }

    /// <summary>The culture code, as .NET names it (<c>fa-IR</c>, whatever case it was given in). Unique.</summary>
    public CultureName Culture { get; private set; }

    public LanguageDisplayName DisplayName { get; private set; }

    public LanguageDirection Direction { get; private set; }

    public LanguageStatus Status { get; private set; }

    /// <summary>The direction the language is written in: the culture's own unless an administrator overrode it.</summary>
    public bool IsRightToLeft => ResolveRightToLeft(Culture.Value, Direction);

    /// <summary>
    /// A new language, a draft until published. Refuses a culture .NET does not know, the invariant culture and any
    /// English culture; stores the culture under .NET's own name for it.
    /// </summary>
    public static ErrorOr<Language> Create(CultureName culture, LanguageDisplayName displayName)
    {
        ArgumentNullException.ThrowIfNull(culture);

        ErrorOr<CultureName> normalised = culture.Normalise();

        if (normalised.IsError)
        {
            return normalised.Errors;
        }

        return new Language
        {
            Id = new LanguageId(Guid.CreateVersion7()),
            Culture = normalised.Value,
            DisplayName = displayName,
            Direction = LanguageDirection.FromCulture,
            Status = LanguageStatus.Draft,
        };
    }

    /// <summary>
    /// Whether text in <paramref name="culture"/> runs right to left under <paramref name="direction"/>: the override
    /// when there is one, the culture's own direction otherwise. <paramref name="culture"/> is a stored culture code.
    /// </summary>
    public static bool ResolveRightToLeft(string culture, LanguageDirection direction) => direction switch
    {
        LanguageDirection.LeftToRight => false,
        LanguageDirection.RightToLeft => true,
        _ => CultureIsRightToLeft(culture),
    };

    public void Rename(LanguageDisplayName displayName) => DisplayName = displayName;

    public void SetDirection(LanguageDirection direction) => Direction = direction;

    /// <summary>Offers the language to users. Publishing a published language changes nothing.</summary>
    public void Publish() => Status = LanguageStatus.Published;

    /// <summary>Takes the language back to a draft, which only translators see. Unpublishing a draft changes nothing.</summary>
    public void Unpublish() => Status = LanguageStatus.Draft;

    // A stored culture this machine's ICU cannot build (a row written on another host) reads left to right rather than
    // failing every read of the language list.
    private static bool CultureIsRightToLeft(string culture)
    {
        try
        {
            return CultureInfo.GetCultureInfo(culture).TextInfo.IsRightToLeft;
        }
        catch (CultureNotFoundException)
        {
            return false;
        }
    }
}

public sealed record LanguageId(Guid Value);

/// <summary>A culture code as given; <see cref="Normalise"/> turns it into the name .NET knows the culture by.</summary>
public sealed record CultureName(string Value)
{
    public const int MaxLength = 20;

    /// <summary>
    /// The culture .NET knows by <paramref name="value"/>, in any case; null for a name .NET does not know and for the
    /// invariant culture, which no language is.
    /// </summary>
    public static CultureInfo? Resolve(string? value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > MaxLength)
        {
            return null;
        }

        try
        {
            var culture = CultureInfo.GetCultureInfo(value, predefinedOnly: true);

            return culture.Name.Length == 0 ? null : culture;
        }
        catch (CultureNotFoundException)
        {
            return null;
        }
    }

    /// <summary>Whether <paramref name="culture"/> is English, in any region: English is the code default, not a language row.</summary>
    public static bool IsEnglish(CultureInfo culture)
    {
        ArgumentNullException.ThrowIfNull(culture);

        return string.Equals(culture.TwoLetterISOLanguageName, "en", StringComparison.Ordinal);
    }

    // Error.Validation with the property name as code, so it renders through the validator path: the metadata names the
    // declared text a presentation layer renders in the caller's culture, the description is its English default.
    public ErrorOr<CultureName> Normalise()
    {
        if (Resolve(Value) is not { } culture)
        {
            return Error.Validation(
                nameof(Language.Culture), "The culture is not one .NET knows.", ErrorText.Of(LanguageErrorKeys.UnknownCulture));
        }

        if (IsEnglish(culture))
        {
            return Error.Validation(
                nameof(Language.Culture),
                "English is built in and cannot be added as a language.",
                ErrorText.Of(LanguageErrorKeys.BuiltInEnglish));
        }

        return new CultureName(culture.Name);
    }
}

/// <summary>The language's name in its own script ("فارسی").</summary>
public sealed record LanguageDisplayName(string Value)
{
    public const int MaxLength = 100;
}

/// <summary>Which way the language is written: the culture's own direction, or an administrator's override.</summary>
public enum LanguageDirection
{
    FromCulture,
    LeftToRight,
    RightToLeft,
}

/// <summary>A draft is seen by translators only; a published language is offered to every user.</summary>
public enum LanguageStatus
{
    Draft,
    Published,
}
