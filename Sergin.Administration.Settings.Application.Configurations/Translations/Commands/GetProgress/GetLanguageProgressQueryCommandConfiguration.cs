using Sergin.Administration.Settings.Application.Configurations.Languages;
using Sergin.Administration.Settings.Application.Contracts.Translations.Commands.GetProgress;
using Sergin.Administration.Settings.Domain.Translations;
using Sergin.SharedKernel.Application.Commands.Configuration;

namespace Sergin.Administration.Settings.Application.Configurations.Translations.Commands.GetProgress;

internal sealed class GetLanguageProgressQueryCommandConfiguration : ICommandConfiguration<GetLanguageProgressQueryCommand, Translation>
{
    public void Configure(CommandConfigurationBuilder<GetLanguageProgressQueryCommand> builder) =>
        builder.RequirePermissions(LanguagePermissions.Read).NotShown(command => command.LanguageId);
}
