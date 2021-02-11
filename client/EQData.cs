using System;

using System.IO;

using SpeechLib;

using System.Net.Mail;

using System.Data;

using System.Text;

using System.Media;

using System.Drawing;

using System.Threading;

using System.Collections;

using System.Globalization;

using System.Windows.Forms;

using System.ComponentModel;

using System.Drawing.Drawing2D;

using System.Text.RegularExpressions;

using System.Runtime.InteropServices;



using Structures;



namespace myseq
{



    // This is the "model" part - no UI related things in here, only hard EQ data.    

    public class EQData
    {

        [DllImport("winmm.dll")]

        public static extern long PlaySound(string lpszName, long hModule, long dwFlags);



        [DllImport("kernel32.dll", ExactSpelling = true)]

        internal static extern bool Beep(uint freq, uint dur);



        // player details

        public SPAWNINFO playerinfo = new SPAWNINFO();



        // Map details

        public string longname = "";

        public string shortname = "";



        // Map data

        private ArrayList lines = new ArrayList();//MapLine[MAX_LINES];

        private ArrayList texts = new ArrayList();//MapText[50];

        private ArrayList mobtrails = new ArrayList();//MobTrailPoint[1000];

        private ArrayList xlabels = new ArrayList();

        private ArrayList ylabels = new ArrayList();

        private bool playAlerts = false;

        // Max + Min map coordinates - define the bounds of the zone

        public float minx = -1000;

        public float maxx = 1000;

        public float miny = -1000;

        public float maxy = 1000;

        public float minz = -1000;

        public float maxz = 1000;

        // Mobs

        private ArrayList items = new ArrayList();          // Hold the items that are on the ground    

        private Hashtable mobs = new Hashtable();           // Holds the details of the mobs in the current zone.

        public MobsTimers mobsTimers = new MobsTimers();   // Manages the timers

        public int selectedID = 99999;

        public float SpawnX = -1;

        public float SpawnY = -1;

        private int EQSelectedID = 0;

        public DateTime gametime = new DateTime();

        Random rnd = new Random();

        // Mobs / UI Lists        

        public ArrayList newSpawns = new ArrayList();

        public ArrayList newGroundItems = new ArrayList();

        // Items List by ID and Description loaded from file

        public Hashtable itemList = new Hashtable();



        // Mobs / Filters



        // Used to improve packet processing speed

        private bool PrefixStars = true;

        private bool AffixStars = true;

        private bool CorpseAlerts = true;

        private bool MatchFullTextH = false;

        private bool MatchFullTextC = false;

        private bool MatchFullTextD = false;

        private bool MatchFullTextA = false;

        private string HuntPrefix = "";

        private string CautionPrefix = "";

        private string DangerPrefix = "";

        private string AlertPrefix = "";



        public string[] Classes = null;

        public string[] Spawntypes = { "Player", "NPC", "Corpse", "Any", "Pet" };

        public string[] Races = null;

        public string[] VisTypes = { "Visible", "Invisible", "Hidden/Stealth", "Invis to Undead", "Invis to Animals" };

        // Blech, some UI stuff... but at least it's shareable between several users

        public SolidBrush[] conColors = new SolidBrush[1000];

        public int greenRange = 0;

        public int cyanRange = 0;

        public int greyRange = 0;

        public int yellowRange = 3;

        private ColorConverter ColorChart = new ColorConverter();

        private bool zoning = false;

        public bool Zoning { get { return zoning; } set { zoning = value; } }

        // TODO: this is to be replaced by a notification mechanism

        //public bool m_readjustRequired = false;

        private const int ditchGone = 2;



        private static string sndFile;
        private string search0 = "";
        private string search1 = "";
        private string search2 = "";
        private string search3 = "";
        private string search4 = "";
        private string search5 = "";
        private bool filter0 = false;
        private bool filter1 = false;
        private bool filter2 = false;
        private bool filter3 = false;
        private bool filter4 = false;
        private bool filter5 = false;
        private bool levelCheck = false;
        private int searchLevel = 0;



        private static void PlaySnd()
        {

            PlaySound(sndFile, 0, 0);

        }



        public static void Play(string fileName)
        {

            sndFile = fileName;

            System.Threading.ThreadStart entry =

                               new System.Threading.ThreadStart(PlaySnd);

            System.Threading.Thread thrd =

                                    new System.Threading.Thread(entry);

            thrd.Start();

        }

        public void EnablePlayAlerts()
        {

            playAlerts = true;

        }

        public void DisablePlayAlerts()
        {

            playAlerts = false;

        }


        public void MarkLookups(string name, bool filterMob = false)
        {
            if (name.Length > 2 && name.Substring(2) == "Mob_Search") { name = name.Substring(0, 2); }
            if (name.Substring(0, 2) == "0:")
            {
                if (name.Length > 2)
                {
                    search0 = name.Substring(2);
                    filter0 = filterMob;
                }
                else
                { search0 = ""; }
            }
            if (name.Substring(0, 2) == "1:")
            {
                if (name.Length > 2)
                {
                    search1 = name.Substring(2);
                    filter1 = filterMob;
                }
                else
                { search1 = ""; }
            }
            if (name.Substring(0, 2) == "2:")
            {
                if (name.Length > 2)
                {
                    search2 = name.Substring(2);
                    filter2 = filterMob;
                }
                else
                { search2 = ""; }
            }
            if (name.Substring(0, 2) == "3:")
            {
                if (name.Length > 2)
                {
                    search3 = name.Substring(2);
                    filter3 = filterMob;
                }
                else
                { search3 = ""; }
            }
            if (name.Substring(0, 2) == "4:")
            {
                if (name.Length > 2)
                {
                    search4 = name.Substring(2);
                    filter4 = filterMob;
                }
                else
                { search4 = ""; }
            }
            if (name.Substring(0, 2) == "5:")
            {
                if (name.Length > 2)
                {
                    search5 = name.Substring(2);
                    filter5 = filterMob;
                }
                else
                { search5 = ""; }
            }

            foreach (Structures.SPAWNINFO sp in mobs.Values)
            {
                sp.isLookup = false;
                sp.lookupNumber = "";
                if (search0.Length > 0)
                {
                    levelCheck = false;
                    if (search0.Length > 2 && search0.Substring(0,2).ToUpper() == "L:")
                    {
                        int.TryParse(search0.Substring(2), out searchLevel);
                        if (searchLevel != 0 && (sp.Level == searchLevel))
                            {
                            levelCheck = true;
                        }
                        
                    }
                    Regex regEx0 = new Regex(".*" + search0 + ".*", RegexOptions.IgnoreCase);
                    if (levelCheck || (regEx0.Match(sp.Name).Success == true))
                    {
                        sp.isLookup = true;
                        sp.lookupNumber = "1";
                        sp.hidden = false;
                        if (filter0) { sp.hidden = true; }
                    }
                }
                if (search1.Length > 0)
                {
                    levelCheck = false;
                    if (search1.Length > 2 && search1.Substring(0, 2).ToUpper() == "L:")
                    {
                        int.TryParse(search1.Substring(2), out searchLevel);
                        if (searchLevel != 0 && (sp.Level == searchLevel))
                        {
                            levelCheck = true;
                        }

                    }
                    Regex regEx1 = new Regex(".*" + search1 + ".*", RegexOptions.IgnoreCase);
                    if (levelCheck || (regEx1.Match(sp.Name).Success == true))
                    {
                        sp.isLookup = true;
                        sp.lookupNumber = "2";
                        sp.hidden = false;
                        if (filter1) { sp.hidden = true; }
                    }
                }
                if (search2.Length > 0)
                {
                    levelCheck = false;
                    if (search2.Length > 2 && search2.Substring(0, 2).ToUpper() == "L:")
                    {
                        int.TryParse(search2.Substring(2), out searchLevel);
                        if (searchLevel != 0 && (sp.Level == searchLevel))
                        {
                            levelCheck = true;
                        }

                    }
                    Regex regEx2 = new Regex(".*" + search2 + ".*", RegexOptions.IgnoreCase);
                    if (levelCheck || (regEx2.Match(sp.Name).Success == true))
                    {
                        sp.isLookup = true;
                        sp.lookupNumber = "3";
                        sp.hidden = false;
                        if (filter2) { sp.hidden = true; }
                    }
                }
                if (search3.Length > 0)
                {
                    levelCheck = false;
                    if (search3.Length > 2 && search3.Substring(0, 2).ToUpper() == "L:")
                    {
                        int.TryParse(search3.Substring(2), out searchLevel);
                        if (searchLevel != 0 && (sp.Level == searchLevel))
                        {
                            levelCheck = true;
                        }

                    }
                    Regex regEx3 = new Regex(".*" + search3 + ".*", RegexOptions.IgnoreCase);
                    if (levelCheck || (regEx3.Match(sp.Name).Success == true))
                    {
                        sp.isLookup = true;
                        sp.lookupNumber = "4";
                        sp.hidden = false;
                        if (filter3) { sp.hidden = true; }
                    }
                }
                if (search4.Length > 0)
                {
                    levelCheck = false;
                    if (search4.Length > 2 && search4.Substring(0, 2).ToUpper() == "L:")
                    {
                        int.TryParse(search4.Substring(2), out searchLevel);
                        if (searchLevel != 0 && (sp.Level == searchLevel))
                        {
                            levelCheck = true;
                        }

                    }
                    Regex regEx4 = new Regex(".*" + search4 + ".*", RegexOptions.IgnoreCase);
                    if (levelCheck || (regEx4.Match(sp.Name).Success == true))
                    {
                        sp.isLookup = true;
                        sp.lookupNumber = "5";
                        sp.hidden = false;
                        if (filter4) { sp.hidden = true; }
                    }
                }
                if (search5.Length > 0)
                {
                    levelCheck = false;
                    if (search5.Length > 2 && search5.Substring(0, 2).ToUpper() == "L:")
                    {
                        int.TryParse(search5.Substring(2), out searchLevel);
                        if (searchLevel != 0 && (sp.Level == searchLevel))
                        {
                            levelCheck = true;
                        }

                    }
                    Regex regEx5 = new Regex(".*" + search5 + ".*", RegexOptions.IgnoreCase);
                    if (levelCheck || (regEx5.Match(sp.Name).Success == true))
                    {
                        sp.isLookup = true;
                        sp.lookupNumber = "6";
                        sp.hidden = false;
                        if (filter5) { sp.hidden = true; }
                    }
                }
            }

        }



        public void AddMobTrailPoint(MobTrailPoint work)
        {

            if (!mobtrails.Contains(work))

                mobtrails.Add(work);

        }



        // TODO: make sure it is not changed... 

        public Hashtable GetMobsReadonly()
        {

            return mobs;

        }



        // TODO: make sure it is not changed... 

        public ArrayList GetMobTrailsReadonly()
        {

            return mobtrails;

        }



        // TODO: make sure it is not changed... 

        public ArrayList GetLinesReadonly()
        {

            return lines;

        }



        // TODO: make sure it is not changed... 

        public ArrayList GetTextsReadonly()
        {

            return texts;

        }



        // TODO: make sure it is not changed... 

        public ArrayList GetItemsReadonly()
        {

            return items;

        }



        public void PlayAlertSound()
        {

            switch (Settings.Instance.AlertSound)
            {

                case "Asterisk":

                    System.Media.SystemSounds.Asterisk.Play();

                    break;

                case "Beep":

                    System.Media.SystemSounds.Beep.Play();

                    break;

                case "Exclamation":

                    System.Media.SystemSounds.Exclamation.Play();

                    break;

                case "Hand":

                    System.Media.SystemSounds.Hand.Play();

                    break;

                case "Question":

                    System.Media.SystemSounds.Question.Play();

                    break;

                default:

                    break;

            }

        }

        public bool SelectTimer(float delta, float x, float y)
        {



            SPAWNTIMER st = FindTimer(delta, x, y);



            if ((st != null))
            {
                if (Settings.Instance.AutoSelectSpawnList && st.itmSpawnTimerList != null)
                {
                    st.itmSpawnTimerList.EnsureVisible();
                    st.itmSpawnTimerList.Selected = true;
                }
                SPAWNINFO sp = FindMobTimer(st.SpawnLoc);

                if (sp == null)

                    selectedID = 99999;

                else

                    selectedID = (int)sp.SpawnID;



                SpawnX = st.X;

                SpawnY = st.Y;

                //selectedID = 99999;

                return true;

            }

            else
            {

                return false;

            }

        }


        public bool SelectGroundItem(float delta, float x, float y)
        {


            GroundItem gi = FindGroundItem(delta, x, y);

            if ((gi != null))
            {
                if (Settings.Instance.AutoSelectSpawnList)
                {

                    gi.listitem.EnsureVisible();

                    gi.listitem.Selected = true;

                }

                selectedID = 99999;

                SpawnX = gi.X;

                SpawnY = gi.Y;

                return true;

            }

            else
            {

                return false;

            }

        }


        public bool SelectMob(float delta, float x, float y)
        {

            SPAWNINFO sp = FindMobNoPet(delta, x, y);

            if (sp == null)

                sp = FindMob(delta, x, y);



            if (sp != null)
            {

                if (Settings.Instance.AutoSelectSpawnList)
                {

                    sp.listitem.EnsureVisible();

                    sp.listitem.Selected = true;

                }

                selectedID = (int)sp.SpawnID;

                SpawnX = -1.0f;

                SpawnY = -1.0f;

                return true;

            }

            else
            {

                return false;

            }

        }

        public SPAWNINFO FindMobNoPet(float delta, float x, float y)
        {

            try
            {

                foreach (SPAWNINFO sp in mobs.Values)
                {

                    if (!sp.filtered && !sp.hidden && !sp.isFamiliar && !sp.isPet && !sp.isFamiliar && !sp.isMerc)
                    {

                        if (sp.X < x + delta && sp.X > x - delta && sp.Y < y + delta && sp.Y > y - delta)
                        {

                            return sp;

                        }

                    }

                }

                return null;

            }

            catch (Exception ex)
            {

                LogLib.WriteLine("Error in FindMobNoPet(): ", ex);

                return null;

            }

        }

