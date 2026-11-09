using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.SetDirection;
using Sergin.SharedKernel.Application.Commands.Configuration;

namespace Sergin.Administration.Settings.Application.Configurations.Languages.Commands.SetDirection;

internal sealed class SetLanguageDirectionCommandConfiguration : ICommandConfiguration<SetLanguageDirectionCommand>
{
    public void Configure(CommandConfigurationBuilder<SetLanguageDirectionCommand> builder) =>
        builder.RequirePermissions(LanguagePermissions.Manage);
}
