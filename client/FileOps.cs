using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using myseq.Properties;

namespace Structures
{
    public class FileOps
    {
        public static string CombineCfgDir(string file) => Path.Combine(Settings.Default.CfgDir, file);
        public static string CombineSpawnTim(string mapName) => Path.Combine(Settings.Default.TimerDir, $"spawns-{mapName}.txt");

        public static void DeleteFile(string timerfile)
        {
            if (File.Exists(timerfile))
            {
                File.Delete(timerfile);
            }
        }

        public void MakeFilterExist(string filterFile)
        {
            if (!File.Exists(filterFile))
            {
                CreateAlertFile(filterFile);
            }
        }

        public string[] GetStrArrayFromTextFile(string file)
        {
            var filePath = CombineCfgDir(file);
            List<string> arList = new List<string>();
            if (File.Exists(filePath))
            {
                foreach (var line in File.ReadAllLines(filePath))
                {
                    var ln = line.Trim();
                    if (ln != null && ln.Substring(0, 1) != "#")
                    {
                        arList.Add(ln);
                    }
                }
            }
            return arList.ToArray();
        }

        public void ReadItemList(string file, ref List<ListItem> ground)
        {
            var filePath = CombineCfgDir(file);
            if (!File.Exists(filePath))
            {
                LogLib.WriteLine("GroundItems.ini file not found", LogLevel.Warning);
                return;
            }

            foreach (var line in File.ReadAllLines(filePath).ToList())
            {
                //sample:  IT0_ACTORDEF = Generic
                if (!line.StartsWith("[") && !string.IsNullOrWhiteSpace(line))
                {
                    var entries = line.Split('=');
                    var tmp = entries[0].Split('_');
                    ListItem newGround = new ListItem
                    {
                        ID = int.Parse(tmp[0].Remove(0, 2)),/* NumFormat),*/ //0
                        ActorDef = entries[0], //IT0_ACTORDEF
                        Name = entries[1] //NAME
                    };
                    ground.Add(newGround);
                }
            }
        }

        private void CreateAlertFile(string fileName)
        {
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                var isglobal = false;

                if (fileName.EndsWith("global.xml"))
                {
                    isglobal = true;
                }

                sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                sw.WriteLine("<!DOCTYPE seqfilters SYSTEM \"seqfilters.dtd\">");
                sw.WriteLine("<seqfilters>");
                sw.WriteLine("    <section name=\"Hunt\">");
                sw.WriteLine("    </section>");
                sw.WriteLine("    <section name=\"Caution\">");
                sw.WriteLine("    </section>");
                sw.WriteLine("    <section name=\"Danger\">");
                sw.WriteLine("    </section>");
                sw.WriteLine("    <section name=\"Locate\">"); // Not Used in MySEQ
                sw.WriteLine("    </section>");
                sw.WriteLine("    <section name=\"Alert\">");  // Rares
                sw.WriteLine("    </section>");
                sw.WriteLine("    <section name=\"Filtered\">");
                sw.WriteLine("    </section>");
                if (!isglobal)
                {
                    sw.WriteLine("    <section name=\"Email\">");
                    sw.WriteLine("    <section name=\"Primary\">");
                    sw.WriteLine("    <section name=\"Offhand\">");
                    sw.WriteLine("    </section>");
                }
                sw.WriteLine("</seqfilters>");
            }
        }

        public static void CreateFolders()
        {
            if (string.IsNullOrEmpty(Settings.Default.MapDir))
            {
                Settings.Default.MapDir = Path.Combine(Application.StartupPath, "maps");
                Directory.CreateDirectory(Settings.Default.MapDir);
            }
            if (string.IsNullOrEmpty(Settings.Default.FilterDir))
            {
                Settings.Default.FilterDir = Path.Combine(Application.StartupPath, "filters");
                Directory.CreateDirectory(Settings.Default.FilterDir);
            }
            if (string.IsNullOrEmpty(Settings.Default.CfgDir))
            {
                Settings.Default.CfgDir = Path.Combine(Application.StartupPath, "cfg");
                Directory.CreateDirectory(Settings.Default.CfgDir);
            }
            if (string.IsNullOrEmpty(Settings.Default.LogDir))
            {
                Settings.Default.LogDir = Path.Combine(Application.StartupPath, "logs");
                Directory.CreateDirectory(Settings.Default.LogDir);
            }
            if (string.IsNullOrEmpty(Settings.Default.TimerDir))
            {
                Settings.Default.TimerDir = Path.Combine(Application.StartupPath, "timers");
                Directory.CreateDirectory(Settings.Default.TimerDir);
            }
        }
    }
}