        public SPAWNINFO FindMobNoPetNoPlayerNoCorpse(float delta, float x, float y)
        {

            try
            {

                foreach (SPAWNINFO sp in mobs.Values)
                {

                    if (!sp.hidden && !sp.isFamiliar && !sp.isPet && !sp.isFamiliar && !sp.isMerc && !sp.m_isPlayer && !sp.isCorpse)
                    {

                        if (sp.X < x + delta && sp.X > x - delta && sp.Y < y + delta && sp.Y > y - delta)
                        {

                            return sp;

                        }

                    }

                }

                return null;

            }

            catch (Exception ex)
            {

                LogLib.WriteLine("Error in FindMobNoPetNoPlayerNoCorpse(): ", ex);

                return null;

            }

        }

        public SPAWNINFO FindMobNoPetNoPlayer(float delta, float x, float y)
        {

            try
            {

                foreach (SPAWNINFO sp in mobs.Values)
                {

                    if (!sp.hidden && !sp.isFamiliar && !sp.isPet && !sp.isFamiliar && !sp.isMerc && !sp.isCorpse)
                    {

                        if (sp.X < x + delta && sp.X > x - delta && sp.Y < y + delta && sp.Y > y - delta)
                        {

                            return sp;

                        }

                    }

                }

                return null;

            }

            catch (Exception ex)
            {

                LogLib.WriteLine("Error in FindMobNoPetNoPlayer(): ", ex);

                return null;

            }

        }

        public SPAWNINFO FindMob(float delta, float x, float y)
        {

            try
            {

                foreach (SPAWNINFO sp in mobs.Values)
                {

                    if (!sp.hidden && !sp.filtered)
                    {

                        if (sp.X < x + delta && sp.X > x - delta && sp.Y < y + delta && sp.Y > y - delta)
                        {

                            return sp;

                        }

                    }

                }

                return null;

            }

            catch (Exception ex)
            {

                LogLib.WriteLine("Error in FindMob(): ", ex);

                return null;

            }

        }

        public SPAWNINFO FindMobTimer(string spawnLoc)
        {

            try
            {

                foreach (SPAWNINFO sp in mobs.Values)
                {

                    if ((sp.SpawnLoc == spawnLoc) && (sp.Type == 1))
                    {

                        return sp;

                    }

                }

                return null;

            }

            catch (Exception ex)
            {

                LogLib.WriteLine("Error in FindMobTimer(): ", ex);

                return null;

            }

        }

        public SPAWNTIMER FindListViewTimer(ListViewItem listItem)
        {
            try
            {
                // This returns mobsTimer2
                foreach (SPAWNTIMER st in mobsTimers.GetRespawned().Values)
                {
                    if (st.itmSpawnTimerList == listItem)
                        return st;
                }
                return null;

            }

            catch (Exception ex)
            {

                LogLib.WriteLine("Error in SPAWNTIMER FindTimer(): ", ex);

                return null;

            }

        }


        public SPAWNTIMER FindTimer(float delta, float x, float y)
        {

            try
            {
                // This returns mobsTimer2
                foreach (SPAWNTIMER st in mobsTimers.GetRespawned().Values)
                {

                    if (st.X < x + delta && st.X > x - delta && st.Y < y + delta && st.Y > y - delta)

                        return st;

                }

                return null;

            }

            catch (Exception ex)
            {

                LogLib.WriteLine("Error in SPAWNTIMER FindTimer(): ", ex);

                return null;

            }

        }

        public SPAWNINFO FindSpawnTimer(float delta, float x, float y)
        {

            try
            {

                foreach (SPAWNINFO st in mobsTimers.GetRespawned().Values)
                {

                    if (st.X < x + delta && st.X > x - delta && st.Y < y + delta && st.Y > y - delta)

                        return st;

                }

                return null;

            }

            catch (Exception ex)
            {

                LogLib.WriteLine("Error in SPAWNINFO FindTimer(): ", ex);

                return null;

            }

        }



        public GroundItem FindGroundItem(float delta, float x, float y)
        {

            foreach (GroundItem gi in items)
            {

                if (!gi.filtered && gi.X < x + delta && gi.X > x - delta && gi.Y < y + delta && gi.Y > y - delta)
                {

                    return gi;

                }

            }

            return null;

        }

        public GroundItem FindGroundItemNoFilter(float delta, float x, float y)
        {

            foreach (GroundItem gi in items)
            {

                if (gi.X < x + delta && gi.X > x - delta && gi.Y < y + delta && gi.Y > y - delta)
                {

                    return gi;

                }

            }

            return null;

        }

        public SPAWNINFO GetSelectedMob()
        {

            return (SPAWNINFO)mobs[(uint)selectedID];

        }





        public void InitLookups()
        {

            Classes = GetStrArrayFromTextFile(Path.Combine(Settings.Instance.CfgDir, "Classes.txt"));

            Races = GetStrArrayFromTextFile(Path.Combine(Settings.Instance.CfgDir, "Races.txt"));

            itemList.Clear();

            ReadItemList(Path.Combine(Settings.Instance.CfgDir, "GroundItems.ini"));

            ColorChart.Initialise(Path.Combine(Settings.Instance.CfgDir, "RGB.txt"));

        }



        public bool loadMapInternal(string filename)
        {

            // All Parse Routines MUST be passed this culture info so that they work

            // correctly when used on an operating system configured to use a different culture.

            IFormatProvider NumFormat = new CultureInfo("en-US");

            StreamReader tr;

            string line = "";

            int numtexts = 0;

            int numlines = 0;

            int curLine = 0;

            if (!File.Exists(filename))
            {
                LogLib.WriteLine("File not found loading " + filename + " in loadMap()");

                return false;
            }

            try { tr = new StreamReader(File.OpenRead(filename)); }

            catch (System.IO.FileNotFoundException)
            {

                LogLib.WriteLine("File not found loading " + filename + " in loadMap()");

                return false;

            }



            LogLib.WriteLine("Loading Zone Map (non LoY): " + filename);



            string tok = "";



            // First line is file ID info

            line = tr.ReadLine();

            curLine++;



            if (line == null)

                return false;

            int lineCount = 1;



            if ((longname = getnexttoken(ref line, ',')) == null)

                return false;



            if ((shortname = getnexttoken(ref line, ',')) == null)

                return false;



            // Rest of header is optional.

            try
            {

                // ICK....

                if ((tok = getnexttoken(ref line, ',')) != null)
                {

                    minx = int.Parse(tok, NumFormat);

                    if ((tok = getnexttoken(ref line, ',')) != null)
                    {

                        miny = int.Parse(tok, NumFormat);

                        if ((tok = getnexttoken(ref line, ',')) != null)
                        {

                            minz = int.Parse(tok, NumFormat);

                            if ((tok = getnexttoken(ref line, ',')) != null)
                            {

                                maxx = int.Parse(tok, NumFormat);

                                if ((tok = getnexttoken(ref line, ',')) != null)
                                {

                                    maxy = int.Parse(tok, NumFormat);

                                    if ((tok = getnexttoken(ref line, ',')) != null)

                                        maxz = int.Parse(tok, NumFormat);

                                }

                            }

                        }

                    }

                }

            }

            catch (Exception ex) { LogLib.WriteLine("Error in loadMap() Reading Map Header: ", ex); }



            // Now the bulk of the file....

            // Note A and Z are not implemented as they don't ever seem to be used...



            while ((line = tr.ReadLine()) != null)
            {

                curLine++;



                LogLib.WriteLine("> " + line, LogLevel.DebugHeavy);

                try
                {

                    if (line != "")
                    {

                        if ((tok = getnexttoken(ref line, ',')) == null)
                        {

                            LogLib.WriteLine(string.Format("Warning - Line {0} of map '{1}' has an invalid format and will be ignored.", curLine, filename), LogLevel.Warning);

                        }

                        else
                        {

                            if (tok == "Z") { } // Ignore the ZEM one

                            else if (tok == "M")
                            {

                                MapLine work = new MapLine();

                                int numpoints = 0;



                                bool bOK = true;

                                if (bOK && (tok = getnexttoken(ref line, ',')) != null)

                                    work.name = tok;

                                else

                                    bOK = false;



                                if (bOK && (tok = getnexttoken(ref line, ',')) != null)

                                    work.color = new Pen(new SolidBrush(ColorChart.StringToColor(tok)));

                                else

                                    bOK = false;



                                if ((tok = getnexttoken(ref line, ',')) != null)

                                    numpoints = int.Parse(tok, NumFormat);

                                else

                                    bOK = false;



                                if (bOK)

                                    work.linePoints = new PointF[numpoints];

                                int pointnum = 0;

                                while (bOK && (tok = getnexttoken(ref line, ',')) != null)
                                {

                                    MapPoint temp = new MapPoint();

                                    temp.x = (int)float.Parse(tok, NumFormat);

                                    if ((tok = getnexttoken(ref line, ',')) != null)
                                    {

                                        temp.y = (int)float.Parse(tok, NumFormat);

                                        if ((tok = getnexttoken(ref line, ',')) != null)
                                        {

                                            temp.z = (int)float.Parse(tok, NumFormat) / 10;

                                            work.aPoints.Add(temp);

                                            work.linePoints[pointnum] = new PointF(temp.x, temp.y);

                                            pointnum++;

                                        }

                                    }

                                }

                                if (numpoints != work.aPoints.Count)
                                {

                                    LogLib.WriteLine(string.Format("Warning - Line {0} of map '{1}' has an invalid point count. Expected - {2}, Actual {3}. Continuing anyway...", curLine, filename, numpoints, work.aPoints.Count), LogLevel.Warning);

                                }

                                if (bOK && work.aPoints.Count >= 2)
                                {

                                    lines.Add(work);

                                    numlines++;

                                }

                            }

                            else if (tok == "L")
                            {

                                MapLine work = new MapLine();

                                int numpoints = 0;

                                bool bOK = true;



                                if (bOK && (tok = getnexttoken(ref line, ',')) != null)

                                    work.name = tok;

                                else

                                    bOK = false;



                                if (bOK && (tok = getnexttoken(ref line, ',')) != null)

                                    work.color = new Pen(new SolidBrush(ColorChart.StringToColor(tok)));

                                else

                                    bOK = false;





                                if (bOK && (tok = getnexttoken(ref line, ',')) != null)

                                    numpoints = int.Parse(tok, NumFormat);

                                else

                                    bOK = false;



                                if (bOK)
                                {

                                    int pointnum = 0;

                                    while ((tok = getnexttoken(ref line, ',')) != null)
                                    {

                                        MapPoint temp = new MapPoint();



                                        temp.x = (int)float.Parse(tok, NumFormat);

                                        if ((tok = getnexttoken(ref line, ',')) != null)
                                        {

                                            temp.y = (int)float.Parse(tok, NumFormat);

                                            work.aPoints.Add(temp);

                                            work.linePoints[pointnum] = new PointF(temp.x, temp.y);

                                            pointnum++;

                                        }

                                    }



                                    if (numpoints != work.aPoints.Count)
                                    {

                                        LogLib.WriteLine(string.Format("Warning - Line {0} of map '{1}' has an invalid point count. Expected - {2}, Actual {3}. Continuing anyway...", curLine, filename, numpoints, work.aPoints.Count), LogLevel.Warning);

                                    }

                                    if (work.aPoints.Count >= 2)
                                    {

                                        lines.Add(work);

                                        numlines++;

                                    }

                                }

                                else
                                {

                                    LogLib.WriteLine(string.Format("Warning - Line {0} of map '{1}' has an invlaid format and will be ignored.", curLine, filename), LogLevel.Warning);

                                }

                            }

                            else if (tok == "P")
                            {

                                MapText work = new MapText();

                                bool bOK = true;



                                if (bOK && (tok = getnexttoken(ref line, ',')) != null)

                                    work.text = tok;

                                else

                                    bOK = false;



                                if (bOK && (tok = getnexttoken(ref line, ',')) != null)

                                    work.color = new SolidBrush(ColorChart.StringToColor(tok));

                                else

                                    bOK = false;



                                if (bOK && (tok = getnexttoken(ref line, ',')) != null)

                                    work.x = int.Parse(tok, NumFormat);

                                else

                                    bOK = false;



                                if ((tok = getnexttoken(ref line, ',')) != null)

                                    work.y = int.Parse(tok, NumFormat);

                                else

                                    bOK = false;



                                if (bOK)
                                {
                                    // add a z value if it exists
                                    if ((tok = getnexttoken(ref line, ',')) != null)
                                        work.z = int.Parse(tok, NumFormat);
                                    else
                                        work.z = -99999;

                                    texts.Add(work);

                                    numtexts++;

                                }

                            }

                        }

                    }

                }

                catch (Exception ex) { LogLib.WriteLine(string.Format("Error in loadMap() Line: {0) Reading the rest of Map:", curLine), ex); }

            }



            LogLib.WriteLine(curLine + " lines processed.", LogLevel.Debug);

            LogLib.WriteLine("Loaded " + lines.Count + " lines", LogLevel.Debug);



            tr.Close();



            if (numtexts > 0 || lineCount > 0)
            {

                calcExtents();

                return true;

            }

            else
            {

                longname = "";

                shortname = "";

                return false;

            }

        }

        public void AddMapText(MapText work)
        {
            texts.Add(work);
        }

        public void DeleteMapText(MapText work)
        {
            texts.Remove(work);
        }

