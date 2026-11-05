using Sergin.Platform.Application.Contracts.Translations.Commands.Delete;
using Sergin.Platform.Domain.Translations;
using Sergin.SharedKernel.Application.Commands.Configuration;

namespace Sergin.Platform.Application.Configurations.Translations.Commands.Delete;

internal sealed class DeleteTranslationCommandConfiguration : ICommandConfiguration<DeleteTranslationCommand, Translation>
{
    public void Configure(CommandConfigurationBuilder<DeleteTranslationCommand> builder) =>
        builder.RequirePermissions(TranslationPermissions.Edit).RequireExpectedVersion().NotShown(command => command.Id);
}
