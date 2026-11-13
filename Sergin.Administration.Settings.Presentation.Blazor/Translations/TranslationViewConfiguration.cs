using Sergin.Administration.Settings.Application.Contracts.Translations.Commands.Add;
using Sergin.Administration.Settings.Application.Contracts.Translations.Commands.Change;
using Sergin.Administration.Settings.Application.Contracts.Translations.Commands.Delete;
using Sergin.Administration.Settings.Domain.Languages;
using Sergin.Administration.Settings.Domain.Translations;
using Sergin.Administration.Settings.Presentation.Blazor.Translations.Components;
using Sergin.SharedKernel.Presentation.Blazor.Aggregates;

namespace Sergin.Administration.Settings.Presentation.Blazor.Translations;

/// <summary>
/// The translation's generated list: listed under its language, its table the <see cref="TranslationGrid"/> in the list
/// body slot, which sends the three write commands itself, so none is offered as an action.
/// </summary>
internal sealed class TranslationViewConfiguration : IAggregateViewConfiguration<Translation>
{
    public void Configure(AggregateViewBuilder<Translation> view)
    {
        view.Title(TranslationUiTexts.Title)
            .ListUnder<Language>(languageId => $"/st/languages/{languageId}/translations");
        view.Command<AddTranslationCommand>().Hidden();
        view.Command<ChangeTranslationCommand>().Hidden();
        view.Command<DeleteTranslationCommand>().Hidden();
        view.Slot<TranslationGrid>(SerginViewSlot.ListBody);
    }
}
