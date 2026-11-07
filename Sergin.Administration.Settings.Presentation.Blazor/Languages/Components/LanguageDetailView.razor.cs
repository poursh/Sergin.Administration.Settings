using Microsoft.AspNetCore.Components;
using MudBlazor;
using Sergin.Administration.Settings.Application.Configurations.Languages;
using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.Delete;
using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.GetAudit;
using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.GetOne;
using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.Publish;
using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.Rename;
using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.SetDirection;
using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.Unpublish;
using Sergin.Administration.Settings.Application.Contracts.Translations.Commands.GetProgress;
using Sergin.Administration.Settings.Domain.Languages;
using Sergin.Administration.Settings.Presentation.Blazor.Languages.Models;
using Sergin.SharedKernel.Application.Aggregates;
using Sergin.SharedKernel.Application.Concurrency;
using Sergin.SharedKernel.Application.Securities.Authorization;
using Sergin.SharedKernel.Domain;
using Sergin.SharedKernel.Presentation.Blazor.Errors;
using Sergin.SharedKernel.Presentation.Errors;
using Sergin.SharedKernel.Presentation.Localizations;

namespace Sergin.Administration.Settings.Presentation.Blazor.Languages.Components;

/// <summary>
/// One language: its culture, name, status and direction, how much of the application it translates, rename, set
/// direction, publish or unpublish, and delete. Every edit is sent at the version the view loaded, and the view reloads
/// afterwards either way, so a stale answer leaves it showing the current language.
/// </summary>
public sealed partial class LanguageDetailView
{
    private readonly LanguageNameFormModel model = new();

    private LanguageQueryResponse? language;
    private LanguageProgressQueryResponse? progress;
    private RowVersion? version;
    private SerginProblem? problem;
    private bool busy;
    private bool canManage;

    private MudForm form = default!;
    private bool isValid;
    private Func<object, string, Task<IEnumerable<string>>> validation = default!;

    [Parameter]
    public Guid Id { get; set; }

    [Inject]
    private ISerginDispatcher Dispatcher { get; set; } = default!;

    [Inject]
    private ISerginFormValidator FormValidator { get; set; } = default!;

    [Inject]
    private IUiErrorPresenter ErrorPresenter { get; set; } = default!;

    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    [Inject]
    private IStringLocalizer Localizer { get; set; } = default!;

    [Inject]
    private ICultureFormatter Formatter { get; set; } = default!;

    [Inject]
    private IScopeAuthorization ScopeAuthorization { get; set; } = default!;

    [CascadingParameter]
    private SerginViewHost Host { get; set; } = SerginViewHost.Unhosted;

    private EventCallback OnRename => EventCallback.Factory.Create(this, RenameAsync);

    // The tail is the page title's word until the load fills in the name; on the not-found path it stays that way.
    private IReadOnlyList<SerginBreadcrumb> Trail =>
    [
        SerginBreadcrumb.Of(SettingsNavigation.Languages),
        language is null ? new(LanguageTexts.Singular) : SerginBreadcrumb.Data(language.DisplayName),
    ];

    // Languages belong to no scope, so every action is checked on any scope, as the pipeline checks it.
    private IReadOnlyList<SerginPageAction> Actions =>
    [
        language?.Status == LanguageStatus.Published
            ? new(LanguageUiTexts.Unpublish, Icons.Material.Outlined.Unpublished, SerginActionKind.Secondary,
                OnClick: EventCallback.Factory.Create(this, UnpublishAsync), Permission: LanguagePermissions.Manage, AnyScope: true, Disabled: busy)
            : new(LanguageUiTexts.Publish, Icons.Material.Outlined.Publish, SerginActionKind.Primary,
                OnClick: EventCallback.Factory.Create(this, PublishAsync), Permission: LanguagePermissions.Manage, AnyScope: true, Disabled: busy),
        new(LanguageUiTexts.Translations, Icons.Material.Outlined.Translate, SerginActionKind.Secondary, Href: $"/st/languages/{Id}/translations",
            Permission: LanguagePermissions.Read, AnyScope: true),
        new(CommonTexts.Delete, Icons.Material.Outlined.Delete, SerginActionKind.Danger, OnClick: EventCallback.Factory.Create(this, DeleteAsync),
            Permission: LanguagePermissions.Manage, AnyScope: true, Disabled: busy),
    ];

