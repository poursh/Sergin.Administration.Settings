using Sergin.Platform.Application.Contracts.Languages.Commands.SetDirection;
using Sergin.Platform.Domain.Languages;
using Sergin.SharedKernel.Application.Commands.Configuration;

namespace Sergin.Platform.Application.Configurations.Languages.Commands.SetDirection;

internal sealed class SetLanguageDirectionCommandConfiguration : ICommandConfiguration<SetLanguageDirectionCommand, Language>
{
    public void Configure(CommandConfigurationBuilder<SetLanguageDirectionCommand> builder) =>
        builder.RequirePermissions(LanguagePermissions.Manage).RequireExpectedVersion().NotShown(command => command.Id);
}
