using Sergin.Platform.Application.Configurations.Languages;
using Sergin.Platform.Application.Contracts.Translations.Commands.GetOne;
using Sergin.Platform.Domain.Translations;
using Sergin.SharedKernel.Application.Commands.Configuration;

namespace Sergin.Platform.Application.Configurations.Translations.Commands.GetOne;

internal sealed class GetTranslationByIdQueryCommandConfiguration : ICommandConfiguration<GetTranslationByIdQueryCommand, Translation>
{
    public void Configure(CommandConfigurationBuilder<GetTranslationByIdQueryCommand> builder) =>
        builder.RequirePermissions(LanguagePermissions.Read).NotShown(command => command.Id);
}
