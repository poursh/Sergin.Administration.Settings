using Sergin.SharedKernel.Application.Commands.Queries;

namespace Sergin.Platform.Application.Contracts.Languages.Commands.GetOne;

public sealed record GetLanguageByIdQueryCommand(Guid Id) : IQuery<LanguageQueryResponse>;
