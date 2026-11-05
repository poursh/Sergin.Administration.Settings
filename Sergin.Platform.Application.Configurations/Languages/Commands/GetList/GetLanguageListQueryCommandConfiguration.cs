using Sergin.Platform.Application.Contracts.Languages.Commands.GetList;
using Sergin.Platform.Domain.Languages;
using Sergin.SharedKernel.Application.Commands.Configuration;

namespace Sergin.Platform.Application.Configurations.Languages.Commands.GetList;

internal sealed class GetLanguageListQueryCommandConfiguration : ICommandConfiguration<GetLanguageListQueryCommand, Language>
{
    public void Configure(CommandConfigurationBuilder<GetLanguageListQueryCommand> builder) =>
        builder.RequirePermissions(LanguagePermissions.Read);
}
