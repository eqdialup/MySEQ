// Class Files

using myseq.Properties;
using Structures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace myseq
{
    public class Filters
    {
        public ArrayList Hunt { get; set; } = new ArrayList();
        public ArrayList Caution { get; set; } = new ArrayList();
        public ArrayList GlobalCaution { get; set; } = new ArrayList();
        public ArrayList Danger { get; set; } = new ArrayList();
        public ArrayList GlobalHunt { get; set; } = new ArrayList();
        public ArrayList Alert { get; set; } = new ArrayList();
        public ArrayList GlobalDanger { get; set; } = new ArrayList();
        public ArrayList EmailAlert { get; set; } = new ArrayList();
        public ArrayList WieldedItems { get; set; } = new ArrayList();

        public ArrayList GlobalAlert { get; set; } = new ArrayList();

        private readonly char[] anyOf = new char[] { '[', ':', '^', '*' };

        public void ClearArrays()
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

            //            OffhandItem.Clear();
        }

        public void AddToAlerts(ArrayList list, string additem)
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
            int type = 0;
            zoneName = zoneName.ToLower();

            string filterFile = Path.Combine(Settings.Default.FilterDir, $"{zoneName}.xml");

            if (!File.Exists(filterFile))
            {
                // we did not find the alert file
                // create an empty alerts file.
                LogLib.WriteLine($"Alert file not found for {zoneName}, creating empty one.", LogLevel.Warning);

                CreateAlertFile(filterFile);
                return;
            }

            // Load the 5.xx version alerts
            // open the existing filter file
            foreach(string line in File.ReadAllLines(filterFile))
                {
                line.Trim();
                if (line.Length > 1 && !line.StartsWith("<!"))
                {
                    line.ToLower();

                    if (line.StartsWith("<section name=\"hunt\">"))
                    {
                        type = 1;
                    }
                    else if (line.StartsWith("<section name=\"caution\">"))
                    {
                        type = 2;
                    }
                    else if (line.StartsWith("<section name=\"danger\">"))
                    {
                        type = 3;
                    }
                    else if (line.StartsWith("<section name=\"alert\">") || line.StartsWith("<section name=\"locate\">"))
                    {
                        type = 4;
                    }
                    else if (line.StartsWith("<section name=\"email\">"))
                    {
                        type = 5;
                    }
                    else if (line.StartsWith("<section name=\"primary\">") || line.StartsWith("<section name=\"offhand\">"))
                    {
                        type = 6;
                    }
                    else if (type > 0 && type < 7)
                    {
                        // unknown section headers

                        if (line.StartsWith("</section>"))
                        {
                            type = 0;
                            continue;
                        }

                        string inputstring = line;
                        // Remove extra stuff

                        if (line.StartsWith("<oldfilter>"))
                        {
                            line.Remove(0, 11);

                            inputstring = inputstring.Remove(0, 11);
                        }
                        if (line.StartsWith("<regex>"))
                        {
                            line.Remove(0, 7);

                            inputstring = inputstring.Remove(0, 7);
                        }

                        if (line.EndsWith("</oldfilter>"))
                        {
                            line.Remove(line.Length - 12, 12);

                            inputstring = inputstring.Remove(inputstring.Length - 12, 12);
                        }
                        if (line.EndsWith("</regex>"))
                        {
                            line.Remove(line.Length - 8, 8);

                            inputstring = inputstring.Remove(inputstring.Length - 8, 8);
                        }
                        // remove Name: from line if it exists
                        if (line.StartsWith("name:"))
                        {
                            line.Remove(0, 5);

                            inputstring = inputstring.Remove(0, 5);
                        }

                        // if there are any odd characters in the name, just skip it
                        if (line.IndexOfAny(anyOf) != -1)
                            continue;

                        if (inputstring.IndexOfAny(anyOf) != -1)
                            continue;

                        DetermineType(zoneName, type, inputstring);
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

                string filterFile = Path.Combine(Settings.Default.FilterDir, $"{zoneName}.xml");

                if (File.Exists(filterFile))
                {
                    File.Delete(filterFile);
                }
                // create the filter file - truncate if exists - going to write all the filters

                List<string> lines = new List<string>();
                lines.Add("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                lines.Add("<!DOCTYPE seqfilters SYSTEM \"seqfilters.dtd\">");
                lines.Add("<seqfilters>");
                lines.Add("    <section name=\"Hunt\">");

                if (zoneName == "global")
                {
                    foreach (string str in GlobalHunt)
                    {
                        if (str.Length > 0)
                        {
                            lines.Add("        <oldfilter><regex>Name:" + str + "</regex></oldfilter>");
                        }
                    }
                }
                else
                {
                    foreach (string str in Hunt)
                    {
                        if (str.Length > 0)
                        {
                            lines.Add("        <oldfilter><regex>Name:" + str + "</regex></oldfilter>");
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
                            lines.Add("        <oldfilter><regex>Name:" + str + "</regex></oldfilter>");
                        }
                    }
                }
                else
                {
                    foreach (string str in Caution)
                    {
                        if (str.Length > 0)
                        {
                            lines.Add("        <oldfilter><regex>Name:" + str + "</regex></oldfilter>");
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
                            lines.Add("        <oldfilter><regex>Name:" + str + "</regex></oldfilter>");
                        }
                    }
                }
                else
                {
                    foreach (string str in Danger)
                    {
                        if (str.Length > 0)
                        {
                            lines.Add("        <oldfilter><regex>Name:" + str + "</regex></oldfilter>");
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
                            lines.Add("        <oldfilter><regex>Name:" + str + "</regex></oldfilter>");
                        }
                    }
                }
                else
                {
                    foreach (string str in Alert)
                    {
                        if (str.Length > 0)
                        {
                            lines.Add("        <oldfilter><regex>Name:" + str + "</regex></oldfilter>");
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
                            lines.Add("        <oldfilter><regex>Name:" + str + "</regex></oldfilter>");
                        }
                    }
                   lines.Add("    </section>");
                    lines.Add("    <section name=\"Primary\">");  // Item In Primary Hand Alerts - zone only
                    foreach (string str in WieldedItems)
                    {
                        if (str.Length > 0)
                        {
                            lines.Add("        <oldfilter><regex>Name:" + str + "</regex></oldfilter>");
                        }
                    }
                    lines.Add("    </section>");
                    lines.Add("    <section name=\"Offhand\">");  // Item In Offhand Hand Alerts - zone only
                    foreach (string str in WieldedItems)
                    {
                        if (str.Length > 0)
                        {
                            lines.Add("        <oldfilter><regex>Name:" + str + "</regex></oldfilter>");
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

            bool isglobal = false;

            if (fileName.EndsWith("global.xml"))
                isglobal = true;

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

                string filterFile = Path.Combine(Settings.Default.FilterDir, $"{zoneName}.xml");

                if (!File.Exists(filterFile))
                    CreateAlertFile(filterFile);

                Process.Start("notepad.exe", filterFile);
            }
        }
    }
}