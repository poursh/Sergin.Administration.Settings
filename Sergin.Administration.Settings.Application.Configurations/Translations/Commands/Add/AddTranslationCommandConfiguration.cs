using Sergin.Administration.Settings.Application.Contracts.Translations.Commands.Add;
using Sergin.SharedKernel.Application.Commands.Configuration;

namespace Sergin.Administration.Settings.Application.Configurations.Translations.Commands.Add;

internal sealed class AddTranslationCommandConfiguration : ICommandConfiguration<AddTranslationCommand>
{
    public void Configure(CommandConfigurationBuilder<AddTranslationCommand> builder) =>
        builder.RequirePermissions(TranslationPermissions.Edit).NotShown(command => command.ParentId);
}
