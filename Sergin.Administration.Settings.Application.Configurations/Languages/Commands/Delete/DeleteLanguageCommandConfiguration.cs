using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.Delete;
using Sergin.Administration.Settings.Domain.Languages;
using Sergin.SharedKernel.Application.Commands.Configuration;

namespace Sergin.Administration.Settings.Application.Configurations.Languages.Commands.Delete;

internal sealed class DeleteLanguageCommandConfiguration : ICommandConfiguration<DeleteLanguageCommand, Language>
{
    public void Configure(CommandConfigurationBuilder<DeleteLanguageCommand> builder) =>
        builder.RequirePermissions(LanguagePermissions.Manage).RequireExpectedVersion().NotShown(command => command.Id);
}