        public bool loadLoYMapInternal(string filename)
        {

            IFormatProvider NumFormat = new CultureInfo("en-US");

            StreamReader tr;

            string line = "";

            string tok = "";



            int numtexts = 0;

            int numlines = 0;

            int curLine = 0;

            if (!File.Exists(filename))
            {
                LogLib.WriteLine("File not found loading " + filename + " in loadLoYMap");

                return false;
            }

            try { tr = new StreamReader(File.OpenRead(filename)); }

            catch (System.IO.FileNotFoundException)
            {

                LogLib.WriteLine("File not found loading " + filename + " in loadLoYMap");

                return false;

            }

            LogLib.WriteLine("Loading Zone Map (LoY): " + filename);



            int lineCount = 0;

            while ((line = tr.ReadLine()) != null)
            {

                curLine++;



                try
                {

                    lineCount++;

                    if (line != "")
                    {

                        if ((tok = getnexttoken(ref line, ' ')) == null)
                        {

                            LogLib.WriteLine(string.Format("Warning - Line {0} of map '{1}' has an invalid format and will be ignored.", curLine, filename), LogLevel.Warning);

                        }

                        else
                        {

                            if (tok == "L")
                            {

                                MapLine work = new MapLine();

                                MapPoint point1 = new MapPoint();

                                MapPoint point2 = new MapPoint();



                                bool bOK = true;

                                if (bOK && (tok = getnexttoken(ref line, ',')) != null)

                                    point1.x = -(int)float.Parse(tok, NumFormat);

                                else

                                    bOK = false;





                                if (bOK && (tok = getnexttoken(ref line, ',')) != null)

                                    point1.y = -(int)float.Parse(tok, NumFormat);

                                else

                                    bOK = false;



                                if (bOK && (tok = getnexttoken(ref line, ',')) != null)

                                    point1.z = (int)float.Parse(tok, NumFormat);

                                else

                                    bOK = false;



                                if (bOK && (tok = getnexttoken(ref line, ',')) != null)

                                    point2.x = -(int)float.Parse(tok, NumFormat);

                                else

                                    bOK = false;



                                if (bOK && (tok = getnexttoken(ref line, ',')) != null)

                                    point2.y = -(int)float.Parse(tok, NumFormat);

                                else

                                    bOK = false;



                                if (bOK && (tok = getnexttoken(ref line, ',')) != null)

                                    point2.z = (int)float.Parse(tok, NumFormat);

                                else

                                    bOK = false;



                                int r = 0, g = 0, b = 0;

                                if (bOK && (tok = getnexttoken(ref line, ',')) != null)

                                    r = int.Parse(tok, NumFormat);

                                else

                                    bOK = false;



                                if (bOK && (tok = getnexttoken(ref line, ',')) != null)

                                    g = int.Parse(tok, NumFormat);

                                else

                                    bOK = false;



                                if (bOK && (tok = getnexttoken(ref line, ',')) != null)

                                    b = int.Parse(tok, NumFormat);

                                else

                                    bOK = false;



                                if (bOK)
                                {

                                    work.color = new Pen(new SolidBrush(Color.FromArgb(r, g, b)));

                                    work.aPoints.Add(point1);

                                    work.aPoints.Add(point2);

                                    work.linePoints = new PointF[2];

                                    work.linePoints[0] = new PointF(point1.x, point1.y);

                                    work.linePoints[1] = new PointF(point2.x, point2.y);

                                    lines.Add(work);

                                    numlines++;

                                }

                                else

                                    LogLib.WriteLine(string.Format("Warning - Line {0} of map '{1}' has an invalid format and will be ignored.", curLine, filename), LogLevel.Warning);

                            }

                            else if (tok == "P")
                            {

                                MapText work = new MapText();



                                bool bOK = true;

                                if (bOK && (tok = getnexttoken(ref line, ',')) != null)

                                    work.x = -(int)float.Parse(tok, NumFormat);

                                else

                                    bOK = false;



                                if (bOK && (tok = getnexttoken(ref line, ',')) != null)

                                    work.y = -(int)float.Parse(tok, NumFormat);

                                else

                                    bOK = false;



                                if (bOK && (tok = getnexttoken(ref line, ',')) != null)
                                {

                                    work.z = (int)float.Parse(tok, NumFormat);

                                }

                                else

                                    bOK = false;



                                int r = 0, g = 0, b = 0;

                                if (bOK && (tok = getnexttoken(ref line, ',')) != null)

                                    r = int.Parse(tok, NumFormat);

                                else

                                    bOK = false;



                                if (bOK && (tok = getnexttoken(ref line, ',')) != null)

                                    g = int.Parse(tok, NumFormat);

                                else

                                    bOK = false;



                                if (bOK && (tok = getnexttoken(ref line, ',')) != null)

                                    b = int.Parse(tok, NumFormat);

                                else

                                    bOK = false;



                                work.color = new SolidBrush(Color.FromArgb(r, g, b));



                                if ((tok = getnexttoken(ref line, ',')) != null)
                                {

                                    // This is text size
                                    // 1 - small
                                    // 2 - medium
                                    // 3 - large
                                    int text_size = 2;

                                    text_size = int.Parse(tok, NumFormat);

                                    if (text_size > 0 && text_size <= 3)
                                        work.size = text_size;
                                }

                                else

                                    bOK = false;





                                if (bOK && (tok = getnexttoken(ref line, ',')) != null)

                                    work.text = tok;

                                else

                                    bOK = false;





                                if (bOK)
                                {

                                    texts.Add(work);

                                    numtexts++;

                                }

                                else

                                    LogLib.WriteLine(string.Format("Warning - Line {0} of map '{1}' has an invlaid format and will be ignored.", curLine, filename), LogLevel.Warning);

                            }

                        }

                    }

                }

                catch (Exception ex) { LogLib.WriteLine("Error in loadLoYMap() Line: " + lineCount + " : ", ex); }

            }



            LogLib.WriteLine(curLine + " lines processed.", LogLevel.Debug);

            LogLib.WriteLine("Loaded " + lines.Count + " lines", LogLevel.Debug);



            tr.Close();



            if (numtexts > 0 || lineCount > 0)
            {

                //longname = filename;

                shortname = Path.GetFileNameWithoutExtension(filename);

                if (shortname.IndexOf("_") > 0)

                    shortname = shortname.Substring(0, shortname.Length - 2);

                longname = shortname;

                calcExtents();

                return true;

            }

            else

                return false;

        }



        public string getnexttoken(ref string s, char seperator)
        {

            int c = 0;

            string token = "";



            if (s.Length == 0)
            {

                //LogLib.WriteLine(" # string empty",LogLevel.DebugHeavy);

                return null;

            }



            while (c < s.Length && s[c] != seperator)
            {

                token += s[c];

                c++;

            }

            c++;



            if (c == s.Length + 1)

                s = "";

            else

                s = s.Substring(c, s.Length - c);



            //LogLib.WriteLine(" # > '"+token,LogLevel.DebugHeavy);



            return token;

        }





        private string[] GetStrArrayFromTextFile(string filePath)
        {

            ArrayList al = new ArrayList();

            string line;

            if (File.Exists(filePath))
            {

                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

                StreamReader sr = new StreamReader(fs);
                do
                {

                    line = sr.ReadLine();

                    if (line != null)
                    {

                        line = line.Trim();

                        if (line != "" && line.Substring(0, 1) != "#")

                            al.Add(line);

                    }

                } while (line != null);



                sr.Close();

                fs.Close();
            }

            string[] retArray = (string[])al.ToArray(Type.GetType("System.String"));

            return retArray;

        }

        private void ReadItemList(string filePath)
        {

            string tok = "";

            string line = "";

            IFormatProvider NumFormat = new CultureInfo("en-US");

            if (!File.Exists(filePath))
            {

                // we did not find the GroundItems file

                LogLib.WriteLine("GroundItems.ini file not found", LogLevel.Warning);

                return;

            }



            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

            StreamReader sr = new StreamReader(fs);



            do
            {

                line = sr.ReadLine();

                if (line != null)
                {

                    line = line.Trim();

                    if (line.Length > 0 && (!line.StartsWith("[") && !line.StartsWith("#")))
                    {

                        ListItem thisitem = new ListItem();

                        if ((tok = getnexttoken(ref line, '=')) != null)
                        {

                            thisitem.ActorDef = tok.ToUpper();

                            if ((tok = getnexttoken(ref line, ',')) != null)
                            {

                                thisitem.Name = tok;

                                // Remove the starting IT to get at ID number

                                string temp = thisitem.ActorDef.Remove(0, 2);

                                if ((tok = getnexttoken(ref temp, '_')) != null)
                                {

                                    thisitem.ID = int.Parse(tok, NumFormat);

                                    // We got this far, so we have a valid item to add

                                    if (!itemList.ContainsKey(thisitem.ID))
                                    {

                                        try { itemList.Add(thisitem.ID, thisitem); }

                                        catch (Exception ex) { LogLib.WriteLine("Error adding " + thisitem.ID + " to items hashtable: ", ex); }

                                    }

                                }

                            }

                        }

                    }

                }

            } while (line != null);



            sr.Close();

            fs.Close();

            //string[] retArray = (string[])al.ToArray(Type.GetType("System.String"));

            return;

        }


        public string GetItemDescription(string ActorDef)
        {

            // Get description from list made using GroundItems.txt

            string tok = "";

            // Remove the starting IT to get at ID number

            string temp = ActorDef.Remove(0, 2);

            if ((tok = getnexttoken(ref temp, '_')) != null)
            {

                IFormatProvider NumFormat = new CultureInfo("en-US");

                int lookupid = int.Parse(tok, NumFormat);

                // We got this far, so we have a valid item to add

                if (itemList.ContainsKey(lookupid))
                {

                    ListItem lis = (ListItem)itemList[lookupid];

                    return lis.Name.ToString();
                }
            }
            return ActorDef;
        }


        private string ArrayIndextoStr(string[] source, int index)
        {

            if (index < source.GetLowerBound(0) || index > source.GetUpperBound(0))

                return (String.Format("{0}: Unknown", index));

            else

                return source[index];

        }

        public void ClearMapStructures()
        {

            LogLib.WriteLine("Entering ClearMapStructures", LogLevel.Trace);



            lines.Clear();

            texts.Clear();

            xlabels.Clear();

            ylabels.Clear();

            calcExtents();



            LogLib.WriteLine("Exiting ClearMapStructures", LogLevel.Trace);

        }



        public void calcExtents()
        {

            if (this.longname != "" && lines.Count > 0)
            {



                maxx = minx = ((MapLine)(lines[0])).Point(0).x;

                maxy = miny = ((MapLine)(lines[0])).Point(0).y;

                maxz = minz = ((MapLine)(lines[0])).Point(0).z;



                foreach (MapLine l in lines)
                {

                    foreach (MapPoint p in l.aPoints)
                    {

                        if (p.x > maxx)

                            maxx = p.x;

                        else if (p.x < minx)

                            minx = p.x;



                        if (p.y > maxy)

                            maxy = p.y;

                        else if (p.y < miny)

                            miny = p.y;



                        if (p.z > maxz)

                            maxz = p.z;

                        else if (p.z < minz)

                            minz = p.z;

                    }

                }

            }
            else
            {

                minx = -1000;

                maxx = 1000;

                miny = -1000;

                maxy = 1000;

                minz = -1000;

                maxz = 1000;

            }

        }

        public void checkMobs(ListViewPanel SpawnList, ListViewPanel GroundItemList)
        {

            ArrayList deletedItems = new ArrayList();

            ArrayList delListItems = new ArrayList();



            // Increment the remove timers on all the ground spawns

            foreach (GroundItem sp in items)
            {

                if (sp.gone >= ditchGone) deletedItems.Add(sp);

                else sp.gone++;

            }

            // Remove any that have been marked for deletion
            if (deletedItems.Count > 0)
            {
                if (zoning || deletedItems.Count > 5)
                    GroundItemList.listView.BeginUpdate();

                foreach (GroundItem gi in deletedItems)
                {

                    GroundItemList.listView.Items.Remove(gi.listitem);

                    gi.listitem = null;

                    items.Remove(gi);

                }

                if (zoning || deletedItems.Count > 5)

                    GroundItemList.listView.EndUpdate();

            }

            deletedItems.Clear();

            // Increment the remove timers on all the mobs

            foreach (SPAWNINFO sp in mobs.Values)
            {

                if (sp.delFromList)
                {

                    sp.delFromList = false;

                    delListItems.Add(sp);

                }

                else if (sp.gone >= ditchGone) deletedItems.Add(sp);

                else sp.gone++;

                //if (sp.gone != ditchGone) sp.gone++;

                //else deletedItems.Add(sp);

            }

            // Remove any that have been marked for deletion

            if (deletedItems.Count > 0 || delListItems.Count > 0)
            {
                if (zoning || deletedItems.Count > 5 || delListItems.Count > 5)
                    SpawnList.listView.BeginUpdate();

                foreach (SPAWNINFO sp in deletedItems)
                {

                    SpawnList.listView.Items.Remove(sp.listitem);

                    sp.listitem = null;

                    mobs.Remove(sp.SpawnID);

                }

                foreach (SPAWNINFO sp in delListItems)
                {

                    SpawnList.listView.Items.Remove(sp.listitem);

                }

                if (zoning || deletedItems.Count > 5 || delListItems.Count > 5)
                    SpawnList.listView.EndUpdate();

                delListItems.Clear();

                deletedItems.Clear();

            }

        }



        private void CheckMapMinMax(SPAWNINFO si)
        {

            if (Settings.Instance.AutoExpand)
            {

                if ((minx > si.X) && (si.X > -20000)) minx = si.X;

                if ((maxx < si.X) && (si.X < 20000)) maxx = si.X;

                if ((miny > si.Y) && (si.Y > -20000)) miny = si.Y;

                if ((maxy < si.Y) && (si.Y < 20000)) maxy = si.Y;

            }

        }



