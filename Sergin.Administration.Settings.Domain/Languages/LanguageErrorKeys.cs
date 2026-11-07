namespace Sergin.Administration.Settings.Domain.Languages;

/// <summary>
/// The text keys the Language aggregate's own validation errors name in their metadata (<c>ErrorText.Of</c>). The domain
/// cannot see the declarations in <c>.Application.Configurations</c>, so each key is written here once and declared there
/// (<c>LanguageTexts</c>); a test refuses a key with no declaration.
/// </summary>
public static class LanguageErrorKeys
{
    /// <summary>A culture .NET does not know, or the invariant culture, from <see cref="CultureName.Normalise"/>.</summary>
    public const string UnknownCulture = "settings.languages.errors.unknown-culture";

    /// <summary>An English culture, from <see cref="CultureName.Normalise"/>.</summary>
    public const string BuiltInEnglish = "settings.languages.errors.built-in-english";
}
