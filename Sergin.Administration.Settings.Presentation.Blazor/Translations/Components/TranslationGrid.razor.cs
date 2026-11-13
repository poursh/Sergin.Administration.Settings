using Microsoft.AspNetCore.Components;
using MudBlazor;
using Sergin.Administration.Settings.Application.Configurations.Translations;
using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.GetOne;
using Sergin.SharedKernel.Presentation.Blazor.Aggregates;
using Sergin.Administration.Settings.Application.Contracts.Translations.Commands.Add;
using Sergin.Administration.Settings.Application.Contracts.Translations.Commands.Change;
using Sergin.Administration.Settings.Application.Contracts.Translations.Commands.Delete;
using Sergin.Administration.Settings.Application.Contracts.Translations.Commands.GetList;
using Sergin.Administration.Settings.Application.Contracts.Translations.Commands.GetOne;
using Sergin.Administration.Settings.Domain.Languages;
using Sergin.Administration.Settings.Domain.Translations;
using Sergin.SharedKernel.Application.Concurrency;
using Sergin.SharedKernel.Application.Securities.Authorization;
using Sergin.SharedKernel.Domain;
using Sergin.SharedKernel.Presentation.Blazor.Errors;

namespace Sergin.Administration.Settings.Presentation.Blazor.Translations.Components;

/// <summary>
/// The translation list's body, in the generated list view's <c>ListBody</c> slot: every declared text in one language, merged with its translations: filtered by module, then category, by status,
/// and by origin (screens or messages); searched over key, default and translation; edited in place.
/// </summary>
/// <remarks>
/// <para>
/// The list query carries module, category, status and origin as its own properties, not as <c>Filtering</c>: the filter
/// panel's Apply copies the pickers into the applied values the load sends, and the chips mirror those.
/// </para>
/// <para>
/// Editing a row with no translation sends <c>AddTranslation</c>; editing one with a translation first loads it, and
/// its version, then sends <c>ChangeTranslation</c> at that version. Every refusal is the pipeline's: a placeholder
/// mismatch is a validation error shown as it is, and a stale version reloads the row's version and says what the
/// translation reads now, keeping the user's text so a second save replaces it deliberately.
/// </para>
/// </remarks>
public sealed partial class TranslationGrid
{
    private static readonly TranslationStatus[] Statuses =
        [TranslationStatus.Untranslated, TranslationStatus.Translated, TranslationStatus.NeedsReview, TranslationStatus.Orphaned];

    private static readonly TextOrigin[] Origins = [TextOrigin.Ui, TextOrigin.Application];

    private SerginAggregateList<GetTranslationListItem> list = default!;
    private int generation;
    private bool canEdit;

    private IReadOnlyList<string> modules = [];
    private IReadOnlyList<string> categories = [];
    private string? filterModule;
    private string? filterCategory;
    private TranslationStatus? filterStatus;
    private TextOrigin? filterOrigin;
    private string? appliedModule;
    private string? appliedCategory;
    private TranslationStatus? appliedStatus;
    private TextOrigin? appliedOrigin;

    private GetTranslationListItem? editing;
    private RowVersion? editingVersion;
    private string draft = string.Empty;
    private bool saving;

    /// <summary>What the generated list view hands the slot: the language (<c>Parent</c>, its id <c>ParentId</c>) and the reload <c>Generation</c>.</summary>
    [Parameter]
    [EditorRequired]
    public AggregateViewContext Context { get; set; } = default!;

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

    // The generated view loads the language for its trail and hands it over; a failed load leaves it null.
    private LanguageQueryResponse? Language => Context.Parent as LanguageQueryResponse;

    private Guid LanguageId => Context.ParentId ?? throw new InvalidOperationException("The translation grid is listed under a language: it needs Context.ParentId.");

    // The translations are shown in the language's own direction, which the page around them need not share.
    private string Direction => Language?.IsRightToLeft == true ? "rtl" : "auto";

