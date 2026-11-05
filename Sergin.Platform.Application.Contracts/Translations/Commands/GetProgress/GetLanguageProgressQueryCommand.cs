using Sergin.Platform.Domain.Languages;
using Sergin.SharedKernel.Application.Commands.Queries;

namespace Sergin.Platform.Application.Contracts.Translations.Commands.GetProgress;

public sealed record GetLanguageProgressQueryCommand(LanguageId LanguageId) : IQuery<LanguageProgressQueryResponse>;
