using Sergin.Platform.Application.Contracts.Translations.Commands.Change;
using Sergin.Platform.Domain.Translations;
using Sergin.SharedKernel.Application.Commands.Configuration;

namespace Sergin.Platform.Application.Configurations.Translations.Commands.Change;

internal sealed class ChangeTranslationCommandConfiguration : ICommandConfiguration<ChangeTranslationCommand, Translation>
{
    public void Configure(CommandConfigurationBuilder<ChangeTranslationCommand> builder) =>
        builder.RequirePermissions(TranslationPermissions.Edit).RequireExpectedVersion().NotShown(command => command.Id);
}
