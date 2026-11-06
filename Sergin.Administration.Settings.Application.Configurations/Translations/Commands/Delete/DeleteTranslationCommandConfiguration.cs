using Sergin.Administration.Settings.Application.Contracts.Translations.Commands.Delete;
using Sergin.Administration.Settings.Domain.Translations;
using Sergin.SharedKernel.Application.Commands.Configuration;

namespace Sergin.Administration.Settings.Application.Configurations.Translations.Commands.Delete;

internal sealed class DeleteTranslationCommandConfiguration : ICommandConfiguration<DeleteTranslationCommand, Translation>
{
    public void Configure(CommandConfigurationBuilder<DeleteTranslationCommand> builder) =>
        builder.RequirePermissions(TranslationPermissions.Edit).RequireExpectedVersion().NotShown(command => command.Id);
}
