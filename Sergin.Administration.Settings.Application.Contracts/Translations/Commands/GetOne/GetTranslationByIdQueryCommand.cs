using Sergin.Administration.Settings.Domain.Translations;
using Sergin.SharedKernel.Application.Commands.Queries;

namespace Sergin.Administration.Settings.Application.Contracts.Translations.Commands.GetOne;

/// <summary>One translation with its version, which a change or delete is sent at.</summary>
public sealed record GetTranslationByIdQueryCommand(TranslationId Id) : IQuery<TranslationQueryResponse>, IGetOneQuery<Translation, TranslationId>;
