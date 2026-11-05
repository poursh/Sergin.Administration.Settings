using Sergin.Platform.Domain.Translations;
using Sergin.SharedKernel.Application.Commands.Queries;

namespace Sergin.Platform.Application.Contracts.Translations.Commands.GetOne;

/// <summary>One translation with its version, which a change or delete is sent at.</summary>
public sealed record GetTranslationByIdQueryCommand(TranslationId Id) : IQuery<TranslationQueryResponse>;
