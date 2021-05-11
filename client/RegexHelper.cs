using System;
using System.Drawing;
using System.Text.RegularExpressions;
using myseq.Properties;

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
        public static bool IsMount(this string mobName)
        => mobName.IndexOf("s_Mount") >= 1;

        public static bool IsFamiliar(this string mobName)
        => mobName.IndexOf("`s_fami") >= 1;

        public static bool IsMerc(this string mobName)
        => mobName.IndexOf("'s Merc") >= 1;

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

        public static void MakeVisOnWhite(this Spawninfo si)
        {
            if (Settings.Default.ListBackColor == Color.White)
            {
                if (si.listitem.ForeColor == Color.White)
                {
                    si.listitem.ForeColor = Color.Black;
                }
                else if (si.listitem.ForeColor == Color.Yellow)
                {
                    si.listitem.ForeColor = Color.Goldenrod;
                }
            }
        }

        public  static bool ValidateZNum(this string Str)
        {
            var validnum = true;
            if (Str.Length > 0)
            {
                var isNum = decimal.TryParse(Str, out var Num);
                validnum = false;
                if (isNum)
                {
                    validnum = Num >= 0 && Num <= 3500;
                }
                if (Str.Length == 1 && Str == ".")
                {
                    validnum = true;
                }
            }

            return validnum;
        }
    }
}