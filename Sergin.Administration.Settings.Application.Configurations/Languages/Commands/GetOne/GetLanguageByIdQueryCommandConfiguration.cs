using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.GetOne;
using Sergin.SharedKernel.Application.Commands.Configuration;

namespace Sergin.Administration.Settings.Application.Configurations.Languages.Commands.GetOne;

internal sealed class GetLanguageByIdQueryCommandConfiguration : ICommandConfiguration<GetLanguageByIdQueryCommand>
{
    public void Configure(CommandConfigurationBuilder<GetLanguageByIdQueryCommand> builder) =>
        builder.RequirePermissions(LanguagePermissions.Read);
}
