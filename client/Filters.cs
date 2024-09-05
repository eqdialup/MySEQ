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

        public void AddToAlerts(List<string> list, string addItem)
        {
            if (list == null || string.IsNullOrWhiteSpace(addItem))
            {
                LogLib.WriteLine($"Invalid input. List or item is null/empty.");
                return;
            }
            try
            {
                // Check if the item is already in the list (case-insensitive comparison)
                if (!list.Contains(addItem, StringComparer.OrdinalIgnoreCase))
                {
                    list.Add(addItem);
                }
            }
            catch (Exception ex)
            {
                LogLib.WriteLine($"Error adding alert for '{addItem}': {ex.Message}", ex);
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
            var sectionTypes = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
    {
        { "hunt", 1 },
        { "caution", 2 },
        { "danger", 3 },
        { "alert", 4 },
        { "email", 5 }, { "locate", 5 },
        { "primary", 6 }, { "offhand", 6 }
    };

            int type = 0;

            foreach (var line in File.ReadLines(filterFile))
            {
                var inp = line.Trim();
                if (string.IsNullOrEmpty(inp) || inp.Length <= 1) continue;

                if (inp.StartsWith("<section name=\"", StringComparison.OrdinalIgnoreCase))
                {
                    type = GetSectionType(inp, sectionTypes);
                }
                else if (type > 0 && type < 7)
                {
                    if (inp.StartsWith("</section>", StringComparison.OrdinalIgnoreCase))
                    {
                        type = 0;
                        continue;
                    }

                    var formattedString = FormatStrings(inp, inp);
                    DetermineType(type, formattedString, zoneName);
                }
            }
        }

        private int GetSectionType(string input, Dictionary<string, int> sectionTypes)
        {
            foreach (var entry in sectionTypes)
            {
                if (input.StartsWith($"<section name=\"{entry.Key}\"", StringComparison.OrdinalIgnoreCase))
                {
                    return entry.Value;
                }
            }
            return 0;
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
            // Write new xml alert file
            try
            {
                zoneName = zoneName.ToLower();

                var filterFile = CombineFilter($"{zoneName}.xml");

                DeleteFile(filterFile);
                // create the filter file - truncate if exists - going to write all the filters

                if (zoneName == "global")
                {
                    WriteGlobalFile(filterFile);
                }
                else
                {
                    WriteZoneFile(filterFile);
                }
                PurgeFilters(zoneName);
            }
            catch (Exception ex)
            {
                LogLib.WriteLine($"Error opening writing filter file for {zoneName}: ", ex);
            }
        }

        private void WriteZoneFile(string filterFile)
        {
            List<string> lines = ListHeader();
            AddNameFromList(lines, Hunt);
            lines.Add("    </section>\n    <section name=\"Caution\">");
            AddNameFromList(lines, Caution);
            lines.Add("    </section>\n     <section name=\"Danger\">");
            AddNameFromList(lines, Danger);
            lines.Add("    </section>\n    <section name=\"Alert\">");  // Rares
            AddNameFromList(lines, Alert);
            lines.Add("    </section>");
            lines.Add("    <section name=\"Email\">");  // Email Alerts - zone only
            AddNameFromList(lines, EmailAlert);
            lines.Add("    <section name=\"Primary\">");  // Item In Primary Hand Alerts - zone only
            AddNameFromList(lines, WieldedItems);
            lines.Add("    </section>");
            lines.Add("    <section name=\"Offhand\">");  // Item In Offhand Hand Alerts - zone only
            AddNameFromList(lines, WieldedItems);
            ListFooter(lines);

            File.WriteAllLines(filterFile, lines);
        }

        private void WriteGlobalFile(string filterFile)
        {
            List<string> lines = ListHeader();
            AddNameFromList(lines, GlobalHunt);
            lines.Add("    </section>\n    <section name=\"Caution\">");
            AddNameFromList(lines, GlobalCaution);
            lines.Add("    </section>\n     <section name=\"Danger\">");
            AddNameFromList(lines, GlobalDanger);
            lines.Add("    </section>\n    <section name=\"Alert\">");
            AddNameFromList(lines, GlobalAlert);
            ListFooter(lines);

            File.WriteAllLines(filterFile, lines);
        }

        private static List<string> ListHeader() => new List<string>
                {
                    "<?xml version=\"1.0\" encoding=\"UTF-8\"?>",
                    "<!DOCTYPE seqfilters SYSTEM \"seqfilters.dtd\">",
                    "<seqfilters>",
                    "    <section name=\"Hunt\">"
                };

        private static void ListFooter(List<string> lines)
        {
            lines.Add("    </section>");
            lines.Add("</seqfilters>");
        }

        private void AddNameFromList(List<string> lines, List<string> alertlist)
        {
            foreach (var str in alertlist)
            {
                lines.Add(str);
            }
        }

        private static void PurgeFilters(string zoneName)
        {
            DeleteFile(CombineFilter($"custom_{zoneName}.conf"));
            DeleteFile(CombineFilter($"filters_{zoneName}.conf"));
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

        private void OpenInNotepad(string filePath)
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
    }
}