using Sergin.Administration.Settings.Application.Contracts.Translations.Commands.Change;
using Sergin.Administration.Settings.Domain.Translations;
using Sergin.SharedKernel.Application.Commands.Configuration;

namespace Sergin.Administration.Settings.Application.Configurations.Translations.Commands.Change;

internal sealed class ChangeTranslationCommandConfiguration : ICommandConfiguration<ChangeTranslationCommand, Translation>
{
    public void Configure(CommandConfigurationBuilder<ChangeTranslationCommand> builder) =>
        builder.RequirePermissions(TranslationPermissions.Edit).RequireExpectedVersion().NotShown(command => command.Id);
}
