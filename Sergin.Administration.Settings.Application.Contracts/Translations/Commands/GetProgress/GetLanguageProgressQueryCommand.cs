using Sergin.Administration.Settings.Domain.Languages;
using Sergin.SharedKernel.Application.Commands.Queries;

namespace Sergin.Administration.Settings.Application.Contracts.Translations.Commands.GetProgress;

public sealed record GetLanguageProgressQueryCommand(LanguageId Id) : IQuery<LanguageProgressQueryResponse>, IEntityViewQuery<Language, LanguageId>;
