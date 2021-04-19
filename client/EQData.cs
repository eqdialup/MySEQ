using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using myseq.Properties;
using Structures;

namespace myseq
{
    // This is the "model" part - no UI related things in here, only hard EQ data.

    public class EQData : IAlertStatus
    {
        private static readonly Spawninfo sPAWNINFO = new Spawninfo();

        private ConColors GetConColors = new ConColors();

        // player details
        public Spawninfo gamerInfo = sPAWNINFO;

        // Map details
        public string longname = "";

        public string shortname = "";
        //// Map data
        // Max + Min map coordinates - define the bounds of the zone

        public float minx = -1000;

        public float maxx = 1000;

        public float miny = -1000;

        public float maxy = 1000;

        public float minz = -1000;

        public float maxz = 1000;

        private readonly List<GroundItem> itemcollection = new List<GroundItem>();          // Hold the items that are on the ground

        private readonly Hashtable mobsHashTable = new Hashtable();             // Holds the details of the mobs in the current zone.

        public MobsTimers mobsTimers { get; } = new MobsTimers();               // Manages the timers

        public int selectedID = 99999;

        public float SpawnX = -1;

        public float SpawnY = -1;

        private int EQSelectedID = 0;

        public DateTime gametime = new DateTime();

        private readonly Random rnd = new Random();

        // Mobs / UI Lists

        private List<ListViewItem> NewSpawns { get; } = new List<ListViewItem>();

        private List<ListViewItem> NewGroundItems { get; } = new List<ListViewItem>();

        // Items List by ID and Description loaded from file

        public List<ListItem> GroundSpawn = new List<ListItem>();

        // Guild List by ID and Description loaded from file

        //        public Hashtable guildList = new Hashtable();

        private bool CorpseAlerts = true;

        public bool Zoning { get; set; }
        public string[] Classes { get; private set; }
        public string[] Races { get; private set; }
        public string GConBaseName { get; set; } = "";
        public SolidBrush[] ConColors { get; set; } = new SolidBrush[500];

        private const int ditchGone = 2;

        public Hashtable GetMobsReadonly() => mobsHashTable;

        public List<GroundItem> GetItemsReadonly() => itemcollection;

        public void ProcessSpawnTimer(MainForm f1)
        {
            if (mobsTimers.mobsTimer2.Count > 0)
            {
                mobsTimers.UpdateList(f1.SpawnTimerList);
            }
        }

