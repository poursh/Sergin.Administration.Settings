using Microsoft.AspNetCore.Components;
using MudBlazor;
using Sergin.Platform.Application.Configurations.Languages;
using Sergin.Platform.Application.Configurations.Translations;
using Sergin.Platform.Application.Contracts.Languages.Commands.GetOne;
using Sergin.Platform.Application.Contracts.Translations.Commands.Add;
using Sergin.Platform.Application.Contracts.Translations.Commands.Change;
using Sergin.Platform.Application.Contracts.Translations.Commands.Delete;
using Sergin.Platform.Application.Contracts.Translations.Commands.GetList;
using Sergin.Platform.Application.Contracts.Translations.Commands.GetOne;
using Sergin.Platform.Domain.Languages;
using Sergin.Platform.Domain.Translations;
using Sergin.SharedKernel.Application.Concurrency;
using Sergin.SharedKernel.Application.Securities.Authorization;
using Sergin.SharedKernel.Domain;
using Sergin.SharedKernel.Presentation.Blazor.Errors;

namespace Sergin.Platform.Presentation.Blazor.Translations.Components;

/// <summary>
/// Every declared text in one language, merged with its translations: filtered by module, then category, and by
/// status; searched over key, default and translation; edited in place.
/// </summary>
/// <remarks>
/// <para>
/// The list query carries module, category and status as its own properties, not as <c>Filtering</c>: the filter
/// panel's Apply copies the pickers into the applied values the load sends, and the chips mirror those.
/// </para>
/// <para>
/// Editing a row with no translation sends <c>AddTranslation</c>; editing one with a translation first loads it, and
/// its version, then sends <c>ChangeTranslation</c> at that version. Every refusal is the pipeline's: a placeholder
/// mismatch is a validation error shown as it is, and a stale version reloads the row's version and says what the
/// translation reads now, keeping the user's text so a second save replaces it deliberately.
/// </para>
/// </remarks>
public sealed partial class TranslationListView
{
    private static readonly TranslationStatus[] Statuses =
        [TranslationStatus.Untranslated, TranslationStatus.Translated, TranslationStatus.NeedsReview, TranslationStatus.Orphaned];

    private SerginAggregateList<GetTranslationListItem> list = default!;
    private LanguageQueryResponse? language;
    private bool canEdit;

    private IReadOnlyList<string> modules = [];
    private IReadOnlyList<string> categories = [];
    private string? filterModule;
    private string? filterCategory;
    private TranslationStatus? filterStatus;
    private string? appliedModule;
    private string? appliedCategory;
    private TranslationStatus? appliedStatus;

    private GetTranslationListItem? editing;
    private RowVersion? editingVersion;
    private string draft = string.Empty;
    private bool saving;

    [Parameter]
    [EditorRequired]
    public Guid LanguageId { get; set; }

    [Inject]
    private IStringLocalizer Localizer { get; set; } = default!;

    [Inject]
    private ISerginDispatcher Dispatcher { get; set; } = default!;

    [Inject]
    private IUiErrorPresenter ErrorPresenter { get; set; } = default!;

    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    [Inject]
    private TextCatalog Catalog { get; set; } = default!;

    [Inject]
    private IScopeAuthorization ScopeAuthorization { get; set; } = default!;

    private IReadOnlyList<SerginBreadcrumb> Trail =>
    [
        SerginBreadcrumb.Of(PlatformNavigation.Languages),
        language is null
            ? new(LanguageTexts.Singular, $"/pf/languages/{LanguageId}")
            : SerginBreadcrumb.Data(language.DisplayName, $"/pf/languages/{LanguageId}"),
        new(TranslationTexts.Title),
    ];

    // The translations are shown in the language's own direction, which the page around them need not share.
    private string Direction => language?.IsRightToLeft == true ? "rtl" : "auto";

