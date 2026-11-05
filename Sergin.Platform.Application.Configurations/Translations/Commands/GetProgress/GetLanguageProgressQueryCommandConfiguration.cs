using Sergin.Platform.Application.Configurations.Languages;
using Sergin.Platform.Application.Contracts.Translations.Commands.GetProgress;
using Sergin.Platform.Domain.Translations;
using Sergin.SharedKernel.Application.Commands.Configuration;

namespace Sergin.Platform.Application.Configurations.Translations.Commands.GetProgress;

internal sealed class GetLanguageProgressQueryCommandConfiguration : ICommandConfiguration<GetLanguageProgressQueryCommand, Translation>
{
    public void Configure(CommandConfigurationBuilder<GetLanguageProgressQueryCommand> builder) =>
        builder.RequirePermissions(LanguagePermissions.Read).NotShown(command => command.LanguageId);
}
