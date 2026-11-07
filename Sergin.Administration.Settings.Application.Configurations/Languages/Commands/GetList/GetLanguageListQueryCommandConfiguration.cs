using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.GetList;
using Sergin.Administration.Settings.Domain.Languages;
using Sergin.SharedKernel.Application.Commands.Configuration;

namespace Sergin.Administration.Settings.Application.Configurations.Languages.Commands.GetList;

internal sealed class GetLanguageListQueryCommandConfiguration : ICommandConfiguration<GetLanguageListQueryCommand, Language>
{
    public void Configure(CommandConfigurationBuilder<GetLanguageListQueryCommand> builder) =>
        builder.RequirePermissions(LanguagePermissions.Read);
}
