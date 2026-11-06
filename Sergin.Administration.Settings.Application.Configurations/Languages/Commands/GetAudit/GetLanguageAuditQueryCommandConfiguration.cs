using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.GetAudit;
using Sergin.Administration.Settings.Domain.Languages;
using Sergin.SharedKernel.Application.Commands.Configuration;

namespace Sergin.Administration.Settings.Application.Configurations.Languages.Commands.GetAudit;

internal sealed class GetLanguageAuditQueryCommandConfiguration : ICommandConfiguration<GetLanguageAuditQueryCommand, Language>
{
    public void Configure(CommandConfigurationBuilder<GetLanguageAuditQueryCommand> builder) =>
        builder.RequirePermissions(LanguagePermissions.Read).NotShown(command => command.Id);
}
