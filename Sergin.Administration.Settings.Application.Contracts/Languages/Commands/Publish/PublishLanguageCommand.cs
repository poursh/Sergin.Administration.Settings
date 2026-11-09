using Sergin.Administration.Settings.Domain.Languages;
using Sergin.SharedKernel.Application.Commands;

namespace Sergin.Administration.Settings.Application.Contracts.Languages.Commands.Publish;

public sealed record PublishLanguageCommand(LanguageId Id) : ICommand<PublishLanguageCommandResponse>, ITransitionCommand<Language, LanguageId>;
