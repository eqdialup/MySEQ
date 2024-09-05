using System.Diagnostics;
using System.Drawing;
using myseq.Properties;
using Structures;

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
        public static decimal FiveHundred(this decimal current_val)
        {
            current_val -= 25;
            if (current_val < 400)
            {
                current_val = 400;
            }

            return current_val;
        }

        public static decimal Fourhundred(this decimal current_val)
        {
            current_val -= 25;
            if (current_val < 300)
            {
                current_val = 300;
            }

            return current_val;
        }

        public static decimal Threehundred(this decimal current_val)
        {
            current_val -= 25;
            if (current_val <= 200)
            {
                current_val = 200;
            }

            return current_val;
        }

        public static decimal Twohundred(this decimal current_val)
        {
            current_val -= 25;
            if (current_val < 100)
            {
                current_val = 100;
            }

            return current_val;
        }

        public static decimal Subhundred(this decimal current_val)
        {
            current_val -= 10;
            if (current_val < 10)
            {
                current_val = 10;
            }

            return current_val;
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

        public static bool ValidateZNum(this string Str)
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

        public static string GetLastSlash(this string filename)
        {
            var lastSlashIndex = filename.LastIndexOf("\\");

            if (lastSlashIndex > 0)
            {
                filename = filename.Substring(lastSlashIndex + 1);
            }

            return filename;
        }
        public static void StartSearch(this string mobname)
        {
            var searchname = mobname.SearchName();

            if (searchname.Length > 0)
            {
                var searchURL = string.Format(Settings.Default.SearchString, searchname);

                Process.Start(searchURL);
            }
        }
    }
}
