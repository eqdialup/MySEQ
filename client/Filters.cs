using System;

using SpeechLib;

using System.IO;

using System.Drawing;

using System.Collections;

using System.Diagnostics;

using System.Windows.Forms;

using System.ComponentModel;

using WeifenLuo.WinFormsUI;

using System.Text.RegularExpressions;

using System.Runtime.InteropServices;



// Class Files

using Structures;



namespace myseq 

{   



    public class Filters

    {

        // keep global and zone alerts seperate, so can write them out easier

        public ArrayList hunt = new ArrayList(); // hunts

        public ArrayList globalHunt = new ArrayList(); // global hunts

        public ArrayList caution = new ArrayList(); // caution

        public ArrayList globalCaution = new ArrayList(); // global caution

        public ArrayList danger = new ArrayList();

        public ArrayList globalDanger = new ArrayList();

        public ArrayList alert = new ArrayList(); // rare

        public ArrayList globalAlert = new ArrayList(); // global rare

        public ArrayList emailAlert = new ArrayList(); // email alert - not global

        public ArrayList primaryItem = new ArrayList(); // item in primary hand.

        public ArrayList offhandItem = new ArrayList(); // item in offhand hand.


        public void ClearArrays()

        {

            this.hunt.Clear();

            this.caution.Clear();

            this.danger.Clear();

            this.alert.Clear();

            this.globalHunt.Clear();

            this.globalCaution.Clear();

            this.globalDanger.Clear();

            this.globalAlert.Clear();

            this.emailAlert.Clear();

            this.primaryItem.Clear();

            this.offhandItem.Clear();

        }

        public void AddToAlerts(ArrayList list, string additem)

