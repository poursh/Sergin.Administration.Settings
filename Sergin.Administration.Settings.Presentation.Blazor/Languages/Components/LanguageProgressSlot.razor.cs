using Microsoft.AspNetCore.Components;
using Sergin.Administration.Settings.Application.Contracts.Translations.Commands.GetProgress;
using Sergin.SharedKernel.Presentation.Blazor.Aggregates;
using Sergin.SharedKernel.Presentation.Localizations;

namespace Sergin.Administration.Settings.Presentation.Blazor.Languages.Components;

/// <summary>
/// The language detail's overview card, in the generated detail view's <c>DetailOverview</c> slot: how much of the
/// application the language translates. It reads the progress the view loaded; without it the figure is a dash.
/// </summary>
public sealed partial class LanguageProgressSlot
{
    /// <summary>What the generated view hands a slot: the language's progress it loaded, in <c>Overview</c>.</summary>
    [Parameter]
    public AggregateViewContext? Context { get; set; }

    [Inject]
    private IStringLocalizer Localizer { get; set; } = default!;

    [Inject]
    private ICultureFormatter Formatter { get; set; } = default!;

    private LanguageProgressQueryResponse? Progress => Context?.Overview as LanguageProgressQueryResponse;

    // A whole percentage, rounded down so a language one text short of complete does not read 100%.
    private string Percent => Progress is { Total: > 0 } figures
        ? Localizer.Text(LanguageUiTexts.OverviewPercent, new { Percent = Formatter.Number(figures.Translated * 100L / figures.Total) })
        : "–";

    private string? Caption => Progress is { } figures
        ? Localizer.Text(TranslationUiTexts.Progress, new { Translated = Formatter.Number(figures.Translated), Total = Formatter.Number(figures.Total) })
        : null;
}
