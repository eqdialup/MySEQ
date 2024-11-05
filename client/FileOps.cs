using myseq.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

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

        public static void MakeFilter(string filterFile)
        {
            if (!File.Exists(filterFile))
            {
                CreateAlertFile(filterFile);
            }
        }

        public static string[] GetStrArrayFromTextFile(string file)
        {
            var filePath = CombineCfgDir(file);

            // Return an empty array if the file does not exist
            if (!File.Exists(filePath))
            {
                return Array.Empty<string>();
            }

            try
            {
                // Read all lines, trim, filter out comments and empty lines, and return the result as an array
                return File.ReadLines(filePath)
                           .Select(line => line.Trim())
                           .Where(line => !string.IsNullOrEmpty(line) && !line.StartsWith("#"))
                           .ToArray();
            }
            catch (Exception ex)
            {
                // Log error or handle it accordingly
                Console.WriteLine($"Failed to read the file: {filePath}. Error: {ex.Message}");
                return Array.Empty<string>();
            }
        }

        public static void ReadIniFile(string file, myseq.EQData eq)
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

        private static void CreateAlertFile(string fileName)
        {
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                var isGlobal = fileName.EndsWith("global.xml", StringComparison.OrdinalIgnoreCase);

                sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                sw.WriteLine("<!DOCTYPE seqfilters SYSTEM \"seqfilters.dtd\">");
                sw.WriteLine("<seqfilters>");
                // Write each alert section
                WriteSection(sw, "Hunt");
                WriteSection(sw, "Caution");
                WriteSection(sw, "Danger");
                WriteSection(sw, "Locate");   // Not used in MySEQ
                WriteSection(sw, "Alert");    // Rares
                WriteSection(sw, "Filtered");

                // Write additional sections if the file is not global
                if (!isGlobal)
                {
                    WriteSection(sw, "Email");
                    WriteSection(sw, "Primary");
                    WriteSection(sw, "Offhand");
                }
                sw.WriteLine("</seqfilters>");
            }
        }

        private static void WriteSection(StreamWriter sw, string sectionName)
        {
            sw.WriteLine($"    <section name=\"{sectionName}\">");
            sw.WriteLine("    </section>");
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

        public static void PurgeFilters(string zoneName)
        {
            DeleteFile(CombineFilter($"custom_{zoneName}.conf"));
            DeleteFile(CombineFilter($"filters_{zoneName}.conf"));
        }

        private static void OpenInNotepad(string filePath)
        {
            try
            {
                Process.Start("notepad.exe", filePath);
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., Notepad not found, file path issues)
                LogLib.WriteLine($"Failed to open file in Notepad: {filePath}", ex);
            }
        }

        public static void EditAlertFile(string zoneName)
        {
            if (string.IsNullOrWhiteSpace(zoneName)) return;

            // Convert zone name to lowercase
            zoneName = zoneName.Trim().ToLower();

            // Combine zone name with the .xml extension to get the filter file path
            var filterFile = CombineFilter($"{zoneName}.xml");

            // Create the filter file if it doesn't already exist
            MakeFilter(filterFile);

            // Open the filter file in Notepad
            OpenInNotepad(filterFile);
        }

        //public static void ReadGuildList(string file)
        //{
        //   var filePath = CombineCfgDir(file);
        //    string line = "";

        //    IFormatProvider NumFormat = new CultureInfo("en-US");

        //    if (!File.Exists(filePath))
        //    {
        //        // we did not find the Guild file
        //        LogLib.WriteLine("Guild file not found", LogLevel.Warning);
        //        return;
        //    }

        //    FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        //    StreamReader sr = new StreamReader(fs);

        //    while (line != null)
        //    {
        //        line = sr.ReadLine();

        //        if (line != null)
        //        {
        //            line = line.Trim();

        //            if (line.Length > 0 && (!line.StartsWith("[") && !line.StartsWith("#")))
        //            {
        //                ListItem thisitem = new ListItem();

        //                string tok;
        //                if ((tok = Getnexttoken(ref line, '=')) != null)
        //                {
        //                    thisitem.ActorDef = tok.ToUpper();

        //                    if ((tok = Getnexttoken(ref line, ',')) != null)
        //                    {
        //                        thisitem.Name = tok;

        //                        if ((tok = Getnexttoken(ref thisitem.ActorDef, '_')) != null)
        //                        {
        //                            thisitem.ID = int.Parse(tok, NumFormat);

        //                            // We got this far, so we have a valid item to add

        //                            if (!guildList.ContainsKey(thisitem.ID))
        //                            {
        //                                try { guildList.Add(thisitem.ID, thisitem); }
        //                                catch (Exception ex) { LogLib.WriteLine("Error adding " + thisitem.ID + " to Guilds hashtable: ", ex); }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    sr.Close();
        //    fs.Close();
        //}
        //public string GetGuildDescription(string guildDef)
        //{
        //    // I know - ## NEEDS CLEANUP
        //    // Get description from list made using Guildlist.txt
        //    string tok;
        //    return (tok = Getnexttoken(ref guildDef, '_')) != null
        //        && guildList.ContainsKey(int.Parse(tok, new CultureInfo("en-US")))
        //        ? ((ListItem)guildList[int.Parse(tok, new CultureInfo("en-US"))]).Name
        //        : guildDef;
        //}
    }
}