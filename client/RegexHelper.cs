using System;
using System.Text.RegularExpressions;

namespace Structures
{
    /// <summary>
    /// This class contains a set of pre-compiled regular expressions for various string processing operations related to mob names.
    /// Using pre-compiled expressions improves performance by avoiding re-instantiation of the same expressions.
    /// </summary>
    public static class RegexHelper
    {
        // Pre-compiled regular expressions used throughout the class
        private static readonly Regex RemoveUnderscoreRegex = new Regex("_", RegexOptions.Compiled);
        private static readonly Regex FixMobNameMatchRegex = new Regex("^[^a-zA-Z #]", RegexOptions.Compiled);
        private static readonly Regex FilterMobNameRegex = new Regex("^[^a-zA-Z_ #]", RegexOptions.Compiled);
        private static readonly Regex RemoveNumbersRegex = new Regex("[0-9]", RegexOptions.Compiled);
        private static readonly Regex RemoveNumbersAndHashRegex = new Regex("[0-9#]", RegexOptions.Compiled);
        private static readonly Regex UppercaseOrHashRegex = new Regex("^[A-Z#]", RegexOptions.Compiled);

        /// Fixes a mob name by replacing underscores with spaces, unless it starts with an underscore.
        /// <returns>The modified name with underscores replaced by spaces, or the original if it starts with an underscore.</returns>
        public static string FixMobName(this string name)
            => name?.StartsWith("_", StringComparison.OrdinalIgnoreCase) == true ? name : RemoveUnderscoreRegex.Replace(name ?? string.Empty, " ").Trim();

        /// Removes all characters from the beginning of the name until an alphabetic character or '#' is encountered.
        /// <returns>The modified name after removing unwanted characters from the start.</returns>
        public static string FixMobNameMatch(this string name)
            => FixMobNameMatchRegex.Replace(name ?? string.Empty, "");

        /// Filters out unwanted characters at the beginning of the mob name, keeping only alphabetic, underscore, or '#' characters.
        /// <returns>The modified name after filtering out unwanted characters.</returns>
        public static string FilterMobName(this string name)
            => FilterMobNameRegex.Replace(name ?? string.Empty, "").Trim();

        /// Trims the name by replacing underscores with spaces and removing all numeric characters.
        /// <returns>The modified name with underscores replaced and numbers removed.</returns>
        public static string TrimName(this string name)
            => RemoveNumbersRegex.Replace(RemoveUnderscoreRegex.Replace(name ?? string.Empty, " "), "").Trim();

        /// Searches a name by replacing underscores with spaces and removing all numeric characters and '#' characters.
        /// <returns>The modified name suitable for search operations.</returns>
        internal static string SearchName(this string name)
            => RemoveNumbersAndHashRegex.Replace(RemoveUnderscoreRegex.Replace(name ?? string.Empty, " "), "").Trim();

        /// Checks if a name matches a pattern where it starts with an uppercase letter or a '#' character.
        /// <returns>True if the name matches the pattern; otherwise, false.</returns>
        public static bool RegexMatch(this string name)
            => UppercaseOrHashRegex.IsMatch(name ?? string.Empty);

        /// Creates a case-insensitive regex pattern that searches for the specified string anywhere within another string.
        /// <returns>A compiled regex pattern.</returns>
        public static Regex GetRegex(this string name)
            => new Regex($".*{Regex.Escape(name)}.*", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    }
}