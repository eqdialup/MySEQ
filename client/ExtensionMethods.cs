using myseq.Properties;
using Structures;
using System.Diagnostics;
using System.Drawing;

namespace myseq
{
    public static class ExtensionMethods
    {
        public static bool IsMount(this string mobName)
        => mobName.IndexOf("s_Mount") >= 1;

        public static bool IsFamiliar(this string mobName)
        => mobName.IndexOf("`s_fami") >= 1;

        public static bool IsMerc(this string mobName)
        => mobName.IndexOf("'s Merc") >= 1;

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

        public static bool ValidateZNum(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            if (str.Length == 1 && str == ".")
                return true;
            if (decimal.TryParse(str, out var num))
            {
                return num >= 0 && num <= 3500;
            }
            return false;
        }

        public static string GetLastSlash(this string filename)
        {
            var lastSlashIndex = filename.LastIndexOf('\\');

            // If a backslash is found, return the substring after the last slash
            return lastSlashIndex >= 0 ? filename.Substring(lastSlashIndex + 1) : filename;
        }

        public static void StartSearch(this string mobname)
        {
            var searchname = mobname.SearchName();

            if (searchname.Length > 0)
            {
                var searchURL = string.Format(Settings.Default.SearchString, searchname);

                Process.Start(new ProcessStartInfo(searchURL) { UseShellExecute = true });
            }
        }
    }
}