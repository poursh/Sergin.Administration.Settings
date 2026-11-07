using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.SetDirection;
using Sergin.Administration.Settings.Domain.Languages;
using Sergin.SharedKernel.Application.Commands.Configuration;

namespace Sergin.Administration.Settings.Application.Configurations.Languages.Commands.SetDirection;

internal sealed class SetLanguageDirectionCommandConfiguration : ICommandConfiguration<SetLanguageDirectionCommand, Language>
{
    public void Configure(CommandConfigurationBuilder<SetLanguageDirectionCommand> builder) =>
        builder.RequirePermissions(LanguagePermissions.Manage).RequireExpectedVersion().NotShown(command => command.Id);
}
