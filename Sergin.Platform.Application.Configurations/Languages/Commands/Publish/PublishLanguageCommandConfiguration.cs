using Sergin.Platform.Application.Contracts.Languages.Commands.Publish;
using Sergin.Platform.Domain.Languages;
using Sergin.SharedKernel.Application.Commands.Configuration;

namespace Sergin.Platform.Application.Configurations.Languages.Commands.Publish;

internal sealed class PublishLanguageCommandConfiguration : ICommandConfiguration<PublishLanguageCommand, Language>
{
    public void Configure(CommandConfigurationBuilder<PublishLanguageCommand> builder) =>
        builder.RequirePermissions(LanguagePermissions.Manage).RequireExpectedVersion().NotShown(command => command.Id);
}
