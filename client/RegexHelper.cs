﻿
using System;
using System.Text.RegularExpressions;

namespace Structures
{
    /// <summary>
    /// This class contains a limited set of pre-compiled Regex expressions
    /// to be used for various filtering capabilities (until such time as we
    /// get enough information from the server not to need them anymore)
    ///
    /// The need for it's own class is because we're re-using the same expressions
    /// over and over and it's not efficient to re-instantiate them for every
    /// spawn packet.
    /// </summary>

    public class RegexHelper
    {

        public static bool IsMount(string mobName)
        => mobName.IndexOf("s_Mount") > 0;

        public static bool IsFamiliar(string mobName)
        => mobName.IndexOf("`s_fami") > 0;

        public static bool IsMerc(string mobName)
        => mobName.IndexOf("'s Merc") > 0;

        public static string FixMobName(string name)
            => name?.IndexOf("_", StringComparison.OrdinalIgnoreCase) == 0 ? name : name?.Replace("_", " ");

        public static string FixMercName(string name) => Regex.Replace(name, "^*[^a-zA-Z ]", "");

        public static string FixMobNameMatch(string name) => Regex.Replace(name, "^*[^a-zA-Z #]", "");

        public static string FilterMobName(string name) => Regex.Replace(name, "^*[^a-zA-Z_ #]", "");

        public static string TrimName(string name) => Regex.Replace(name?.Replace("_", " "), "[0-9]", "").Trim();

        internal static string SearchName(string name) => Regex.Replace(name.Replace("_", " "), "[0-9#]", "").Trim();

        public static bool RegexMatch(string name) => Regex.IsMatch(name, "^[A-Z#]");

        public static Regex GetRegex(string name) => new Regex($".*{name}.*", RegexOptions.IgnoreCase);

        public static bool IsSubstring(string toSearch, string forSearch)
        {
            var regex = GetRegex(forSearch);
            return regex.Match(toSearch).Success;
        }
    }
}