        public void ProcessGroundItems(SPAWNINFO si, Filters filters, ListViewPanel GroundItemList)
        {

            LogLib.WriteLine("Entering ProcessGroundItems()", LogLevel.Trace);

            try
            {

                bool found = false;



                foreach (GroundItem gi in items)

                    if (gi.Name == si.Name && gi.X == si.X && gi.Y == si.Y && gi.Z == si.Z)
                    {

                        found = true;

                        gi.gone = 0;

                        break;

                    }



                if (!found)
                {

                    GroundItem gi = new GroundItem();

                    gi.X = si.X;

                    gi.Y = si.Y;

                    gi.Z = si.Z;

                    gi.Name = si.Name;

                    gi.Desc = GetItemDescription(si.Name);

                    string itemname = gi.Desc.ToLower();

                    /* ************************************* *

                    * ************* ALERTS **************** *

                    * ************************************* */

                    bool alert = false;

                    // [hunt]

                    if (filters.hunt.Count > 0)

                        if (FindMatches(filters.hunt, itemname, Settings.Instance.NoneOnHunt,

                            Settings.Instance.TalkOnHunt, "Ground Item",

                            Settings.Instance.PlayOnHunt, Settings.Instance.HuntAudioFile,

                            Settings.Instance.BeepOnHunt, MatchFullTextH))
                        {

                            alert = true;

                            gi.isHunt = true;

                        }

                    if (filters.globalHunt.Count > 0 && !alert)

                        if (FindMatches(filters.globalHunt, itemname, Settings.Instance.NoneOnHunt,

                            Settings.Instance.TalkOnHunt, "Ground Item",

                            Settings.Instance.PlayOnHunt, Settings.Instance.HuntAudioFile,

                            Settings.Instance.BeepOnHunt, MatchFullTextH))
                        {

                            alert = true;

                            gi.isHunt = true;

                        }


                    // [caution]

                    if (filters.caution.Count > 0 && !alert)

                        if (FindMatches(filters.caution, itemname, Settings.Instance.NoneOnCaution,

                            Settings.Instance.TalkOnCaution, "Ground Item",

                            Settings.Instance.PlayOnCaution, Settings.Instance.CautionAudioFile,

                            Settings.Instance.BeepOnCaution, MatchFullTextC))
                        {

                            alert = true;

                            gi.isCaution = true;

                        }

                    if (filters.globalCaution.Count > 0 && !alert)

                        if (FindMatches(filters.globalCaution, itemname, Settings.Instance.NoneOnCaution,

                            Settings.Instance.TalkOnCaution, "Ground Item",

                            Settings.Instance.PlayOnCaution, Settings.Instance.CautionAudioFile,

                            Settings.Instance.BeepOnCaution, MatchFullTextC))
                        {

                            alert = true;

                            gi.isCaution = true;

                        }

                    // [danger]

                    if (filters.danger.Count > 0 && !alert)

                        if (FindMatches(filters.danger, itemname, Settings.Instance.NoneOnDanger,

                            Settings.Instance.TalkOnDanger, "Ground Item",

                            Settings.Instance.PlayOnDanger, Settings.Instance.DangerAudioFile,

                            Settings.Instance.BeepOnDanger, MatchFullTextD))
                        {

                            alert = true;

                            gi.isDanger = true;

                        }

                    if (filters.globalDanger.Count > 0 && !alert)

                        if (FindMatches(filters.globalDanger, itemname, Settings.Instance.NoneOnDanger,

                            Settings.Instance.TalkOnDanger, "Ground Item",

                            Settings.Instance.PlayOnDanger, Settings.Instance.DangerAudioFile,

                            Settings.Instance.BeepOnDanger, MatchFullTextD))
                        {

                            alert = true;

                            gi.isDanger = true;

                        }


                    // [rare]

                    if (filters.alert.Count > 0 && !alert)

                        if (FindMatches(filters.alert, itemname, Settings.Instance.NoneOnAlert,

                            Settings.Instance.TalkOnAlert, "Ground Item",

                            Settings.Instance.PlayOnAlert, Settings.Instance.AlertAudioFile,

                            Settings.Instance.BeepOnAlert, MatchFullTextA))
                        {

                            alert = true;

                            gi.isAlert = true;

                        }

                    if (filters.globalAlert.Count > 0 && !alert)

                        if (FindMatches(filters.globalAlert, itemname, Settings.Instance.NoneOnAlert,

                            Settings.Instance.TalkOnAlert, "Ground Item",

                            Settings.Instance.PlayOnAlert, Settings.Instance.AlertAudioFile,

                            Settings.Instance.BeepOnAlert, MatchFullTextA))
                        {

                            alert = true;

                            gi.isAlert = true;

                        }

                    ListViewItem item1 = new ListViewItem(gi.Desc);

                    item1.SubItems.Add(si.Name);

                    DateTime dt = DateTime.Now;

                    item1.SubItems.Add(dt.ToLongTimeString());

                    item1.SubItems.Add(si.X.ToString("#.000"));

                    item1.SubItems.Add(si.Y.ToString("#.000"));

                    item1.SubItems.Add(si.Z.ToString("#.000"));

                    gi.listitem = item1;

                    items.Add(gi);


                    // Add it to the ground item list
                    newGroundItems.Add(item1);

                }

            }

            catch (Exception ex) { LogLib.WriteLine("Error in ProcessGroundItems(): ", ex); }

            LogLib.WriteLine("Exiting ProcessGroundItems()", LogLevel.Trace);

        }



        public void ProcessTarget(SPAWNINFO si)
        {

            LogLib.WriteLine("Entering ProcessTarget()", LogLevel.Trace);

            try
            {

                if (Settings.Instance.AutoSelectEQTarget)
                {

                    if ((EQSelectedID != (int)si.SpawnID))
                    {

                        EQSelectedID = (int)si.SpawnID;

                        selectedID = (int)si.SpawnID;



                        SpawnX = -1.0f;

                        SpawnY = -1.0f;



                        foreach (SPAWNINFO sp in mobs.Values)
                        {

                            if (sp.SpawnID == EQSelectedID)
                            {

                                if (Settings.Instance.AutoSelectSpawnList)
                                {

                                    sp.listitem.EnsureVisible();

                                    sp.listitem.Selected = true;

                                }

                                break;

                            }

                        }

                    }

                }

            }

            catch (Exception ex) { LogLib.WriteLine("Error in ProcessTarget(): ", ex); }

            LogLib.WriteLine("Exiting ProcessTarget()", LogLevel.Trace);

        }

        public void ProcessWorld(SPAWNINFO si)
        {

            LogLib.WriteLine("Entering ProcessWorld()", LogLevel.Trace);

            try
            {

                int gameDay = (int)si.Level;

                int gameHour = (int)si.Type - 1;

                int gameMin = (int)si.Class;

                int gameMonth = (int)si.Hide;

                int gameYear = (int)si.Race;

                gametime = new DateTime(gameYear, gameMonth, gameDay, gameHour, gameMin, 0);

            }

            catch (Exception ex) { LogLib.WriteLine("Error in ProcessWorld(): ", ex); }

            LogLib.WriteLine("Exiting ProcessWorld()", LogLevel.Trace);

        }

