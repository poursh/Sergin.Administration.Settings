using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.Unpublish;
using Sergin.Administration.Settings.Domain.Languages;
using Sergin.SharedKernel.Application.Commands.Configuration;

namespace Sergin.Administration.Settings.Application.Configurations.Languages.Commands.Unpublish;

internal sealed class UnpublishLanguageCommandConfiguration : ICommandConfiguration<UnpublishLanguageCommand, Language>
{
    public void Configure(CommandConfigurationBuilder<UnpublishLanguageCommand> builder) =>
        builder.RequirePermissions(LanguagePermissions.Manage).RequireExpectedVersion().NotShown(command => command.Id);
}