    protected override async Task OnInitializedAsync()
    {
        generation = Context.Generation;

        // Presentation only: the pipeline checks the same permission on every write. Translations belong to no scope.
        canEdit = await ScopeAuthorization.CurrentUserHoldsAnywhereAsync(TranslationPermissions.Edit);

        modules = [.. Catalog.Entries.Select(entry => entry.Module).Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal)];
    }

    // The slot keeps its instance across the header's Reload, so a new generation is the cue to load again.
    protected override async Task OnParametersSetAsync()
    {
        if (generation == Context.Generation)
        {
            return;
        }

        generation = Context.Generation;
        await ReloadAsync();
    }

    private static TextDefinition StatusText(TranslationStatus status) => TranslationTexts.StatusValues.For(status);

    private static TextDefinition OriginText(TextOrigin origin) => TranslationTexts.OriginValues.For(origin);

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
                new LanguageId(LanguageId), request.Paggination, request.Term, appliedModule, appliedCategory, appliedStatus, appliedOrigin),
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
        appliedOrigin = filterOrigin;

        return AppliedChips();
    }

    // The chips of the applied values, which the load sends: built on Apply, and again when a removal clears another.
    private IReadOnlyList<SerginAppliedFilter> AppliedChips()
    {
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

        if (appliedOrigin is { } origin)
        {
            filters.Add(new(
                nameof(GetTranslationListQueryCommand.Origin),
                FilteringType.equals,
                origin.ToString(),
                Chip(TranslationTexts.Origin, Localizer.Text(OriginText(origin)))));
        }

        return [.. filters];
    }

    private string Chip(FieldText field, string value) =>
        Localizer.Text(TranslationUiTexts.FilterChip, new { Field = Localizer.Text(field.Label), Value = value });

    // A category belongs to its module, so removing the module filter takes the category, and its chip, with it.
    private void OnFilterRemoved(SerginAppliedFilter filter)
    {
        switch (filter.Id)
        {
            case nameof(GetTranslationListQueryCommand.Module):
                appliedModule = filterModule = null;
                appliedCategory = filterCategory = null;
                categories = [];
                list.SetFilters(AppliedChips());
                break;
            case nameof(GetTranslationListQueryCommand.Category):
                appliedCategory = filterCategory = null;
                break;
            case nameof(GetTranslationListQueryCommand.Status):
                appliedStatus = filterStatus = null;
                break;
            case nameof(GetTranslationListQueryCommand.Origin):
                appliedOrigin = filterOrigin = null;
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
        appliedOrigin = filterOrigin = null;
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
            else if (item.TranslationId is null && errors.Any(error => error.Code == nameof(AddTranslationCommand.Key)))
            {
                await SwitchToChangeAsync(item);
            }

            return;
        }

        Snackbar.Add(Localizer.Text(TranslationUiTexts.Saved), Severity.Success);
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

    // Someone translated the text after this row was opened as untranslated, so the add is refused as already
    // translated. The row is looked up again by its key; when it now has a translation, the edit goes on as a change of
    // it, at its version, keeping the user's text, as after a stale change.
    private async Task SwitchToChangeAsync(GetTranslationListItem item)
    {
        ErrorOr<ListQueryResponse<GetTranslationListItem>> found = await Dispatcher.SendAsync(
            new GetTranslationListQueryCommand(new LanguageId(LanguageId), Paggination.Create(50, 1), new Term(item.Key)));

        if (found.IsError
            || found.Value.Data.FirstOrDefault(row => string.Equals(row.Key, item.Key, StringComparison.Ordinal))
                is not { TranslationId: { } id } current)
        {
            return;
        }

        editing = current;
        await ReloadRowVersionAsync(new TranslationId(id));
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
        Snackbar.Add(Localizer.Text(TranslationUiTexts.ChangedMeanwhile, new { loaded.Value.Value.Text }), Severity.Info);
    }

    // Only an orphan can be deleted; the delete is checked against the version read just before it.
    private async Task DeleteAsync(GetTranslationListItem item)
    {
        if (item.TranslationId is not { } id
            || !await DialogService.ConfirmDeleteAsync(Localizer, TranslationUiTexts.DeleteTitle, TranslationUiTexts.DeleteConfirm, new { item.Key }))
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
