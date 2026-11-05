using Sergin.Platform.Application.Contracts.Languages.Commands.Add;
using Sergin.Platform.Domain.Languages;
using Sergin.SharedKernel.Application.Commands.Configuration;

namespace Sergin.Platform.Application.Configurations.Languages.Commands.Add;

internal sealed class AddLanguageCommandConfiguration : ICommandConfiguration<AddLanguageCommand, Language>
{
    public void Configure(CommandConfigurationBuilder<AddLanguageCommand> builder) =>
        builder.RequirePermissions(LanguagePermissions.Manage);
}
