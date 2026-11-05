using Sergin.Platform.Application.Configurations.Languages;
using Sergin.Platform.Application.Contracts.Translations.Commands.GetList;
using Sergin.Platform.Domain.Translations;
using Sergin.SharedKernel.Application.Commands.Configuration;

namespace Sergin.Platform.Application.Configurations.Translations.Commands.GetList;

internal sealed class GetTranslationListQueryCommandConfiguration : ICommandConfiguration<GetTranslationListQueryCommand, Translation>
{
    public void Configure(CommandConfigurationBuilder<GetTranslationListQueryCommand> builder) =>
        builder.RequirePermissions(LanguagePermissions.Read).NotShown(command => command.LanguageId);
}