        public void ProcessSpawns(SPAWNINFO si, frmMain f1, ListViewPanel SpawnList, Filters filters, MapPane mapPane, RegexHelper reh, bool update_hidden)
        {

            //LogLib.WriteLine("Entering ProcessSpawns()", LogLevel.Trace);

            CorpseAlerts = Settings.Instance.CorpseAlerts;
            if (si.Name.Contains("a_tainted_egg"))
            {
                si.Class = 1;
            }

            try
            {

                bool listReAdd = false;

                bool found = false;

                SPAWNINFO mob;

                // Converted mob collection to a hashtable so we can do

                // a fast lookup to see if a mob already exists

                if (mobs.ContainsKey(si.SpawnID))
                {

                    found = true;

                    mob = (SPAWNINFO)mobs[si.SpawnID];

                    mob.gone = 0;

                    if (update_hidden)
                        mob.refresh = 100;

                    // keep a reference to the listview item to speed up lookups.

                    ListViewItem li = mob.listitem;

                    // some of these should not change often, so only check every 10 times through
                    if (mob.refresh > 10)
                    {

                        // Update Hidden flags
                        if (update_hidden)
                        {
                            if (mob.isCorpse)
                            {

                                if (mob.m_isPlayer)
                                {

                                    // My Corpse

                                    if (mob.m_isMyCorpse)
                                    {

                                        if (!Settings.Instance.ShowMyCorpse)

                                            si.hidden = true;

                                        else

                                            si.hidden = false;

                                    }

                                    else
                                    {

                                        // Other Players Corpses

                                        if (!Settings.Instance.ShowPCCorpses)

                                            si.hidden = true;

                                        else

                                            si.hidden = false;

                                    }

                                }
                                else
                                {

                                    if (!Settings.Instance.ShowCorpses)

                                        si.hidden = true;

                                    else

                                        si.hidden = false;

                                }

                            }

                            else if (mob.m_isPlayer)
                            {

                                if (!Settings.Instance.ShowPlayers)

                                    si.hidden = true;

                                else

                                    si.hidden = false;

                            }

                            else
                            {

                                // non-corpse, non-player spawn (aka NPC)

                                if (!Settings.Instance.ShowNPCs) // hides all NPCs

                                    si.hidden = true;

                                else
                                {

                                    si.hidden = false;

                                    if (si.isEventController && !Settings.Instance.ShowInvis) // Invis Men

                                        si.hidden = true;

                                    else if (mob.isMount && !Settings.Instance.ShowMounts) // Mounts

                                        si.hidden = true;

                                    else if (mob.isPet && !Settings.Instance.ShowPets) // Pets

                                        si.hidden = true;

                                    else if (mob.isFamiliar && !Settings.Instance.ShowFamiliars) // Familiars

                                        si.hidden = true;

                                }

                            }

                            if (si.hidden && !mob.hidden) mob.delFromList = true;

                            if (!si.hidden && mob.hidden) listReAdd = true;

                            mob.hidden = si.hidden;

                        } // end update_hidden

                        // Update mob types
                        if (mob.Type != si.Type)
                        {
                            mob.Type = si.Type;

                            li.SubItems[8].Text = typeToString(si.Type);

                            if (si.Type == 2 || si.Type == 3)
                            {

                                li.ForeColor = Color.Gray;

                                mob.isCorpse = true;

                                if (!CorpseAlerts)
                                {
                                    mob.isHunt = false;

                                    mob.isCaution = false;

                                    mob.isDanger = false;

                                    mob.isAlert = false;
                                }
                            }

                            else if ((si.Race == 127) && ((si.Name.IndexOf("_") == 0) || (si.Level < 2) || (si.Class == 62))) // Invisible Man Race
                            {

                                li.ForeColor = Color.DarkOrchid;
                                si.isEventController = true;
                            }
                            else if (si.Class == 62)
                            {
                                li.ForeColor = Color.Gray;
                                si.isLDONObject = true;
                            }
                            else
                            {

                                li.ForeColor = conColors[si.Level].Color;

                                if (li.ForeColor == Color.Maroon)

                                    li.ForeColor = Color.Red;

                                else if (SpawnList.listView.BackColor == Color.White)
                                {

                                    // Change the colors to be more visible on white if the background is white

                                    if (li.ForeColor == Color.White)

                                        li.ForeColor = Color.Black;

                                    else if (li.ForeColor == Color.Yellow)

                                        li.ForeColor = Color.Goldenrod;

                                }

                            }

                        }

                        // check if the mob name has changed - eg when a mob dies.

                        if ((si.Name.Length > 0) && (string.Compare(mob.Name, si.Name) != 0))
                        {

                            string newname = FixMobName(si.Name);

                            string oldname = FixMobName(mob.Name);

                            // use replace so that we dont loose the alert prefixes.

                            li.Text = li.Text.Replace(oldname, newname);

                            if (!si.IsPlayer())
                            {
                                if ((si.Type == 2 || si.Type == 3))
                                {

                                    // Corpses - lose alerts on map

                                    si.isCorpse = true;

                                    si.isHunt = false;

                                    si.isCaution = false;

                                    si.isDanger = false;

                                    si.isAlert = false;

                                    // moved the type change before this.  So now only trigger kills
                                    // for name changes of corpses.
                                    mobsTimers.Kill(mob);
                                }

                            }

                            mob.Name = si.Name;

                        }

                        if (mob.Level != si.Level)
                        {

                            mob.Level = si.Level;

                            li.SubItems[1].Text = si.Level.ToString();

                            // update forecolor
                            if (mob.Type == 2 || mob.Type == 3 || mob.isLDONObject)
                            {

                                li.ForeColor = Color.Gray;

                            }

                            else if (mob.isEventController)
                            {

                                li.ForeColor = Color.DarkOrchid;

                            }
                            else
                            {

                                li.ForeColor = conColors[mob.Level].Color;

                                if (li.ForeColor == Color.Maroon)

                                    li.ForeColor = Color.Red;

                                else if (SpawnList.listView.BackColor == Color.White)
                                {

                                    // Change the colors to be more visible on white if the background is white

                                    if (li.ForeColor == Color.White)

                                        li.ForeColor = Color.Black;

                                    else if (li.ForeColor == Color.Yellow)

                                        li.ForeColor = Color.Goldenrod;

                                }

                            }

                        }

                        if (mob.Class != si.Class)
                        {

                            mob.Class = si.Class;

                            li.SubItems[2].Text = classNumToString(si.Class);

                        }

                        if (mob.Primary != si.Primary)
                        {

                            mob.Primary = si.Primary;

                            if (si.Primary > 0)

                                li.SubItems[3].Text = itemNumToString(si.Primary);

                            else

                                li.SubItems[3].Text = "";

                        }

                        if (mob.Offhand != si.Offhand)
                        {

                            mob.Offhand = si.Offhand;

                            if (si.Offhand > 0)

                                li.SubItems[4].Text = itemNumToString(si.Offhand);

                            else

                                li.SubItems[4].Text = "";

                        }

                        if (mob.Race != si.Race)
                        {

                            mob.Race = si.Race;

                            li.SubItems[5].Text = raceNumtoString(si.Race);

                        }

                        if (mob.OwnerID != si.OwnerID)
                        {

                            mob.OwnerID = si.OwnerID;
                            if (mob.OwnerID == 0)
                            {

                                li.SubItems[6].Text = "";
                                mob.isPet = false;


                            }
                            else if (mobs.ContainsKey(mob.OwnerID))
                            {
                                SPAWNINFO owner = (SPAWNINFO)mobs[mob.OwnerID];

                                if (owner.IsPlayer())
                                {
                                    mob.isPet = true;
                                    li.ForeColor = Color.Gray;
                                }
                                else
                                {
                                    mob.isPet = false;
                                }
                                li.SubItems[6].Text = FixMobName(owner.Name);
                            }
                            else
                            {
                                li.SubItems[6].Text = mob.OwnerID.ToString();
                                mob.isPet = false;
                            }
                        }

                        if (mob.Hide != si.Hide)
                        {

                            mob.Hide = si.Hide;

                            li.SubItems[9].Text = hideNumToString(si.Hide);

                        }

                        mob.refresh = 0;

                    } // end refresh > 10

                    mob.refresh++;

                    // Set variables we dont want to trigger list update

                    if ((selectedID != (int)mob.SpawnID))
                    {

                        if (mob.X != si.X)
                        {

                            // ensure that map is big enough to show all spawns.

                            if (mapPane != null && mapPane.scale.Value == 100M)

                                CheckMapMinMax(si);

                            mob.X = si.X;

                            if (mob.Y != si.Y)
                            {

                                mob.Y = si.Y;
                            }

                        }
                        else if (mob.Y != si.Y)
                        {

                            mob.Y = si.Y;

                        }

                        // update these for all but selected mob, so they do not refresh for all mobs

                        mob.Z = si.Z;
                    }

                    mob.Heading = si.Heading;

                    mob.SpeedRun = si.SpeedRun;



                    if (mob.SpeedRun != si.SpeedRun)
                    {

                        mob.SpeedRun = si.SpeedRun;

                        li.SubItems[10].Text = si.SpeedRun.ToString();

                    }



                    if ((mob.X != si.X) || (mob.Y != si.Y) || (mob.Z != si.Z))
                    {
                        // this should be the selected id
                        // ensure that map is big enough to show all spawns.

                        if (mapPane != null && mapPane.scale.Value == 100M)

                            CheckMapMinMax(si);

                        mob.X = si.X;

                        li.SubItems[14].Text = si.Y.ToString();



                        mob.Y = si.Y;

                        li.SubItems[13].Text = si.X.ToString();



                        mob.Z = si.Z;

                        li.SubItems[15].Text = si.Z.ToString();

                        float sd = (float)Math.Sqrt(((si.X - playerinfo.X) * ((si.X - playerinfo.X))) +

                            ((si.Y - playerinfo.Y) * (si.Y - playerinfo.Y)) +

                            ((si.Z - playerinfo.Z) * (si.Z - playerinfo.Z)));


                        if (Settings.Instance.FollowOption == FollowOption.Target)

                            f1.ReAdjust();

                        li.SubItems[16].Text = sd.ToString("#.000");

                    }

                    if (listReAdd) newSpawns.Add(li);

                } // end of if found



                // If it's not already in there, add it

                if (!found && si.Name.Length > 0)
                {

                    // LogLib.WriteLine(String.Format("Spawninfo Name: {0} Type: {1} Class {2} Race {3}",si.Name,si.Type,si.Class,si.Race), LogLevel.Info);
                    bool alert = false;

                    // ensure that map is big enough to show all spawns.

                    if (mapPane != null && mapPane.scale.Value == 100M)

                        CheckMapMinMax(si);

                    // Set mob type info

                    if (si.Type == 0)
                    {

                        // Players

                        si.m_isPlayer = true;

                        if (!Settings.Instance.ShowPlayers) si.hidden = true;

                    }

                    else
                    {

                        if (si.Type == 2 || si.Type == 3)
                        {

                            // Corpses

                            si.isCorpse = true;

                            if (!CorpseAlerts)
                            {

                                si.isHunt = false;

                                si.isCaution = false;

                                si.isDanger = false;

                                si.isAlert = false;
                            }

                            if ((si.Name.IndexOf("_") == -1) && (si.Name.IndexOf("a ") != 0) && (si.Name.IndexOf("an ") != 0))
                            {
                                si.m_isPlayer = true;

                                if (!Settings.Instance.ShowPCCorpses) si.hidden = true;

                                if (si.Name.Length > 0 && CheckMyCorpse(si.Name))
                                {

                                    si.m_isMyCorpse = true;

                                    if (!Settings.Instance.ShowMyCorpse)

                                        si.hidden = true;

                                    else

                                        si.hidden = false;

                                }

                            }

                            else
                            {

                                if (!Settings.Instance.ShowCorpses) si.hidden = true;

                            }

                        }

                        else
                        {

                            // non-corpse, non-player spawn (aka NPC)

                            if (!Settings.Instance.ShowNPCs) si.hidden = true;

                            if (si.OwnerID > 0)
                            {

                                SPAWNINFO owner;

                                if (mobs.ContainsKey(si.OwnerID))
                                {
                                    owner = (SPAWNINFO)mobs[si.OwnerID];
                                    if (owner.IsPlayer())
                                    {
                                        si.isPet = true;
                                        if (!Settings.Instance.ShowPets) si.hidden = true;
                                    }
                                }
                                else
                                {
                                    // we didnt find owner, so set to 0
                                    // so we can check again next update
                                    si.OwnerID = 0;
                                }
                            }


                            if ((si.Race == 127) && ((si.Name.IndexOf("_") == 0) || (si.Level < 2) || (si.Class == 62))) // Invisible Man Race
                            {
                                si.isEventController = true;
                                if (!Settings.Instance.ShowInvis)
                                    si.hidden = true;
                            }
                            else if (si.Class == 62)
                            {
                                si.isLDONObject = true;
                            }

                            // Mercenary Identification - Only do it once now

                            if ((si.Lastname != null) && (si.Lastname.Length > 0))
                            {

                                if (reh.IsMerc(si.Lastname))

                                    si.isMerc = true;

                            }

                            else if (reh.IsMount(si.Name)) // Mounts
                            {

                                si.isMount = true;

                                if (!Settings.Instance.ShowMounts) si.hidden = true;

                            }

                            else if (reh.IsFamiliar(si.Name))
                            {
                                // reset these, if match a familiar
                                si.isPet = false;
                                si.hidden = false;

                                si.isFamiliar = true;

                                if (!Settings.Instance.ShowFamiliars) si.hidden = true;

                            }

                        }

                    }

                    mobsTimers.Spawn(si);

                    if (si.Name.Length > 0)
                    {

                        string mobname;

                        string matchmobname;


                        if (si.isMerc)

                            mobname = FixMobNameToo(si.Name);

                        else

                            mobname = FixMobName(si.Name);

                        matchmobname = FixMobNameMatch(mobname);

                        if (matchmobname.Length < 2)
                            matchmobname = mobname;

                        string mobnameWithInfo = mobname;

                        string primaryName = "";

                        if (si.Primary > 0)

                            primaryName = itemNumToString(si.Primary);

                        string offhandName = "";

                        if (si.Offhand > 0)

                            offhandName = itemNumToString(si.Offhand);

                        string loc = String.Format("{0:f3},{1:f3},{2:f3}", si.Y, si.X, si.Z);


                        // Don't do alert matches for controllers, Ldon objects, pets, mercs, mounts, or familiars
                        if (!si.isLDONObject && !si.isEventController && !si.isFamiliar && !si.isMount && !si.isMerc && si.OwnerID == 0)
                        {
                            /* ************************************* *

                            * ************* ALERTS **************** *

                            * ************************************* */

                            // [hunt]

                            if (filters.hunt.Count > 0 && (!si.isCorpse || CorpseAlerts))

                                if (FindMatches(filters.hunt, matchmobname, Settings.Instance.NoneOnHunt,

                                    Settings.Instance.TalkOnHunt, "Hunt Mob",

                                    Settings.Instance.PlayOnHunt, Settings.Instance.HuntAudioFile,

                                    Settings.Instance.BeepOnHunt, MatchFullTextH))
                                {

                                    alert = true;

                                    si.isHunt = true;

                                    if (PrefixStars)

                                        mobnameWithInfo = HuntPrefix + " " + mobnameWithInfo;

                                    if (AffixStars)

                                        mobnameWithInfo += " " + HuntPrefix;

                                }

                            if (filters.globalHunt.Count > 0 && !alert && (!si.isCorpse || CorpseAlerts))

                                if (FindMatches(filters.globalHunt, matchmobname, Settings.Instance.NoneOnHunt,

                                    Settings.Instance.TalkOnHunt, "Hunt Mob",

                                    Settings.Instance.PlayOnHunt, Settings.Instance.HuntAudioFile,

                                    Settings.Instance.BeepOnHunt, MatchFullTextH))
                                {

                                    alert = true;

                                    si.isHunt = true;

                                    if (PrefixStars)

                                        mobnameWithInfo = HuntPrefix + " " + mobnameWithInfo;

                                    if (AffixStars)

                                        mobnameWithInfo += " " + HuntPrefix;

                                }



                            // [caution]

                            if (filters.caution.Count > 0 && !alert && (!si.isCorpse || CorpseAlerts))

                                if (FindMatches(filters.caution, matchmobname, Settings.Instance.NoneOnCaution,

                                    Settings.Instance.TalkOnCaution, "Caution Mob",

                                    Settings.Instance.PlayOnCaution, Settings.Instance.CautionAudioFile,

                                    Settings.Instance.BeepOnCaution, MatchFullTextC))
                                {

                                    alert = true;

                                    si.isCaution = true;

                                    if (PrefixStars)

                                        mobnameWithInfo = CautionPrefix + " " + mobnameWithInfo;

                                    if (AffixStars)

                                        mobnameWithInfo += " " + CautionPrefix;

                                }

                            if (filters.globalCaution.Count > 0 && !alert && (!si.isCorpse || CorpseAlerts))

                                if (FindMatches(filters.globalCaution, matchmobname, Settings.Instance.NoneOnCaution,

                                    Settings.Instance.TalkOnCaution, "Caution Mob",

                                    Settings.Instance.PlayOnCaution, Settings.Instance.CautionAudioFile,

                                    Settings.Instance.BeepOnCaution, MatchFullTextC))
                                {

                                    alert = true;

                                    si.isCaution = true;

                                    if (PrefixStars)

                                        mobnameWithInfo = CautionPrefix + " " + mobnameWithInfo;

                                    if (AffixStars)

                                        mobnameWithInfo += " " + CautionPrefix;

                                }

                            // [danger]

                            if (filters.danger.Count > 0 && !alert && (!si.isCorpse || CorpseAlerts))

                                if (FindMatches(filters.danger, matchmobname, Settings.Instance.NoneOnDanger,

                                    Settings.Instance.TalkOnDanger, "Danger Mob",

                                    Settings.Instance.PlayOnDanger, Settings.Instance.DangerAudioFile,

                                    Settings.Instance.BeepOnDanger, MatchFullTextD))
                                {

                                    alert = true;

                                    si.isDanger = true;

                                    if (PrefixStars)

                                        mobnameWithInfo = DangerPrefix + " " + mobnameWithInfo;

                                    if (AffixStars)

                                        mobnameWithInfo += " " + DangerPrefix;

                                }

                            if (filters.globalDanger.Count > 0 && !alert && (!si.isCorpse || CorpseAlerts))

                                if (FindMatches(filters.globalDanger, matchmobname, Settings.Instance.NoneOnDanger,

                                    Settings.Instance.TalkOnDanger, "Danger Mob",

                                    Settings.Instance.PlayOnDanger, Settings.Instance.DangerAudioFile,

                                    Settings.Instance.BeepOnDanger, MatchFullTextD))
                                {

                                    alert = true;

                                    si.isDanger = true;

                                    if (PrefixStars)

                                        mobnameWithInfo = DangerPrefix + " " + mobnameWithInfo;

                                    if (AffixStars)

                                        mobnameWithInfo += " " + DangerPrefix;

                                }



                            // [rare]

                            if (filters.alert.Count > 0 && !alert && (!si.isCorpse || CorpseAlerts))

                                if (FindMatches(filters.alert, matchmobname, Settings.Instance.NoneOnAlert,

                                    Settings.Instance.TalkOnAlert, "Rare Mob",

                                    Settings.Instance.PlayOnAlert, Settings.Instance.AlertAudioFile,

                                    Settings.Instance.BeepOnAlert, MatchFullTextA))
                                {

                                    alert = true;

                                    si.isAlert = true;

                                    if (PrefixStars)

                                        mobnameWithInfo = AlertPrefix + " " + mobnameWithInfo;

                                    if (AffixStars)

                                        mobnameWithInfo += " " + AlertPrefix;

                                }

                            if (filters.globalAlert.Count > 0 && !alert && (!si.isCorpse || CorpseAlerts))

                                if (FindMatches(filters.globalAlert, matchmobname, Settings.Instance.NoneOnAlert,

                                    Settings.Instance.TalkOnAlert, "Rare Mob",

                                    Settings.Instance.PlayOnAlert, Settings.Instance.AlertAudioFile,

                                    Settings.Instance.BeepOnAlert, MatchFullTextA))
                                {

                                    alert = true;

                                    si.isAlert = true;

                                    if (PrefixStars)

                                        mobnameWithInfo = AlertPrefix + " " + mobnameWithInfo;

                                    if (AffixStars)

                                        mobnameWithInfo += " " + AlertPrefix;

                                }
                            // [Email]

                            if (filters.emailAlert.Count > 0 && !si.isCorpse)
                            {
                                // email alerts will always match the full text,
                                // never match corpses
                                // will send a beep
                                if (FindMatches(filters.emailAlert, matchmobname, false, false, "", false, "", !si.isAlert && !si.isCaution && !si.isDanger && !si.isHunt, true, Settings.Instance.EmailAlerts))
                                {
                                    alert = true;
                                    // Flag on map as an alert mob
                                    si.isAlert = true;
                                }
                            }

                            // [Primary]
                            // Acts like a hunt mob.

                            if (filters.primaryItem.Count > 0 && (!si.isCorpse || CorpseAlerts))

                                if (FindMatches(filters.primaryItem, primaryName, Settings.Instance.NoneOnHunt,

                                    Settings.Instance.TalkOnHunt, "Hunt Mob Primary",

                                    Settings.Instance.PlayOnHunt, Settings.Instance.HuntAudioFile,

                                    Settings.Instance.BeepOnHunt, MatchFullTextH))
                                {

                                    alert = true;

                                    si.isHunt = true;

                                    if (PrefixStars)

                                        mobnameWithInfo = HuntPrefix + " " + mobnameWithInfo;

                                    if (AffixStars)

                                        mobnameWithInfo += " " + HuntPrefix;

                                }

                            // [Offhand]
                            // Acts like a hunt mob.

                            if (filters.offhandItem.Count > 0 && (!si.isCorpse || CorpseAlerts))

                                if (FindMatches(filters.offhandItem, offhandName, Settings.Instance.NoneOnHunt,

                                    Settings.Instance.TalkOnHunt, "Hunt Mob",

                                    Settings.Instance.PlayOnHunt, Settings.Instance.HuntAudioFile,

                                    Settings.Instance.BeepOnHunt, MatchFullTextH))
                                {

                                    alert = true;

                                    si.isHunt = true;

                                    if (PrefixStars)

                                        mobnameWithInfo = HuntPrefix + " " + mobnameWithInfo;

                                    if (AffixStars)

                                        mobnameWithInfo += " " + HuntPrefix;

                                }


                            si.isLookup = false;

                                if (f1.toolStripLookupBox.Text.Length > 0 && f1.toolStripLookupBox.Text != "Mob Search")
                            {
                                string matchstring = f1.toolStripLookupBox.Text.ToString();
                                Regex regEx = new Regex(".*" + matchstring + ".*", RegexOptions.IgnoreCase);

                                if (regEx.Match(matchmobname).Success) si.isLookup = true;

                            }

                            if (f1.toolStripLookupBox1.Text.Length > 0 && f1.toolStripLookupBox1.Text != "Mob Search")
                            {
                                string matchstring = f1.toolStripLookupBox1.Text.ToString();
                                Regex regEx1 = new Regex(".*" + matchstring + ".*", RegexOptions.IgnoreCase);

                                if (regEx1.Match(matchmobname).Success) si.isLookup = true;

                            }

                            if (f1.toolStripLookupBox2.Text.Length > 0 && f1.toolStripLookupBox2.Text != "Mob Search")
                            {
                                string matchstring = f1.toolStripLookupBox2.Text.ToString();
                                Regex regEx2 = new Regex(".*" + matchstring + ".*", RegexOptions.IgnoreCase);

                                if (regEx2.Match(matchmobname).Success) si.isLookup = true;

                            }

                            if (f1.toolStripLookupBox3.Text.Length > 0 && f1.toolStripLookupBox3.Text != "Mob Search")
                            {
                                string matchstring = f1.toolStripLookupBox3.Text.ToString();
                                Regex regEx3 = new Regex(".*" + matchstring + ".*", RegexOptions.IgnoreCase);

                                if (regEx3.Match(matchmobname).Success) si.isLookup = true;

                            }

                            if (f1.toolStripLookupBox4.Text.Length > 0 && f1.toolStripLookupBox4.Text != "Mob Search")
                            {
                                string matchstring = f1.toolStripLookupBox.Text.ToString();
                                Regex regEx4 = new Regex(".*" + matchstring + ".*", RegexOptions.IgnoreCase);

                                if (regEx4.Match(matchmobname).Success) si.isLookup = true;

                            }

                            if(f1.toolStripLookupBox5.Text.Length > 0 && f1.toolStripLookupBox5.Text != "Mob Search")
                            {
                                string matchstring = f1.toolStripLookupBox5.Text.ToString();
                                Regex regEx5 = new Regex(".*" + matchstring + ".*", RegexOptions.IgnoreCase);

                                if (regEx5.Match(matchmobname).Success) si.isLookup = true;

                            }
                        }


                        ListViewItem item1 = new ListViewItem(mobnameWithInfo);

                        item1.SubItems.Add(si.Level.ToString());

                        item1.SubItems.Add(classNumToString(si.Class));

                        if (si.Primary > 0)

                            item1.SubItems.Add(itemNumToString(si.Primary));

                        else

                            item1.SubItems.Add("");

                        if (si.Offhand > 0)

                            item1.SubItems.Add(itemNumToString(si.Offhand));

                        else

                            item1.SubItems.Add("");

                        item1.SubItems.Add(raceNumtoString(si.Race));

                        if (si.OwnerID > 0)
                        {

                            if (mobs.ContainsKey(si.OwnerID))
                            {
                                SPAWNINFO owner = (SPAWNINFO)mobs[si.OwnerID];
                                item1.SubItems.Add(FixMobName(owner.Name));
                            }
                            else
                            {
                                item1.SubItems.Add(si.OwnerID.ToString());
                            }
                        }
                        else
                        {
                            item1.SubItems.Add("");
                        }

                        item1.SubItems.Add(si.Lastname);

                        item1.SubItems.Add(typeToString(si.Type));

                        item1.SubItems.Add(hideNumToString(si.Hide));

                        item1.SubItems.Add(si.SpeedRun.ToString());

                        item1.SubItems.Add(si.SpawnID.ToString());

                        item1.SubItems.Add(DateTime.Now.ToLongTimeString());

                        item1.SubItems.Add(si.X.ToString("#.000"));

                        item1.SubItems.Add(si.Y.ToString("#.000"));

                        item1.SubItems.Add(si.Z.ToString("#.000"));



                        float sd = (float)Math.Sqrt((si.X - playerinfo.X) * (si.X - playerinfo.X) +

                            (si.Y - playerinfo.Y) * (si.Y - playerinfo.Y) +

                            (si.Z - playerinfo.Z) * (si.Z - playerinfo.Z));



                        item1.SubItems.Add(sd.ToString("#.000"));

                        item1.SubItems.Add(FixMobName(si.Name));


                        if (si.Type == 2 || si.Type == 3 || si.isLDONObject)

                            item1.ForeColor = Color.Gray;

                        else if (si.isEventController)

                            item1.ForeColor = Color.DarkOrchid;

                        else
                        {

                            item1.ForeColor = conColors[si.Level].Color;

                            if (item1.ForeColor == Color.Maroon)

                                item1.ForeColor = Color.Red;

                            // Change the colors to be more visible on white if the background is white

                            if (SpawnList.listView.BackColor == Color.White)
                            {

                                if (item1.ForeColor == Color.White)

                                    item1.ForeColor = Color.Black;

                                else if (item1.ForeColor == Color.Yellow)

                                    item1.ForeColor = Color.Goldenrod;

                            }

                        }



                        if (alert)

                            item1.Font = new Font(Settings.Instance.ListFontName, Settings.Instance.ListFontSize, FontStyle.Bold);



                        //si.lvi = item1;

                        si.gone = 0;


                        si.refresh = rnd.Next(0, 10);

                        si.listitem = item1;

                        try { mobs.Add(si.SpawnID, si); }

                        catch (Exception ex) { LogLib.WriteLine("Error adding " + si.Name + " to mobs hashtable: ", ex); }



                        // Add it to the spawn list if it's not supposed to be hidden

                        if (!si.hidden) newSpawns.Add(item1);

                    }

                }

            }

            catch (Exception ex) { LogLib.WriteLine("Error in ProcessSpawns(): ", ex); }

            //LogLib.WriteLine("Exiting ProcessSpawns()", LogLevel.Trace);        

        }

