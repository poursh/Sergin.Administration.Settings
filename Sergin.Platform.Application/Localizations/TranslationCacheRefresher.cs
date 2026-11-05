using Microsoft.Extensions.Logging;
using Sergin.SharedKernel.Application.Localizations;

namespace Sergin.Platform.Application.Localizations;

/// <summary>
/// Refreshes the translation cache once a write has committed. The write has succeeded by then, so a failing refresh (the
/// store unreadable, the caller gone) must not reach the caller, who would retry into a refusal: it runs without the
/// caller's cancellation, is logged, and the stale entry is forgotten so the next load reads it again. Every Platform
/// handler that changes what the cache holds calls it after <c>SaveChangesAsync</c>.
/// </summary>
internal sealed class TranslationCacheRefresher(ITranslationCache cache, ILogger<TranslationCacheRefresher> logger)
{
    /// <summary>After a language write: reloads the language list, or forgets it when the reload fails.</summary>
    public async Task LanguagesChangedAsync()
    {
        try
        {
            await cache.RefreshLanguagesAsync(CancellationToken.None);
        }
        catch (Exception exception)
        {
            cache.InvalidateLanguages();
            logger.LogWarning(
                exception, "Refreshing the language list after a saved language write failed; it is reloaded on the next request.");
        }
    }

    /// <summary>
    /// After a translation write, or a language delete: reloads <paramref name="culture"/>'s translations, or forgets them
    /// when the reload fails.
    /// </summary>
    public async Task TranslationsChangedAsync(string culture)
    {
        try
        {
            await cache.RefreshAsync(culture, CancellationToken.None);
        }
        catch (Exception exception)
        {
            cache.Invalidate(culture);
            logger.LogWarning(
                exception,
                "Refreshing the translations of {Culture} after a saved translation write failed; they are reloaded on the next request.",
                culture);
        }
    }
}
