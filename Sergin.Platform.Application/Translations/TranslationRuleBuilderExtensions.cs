using FluentValidation;
using Sergin.Platform.Application.Configurations.Translations;
using Sergin.Platform.Domain.Translations;
using Sergin.SharedKernel.Application.Localizations;
using Sergin.SharedKernel.Application.Localizations.Validation;

namespace Sergin.Platform.Application.Translations;

/// <summary>The rules every translation text obeys (spec decision 5), shared by Add and Change.</summary>
internal static class TranslationRuleBuilderExtensions
{
    private const string ExpectedPlaceholder = "Expected";

    public static IRuleBuilderOptions<T, string> MustBeATranslationText<T>(this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder.NotEmpty().MaximumLength(TranslationText.MaxLength);

    /// <summary>
    /// The text must use exactly the placeholders of the default <paramref name="defaultOf"/> finds: a misspelt one would
    /// render as written, a missing one would drop a value. Passes when there is no default to compare with (an unknown
    /// key, refused by its own rule). The message lists the expected placeholders as <c>{Expected}</c>.
    /// </summary>
    public static IRuleBuilderOptions<T, string> MustKeepThePlaceholdersOf<T>(
        this IRuleBuilder<T, string> ruleBuilder, Func<T, CancellationToken, Task<TextDefinition?>> defaultOf)
    {
        ArgumentNullException.ThrowIfNull(defaultOf);

        return ruleBuilder
            .MustAsync(async (command, text, context, cancellationToken) =>
            {
                if (await defaultOf(command, cancellationToken) is not { } definition)
                {
                    return true;
                }

                IReadOnlySet<string> expected = definition.Placeholders;
                context.MessageFormatter.AppendArgument(ExpectedPlaceholder, Describe(expected));

                return TextPlaceholders.In(text).SetEquals(expected);
            })
            .WithMessage(_ => ValidationTexts.Template(TranslationTexts.PlaceholdersDiffer));
    }

    private static string Describe(IReadOnlySet<string> placeholders) => placeholders.Count == 0
        ? ValidationTexts.Template(TranslationTexts.NoPlaceholders)
        : string.Join(", ", placeholders.Order(StringComparer.Ordinal).Select(name => $"{{{name}}}"));
}
