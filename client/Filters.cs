using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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

        // Class-level dictionary for section types
        private static readonly Dictionary<string, int> SectionTypes = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
    {
        { "hunt", 1 },
        { "caution", 2 },
        { "danger", 3 },
        { "alert", 4 },
        { "email", 5 }, { "locate", 5 },
        { "primary", 6 }, { "offhand", 6 }
    };

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
            int currentSectionType = 0;

            foreach (var line in File.ReadLines(filterFile))
            {
                var trimmedLine = line.Trim();

                if (string.IsNullOrWhiteSpace(trimmedLine) || trimmedLine.Length <= 1)
                {
                    continue; // Skip empty or invalid lines
                }

                if (trimmedLine.StartsWith("<section name=\"", StringComparison.OrdinalIgnoreCase))
                {
                    currentSectionType = GetSectionType(trimmedLine);
                }
                else if (currentSectionType > 0 && currentSectionType < 7)
                {
                    if (trimmedLine.StartsWith("</section>", StringComparison.OrdinalIgnoreCase))
                    {
                        currentSectionType = 0; // Reset section type at the end of a section
                        continue;
                    }
                    var formattedString = FormatStrings(trimmedLine);
                    DetermineType(currentSectionType, formattedString, zoneName);
                }
            }
        }

        private int GetSectionType(string line)
        {
            var sectionName = line.Split('"')[1];
            return SectionTypes.TryGetValue(sectionName, out int type) ? type : 0;
        }

        private static string FormatStrings(string inp)
        {
            var match = Regex.Match(inp, @"name:\s*(?<name>.*?)<\/", RegexOptions.IgnoreCase);
            return match.Success ? match.Groups["name"].Value.Trim() : string.Empty;
        }

        private void DetermineType(int type, string inputstring, string zoneName)
        {
            // Determine whether it's a global or zone-specific filter and add it to the appropriate list.
            AddFilter(type, inputstring, isGlobal: zoneName.Equals("global", StringComparison.OrdinalIgnoreCase));
        }

        private void AddFilter(int type, string inputstring, bool isGlobal)
        {
            List<string> alertList = null;

            // Select the appropriate list based on the type and whether it's a global or zone-specific filter.
            switch (type)
            {
                case 1:
                    alertList = isGlobal ? GlobalHunt : Hunt;
                    break;

                case 2:
                    alertList = isGlobal ? GlobalCaution : Caution;
                    break;

                case 3:
                    alertList = isGlobal ? GlobalDanger : Danger;
                    break;

                case 4:
                    alertList = isGlobal ? GlobalAlert : Alert;
                    break;

                case 5:
                    if (!isGlobal) alertList = EmailAlert;
                    break;

                case 6:
                    if (!isGlobal) alertList = WieldedItems;
                    break;
            }

            // Add the input string to the selected list if the alertList is not null.
            if (alertList != null)
            {
                AddToAlerts(alertList, inputstring);
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
            List<string> lines = new List<string>();
            AddHeader(lines);

            AddSection(lines, "Hunt", Hunt);
            AddSection(lines, "Caution", Caution);
            AddSection(lines, "Danger", Danger);
            AddSection(lines, "Alert", Alert);
            AddSection(lines, "Email", EmailAlert);
            AddSection(lines, "Primary", WieldedItems);
            AddSection(lines, "Offhand", WieldedItems);
            lines.Add("</seqfilters>");

            File.WriteAllLines(filterFile, lines);
        }

        private void WriteGlobalFile(string filterFile)
        {
            List<string> lines = new List<string>();
            AddHeader(lines);
            // Add all the predefined sections to the lines dynamically
            AddSection(lines, "Hunt", GlobalHunt);
            AddSection(lines, "Caution", GlobalCaution);
            AddSection(lines, "Danger", GlobalDanger);
            AddSection(lines, "Alert", GlobalAlert);
            lines.Add("</seqfilters>");

            File.WriteAllLines(filterFile, lines);
        }

        // Adds the header section to the lines
        private static void AddHeader(List<string> lines)
        {
            lines.AddRange(new[]
            {
        "<?xml version=\"1.0\" encoding=\"UTF-8\"?>",
        "<!DOCTYPE seqfilters SYSTEM \"seqfilters.dtd\">",
        "<seqfilters>"
    });
        }

        // Adds a named section and its content to the lines
        private static void AddSection(List<string> lines, string sectionName, List<string> contentList)
        {
            lines.Add($"    <section name=\"{sectionName}\">");
            AddNameFromList(lines, contentList);
            lines.Add("    </section>");
        }

        private static void AddNameFromList(List<string> lines, List<string> alertlist)
        {
            foreach (var str in alertlist)
            {
                lines.Add(str);
            }
        }

        public void LoadAlerts(string zoneName)
        {
            if (!string.IsNullOrEmpty(zoneName))
            {
                ReadNewAlertFile(zoneName);
                ReadNewAlertFile("global");
            }
        }
    }
}