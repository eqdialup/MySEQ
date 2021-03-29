// Class Files

using myseq.Properties;
using Structures;
using System;
using System.Collections;
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
            // Load the 5.xx version alerts
            StreamReader sw;

            try
            {
                string filterDir = Settings.Default.FilterDir;
                zoneName = zoneName.ToLower();

                string filterFile = Path.Combine(filterDir, $"{zoneName}.xml");

                if (!File.Exists(filterFile))
                {
                    // we did not find the alert file
                    // create an empty alerts file.
                    LogLib.WriteLine($"Alert file not found for {zoneName}, creating empty one.", LogLevel.Warning);

                    CreateAlertFile(filterFile);
                    return;
                }

                // open the existing filter file
                sw = new StreamReader(File.Open(filterFile, FileMode.Open));
            }
            catch (Exception ex)
            {
                LogLib.WriteLine($"Error opening file stream for {zoneName}: ", ex);
                return;
            }

            string inp;
            while ((inp = sw.ReadLine()) != null)
            {
                inp = inp.Trim();
                if (inp.Length > 1 && !inp.StartsWith("<!"))
                {
                    inp = inp.ToLower();

                    if (inp.StartsWith("<section name=\"hunt\">"))
                    {
                        type = 1;
                    }
                    else if (inp.StartsWith("<section name=\"caution\">"))
                    {
                        type = 2;
                    }
                    else if (inp.StartsWith("<section name=\"danger\">"))
                    {
                        type = 3;
                    }
                    else if (inp.StartsWith("<section name=\"alert\">") || inp.StartsWith("<section name=\"locate\">"))
                    {
                        type = 4;
                    }
                    else if (inp.StartsWith("<section name=\"email\">"))
                    {
                        type = 5;
                    }
                    else if (inp.StartsWith("<section name=\"primary\">") || inp.StartsWith("<section name=\"offhand\">"))
                    {
                        type = 6;
                    }
                    else if (type > 0 && type < 7)
                    {
                        // unknown section headers

                        if (inp.StartsWith("</section>"))
                        {
                            type = 0;
                            continue;
                        }

                        string inputstring = inp;
                        // Remove extra stuff

                        if (inp.StartsWith("<oldfilter>"))
                        {
                            inp = inp.Remove(0, 11);

                            inputstring = inputstring.Remove(0, 11);
                        }
                        if (inp.StartsWith("<regex>"))
                        {
                            inp = inp.Remove(0, 7);

                            inputstring = inputstring.Remove(0, 7);
                        }

                        if (inp.EndsWith("</oldfilter>"))
                        {
                            inp = inp.Remove(inp.Length - 12, 12);

                            inputstring = inputstring.Remove(inputstring.Length - 12, 12);
                        }
                        if (inp.EndsWith("</regex>"))
                        {
                            inp = inp.Remove(inp.Length - 8, 8);

                            inputstring = inputstring.Remove(inputstring.Length - 8, 8);
                        }
                        // remove Name: from line if it exists

                        if (inp.StartsWith("name:"))
                        {
                            inp = inp.Remove(0, 5);

                            inputstring = inputstring.Remove(0, 5);
                        }

                        // if there are any odd characters in the name, just skip it

                        if (inp.IndexOfAny(anyOf) != -1)
                            continue;

                        if (inputstring.IndexOfAny(anyOf) != -1)
                            continue;

                        DetermineType(zoneName, type, inputstring);
                    }
                }
            }

            sw.Close();
        }

        private void DetermineType(string zoneName, int type, string inputstring)
        {
            if (zoneName == "global")
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
            else
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
        }

        public void WriteAlertFile(string zoneName)
        {
            // Write new xml alert file
            try
            {
                string filterFile;

                string filterDir = Settings.Default.FilterDir;

                zoneName = zoneName.ToLower();

                filterFile = Path.Combine(filterDir, $"{zoneName}.xml");

                if (File.Exists(filterFile))
                {
                    File.Delete(filterFile);
                }
                // create the filter file - truncate if exists - going to write all the filters

                StreamWriter sw = new StreamWriter(File.Open(filterFile, FileMode.Create));

                sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                sw.WriteLine("<!DOCTYPE seqfilters SYSTEM \"seqfilters.dtd\">");
                sw.WriteLine("<seqfilters>");
                sw.WriteLine("    <section name=\"Hunt\">");

                if (zoneName == "global")
                {
                    foreach (string str in GlobalHunt)

                    {
                        if (str.Length > 0)

                        {
                            sw.WriteLine("        <oldfilter><regex>Name:" + str + "</regex></oldfilter>");
                        }
                    }
                }
                else
                {
                    foreach (string str in Hunt)

                    {
                        if (str.Length > 0)

                        {
                            sw.WriteLine("        <oldfilter><regex>Name:" + str + "</regex></oldfilter>");
                        }
                    }
                }

                sw.WriteLine("    </section>");
                sw.WriteLine("    <section name=\"Caution\">");

                if (zoneName == "global")
                {
                    foreach (string str in GlobalCaution)

                    {
                        if (str.Length > 0)

                        {
                            sw.WriteLine("        <oldfilter><regex>Name:" + str + "</regex></oldfilter>");
                        }
                    }
                }
                else
                {
                    foreach (string str in Caution)

                    {
                        if (str.Length > 0)

                        {
                            sw.WriteLine("        <oldfilter><regex>Name:" + str + "</regex></oldfilter>");
                        }
                    }
                }

                sw.WriteLine("    </section>");
                sw.WriteLine("    <section name=\"Danger\">");

                if (zoneName == "global")
                {
                    foreach (string str in GlobalDanger)

                    {
                        if (str.Length > 0)

                        {
                            sw.WriteLine("        <oldfilter><regex>Name:" + str + "</regex></oldfilter>");
                        }
                    }
                }
                else
                {
                    foreach (string str in Danger)

                    {
                        if (str.Length > 0)

                        {
                            sw.WriteLine("        <oldfilter><regex>Name:" + str + "</regex></oldfilter>");
                        }
                    }
                }

                sw.WriteLine("    </section>");
                sw.WriteLine("    <section name=\"Locate\">"); // Not Used in MySEQ
                sw.WriteLine("    </section>");
                sw.WriteLine("    <section name=\"Alert\">");  // Rares

                if (zoneName == "global")
                {
                    foreach (string str in GlobalAlert)

                    {
                        if (str.Length > 0)

                        {
                            sw.WriteLine("        <oldfilter><regex>Name:" + str + "</regex></oldfilter>");
                        }
                    }
                }
                else
                {
                    foreach (string str in Alert)

                    {
                        if (str.Length > 0)

                        {
                            sw.WriteLine("        <oldfilter><regex>Name:" + str + "</regex></oldfilter>");
                        }
                    }
                }

                sw.WriteLine("    </section>");

                sw.WriteLine("    <section name=\"Filtered\">");

                sw.WriteLine("    </section>");

                if (zoneName != "global")
                {
                    sw.WriteLine("    <section name=\"Email\">");  // Email Alerts - zone only
                    foreach (string str in EmailAlert)
                    {
                        if (str.Length > 0)
                        {
                            sw.WriteLine("        <oldfilter><regex>Name:" + str + "</regex></oldfilter>");
                        }
                    }
                    sw.WriteLine("    </section>");

                    sw.WriteLine("    <section name=\"Primary\">");  // Item In Primary Hand Alerts - zone only
                    foreach (string str in WieldedItems)
                    {
                        if (str.Length > 0)
                        {
                            sw.WriteLine("        <oldfilter><regex>Name:" + str + "</regex></oldfilter>");
                        }
                    }
                    sw.WriteLine("    </section>");

                    sw.WriteLine("    <section name=\"Offhand\">");  // Item In Offhand Hand Alerts - zone only
                    foreach (string str in WieldedItems)
                    {
                        if (str.Length > 0)
                        {
                            sw.WriteLine("        <oldfilter><regex>Name:" + str + "</regex></oldfilter>");
                        }
                    }
                    sw.WriteLine("    </section>");
                }

                sw.WriteLine("</seqfilters>");

                sw.Close();

                // Only want one alert file.  Get rid of old format files.

                PurgeFilters(zoneName, filterDir);
            }
            catch (Exception ex)

            {
                LogLib.WriteLine($"Error opening file stream for {zoneName}: ", ex);
            }
        }

        private static void PurgeFilters(string zoneName, string filterDir)
        {
            var filterFile = Path.Combine(filterDir, $"custom_{zoneName}.conf");
            if (File.Exists(filterFile))

            {
                File.Delete(filterFile);
            }

            filterFile = Path.Combine(filterDir, $"filters_{zoneName}.conf");

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
            string filterDir = Settings.Default.FilterDir;

            if ((zoneName?.Length) != 0)
            {
                zoneName = zoneName.ToLower();

                string filterFile = Path.Combine(filterDir, $"{zoneName}.xml");

                if (!File.Exists(filterFile))
                    CreateAlertFile(filterFile);

                Process.Start("notepad.exe", filterFile);
            }
        }

        private void createCustomAlertFile(string fileName)
		{
			StreamWriter streamWriter = new StreamWriter(fileName);
			streamWriter.WriteLine("# Include # on every line that is not a filter");
			streamWriter.WriteLine("# Be sure to only have up to 1 extra space at the end ");
			streamWriter.WriteLine("#");
			streamWriter.WriteLine("#");
			streamWriter.WriteLine("# Hunt will put a yellow circle around the mob");
			streamWriter.WriteLine("[hunt]");
			streamWriter.WriteLine("#");
			streamWriter.WriteLine("#");
			streamWriter.WriteLine("# Caution will draw an flashing line to the target (traps etc)");
			streamWriter.WriteLine("[caution]");
			streamWriter.WriteLine("#");
			streamWriter.WriteLine("#");
			streamWriter.WriteLine("# Rare will draw a flashing white circle around the mob");
			streamWriter.WriteLine("[rare]");
			streamWriter.Close();
		}
    }
}