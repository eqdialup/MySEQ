using Structures;
using System;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using myseq.Properties;

namespace myseq
{
    // This is the "model" part - no UI related things in here, only hard EQ data.

    public class EQData
    {
        // player details
        public SPAWNINFO playerinfo = new SPAWNINFO();

        // Map details
        public string longname = "";

        public string shortname = "";

        // Map data
        public ArrayList lines = new ArrayList();//MapLine[MAX_LINES]

        public ArrayList texts = new ArrayList();//MapText[50]

        private ArrayList mobtrails = new ArrayList();//MobTrailPoint[1000]

        private ArrayList xlabels = new ArrayList();

        private ArrayList ylabels = new ArrayList();

        private bool playAlerts;

        // Max + Min map coordinates - define the bounds of the zone

        public float minx = -1000;

        public float maxx = 1000;

        public float miny = -1000;

        public float maxy = 1000;

        public float minz = -1000;

        public float maxz = 1000;

        // Mobs

        private ArrayList itemcollection = new ArrayList();          // Hold the items that are on the ground

        private Hashtable mobs = new Hashtable();           // Holds the details of the mobs in the current zone.

        public MobsTimers mobsTimers = new MobsTimers();   // Manages the timers

        public int selectedID = 99999;

        public float SpawnX = -1;

        public float SpawnY = -1;

        private int EQSelectedID = 0;

        public DateTime gametime = new DateTime();

        private readonly Random rnd = new Random();

        // Mobs / UI Lists

        public ArrayList newSpawns = new ArrayList();

        public ArrayList newGroundItems = new ArrayList();

        // Items List by ID and Description loaded from file

        public Hashtable itemList = new Hashtable();

        // Guild List by ID and Description loaded from file

        public Hashtable guildList = new Hashtable();

        // Mobs / Filters

        // Used to improve packet processing speed

        private bool PrefixStars = true;

        private bool AffixStars = true;

        private bool CorpseAlerts = true;

        private bool MatchFullTextH;

        private bool MatchFullTextC;

        private bool MatchFullTextD;

        private bool MatchFullTextA;

        private string HuntPrefix = "";

        private string CautionPrefix = "";

        private string DangerPrefix = "";

        private string AlertPrefix = "";

        private string[] Classes;

        private string[] Races;

        // Blech, some UI stuff... but at least it's shareable between several users

        public SolidBrush[] conColors = new SolidBrush[1000];
        public int GreenRange { get; set; }

        public int CyanRange { get; set; }

        public int GreyRange { get; set; }
        public int YellowRange { get; set; } = 3;

        private readonly Structures.ColorConverter ColorChart = new Structures.ColorConverter();

        public bool Zoning { get; set; }

        private const int ditchGone = 2;
        private const string dflt = "Mob Search";
        private string search0 = "";
        private string search1 = "";
        private string search2 = "";
        private string search3 = "";
        private string search4 = "";
        private string search5 = "";
        private bool filter0;
        private bool filter1;
        private bool filter2;
        private bool filter3;
        private bool filter4;
        private bool filter5;

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
            if (name.Length > 2 && name.Substring(2) == dflt) { name = name.Substring(0, 2); }
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

