using Sergin.Administration.Settings.Domain.Languages;
using Sergin.SharedKernel.Application.Commands;

namespace Sergin.Administration.Settings.Application.Contracts.Languages.Commands.Delete;

public sealed record DeleteLanguageCommand(LanguageId Id) : ICommand<DeleteLanguageCommandResponse>, IDeleteCommand<Language, LanguageId>;
