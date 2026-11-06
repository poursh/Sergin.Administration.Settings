using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.Publish;
using Sergin.Administration.Settings.Domain.Languages;
using Sergin.SharedKernel.Application.Commands.Configuration;

namespace Sergin.Administration.Settings.Application.Configurations.Languages.Commands.Publish;

internal sealed class PublishLanguageCommandConfiguration : ICommandConfiguration<PublishLanguageCommand, Language>
{
    public void Configure(CommandConfigurationBuilder<PublishLanguageCommand> builder) =>
        builder.RequirePermissions(LanguagePermissions.Manage).RequireExpectedVersion().NotShown(command => command.Id);
}
