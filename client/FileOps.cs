using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using myseq.Properties;

namespace Structures
{
    public class FileOps
    {
        public static string CombineCfgDir(string file) => Path.Combine(Settings.Default.CfgDir, file);

        public static string CombineTimer(string mapName) => Path.Combine(Settings.Default.TimerDir, $"spawns-{mapName}.txt");

        public static string CombineFilter(string filename) => Path.Combine(Settings.Default.FilterDir, filename);

        public static string CombineLog(string filename) => Path.Combine(Settings.Default.LogDir, filename);

        public static void DeleteFile(string timerfile)
        {
            if (File.Exists(timerfile))
            {
                File.Delete(timerfile);
            }
        }

        public void MakeFilter(string filterFile)
        {
            if (!File.Exists(filterFile))
            {
                CreateAlertFile(filterFile);
            }
        }

        public string[] GetStrArrayFromTextFile(string file)
        {
            var filePath = CombineCfgDir(file);

            // Check if the file exists before processing
            if (!File.Exists(filePath))
            {
                return Array.Empty<string>();
            }

            // Read all lines, filter out comments and return the result
            return File.ReadAllLines(filePath)
                .Select(line => line.Trim())
                .Where(trimmedLine => !string.IsNullOrEmpty(trimmedLine) && !trimmedLine.StartsWith("#"))
                .ToArray();
        }

        public void ReadItemList(string file, myseq.EQData eq)
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
                    eq.GroundSpawn.Add(new ListItem
                    {
                        ID = int.Parse(tmp[0].Remove(0, 2)),
                        ActorDef = entries[0],
                        Name = entries[1]
                    });
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
                Settings.Default.MapDir = StartPath("maps");
                Directory.CreateDirectory(Settings.Default.MapDir);
            }
            if (string.IsNullOrEmpty(Settings.Default.FilterDir))
            {
                Settings.Default.FilterDir = StartPath("filters");
                Directory.CreateDirectory(Settings.Default.FilterDir);
            }
            if (string.IsNullOrEmpty(Settings.Default.CfgDir))
            {
                Settings.Default.CfgDir = StartPath("cfg");
                Directory.CreateDirectory(Settings.Default.CfgDir);
            }
            if (string.IsNullOrEmpty(Settings.Default.LogDir))
            {
                Settings.Default.LogDir = StartPath("logs");
                Directory.CreateDirectory(Settings.Default.LogDir);
            }
            if (string.IsNullOrEmpty(Settings.Default.TimerDir))
            {
                Settings.Default.TimerDir = StartPath("timers");
                Directory.CreateDirectory(Settings.Default.TimerDir);
            }
        }

        public static string StartPath(string folder) => Path.Combine(Application.StartupPath, folder);
    }
}