        {

            // only add to list, if not in list already

            try

            {

                foreach (string item in list)

                    if (string.Compare(item, additem,true) == 0)

                        return;

                list.Add(additem);

            }

            catch (Exception ex)

            {

                LogLib.WriteLine(String.Format("Error Adding Alert for {0}: ", additem), ex);

                return;

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

                

                if (loadCustomAlerts)

                    filterFile = Path.Combine(filterDir, String.Format("custom_{0}.conf", zoneName));

                else

                    filterFile = Path.Combine(filterDir, String.Format("filters_{0}.conf", zoneName));

                

                if ( !File.Exists(filterFile) )

                    return;

                

                // open the existing filter file

                sw = new StreamReader(File.Open(filterFile,FileMode.Open));

            } 

            catch (Exception ex) 

            {

                LogLib.WriteLine(String.Format("Error opening file stream for {0}: ", zoneName), ex);

                return;

            }



            string inp = "";

            while ((inp = sw.ReadLine()) != null) 

            {

                if (inp.Trim().Length > 0) 

                {

                    // all the non commented and blank lines

                    if (!inp.StartsWith("#"))

                    {
                        string inputstring = inp;
                        inp = inp.ToLower();

                        if (inp.StartsWith("[hunt]"))

                            type = 1; 

                        else if (inp.StartsWith("[caution]") || inp.StartsWith("[danger]"))

                            type = 2;                        

                        else if (inp.StartsWith("[rare]") || inp.StartsWith("[locate]") || inp.StartsWith("[alert]"))

                            type = 3;

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

                            if (inp.IndexOfAny(new char[]{'[',':','^','*'}) != -1)

                                continue;

                            if (inputstring.IndexOfAny(new char[] { '[', ':', '^', '*' }) != -1)

                                continue;

                            if (zoneName == "global")

                                switch (type)

                                {

                                    case 1:

                                        AddToAlerts(globalHunt, inputstring);

                                        break;

                                    case 2:

                                        AddToAlerts(globalCaution, inputstring);

                                        break;

                                    case 3:

                                        AddToAlerts(globalAlert, inputstring);

                                        break;

                                }

                            else

                                switch (type)

                                {

                                    case 1:

                                        AddToAlerts(hunt, inputstring);

                                        break;

                                    case 2:

                                        AddToAlerts(caution, inputstring);

                                        break;

                                    case 3:

                                        AddToAlerts(alert, inputstring);

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

            string inp = "";



            // Load the 5.xx version alerts

            StreamReader sw;



            try

            {

                string filterFile;

                string filterDir = Settings.Instance.FilterDir;

                zoneName = zoneName.ToLower();

                filterFile = Path.Combine(filterDir, String.Format("{0}.xml", zoneName));
                

                if (!File.Exists(filterFile))

                {

                    // we did not find the alert file

                    // create an empty alerts file.

                    LogLib.WriteLine("Alert file not found for " + zoneName + ", creating empty one.", LogLevel.Warning);

                    createAlertFile(filterFile);

                    return;

                }

                // open the existing filter file

                sw = new StreamReader(File.Open(filterFile, FileMode.Open));

            }

            catch (Exception ex)

            {

                LogLib.WriteLine(String.Format("Error opening file stream for {0}: ", zoneName), ex);

                return;

            }

            while ((inp = sw.ReadLine()) != null)

            {

                inp = inp.Trim();

                if (inp.Length > 0)

                {

                    // all the non commented and blank lines

                    if (!inp.StartsWith("<!"))

                    {
                        string inputstring = inp;
                        inp = inp.ToLower();

                        if (inp.StartsWith("<section name=\"hunt\">"))

                            type = 1;

                        else if (inp.StartsWith("<section name=\"caution\">"))

                            type = 2;

                        else if (inp.StartsWith("<section name=\"danger\">"))

                            type = 3;

                        else if (inp.StartsWith("<section name=\"alert\">") || inp.StartsWith("<section name=\"locate\">"))

                            type = 4;

                        else if (inp.StartsWith("<section name=\"email\">"))

                            type = 5;

                        else if (inp.StartsWith("<section name=\"primary\">"))

                            type = 6;

                        else if (inp.StartsWith("<section name=\"offhand\">"))

                            type = 7;

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

                                switch (type)
                                {

                                    case 1:

                                        AddToAlerts(globalHunt, inputstring);

                                        break;

                                    case 2:

                                        AddToAlerts(globalCaution, inputstring);

                                        break;

                                    case 3:

                                        AddToAlerts(globalDanger, inputstring);

                                        break;

                                    case 4:

                                        AddToAlerts(globalAlert, inputstring);

                                        break;

                                }

                            }

                            else
                            {

                                switch (type)
                                {

                                    case 1:

                                        AddToAlerts(hunt, inputstring);

                                        break;

                                    case 2:

                                        AddToAlerts(caution, inputstring);

                                        break;

                                    case 3:

                                        AddToAlerts(danger, inputstring);

                                        break;

                                    case 4:

                                        AddToAlerts(alert, inputstring);

                                        break;

                                    case 5:

                                        AddToAlerts(emailAlert, inputstring);

                                        break;

                                    case 6:

                                        AddToAlerts(primaryItem, inputstring);

                                        break;

                                    case 7:

                                        AddToAlerts(offhandItem, inputstring);

                                        break;

                                }

                            }

                        }

                    }

                }

            }

            sw.Close();

        }





        public void writeAlertFile(string zoneName)

        {

            // Write new xml alert file

            try

            {

                string filterFile;

                string filterDir = Settings.Instance.FilterDir;



                zoneName = zoneName.ToLower();



                filterFile = Path.Combine(filterDir, String.Format("{0}.xml", zoneName));



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

                    foreach (string str in globalHunt)

                    {

                        if (str.Length > 0)

                        {

                            sw.WriteLine("        <oldfilter><regex>Name:" + str + "</regex></oldfilter>");

                        }

                    }

                else

                    foreach (string str in hunt)

                    {

                        if (str.Length > 0)

                        {

                            sw.WriteLine("        <oldfilter><regex>Name:" + str + "</regex></oldfilter>");

                        }

                    }

                sw.WriteLine("    </section>");

                sw.WriteLine("    <section name=\"Caution\">");

                if (zoneName == "global")

                    foreach (string str in globalCaution)

                    {

                        if (str.Length > 0)

                        {

                            sw.WriteLine("        <oldfilter><regex>Name:" + str + "</regex></oldfilter>");

                        }

                    }

                else

                    foreach (string str in caution)

                    {

                        if (str.Length > 0)

                        {

                            sw.WriteLine("        <oldfilter><regex>Name:" + str + "</regex></oldfilter>");

                        }

                    }

                sw.WriteLine("    </section>");

                sw.WriteLine("    <section name=\"Danger\">");

                if (zoneName == "global")

                    foreach (string str in globalDanger)

                    {

                        if (str.Length > 0)

                        {

                            sw.WriteLine("        <oldfilter><regex>Name:" + str + "</regex></oldfilter>");

                        }

                    }

                else

                    foreach (string str in danger)

                    {

                        if (str.Length > 0)

                        {

                            sw.WriteLine("        <oldfilter><regex>Name:" + str + "</regex></oldfilter>");

                        }

                    }

                sw.WriteLine("    </section>");

                sw.WriteLine("    <section name=\"Locate\">"); // Not Used in MySEQ

                sw.WriteLine("    </section>");

                sw.WriteLine("    <section name=\"Alert\">");  // Rares

                if (zoneName == "global")

                    foreach (string str in globalAlert)

                    {

                        if (str.Length > 0)

                        {

                            sw.WriteLine("        <oldfilter><regex>Name:" + str + "</regex></oldfilter>");

                        }

                    }

                else

                    foreach (string str in alert)

                    {

                        if (str.Length > 0)

                        {

                            sw.WriteLine("        <oldfilter><regex>Name:" + str + "</regex></oldfilter>");

                        }

                    }

                sw.WriteLine("    </section>");

                sw.WriteLine("    <section name=\"Filtered\">");

                sw.WriteLine("    </section>");

                if (zoneName != "global")
                {
                    sw.WriteLine("    <section name=\"Email\">");  // Email Alerts - zone only
                    foreach (string str in emailAlert)
                    {
                        if (str.Length > 0)
                        {
                            sw.WriteLine("        <oldfilter><regex>Name:" + str + "</regex></oldfilter>");
                        }
                    }
                    sw.WriteLine("    </section>");

                    sw.WriteLine("    <section name=\"Primary\">");  // Item In Primary Hand Alerts - zone only
                    foreach (string str in primaryItem)
                    {
                        if (str.Length > 0)
                        {
                            sw.WriteLine("        <oldfilter><regex>Name:" + str + "</regex></oldfilter>");
                        }
                    }
                    sw.WriteLine("    </section>");

                    sw.WriteLine("    <section name=\"Offhand\">");  // Item In Offhand Hand Alerts - zone only
                    foreach (string str in offhandItem)
                    {
                        if (str.Length > 0)
                        {
                            sw.WriteLine("        <oldfilter><regex>Name:" + str + "</regex></oldfilter>");
                        }
                    }
                    sw.WriteLine("    </section>");
                }
                else
                {

                }

                sw.WriteLine("</seqfilters>");

                sw.Close();



                // Only want one alert file.  Get rid of old format files.

                filterFile = Path.Combine(filterDir, String.Format("custom_{0}.conf", zoneName));

                if (File.Exists(filterFile))

                {

                    File.Delete(filterFile);

                }



                filterFile = Path.Combine(filterDir, String.Format("filters_{0}.conf", zoneName));

                if (File.Exists(filterFile))

                {

                    File.Delete(filterFile);

                }

            }

            catch (Exception ex)

            {

                LogLib.WriteLine(String.Format("Error opening file stream for {0}: ", zoneName), ex);

                return;

            }

        }



        public void loadAlerts(string zoneName) 

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



        private void createAlertFile(string fileName)

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

            if (isglobal == false)
            {
                sw.WriteLine("    <section name=\"Email\">");

                sw.WriteLine("    <section name=\"Primary\">");

                sw.WriteLine("    <section name=\"Offhand\">");

                sw.WriteLine("    </section>");
            }

            sw.WriteLine("</seqfilters>");

            sw.Close();

        }



        public void editAlertFile(string zoneName)

        {

            string filterFile;

            string filterDir = Settings.Instance.FilterDir;



            if (zoneName == string.Empty)

                return;

            

            zoneName = zoneName.ToLower();

                

            filterFile = Path.Combine(filterDir, String.Format("{0}.xml", zoneName));

                

            if ( !File.Exists(filterFile) )

                createAlertFile(filterFile);



            Process.Start("notepad.exe", filterFile);

        }

    }

}