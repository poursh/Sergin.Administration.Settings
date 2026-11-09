using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.GetAudit;
using Sergin.SharedKernel.Application.Commands.Configuration;

namespace Sergin.Administration.Settings.Application.Configurations.Languages.Commands.GetAudit;

internal sealed class GetLanguageAuditQueryCommandConfiguration : ICommandConfiguration<GetLanguageAuditQueryCommand>
{
    public void Configure(CommandConfigurationBuilder<GetLanguageAuditQueryCommand> builder) =>
        builder.RequirePermissions(LanguagePermissions.Read);
}
