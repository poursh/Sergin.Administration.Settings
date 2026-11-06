using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.GetOne;
using Sergin.Administration.Settings.Domain.Languages;
using Sergin.SharedKernel.Application.Commands.Configuration;

namespace Sergin.Administration.Settings.Application.Configurations.Languages.Commands.GetOne;

internal sealed class GetLanguageByIdQueryCommandConfiguration : ICommandConfiguration<GetLanguageByIdQueryCommand, Language>
{
    public void Configure(CommandConfigurationBuilder<GetLanguageByIdQueryCommand> builder) =>
        builder.RequirePermissions(LanguagePermissions.Read).NotShown(command => command.Id);
}
