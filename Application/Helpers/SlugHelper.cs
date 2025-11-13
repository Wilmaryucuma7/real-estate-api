using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace RealEstateAPI.Application.Helpers;

/// <summary>
/// Helper class for generating URL-friendly slugs from text.
/// </summary>
public static class SlugHelper
{
    /// <summary>
    /// Converts a string to a URL-friendly slug.
    /// Example: "Modern Beach House" ? "modern-beach-house"
    /// </summary>
    public static string GenerateSlug(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        // Convert to lowercase
        text = text.ToLowerInvariant();

        // Remove accents
        text = RemoveDiacritics(text);

        // Replace spaces and special characters with hyphens
        text = Regex.Replace(text, @"[^a-z0-9\s-]", "");
        text = Regex.Replace(text, @"\s+", "-");
        text = Regex.Replace(text, @"-+", "-");

        // Trim hyphens from start and end
        text = text.Trim('-');

        return text;
    }

    private static string RemoveDiacritics(string text)
    {
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }
}
