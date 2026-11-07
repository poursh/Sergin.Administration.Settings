using Sergin.Administration.Settings.Application.Configurations.Languages;
using Sergin.Administration.Settings.Application.Contracts.Translations.Commands.GetOne;
using Sergin.Administration.Settings.Domain.Translations;
using Sergin.SharedKernel.Application.Commands.Configuration;

namespace Sergin.Administration.Settings.Application.Configurations.Translations.Commands.GetOne;

internal sealed class GetTranslationByIdQueryCommandConfiguration : ICommandConfiguration<GetTranslationByIdQueryCommand, Translation>
{
    public void Configure(CommandConfigurationBuilder<GetTranslationByIdQueryCommand> builder) =>
        builder.RequirePermissions(LanguagePermissions.Read).NotShown(command => command.Id);
}
