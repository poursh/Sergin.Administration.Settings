using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.Add;
using Sergin.SharedKernel.Application.Commands.Configuration;

namespace Sergin.Administration.Settings.Application.Configurations.Languages.Commands.Add;

internal sealed class AddLanguageCommandConfiguration : ICommandConfiguration<AddLanguageCommand>
{
    public void Configure(CommandConfigurationBuilder<AddLanguageCommand> builder) =>
        builder.RequirePermissions(LanguagePermissions.Manage);
}
