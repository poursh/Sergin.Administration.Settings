using Microsoft.AspNetCore.Components;
using Sergin.Administration.Settings.Application.Configurations.Languages;
using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.GetOne;
using Sergin.Administration.Settings.Domain.Languages;
using Sergin.SharedKernel.Presentation.Blazor.Aggregates;

namespace Sergin.Administration.Settings.Presentation.Blazor.Languages.Components;

/// <summary>
/// The language detail's direction as a reader sees it: "From the culture (Right to left)" for the culture's own
/// direction, else the direction's value text. A manager edits the field inline instead.
/// </summary>
public sealed partial class LanguageDirectionValue
{
    /// <summary>What the generated view hands a field component: the loaded language, in <c>Record</c>.</summary>
    [Parameter]
    public AggregateViewContext? Context { get; set; }

    /// <summary>The field's value: the language's direction setting.</summary>
    [Parameter]
    public object? Value { get; set; }

    [Inject]
    private IStringLocalizer Localizer { get; set; } = default!;

    private string Text => Value switch
    {
        LanguageDirection.FromCulture when Context?.Record is LanguageQueryResponse language => Localizer.Text(LanguageUiTexts.DirectionResolved, new
        {
            Setting = Localizer.Text(LanguageTexts.DirectionValues.For(LanguageDirection.FromCulture)),
            Resolved = Localizer.Text(LanguageTexts.DirectionValues.For(language.IsRightToLeft ? LanguageDirection.RightToLeft : LanguageDirection.LeftToRight)),
        }),
        LanguageDirection direction => Localizer.Text(LanguageTexts.DirectionValues.For(direction)),
        _ => string.Empty,
    };
}
