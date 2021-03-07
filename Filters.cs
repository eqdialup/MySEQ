// Class Files

using Structures;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;

namespace myseq

{
    public class Filters

    {
        private static readonly ArrayList arrayList = new ArrayList();

        public ArrayList Hunt { get; set; } = arrayList;
        public ArrayList Caution { get; set; } = arrayList;
        public ArrayList GlobalCaution { get; set; } = arrayList;
        public ArrayList Danger { get; set; } = arrayList;
        public ArrayList GlobalHunt { get; set; } = arrayList;
        public ArrayList OffhandItem { get; set; } = arrayList;
        public ArrayList Alert { get; set; } = arrayList;
        public ArrayList GlobalDanger { get; set; } = arrayList;
        public ArrayList EmailAlert { get; set; } = arrayList;
        public ArrayList PrimaryItem { get; set; } = arrayList;
        public ArrayList GlobalAlert { get; set; } = arrayList;

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

            PrimaryItem.Clear();

            OffhandItem.Clear();
        }

        public void AddToAlerts(ArrayList list, string additem)

        {
            // only add to list, if not in list already

            try

            {
                foreach (string item in list)
                {
                    if (string.Compare(item, additem,true) == 0)
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

        public void ReadAlertFile(string zoneName, bool loadCustomAlerts)

        {
            int type = 0;

            // Load the 4.xx alerts

            StreamReader sw;

            try

            {
                string filterFile;

                string filterDir = Settings.Instance.FilterDir;

                zoneName = zoneName.ToLower();

                filterFile = loadCustomAlerts
                    ? Path.Combine(filterDir, $"custom_{zoneName}.conf")
                    : Path.Combine(filterDir, $"filters_{zoneName}.conf");

                if ( !File.Exists(filterFile) )
                    return;

                // open the existing filter file

                sw = new StreamReader(File.Open(filterFile,FileMode.Open));
            }
            catch (Exception ex)

            {
                LogLib.WriteLine($"Error opening file stream for {zoneName}: ", ex);

                return;
            }

            string inp;
            while ((inp = sw.ReadLine()) != null)

            {
                if (inp.Trim().Length > 0 && !inp.StartsWith("#"))

                {
                    string inputstring = inp;
                    inp = inp.ToLower();

                    if (inp.StartsWith("[hunt]"))
                    {
                        type = 1;
                    }
                    else if (inp.StartsWith("[caution]") || inp.StartsWith("[danger]"))
                    {
                        type = 2;
                    }
                    else if (inp.StartsWith("[rare]") || inp.StartsWith("[locate]") || inp.StartsWith("[alert]"))
                    {
                        type = 3;
                    }
                    else if (type > 0 && type < 4)

                    {
                        // unknown section headers

                        if (inp.StartsWith("["))

                        {
                            type = 0;

                            continue;
                        }

                        // remove Name: from line if it exists

                        if (inp.StartsWith("name:"))
                        {
                            inp = inp.Remove(0, 5);
                            inputstring = inputstring.Remove(0, 5);
                        }

                        // if there are any odd characters in the name, just skip it

                        if (inp.IndexOfAny(new char[] { '[', ':', '^', '*' }) != -1)
                            continue;

                        if (inputstring.IndexOfAny(new char[] { '[', ':', '^', '*' }) != -1)
                            continue;

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

                                    AddToAlerts(Alert, inputstring);

                                    break;
                            }
                        }
                    }
                }
            }

            sw.Close();
        }

        public void ReadNewAlertFile(string zoneName)

        {
            int type = 0;

            // Load the 5.xx version alerts

            StreamReader sw;

            try

            {
                string filterFile;

                string filterDir = Settings.Instance.FilterDir;

                zoneName = zoneName.ToLower();

                filterFile = Path.Combine(filterDir, $"{zoneName}.xml");

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
                    string inputstring = inp;
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
                    else if (inp.StartsWith("<section name=\"primary\">"))
                    {
                        type = 6;
                    }
                    else if (inp.StartsWith("<section name=\"offhand\">"))
                    {
                        type = 7;
                    }
                    else if (type > 0 && type < 8)
                    {
                        // unknown section headers

                        if (inp.StartsWith("</section>"))
                        {
                            type = 0;

                            continue;
                        }

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

                        if (inp.IndexOfAny(new char[] { '[', ':', '^', '*' }) != -1)
                            continue;

                        if (inputstring.IndexOfAny(new char[] { '[', ':', '^', '*' }) != -1)
                            continue;

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
                                AddToAlerts(PrimaryItem, inputstring);
                            }
                            else if (type == 7)
                            {
                                AddToAlerts(OffhandItem, inputstring);
                            }
                        }
                    }
                }
            }

            sw.Close();
        }

        public void WriteAlertFile(string zoneName)

        {
            // Write new xml alert file

            try

            {
                string filterFile;

                string filterDir = Settings.Instance.FilterDir;

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
                    foreach (string str in PrimaryItem)
                    {
                        if (str.Length > 0)
                        {
                            sw.WriteLine("        <oldfilter><regex>Name:" + str + "</regex></oldfilter>");
                        }
                    }
                    sw.WriteLine("    </section>");

                    sw.WriteLine("    <section name=\"Offhand\">");  // Item In Offhand Hand Alerts - zone only
                    foreach (string str in OffhandItem)
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

                filterFile = Path.Combine(filterDir, $"custom_{zoneName}.conf");

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
            catch (Exception ex)

            {
                LogLib.WriteLine($"Error opening file stream for {zoneName}: ", ex);
            }
        }

        public void LoadAlerts(string zoneName)

        {
            if (zoneName.Length > 0)

            {
                // read in old 4.xx alerts

                ReadAlertFile("global", true);

                ReadAlertFile("global", false);

                ReadAlertFile(zoneName, true);

                ReadAlertFile(zoneName, false);

                // read in new 5.xx alerts

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
            string filterDir = Settings.Instance.FilterDir;

            if ((zoneName?.Length) != 0)
            {
                zoneName = zoneName.ToLower();

                string filterFile = Path.Combine(filterDir, $"{zoneName}.xml");

                if (!File.Exists(filterFile))
                    CreateAlertFile(filterFile);

                Process.Start("notepad.exe", filterFile);
            }
        }
    }
}