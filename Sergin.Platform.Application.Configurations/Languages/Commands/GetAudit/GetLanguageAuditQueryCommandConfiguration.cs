using Sergin.Platform.Application.Contracts.Languages.Commands.GetAudit;
using Sergin.Platform.Domain.Languages;
using Sergin.SharedKernel.Application.Commands.Configuration;

namespace Sergin.Platform.Application.Configurations.Languages.Commands.GetAudit;

internal sealed class GetLanguageAuditQueryCommandConfiguration : ICommandConfiguration<GetLanguageAuditQueryCommand, Language>
{
    public void Configure(CommandConfigurationBuilder<GetLanguageAuditQueryCommand> builder) =>
        builder.RequirePermissions(LanguagePermissions.Read).NotShown(command => command.Id);
}
