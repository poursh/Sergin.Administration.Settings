using Sergin.Administration.Settings.Domain.Languages;
using Sergin.SharedKernel.Application.Commands.Queries;

namespace Sergin.Administration.Settings.Application.Contracts.Languages.Commands.GetOne;

public sealed record GetLanguageByIdQueryCommand(LanguageId Id) : IQuery<LanguageQueryResponse>, IGetOneQuery<Language, LanguageId>;
