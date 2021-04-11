using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
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
        public static bool IsMount(string mobName)
        => mobName.IndexOf("s_Mount") >= 1;

        public static bool IsFamiliar(string mobName)
        => mobName.IndexOf("`s_fami") >= 1;

        public static bool IsMerc(string mobName)
        => mobName.IndexOf("'s Merc") >= 1;

        public static string FixMobName(string name)
            => name?.IndexOf("_", StringComparison.OrdinalIgnoreCase) == 0 ? name : name?.Replace("_", " ").Trim();

        public static string FixMercName(string name) => Regex.Replace(name, "^*[^a-zA-Z ]", "");

        public static string FixMobNameMatch(string name) => Regex.Replace(name, "^*[^a-zA-Z #]", "");

        public static string FilterMobName(string name) => Regex.Replace(name, "^*[^a-zA-Z_ #]", "").Trim();

        public static string TrimName(string name) => Regex.Replace(name?.Replace("_", " "), "[0-9]", "").Trim();

        internal static string SearchName(string name) => Regex.Replace(name.Replace("_", " "), "[0-9#]", "").Trim();

        public static bool RegexMatch(string name) => Regex.IsMatch(name, "^[A-Z#]");

        public static Regex GetRegex(string name) => new Regex($".*{name}.*", RegexOptions.IgnoreCase);

        public static bool IsSubstring(string toSearch, string forSearch)
        {
            Regex regex = GetRegex(forSearch);
            return regex.Match(toSearch).Success;
        }

        public static void CreateFolders()
        {
            if (Settings.Default.MapDir?.Length == 0 || !Directory.Exists(Settings.Default.MapDir))
            {
                Settings.Default.MapDir = Path.Combine(Application.ExecutablePath, "maps");
                Directory.CreateDirectory(Settings.Default.MapDir);
            }

            if (Settings.Default.FilterDir?.Length == 0 || !Directory.Exists(Settings.Default.FilterDir))
            {
                Settings.Default.FilterDir = Path.Combine(Application.ExecutablePath, "filters");
                Directory.CreateDirectory(Settings.Default.FilterDir);
            }

            if (Settings.Default.CfgDir?.Length == 0 || !Directory.Exists(Settings.Default.CfgDir))
            {
                Settings.Default.CfgDir = Path.Combine(Application.ExecutablePath, "cfg");
                Directory.CreateDirectory(Settings.Default.CfgDir);
            }

            if (Settings.Default.LogDir?.Length == 0 || !Directory.Exists(Settings.Default.LogDir))
            {
                Settings.Default.LogDir = Path.Combine(Application.ExecutablePath, "logs");
                Directory.CreateDirectory(Settings.Default.LogDir);
            }

            if (Settings.Default.TimerDir?.Length == 0 || !Directory.Exists(Settings.Default.TimerDir))
            {
                Settings.Default.TimerDir = Path.Combine(Application.ExecutablePath, "timers");
                Directory.CreateDirectory(Settings.Default.TimerDir);
            }
        }
    }
}