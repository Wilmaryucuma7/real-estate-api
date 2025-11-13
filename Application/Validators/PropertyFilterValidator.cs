using FluentValidation;
using RealEstateAPI.Application.DTOs;
using System.Text.RegularExpressions;

namespace RealEstateAPI.Application.Validators;

/// <summary>
/// Validator for property filter criteria with security constraints.
/// </summary>
public sealed partial class PropertyFilterValidator : AbstractValidator<PropertyFilterDto>
{
    public PropertyFilterValidator()
    {
        // Name validation: letters, numbers, spaces, accents, hyphens
        When(x => !string.IsNullOrWhiteSpace(x.Name), () =>
        {
            RuleFor(x => x.Name)
                .MaximumLength(100)
                .WithMessage("Property name cannot exceed 100 characters")
                .Must(BeValidTextInput!)
                .WithMessage("Property name contains invalid characters. Only letters, numbers, spaces, accents and hyphens are allowed");
        });

        // Address validation: similar rules
        When(x => !string.IsNullOrWhiteSpace(x.Address), () =>
        {
            RuleFor(x => x.Address)
                .MaximumLength(200)
                .WithMessage("Address cannot exceed 200 characters")
                .Must(BeValidTextInput!)
                .WithMessage("Address contains invalid characters. Only letters, numbers, spaces, accents, hyphens and commas are allowed");
        });

        // Price range validation
        When(x => x.MinPrice.HasValue, () =>
        {
            RuleFor(x => x.MinPrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Minimum price must be greater than or equal to 0");
        });

        When(x => x.MaxPrice.HasValue, () =>
        {
            RuleFor(x => x.MaxPrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Maximum price must be greater than or equal to 0");
        });

        // Price range logic validation
        When(x => x.MinPrice.HasValue && x.MaxPrice.HasValue, () =>
        {
            RuleFor(x => x.MaxPrice)
                .GreaterThanOrEqualTo(x => x.MinPrice!.Value)
                .WithMessage("Maximum price must be greater than or equal to minimum price");
        });

        // Page validation (if pagination is added)
        When(x => x.Page.HasValue, () =>
        {
            RuleFor(x => x.Page)
                .GreaterThan(0)
                .WithMessage("Page must be greater than 0");
        });

        When(x => x.PageSize.HasValue, () =>
        {
            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("Page size must be between 1 and 100");
        });
    }

    /// <summary>
    /// Validates text input allowing only safe characters:
    /// - Letters (including accented: ·, È, Ì, Û, ˙, Ò, etc.)
    /// - Numbers
    /// - Spaces
    /// - Hyphens and commas
    /// </summary>
    private static bool BeValidTextInput(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return true;

        // Allow: letters (with accents), numbers, spaces, hyphens, commas, dots
        // Disallow: SQL injection chars, scripts, special symbols
        return SafeTextRegex().IsMatch(input);
    }

    [GeneratedRegex(@"^[a-zA-Z0-9\s·ÈÌÛ˙¡…Õ”⁄Ò—¸‹,.\-]+$", RegexOptions.Compiled)]
    private static partial Regex SafeTextRegex();
}
