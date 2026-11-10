using Sergin.Administration.Settings.Application.Contracts.Translations.Commands.Delete;
using Sergin.SharedKernel.Application.Commands.Configuration;

namespace Sergin.Administration.Settings.Application.Configurations.Translations.Commands.Delete;

internal sealed class DeleteTranslationCommandConfiguration : ICommandConfiguration<DeleteTranslationCommand>
{
    public void Configure(CommandConfigurationBuilder<DeleteTranslationCommand> builder) =>
        builder.RequirePermissions(TranslationPermissions.Edit);
}