            foreach (SPAWNINFO sp in mobs.Values)
            {
                sp.isLookup = false;
                sp.lookupNumber = "";
                if (search0.Length > 1)
                {
                    bool levelCheck = false;
                    if (search0.Length > 2 && string.Equals(search0.Substring(0, 2), "L:", StringComparison.OrdinalIgnoreCase))
                    {
                        int.TryParse(search0.Substring(2), out var searchLevel);
                        if (searchLevel != 0 && (sp.Level == searchLevel))
                        {
                            levelCheck = true;
                        }
                    }
                    if (levelCheck || RegexHelper.GetRegex(search0).Match(sp.Name).Success)
                    {
                        sp.isLookup = true;
                        sp.lookupNumber = "1";
                        sp.hidden = false;
                        if (filter0) { sp.hidden = true; }
                    }
                }
                if (search1.Length > 1)
                {
                    bool levelCheck = false;
                    if (search1.Length > 2 && string.Equals(search1.Substring(0, 2), "L:", StringComparison.OrdinalIgnoreCase))
                    {
                        int.TryParse(search1.Substring(2), out var searchLevel);
                        if (searchLevel != 0 && (sp.Level == searchLevel))
                        {
                            levelCheck = true;
                        }
                    }
                    if (levelCheck || RegexHelper.GetRegex(search1).Match(sp.Name).Success)
                    {
                        sp.isLookup = true;
                        sp.lookupNumber = "2";
                        sp.hidden = false;
                        if (filter1) { sp.hidden = true; }
                    }
                }
                if (search2.Length > 1)
                {
                    bool levelCheck = false;
                    if (search2.Length > 2 && string.Equals(search2.Substring(0, 2), "L:", StringComparison.OrdinalIgnoreCase))
                    {
                        int.TryParse(search2.Substring(2), out var searchLevel);
                        if (searchLevel != 0 && (sp.Level == searchLevel))
                        {
                            levelCheck = true;
                        }
                    }
                    if (levelCheck || RegexHelper.GetRegex(search2).Match(sp.Name).Success)
                    {
                        sp.isLookup = true;
                        sp.lookupNumber = "3";
                        sp.hidden = false;
                        if (filter2) { sp.hidden = true; }
                    }
                }
                if (search3.Length > 1)
                {
                    bool levelCheck = false;
                    if (search3.Length > 2 && string.Equals(search3.Substring(0, 2), "L:", StringComparison.OrdinalIgnoreCase))
                    {
                        int.TryParse(search3.Substring(2), out var searchLevel);
                        if (searchLevel != 0 && (sp.Level == searchLevel))
                        {
                            levelCheck = true;
                        }
                    }
                    if (levelCheck || RegexHelper.GetRegex(search3).Match(sp.Name).Success)
                    {
                        sp.isLookup = true;
                        sp.lookupNumber = "4";
                        sp.hidden = false;
                        if (filter3) { sp.hidden = true; }
                    }
                }
                if (search4.Length > 1)
                {
                    bool levelCheck = false;
                    if (search4.Length > 2 && string.Equals(search4.Substring(0, 2), "L:", StringComparison.OrdinalIgnoreCase))
                    {
                        int.TryParse(search4.Substring(2), out var searchLevel);
                        if (searchLevel != 0 && (sp.Level == searchLevel))
                        {
                            levelCheck = true;
                        }
                    }
                    if (levelCheck || RegexHelper.GetRegex(search4).Match(sp.Name).Success)
                    {
                        sp.isLookup = true;
                        sp.lookupNumber = "5";
                        sp.hidden = false;
                        if (filter4) { sp.hidden = true; }
                    }
                }
                if (search5.Length > 1)
                {
                    bool levelCheck = false;
                    if (search5.Length > 2 && string.Equals(search5.Substring(0, 2), "L:", StringComparison.OrdinalIgnoreCase))
                    {
                        int.TryParse(search5.Substring(2), out var searchLevel);
                        if (searchLevel != 0 && (sp.Level == searchLevel))
                        {
                            levelCheck = true;
                        }
                    }
                    if (levelCheck || RegexHelper.GetRegex(search5).Match(sp.Name).Success)
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

        public Hashtable GetMobsReadonly()
        {
            return mobs;
        }

        public ArrayList GetMobTrailsReadonly()
        {
            return mobtrails;
        }

        public ArrayList GetLinesReadonly()
        {
            return lines;
        }

        public ArrayList GetTextsReadonly()
        {
            return texts;
        }

        public ArrayList GetItemsReadonly()
        {
            return itemcollection;
        }

        public void PlayAlertSound()
        {
            switch (Settings.Default.AlertSound)
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

            if (st != null)
            {
                if (Settings.Default.AutoSelectSpawnList && st.itmSpawnTimerList != null)
                {
                    st.itmSpawnTimerList.EnsureVisible();
                    st.itmSpawnTimerList.Selected = true;
                }
                SPAWNINFO sp = FindMobTimer(st.SpawnLoc);

                selectedID = sp == null ? 99999 : (int)sp.SpawnID;

                SpawnX = st.X;

                SpawnY = st.Y;

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

            if (gi != null)
            {
                if (Settings.Default.AutoSelectSpawnList)
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
            SPAWNINFO sp = FindMobNoPet(delta, x, y) ?? FindMob(delta, x, y);

            if (sp != null)
            {
                if (Settings.Default.AutoSelectSpawnList)
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
                    if (!sp.filtered && !sp.hidden && !sp.isFamiliar && !sp.isPet && !sp.isFamiliar && !sp.isMerc && sp.X < x + delta && sp.X > x - delta && sp.Y < y + delta && sp.Y > y - delta)
                    {
                        return sp;
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
                    if (!sp.hidden && !sp.isFamiliar && !sp.isPet && !sp.isFamiliar && !sp.isMerc && !sp.m_isPlayer && !sp.isCorpse && sp.X < x + delta && sp.X > x - delta && sp.Y < y + delta && sp.Y > y - delta)
                    {
                        return sp;
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
                    if (!sp.hidden && !sp.isFamiliar && !sp.isPet && !sp.isFamiliar && !sp.isMerc && !sp.isCorpse && sp.X < x + delta && sp.X > x - delta && sp.Y < y + delta && sp.Y > y - delta)
                    {
                        return sp;
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
                    if (!sp.hidden && !sp.filtered && sp.X < x + delta && sp.X > x - delta && sp.Y < y + delta && sp.Y > y - delta)
                    {
                        return sp;
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

        public GroundItem FindGroundItem(float delta, float x, float y)
        {
            foreach (GroundItem gi in itemcollection)
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
            foreach (GroundItem gi in itemcollection)
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
            Classes = GetStrArrayFromTextFile(Path.Combine(Settings.Default.CfgDir, "Classes.txt"));

            Races = GetStrArrayFromTextFile(Path.Combine(Settings.Default.CfgDir, "Races.txt"));

            itemList.Clear();

            ReadItemList(Path.Combine(Settings.Default.CfgDir, "GroundItems.ini"));

            guildList.Clear();

            ReadGuildList(Path.Combine(Settings.Default.CfgDir, "Guilds.txt"));

            ColorChart.Initialise(Path.Combine(Settings.Default.CfgDir, "RGB.txt"));
        }

        public bool LoadMapInternal(string filename)
        {
            // All Parse Routines MUST be passed this culture info so that they work

            // correctly when used on an operating system configured to use a different culture.

            IFormatProvider NumFormat = new CultureInfo("en-US");

            StreamReader tr;
            int numtexts = 0;

            int numlines = 0;

            int curLine = 0;

            if (!File.Exists(filename))
            {
                LogLib.WriteLine($"File not found loading {filename} in loadMap()");

                return false;
            }

            try { tr = new StreamReader(File.OpenRead(filename)); }
            catch (FileNotFoundException)
            {
                LogLib.WriteLine($"File not found loading {filename} in loadMap()");

                return false;
            }

            LogLib.WriteLine($"Loading Zone Map (non LoY): {filename}");

            // First line is file ID info

            var line = tr.ReadLine();
            curLine++;

            if (line == null)
                return false;

            int lineCount = 1;

            if ((longname = Getnexttoken(ref line, ',')) == null)
                return false;

            if ((shortname = Getnexttoken(ref line, ',')) == null)
                return false;

            string tok = "";
            // Rest of header is optional.

            try
            {
                // ICK....

                if ((tok = Getnexttoken(ref line, ',')) != null)
                {
                    minx = int.Parse(tok, NumFormat);

                    if ((tok = Getnexttoken(ref line, ',')) != null)
                    {
                        miny = int.Parse(tok, NumFormat);

                        if ((tok = Getnexttoken(ref line, ',')) != null)
                        {
                            minz = int.Parse(tok, NumFormat);

                            if ((tok = Getnexttoken(ref line, ',')) != null)
                            {
                                maxx = int.Parse(tok, NumFormat);

                                if ((tok = Getnexttoken(ref line, ',')) != null)
                                {
                                    maxy = int.Parse(tok, NumFormat);

                                    if ((tok = Getnexttoken(ref line, ',')) != null)
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
                try
                {
                    if (line != "")
                    {
                        if ((tok = Getnexttoken(ref line, ',')) == null)
                        {
                            LogLib.WriteLine($"Warning - Line {curLine} of map '{filename}' has an invalid format and will be ignored.", LogLevel.Warning);
                        }
                        else
                        {
                            ParseMLP(filename, NumFormat, ref numtexts, ref numlines, curLine, ref tok, ref line);
                        }
                    }
                }
                catch (Exception ex) { LogLib.WriteLine($"Error in loadMap() Line: {curLine}:", ex); }
            }

            LogLib.WriteLine($"{curLine} lines processed.", LogLevel.Debug);

            LogLib.WriteLine($"Loaded {lines.Count} lines", LogLevel.Debug);

            tr.Close();

            if (numtexts > 0 || lineCount > 0)
            {
                CalcExtents();

                return true;
            }
            else
            {
                longname = "";

                shortname = "";

                return false;
            }
        }

        private void ParseMLP(string filename, IFormatProvider NumFormat, ref int numtexts, ref int numlines, int curLine, ref string tok, ref string line)
        {
            if (tok == "Z") { } // Ignore the ZEM one
            else if (tok == "M")
            {
                MapLine work = new MapLine();

                int numpoints = 0;

                bool bOK = true;

                if (bOK && (tok = Getnexttoken(ref line, ',')) != null)
                    work.name = tok;
                else
                    bOK = false;

                if (bOK && (tok = Getnexttoken(ref line, ',')) != null)
                    work.color = new Pen(new SolidBrush(ColorChart.StringToColor(tok)));
                else
                    bOK = false;

                if ((tok = Getnexttoken(ref line, ',')) != null)
                    numpoints = int.Parse(tok, NumFormat);
                else
                    bOK = false;

                if (bOK)
                    work.linePoints = new PointF[numpoints];

                int pointnum = 0;

                while (bOK && (tok = Getnexttoken(ref line, ',')) != null)
                {
                    MapPoint temp = new MapPoint
                    {
                        x = (int)float.Parse(tok, NumFormat)
                    };

                    if ((tok = Getnexttoken(ref line, ',')) != null)
                    {
                        temp.y = (int)float.Parse(tok, NumFormat);

                        if ((tok = Getnexttoken(ref line, ',')) != null)
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
                    LogLib.WriteLine(
                        $"Warning - Line {curLine} of map '{filename}' has an invalid point count. Expected - {numpoints}, Actual {work.aPoints.Count}. Continuing...",
                        LogLevel.Warning);
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

                if (bOK && (tok = Getnexttoken(ref line, ',')) != null)
                    work.name = tok;
                else
                    bOK = false;

                if (bOK && (tok = Getnexttoken(ref line, ',')) != null)
                    work.color = new Pen(new SolidBrush(ColorChart.StringToColor(tok)));
                else
                    bOK = false;

                if (bOK && (tok = Getnexttoken(ref line, ',')) != null)
                    numpoints = int.Parse(tok, NumFormat);
                else
                    bOK = false;

                if (bOK)
                {
                    int pointnum = 0;

                    while ((tok = Getnexttoken(ref line, ',')) != null)
                    {
                        MapPoint temp = new MapPoint
                        {
                            x = (int)float.Parse(tok, NumFormat)
                        };

                        if ((tok = Getnexttoken(ref line, ',')) != null)
                        {
                            temp.y = (int)float.Parse(tok, NumFormat);

                            work.aPoints.Add(temp);

                            work.linePoints[pointnum] = new PointF(temp.x, temp.y);

                            pointnum++;
                        }
                    }

                    if (numpoints != work.aPoints.Count)
                    {
                        LogLib.WriteLine(
                            $"Warning - Line {curLine} of map '{filename}' has an invalid point count. Expected - {numpoints}, Actual {work.aPoints.Count}. Continuing...", LogLevel.Warning);
                    }

                    if (work.aPoints.Count >= 2)
                    {
                        lines.Add(work);

                        numlines++;
                    }
                }
                else
                {
                    LogLib.WriteLine($"Warning - Line {curLine} of map '{filename}' has an invalid format and will be ignored.", LogLevel.Warning);
                }
            }
            else if (tok == "P")
            {
                MapText work = new MapText();

                bool bOK = true;

                if (bOK && (tok = Getnexttoken(ref line, ',')) != null)
                    work.text = tok;
                else
                    bOK = false;

                if (bOK && (tok = Getnexttoken(ref line, ',')) != null)
                    work.color = new SolidBrush(ColorChart.StringToColor(tok));
                else
                    bOK = false;

                if (bOK && (tok = Getnexttoken(ref line, ',')) != null)
                    work.x = int.Parse(tok, NumFormat);
                else
                    bOK = false;

                if ((tok = Getnexttoken(ref line, ',')) != null)
                    work.y = int.Parse(tok, NumFormat);
                else
                    bOK = false;

                if (bOK)
                {
                    // add a z value if it exists
                    work.z = (tok = Getnexttoken(ref line, ',')) != null
                        ? int.Parse(tok, NumFormat)
                        : -99999;

                    texts.Add(work);

                    numtexts++;
                }
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

        public bool LoadLoYMapInternal(string filename)
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
                LogLib.WriteLine($"File not found loading {filename} in loadLoYMap");

                return false;
            }

            try { tr = new StreamReader(File.OpenRead(filename)); }
            catch (FileNotFoundException)
            {
                LogLib.WriteLine($"File not found loading {filename} in loadLoYMap");

                return false;
            }

            LogLib.WriteLine($"Loading Zone Map (LoY): {filename}");

            int lineCount = 0;

            while ((line = tr.ReadLine()) != null)
            {
                curLine++;

                try
                {
                    lineCount++;

                    if (line != "")
                    {
                        if ((tok = Getnexttoken(ref line, ' ')) == null)
                        {
                            LogLib.WriteLine($"Warning - Line {curLine} of map '{filename}' has an invalid format and will be ignored.", LogLevel.Warning);
                        }
                        else
                        {
                            if (tok == "L")
                            {
                                MapLine work = new MapLine();

                                MapPoint point1 = new MapPoint();

                                MapPoint point2 = new MapPoint();

                                bool bOK = true;

                                if (bOK && (tok = Getnexttoken(ref line, ',')) != null)
                                    point1.x = -(int)float.Parse(tok, NumFormat);
                                else
                                    bOK = false;

                                if (bOK && (tok = Getnexttoken(ref line, ',')) != null)
                                    point1.y = -(int)float.Parse(tok, NumFormat);
                                else
                                    bOK = false;

                                if (bOK && (tok = Getnexttoken(ref line, ',')) != null)
                                    point1.z = (int)float.Parse(tok, NumFormat);
                                else
                                    bOK = false;

                                if (bOK && (tok = Getnexttoken(ref line, ',')) != null)
                                    point2.x = -(int)float.Parse(tok, NumFormat);
                                else
                                    bOK = false;

                                if (bOK && (tok = Getnexttoken(ref line, ',')) != null)
                                    point2.y = -(int)float.Parse(tok, NumFormat);
                                else
                                    bOK = false;

                                if (bOK && (tok = Getnexttoken(ref line, ',')) != null)
                                    point2.z = (int)float.Parse(tok, NumFormat);
                                else
                                    bOK = false;

                                int r = 0, g = 0, b = 0;

                                if (bOK && (tok = Getnexttoken(ref line, ',')) != null)
                                    r = int.Parse(tok, NumFormat);
                                else
                                    bOK = false;

                                if (bOK && (tok = Getnexttoken(ref line, ',')) != null)
                                    g = int.Parse(tok, NumFormat);
                                else
                                    bOK = false;

                                if (bOK && (tok = Getnexttoken(ref line, ',')) != null)
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
                                {
                                    LogLib.WriteLine($"Warning - Line {curLine} of map '{filename}' has an invalid format and will be ignored.", LogLevel.Warning);
                                }
                            }
                            else if (tok == "P")
                            {
                                MapText work = new MapText();

                                bool bOK = true;

                                if (bOK && (tok = Getnexttoken(ref line, ',')) != null)
                                    work.x = -(int)float.Parse(tok, NumFormat);
                                else
                                    bOK = false;

                                if (bOK && (tok = Getnexttoken(ref line, ',')) != null)
                                    work.y = -(int)float.Parse(tok, NumFormat);
                                else
                                    bOK = false;

                                if (bOK && (tok = Getnexttoken(ref line, ',')) != null)
                                {
                                    work.z = (int)float.Parse(tok, NumFormat);
                                }
                                else
                                {
                                    bOK = false;
                                }

                                int r = 0, g = 0, b = 0;

                                if (bOK && (tok = Getnexttoken(ref line, ',')) != null)
                                    r = int.Parse(tok, NumFormat);
                                else
                                    bOK = false;

                                if (bOK && (tok = Getnexttoken(ref line, ',')) != null)
                                    g = int.Parse(tok, NumFormat);
                                else
                                    bOK = false;

                                if (bOK && (tok = Getnexttoken(ref line, ',')) != null)
                                    b = int.Parse(tok, NumFormat);
                                else
                                    bOK = false;

                                work.color = new SolidBrush(Color.FromArgb(r, g, b));

                                if ((tok = Getnexttoken(ref line, ',')) != null)
                                {
                                    // This is text size
                                    // 1 - small
                                    // 2 - medium
                                    // 3 - large

                                    var text_size = int.Parse(tok, NumFormat);

                                    if (text_size > 0 && text_size <= 3)
                                        work.size = text_size;
                                }
                                else
                                {
                                    bOK = false;
                                }

                                if (bOK && (tok = Getnexttoken(ref line, ',')) != null)
                                    work.text = tok;
                                else
                                    bOK = false;

                                if (bOK)
                                {
                                    texts.Add(work);

                                    numtexts++;
                                }
                                else
                                {
                                    LogLib.WriteLine($"Warning - Line {curLine} of map '{filename}' has an invalid format and will be ignored.", LogLevel.Warning);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex) { LogLib.WriteLine("Error in loadLoYMap() Line: " + lineCount + " : ", ex); }
            }

            LogLib.WriteLine($"{curLine} lines processed.", LogLevel.Debug);

            LogLib.WriteLine($"Loaded {lines.Count} lines", LogLevel.Debug);

            tr.Close();

            if (numtexts > 0 || lineCount > 0)
            {
                shortname = Path.GetFileNameWithoutExtension(filename);

                if (shortname.IndexOf("_") > 0)
                    shortname = shortname.Substring(0, shortname.Length - 2);

                longname = shortname;

                CalcExtents();

                return true;
            }
            else
            {
                return false;
            }
        }

        public string Getnexttoken(ref string s, char seperator)
        {
            int c = 0;

            string token = "";

            if (s.Length == 0)
            {
                return null;
            }

            while (c < s.Length && s[c] != seperator)
            {
                token += s[c];

                c++;
            }

            c++;

            s = c == s.Length + 1 ? "" : s.Substring(c, s.Length - c);

            return token;
        }

        private string[] GetStrArrayFromTextFile(string filePath)
        {
            ArrayList arList = new ArrayList();

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
                            arList.Add(line);
                    }
                } while (line != null);

                sr.Close();

                fs.Close();
            }

            return (string[])arList.ToArray(Type.GetType("System.String"));
        }

        private void ReadItemList(string filePath)
        {
            string tok;

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

            while (line != null)
            {
                line = sr.ReadLine();

                if (line != null)
                {
                    line = line.Trim();

                    if (line.Length > 0 && !line.StartsWith("[") && !line.StartsWith("#"))
                    {
                        ListItem thisitem = new ListItem();

                        if ((tok = Getnexttoken(ref line, '=')) != null)
                        {
                            thisitem.ActorDef = tok.ToUpper();

                            if ((tok = Getnexttoken(ref line, ',')) != null)
                            {
                                thisitem.Name = tok;

                                // Remove the starting IT to get at ID number

                                string temp = thisitem.ActorDef.Remove(0, 2);

                                if ((tok = Getnexttoken(ref temp, '_')) != null)
                                {
                                    thisitem.ID = int.Parse(tok, NumFormat);

                                    // We got this far, so we have a valid item to add

                                    if (!itemList.ContainsKey(thisitem.ID))
                                    {
                                        try { itemList.Add(thisitem.ID, thisitem); }
                                        catch (Exception ex) { LogLib.WriteLine($"Error adding {thisitem.ID} to items hashtable: ", ex); }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            sr.Close();
            fs.Close();
        }

        private void ReadGuildList(string filePath)
        {
            string line = "";

            IFormatProvider NumFormat = new CultureInfo("en-US");

            if (!File.Exists(filePath))
            {
                // we did not find the Guild file
                LogLib.WriteLine("Guild file not found", LogLevel.Warning);
                return;
            }

            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader sr = new StreamReader(fs);

            while (line != null)
            {
                line = sr.ReadLine();

                if (line != null)
                {
                    line = line.Trim();

                    if (line.Length > 0 && (!line.StartsWith("[") && !line.StartsWith("#")))
                    {
                        ListItem thisitem = new ListItem();

                        string tok;
                        if ((tok = Getnexttoken(ref line, '=')) != null)
                        {
                            thisitem.ActorDef = tok.ToUpper();

                            if ((tok = Getnexttoken(ref line, ',')) != null)
                            {
                                thisitem.Name = tok;

                                if ((tok = Getnexttoken(ref thisitem.ActorDef, '_')) != null)
                                {
                                    thisitem.ID = int.Parse(tok, NumFormat);

                                    // We got this far, so we have a valid item to add

                                    if (!guildList.ContainsKey(thisitem.ID))
                                    {
                                        try { guildList.Add(thisitem.ID, thisitem); }
                                        catch (Exception ex) { LogLib.WriteLine("Error adding " + thisitem.ID + " to Guilds hashtable: ", ex); }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            sr.Close();
            fs.Close();
        }

        public string GetItemDescription(string ActorDef)
        {
            // Remove the starting IT to get at ID number
            string temp = ActorDef.Remove(0, 2);

            // Get description from list made using GroundItems.txt
            string tok;
            if ((tok = Getnexttoken(ref temp, '_')) != null)
            {
                IFormatProvider NumFormat = new CultureInfo("en-US");
                int lookupid = int.Parse(tok, NumFormat);

                // We got this far, so we have a valid item to add

                if (itemList.ContainsKey(lookupid))
                {
                    ListItem lis = (ListItem)itemList[lookupid];
                    return lis.Name;
                }
            }
            return ActorDef;
        }

        public string GetGuildDescription(string ActorDef)
        {
            // I know - ## NEEDS CLEANUP
            string temp = ActorDef;

            // Get description from list made using Guildlist.txt
            string tok;
            if ((tok = Getnexttoken(ref temp, '_')) != null)
            {
                IFormatProvider NumFormat = new CultureInfo("en-US");
                int lookupid = int.Parse(tok, NumFormat);

                // We got this far, so we have a valid item to add
                if (guildList.ContainsKey(lookupid))
                {
                    ListItem lis = (ListItem)guildList[lookupid];
                    return lis.Name;
                }
            }
            return ActorDef;
        }

        private string ArrayIndextoStr(string[] source, int index)
        {
            if (index < source.GetLowerBound(0) || index > source.GetUpperBound(0))
                return $"{index}: Unknown";
            else
                return source[index];
        }

        public void ClearMapStructures()
        {
            lines.Clear();
            texts.Clear();
            xlabels.Clear();
            ylabels.Clear();
            CalcExtents();
        }

        public void CalcExtents()
        {
            if (longname != "" && lines.Count > 0)
            {
                maxx = minx = ((MapLine)lines[0]).Point(0).x;

                maxy = miny = ((MapLine)lines[0]).Point(0).y;

                maxz = minz = ((MapLine)lines[0]).Point(0).z;

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

        public void CheckMobs(ListViewPanel SpawnList, ListViewPanel GroundItemList)
        {
            ArrayList deletedItems = new ArrayList();

            ArrayList delListItems = new ArrayList();

            // Increment the remove timers on all the ground spawns

            foreach (GroundItem sp in itemcollection)
            {
                if (sp.gone >= ditchGone) deletedItems.Add(sp);
                else sp.gone++;
            }

            // Remove any that have been marked for deletion
            if (deletedItems.Count > 0)
            {
                if (Zoning || deletedItems.Count > 5)
                    GroundItemList.listView.BeginUpdate();

                foreach (GroundItem gi in deletedItems)
                {
                    GroundItemList.listView.Items.Remove(gi.listitem);

                    gi.listitem = null;

                    itemcollection.Remove(gi);
                }

                if (Zoning || deletedItems.Count > 5)
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
                else if (sp.gone >= ditchGone)
                {
                    deletedItems.Add(sp);
                }
                else
                {
                    sp.gone++;
                }
            }

            // Remove any that have been marked for deletion

            if (deletedItems.Count > 0 || delListItems.Count > 0)
            {
                if (Zoning || deletedItems.Count > 5 || delListItems.Count > 5)
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

                if (Zoning || deletedItems.Count > 5 || delListItems.Count > 5)
                    SpawnList.listView.EndUpdate();

                delListItems.Clear();

                deletedItems.Clear();
            }
        }

        private void CheckMapMinMax(SPAWNINFO si)
        {
            if (Settings.Default.AutoExpand)
            {
                if ((minx > si.X) && (si.X > -20000)) minx = si.X;

                if ((maxx < si.X) && (si.X < 20000)) maxx = si.X;

                if ((miny > si.Y) && (si.Y > -20000)) miny = si.Y;

                if ((maxy < si.Y) && (si.Y < 20000)) maxy = si.Y;
            }
        }

        public void ProcessGroundItems(SPAWNINFO si, Filters filters)//, ListViewPanel GroundItemList)
        {
            try
            {
                bool found = false;

                foreach (GroundItem gi in itemcollection)
                {
                    if (gi.Name == si.Name && gi.X == si.X && gi.Y == si.Y && gi.Z == si.Z)
                    {
                        found = true;

                        gi.gone = 0;

                        break;
                    }
                }

                if (!found)
                {
                    GroundItem gi = new GroundItem
                    {
                        X = si.X,

                        Y = si.Y,

                        Z = si.Z,

                        Name = si.Name,

                        Desc = GetItemDescription(si.Name)
                    };

                    string itemname = gi.Desc.ToLower();

                    /* ************************************* *

                    * ************* ALERTS **************** *

                    * ************************************* */

                    // [hunt]

                    if (filters.Hunt.Count > 0 && FindMatches(filters.Hunt, itemname, Settings.Default.NoneOnHunt,

                            Settings.Default.TalkOnHunt, "Ground Item",

                            Settings.Default.PlayOnHunt, Settings.Default.HuntAudioFile,

                            Settings.Default.BeepOnHunt, MatchFullTextH))
                    {
                        gi.isHunt = true;
                    }

                    if (filters.GlobalHunt.Count > 0 && FindMatches(filters.GlobalHunt, itemname, Settings.Default.NoneOnHunt,

                            Settings.Default.TalkOnHunt, "Ground Item",

                            Settings.Default.PlayOnHunt, Settings.Default.HuntAudioFile,

                            Settings.Default.BeepOnHunt, MatchFullTextH))
                    {
                        gi.isHunt = true;
                    }

                    // [caution]

                    if (filters.Caution.Count > 0 && FindMatches(filters.Caution, itemname, Settings.Default.NoneOnCaution,

                            Settings.Default.TalkOnCaution, "Ground Item",

                            Settings.Default.PlayOnCaution, Settings.Default.CautionAudioFile,

                            Settings.Default.BeepOnCaution, MatchFullTextC))
                    {
                        gi.isCaution = true;
                    }

                    if (filters.GlobalCaution.Count > 0 && FindMatches(filters.GlobalCaution, itemname, Settings.Default.NoneOnCaution,

                            Settings.Default.TalkOnCaution, "Ground Item",

                            Settings.Default.PlayOnCaution, Settings.Default.CautionAudioFile,

                            Settings.Default.BeepOnCaution, MatchFullTextC))
                    {
                        gi.isCaution = true;
                    }

                    // [danger]

                    if (filters.Danger.Count > 0 && FindMatches(filters.Danger, itemname, Settings.Default.NoneOnDanger,

                            Settings.Default.TalkOnDanger, "Ground Item",

                            Settings.Default.PlayOnDanger, Settings.Default.DangerAudioFile,

                            Settings.Default.BeepOnDanger, MatchFullTextD))
                    {
                        gi.isDanger = true;
                    }

                    if (filters.GlobalDanger.Count > 0 && FindMatches(filters.GlobalDanger, itemname, Settings.Default.NoneOnDanger,

                            Settings.Default.TalkOnDanger, "Ground Item",

                            Settings.Default.PlayOnDanger, Settings.Default.DangerAudioFile,

                            Settings.Default.BeepOnDanger, MatchFullTextD))
                    {
                        gi.isDanger = true;
                    }

                    // [rare]

                    if (filters.Alert.Count > 0 && FindMatches(filters.Alert, itemname, Settings.Default.NoneOnAlert,

                            Settings.Default.TalkOnAlert, "Ground Item",

                            Settings.Default.PlayOnAlert, Settings.Default.AlertAudioFile,

                            Settings.Default.BeepOnAlert, MatchFullTextA))
                    {
                        gi.isAlert = true;
                    }

                    if (filters.GlobalAlert.Count > 0 && FindMatches(filters.GlobalAlert, itemname, Settings.Default.NoneOnAlert,

                            Settings.Default.TalkOnAlert, "Ground Item",

                            Settings.Default.PlayOnAlert, Settings.Default.AlertAudioFile,

                            Settings.Default.BeepOnAlert, MatchFullTextA))
                    {
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

                    itemcollection.Add(gi);

                    // Add it to the ground item list
                    newGroundItems.Add(item1);
                }
            }
            catch (Exception ex) { LogLib.WriteLine("Error in ProcessGroundItems(): ", ex); }
        }

        public void ProcessTarget(SPAWNINFO si)
        {
            try
            {
                if (Settings.Default.AutoSelectEQTarget && EQSelectedID != (int)si.SpawnID)
                {
                    EQSelectedID = (int)si.SpawnID;

                    selectedID = (int)si.SpawnID;

                    SpawnX = -1.0f;

                    SpawnY = -1.0f;

                    foreach (SPAWNINFO sp in mobs.Values)
                    {
                        if (sp.SpawnID == EQSelectedID)
                        {
                            if (Settings.Default.AutoSelectSpawnList)
                            {
                                sp.listitem.EnsureVisible();

                                sp.listitem.Selected = true;
                            }

                            break;
                        }
                    }
                }
            }
            catch (Exception ex) { LogLib.WriteLine("Error in ProcessTarget(): ", ex); }
        }

        public void ProcessWorld(SPAWNINFO si)
        {
            try
            {
                int gameDay = si.Level;

                int gameHour = si.Type - 1;

                int gameMin = si.Class;

                int gameMonth = si.Hide;

                int gameYear = si.Race;

                gametime = new DateTime(gameYear, gameMonth, gameDay, gameHour, gameMin, 0);
            }
            catch (Exception ex) { LogLib.WriteLine("Error in ProcessWorld(): ", ex); }
        }

        public void ProcessSpawns(SPAWNINFO si, FrmMain f1, ListViewPanel SpawnList, Filters filters, MapPane mapPane, bool update_hidden)
        {
            CorpseAlerts = Settings.Default.CorpseAlerts;
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
                                        si.hidden = !Settings.Default.ShowMyCorpse;
                                    }
                                    else
                                    {
                                        // Other Players Corpses

                                        si.hidden = !Settings.Default.ShowPCCorpses;
                                    }
                                }
                                else
                                {
                                    si.hidden = !Settings.Default.ShowCorpses;
                                }
                            }
                            else if (mob.m_isPlayer)
                            {
                                si.hidden = !Settings.Default.ShowPlayers;
                            }
                            else
                            {
                                // non-corpse, non-player spawn (aka NPC)

                                if (!Settings.Default.ShowNPCs) // hides all NPCs
                                {
                                    si.hidden = true;
                                }
                                else
                                {
                                    si.hidden = false;

                                    if (si.isEventController && !Settings.Default.ShowInvis) // Invis Men

                                        si.hidden = true;
                                    else if (mob.isMount && !Settings.Default.ShowMounts) // Mounts

                                        si.hidden = true;
                                    else if (mob.isPet && !Settings.Default.ShowPets) // Pets

                                        si.hidden = true;
                                    else if (mob.isFamiliar && !Settings.Default.ShowFamiliars) // Familiars

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

                            li.SubItems[8].Text = PrettyNames.GetSpawnType(si.Type);

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
                                {
                                    li.ForeColor = Color.Red;
                                }
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
                            string newname = RegexHelper.FixMobName(si.Name);

                            string oldname = RegexHelper.FixMobName(mob.Name);

                            // use replace so that we dont loose the alert prefixes.

                            li.Text = li.Text.Replace(oldname, newname);

                            if (!si.IsPlayer() && (si.Type == 2 || si.Type == 3))
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
                                {
                                    li.ForeColor = Color.Red;
                                }
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

                            li.SubItems[2].Text = ClassNumToString(si.Class);
                        }

                        if (mob.Primary != si.Primary)
                        {
                            mob.Primary = si.Primary;

                            li.SubItems[3].Text = si.Primary > 0 ? ItemNumToString(si.Primary) : "";
                        }

                        if (mob.Offhand != si.Offhand)
                        {
                            mob.Offhand = si.Offhand;

                            li.SubItems[4].Text = si.Offhand > 0 ? ItemNumToString(si.Offhand) : "";
                        }

                        if (mob.Race != si.Race)
                        {
                            mob.Race = si.Race;

                            li.SubItems[5].Text = RaceNumtoString(si.Race);
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
                                li.SubItems[6].Text = RegexHelper.FixMobName(owner.Name);
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

                            li.SubItems[9].Text = PrettyNames.GetHideStatus(si.Hide);
                        }

                        if (mob.Guild != si.Guild)
                        {
                            mob.Guild = si.Guild;

                            if (si.Guild > 0)
                                li.SubItems[17].Text = GuildNumToString(si.Guild);
                            else
                                li.SubItems[17].Text = "";
                        }

                        mob.refresh = 0;
                    } // end refresh > 10

                    mob.refresh++;

                    // Set variables we dont want to trigger list update

                    if (selectedID != (int)mob.SpawnID)
                    {
                        if (mob.X != si.X)
                        {
                            // ensure that map is big enough to show all spawns.

                            if (mapPane?.scale.Value == 100M)
                                CheckMapMinMax(si);

                            mob.X = si.X;

                            mob.Y = si.Y;
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

                        if (mapPane?.scale.Value == 100M)
                            CheckMapMinMax(si);

                        mob.X = si.X;

                        li.SubItems[14].Text = si.Y.ToString();

                        mob.Y = si.Y;

                        li.SubItems[13].Text = si.X.ToString();

                        mob.Z = si.Z;

                        li.SubItems[15].Text = si.Z.ToString();

                        float sd = (float)Math.Sqrt(((si.X - playerinfo.X) * (si.X - playerinfo.X)) +

                            ((si.Y - playerinfo.Y) * (si.Y - playerinfo.Y)) +

                            ((si.Z - playerinfo.Z) * (si.Z - playerinfo.Z)));

                        if (Settings.Default.FollowOption == FollowOption.Target)
                            f1.ReAdjust();

                        li.SubItems[16].Text = sd.ToString("#.000");
                    }

                    if (listReAdd) newSpawns.Add(li);
                } // end of if found

                // If it's not already in there, add it

                if (!found && si.Name.Length > 0)
                {
                    bool alert = false;

                    // ensure that map is big enough to show all spawns.

                    if (mapPane?.scale.Value == 100M)
                        CheckMapMinMax(si);

                    // Set mob type info

                    if (si.Type == 0)
                    {
                        // Players

                        si.m_isPlayer = true;

                        if (!Settings.Default.ShowPlayers) si.hidden = true;
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

                                if (!Settings.Default.ShowPCCorpses) si.hidden = true;

                                if (si.Name.Length > 0 && CheckMyCorpse(si.Name))
                                {
                                    si.m_isMyCorpse = true;

                                    si.hidden = !Settings.Default.ShowMyCorpse;
                                }
                            }
                            else
                            {
                                if (!Settings.Default.ShowCorpses) si.hidden = true;
                            }
                        }
                        else
                        {
                            // non-corpse, non-player spawn (aka NPC)

                            if (!Settings.Default.ShowNPCs) si.hidden = true;

                            if (si.OwnerID > 0)
                            {
                                SPAWNINFO owner;

                                if (mobs.ContainsKey(si.OwnerID))
                                {
                                    owner = (SPAWNINFO)mobs[si.OwnerID];
                                    if (owner.IsPlayer())
                                    {
                                        si.isPet = true;
                                        if (!Settings.Default.ShowPets) si.hidden = true;
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
                                if (!Settings.Default.ShowInvis)
                                    si.hidden = true;
                            }
                            else if (si.Class == 62)
                            {
                                si.isLDONObject = true;
                            }

                            // Mercenary Identification - Only do it once now

                            if (!string.IsNullOrEmpty(si.Lastname))
                            {
                                if (RegexHelper.IsMerc(si.Lastname))
                                    si.isMerc = true;
                            }
                            else if (RegexHelper.IsMount(si.Name)) // Mounts
                            {
                                si.isMount = true;

                                if (!Settings.Default.ShowMounts) si.hidden = true;
                            }
                            else if (RegexHelper.IsFamiliar(si.Name))
                            {
                                // reset these, if match a familiar
                                si.isPet = false;
                                si.hidden = false;

                                si.isFamiliar = true;

                                if (!Settings.Default.ShowFamiliars) si.hidden = true;
                            }
                        }
                    }

                    mobsTimers.Spawn(si);

                    if (si.Name.Length > 0)
                    {
                        string mobname = si.isMerc ? RegexHelper.FixMobNameMatch(si.Name) : RegexHelper.FixMobName(si.Name);

                        string matchmobname = RegexHelper.FixMobNameMatch(mobname);

                        if (matchmobname.Length < 2)
                            matchmobname = mobname;

                        string mobnameWithInfo = mobname;

                        string primaryName = "";

                        if (si.Primary > 0 || si.Offhand > 0)
                            primaryName = ItemNumToString(si.Primary);

                        string offhandName = "";

                        if (si.Offhand > 0)
                            offhandName = ItemNumToString(si.Offhand);

                        // Don't do alert matches for controllers, Ldon objects, pets, mercs, mounts, or familiars
                        if (!(si.isLDONObject || si.isEventController || si.isFamiliar || si.isMount || (si.isMerc && si.OwnerID != 0)))
                        {
                            /* ************************************* *
                            * ************* ALERTS **************** *
                            * ************************************* */

                            // [hunt]

                            if (filters.Hunt.Count > 0 && (!si.isCorpse || CorpseAlerts) && FindMatches(filters.Hunt, matchmobname, Settings.Default.NoneOnHunt,

                                    Settings.Default.TalkOnHunt, "Hunt Mob",
                                    Settings.Default.PlayOnHunt, Settings.Default.HuntAudioFile,
                                    Settings.Default.BeepOnHunt, MatchFullTextH))
                            {
                                alert = true;
                                if (PrefixStars)
                                {
                                    mobnameWithInfo = HuntPrefix + " " + mobnameWithInfo;
                                }

                                if (AffixStars)
                                {
                                    mobnameWithInfo += " " + HuntPrefix;
                                }

                                si.isHunt = true;
                            }
                            if (filters.GlobalHunt.Count > 0 && !alert && (!si.isCorpse || CorpseAlerts) && FindMatches(filters.GlobalHunt, matchmobname, Settings.Default.NoneOnHunt,

                                    Settings.Default.TalkOnHunt, "Hunt Mob",
                                    Settings.Default.PlayOnHunt, Settings.Default.HuntAudioFile,
                                    Settings.Default.BeepOnHunt, MatchFullTextH))
                            {
                                alert = true;
                                if (PrefixStars)
                                {
                                    mobnameWithInfo = HuntPrefix + " " + mobnameWithInfo;
                                }

                                if (AffixStars)
                                {
                                    mobnameWithInfo += " " + HuntPrefix;
                                }
                                si.isHunt = true;
                            }

                            // [caution]
                            if (filters.Caution.Count > 0 && !alert && (!si.isCorpse || CorpseAlerts) && FindMatches(filters.Caution, matchmobname, Settings.Default.NoneOnCaution,

                                    Settings.Default.TalkOnCaution, "Caution Mob",
                                    Settings.Default.PlayOnCaution, Settings.Default.CautionAudioFile,
                                    Settings.Default.BeepOnCaution, MatchFullTextC))
                            {
                                alert = MarkCaution(ref mobnameWithInfo);
                                si.isCaution = true;
                            }
                            if (filters.GlobalCaution.Count > 0 && !alert && (!si.isCorpse || CorpseAlerts) && FindMatches(filters.GlobalCaution, matchmobname, Settings.Default.NoneOnCaution,

                                    Settings.Default.TalkOnCaution, "Caution Mob",
                                    Settings.Default.PlayOnCaution, Settings.Default.CautionAudioFile,
                                    Settings.Default.BeepOnCaution, MatchFullTextC))
                            {
                                alert = true;

                                si.isCaution = true;

                                if (PrefixStars)
                                    mobnameWithInfo = CautionPrefix + " " + mobnameWithInfo;

                                if (AffixStars)
                                    mobnameWithInfo += " " + CautionPrefix;
                            }

                            // [danger]
                            if (filters.Danger.Count > 0 && !alert && (!si.isCorpse || CorpseAlerts) && FindMatches(filters.Danger, matchmobname, Settings.Default.NoneOnDanger,

                                    Settings.Default.TalkOnDanger, "Danger Mob",

                                    Settings.Default.PlayOnDanger, Settings.Default.DangerAudioFile,

                                    Settings.Default.BeepOnDanger, MatchFullTextD))
                            {
                                alert = MarkDanger(ref mobnameWithInfo);
                                si.isDanger = true;
                            }
                            if (filters.GlobalDanger.Count > 0 && !alert && (!si.isCorpse || CorpseAlerts) && FindMatches(filters.GlobalDanger, matchmobname, Settings.Default.NoneOnDanger,

                                    Settings.Default.TalkOnDanger, "Danger Mob",

                                    Settings.Default.PlayOnDanger, Settings.Default.DangerAudioFile,

                                    Settings.Default.BeepOnDanger, MatchFullTextD))
                            {
                                alert = MarkDanger(ref mobnameWithInfo);
                                si.isDanger = true;
                            }

                            // [rare]
                            if (filters.Alert.Count > 0 && !alert && (!si.isCorpse || CorpseAlerts) && FindMatches(filters.Alert, matchmobname, Settings.Default.NoneOnAlert,
                                    Settings.Default.TalkOnAlert, "Rare Mob",
                                    Settings.Default.PlayOnAlert, Settings.Default.AlertAudioFile,
                                    Settings.Default.BeepOnAlert, MatchFullTextA))
                            {
                                alert = true;

                                si.isAlert = true;

                                if (PrefixStars)
                                    mobnameWithInfo = AlertPrefix + " " + mobnameWithInfo;

                                if (AffixStars)
                                    mobnameWithInfo += " " + AlertPrefix;
                            }
                            if (filters.GlobalAlert.Count > 0 && !alert && (!si.isCorpse || CorpseAlerts) && FindMatches(filters.GlobalAlert, matchmobname, Settings.Default.NoneOnAlert,
                                    Settings.Default.TalkOnAlert, "Rare Mob",
                                    Settings.Default.PlayOnAlert, Settings.Default.AlertAudioFile,
                                    Settings.Default.BeepOnAlert, MatchFullTextA))
                            {
                                alert = true;
                                if (PrefixStars)
                                {
                                    mobnameWithInfo = AlertPrefix + " " + mobnameWithInfo;
                                }
                                if (AffixStars)
                                {
                                    mobnameWithInfo += " " + AlertPrefix;
                                }

                                si.isAlert = true;
                            }
                            // [Email]
                            if (filters.EmailAlert.Count > 0 && !si.isCorpse && FindMatches(filters.EmailAlert, matchmobname, false, false, "", false, "", !si.isAlert && !si.isCaution && !si.isDanger && !si.isHunt, true))
                            {
                                alert = true;
                                // Flag on map as an alert mob
                                si.isAlert = true;
                            }

                            // [Wielded Items]
                            // Acts like a hunt mob.
                            if (filters.WieldedItems.Count > 0 && (!si.isCorpse || CorpseAlerts) && FindMatches(filters.WieldedItems, primaryName, Settings.Default.NoneOnHunt,
                                    Settings.Default.TalkOnHunt, "Hunt Mob Wielded",
                                    Settings.Default.PlayOnHunt, Settings.Default.HuntAudioFile,
                                    Settings.Default.BeepOnHunt, MatchFullTextH))
                            {
                                alert = true;
                                if (PrefixStars)
                                {
                                    mobnameWithInfo = HuntPrefix + "W" + mobnameWithInfo;
                                }

                                if (AffixStars)
                                {
                                    mobnameWithInfo += " " + HuntPrefix;
                                }

                                si.isHunt = true;
                            }

                            // [Offhand]
                            // Acts like a hunt mob.
                            if (filters.WieldedItems.Count > 0 && (!si.isCorpse || CorpseAlerts) && FindMatches(filters.WieldedItems, offhandName,
                                Settings.Default.NoneOnHunt,
                                    Settings.Default.TalkOnHunt, "Hunt Mob Wielded",
                                    Settings.Default.PlayOnHunt, Settings.Default.HuntAudioFile,
                                    Settings.Default.BeepOnHunt, MatchFullTextH))
                            {
                                alert = true;
                                if (PrefixStars)
                                {
                                    mobnameWithInfo = HuntPrefix + "O " + mobnameWithInfo;
                                }
                                if (AffixStars)
                                {
                                    mobnameWithInfo += " " + HuntPrefix;
                                }
                                si.isAlert = true;
                            }

                            LookupBoxMatch(si, f1);
                        }

                        ListViewItem item1 = new ListViewItem(mobnameWithInfo);

                        item1.SubItems.Add(si.Level.ToString());

                        item1.SubItems.Add(ClassNumToString(si.Class));

                        if (si.Primary > 0)
                            item1.SubItems.Add(ItemNumToString(si.Primary));
                        else
                            item1.SubItems.Add("");

                        if (si.Offhand > 0)
                            item1.SubItems.Add(ItemNumToString(si.Offhand));
                        else
                            item1.SubItems.Add("");

                        item1.SubItems.Add(RaceNumtoString(si.Race));

                        OwnerFlag(si, item1);

                        item1.SubItems.Add(si.Lastname);

                        item1.SubItems.Add(PrettyNames.GetSpawnType(si.Type));

                        item1.SubItems.Add(PrettyNames.GetHideStatus(si.Hide));

                        item1.SubItems.Add(si.SpeedRun.ToString());

                        item1.SubItems.Add(si.SpawnID.ToString());

                        item1.SubItems.Add(DateTime.Now.ToLongTimeString());

                        item1.SubItems.Add(si.X.ToString("#.000"));

                        item1.SubItems.Add(si.Y.ToString("#.000"));

                        item1.SubItems.Add(si.Z.ToString("#.000"));

                        float sd = (float)Math.Sqrt(((si.X - playerinfo.X) * (si.X - playerinfo.X)) +

                            ((si.Y - playerinfo.Y) * (si.Y - playerinfo.Y)) +

                            ((si.Z - playerinfo.Z) * (si.Z - playerinfo.Z)));

                        item1.SubItems.Add(sd.ToString("#.000"));

                        item1.SubItems.Add(GuildNumToString(si.Guild));

                        item1.SubItems.Add(RegexHelper.FixMobName(si.Name));

                        if (si.Type == 2 || si.Type == 3 || si.isLDONObject)
                        {
                            item1.ForeColor = Color.Gray;
                        }
                        else if (si.isEventController)
                        {
                            item1.ForeColor = Color.DarkOrchid;
                        }
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
                            item1.Font = Settings.Default.ListFont;

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

            bool MarkCaution(ref string mobnameWithInfo)
            {
                var alert = true;
                si.isCaution = true;

                if (PrefixStars)
                    mobnameWithInfo = CautionPrefix + " " + mobnameWithInfo;

                if (AffixStars)
                    mobnameWithInfo += " " + CautionPrefix;
                return alert;
            }

            bool MarkDanger(ref string mobnameWithInfo)
            {
                var alert = true;

                if (PrefixStars)
                    mobnameWithInfo = DangerPrefix + " " + mobnameWithInfo;

                if (AffixStars)
                    mobnameWithInfo += " " + DangerPrefix;
                return alert;
            }
        }

        private static void LookupBoxMatch(SPAWNINFO si, FrmMain f1)
        {
            si.isLookup = false;
            if (f1.toolStripLookupBox.Text.Length > 1
                && f1.toolStripLookupBox.Text != dflt
                && RegexHelper.GetRegex(f1.toolStripLookupBox.Text).Match(si.Name).Success)
            {
                si.isLookup = true;
            }

            if (f1.toolStripLookupBox1.Text.Length > 1
                && f1.toolStripLookupBox1.Text != dflt
                && RegexHelper.GetRegex(f1.toolStripLookupBox1.Text).Match(si.Name).Success)
            {
                si.isLookup = true;
            }

            if (f1.toolStripLookupBox2.Text.Length > 1
                && f1.toolStripLookupBox2.Text != dflt
                && RegexHelper.GetRegex(f1.toolStripLookupBox2.Text).Match(si.Name).Success)
            {
                si.isLookup = true;
            }

            if (f1.toolStripLookupBox3.Text.Length > 1
                && f1.toolStripLookupBox3.Text != dflt
                && RegexHelper.GetRegex(f1.toolStripLookupBox3.Text).Match(si.Name).Success)
            {
                si.isLookup = true;
            }

            if (f1.toolStripLookupBox4.Text.Length > 1
                && f1.toolStripLookupBox4.Text != dflt
                && RegexHelper.GetRegex(f1.toolStripLookupBox4.Text).Match(si.Name).Success)
            {
                si.isLookup = true;
            }

            if (f1.toolStripLookupBox5.Text.Length > 1
                && f1.toolStripLookupBox5.Text != dflt
                && RegexHelper.GetRegex(f1.toolStripLookupBox5.Text).Match(si.Name).Success)
            {
                si.isLookup = true;
            }
        }

        private void OwnerFlag(SPAWNINFO si, ListViewItem item1)
        {
            if (si.OwnerID > 0)
            {
                if (mobs.ContainsKey(si.OwnerID))
                {
                    SPAWNINFO owner = (SPAWNINFO)mobs[si.OwnerID];
                    item1.SubItems.Add(RegexHelper.FixMobName(owner.Name));
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
                        {
                            li.ForeColor = Color.Gray;
                        }
                        else if (si.isEventController)
                        {
                            li.ForeColor = Color.DarkOrchid;
                        }
                        else
                        {
                            li.ForeColor = conColors[si.Level].Color;

                            if (li.ForeColor == Color.Maroon)
                                li.ForeColor = Color.Red;

                            // Change the colors to be more visible on white if the background is white

                            if (Settings.Default.ListBackColor == Color.White)
                            {
                                if (li.ForeColor == Color.White)
                                    li.ForeColor = Color.Black;
                                else if (li.ForeColor == Color.Yellow)
                                    li.ForeColor = Color.Goldenrod;
                            }

                            if (Settings.Default.ListBackColor == Color.Black && li.ForeColor == Color.Black)
                            {
                                li.ForeColor = Color.White;
                            }
                        }
                    }
                }
            }
        }

        private bool FindMatches(ArrayList exps, string mobname, bool NoneOnMatch,
             bool TalkOnMatch, string TalkDescr, bool PlayOnMatch, string AudioFile,
             bool BeepOnMatch, bool MatchFullText)
        {
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
                else if (RegexHelper.IsSubstring(mobname, str))
                {
                    matched = true;
                }
                // if item has been matched...

                if (matched)
                {
                    if (!NoneOnMatch && playAlerts)
                    {
                        AudioMatch(mobname, TalkOnMatch, TalkDescr, PlayOnMatch, AudioFile, BeepOnMatch);
                    }

                    alert = true;

                    break;
                }
            }

            return alert;
        }

        private static void AudioMatch(string mobname, bool TalkOnMatch, string TalkDescr, bool PlayOnMatch, string AudioFile, bool BeepOnMatch)
        {
            if (TalkOnMatch)
            {
                ThreadStart threadDelegate = new ThreadStart(new Talker
                {
                    speakText = $"{TalkDescr}, {RegexHelper.FixMobNameMatch(mobname)}, is up."
                }.SpeakText);

                new Thread(threadDelegate).Start();
            }
            else if (PlayOnMatch)
            {
                SAudio.Play(AudioFile.Replace("\\", "\\\\"));
            }
            else if (BeepOnMatch)
            {
                SafeNativeMethods.Beep(300, 100);
            }
        }

        public bool CheckMyCorpse(string mobname)
        {
            return (mobname.Length < (playerinfo.Name.Length + 14)) && (mobname.IndexOf(playerinfo.Name) == 0);
        }

        public void SaveMobs()
        {
            DateTime dt = DateTime.Now;

            string filename = $"{shortname} - ";

            filename += $"{dt.Month}-{dt.Day}-{dt.Year} {dt.Hour}-{dt.Minute}.txt";

            StreamWriter sw = new StreamWriter(filename, false);

            sw.Write("Name	Level	Class	Race	Lastname	Type	Invis	Run Speed	SpawnID	X	Y	Z	Heading");

            foreach (SPAWNINFO si in mobs.Values)
            {
                sw.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}",
                             si.Name,
                             si.Level,
                             ClassNumToString(si.Class),
                             RaceNumtoString(si.Race),
                             si.Lastname,
                             PrettyNames.GetSpawnType(si.Type),
                             PrettyNames.GetHideStatus(si.Hide),
                             si.SpeedRun,
                             si.SpawnID,
                             si.Y,
                             si.X,
                             si.Z,
                             CalcRealHeading(si));
            }

            sw.Close();
        }

        public void SetSelectedID(int id)
        {
            selectedID = id;

            SpawnX = -1.0f;

            SpawnY = -1.0f;
        }

        public void SetSelectedTimer(float x, float y)
        {
            SPAWNTIMER st = FindTimer(1.0f, x, y);

            if (st != null)
            {
                SPAWNINFO sp = FindMobTimer(st.SpawnLoc);

                selectedID = sp == null ? 99999 : (int)sp.SpawnID;

                SpawnX = st.X;

                SpawnY = st.Y;
            }
        }

        public void SetSelectedGroundItem(float x, float y)
        {
            GroundItem gi = FindGroundItemNoFilter(1.0f, x, y);

            if (gi != null)
            {
                selectedID = 99999;

                SpawnX = gi.X;

                SpawnY = gi.Y;
            }
        }

        public string ClassNumToString(int num)
        {
            return ArrayIndextoStr(Classes, num);
        }

        public string ItemNumToString(int num)
        {
            if (itemList.ContainsKey(num))
            {
                ListItem lis = (ListItem)itemList[num];

                return lis.Name;
            }
            else
            {
                return num.ToString();
            }
        }

        public string GuildNumToString(int num)
        {
            if (guildList.ContainsKey(num))
            {
                ListItem lis = (ListItem)guildList[num];

                return lis.Name;
            }
            else
            {
                if (num == 0) return "";

                return num.ToString();
            }
        }

        public string RaceNumtoString(int num)
        {
            if (num == 2250)
                return "Interactive Object";

            return ArrayIndextoStr(Races, num);
        }

        public void BeginProcessPacket()
        {
            newSpawns.Clear();

            newGroundItems.Clear();
        }

        public void ProcessSpawnList(ListViewPanel SpawnList)
        {
            try
            {
                if (newSpawns.Count > 0)
                {
                    if (Zoning)
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
                    if (Zoning)
                        SpawnList.listView.EndUpdate();
                }
            }
            catch (Exception ex) { LogLib.WriteLine("Error in ProcessSpawnList(): ", ex); }
        }

        public void ProcessGroundItemList(ListViewPanel GroundItemList)
        {
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
        }

        public float CalcRealHeading(SPAWNINFO sp)
        {
            try
            {
                if (sp.Heading >= 0 && sp.Heading < 512)
                    return sp.Heading / 512 * 360;
                else
                    return 0;
            }
            catch (Exception ex)
            {
                LogLib.WriteLine("Error with CalcRealHeading: ", ex);

                return 0;
            }
        }

        public void LoadSpawnInfo()
        {
            // Used to improve packet processing speed

            PrefixStars = Settings.Default.PrefixStars;

            AffixStars = Settings.Default.AffixStars;

            CorpseAlerts = Settings.Default.CorpseAlerts;

            MatchFullTextH = Settings.Default.MatchFullTextH;

            MatchFullTextC = Settings.Default.MatchFullTextC;

            MatchFullTextD = Settings.Default.MatchFullTextD;

            MatchFullTextA = Settings.Default.MatchFullTextA;

            HuntPrefix = Settings.Default.HuntPrefix;

            CautionPrefix = Settings.Default.CautionPrefix;

            DangerPrefix = Settings.Default.DangerPrefix;

            AlertPrefix = Settings.Default.AlertPrefix;
        }

        #region ProcessPlayer

        public void ProcessPlayer(SPAWNINFO si, FrmMain f1)
        {
            try
            {
                playerinfo.SpawnID = si.SpawnID;

                if (playerinfo.Name != si.Name || playerinfo.Name?.Length == 0)
                {
                    playerinfo.Name = si.Name;
                    f1.SetCharSelection(playerinfo.Name);
                    f1.SetTitle();
                }

                playerinfo.Lastname = si.Lastname;

                if ((playerinfo.X != si.X) || (playerinfo.Y != si.Y))
                {
                    playerinfo.X = si.X;

                    playerinfo.Y = si.Y;

                    if (Settings.Default.FollowOption == FollowOption.Player)
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
                        if (f1.gconLevel > 115)
                            f1.gconLevel = 115;
                        if (f1.gconLevel < 1)
                            f1.gconLevel = -1;
                        if (f1.gconLevel == -1)
                        {
                            f1.gConBaseName = "";
                        }
                        f1.gLastconLevel = f1.gconLevel;
                        Settings.Default.LevelOverride = f1.gconLevel;
                    }
                    playerinfo.Level = si.Level;
                    FillConColors(f1);

                    // update mob list con colors

                    UpdateMobListColors();
                }
                if (f1.gLastconLevel != f1.gconLevel)
                {
                    f1.gLastconLevel = f1.gconLevel;
                    FillConColors(f1);
                    UpdateMobListColors();
                }
            }
            catch (Exception ex) { LogLib.WriteLine("Error in ProcessPlayer(): ", ex); }
        }

        #endregion ProcessPlayer

        public void FillConColors(FrmMain f1)
        {
            try
            {
                int c = 0;

                int level;

                if (Settings.Default.LevelOverride == -1)
                {
                    f1.toolStripLevel.Text = "Auto";
                    level = playerinfo.Level;
                }
                else
                {
                    level = Settings.Default.LevelOverride;
                    f1.toolStripLevel.Text = level.ToString();
                }
                YellowRange = 3;

                CyanRange = -5;

                GreenRange = (-1) * level;

                GreyRange = (-1) * level;

                // If using SoD/Titanium Con Colors
                VersionColorVariation(level);

                // Set the Grey Cons
                for (c = 0; c < (GreyRange + level); c++)
                {
                    conColors[c] = new SolidBrush(Color.Gray);
                }

                // Set the Green Cons
                for (; c < (GreenRange + level); c++)
                {
                    conColors[c] = new SolidBrush(Color.Lime);
                }

                // Set the Light Blue Cons
                for (; c < (CyanRange + level); c++)
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
                for (; c < (level + YellowRange + 1); c++)
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

        private void VersionColorVariation(int level)
        // Check for SoD, SoF or Real EQ con levels in use
        {
            var ConColorsFile = Path.Combine(Settings.Default.CfgDir, "ConLevels.Ini");
            if (Settings.Default.SoDCon)
            {
                YellowRange = 2;

                GreyRange = (-1) * level;

                if (level < 9)
                {
                    GreenRange = -3;

                    CyanRange = -7;
                }
                else if (level < 13)
                {
                    GreenRange = -5;

                    CyanRange = -3;
                }
                else if (level < 17)
                {
                    GreenRange = -6;

                    CyanRange = -4;
                }
                else if (level < 21)
                {
                    GreenRange = -7;

                    CyanRange = -5;
                }
                else if (level < 25)
                {
                    GreenRange = -8;

                    CyanRange = -6;
                }
                else if (level < 29)
                {
                    GreenRange = -9;

                    CyanRange = -7;
                }
                else if (level < 31)
                {
                    GreenRange = -10;

                    CyanRange = -8;
                }
                else if (level < 33)
                {
                    GreenRange = -11;

                    CyanRange = -8;
                }
                else if (level < 37)
                {
                    GreenRange = -12;

                    CyanRange = -9;
                }
                else if (level < 41)
                {
                    GreenRange = -13;

                    CyanRange = -10;
                }
                else if (level < 45)
                {
                    GreenRange = -15;

                    CyanRange = -11;
                }
                else if (level < 49)
                {
                    GreenRange = -16;

                    CyanRange = -12;
                }
                else if (level < 53)
                {
                    GreenRange = -17;

                    CyanRange = -13;
                }
                else if (level < 55)
                {
                    GreenRange = -18;

                    CyanRange = -14;
                }
                else if (level < 57)
                {
                    GreenRange = -19;

                    CyanRange = -14;
                }
                else
                {
                    GreenRange = -20;

                    CyanRange = -15;
                }
            }

            // If using SoF Con Colors
            else if (Settings.Default.SoFCon)
            {
                YellowRange = 3;

                CyanRange = -5;

                if (level < 9)
                {
                    GreyRange = -3;

                    GreenRange = -7;
                }
                else if (level < 10)
                {
                    GreyRange = -4;

                    GreenRange = -3;
                }
                else if (level < 13)
                {
                    GreyRange = -5;

                    GreenRange = -3;
                }
                else if (level < 17)
                {
                    GreyRange = -6;

                    GreenRange = -4;
                }
                else if (level < 21)
                {
                    GreyRange = -7;

                    GreenRange = -5;
                }
                else if (level < 25)
                {
                    GreyRange = -8;

                    GreenRange = -6;
                }
                else if (level < 29)
                {
                    GreyRange = -9;

                    GreenRange = -7;
                }
                else if (level < 31)
                {
                    GreyRange = -10;

                    GreenRange = -8;
                }
                else if (level < 33)
                {
                    GreyRange = -11;

                    GreenRange = -8;
                }
                else if (level < 37)
                {
                    GreyRange = -12;

                    GreenRange = -9;
                }
                else if (level < 41)
                {
                    GreyRange = -13;

                    GreenRange = -10;
                }
                else if (level < 45)
                {
                    GreyRange = -15;

                    GreenRange = -11;
                }
                else if (level < 49)
                {
                    GreyRange = -16;

                    GreenRange = -12;
                }
                else if (level < 53)
                {
                    GreyRange = -17;

                    GreenRange = -13;
                }
                else if (level < 55)
                {
                    GreyRange = -18;

                    GreenRange = -14;
                }
                else if (level < 57)
                {
                    GreyRange = -19;

                    GreenRange = -14;
                }
                else
                {
                    GreyRange = -20;

                    GreenRange = -15;
                }
            }
            else if (File.Exists(ConColorsFile))
            {
                IniFile Ini = new IniFile(ConColorsFile);

                string sIniValue = Ini.ReadValue("Con Levels", level.ToString(), "0/0/0");
                var yellowLevels = Ini.ReadValue("Con Levels", "0", "3");
                string[] ConLevels = sIniValue.Split('/');

                GreyRange = Convert.ToInt32(ConLevels[0]) - level + 1;

                GreenRange = Convert.ToInt32(ConLevels[1]) - level + 1;

                CyanRange = Convert.ToInt32(ConLevels[2]) - level + 1;

                YellowRange = Convert.ToInt32(yellowLevels);
            }
            else if (Settings.Default.DefaultCon)
            {
                // Using Default Con Colors

                CyanRange = -5;

                if (level < 16) // verified
                {
                    GreyRange = -5;

                    GreenRange = -5;
                }
                else if (level < 19) // verified
                {
                    GreyRange = -6;

                    GreenRange = -5;
                }
                else if (level < 21) // verified
                {
                    GreyRange = -7;

                    GreenRange = -5;
                }
                else if (level < 22) // verified
                {
                    GreyRange = -7;

                    GreenRange = -6;
                }
                else if (level < 25) // verified
                {
                    GreyRange = -8;

                    GreenRange = -6;
                }
                else if (level < 28) // verified
                {
                    GreyRange = -9;

                    GreenRange = -7;
                }
                else if (level < 29) // verified
                {
                    GreyRange = -10;

                    GreenRange = -7;
                }
                else if (level < 31) // verified
                {
                    GreyRange = -10;

                    GreenRange = -8;
                }
                else if (level < 33) // verified
                {
                    GreyRange = -11;

                    GreenRange = -8;
                }
                else if (level < 34) // verified
                {
                    GreyRange = -11;

                    GreenRange = -9;
                }
                else if (level < 37) // verified
                {
                    GreyRange = -12;

                    GreenRange = -9;
                }
                else if (level < 40) // verified
                {
                    GreyRange = -13;

                    GreenRange = -10;
                }
                else if (level < 41) // Verified
                {
                    GreyRange = -14;

                    GreenRange = -10;
                }
                else if (level < 43) // Verified
                {
                    GreyRange = -14;

                    GreenRange = -11;
                }
                else if (level < 45)  // Verified
                {
                    GreyRange = -15;

                    GreenRange = -11;
                }
                else if (level < 46)  // Verified
                {
                    GreyRange = -15;

                    GreenRange = -12;
                }
                else if (level < 49)  // Verified
                {
                    GreyRange = -16;

                    GreenRange = -12;
                }
                else if (level < 51) // Verified at 50
                {
                    GreyRange = -17;

                    GreenRange = -13;
                }
                else if (level < 53)
                {
                    GreyRange = -18;

                    GreenRange = -14;
                }
                else if (level < 57)
                {
                    GreyRange = -20;

                    GreenRange = -15;
                }
                else
                {
                    GreyRange = -21;

                    GreenRange = -16;
                }
            }
        }

        public void Clear()
        {
            mobs.Clear();

            itemcollection.Clear();

            mobtrails.Clear();
        }

        public void CalculateMapLinePens()
        {
            if (lines == null)
                return;

            Pen darkpen = new Pen(Color.Black);
            foreach (MapLine line in lines)
            {
                if (Settings.Default.ForceDistinct)
                {
                    line.draw_color = GetDistinctColor(darkpen);
                    line.fade_color = new Pen(Color.FromArgb(Settings.Default.FadedLines * 255 / 100, line.draw_color.Color));
                }
                else
                {
                    line.draw_color = GetDistinctColor(new Pen(line.color.Color));
                    line.fade_color = new Pen(Color.FromArgb(Settings.Default.FadedLines * 255 / 100, line.draw_color.Color));
                }
            }
            SolidBrush distinctbrush = new SolidBrush(Color.Black);
            foreach (MapText t in texts)
            {
                t.draw_color = Settings.Default.ForceDistinctText ? GetDistinctColor(distinctbrush) : GetDistinctColor(t.color);
                t.draw_pen = new Pen(t.draw_color.Color);
            }
        }

        public void CollectMobTrails()
        {
            // Collect Mob Trails

            foreach (SPAWNINFO sp in GetMobsReadonly().Values)
            {
                if (sp.Type == 1)
                {
                    // Setup NPCs Trails

                    //add point to mobtrails arraylist if not already there

                    MobTrailPoint work = new MobTrailPoint
                    {
                        x = (int)sp.X,

                        y = (int)sp.Y
                    };

                    AddMobTrailPoint(work);
                }
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

            return Math.Max(lColDiff, lTmp);
        }

        private Color GetInverseColor(Color foreColor)
        {
            short iFRed, iFGreen, iFBlue;

            iFRed = foreColor.R;

            iFGreen = foreColor.G;

            iFBlue = foreColor.B;

            return Color.FromArgb((int)(192 - (iFRed * 0.75)), (int)(192 - (iFGreen * 0.75)), (int)(192 - (iFBlue * 0.75)));
        }

        public Color GetDistinctColor(Color foreColor, Color backColor)
        {
            // make sure the fore + back color can be distinguished.

            const int ColorThreshold = 55;

            if (GetColorDiff(foreColor, backColor) >= ColorThreshold)
            {
                return foreColor;
            }
            else
            {
                Color inverseColor = GetInverseColor(foreColor);

                if (GetColorDiff(inverseColor, backColor) > ColorThreshold)
                    return inverseColor;
                else //' if we have grey rgb(127,127,127) the inverse is the same so return black...

                    return Color.Black;
            }
        }

        public Pen GetDistinctColor(Pen curPen)
        {
            curPen.Color = GetDistinctColor(curPen.Color, Settings.Default.BackColor);

            return curPen;
        }

        public Color GetDistinctColor(Color curColor) => GetDistinctColor(curColor, Settings.Default.BackColor);

        public SolidBrush GetDistinctColor(SolidBrush curBrush)
        {
            curBrush.Color = GetDistinctColor(curBrush.Color, Settings.Default.BackColor);

            return curBrush;
        }

        #endregion ColorCheck between Foreground and Background
    }
}