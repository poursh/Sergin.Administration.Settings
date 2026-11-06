using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.Rename;
using Sergin.Administration.Settings.Domain.Languages;
using Sergin.SharedKernel.Application.Commands.Configuration;

namespace Sergin.Administration.Settings.Application.Configurations.Languages.Commands.Rename;

internal sealed class RenameLanguageCommandConfiguration : ICommandConfiguration<RenameLanguageCommand, Language>
{
    public void Configure(CommandConfigurationBuilder<RenameLanguageCommand> builder) =>
        builder.RequirePermissions(LanguagePermissions.Manage).RequireExpectedVersion().NotShown(command => command.Id);
}
