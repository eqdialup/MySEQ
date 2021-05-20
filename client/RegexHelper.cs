using System;
using System.Text.RegularExpressions;

namespace Structures
{
    /// <summary>
    /// This class contains a limited set of pre-compiled Regex expressions to be used for various filtering capabilities
    /// (until such time as we get enough information from the server not to need them anymore)
    ///
    /// The need for it's own class is because we're re-using the same expressions over and over and
    /// it's not efficient to re-instantiate them for every spawn packet.
    /// </summary>
    public static class RegexHelper
    {
        public static string FixMobName(this string name)
            => name?.IndexOf("_", StringComparison.OrdinalIgnoreCase) == 0 ? name : name?.Replace("_", " ").Trim();

        public static string FixMobNameMatch(this string name) => Regex.Replace(name, "^*[^a-zA-Z #]", "");

        public static string FilterMobName(this string name) => Regex.Replace(name, "^*[^a-zA-Z_ #]", "").Trim();

        public static string TrimName(this string name) => Regex.Replace(name?.Replace("_", " "), "[0-9]", "").Trim();

        internal static string SearchName(this string name) => Regex.Replace(name.Replace("_", " "), "[0-9#]", "").Trim();

        public static bool RegexMatch(this string name) => Regex.IsMatch(name, "^[A-Z#]");

        public static Regex GetRegex(this string name) => new Regex($".*{name}.*", RegexOptions.IgnoreCase);

        public static bool IsSubstring(string toSearch, string forSearch)
        {
            Regex regex = GetRegex(forSearch);
            return regex.Match(toSearch).Success;
        }
    }
}