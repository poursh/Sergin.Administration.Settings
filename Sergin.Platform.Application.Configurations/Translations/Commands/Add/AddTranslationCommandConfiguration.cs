using Sergin.Platform.Application.Contracts.Translations.Commands.Add;
using Sergin.Platform.Domain.Translations;
using Sergin.SharedKernel.Application.Commands.Configuration;

namespace Sergin.Platform.Application.Configurations.Translations.Commands.Add;

internal sealed class AddTranslationCommandConfiguration : ICommandConfiguration<AddTranslationCommand, Translation>
{
    public void Configure(CommandConfigurationBuilder<AddTranslationCommand> builder) =>
        builder.RequirePermissions(TranslationPermissions.Edit).NotShown(command => command.LanguageId);
}
