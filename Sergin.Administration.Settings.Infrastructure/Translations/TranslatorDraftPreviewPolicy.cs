using Microsoft.Extensions.Logging;
using Sergin.Administration.Settings.Application.Configurations.Translations;
using Sergin.SharedKernel.Application.Localizations;
using Sergin.SharedKernel.Application.Securities.Authorization;

namespace Sergin.Administration.Settings.Infrastructure.Translations;

/// <summary>
/// Who may preview a draft language: a holder of <see cref="TranslationPermissions.Edit"/> on any scope (translations
/// belong to no scope), since translators check their work in context. An anonymous caller holds nothing. The culture
/// provider asks this for every request that names a draft, static assets included, so a failing check denies and logs
/// a warning rather than failing the request.
/// </summary>
internal sealed partial class TranslatorDraftPreviewPolicy(IScopeAuthorization authorization, ILogger<TranslatorDraftPreviewPolicy> logger)
    : IDraftLanguagePreviewPolicy
{
    public async Task<bool> CanPreviewAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await authorization.CurrentUserHoldsAnywhereAsync(TranslationPermissions.Edit, cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
#pragma warning disable CA1031 // A failing check must deny the preview, never fail the request.
        catch (Exception exception)
#pragma warning restore CA1031
        {
            LogCheckFailed(logger, exception);
            return false;
        }
    }

    [LoggerMessage(Level = LogLevel.Warning, Message = "The draft language preview check failed; the preview is denied.")]
    private static partial void LogCheckFailed(ILogger logger, Exception exception);
}
