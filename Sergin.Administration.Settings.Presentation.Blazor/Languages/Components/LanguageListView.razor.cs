using Microsoft.AspNetCore.Components;
using MudBlazor;
using Sergin.Administration.Settings.Application.Configurations.Languages;
using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.Delete;
using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.GetList;
using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.GetOne;
using Sergin.SharedKernel.Application.Concurrency;
using Sergin.SharedKernel.Application.Securities.Authorization;
using Sergin.SharedKernel.Presentation.Blazor.Errors;

namespace Sergin.Administration.Settings.Presentation.Blazor.Languages.Components;

/// <summary>Every language the platform knows, drafts included, with a link to each one's translations.</summary>
public sealed partial class LanguageListView
{
    private SerginAggregateList<GetLanguageListItem> list = default!;
    private bool canManage;

    private IReadOnlyList<SerginBreadcrumb> Trail { get; } = [SerginBreadcrumb.Of(SettingsNavigation.Languages)];

    // Languages belong to no scope, so the permission is checked on any scope, as the pipeline checks it.
    private IReadOnlyList<SerginPageAction> Actions { get; } =
    [
        new(LanguageTexts.New, Icons.Material.Filled.Add, SerginActionKind.Primary, Href: "/st/languages/new",
            Permission: LanguagePermissions.Manage, AnyScope: true),
    ];

    [Inject]
    private IStringLocalizer Localizer { get; set; } = default!;

    [Inject]
    private ISerginDispatcher Dispatcher { get; set; } = default!;

    [Inject]
    private IUiErrorPresenter ErrorPresenter { get; set; } = default!;

    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    [Inject]
    private IScopeAuthorization ScopeAuthorization { get; set; } = default!;

    // Presentation only: the pipeline checks the same permission on the delete. Languages belong to no scope.
    protected override async Task OnInitializedAsync() =>
        canManage = await ScopeAuthorization.CurrentUserHoldsAnywhereAsync(LanguagePermissions.Manage);

    private Task<ErrorOr<ListQueryResponse<GetLanguageListItem>>> LoadAsync(SerginListRequest request, CancellationToken cancellationToken) =>
        Dispatcher.SendAsync(new GetLanguageListQueryCommand(request.Paggination, request.Term, request.Filtering, request.Sorting), cancellationToken);

    private Task ReloadAsync() => list.ReloadAsync();

    // The detail view's confirm and version rule: checked against the version read just before the delete.
    private async Task DeleteAsync(GetLanguageListItem item)
    {
        if (!await DialogService.ConfirmDeleteAsync(Localizer, LanguageTexts.DeleteTitle, LanguageTexts.DeleteConfirm, new { item.DisplayName }))
        {
            return;
        }

        ErrorOr<Versioned<LanguageQueryResponse>> loaded = await Dispatcher.SendVersionedAsync(new GetLanguageByIdQueryCommand(item.Id));

        if (loaded.IsError)
        {
            ErrorPresenter.Notify(loaded.Errors);
        }
        else
        {
            ErrorOr<Versioned<DeleteLanguageCommandResponse>> result =
                await Dispatcher.SendVersionedAsync(new DeleteLanguageCommand(item.Id), loaded.Value.Version);

            if (result.IsError)
            {
                ErrorPresenter.Notify(result.Errors);
            }
        }

        await list.ReloadAsync();
    }
}