        public bool SelectTimer(float x, float y, float delta)
        {
            Spawntimer st = FindTimer(x, y, delta);

            if (st != null)
            {
                if (Settings.Default.AutoSelectSpawnList && st.itmSpawnTimerList != null)
                {
                    st.itmSpawnTimerList.EnsureVisible();
                    st.itmSpawnTimerList.Selected = true;
                }
                Spawninfo sp = FindMobTimer(st.SpawnLoc);

                selectedID = sp == null ? 99999 : sp.SpawnID;

                SpawnX = st.X;

                SpawnY = st.Y;

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SelectGroundItem(float x, float y, float delta)
        {
            GroundItem gi = FindGroundItem(x, y, delta);

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

        public bool SelectMob(float x, float y, float delta)
        {
            Spawninfo sp = FindMobNoPet(x, y, delta) ?? FindMob(x, y, delta);

            if (sp != null)
            {
                if (Settings.Default.AutoSelectSpawnList)
                {
                    sp.listitem.EnsureVisible();

                    sp.listitem.Selected = true;
                }

                selectedID = sp.SpawnID;

                SpawnX = -1.0f;

                SpawnY = -1.0f;

                return true;
            }
            else
            {
                return false;
            }
        }

        public Spawninfo FindMobNoPet(float x, float y, float delta)
        {
            try
            {
                foreach (Spawninfo sp in mobsHashTable.Values)
                {
                    var dely = sp.Y < y + delta && sp.Y > y - delta;
                    var delx = sp.X < x + delta && sp.X > x - delta;
                    if (!sp.filtered && HiddenFamPet(sp) && delx && dely)
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

        public Spawninfo FindMobNoPetNoPlayerNoCorpse(float x, float y, float delta)
        {
            try
            {
                foreach (Spawninfo sp in mobsHashTable.Values)
                {
                    var dely = sp.Y < y + delta && sp.Y > y - delta;
                    var delx = sp.X < x + delta && sp.X > x - delta;

                    if (HiddenFamPet(sp) && !sp.m_isPlayer && !sp.isCorpse && delx && dely)
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

        public Spawninfo FindMobNoPetNoPlayer(float x, float y, float delta)
        {
            try
            {
                foreach (Spawninfo sp in mobsHashTable.Values)
                {
                    if (HiddenFamPet(sp) && !sp.isCorpse && sp.X < x + delta && sp.X > x - delta && sp.Y < y + delta && sp.Y > y - delta)
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

        private static bool HiddenFamPet(Spawninfo sp) => !sp.hidden && !sp.isFamiliar && !sp.isPet && !sp.isMerc;

        public Spawninfo FindMob(float x, float y, float delta)
        {
            try
            {
                foreach (Spawninfo sp in mobsHashTable.Values)
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

        public Spawninfo FindMobTimer(string spawnLoc)
        {
            try
            {
                foreach (Spawninfo sp in mobsHashTable.Values)
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

        public Spawntimer FindListViewTimer(ListViewItem listItem)
        {
            try
            {
                // This returns mobsTimer2
                foreach (Spawntimer st in mobsTimers.GetRespawned().Values)
                {
                    if (st.itmSpawnTimerList == listItem)
                    {
                        return st;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                LogLib.WriteLine("Error in SPAWNTIMER FindTimer(): ", ex);

                return null;
            }
        }

        public Spawntimer FindTimer(float x, float y, float delta)
        {
            try
            {
                // This returns mobsTimer2
                foreach (Spawntimer st in mobsTimers.GetRespawned().Values)
                {
                    if (st.X < x + delta && st.X > x - delta && st.Y < y + delta && st.Y > y - delta)
                    {
                        return st;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                LogLib.WriteLine("Error in SPAWNTIMER FindTimer(): ", ex);

                return null;
            }
        }

        public GroundItem FindGroundItem(float x, float y, float delta)
        {
            foreach (GroundItem gi in itemcollection)
            {
                if (!gi.Filtered && gi.X < x + delta && gi.X > x - delta && gi.Y < y + delta && gi.Y > y - delta)
                {
                    return gi;
                }
            }

            return null;
        }

        public GroundItem FindGroundItemNoFilter(float x, float y, float delta)
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

        public Spawninfo GetSelectedMob() => (Spawninfo)mobsHashTable[selectedID];

        // Originally called when first loading program.
        // Now called when reconnect(and char change), so we can modify these list more "on the fly"
        public void InitLookups()
        {
            Classes = GetStrArrayFromTextFile(Path.Combine(Settings.Default.CfgDir, "Classes.txt"));

            Races = GetStrArrayFromTextFile(Path.Combine(Settings.Default.CfgDir, "Races.txt"));

            GroundSpawn.Clear();

            ReadItemList(Path.Combine(Settings.Default.CfgDir, "GroundItems.ini"));
            //guildList.Clear();

            //ReadGuildList(Path.Combine(Settings.Default.CfgDir, "Guilds.txt"));
        }

        private string[] GetStrArrayFromTextFile(string filePath)
        {
            List<string> arList = new List<string>();
            if (File.Exists(filePath))
            {
                foreach (var line in File.ReadAllLines(filePath))
                {
                    var ln = line.Trim();
                    if (ln != null && ln.Substring(0, 1) != "#")
                    {
                        arList.Add(ln);
                    }
                }
            }
            return arList.ToArray();
        }

        private void ReadItemList(string filePath)
        {
            if (!File.Exists(filePath))
            {
                LogLib.WriteLine("GroundItems.ini file not found", LogLevel.Warning);
                return;
            }

            foreach (var line in File.ReadAllLines(filePath).ToList())
            {
                //sample:  IT0_ACTORDEF = Generic
                if (!line.StartsWith("[") && !string.IsNullOrWhiteSpace(line))
                {
                    var entries = line.Split('=');
                    var tmp = entries[0].Split('_');
                    ListItem newGround = new ListItem
                    {
                        ID = int.Parse(tmp[0].Remove(0, 2)),/* NumFormat),*/ //0
                        ActorDef = entries[0], //IT0_ACTORDEF
                        Name = entries[1] //NAME
                    };
                    GroundSpawn.Add(newGround);
                }
            }
        }

        //private void ReadGuildList(string filePath)
        //{
        //    string line = "";

        //    IFormatProvider NumFormat = new CultureInfo("en-US");

        //    if (!File.Exists(filePath))
        //    {
        //        // we did not find the Guild file
        //        LogLib.WriteLine("Guild file not found", LogLevel.Warning);
        //        return;
        //    }

        //    FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        //    StreamReader sr = new StreamReader(fs);

        //    while (line != null)
        //    {
        //        line = sr.ReadLine();

        //        if (line != null)
        //        {
        //            line = line.Trim();

        //            if (line.Length > 0 && (!line.StartsWith("[") && !line.StartsWith("#")))
        //            {
        //                ListItem thisitem = new ListItem();

        //                string tok;
        //                if ((tok = Getnexttoken(ref line, '=')) != null)
        //                {
        //                    thisitem.ActorDef = tok.ToUpper();

        //                    if ((tok = Getnexttoken(ref line, ',')) != null)
        //                    {
        //                        thisitem.Name = tok;

        //                        if ((tok = Getnexttoken(ref thisitem.ActorDef, '_')) != null)
        //                        {
        //                            thisitem.ID = int.Parse(tok, NumFormat);

        //                            // We got this far, so we have a valid item to add

        //                            if (!guildList.ContainsKey(thisitem.ID))
        //                            {
        //                                try { guildList.Add(thisitem.ID, thisitem); }
        //                                catch (Exception ex) { LogLib.WriteLine("Error adding " + thisitem.ID + " to Guilds hashtable: ", ex); }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    sr.Close();
        //    fs.Close();
        //}
        //public string GetGuildDescription(string guildDef)
        //{
        //    // I know - ## NEEDS CLEANUP
        //    // Get description from list made using Guildlist.txt
        //    string tok;
        //    return (tok = Getnexttoken(ref guildDef, '_')) != null
        //        && guildList.ContainsKey(int.Parse(tok, new CultureInfo("en-US")))
        //        ? ((ListItem)guildList[int.Parse(tok, new CultureInfo("en-US"))]).Name
        //        : guildDef;
        //}

        private string ArrayIndextoStr(string[] source, int index) => index < source.GetLowerBound(0) || index > source.GetUpperBound(0) ? $"{index}: Unknown" : source[index];

        public void CalcExtents(List<MapLine> lines)
        {
            if (longname != "" && lines.Count > 0)
            {
                maxx = minx = lines[0].Point(0).x;

                maxy = miny = lines[0].Point(0).y;

                maxz = minz = lines[0].Point(0).z;

                foreach (MapLine mapLine in lines)
                {
                    ExtendMapLines(mapLine);
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

        private void ExtendMapLines(MapLine mapLine)
        {
            foreach (MapPoint mapPoint in mapLine.aPoints)
            {
                if (mapPoint.x > maxx)
                {
                    maxx = mapPoint.x;
                }
                else if (mapPoint.x < minx)
                {
                    minx = mapPoint.x;
                }

                if (mapPoint.y > maxy)
                {
                    maxy = mapPoint.y;
                }
                else if (mapPoint.y < miny)
                {
                    miny = mapPoint.y;
                }

                if (mapPoint.z > maxz)
                {
                    maxz = mapPoint.z;
                }
                else if (mapPoint.z < minz)
                {
                    minz = mapPoint.z;
                }
            }
        }

        public void CheckMobs(ListViewPanel SpawnList, ListViewPanel GroundItemList)
        {
            ArrayList deletedItems = new ArrayList();

            ArrayList delListItems = new ArrayList();

            // Increment the remove timers on all the ground spawns

            foreach (GroundItem sp in itemcollection)
            {
                if (sp.gone >= ditchGone)
                {
                    deletedItems.Add(sp);
                }
                else
                {
                    sp.gone++;
                }
            }

            // Remove any that have been marked for deletion
            if (deletedItems.Count > 0)
            {
                if (Zoning || deletedItems.Count > 5)
                {
                    GroundItemList.listView.BeginUpdate();
                }

                foreach (GroundItem gi in deletedItems)
                {
                    GroundItemList.listView.Items.Remove(gi.listitem);

                    gi.listitem = null;

                    itemcollection.Remove(gi);
                }

                if (Zoning || deletedItems.Count > 5)
                {
                    GroundItemList.listView.EndUpdate();
                }
            }
            deletedItems.Clear();

            foreach (Spawninfo sp in mobsHashTable.Values)
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

            if (deletedItems.Count > 5 || delListItems.Count > 5)
            {
                SpawnList.listView.BeginUpdate();

                RenoveDeadEntries(SpawnList, deletedItems, delListItems);

                SpawnList.listView.EndUpdate();
                delListItems.Clear();

                deletedItems.Clear();
            }
        }

        private void RenoveDeadEntries(ListViewPanel SpawnList, ArrayList deletedItems, ArrayList delListItems)
        {
            foreach (Spawninfo sp in deletedItems)
            {
                SpawnList.listView.Items.Remove(sp.listitem);

                sp.listitem = null;

                mobsHashTable.Remove(sp.SpawnID);
            }

            foreach (Spawninfo sp in delListItems)
            {
                SpawnList.listView.Items.Remove(sp.listitem);
            }
        }

        public void ProcessGroundItems(Spawninfo si, Filters filters)
        {
            try
            {
                var found = false;
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
                    GroundItem gi = new GroundItem(si, this);

                    var itemname = gi.Desc.ToLower();
                    CheckGrounditemForAlerts(filters, gi, itemname);
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
                    NewGroundItems.Add(item1);
                }
            }
            catch (Exception ex) { LogLib.WriteLine("Error in ProcessGroundItems(): ", ex); }
        }

        public void ProcessTarget(Spawninfo si)
        {
            try
            {
                if (Settings.Default.AutoSelectEQTarget && EQSelectedID != si.SpawnID)
                {
                    EQSelectedID = selectedID = si.SpawnID;

                    SpawnX = -1.0f;

                    SpawnY = -1.0f;

                    foreach (Spawninfo sp in mobsHashTable.Values)
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

        public void ProcessWorld(Spawninfo si) => gametime = new DateTime(si.Race, si.Hide, si.Level, si.Type - 1, si.Class, 0);
        /*  yy/mm/dd/hh/min
         * gameYear = si.Race
         * gameMonth = si.Hide
         * gameDay = si.Levelafk
         * gameHour = si.Type - 1
         * gameMin = si.Class
        */

        public void ProcessSpawns(Spawninfo si, MainForm f1, Filters filters, bool update_hidden)
        {
            Tainted_Egg(si);

            try
            {
                var listReAdd = false;

                var found = false;

                Spawninfo mob;

                // Converted mob collection to a hashtable so we can do
                // a fast lookup to see if a mob already exists

                if (mobsHashTable.ContainsKey(si.SpawnID))
                {
                    found = true;

                    mob = (Spawninfo)mobsHashTable[si.SpawnID];

                    mob.gone = 0;

                    if (update_hidden)
                    {
                        mob.refresh = 100;
                    }

                    // some of these should not change often, so only check every 10 times through
                    if (mob.refresh > 10)
                    {   // Update mob types
                        if (mob.Type != si.Type)
                        {
                            UpdateMobTypes(si, f1.SpawnList, mob);
                        }
                        MobLevelSetColor(si, f1.SpawnList, mob);

                        // Update Hidden flags
                        if (update_hidden)
                        {
                            UpdateHidden(si, mob);
                            if (!si.hidden && mob.hidden)
                            {
                                listReAdd = true;
                            }
                        }

                        // check if the mob name has changed - eg when a mob dies.
                        if ((si.Name.Length > 0) && (string.Compare(mob.Name, si.Name) != 0))
                        {
                            NameChngOrDead(si, mob);
                        }

                        if (mob.Class != si.Class)
                        {
                            mob.Class = si.Class;
                            mob.listitem.SubItems[2].Text = GetClass(si.Class);
                        }

                        if (mob.Primary != si.Primary)
                        {
                            mob.Primary = si.Primary;
                            mob.listitem.SubItems[3].Text = si.Primary > 0 ? ItemNumToString(si.Primary) : "";
                        }

                        if (mob.Offhand != si.Offhand)
                        {
                            mob.Offhand = si.Offhand;
                            mob.listitem.SubItems[4].Text = si.Offhand > 0 ? ItemNumToString(si.Offhand) : "";
                        }

                        if (mob.Race != si.Race)
                        {
                            mob.Race = si.Race;
                            mob.listitem.SubItems[5].Text = GetRace(si.Race);
                        }

                        if (mob.OwnerID != si.OwnerID)
                        {
                            mob.OwnerID = si.OwnerID;
                            MobHasOwner(mob);
                            mob.Hide = si.Hide;
                            mob.listitem.SubItems[9].Text = PrettyNames.GetHideStatus(si.Hide);
                        }

                        //if (mob.Guild != si.Guild)
                        //{
                        //    mob.Guild = si.Guild;

                        //    if (si.Guild > 0)
                        //        mob.listitem.SubItems[17].Text = GuildNumToString(si.Guild);
                        //    else
                        //        mob.listitem.SubItems[17].Text = "";
                        //}

                        mob.refresh = 0;
                    } // end refresh > 10

                    mob.refresh++;

                    // Set variables we dont want to trigger list update
                    UpdateMobPosition(si, f1, mob);

                    mob.Heading = si.Heading;

                    mob.SpeedRun = si.SpeedRun;

                    if (mob.SpeedRun != si.SpeedRun)
                    {
                        mob.SpeedRun = si.SpeedRun;
                        mob.listitem.SubItems[10].Text = si.SpeedRun.ToString();
                    }

                    if ((mob.X != si.X) || (mob.Y != si.Y) || (mob.Z != si.Z))
                    {
                        // this should be the selected id
                        // ensure that map is big enough to show all spawns.
                        CheckBigMap(si, f1.mapPane);
                        PopulateListview(si, f1, mob);
                    }

                    if (listReAdd)
                    {
                        NewSpawns.Add(mob.listitem);
                    }
                } // end of if found

                // If it's not already in there, add it

                if (!found && !string.IsNullOrEmpty(si.Name))
                {
                    SpawnNotFound(si, f1, filters);
                }
            }
            catch (Exception ex) { LogLib.WriteLine("Error in ProcessSpawns(): ", ex); }
        }

        private void UpdateMobPosition(Spawninfo si, MainForm f1, Spawninfo mob)
        {
            if (selectedID != mob.SpawnID)
            {
                if (mob.X != si.X)
                {
                    // ensure that map is big enough to show all spawns.
                    CheckBigMap(si, f1.mapPane);

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
        }

        private void SpawnNotFound(Spawninfo si, MainForm f1, Filters filters)
        {
            // ensure that map is big enough to show all spawns.
            CheckBigMap(si, f1.mapPane);

            // Set mob type info
            if (si.Type == 0)
            {
                // Players

                si.m_isPlayer = true;

                if (!Settings.Default.ShowPlayers)
                {
                    si.hidden = true;
                }
            }
            else if (si.Type == 2 || si.Type == 3)
            {
                // Corpses
                HandleCorpses(si);
            }
            else
            {
                // non-corpse, non-player spawn (aka NPC)
                HandleNPCs(si);
            }

            mobsTimers.Spawn(si);

            IsSpawnInFilterLists(si, f1, filters);
        }

        private static void Tainted_Egg(Spawninfo si)
        {
            if (si.Name.Contains("a_tainted_egg"))
            {
                si.Class = 1;
            }
        }

        private void MobHasOwner(Spawninfo mob)
        {
            if (mob.OwnerID == 0)
            {
                mob.listitem.SubItems[6].Text = "";
                mob.isPet = false;
            }
            else if (mobsHashTable.ContainsKey(mob.OwnerID))
            {
                Spawninfo owner = (Spawninfo)mobsHashTable[mob.OwnerID];

                if (owner.IsPlayer)
                {
                    mob.isPet = true;
                    mob.listitem.ForeColor = Color.Gray;
                }
                else
                {
                    mob.isPet = false;
                }
                mob.listitem.SubItems[6].Text = RegexHelper.FixMobName(owner.Name);
            }
            else
            {
                mob.listitem.SubItems[6].Text = mob.OwnerID.ToString();
                mob.isPet = false;
            }
        }

        private void MobLevelSetColor(Spawninfo si, ListViewPanel SpawnList, Spawninfo mob)
        {
            if (mob.Level != si.Level)
            {
                mob.Level = si.Level;
                mob.listitem.SubItems[1].Text = si.Level.ToString();

                // update forecolor
                if (mob.Type == 2 || mob.Type == 3 || mob.isLDONObject)
                {
                    mob.listitem.ForeColor = Color.Gray;
                }
                else if (mob.isEventController)
                {
                    mob.listitem.ForeColor = Color.DarkOrchid;
                }
                else
                {
                    SetListColors(si, SpawnList, mob);
                }
            }
        }

        private void CheckBigMap(Spawninfo si, MapPane mapPane)
        {
            if (mapPane?.scale.Value == 100M && Settings.Default.AutoExpand)
            {
                if ((minx > si.X) && (si.X > -20000))
                {
                    minx = si.X;
                }

                if ((maxx < si.X) && (si.X < 20000))
                {
                    maxx = si.X;
                }

                if ((miny > si.Y) && (si.Y > -20000))
                {
                    miny = si.Y;
                }

                if ((maxy < si.Y) && (si.Y < 20000))
                {
                    maxy = si.Y;
                }
            }
        }

        private void HandleNPCs(Spawninfo si)
        {
            if (!Settings.Default.ShowNPCs)
            {
                si.hidden = true;
            }

            if (si.OwnerID > 0 && mobsHashTable.ContainsKey(si.OwnerID))
            {
                Spawninfo owner = (Spawninfo)mobsHashTable[si.OwnerID];
                if (owner.IsPlayer)
                {
                    si.isPet = true;
                    if (!Settings.Default.ShowPets)
                    {
                        si.hidden = true;
                    }
                }
            }

            if ((si.Race == 127) && ((si.Name.IndexOf("_") == 0) || (si.Level < 2) || (si.Class == 62))) // Invisible Man Race
            {
                si.isEventController = true;
                if (!Settings.Default.ShowInvis)
                {
                    si.hidden = true;
                }
            }
            else if (si.Class == 62)
            {
                si.isLDONObject = true;
            }

            // Mercenary Identification - Only do it once now

            if (!string.IsNullOrEmpty(si.Lastname) && RegexHelper.IsMerc(si.Lastname))
            {
                si.isMerc = true;
            }
            else if (RegexHelper.IsMount(si.Name)) // Mounts
            {
                si.isMount = true;

                if (!Settings.Default.ShowMounts)
                {
                    si.hidden = true;
                }
            }
            else if (RegexHelper.IsFamiliar(si.Name))
            {
                // reset these, if match a familiar
                si.isPet = false;
                si.hidden = false;

                si.isFamiliar = true;

                if (!Settings.Default.ShowFamiliars)
                {
                    si.hidden = true;
                }
            }
        }

        private void HandleCorpses(Spawninfo si)
        {
            CorpseAlerts = Settings.Default.CorpseAlerts;
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

                if (!Settings.Default.ShowPCCorpses)
                {
                    si.hidden = true;
                }

                if (si.Name.Length > 0 && CheckMyCorpse(si.Name))
                {
                    si.m_isMyCorpse = true;

                    si.hidden = !Settings.Default.ShowMyCorpse;
                }
            }
            else if (!Settings.Default.ShowCorpses)
            {
                si.hidden = true;
            }
        }

        private void PopulateListview(Spawninfo si, MainForm f1, Spawninfo mob)
        {
            mob.X = si.X;
            mob.listitem.SubItems[14].Text = si.Y.ToString();

            mob.Y = si.Y;
            mob.listitem.SubItems[13].Text = si.X.ToString();

            mob.Z = si.Z;
            mob.listitem.SubItems[15].Text = si.Z.ToString();

            if (Settings.Default.FollowOption == FollowOption.Target)
            {
                f1.ReAdjust();
            }

            mob.listitem.SubItems[16].Text = si.SpawnDistance(si, gamerInfo).ToString("#.00");
        }

        private void UpdateMobTypes(Spawninfo si, ListViewPanel SpawnList, Spawninfo mob)
        {
            mob.Type = si.Type;
            mob.listitem.SubItems[8].Text = PrettyNames.GetSpawnType(si.Type);

            if (si.Type == 2 || si.Type == 3)
            {
                mob.listitem.ForeColor = Color.Gray;

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
                mob.listitem.ForeColor = Color.DarkOrchid;
                si.isEventController = true;
            }
            else if (si.Class == 62)
            {
                mob.listitem.ForeColor = Color.Gray;
                si.isLDONObject = true;
            }
            else
            {
                SetListColors(si, SpawnList, mob);
            }
        }

        private void SetListColors(Spawninfo si, ListViewPanel SpawnList, Spawninfo mob)
        {
            mob.listitem.ForeColor = ConColors[si.Level].Color;

            if (mob.listitem.ForeColor == Color.Maroon)
            {
                mob.listitem.ForeColor = Color.Red;
            }
            else if (SpawnList.listView.BackColor == Color.White)
            {
                // Change the colors to be more visible on white if the background is white

                if (mob.listitem.ForeColor == Color.White)
                {
                    mob.listitem.ForeColor = Color.Black;
                }
                else if (mob.listitem.ForeColor == Color.Yellow)
                {
                    mob.listitem.ForeColor = Color.Goldenrod;
                }
            }
        }

        private static void UpdateHidden(Spawninfo si, Spawninfo mob)
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
                    {
                        si.hidden = true;
                    }
                    else if (mob.isMount && !Settings.Default.ShowMounts) // Mounts
                    {
                        si.hidden = true;
                    }
                    else if (mob.isPet && !Settings.Default.ShowPets) // Pets
                    {
                        si.hidden = true;
                    }
                    else if (mob.isFamiliar && !Settings.Default.ShowFamiliars) // Familiars
                    {
                        si.hidden = true;
                    }
                }
            }

            if (si.hidden && !mob.hidden)
            {
                mob.delFromList = true;
            }
            mob.hidden = si.hidden;
        }

        private void IsSpawnInFilterLists(Spawninfo si, MainForm f1, Filters filters)//, bool alert)
        {
            var mobname = si.isMerc ? RegexHelper.FixMobNameMatch(si.Name) : RegexHelper.FixMobName(si.Name);

            var matchmobname = RegexHelper.FixMobNameMatch(mobname);
            var alert = false;
            if (matchmobname.Length < 2)
            {
                matchmobname = mobname;
            }

            var mobnameWithInfo = mobname;

            if (si.Primary > 0 || si.Offhand > 0)
            {
                si.PrimaryName = ItemNumToString(si.Primary);
                si.OffhandName = ItemNumToString(si.Offhand);
            }

            // Don't do alert matches for controllers, Ldon objects, pets, mercs, mounts, or familiars
            if (!(si.isLDONObject || si.isEventController || si.isFamiliar || si.isMount || (si.isMerc && si.OwnerID != 0)))
            {
                AssignAlertStatus(si, filters, matchmobname, ref alert, ref mobnameWithInfo);

                PrettyNames.LookupBoxMatch(si, f1);
            }

            ListViewItem item1 = AddDetailsToList(si, f1.SpawnList, mobnameWithInfo);
            PlayAudioMatch(si, mobnameWithInfo);

            try { mobsHashTable.Add(si.SpawnID, si); }
            catch (Exception ex) { LogLib.WriteLine($"Error adding {si.Name} to mobs hashtable: ", ex); }

            // Add it to the spawn list if it's not supposed to be hidden
            if (!si.hidden)
            {
                NewSpawns.Add(item1);
            }
        }

        private ListViewItem AddDetailsToList(Spawninfo si, ListViewPanel SpawnList, string mobnameWithInfo)
        {
            ListViewItem item1 = new ListViewItem(mobnameWithInfo);

            item1.SubItems.Add(si.Level.ToString());

            item1.SubItems.Add(GetClass(si.Class));

            if (si.Primary > 0)
            {
                item1.SubItems.Add(ItemNumToString(si.Primary));
            }
            else
            {
                item1.SubItems.Add("");
            }

            if (si.Offhand > 0)
            {
                item1.SubItems.Add(ItemNumToString(si.Offhand));
            }
            else
            {
                item1.SubItems.Add("");
            }

            item1.SubItems.Add(GetRace(si.Race));

            OwnerFlag(si, item1);

            item1.SubItems.Add(si.Lastname);

            item1.SubItems.Add(PrettyNames.GetSpawnType(si.Type));

            item1.SubItems.Add(PrettyNames.GetHideStatus(si.Hide));

            item1.SubItems.Add(si.SpeedRun.ToString());

            item1.SubItems.Add(si.SpawnID.ToString());

            item1.SubItems.Add(DateTime.Now.ToLongTimeString());

            item1.SubItems.Add(si.X.ToString("#.00"));

            item1.SubItems.Add(si.Y.ToString("#.00"));

            item1.SubItems.Add(si.Z.ToString("#.00"));

            item1.SubItems.Add(si.SpawnDistance(si, gamerInfo).ToString("#.00"));

            //            item1.SubItems.Add(GuildNumToString(si.Guild));

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
                item1.ForeColor = ConColors[si.Level].Color;

                if (item1.ForeColor == Color.Maroon)
                {
                    item1.ForeColor = Color.Red;
                }

                // Change the colors to be more visible on white if the background is white

                if (SpawnList.listView.BackColor == Color.White)
                {
                    if (item1.ForeColor == Color.White)
                    {
                        item1.ForeColor = Color.Black;
                    }
                    else if (item1.ForeColor == Color.Yellow)
                    {
                        item1.ForeColor = Color.Goldenrod;
                    }
                }
            }

            si.gone = 0;

            si.refresh = rnd.Next(0, 10);

            si.listitem = item1;
            return item1;
        }

        private void NameChngOrDead(Spawninfo si, Spawninfo mob)
        {
            var newname = RegexHelper.FixMobName(si.Name);

            var oldname = RegexHelper.FixMobName(mob.Name);
            mob.listitem.Text = mob.listitem.Text.Replace(oldname, newname);

            if (!si.IsPlayer && (si.Type == 2 || si.Type == 3))
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

        private void OwnerFlag(Spawninfo si, ListViewItem item1)
        {
            if (si.OwnerID > 0)
            {
                if (mobsHashTable.ContainsKey(si.OwnerID))
                {
                    Spawninfo owner = (Spawninfo)mobsHashTable[si.OwnerID];
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
            if (mobsHashTable != null)
            {
                foreach (Spawninfo si in mobsHashTable.Values)
                {
                    if (si.listitem != null)
                    {
                        if (si.Type == 2 || si.Type == 3 || si.isLDONObject)
                        {
                            si.listitem.ForeColor = Color.Gray;
                        }
                        else if (si.isEventController)
                        {
                            si.listitem.ForeColor = Color.DarkOrchid;
                        }
                        else
                        {
                            si.listitem.ForeColor = ConColors[si.Level].Color;

                            if (si.listitem.ForeColor == Color.Maroon)
                            {
                                si.listitem.ForeColor = Color.Red;
                            }

                            // Change the colors to be more visible on white if the background is white

                            if (Settings.Default.ListBackColor == Color.White)
                            {
                                if (si.listitem.ForeColor == Color.White)
                                {
                                    si.listitem.ForeColor = Color.Black;
                                }
                                else if (si.listitem.ForeColor == Color.Yellow)
                                {
                                    si.listitem.ForeColor = Color.Goldenrod;
                                }
                            }

                            if (Settings.Default.ListBackColor == Color.Black && si.listitem.ForeColor == Color.Black)
                            {
                                si.listitem.ForeColor = Color.White;
                            }
                        }
                    }
                }
            }
        }

        public bool CheckMyCorpse(string mobname) => (mobname.Length < (gamerInfo.Name.Length + 14)) && (mobname.IndexOf(gamerInfo.Name) == 0);

        public void SaveMobs()
        {
            DateTime dt = DateTime.Now;

            var filename = $"{shortname} - {dt.Month}-{dt.Day}-{dt.Year} {dt.Hour}.txt";

            StreamWriter sw = new StreamWriter(filename, false);

            sw.Write("Name\tLevel\t Class\tRace\tLastname\tType\tInvis\tRun\tSpeed\tSpawnID\tX\tY\tZ\tHeading");

            foreach (Spawninfo si in mobsHashTable.Values)
            {
                sw.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}",
                             si.Name,
                             si.Level,
                             GetClass(si.Class),
                             GetRace(si.Race),
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
            Spawntimer st = FindTimer(x, y, 1.0f);

            if (st != null)
            {
                Spawninfo sp = FindMobTimer(st.SpawnLoc);

                selectedID = sp == null ? 99999 : sp.SpawnID;

                SpawnX = st.X;

                SpawnY = st.Y;
            }
        }

        public void SetSelectedGroundItem(float x, float y)
        {
            GroundItem gi = FindGroundItemNoFilter(x, y, 1.0f);

            if (gi != null)
            {
                selectedID = 99999;

                SpawnX = gi.X;

                SpawnY = gi.Y;
            }
        }

        public string GetClass(int num) => ArrayIndextoStr(Classes, num);

        public string ItemNumToString(int num)
        {
            foreach (ListItem item in GroundSpawn)
            {
                if (item.ID.Equals(num))
                {
                    return item.Name;
                }
            }
            return num.ToString();
        }

        //        public string GuildNumToString(int num) => guildList.ContainsKey(num) ? ((ListItem)guildList[num]).Name : num.ToString();

        public string GetRace(int num) => num == 2250 ? "Interactive Object" : ArrayIndextoStr(Races, num);

        public void BeginProcessPacket()
        {
            NewSpawns.Clear();
            NewGroundItems.Clear();
        }

        public void ProcessSpawnList(ListViewPanel SpawnList)
        {
            try
            {
                if (NewSpawns.Count > 0)
                {
                    if (Zoning)
                    {
                        SpawnList.listView.BeginUpdate();
                    }

                    ListViewItem[] items = new ListViewItem[NewSpawns.Count];

                    var d = 0;

                    foreach (ListViewItem i in NewSpawns)
                    {
                        if (i != null)
                        {
                            items[d++] = i;
                        }
                    }

                    SpawnList.listView.Items.AddRange(items);
                    NewSpawns.Clear();
                    if (Zoning)
                    {
                        SpawnList.listView.EndUpdate();
                    }
                }
            }
            catch (Exception ex) { LogLib.WriteLine("Error in ProcessSpawnList(): ", ex); }
        }

        public void ProcessGroundItemList(ListViewPanel GroundItemList)
        {
            try
            {
                if (NewGroundItems.Count > 0)
                {
                    ListViewItem[] items = new ListViewItem[NewGroundItems.Count];

                    var d = 0;

                    foreach (ListViewItem i in NewGroundItems)
                    {
                        items[d++] = i;
                    }

                    GroundItemList.listView.Items.AddRange(items);

                    NewGroundItems.Clear();
                }
            }
            catch (Exception ex) { LogLib.WriteLine("Error in ProcessGroundItemList(): ", ex); }
        }

        public float CalcRealHeading(Spawninfo sp) => sp.Heading >= 0 && sp.Heading < 512 ? sp.Heading / 512 * 360 : 0;

        #region ProcessGamer

        private int gLastconLevel = -1;
        private int gconLevel;

        public void ProcessGamer(Spawninfo si, MainForm f1)
        {
            try
            {
                gamerInfo.SpawnID = si.SpawnID;

                gamerInfo.Name = si.Name;
                f1.SetCharSelection(gamerInfo.Name);
                f1.SetTitle();

                gamerInfo.Lastname = si.Lastname;

                gamerInfo.X = si.X;
                gamerInfo.Y = si.Y;

                if (Settings.Default.FollowOption == FollowOption.Player)
                {
                    f1.ReAdjust();
                }

                gamerInfo.Z = si.Z;

                gamerInfo.Heading = si.Heading;

                gamerInfo.Hide = si.Hide;

                gamerInfo.SpeedRun = si.SpeedRun;
                gconLevel = Settings.Default.LevelOverride;
                if (gamerInfo.Level != si.Level)
                {
                    if (GConBaseName.Length > 0)
                    {
                        if (si.Level > gamerInfo.Level)
                        {
                            gconLevel += si.Level - gamerInfo.Level;
                        }
                        else
                        {
                            gconLevel -= gamerInfo.Level - si.Level;
                        }
                        if (gconLevel > 115)
                        {
                            gconLevel = 115;
                        }

                        if (gconLevel < 1)
                        {
                            gconLevel = -1;
                        }

                        gLastconLevel = gconLevel;
                        Settings.Default.LevelOverride = gconLevel;
                    }
                    gamerInfo.Level = si.Level;
                    GetConColors.FillConColors(f1, gamerInfo, ConColors);

                    // update mob list con colors

                    UpdateMobListColors();
                }
                if (gLastconLevel != gconLevel)
                {
                    gLastconLevel = gconLevel;
                    GetConColors.FillConColors(f1, gamerInfo, ConColors);
                    UpdateMobListColors();
                }
            }
            catch (Exception ex) { LogLib.WriteLine("Error in ProcessPlayer(): ", ex); }
        }

        #endregion ProcessGamer

        public void Clear()
        {
            mobsHashTable.Clear();
            itemcollection.Clear();
        }

        public void ModKeyControl(MapCon mapCon, float x, float y)
        {
            var delta = 5.0f / mapCon.m_ratio;

            Spawninfo sp = FindMobNoPet(x, y, delta) ?? FindMob(x, y, delta);

            if (sp != null)
            {
                Spawninfo st = FindMobTimer(sp.SpawnLoc);

                if (st == null)
                {
                    SetSelectedID(sp.SpawnID);

                    SpawnX = -1.0f;
                    SpawnY = -1.0f;
                }
                else
                {
                    SetSelectedID(st.SpawnID);
                    Spawntimer spawntimer = FindTimer(st.X, st.Y, 1.0f);
                    if (spawntimer?.itmSpawnTimerList != null)
                    {
                        spawntimer.itmSpawnTimerList.Selected = true;
                        spawntimer.itmSpawnTimerList.EnsureVisible();
                    }

                    SpawnX = st.X;
                    SpawnY = st.Y;
                }
            }
            else
            {
                if (!SelectTimer(x, y, delta))
                {
                    SelectGroundItem(x, y, delta);
                }
            }
        }

        #region ColorOperations

        public void CalculateMapLinePens(List<MapLine> lines, List<MapText> texts)
        {
            if (lines != null)
            {
                var alpha = Settings.Default.FadedLines * 255 / 100;
                foreach (MapLine mapline in lines)
                {
                    SetMaplineColor(alpha, mapline);
                }
                foreach (MapText maptxt in texts)
                {
                    maptxt.draw_color = Settings.Default.ForceDistinctText ? GetDistinctColor(new SolidBrush(Color.Black)) : GetDistinctColor(maptxt.color);
                    maptxt.draw_pen = new Pen(maptxt.draw_color.Color);
                }
            }
        }

        private void SetMaplineColor(int alpha, MapLine mapline)
        {
            if (Settings.Default.ForceDistinct)
            {
                mapline.draw_color = GetDistinctColor(new Pen(Color.Black));
                mapline.fade_color = new Pen(Color.FromArgb(alpha, mapline.draw_color.Color));
            }
            else
            {
                mapline.draw_color = GetDistinctColor(new Pen(mapline.color.Color));
                mapline.fade_color = new Pen(Color.FromArgb(alpha, mapline.draw_color.Color));
            }
        }

        public SolidBrush GetDistinctColor(SolidBrush curBrush)
        {
            curBrush.Color = GetDistinctColor(curBrush.Color, Settings.Default.BackColor);

            return curBrush;
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
                {
                    return inverseColor;
                }
                else //' if we have grey rgb(127,127,127) the inverse is the same so return black...
                {
                    return Color.Black;
                }
            }
        }

        public Pen GetDistinctColor(Pen curPen)
        {
            curPen.Color = GetDistinctColor(curPen.Color, Settings.Default.BackColor);
            return curPen;
        }

        public Color GetDistinctColor(Color curColor) => GetDistinctColor(curColor, Settings.Default.BackColor);

        private int GetColorDiff(Color foreColor, Color backColor)
        {
            int lTmp;
            var lColDiff = 0;

            lTmp = Math.Abs(backColor.R - foreColor.R);

            lColDiff = Math.Max(lColDiff, lTmp);

            lTmp = Math.Abs(backColor.G - foreColor.G);

            lColDiff = Math.Max(lColDiff, lTmp);

            lTmp = Math.Abs(backColor.B - foreColor.B);

            return Math.Max(lColDiff, lTmp);
        }

        private Color GetInverseColor(Color foreColor) => Color.FromArgb((int)(192 - (foreColor.R * 0.75)), (int)(192 - (foreColor.G * 0.75)), (int)(192 - (foreColor.B * 0.75)));
        public void AssignAlertStatus(Spawninfo si, Filters filters, string matchmobname, ref bool alert, ref string mobnameWithInfo){}
        public void PlayAudioMatch(Spawninfo si, string matchmobname) {}
        public void LoadSpawnInfo(){}
        public void CheckGrounditemForAlerts(Filters filters, GroundItem gi, string itemname) {}

        #endregion ColorOperations
    }
}