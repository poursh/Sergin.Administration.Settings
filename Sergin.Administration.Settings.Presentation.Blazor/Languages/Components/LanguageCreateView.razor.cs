using Microsoft.AspNetCore.Components;
using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.Add;
using Sergin.Administration.Settings.Domain.Languages;
using Sergin.Administration.Settings.Presentation.Blazor.Languages.Models;
using Sergin.SharedKernel.Presentation.Blazor.Errors;

namespace Sergin.Administration.Settings.Presentation.Blazor.Languages.Components;

/// <summary>Adds a language as a draft; the detail view it leaves for publishes it.</summary>
public sealed partial class LanguageCreateView
{
    private readonly NewLanguageFormModel model = new();

    private bool submitting;
    private Func<object, string, Task<IEnumerable<string>>> validation = default!;

    private IReadOnlyList<SerginBreadcrumb> Trail { get; } =
    [
        SerginBreadcrumb.Of(SettingsNavigation.Languages),
        new(LanguageUiTexts.New),
    ];

    [Inject]
    private ISerginDispatcher Dispatcher { get; set; } = default!;

    [Inject]
    private ISerginFormValidator FormValidator { get; set; } = default!;

    [Inject]
    private IUiErrorPresenter ErrorPresenter { get; set; } = default!;

    [Inject]
    private IStringLocalizer Localizer { get; set; } = default!;

    [CascadingParameter]
    private SerginViewHost Host { get; set; } = SerginViewHost.Unhosted;

    // Created on this view, so it re-renders after SubmitAsync; SerginFormView invokes it once every field is valid.
    private EventCallback OnSubmit => EventCallback.Factory.Create(this, SubmitAsync);

    protected override void OnInitialized() => validation = FormValidator.RulesFor(ToCommand);

    private string? Hint(FieldText field) => field.Hint is { } hint ? Localizer.Text(hint) : null;

    // One mapping for both the field-by-field validation and the submit, so the two cannot drift.
    private AddLanguageCommand ToCommand() => new(new CultureName(model.Culture.Trim()), new LanguageDisplayName(model.DisplayName.Trim()));

    private async Task SubmitAsync()
    {
        submitting = true;

        ErrorOr<AddLanguageCommandResponse> result = await Dispatcher.SendAsync(ToCommand());

        submitting = false;

        if (result.IsError)
        {
            ErrorPresenter.Notify(result.Errors);

            return;
        }

        await Host.LeaveAsync($"/st/languages/{result.Value.Id}");
    }
}