        public void UpdateMobListColors()
        {

            if (mobs != null)
            {

                foreach (SPAWNINFO si in mobs.Values)
                {

                    ListViewItem li = si.listitem;

                    if (li != null)
                    {

                        if (si.Type == 2 || si.Type == 3 || si.isLDONObject)

                            li.ForeColor = Color.Gray;

                        else if (si.isEventController)

                            li.ForeColor = Color.DarkOrchid;

                        else
                        {

                            li.ForeColor = conColors[si.Level].Color;

                            if (li.ForeColor == Color.Maroon)

                                li.ForeColor = Color.Red;

                            // Change the colors to be more visible on white if the background is white

                            if (Settings.Instance.ListBackColor == Color.White)
                            {

                                if (li.ForeColor == Color.White)

                                    li.ForeColor = Color.Black;

                                else if (li.ForeColor == Color.Yellow)

                                    li.ForeColor = Color.Goldenrod;

                            }

                            if (Settings.Instance.ListBackColor == Color.Black)
                            {

                                if (li.ForeColor == Color.Black)

                                    li.ForeColor = Color.White;

                            }

                        }

                    }

                }

            }

        }

        private bool FindMatches(ArrayList exps, string mobname, bool NoneOnMatch,

             bool TalkOnMatch, string TalkDescr, bool PlayOnMatch, String AudioFile,

             bool BeepOnMatch, bool MatchFullText, bool EmailOnMatch = false)
        {

            string t = mobname;

            bool alert = false;



            foreach (string str in exps)
            {

                bool matched = false;

                // if "match full text" is ON...

                if (MatchFullText)
                {
                    if (string.Compare(mobname, str, true) == 0)
                        matched = true;
                }

                else if (isSubstring(mobname, str))
                {

                    matched = true;

                }
                // if item has been matched...

                if (matched)
                {

                    if (!NoneOnMatch)
                    {
                        if (playAlerts)
                        {
                            if (TalkOnMatch)
                            {

                                Talker T = new Talker();

                                T.speakText = TalkDescr + ", " + FixMobNameToSpeak(t) + ", is up.";

                                ThreadStart threadDelegate = new ThreadStart(T.SpeakText);

                                Thread newThread = new Thread(threadDelegate);

                                newThread.Start();

                            }

                            else if (PlayOnMatch)
                            {

                                String tempstr = AudioFile.Replace("\\", "\\\\");

                                Play(tempstr);

                            }

                            else if (BeepOnMatch)
                            {

                                Beep(300, 100);

                            }

                            if (EmailOnMatch)
                            {
                                // this is where we do an email alert
                                MailMessage mailMessage = new MailMessage();
                                mailMessage.From = new MailAddress(SMTPSettings.Instance.FromEmail);

                                // To Email Address could contain multiple addresses
                                string presplit = SMTPSettings.Instance.ToEmail.ToString();
                                string delim = ",;";
                                char[] delimarray = delim.ToCharArray();
                                string[] split = null;
                                split = presplit.Split(delimarray);
                                foreach (string s in split)
                                {
                                    if (s.Trim().Length > 0)
                                        mailMessage.To.Add(new MailAddress(s.Trim()));
                                }

                                // CC email addresses
                                split = null;
                                presplit = SMTPSettings.Instance.CCEmail.ToString();
                                split = presplit.Split(delimarray);
                                foreach (string s in split)
                                {
                                    if (s.Trim().Length > 0)
                                        mailMessage.CC.Add(new MailAddress(s.Trim()));
                                }

                                mailMessage.Subject = "MySEQ Spawn Alert";
                                mailMessage.Body = FixMobNameToSpeak(t) + " Spawned at " + DateTime.Now.ToShortTimeString() + " " + DateTime.Now.ToShortDateString();
                                //MessageBox.Show(mailMessage.Subject + "\r\n" + mailMessage.Body);
                                SmtpClient smtpClient = new SmtpClient(SMTPSettings.Instance.SmtpServer, SMTPSettings.Instance.SmtpPort);
                                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                                if (SMTPSettings.Instance.UseNetworkCredentials)
                                {
                                    smtpClient.UseDefaultCredentials = true;
                                }
                                else
                                {
                                    smtpClient.UseDefaultCredentials = false;

                                    if (SMTPSettings.Instance.SmtpDomain.ToString() != string.Empty)
                                        smtpClient.Credentials = new System.Net.NetworkCredential(SMTPSettings.Instance.SmtpUsername, SMTPSettings.Instance.SmtpPassword.ToString(), SMTPSettings.Instance.SmtpDomain.ToString());
                                    else
                                        smtpClient.Credentials = new System.Net.NetworkCredential(SMTPSettings.Instance.SmtpUsername.ToString(), SMTPSettings.Instance.SmtpPassword.ToString());

                                }
                                // using SSL to authenticate?
                                if (SMTPSettings.Instance.UseSSL)
                                    smtpClient.EnableSsl = true;
                                else
                                    smtpClient.EnableSsl = false;
                                smtpClient.SendCompleted += new SendCompletedEventHandler(SmtpClient_OnCompleted);

                                object UserState = mailMessage;
                                // Send away ....
                                try
                                {
                                    smtpClient.SendAsync(mailMessage, UserState);
                                }
                                catch (Exception Ex)
                                {
                                    LogLib.WriteLine("Send Mail Error: " + Ex.Message, LogLevel.Error);
                                }
                            }
                        }



                    }

                    alert = true;

                    break;

                }

            }

            return alert;

        }

        private void SmtpClient_OnCompleted(object sender, AsyncCompletedEventArgs e)
        {
            //Get the Original MailMessage object
            MailMessage mail = (MailMessage)e.UserState;

            //write out the subject
            string subject = mail.Subject;

            if (e.Cancelled)
            {
                LogLib.WriteLine("SendAsync() Canceled [" + subject + "]", LogLevel.Error);
            }
            if (e.Error != null)
            {
                LogLib.WriteLine("SendAsync() Error [" + subject + "] " + e.Error.ToString(), LogLevel.Error);
            }
            else
            {
                LogLib.WriteLine("SendAsync() Completed [" + subject + "]", LogLevel.Info);
            }
        }

        public bool CheckMyCorpse(string mobname)
        {

            if ((mobname.Length < (playerinfo.Name.Length + 14)) && (mobname.IndexOf(playerinfo.Name) == 0))

                return true;

            else

                return false;

        }



