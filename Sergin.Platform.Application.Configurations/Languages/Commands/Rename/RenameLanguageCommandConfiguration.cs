using Sergin.Platform.Application.Contracts.Languages.Commands.Rename;
using Sergin.Platform.Domain.Languages;
using Sergin.SharedKernel.Application.Commands.Configuration;

namespace Sergin.Platform.Application.Configurations.Languages.Commands.Rename;

internal sealed class RenameLanguageCommandConfiguration : ICommandConfiguration<RenameLanguageCommand, Language>
{
    public void Configure(CommandConfigurationBuilder<RenameLanguageCommand> builder) =>
        builder.RequirePermissions(LanguagePermissions.Manage).RequireExpectedVersion().NotShown(command => command.Id);
}
