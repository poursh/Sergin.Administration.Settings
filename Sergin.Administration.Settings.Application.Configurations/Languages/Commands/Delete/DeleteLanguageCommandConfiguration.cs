using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.Delete;
using Sergin.SharedKernel.Application.Commands.Configuration;

namespace Sergin.Administration.Settings.Application.Configurations.Languages.Commands.Delete;

internal sealed class DeleteLanguageCommandConfiguration : ICommandConfiguration<DeleteLanguageCommand>
{
    public void Configure(CommandConfigurationBuilder<DeleteLanguageCommand> builder) =>
        builder.RequirePermissions(LanguagePermissions.Manage);
}
