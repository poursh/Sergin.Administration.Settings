using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.Publish;
using Sergin.SharedKernel.Application.Commands.Configuration;

namespace Sergin.Administration.Settings.Application.Configurations.Languages.Commands.Publish;

internal sealed class PublishLanguageCommandConfiguration : ICommandConfiguration<PublishLanguageCommand>
{
    public void Configure(CommandConfigurationBuilder<PublishLanguageCommand> builder) =>
        builder.RequirePermissions(LanguagePermissions.Manage);
}
