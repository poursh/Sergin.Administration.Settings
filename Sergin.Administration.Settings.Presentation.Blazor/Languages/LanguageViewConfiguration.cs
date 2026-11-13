using MudBlazor;
using Sergin.Administration.Settings.Application.Configurations.Languages;
using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.Add;
using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.Delete;
using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.GetList;
using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.GetOne;
using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.Publish;
using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.Rename;
using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.SetDirection;
using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.Unpublish;
using Sergin.Administration.Settings.Domain.Languages;
using Sergin.Administration.Settings.Presentation.Blazor.Languages.Components;
using Sergin.SharedKernel.Presentation.Blazor.Aggregates;

namespace Sergin.Administration.Settings.Presentation.Blazor.Languages;

/// <summary>
/// The language's generated views: its routes and titles, Publish and Unpublish as a reverse pair (only the one that
/// applies is offered), rename and direction edited inline on the detail, the link to its translations, the direction
/// shown with what the culture gives, and its translation progress in the overview.
/// </summary>
internal sealed class LanguageViewConfiguration : IAggregateViewConfiguration<Language>
{
    public void Configure(AggregateViewBuilder<Language> view)
    {
        view.Title(LanguageUiTexts.Title)
            .Routes("/st/languages", id => $"/st/languages/{id}", create: "/st/languages/new");
        view.Slot<LanguageProgressSlot>(SerginViewSlot.DetailOverview);

        view.Command<AddLanguageCommand>().Label(LanguageUiTexts.New);
        view.Command<PublishLanguageCommand>()
            .Label(LanguageUiTexts.Publish)
            .Icon(Icons.Material.Outlined.Publish)
            .Kind(SerginActionKind.Primary)
            .AppliesWhen<LanguageQueryResponse>(language => language.Status == LanguageStatus.Draft);
        view.Command<UnpublishLanguageCommand>().Label(LanguageUiTexts.Unpublish).Icon(Icons.Material.Outlined.Unpublished);
        view.Command<RenameLanguageCommand>().Label(CommonTexts.Rename).Edit(SerginEditMode.Inline);
        view.Command<SetLanguageDirectionCommand>().Edit(SerginEditMode.Inline);
        view.Command<DeleteLanguageCommand>().ConfirmDelete(LanguageUiTexts.DeleteTitle, LanguageUiTexts.DeleteConfirm);

        view.Link(LanguageUiTexts.Translations, Icons.Material.Outlined.Translate, id => $"/st/languages/{id}/translations");

        view.Detail<LanguageQueryResponse>(detail =>
            detail.Title(x => x.DisplayName).Field(x => x.Direction).Component<LanguageDirectionValue>());
        view.List<GetLanguageListItem>(list =>
            list.Title(x => x.Culture)
                .SearchPlaceholder(LanguageUiTexts.SearchPlaceholder)
                .Field(x => x.IsRightToLeft)
                .Label(LanguageTexts.Direction.Label)
                .BoolTexts(LanguageTexts.DirectionValues.For(LanguageDirection.RightToLeft), LanguageTexts.DirectionValues.For(LanguageDirection.LeftToRight)));
        view.Form<AddLanguageCommand>(form => form.Title(LanguageUiTexts.New));
    }
}
