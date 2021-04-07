// Class Files

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using myseq.Properties;
using Structures;

namespace myseq
{
    public class Filters
    {
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

        public void ReadNewAlertFile(string zoneName)
        {
            var type = 0;
            zoneName = zoneName.ToLower();

            var filterFile = Path.Combine(Settings.Default.FilterDir, $"{zoneName}.xml");

            if (!File.Exists(filterFile))
            {
                // we did not find the alert file
                // create an empty alerts file.
                LogLib.WriteLine($"file not found for {zoneName}, creating empty one.", LogLevel.Warning);

                CreateAlertFile(filterFile);
                return;
            }

            // Load the 5.xx version alerts
            // open the existing filter file
            foreach (var line in File.ReadAllLines(filterFile))
            {
                var inp = line.Trim();
                if (inp.Length > 1)
                {
                    if (inp.StartsWith("<section name=\"hunt\">",ignoreCase: true, culture: null))
                    {
                        type = 1;
                    }
                    else if (inp.StartsWith("<section name=\"caution\">",ignoreCase: true, culture: null))
                    {
                        type = 2;
                    }
                    else if (inp.StartsWith("<section name=\"danger\">",ignoreCase: true, culture: null))
                    {
                        type = 3;
                    }
                    else if (inp.StartsWith("<section name=\"alert\">",ignoreCase: true, culture: null))
                    {
                        type = 4;
                    }
                    else if (inp.StartsWith("<section name=\"email\">",ignoreCase: true, culture: null) || inp.StartsWith("<section name=\"locate\">",ignoreCase: true, culture: null))
                    {
                        type = 5;
                    }
                    else if (inp.StartsWith("<section name=\"primary\">",ignoreCase: true, culture: null) || inp.StartsWith("<section name=\"offhand\">",ignoreCase: true, culture: null))
                    {
                        type = 6;
                    }
                    else if (type > 0 && type < 7)
                    {
                        // unknown section headers

                        if (inp.StartsWith("</section>",ignoreCase: true, culture: null))
                        {
                            type = 0;
                            continue;
                        }

                        var inputstring = line;
                        // Remove extra stuff

                        if (inp.StartsWith("<oldfilter>",ignoreCase: true, culture: null))
                        {
                            inputstring = inp.Remove(0, 11);
                        }
                        if (inputstring.StartsWith("<regex>",ignoreCase: true, culture: null))
                        {
                            inputstring = inputstring.Remove(0, 7);
                        }
                        if (inputstring.EndsWith("</oldfilter>",ignoreCase: true, culture: null))
                        {
                            inputstring = inputstring.Remove(inputstring.Length - 12, 12);
                        }
                        if (inputstring.EndsWith("</regex>",ignoreCase: true, culture: null))
                        {
                            inputstring = inputstring.Remove(inputstring.Length - 8, 8);
                        }
                        // remove Name: from line if it exists
                        if (inputstring.StartsWith("name:",ignoreCase: true, culture: null))
                        {
                            inputstring = inputstring.Remove(0, 5);
                            DetermineType(zoneName, type, inputstring);
                        }
                    }
                }
            }
        }

        private void DetermineType(string zoneName, int type, string inputstring)
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

                if (File.Exists(filterFile))
                {
                    File.Delete(filterFile);
                }
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
                    foreach (string str in GlobalHunt)
                    {
                        if (str.Length > 0)
                        {
                            lines.Add($"        Name:{str}");
                        }
                    }
                }
                else
                {
                    foreach (string str in Hunt)
                    {
                        if (str.Length > 0)
                        {
                            lines.Add($"        Name:{str}");
                        }
                    }
                }

                lines.Add("    </section>");
                lines.Add("    <section name=\"Caution\">");

                if (zoneName == "global")
                {
                    foreach (string str in GlobalCaution)

                    {
                        if (str.Length > 0)

                        {
                            lines.Add($"        Name:{str}");
                        }
                    }
                }
                else
                {
                    foreach (string str in Caution)
                    {
                        if (str.Length > 0)
                        {
                            lines.Add($"        Name:{str}");
                        }
                    }
                }

                lines.Add("    </section>");
                lines.Add("    <section name=\"Danger\">");

                if (zoneName == "global")
                {
                    foreach (string str in GlobalDanger)
                    {
                        if (str.Length > 0)
                        {
                            lines.Add($"        Name:{str}");
                        }
                    }
                }
                else
                {
                    foreach (string str in Danger)
                    {
                        if (str.Length > 0)
                        {
                            lines.Add($"        Name:{str}");
                        }
                    }
                }

                lines.Add("    </section>");
                lines.Add("    <section name=\"Alert\">");  // Rares

                if (zoneName == "global")
                {
                    foreach (string str in GlobalAlert)
                    {
                        if (str.Length > 0)
                        {
                            lines.Add($"        Name:{str}");
                        }
                    }
                }
                else
                {
                    foreach (string str in Alert)
                    {
                        if (str.Length > 0)
                        {
                            lines.Add($"        Name:{str}");
                        }
                    }
                }
                lines.Add("    </section>");

                if (zoneName != "global")
                {
                    lines.Add("    <section name=\"Email\">");  // Email Alerts - zone only
                    foreach (string str in EmailAlert)
                    {
                        if (str.Length > 0)
                        {
                            lines.Add($"        Name:{str}");
                        }
                    }
                    lines.Add("    </section>");
                    lines.Add("    <section name=\"Primary\">");  // Item In Primary Hand Alerts - zone only
                    foreach (string str in WieldedItems)
                    {
                        if (str.Length > 0)
                        {
                            lines.Add($"        Name:{str}");
                        }
                    }
                    lines.Add("    </section>");
                    lines.Add("    <section name=\"Offhand\">");  // Item In Offhand Hand Alerts - zone only
                    foreach (string str in WieldedItems)
                    {
                        if (str.Length > 0)
                        {
                            lines.Add($"        Name:{str}");
                        }
                    }
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

        private static void PurgeFilters(string zoneName)
        {
            var filterFile = Path.Combine(Settings.Default.FilterDir, $"custom_{zoneName}.conf");
            if (File.Exists(filterFile))
            {
                File.Delete(filterFile);
            }

            filterFile = Path.Combine(Settings.Default.FilterDir, $"filters_{zoneName}.conf");
            if (File.Exists(filterFile))
            {
                File.Delete(filterFile);
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

        private void CreateAlertFile(string fileName)
        {
            StreamWriter sw = new StreamWriter(fileName);

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
            sw.Close();
        }

        public void EditAlertFile(string zoneName)
        {
            if (!string.IsNullOrEmpty(zoneName))
            {
                zoneName = zoneName.ToLower();

                var filterFile = Path.Combine(Settings.Default.FilterDir, $"{zoneName}.xml");

                if (!File.Exists(filterFile))
                {
                    CreateAlertFile(filterFile);
                }

                Process.Start("notepad.exe", filterFile);
            }
        }
    }
}