    // A whole percentage, rounded down so a language one text short of complete does not read 100%.
    private string Percent => progress is { Total: > 0 } figures
        ? Localizer.Text(LanguageUiTexts.OverviewPercent, new { Percent = Formatter.Number(figures.Translated * 100L / figures.Total) })
        : "–";

    private string? ProgressCaption => progress is { } figures
        ? Localizer.Text(TranslationUiTexts.Progress, new { Translated = Formatter.Number(figures.Translated), Total = Formatter.Number(figures.Total) })
        : null;

    protected override void OnInitialized() => validation = FormValidator.RulesFor(ToRenameCommand);

    // Presentation only: a reader sees the name and direction as text; the pipeline checks the same permission on
    // every write. Languages belong to no scope.
    protected override async Task OnInitializedAsync() =>
        canManage = await ScopeAuthorization.CurrentUserHoldsAnywhereAsync(LanguagePermissions.Manage);

    protected override Task OnParametersSetAsync() => LoadAsync();

    // The setting, and for the culture's own direction what it gives: "From the culture (Right to left)".
    private string DirectionShown => language switch
    {
        null => string.Empty,
        { Direction: LanguageDirection.FromCulture } => Localizer.Text(LanguageUiTexts.DirectionResolved, new
        {
            Setting = Localizer.Text(LanguageUiTexts.FromCulture),
            Resolved = Localizer.Text(language.IsRightToLeft ? LanguageUiTexts.RightToLeft : LanguageUiTexts.LeftToRight),
        }),
        _ => Localizer.Text(LanguageLabels.Direction(language.Direction)),
    };

    private Task<ErrorOr<AuditInfo>> LoadAuditAsync() => Dispatcher.SendAsync(new GetLanguageAuditQueryCommand(Id));

    private async Task LoadAsync()
    {
        ErrorOr<Versioned<LanguageQueryResponse>> loaded = await Dispatcher.SendVersionedAsync(new GetLanguageByIdQueryCommand(Id));

        if (loaded.IsError)
        {
            language = null;
            version = null;
            progress = null;
            problem = ErrorPresenter.Present(loaded.FirstError);

            return;
        }

        problem = null;
        language = loaded.Value.Value;
        version = loaded.Value.Version;
        model.DisplayName = language.DisplayName;

        // The figure is a summary; a failure leaves a dash rather than a second error beside the record.
        ErrorOr<LanguageProgressQueryResponse> figures = await Dispatcher.SendAsync(new GetLanguageProgressQueryCommand(new LanguageId(Id)));
        progress = figures.IsError ? null : figures.Value;
    }

    private RenameLanguageCommand ToRenameCommand() => new(Id, new LanguageDisplayName(model.DisplayName.Trim()));

    private async Task RenameAsync()
    {
        await form.ValidateAsync();

        if (form.IsValid)
        {
            await SendGuardedAsync(ToRenameCommand());
        }
    }

    private Task SetDirectionAsync(LanguageDirection direction) => SendGuardedAsync(new SetLanguageDirectionCommand(Id, direction));

    private Task PublishAsync() => SendGuardedAsync(new PublishLanguageCommand(Id));

    private Task UnpublishAsync() => SendGuardedAsync(new UnpublishLanguageCommand(Id));

    private async Task SendGuardedAsync<TResponse>(IRequest<ErrorOr<TResponse>> command)
    {
        if (version is not { } expected)
        {
            return;
        }

        busy = true;
        ErrorOr<Versioned<TResponse>> result = await Dispatcher.SendVersionedAsync(command, expected);
        busy = false;

        if (result.IsError)
        {
            ErrorPresenter.Notify(result.Errors);
        }

        await LoadAsync();
    }

    private async Task DeleteAsync()
    {
        if (version is not { } expected || language is null)
        {
            return;
        }

        if (!await DialogService.ConfirmDeleteAsync(Localizer, LanguageUiTexts.DeleteTitle, LanguageUiTexts.DeleteConfirm, new { language.DisplayName }))
        {
            return;
        }

        busy = true;
        ErrorOr<Versioned<DeleteLanguageCommandResponse>> result = await Dispatcher.SendVersionedAsync(new DeleteLanguageCommand(Id), expected);
        busy = false;

        if (result.IsError)
        {
            ErrorPresenter.Notify(result.Errors);
            await LoadAsync();

            return;
        }

        await Host.LeaveAsync("/st/languages");
    }
}