        public void SaveMobs()
        {



            DateTime dt = DateTime.Now;



            string filename = shortname + " - ";

            filename += dt.Month.ToString() + "-" + dt.Day + "-" + dt.Year + " " + dt.Hour + "-" + dt.Minute + ".txt";



            StreamWriter sw = new StreamWriter(filename, false);



            sw.Write("Name\tLevel\tClass\tRace\tLastname\tType\tInvis\tRun Speed\tSpawnID\tX\tY\tZ\tHeading");



            foreach (SPAWNINFO si in mobs.Values)
            {

                sw.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}", si.Name, si.Level, classNumToString(si.Class),

                    raceNumtoString(si.Race), si.Lastname, typeToString(si.Type), hideNumToString(si.Hide), si.SpeedRun, si.SpawnID, si.Y, si.X, si.Z, CalcRealHeading(si));

            }



            sw.Close();

        }



        public void SetSelectedID(int id)
        {

            selectedID = id;

            SpawnX = -1.0f;

            SpawnY = -1.0f;



            // TODO: notify/invalidate listeners

        }

        public void SetSelectedTimer(float x, float y)
        {

            SPAWNTIMER st = FindTimer(1.0f, x, y);



            if ((st != null))
            {

                SPAWNINFO sp = FindMobTimer(st.SpawnLoc);

                if (sp == null)

                    selectedID = 99999;

                else

                    selectedID = (int)sp.SpawnID;



                SpawnX = st.X;

                SpawnY = st.Y;

            }

        }

        public void SetSelectedGroundItem(float x, float y)
        {

            GroundItem gi = FindGroundItemNoFilter(1.0f, x, y);

            if ((gi != null))
            {

                selectedID = 99999;

                SpawnX = gi.X;

                SpawnY = gi.Y;

            }

        }

        public static string FixMobName(string name)
        {

            // if first char is "_" dont replace

            if (name.IndexOf("_") == 0)

                return name;

            else

                return name.Replace("_", " ");

        }

        public string FixMobNameToo(string name)
        {

            return Regex.Replace(name, "^*[^a-zA-Z ]", "");

        }

        public string FixMobNameMatch(string name)
        {

            return Regex.Replace(name, "^*[^a-zA-Z #]", "");

        }

        public string FixMobNameToSpeak(string name)
        {

            return Regex.Replace(name, "^*[^a-zA-Z ]", "");

        }



        public string classNumToString(int num)
        {

            return ArrayIndextoStr(Classes, num);

        }



        public string itemNumToString(int num)
        {

            if (itemList.ContainsKey(num))
            {

                ListItem lis = (ListItem)itemList[num];

                return lis.Name.ToString();

            }

            else
            {

                return num.ToString();

            }

        }



        public string raceNumtoString(int num)
        {
            if (num == 2250)
                return "Interactive Object";

            return ArrayIndextoStr(Races, (int)num);
        }


        public string typeToString(byte num)
        {

            return ArrayIndextoStr(Spawntypes, (int)num);

        }


        public string hideNumToString(byte num)
        {

            return ArrayIndextoStr(VisTypes, (int)num);

        }

        public void BeginProcessPacket()
        {

            newSpawns.Clear();

            newGroundItems.Clear();

        }

        public void ProcessSpawnList(ListViewPanel SpawnList)
        {

            LogLib.WriteLine("Entering ProcessSpawnList()", LogLevel.Trace);

            try
            {

                if (newSpawns.Count > 0)
                {
                    if (zoning)
                        SpawnList.listView.BeginUpdate();
                    ListViewItem[] items = new ListViewItem[newSpawns.Count];

                    int d = 0;

                    foreach (ListViewItem i in newSpawns) items[d++] = i;

                    try
                    {
                        SpawnList.listView.Items.AddRange(items);
                    }

                    catch (Exception ex) { LogLib.WriteLine("Error Loading SpawnList: ", ex); }

                    newSpawns.Clear();
                    if (zoning)
                        SpawnList.listView.EndUpdate();
                }

            }

            catch (Exception ex) { LogLib.WriteLine("Error in ProcessSpawnList(): ", ex); }

            LogLib.WriteLine("Exiting ProcessSpawnList()", LogLevel.Trace);

        }

        public void ProcessGroundItemList(ListViewPanel GroundItemList)
        {

            LogLib.WriteLine("Entering ProcessGroundItemList()", LogLevel.Trace);

            try
            {

                if (newGroundItems.Count > 0)
                {

                    ListViewItem[] items = new ListViewItem[newGroundItems.Count];

                    int d = 0;

                    foreach (ListViewItem i in newGroundItems) items[d++] = i;

                    try
                    {

                        GroundItemList.listView.Items.AddRange(items);

                    }

                    catch (Exception ex) { LogLib.WriteLine("Error Loading GroundItem: ", ex); }

                    newGroundItems.Clear();

                }

            }

            catch (Exception ex) { LogLib.WriteLine("Error in ProcessGroundItemList(): ", ex); }

            LogLib.WriteLine("Exiting ProcessGroundItemList()", LogLevel.Trace);

        }

        public float CalcRealHeading(SPAWNINFO sp)
        {

            try
            {

                if (sp.Heading >= 0 && sp.Heading < 512)

                    return (sp.Heading / 512 * 360);

                else

                    return 0;

            }

            catch (Exception ex)
            {

                LogLib.WriteLine("Error with CalcRealHeading: ", ex);

                return 0;

            }

        }



        private bool isSubstring(string toSearch, string forSearch)
        {

            // Compile the regular expression.

            Regex regEx = new Regex(".*" + forSearch + ".*", RegexOptions.IgnoreCase);

            // Match the regular expression pattern against a text string.

            return regEx.Match(toSearch).Success;

        }



        public void LoadSpawnInfo()
        {

            // Used to improve packet processing speed

            PrefixStars = Settings.Instance.PrefixStars;

            AffixStars = Settings.Instance.AffixStars;

            CorpseAlerts = Settings.Instance.CorpseAlerts;

            MatchFullTextH = Settings.Instance.MatchFullTextH;

            MatchFullTextC = Settings.Instance.MatchFullTextC;

            MatchFullTextD = Settings.Instance.MatchFullTextD;

            MatchFullTextA = Settings.Instance.MatchFullTextA;

            HuntPrefix = Settings.Instance.HuntPrefix;

            CautionPrefix = Settings.Instance.CautionPrefix;

            DangerPrefix = Settings.Instance.DangerPrefix;

            AlertPrefix = Settings.Instance.AlertPrefix;

        }





        #region ProcessPlayer

        public void ProcessPlayer(SPAWNINFO si, myseq.frmMain f1)
        {

            LogLib.WriteLine("Entering ProcessPlayer()", LogLevel.Trace);

            try
            {

                playerinfo.SpawnID = si.SpawnID;

                if (playerinfo.Name != si.Name || playerinfo.Name.ToString() == string.Empty)
                {
                    playerinfo.Name = si.Name;
                    f1.SetCharSelection(playerinfo.Name.ToString());
                    f1.SetTitle();

                }
                else
                {
                    // needed? doubt it...
                    playerinfo.Name = si.Name;

                }

                playerinfo.Lastname = si.Lastname;

                if ((playerinfo.X != si.X) || (playerinfo.Y != si.Y))
                {

                    playerinfo.X = si.X;

                    playerinfo.Y = si.Y;

                    if (Settings.Instance.FollowOption == FollowOption.Player)

                        f1.ReAdjust();

                }

                playerinfo.Z = si.Z;

                playerinfo.Heading = si.Heading;

                playerinfo.Hide = si.Hide;

                playerinfo.SpeedRun = si.SpeedRun;

                if (playerinfo.Level != si.Level)
                {
                    if (f1.gConBaseName.Length > 0)
                    {
                        if (si.Level > playerinfo.Level)
                        {
                            int diff = si.Level - playerinfo.Level;
                            f1.gconLevel += diff;
                        }
                        else
                        {
                            int diff = playerinfo.Level - si.Level;
                            f1.gconLevel -= diff;
                        }
                        if (f1.gconLevel > 105)
                            f1.gconLevel = 105;
                        if (f1.gconLevel < 1)
                            f1.gconLevel = -1;
                        if (f1.gconLevel == -1)
                        {
                            f1.gConBaseName = "";
                        }
                        f1.gLastconLevel = f1.gconLevel;
                        Settings.Instance.LevelOverride = f1.gconLevel;
                    }
                    playerinfo.Level = si.Level;
                    fillConColors(f1);

                    // update mob list con colors

                    UpdateMobListColors();

                }
                if (f1.gLastconLevel != f1.gconLevel)
                {
                    f1.gLastconLevel = f1.gconLevel;
                    fillConColors(f1);
                    UpdateMobListColors();
                }

            }

            catch (Exception ex) { LogLib.WriteLine("Error in ProcessPlayer(): ", ex); }

            LogLib.WriteLine("Exiting ProcessPlayer()", LogLevel.Trace);

        }

        #endregion



        // TODO: this is to be replaced by a notification mechanism        

        //public void MustReadjust()

        //{

        //    m_readjustRequired = true;

        //}



        public void fillConColors(frmMain f1)
        {

            try
            {

                int c = 0;

                int level;



                if (Settings.Instance.LevelOverride == -1)
                {
                    f1.toolStripLevel.Text = "Auto";
                    level = playerinfo.Level;
                }
                else
                {
                    level = Settings.Instance.LevelOverride;
                    f1.toolStripLevel.Text = level.ToString();
                }
                yellowRange = 3;

                cyanRange = -5;

                greenRange = (-1) * level;

                greyRange = (-1) * level;



                String ConColorsFile = Path.Combine(Settings.Instance.CfgDir, "ConLevels.Ini");

                // If using SoD/Titanium Con Colors

                if (Settings.Instance.SoDCon)
                {

                    yellowRange = 2;

                    greyRange = (-1) * level;

                    if (level < 9)
                    {

                        greenRange = -3;

                        cyanRange = -7;

                    }
                    else if (level < 13)
                    {

                        greenRange = -5;

                        cyanRange = -3;

                    }
                    else if (level < 17)
                    {

                        greenRange = -6;

                        cyanRange = -4;

                    }
                    else if (level < 21)
                    {

                        greenRange = -7;

                        cyanRange = -5;

                    }
                    else if (level < 25)
                    {

                        greenRange = -8;

                        cyanRange = -6;

                    }
                    else if (level < 29)
                    {

                        greenRange = -9;

                        cyanRange = -7;

                    }
                    else if (level < 31)
                    {

                        greenRange = -10;

                        cyanRange = -8;

                    }
                    else if (level < 33)
                    {

                        greenRange = -11;

                        cyanRange = -8;

                    }
                    else if (level < 37)
                    {

                        greenRange = -12;

                        cyanRange = -9;

                    }
                    else if (level < 41)
                    {

                        greenRange = -13;

                        cyanRange = -10;

                    }
                    else if (level < 45)
                    {

                        greenRange = -15;

                        cyanRange = -11;

                    }
                    else if (level < 49)
                    {

                        greenRange = -16;

                        cyanRange = -12;

                    }
                    else if (level < 53)
                    {

                        greenRange = -17;

                        cyanRange = -13;

                    }
                    else if (level < 55)
                    {

                        greenRange = -18;

                        cyanRange = -14;

                    }
                    else if (level < 57)
                    {

                        greenRange = -19;

                        cyanRange = -14;

                    }

                    else
                    {

                        greenRange = -20;

                        cyanRange = -15;

                    }

                }

                // If using SoF Con Colors

                else if (Settings.Instance.SoFCon)
                {

                    yellowRange = 3;

                    cyanRange = -5;

                    if (level < 9)
                    {

                        greyRange = -3;

                        greenRange = -7;

                    }

                    else if (level < 10)
                    {

                        greyRange = -4;

                        greenRange = -3;

                    }

                    else if (level < 13)
                    {

                        greyRange = -5;

                        greenRange = -3;

                    }

                    else if (level < 17)
                    {

                        greyRange = -6;

                        greenRange = -4;

                    }

                    else if (level < 21)
                    {

                        greyRange = -7;

                        greenRange = -5;

                    }

                    else if (level < 25)
                    {

                        greyRange = -8;

                        greenRange = -6;

                    }

                    else if (level < 29)
                    {

                        greyRange = -9;

                        greenRange = -7;

                    }

                    else if (level < 31)
                    {

                        greyRange = -10;

                        greenRange = -8;

                    }

                    else if (level < 33)
                    {

                        greyRange = -11;

                        greenRange = -8;

                    }

                    else if (level < 37)
                    {

                        greyRange = -12;

                        greenRange = -9;

                    }

                    else if (level < 41)
                    {

                        greyRange = -13;

                        greenRange = -10;

                    }

                    else if (level < 45)
                    {

                        greyRange = -15;

                        greenRange = -11;

                    }

                    else if (level < 49)
                    {

                        greyRange = -16;

                        greenRange = -12;

                    }

                    else if (level < 53)
                    {

                        greyRange = -17;

                        greenRange = -13;

                    }

                    else if (level < 55)
                    {

                        greyRange = -18;

                        greenRange = -14;

                    }

                    else if (level < 57)
                    {

                        greyRange = -19;

                        greenRange = -14;

                    }

                    else
                    {

                        greyRange = -20;

                        greenRange = -15;

                    }

                }

                else if (File.Exists(ConColorsFile))
                {

                    IniFile Ini = new IniFile(ConColorsFile);

                    string sIniValue = "";

                    sIniValue = Ini.ReadValue("Con Levels", level.ToString(), "0/0/0");

                    string yellowLevels = "";

                    yellowLevels = Ini.ReadValue("Con Levels", "0", "3");

                    string[] ConLevels = sIniValue.Split('/');



                    greyRange = Convert.ToInt32(ConLevels[0]) - level + 1;

                    greenRange = Convert.ToInt32(ConLevels[1]) - level + 1;

                    cyanRange = Convert.ToInt32(ConLevels[2]) - level + 1;

                    yellowRange = Convert.ToInt32(yellowLevels);

                }

                else if (Settings.Instance.DefaultCon)
                {

                    // Using Default Con Colors

                    cyanRange = -5;

                    if (level < 16) // verified
                    {

                        greyRange = -5;

                        greenRange = -5;

                    }

                    else if (level < 19) // verified
                    {

                        greyRange = -6;

                        greenRange = -5;

                    }

                    else if (level < 21) // verified
                    {

                        greyRange = -7;

                        greenRange = -5;

                    }

                    else if (level < 22) // verified
                    {

                        greyRange = -7;

                        greenRange = -6;

                    }

                    else if (level < 25) // verified
                    {

                        greyRange = -8;

                        greenRange = -6;

                    }

                    else if (level < 28) // verified
                    {

                        greyRange = -9;

                        greenRange = -7;

                    }

                    else if (level < 29) // verified
                    {

                        greyRange = -10;

                        greenRange = -7;

                    }

                    else if (level < 31) // verified
                    {

                        greyRange = -10;

                        greenRange = -8;

                    }

                    else if (level < 33) // verified
                    {

                        greyRange = -11;

                        greenRange = -8;

                    }

                    else if (level < 34) // verified
                    {

                        greyRange = -11;

                        greenRange = -9;

                    }

                    else if (level < 37) // verified
                    {

                        greyRange = -12;

                        greenRange = -9;

                    }

                    else if (level < 40) // verified
                    {

                        greyRange = -13;

                        greenRange = -10;

                    }

                    else if (level < 41) // Verified
                    {

                        greyRange = -14;

                        greenRange = -10;

                    }

                    else if (level < 43) // Verified
                    {

                        greyRange = -14;

                        greenRange = -11;

                    }

                    else if (level < 45)  // Verified
                    {

                        greyRange = -15;

                        greenRange = -11;

                    }

                    else if (level < 46)  // Verified
                    {

                        greyRange = -15;

                        greenRange = -12;

                    }

                    else if (level < 49)  // Verified
                    {

                        greyRange = -16;

                        greenRange = -12;

                    }

                    else if (level < 51) // Verified at 50
                    {

                        greyRange = -17;

                        greenRange = -13;

                    }

                    else if (level < 53)
                    {

                        greyRange = -18;

                        greenRange = -14;

                    }

                    else if (level < 57)
                    {

                        greyRange = -20;

                        greenRange = -15;

                    }

                    else
                    {

                        greyRange = -21;

                        greenRange = -16;

                    }
                }



                // Set the Grey Cons

                for (c = 0; c < (greyRange + level); c++)
                {

                    conColors[c] = new SolidBrush(Color.Gray);

                }

                // Set the Green Cons

                for (; c < (greenRange + level); c++)
                {

                    conColors[c] = new SolidBrush(Color.Lime);

                }



                // Set the Light Blue Cons

                for (; c < (cyanRange + level); c++)
                {

                    conColors[c] = new SolidBrush(Color.Aqua);

                }



                // Set the Dark Blue Cons

                for (; c < level; c++)
                {

                    conColors[c] = new SolidBrush(Color.Blue);

                }



                // Set the Same Level Con

                conColors[c++] = new SolidBrush(Color.White);



                // Yellow Cons

                for (; c < (level + yellowRange + 1); c++)
                {

                    conColors[c] = new SolidBrush(Color.Yellow);

                }



                // 4 levels of bright red

                conColors[c++] = new SolidBrush(Color.Red);

                conColors[c++] = new SolidBrush(Color.Red);

                conColors[c++] = new SolidBrush(Color.Red);

                conColors[c++] = new SolidBrush(Color.Red);



                // Set the remaining levels to dark red

                for (; c < 1000; c++)
                {

                    conColors[c] = new SolidBrush(Color.Maroon);

                }

            }

            catch { }

        }



        public void Clear()
        {

            mobs.Clear();

            items.Clear();

            mobtrails.Clear();

        }

        public float CalcDotProduct(float x1, float y1, float z1, float x2, float y2, float z2, float x3, float y3, float z3)
        {

            double lenV1;

            double lenV2;

            double lenV3;

            //double dotprod;

            if ((x1 == x2 && y1 == y2 && z1 == z2) || (x2 == x3 && y2 == y3 && z2 == z3))

                return 1.0f;

            lenV1 = Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1) + (z2 - z1) * (z2 - z1));

            lenV2 = Math.Sqrt((x3 - x2) * (x3 - x2) + (y3 - y2) * (y3 - y2) + (z3 - z2) * (z3 - z2));

            lenV3 = Math.Sqrt((x3 - x1) * (x3 - x1) + (y3 - y1) * (y3 - y1) + (z3 - z1) * (z3 - z1));

            // dotprod =(((x2 - x1)/lenV1 * (x3-x2)/lenV2) + ((y2 - y1)/lenV1 * (y3-y2)/lenV2) + ((z2 - z1)/lenV1 * (z3-z2)/lenV2));



            return (float)(lenV3 / (lenV1 + lenV2));

        }

        public void CalculateMapLinePens()
        {
            if (lines == null)
                return;

            Pen darkpen = new Pen(Color.Black);
            foreach (MapLine line in lines)
            {
                if (Settings.Instance.ForceDistinct)
                {

                    line.draw_color = GetDistinctColor(darkpen);
                    line.fade_color = new Pen(Color.FromArgb((int)(Settings.Instance.FadedLines * 255 / 100), line.draw_color.Color));
                }
                else
                {
                    line.draw_color = GetDistinctColor(new Pen(line.color.Color));
                    line.fade_color = new Pen(Color.FromArgb((int)(Settings.Instance.FadedLines * 255 / 100), line.draw_color.Color));
                }
            }
            SolidBrush distinctbrush = new SolidBrush(Color.Black);
            foreach (MapText t in texts)
            {

                if (Settings.Instance.ForceDistinctText)
                    t.draw_color = GetDistinctColor(distinctbrush);
                else
                    t.draw_color = GetDistinctColor(t.color);
                t.draw_pen = new Pen(t.draw_color.Color);
            }

        }


        public void OptimizeMap()
        {

            if (lines == null)

                return;

            ArrayList linesToRemove = new ArrayList();

            Pen thisColor = null;

            Pen lastColor = null;

            MapLine thisline = null;

            MapLine lastline = null;

            MapPoint thisprev = null;

            MapPoint thispoint = null;

            MapPoint thisnext = null;

            int thiscount = 0;

            MapPoint lastpoint = null;

            MapPoint lastprev = null;

            MapPoint lastnext = null;

            int lastcount = 0;

            float prod;

            int droppoint = 0;

            int pointsdrop = 0;

            foreach (MapLine line in lines)
            {

                thisline = line;

                if (thisline != null && lastline != null)
                {

                    thiscount = thisline.aPoints.Count;

                    thispoint = (MapPoint)thisline.aPoints[0];

                    thisnext = (MapPoint)thisline.aPoints[1];

                    thisColor = thisline.color;

                    lastcount = lastline.aPoints.Count;

                    lastpoint = (MapPoint)lastline.aPoints[lastcount - 1];

                    lastprev = (MapPoint)lastline.aPoints[lastcount - 2];

                    lastColor = lastline.color;



                    if (lastpoint.x == thispoint.x && lastpoint.y == thispoint.y && lastpoint.z == thispoint.z && thisColor.Color == lastColor.Color)
                    {

                        prod = 0f;

                        droppoint = 0;

                        // Take Dot Product to see if lines have 0 degrees between angle

                        // Basic Dot Product, where varies from -1 at 180 degrees to 1 at 0 degrees

                        if ((thiscount > 1) && (lastcount > 1))
                        {

                            prod = CalcDotProduct(lastprev.x, lastprev.y, lastprev.z, thispoint.x, thispoint.y, thispoint.z, thisnext.x, thisnext.y, thisnext.z);

                            if (prod > 0.9999f)
                            {

                                pointsdrop++;

                                droppoint = 1;

                            }

                        }

                        // Second Line Starts at End of First Line

                        lastline.linePoints = new PointF[thiscount + lastcount - 1 - droppoint];

                        for (int p = 0; p < (lastcount - droppoint); p++)
                        {

                            MapPoint tmp = (MapPoint)lastline.aPoints[p];

                            lastline.linePoints[p] = new PointF(tmp.x, tmp.y);

                        }

                        if (droppoint == 1)

                            lastline.aPoints.RemoveAt(lastcount - 1);

                        for (int p = 1; p < thiscount; p++)
                        {

                            MapPoint tmp = (MapPoint)thisline.aPoints[p];

                            lastline.linePoints[p + lastcount - 1 - droppoint] = new PointF(tmp.x, tmp.y);

                            MapPoint temp = new MapPoint();

                            temp.x = tmp.x;

                            temp.y = tmp.y;

                            temp.z = tmp.z;

                            lastline.aPoints.Add(temp);

                        }

                        linesToRemove.Add(thisline);

                        thisline = lastline;

                    }

                    else
                    {

                        prod = 0.0f;

                        droppoint = 0;

                        thispoint = (MapPoint)thisline.aPoints[thiscount - 1];

                        thisprev = (MapPoint)thisline.aPoints[thiscount - 2];

                        lastpoint = (MapPoint)lastline.aPoints[0];

                        lastnext = (MapPoint)lastline.aPoints[1];

                        if (lastpoint.x == thispoint.x && lastpoint.y == thispoint.y && lastpoint.z == thispoint.z && thisColor.Color == lastColor.Color)
                        {

                            prod = CalcDotProduct(thisprev.x, thisprev.y, thisprev.z, thispoint.x, thispoint.y, thispoint.z, lastnext.x, lastnext.y, lastnext.z);

                            if (prod > 0.9999f)
                            {

                                pointsdrop++;

                                droppoint = 1;

                                // look here

                                //prod = 1.0f;

                            }

                            // Second Line is at beginning of first line

                            lastline.linePoints = new PointF[thiscount + lastcount - 1 - droppoint];

                            if (droppoint == 1)

                                lastline.aPoints.RemoveAt(0);

                            for (int p = 0; p < (thiscount - 1); p++)
                            {

                                MapPoint tmp = (MapPoint)thisline.aPoints[p];

                                MapPoint temp = new MapPoint();

                                temp.x = tmp.x;

                                temp.y = tmp.y;

                                temp.z = tmp.z;

                                lastline.aPoints.Insert(p, temp);

                            }

                            thiscount = lastline.aPoints.Count;

                            for (int p = 0; p < thiscount; p++)
                            {

                                MapPoint tmp = (MapPoint)lastline.aPoints[p];

                                lastline.linePoints[p] = new PointF(tmp.x, tmp.y);

                            }

                            linesToRemove.Add(thisline);

                            thisline = lastline;

                        }

                    }

                }

                lastline = thisline;

            }

            foreach (MapLine lineToRemove in linesToRemove) lines.Remove(lineToRemove);
            foreach (MapLine line in lines)
            {
                line.maxZ = line.minZ = line.Point(0).z;
                for (int j = 1; j < line.aPoints.Count; j++)
                {
                    if (line.minZ > line.Point(j).z)
                        line.minZ = line.Point(j).z;
                    if (line.maxZ < line.Point(j).z)
                        line.maxZ = line.Point(j).z;
                }
            }
            // Put in offsets for use when drawing text on map, for duplicate text at same location
            int index = 0;
            foreach (MapText tex1 in texts)
            {
                int index2 = 0;
                foreach (MapText tex2 in texts)
                {
                    if (index2 > index && tex1.x == tex2.x && tex1.y == tex2.y && tex1.z == tex2.z && tex1.text != tex2.text)
                    {
                        tex2.offset = tex1.offset + (int)(2.0f * Settings.Instance.MapLabelFontSize);
                    }
                    index2++;
                }
                index++;
            }
        }

        #region ColorCheck between Foreground and Background

        private int GetColorDiff(Color foreColor, Color backColor)
        {

            int lColDiff, lTmp;



            lColDiff = 0;



            lTmp = Math.Abs(backColor.R - foreColor.R);

            lColDiff = Math.Max(lColDiff, lTmp);



            lTmp = Math.Abs(backColor.G - foreColor.G);

            lColDiff = Math.Max(lColDiff, lTmp);



            lTmp = Math.Abs(backColor.B - foreColor.B);

            lColDiff = Math.Max(lColDiff, lTmp);



            return lColDiff;

        }

        private Color GetInverseColor(Color foreColor)
        {

            short iFRed, iFGreen, iFBlue;

            iFRed = foreColor.R;

            iFGreen = foreColor.G;

            iFBlue = foreColor.B;



            return Color.FromArgb((int)(192 - iFRed * 0.75), (int)(192 - iFGreen * 0.75), (int)(192 - iFBlue * 0.75));

        }

        public Color GetDistinctColor(Color foreColor, Color backColor)
        {

            // make sure the fore + back color can be distinguished.

            const int ColorThreshold = 55;



            if (GetColorDiff(foreColor, backColor) >= ColorThreshold)

                return foreColor;

            else
            {

                Color inverseColor = GetInverseColor(foreColor);



                if (GetColorDiff(inverseColor, backColor) > ColorThreshold)

                    return inverseColor;

                else //' if we have grey rgb(127,127,127) the inverse is the same so return black...

                    return Color.Black;

            }

        }

        public Color GetDistinctColor(Color curColor)
        {

            return GetDistinctColor(curColor, Settings.Instance.BackColor);

        }

        public Pen GetDistinctColor(Pen curPen)
        {

            curPen.Color = GetDistinctColor(curPen.Color, Settings.Instance.BackColor);

            return curPen;

        }

        public SolidBrush GetDistinctColor(SolidBrush curBrush)
        {

            curBrush.Color = GetDistinctColor(curBrush.Color, Settings.Instance.BackColor);

            return curBrush;

        }

        #endregion
    }


    public class Talker

    {

        public string speakText;

        public static SpVoice speech = new SpVoice();

        public void SpeakText()

        {

            speech.Speak(speakText, SpeechVoiceSpeakFlags.SVSFDefault);

        }

    }

}