    protected override async Task OnParametersSetAsync()
    {
        ErrorOr<LanguageQueryResponse> loaded = await Dispatcher.SendAsync(new GetLanguageByIdQueryCommand(LanguageId));

        // The list reports a missing language itself; the trail then keeps its placeholder.
        language = loaded.IsError ? null : loaded.Value;

        // Presentation only: the pipeline checks the same permission on every write. Translations belong to no scope.
        canEdit = await ScopeAuthorization.CurrentUserHoldsAnywhereAsync(TranslationPermissions.Edit);

        modules = [.. Catalog.Entries.Select(entry => entry.Module).Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal)];
    }

    private static TextDefinition StatusText(TranslationStatus status) => status switch
    {
        TranslationStatus.Translated => TranslationTexts.Translated,
        TranslationStatus.NeedsReview => TranslationTexts.NeedsReview,
        TranslationStatus.Orphaned => TranslationTexts.Orphaned,
        _ => TranslationTexts.Untranslated,
    };

    private static Color StatusColor(TranslationStatus status) => status switch
    {
        TranslationStatus.Translated => Color.Success,
        TranslationStatus.NeedsReview => Color.Warning,
        TranslationStatus.Orphaned => Color.Error,
        _ => Color.Default,
    };

    private bool IsEditing(GetTranslationListItem item) => editing is not null && string.Equals(editing.Key, item.Key, StringComparison.Ordinal);

    private Task<ErrorOr<ListQueryResponse<GetTranslationListItem>>> LoadAsync(SerginListRequest request, CancellationToken cancellationToken) =>
        Dispatcher.SendAsync(
            new GetTranslationListQueryCommand(
                new LanguageId(LanguageId), request.Paggination, request.Term, appliedModule, appliedCategory, appliedStatus),
            cancellationToken);

    private Task ReloadAsync()
    {
        editing = null;
        return list.ReloadAsync();
    }

    // A category is any level of a key's path below its module: "devices" and "devices.errors" alike.
    private void OnFilterModuleChanged(string? module)
    {
        filterModule = module;
        filterCategory = null;
        categories = module is null
            ? []
            :
            [
                .. Catalog.Entries
                    .Where(entry => string.Equals(entry.Module, module, StringComparison.Ordinal))
                    .SelectMany(entry => Enumerable.Range(1, entry.Category.Count)
                        .Select(depth => string.Join(SerginStringKey.Separator, entry.Category.Take(depth))))
                    .Distinct(StringComparer.Ordinal)
                    .Order(StringComparer.Ordinal),
            ];
    }

    private IReadOnlyList<SerginAppliedFilter> BuildFilters()
    {
        appliedModule = filterModule;
        appliedCategory = filterModule is null ? null : filterCategory;
        appliedStatus = filterStatus;

        List<SerginAppliedFilter> filters = [];

        if (appliedModule is { } module)
        {
            filters.Add(new(nameof(GetTranslationListQueryCommand.Module), FilteringType.equals, module, Chip(TranslationTexts.Module, module)));
        }

        if (appliedCategory is { } category)
        {
            filters.Add(new(
                nameof(GetTranslationListQueryCommand.Category), FilteringType.equals, category, Chip(TranslationTexts.CategoryField, category)));
        }

        if (appliedStatus is { } status)
        {
            filters.Add(new(
                nameof(GetTranslationListQueryCommand.Status),
                FilteringType.equals,
                status.ToString(),
                Chip(TranslationTexts.Status, Localizer.Text(StatusText(status)))));
        }

        return [.. filters];
    }

    private string Chip(FieldText field, string value) =>
        Localizer.Text(TranslationTexts.FilterChip, new { Field = Localizer.Text(field.Label), Value = value });

    // A category belongs to its module, so removing the module filter takes the category with it.
    private void OnFilterRemoved(SerginAppliedFilter filter)
    {
        switch (filter.Id)
        {
            case nameof(GetTranslationListQueryCommand.Module):
                appliedModule = filterModule = null;
                appliedCategory = filterCategory = null;
                categories = [];
                break;
            case nameof(GetTranslationListQueryCommand.Category):
                appliedCategory = filterCategory = null;
                break;
            case nameof(GetTranslationListQueryCommand.Status):
                appliedStatus = filterStatus = null;
                break;
            default:
                break;
        }
    }

    private void ResetFilterFields()
    {
        appliedModule = filterModule = null;
        appliedCategory = filterCategory = null;
        appliedStatus = filterStatus = null;
        categories = [];
    }

    private async Task StartEditAsync(GetTranslationListItem item)
    {
        if (item.TranslationId is { } id)
        {
            ErrorOr<Versioned<TranslationQueryResponse>> loaded =
                await Dispatcher.SendVersionedAsync(new GetTranslationByIdQueryCommand(new TranslationId(id)));

            if (loaded.IsError)
            {
                ErrorPresenter.Notify(loaded.Errors);
                await list.ReloadAsync();

                return;
            }

            draft = loaded.Value.Value.Text;
            editingVersion = loaded.Value.Version;
        }
        else
        {
            draft = string.Empty;
            editingVersion = null;
        }

        editing = item;
    }

    private void CancelEdit() => editing = null;

    private async Task SaveAsync()
    {
        if (editing is not { } item)
        {
            return;
        }

        saving = true;
        IReadOnlyList<Error>? errors = item.TranslationId is { } id ? await ChangeAsync(new TranslationId(id)) : await AddAsync(item);
        saving = false;

        if (errors is not null)
        {
            // Validation (a placeholder mismatch, an empty text) yields one error per broken rule.
            ErrorPresenter.Notify(errors);

            if (errors.Any(VersionErrors.IsStale) && item.TranslationId is { } stale)
            {
                await ReloadRowVersionAsync(new TranslationId(stale));
            }

            return;
        }

        Snackbar.Add(Localizer.Text(TranslationTexts.Saved), Severity.Success);
        editing = null;
        await list.ReloadAsync();
    }

    private async Task<IReadOnlyList<Error>?> AddAsync(GetTranslationListItem item)
    {
        ErrorOr<AddTranslationCommandResponse> result =
            await Dispatcher.SendAsync(new AddTranslationCommand(new LanguageId(LanguageId), item.Key, draft));

        return result.IsError ? result.Errors : null;
    }

    private async Task<IReadOnlyList<Error>?> ChangeAsync(TranslationId id)
    {
        ErrorOr<Versioned<ChangeTranslationCommandResponse>> result =
            await Dispatcher.SendVersionedAsync(new ChangeTranslationCommand(id, draft), editingVersion);

        return result.IsError ? result.Errors : null;
    }

    // Someone saved the translation after this row was opened: take its new version and say what it reads now.
    private async Task ReloadRowVersionAsync(TranslationId id)
    {
        ErrorOr<Versioned<TranslationQueryResponse>> loaded = await Dispatcher.SendVersionedAsync(new GetTranslationByIdQueryCommand(id));

        if (loaded.IsError)
        {
            ErrorPresenter.Notify(loaded.Errors);
            editing = null;
            await list.ReloadAsync();

            return;
        }

        editingVersion = loaded.Value.Version;
        Snackbar.Add(Localizer.Text(TranslationTexts.ChangedMeanwhile, new { loaded.Value.Value.Text }), Severity.Info);
    }

    // Only an orphan can be deleted; the delete is checked against the version read just before it.
    private async Task DeleteAsync(GetTranslationListItem item)
    {
        if (item.TranslationId is not { } id
            || !await DialogService.ConfirmDeleteAsync(Localizer, TranslationTexts.DeleteTitle, TranslationTexts.DeleteConfirm, new { item.Key }))
        {
            return;
        }

        TranslationId translation = new(id);
        ErrorOr<Versioned<TranslationQueryResponse>> loaded = await Dispatcher.SendVersionedAsync(new GetTranslationByIdQueryCommand(translation));

        if (loaded.IsError)
        {
            ErrorPresenter.Notify(loaded.Errors);
        }
        else
        {
            ErrorOr<Versioned<DeleteTranslationCommandResponse>> result =
                await Dispatcher.SendVersionedAsync(new DeleteTranslationCommand(translation), loaded.Value.Version);

            if (result.IsError)
            {
                ErrorPresenter.Notify(result.Errors);
            }
        }

        await list.ReloadAsync();
    }
}
