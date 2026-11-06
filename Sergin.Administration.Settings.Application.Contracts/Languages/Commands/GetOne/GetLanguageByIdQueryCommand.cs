using Sergin.SharedKernel.Application.Commands.Queries;

namespace Sergin.Administration.Settings.Application.Contracts.Languages.Commands.GetOne;

public sealed record GetLanguageByIdQueryCommand(Guid Id) : IQuery<LanguageQueryResponse>;
