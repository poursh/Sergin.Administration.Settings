using Sergin.Administration.Settings.Application.Configurations.Languages;
using Sergin.Administration.Settings.Application.Contracts.Translations.Commands.GetProgress;
using Sergin.SharedKernel.Application.Commands.Configuration;

namespace Sergin.Administration.Settings.Application.Configurations.Translations.Commands.GetProgress;

internal sealed class GetLanguageProgressQueryCommandConfiguration : ICommandConfiguration<GetLanguageProgressQueryCommand>
{
    public void Configure(CommandConfigurationBuilder<GetLanguageProgressQueryCommand> builder) =>
        builder.RequirePermissions(LanguagePermissions.Read);
}
