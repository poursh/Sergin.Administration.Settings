using Sergin.Administration.Settings.Application.Contracts.Translations.Commands.Change;
using Sergin.SharedKernel.Application.Commands.Configuration;

namespace Sergin.Administration.Settings.Application.Configurations.Translations.Commands.Change;

internal sealed class ChangeTranslationCommandConfiguration : ICommandConfiguration<ChangeTranslationCommand>
{
    public void Configure(CommandConfigurationBuilder<ChangeTranslationCommand> builder) =>
        builder.RequirePermissions(TranslationPermissions.Edit);
}
