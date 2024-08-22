// Class Files

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Structures
{
    public class Filters : FileOps
    {
        public static List<string> Hunt { get; set; } = new List<string>();
        public static List<string> GlobalHunt { get; set; } = new List<string>();
        public static List<string> Caution { get; set; } = new List<string>();
        public static List<string> GlobalCaution { get; set; } = new List<string>();
        public static List<string> Danger { get; set; } = new List<string>();
        public static List<string> GlobalDanger { get; set; } = new List<string>();
        public static List<string> Alert { get; set; } = new List<string>();
        public static List<string> GlobalAlert { get; set; } = new List<string>();
        public static List<string> EmailAlert { get; set; } = new List<string>();
        public static List<string> WieldedItems { get; set; } = new List<string>();

        public void ClearLists()
        {
            Hunt.Clear();
            Caution.Clear();
            Danger.Clear();
            Alert.Clear();
            GlobalHunt.Clear();
            GlobalCaution.Clear();
            GlobalDanger.Clear();
            GlobalAlert.Clear();
            EmailAlert.Clear();
            WieldedItems.Clear();
        }

        public void AddToAlerts(List<string> list, string additem)
        {
            try
            {
                if (!list.Any(item => string.Compare(item, additem, true) == 0))
                {
                    list.Add(additem);
                }
            }
            catch (Exception ex)
            {
                LogLib.WriteLine($"Error Adding Alert for {additem}: ", ex);
            }
        }

        public void ReadNewAlertFile(string zoneName)
        {
            zoneName = zoneName.ToLower();

            var filterFile = CombineFilter($"{zoneName}.xml");

            MakeFilter(filterFile);

            ReadAlertLines(zoneName, filterFile);
        }

        private void ReadAlertLines(string zoneName, string filterFile)
        {
            var typeLookup = new Dictionary<string, int>
            {
            {"<section name=\"hunt\">", 1},
            {"<section name=\"caution\">", 2},
            {"<section name=\"danger\">", 3},
            {"<section name=\"alert\">", 4},
            {"<section name=\"email\">", 5},
            {"<section name=\"locate\">", 5},
            {"<section name=\"primary\">", 6},
            {"<section name=\"offhand\">", 6},
            };

            var type = 0;
            foreach (var line in File.ReadAllLines(filterFile))
            {
                var inp = line.Trim();
                if (inp.Length > 1)
                {
                    if (typeLookup.TryGetValue(inp, out var newType))
                    {
                        type = newType;
                    }
                    else if (type > 0 && type < 7)
                    {
                        if (inp == "</section>")
                        {
                            type = 0;
                            continue;
                        }

                        var inputstring = line.Trim();
                        inputstring = FormatStrings(inp, inputstring);
                        DetermineType(type, inputstring, zoneName);
                    }
                }
            }
        }

        private static string FormatStrings(string inp, string inputstring)
        {
            if (inp.StartsWith("<oldfilter>"))
            {
                inputstring = inp.Remove(0, 11);
            }
            if (inputstring.StartsWith("<regex>"))
            {
                inputstring = inputstring.Remove(0, 7);
            }
            if (inputstring.EndsWith("</oldfilter>"))
            {
                inputstring = inputstring.Remove(inputstring.Length - 12, 12);
            }
            if (inputstring.EndsWith("</regex>"))
            {
                inputstring = inputstring.Remove(inputstring.Length - 8, 8);
            }
            if (inputstring.StartsWith("name:"))
            {
                inputstring = inputstring.Remove(0, 5);
            }

            return inputstring;
        }

        private void DetermineType(int type, string inputstring, string zoneName)
        {
            if (zoneName == "global")
            {
                AddGlobalFilter(type, inputstring);
            }
            else
            {
                AddZoneFilter(type, inputstring);
            }
        }

        private void AddGlobalFilter(int type, string inputstring)
        {
            switch (type)
            {
                case 1:
                    AddToAlerts(GlobalHunt, inputstring);
                    break;

                case 2:
                    AddToAlerts(GlobalCaution, inputstring);
                    break;

                case 3:
                    AddToAlerts(GlobalDanger, inputstring);
                    break;

                case 4:
                    AddToAlerts(GlobalAlert, inputstring);
                    break;
            }
        }

        private void AddZoneFilter(int type, string inputstring)
        {
            switch (type)
            {
                case 1:
                    AddToAlerts(Hunt, inputstring);
                    break;

                case 2:
                    AddToAlerts(Caution, inputstring);
                    break;

                case 3:
                    AddToAlerts(Danger, inputstring);
                    break;

                case 4:
                    AddToAlerts(Alert, inputstring);
                    break;

                case 5:
                    AddToAlerts(EmailAlert, inputstring);
                    break;

                case 6:
                    AddToAlerts(WieldedItems, inputstring);
                    break;
            }
        }

        public void WriteAlertFile(string zoneName)
        {
            if (zoneName == "global")
            {
                WriteGlobalFile();
            }
            else
            {
                WriteZoneFile(zoneName);
            }
            DeleteFile(CombineFilter($"custom_{zoneName}.conf"));
            DeleteFile(CombineFilter($"filters_{zoneName}.conf"));
        }

        public void WriteZoneFile(string zoneName)
        {
            zoneName = zoneName.ToLower();
            var filterFile = CombineFilter($"{zoneName}.xml");

            using (var writer = new StreamWriter(filterFile))
            {
                writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                writer.WriteLine("<filters>");

                WriteSection(writer, "hunt", Hunt);
                WriteSection(writer, "caution", Caution);
                WriteSection(writer, "danger", Danger);
                WriteSection(writer, "alert", Alert);
                WriteSection(writer, "email", EmailAlert);
                WriteSection(writer, "primary", WieldedItems);

                writer.WriteLine("</filters>");
            }
        }

        private void WriteGlobalFile()
        {
            var globalFile = CombineFilter("global.xml");
            MakeFilter(globalFile);

            using (var writer = new StreamWriter(globalFile, false))
            {
                WriteSection(writer, "Hunt", GlobalHunt);
                WriteSection(writer, "Caution", GlobalCaution);
                WriteSection(writer, "Danger", GlobalDanger);
                WriteSection(writer, "Alert", GlobalAlert);
                WriteSection(writer, "Email", EmailAlert);
                WriteSection(writer, "Primary", WieldedItems);
                WriteSection(writer, "Offhand", WieldedItems);
            }
        }

        private void WriteSection(StreamWriter writer, string sectionName, List<string> items)
        {
            writer.WriteLine($"<section name=\"{sectionName}\">");
            foreach (var item in items)
            {
                writer.WriteLine($"<oldfilter>{item}</oldfilter>");
            }
            writer.WriteLine("</section>");
        }

        public void LoadAlerts(string zoneName)
        {
            if (!string.IsNullOrEmpty(zoneName))
            {
                ReadNewAlertFile(zoneName);
                ReadNewAlertFile("global");
            }
        }

        public void EditAlertFile(string zoneName)
        {
            if (!string.IsNullOrEmpty(zoneName))
            {
                zoneName = zoneName.ToLower();

                var filterFile = CombineFilter($"{zoneName}.xml");

                MakeFilter(filterFile);

                Process.Start("notepad.exe", filterFile);
            }
        }
    }
}