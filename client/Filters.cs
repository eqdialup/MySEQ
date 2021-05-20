// Class Files

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

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
            // only add to list, if not in list already
            try
            {
                foreach (var item in list)
                {
                    if (string.Compare(item, additem, true) == 0)
                    {
                        return;
                    }
                }
                list.Add(additem);
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

            MakeFilterExist(filterFile);

            ReadAlertLines(zoneName, filterFile);
        }

        private void ReadAlertLines(string zoneName, string filterFile)
        {
            var type = 0;
            foreach (var line in File.ReadAllLines(filterFile))
            {
                var inp = line.Trim();
                if (inp.Length > 1)
                {
                    if (inp.StartsWith("<section name=\"hunt\">", true, null))
                    {
                        type = 1;
                    }
                    else if (inp.StartsWith("<section name=\"caution\">", true, null))
                    {
                        type = 2;
                    }
                    else if (inp.StartsWith("<section name=\"danger\">", true, null))
                    {
                        type = 3;
                    }
                    else if (inp.StartsWith("<section name=\"alert\">", true, null))
                    {
                        type = 4;
                    }
                    else if (inp.StartsWith("<section name=\"email\">", true, null) || inp.StartsWith("<section name=\"locate\">", true, null))
                    {
                        type = 5;
                    }
                    else if (inp.StartsWith("<section name=\"primary\">", true, null) || inp.StartsWith("<section name=\"offhand\">", true, null))
                    {
                        type = 6;
                    }
                    else if (type > 0 && type < 7)
                    {
                        // unknown section headers

                        if (inp.StartsWith("</section>", true, null))
                        {
                            type = 0;
                            continue;
                        }

                        var inputstring = line.Trim();
                        // Remove extra stuff

                        inputstring = FormatStrings(inp, inputstring);
                        // remove Name: from line if it exists
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
            if (!string.IsNullOrEmpty(zoneName))
            {
                zoneName = zoneName.ToLower();

                var filterFile = CombineFilter($"{zoneName}.xml");

                MakeFilterExist(filterFile);

                Process.Start("notepad.exe", filterFile);
            }
        }
    }
}