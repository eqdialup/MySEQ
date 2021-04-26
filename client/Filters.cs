// Class Files

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using myseq.Properties;
using Structures;

namespace myseq
{
    public class Filters
    {
        private readonly FileOps fileop = new FileOps();

        public List<string> Hunt { get; set; } = new List<string>();
        public List<string> Caution { get; set; } = new List<string>();
        public List<string> GlobalCaution { get; set; } = new List<string>();
        public List<string> Danger { get; set; } = new List<string>();
        public List<string> GlobalHunt { get; set; } = new List<string>();
        public List<string> Alert { get; set; } = new List<string>();
        public List<string> GlobalDanger { get; set; } = new List<string>();
        public List<string> EmailAlert { get; set; } = new List<string>();
        public List<string> WieldedItems { get; set; } = new List<string>();

        public List<string> GlobalAlert { get; set; } = new List<string>();

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
                foreach (string item in list)
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

        public async Task ReadNewAlertFile(string zoneName)
        {
            zoneName = zoneName.ToLower();

            var filterFile = Path.Combine(Settings.Default.FilterDir, $"{zoneName}.xml");

            fileop.MakeFilterExist(filterFile);

            await ReadAlertLines(zoneName, filterFile);
        }

        private async Task ReadAlertLines(string zoneName, string filterFile)
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

                        var inputstring = line;
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
                if (type == 1)
                {
                    AddToAlerts(GlobalHunt, inputstring);
                }
                else if (type == 2)
                {
                    AddToAlerts(GlobalCaution, inputstring);
                }
                else if (type == 3)
                {
                    AddToAlerts(GlobalDanger, inputstring);
                }
                else if (type == 4)
                {
                    AddToAlerts(GlobalAlert, inputstring);
                }
            }
            else
            {
                if (type == 1)
                {
                    AddToAlerts(Hunt, inputstring);
                }
                else if (type == 2)
                {
                    AddToAlerts(Caution, inputstring);
                }
                else if (type == 3)
                {
                    AddToAlerts(Danger, inputstring);
                }
                else if (type == 4)
                {
                    AddToAlerts(Alert, inputstring);
                }
                else if (type == 5)
                {
                    AddToAlerts(EmailAlert, inputstring);
                }
                else if (type == 6)
                {
                    AddToAlerts(WieldedItems, inputstring);
                }
            }
        }

        public void WriteAlertFile(string zoneName)
        {
            // Write new xml alert file
            try
            {
                zoneName = zoneName.ToLower();

                var filterFile = Path.Combine(Settings.Default.FilterDir, $"{zoneName}.xml");

                FileOps.DeleteFile(filterFile);
                // create the filter file - truncate if exists - going to write all the filters

                List<string> lines = new List<string>
                {
                    "<?xml version=\"1.0\" encoding=\"UTF-8\"?>",
                    "<!DOCTYPE seqfilters SYSTEM \"seqfilters.dtd\">",
                    "<seqfilters>",
                    "    <section name=\"Hunt\">"
                };

                if (zoneName == "global")
                {
                    AddNameFromList(lines, GlobalHunt);
                }
                else
                {
                    AddNameFromList(lines, Hunt);
                }

                lines.Add("    </section>");
                lines.Add("    <section name=\"Caution\">");

                if (zoneName == "global")
                {
                    AddNameFromList(lines, GlobalCaution);
                }
                else
                {
                    AddNameFromList(lines, Caution);
                }

                lines.Add("    </section>");
                lines.Add("    <section name=\"Danger\">");

                if (zoneName == "global")
                {
                    AddNameFromList(lines, GlobalDanger);
                }
                else
                {
                    AddNameFromList(lines, Danger);
                }

                lines.Add("    </section>");
                lines.Add("    <section name=\"Alert\">");  // Rares

                if (zoneName == "global")
                {
                    AddNameFromList(lines, GlobalAlert);
                }
                else
                {
                    AddNameFromList(lines, Alert);
                }
                lines.Add("    </section>");

                if (zoneName != "global")
                {
                    lines.Add("    <section name=\"Email\">");  // Email Alerts - zone only
                    AddNameFromList(lines, EmailAlert);
                    lines.Add("    <section name=\"Primary\">");  // Item In Primary Hand Alerts - zone only
                    AddNameFromList(lines, WieldedItems);
                    lines.Add("    </section>");
                    lines.Add("    <section name=\"Offhand\">");  // Item In Offhand Hand Alerts - zone only
                    AddNameFromList(lines, WieldedItems);
                    lines.Add("    </section>");
                }
                lines.Add("</seqfilters>");

                // Only want one alert file.  Get rid of old format files.
                File.WriteAllLines(filterFile, lines);
                PurgeFilters(zoneName);
            }
            catch (Exception ex)
            {
                LogLib.WriteLine($"Error opening writing filter file for {zoneName}: ", ex);
            }
        }

        private void AddNameFromList(List<string> lines, List<string> alertlist)
        {
            foreach (string str in alertlist)
            {
                lines.Add($"        Name:{str}");
            }
        }

        private static void PurgeFilters(string zoneName)
        {
            var filterFile = Path.Combine(Settings.Default.FilterDir, $"custom_{zoneName}.conf");
            FileOps.DeleteFile(filterFile);

            filterFile = Path.Combine(Settings.Default.FilterDir, $"filters_{zoneName}.conf");
            FileOps.DeleteFile(filterFile);
        }

        public async Task LoadAlerts(string zoneName)
        {
            if (!string.IsNullOrEmpty(zoneName))
            {
                await ReadNewAlertFile(zoneName);
                await ReadNewAlertFile("global");
            }
        }

        public void EditAlertFile(string zoneName)
        {
            if (!string.IsNullOrEmpty(zoneName))
            {
                zoneName = zoneName.ToLower();

                var filterFile = Path.Combine(Settings.Default.FilterDir, $"{zoneName}.xml");

                fileop.MakeFilterExist(filterFile);

                Process.Start("notepad.exe", filterFile);
            }
        }
    }
}