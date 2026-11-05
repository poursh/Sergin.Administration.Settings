using Sergin.Platform.Application.Contracts.Languages.Commands.Delete;
using Sergin.Platform.Domain.Languages;
using Sergin.SharedKernel.Application.Commands.Configuration;

namespace Sergin.Platform.Application.Configurations.Languages.Commands.Delete;

internal sealed class DeleteLanguageCommandConfiguration : ICommandConfiguration<DeleteLanguageCommand, Language>
{
    public void Configure(CommandConfigurationBuilder<DeleteLanguageCommand> builder) =>
        builder.RequirePermissions(LanguagePermissions.Manage).RequireExpectedVersion().NotShown(command => command.Id);
}
