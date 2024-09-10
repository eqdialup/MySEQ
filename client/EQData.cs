using myseq.Properties;
using Structures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace myseq
{
    // This is the "model" part - no UI related things in here, only hard EQ data.

    public class EQData : FileOps
    {
        public interface IRemovableItem
        {
            bool ShouldBeDeleted { get; set; }
            ListViewItem Listitem { get; set; }
        }

        private static readonly Spawninfo sPAWNINFO = new Spawninfo();

        public readonly SpawnColors spawnColor = new SpawnColors();

        // player details
        public Spawninfo GamerInfo { get; set; } = sPAWNINFO;

        // Map details
        public string Longname { get; set; } = "";

        public string Shortname { get; set; } = "";

        //// Map data
        // Max + Min map coordinates - define the bounds of the zone
        public float MinmapX { get; set; } = -1000;

        public float MaxMapX { get; set; } = 1000;
        public float MinMapY { get; set; } = -1000;
        public float MaxMapY { get; set; } = 1000;
        public float MinMapZ { get; set; } = -1000;
        public float MaxMapZ { get; set; } = 1000;

        private List<GroundItem> itemcollection = new List<GroundItem>();          // Hold the items that are on the ground

        private Hashtable mobsHashTable = new Hashtable();             // Holds the details of the mobs in the current zone.
        public MobsTimers MobsTimers { get; } = new MobsTimers();               // Manages the timers

        private int EQSelectedID;
        public float SpawnX { get; set; } = -1;
        public float SpawnY { get; set; } = -1;
        public int SelectedID { get; set; } = 99999;

        public DateTime Gametime { get; private set; } = new DateTime();

        // Mobs / UI Lists
        private List<ListViewItem> NewSpawns { get; } = new List<ListViewItem>();

        private List<ListViewItem> NewGroundItems { get; } = new List<ListViewItem>();

        // Items List by ID and Description loaded from file
        public List<ListItem> GroundSpawn { get; set; } = new List<ListItem>();

        // Guild List by ID and Description loaded from file

        //        public Hashtable guildList = new Hashtable();

        private bool CorpseAlerts = true;

        public bool Zoning { get; set; }

        private string[] Classes;

        private string[] Races;
        public string GConBaseName { get; set; } = "";

        //private const int ditchGone = 2;

        public Hashtable GetMobsReadonly() => mobsHashTable;

        public List<GroundItem> GetItemsReadonly() => itemcollection;

        public bool SelectTimer(float x, float y, float delta)
        {
            Spawntimer st = FindTimer(x, y, delta);

            if (st != null)
            {
                if (Settings.Default.AutoSelectSpawnList && st.ItmSpawnTimerList != null)
                {
                    st.ItmSpawnTimerList.EnsureVisible();
                    st.ItmSpawnTimerList.Selected = true;
                }
                Spawninfo sp = FindMobTimer(st.SpawnLoc);

                SelectedID = sp == null ? 99999 : sp.SpawnID;

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
                    gi.Listitem.EnsureVisible();

                    gi.Listitem.Selected = true;
                }

                SetGroundID(gi);

                return true;
            }
            else
            {
                return false;
            }
        }

        private void SetGroundID(GroundItem gi)
        {
            SelectedID = 99999;

            SpawnX = gi.X;

            SpawnY = gi.Y;
        }

        public bool SelectMob(float x, float y, float delta)
        {
            Spawninfo sp = FindMob(x, y, delta) ?? FindMob(x, y, delta);

            if (sp != null)
            {
                if (Settings.Default.AutoSelectSpawnList)
                {
                    sp.listitem.EnsureVisible();

                    sp.listitem.Selected = true;
                }

                SelectedID = sp.SpawnID;

                DefaultSpawnLoc();

                return true;
            }
            else
            {
                return false;
            }
        }

        public Spawninfo FindMob(float x, float y, float delta, bool excludePet = false, bool excludePlayer = false, bool excludeCorpse = false)
        {
            foreach (Spawninfo sp in mobsHashTable.Values)
            {
                if (ShouldExcludeMob(sp, excludePet, excludePlayer, excludeCorpse))
                {
                    continue;
                }

                if (CheckXY(sp, x, y, delta))
                {
                    return sp;
                }
            }

            return null;
        }

        private bool ShouldExcludeMob(Spawninfo sp, bool excludePet, bool excludePlayer, bool excludeCorpse)
        {
            // Exclude hidden mobs
            if (sp.hidden)
            {
                return true;
            }

            // Exclude pets, familiars, mercs based on conditions
            if (excludePet && (sp.isPet || sp.isFamiliar || sp.isMerc))
            {
                return true;
            }

            // Exclude players if requested
            if (excludePlayer && sp.IsPlayer)
            {
                return true;
            }

            // Exclude corpses if requested
            if (excludeCorpse && sp.isCorpse)
            {
                return true;
            }

            return false;
        }

        private bool CheckXY(Spawninfo sp, float x, float y, float delta)
        {
            return Math.Abs(sp.X - x) <= delta && Math.Abs(sp.Y - y) <= delta;
        }

        private Spawninfo FindMobTimer(string spawnLoc)
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

        public Spawntimer FindListViewTimer(ListViewItem listItem)
        {
            // This returns mobsTimer2
            foreach (Spawntimer st in MobsTimers.GetRespawned().Values)
            {
                if (st.ItmSpawnTimerList == listItem)
                {
                    return st;
                }
            }
            return null;
        }

        public Spawntimer FindTimer(float x, float y, float delta)
        {
            // This returns mobsTimer2
            foreach (Spawntimer st in MobsTimers.GetRespawned().Values)
            {
                if (st.X < x + delta && st.X > x - delta && st.Y < y + delta && st.Y > y - delta)
                {
                    return st;
                }
            }
            return null;
        }

        public GroundItem FindGroundItem(float x, float y, float delta)
        {
            foreach (GroundItem gi in itemcollection)
            {
                if (gi.Filtered)
                {
                    return null;
                }
                if (gi.X < x + delta && gi.X > x - delta && gi.Y < y + delta && gi.Y > y - delta)
                {
                    return gi;
                }
            }
            return null;
        }

        public Spawninfo GetSelectedMob() => (Spawninfo)mobsHashTable[SelectedID];

        public void InitLookups()
        {
            Classes = GetStrArrayFromTextFile("Classes.txt");
            Races = GetStrArrayFromTextFile("Races.txt");

            GroundSpawn.Clear();
            ReadItemList("GroundItems.ini", this);
            //guildList.Clear();

            //ReadGuildList(FileOps.CombineCfgDir("Guilds.txt"));
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

        public void CalcExtents(List<MapLine> lines)
        {
            if (Longname != "" && lines.Count > 0)
            {
                MaxMapX = MinmapX = lines[0].Point(0).X;

                MaxMapY = MinMapY = lines[0].Point(0).Y;

                MaxMapZ = MinMapZ = lines[0].Point(0).Z;

                foreach (MapLine mapLine in lines)
                {
                    ExtendMapLines(mapLine);
                }
            }
            else
            {
                MinmapX = -1000;

                MaxMapX = 1000;

                MinMapY = -1000;

                MaxMapY = 1000;

                MinMapZ = -1000;

                MaxMapZ = 1000;
            }
        }

        private void ExtendMapLines(MapLine mapLine)
        {
            foreach (MapPoint mapPoint in mapLine.APoints)
            {
                MaxMapX = Math.Max(MaxMapX, mapPoint.X);
                MinmapX = Math.Min(MinmapX, mapPoint.X);

                MaxMapY = Math.Max(MaxMapY, mapPoint.Y);
                MinMapY = Math.Min(MinMapY, mapPoint.Y);

                MaxMapZ = Math.Max(MaxMapZ, mapPoint.Z);
                MinMapZ = Math.Min(MinMapZ, mapPoint.Z);
            }
        }

        public void CheckMobs(ListViewPanel SpawnList, ListViewPanel GroundItemList)
        {
            var deletedGroundItems = new List<GroundItem>();
            var deletedSpawns = new List<Spawninfo>();
            var delListItems = new List<Spawninfo>();

            ProcessGroundItems(itemcollection, deletedGroundItems);

            foreach (Spawninfo sp in mobsHashTable.Values)
            {
                if (sp.delFromList)
                {
                    sp.delFromList = false;
                    delListItems.Add(sp);
                }
                else if (sp.ShouldBeDeleted)
                {
                    deletedSpawns.Add(sp);
                }
                else
                {
                    sp.ShouldBeDeleted = true;
                }
            }

            // Remove any that have been marked for deletion
            bool updateGroundList = Zoning || deletedGroundItems.Count > 2;
            bool updateSpawnList = deletedSpawns.Count > 5 || delListItems.Count > 2;

            if (updateGroundList)
            {
                GroundItemList.listView.BeginUpdate();
                RemoveGroundItems(GroundItemList, deletedGroundItems);
                GroundItemList.listView.EndUpdate();
            }

            if (updateSpawnList)
            {
                SpawnList.listView.BeginUpdate();
                RemoveDeadEntries(SpawnList, deletedSpawns, delListItems);
                SpawnList.listView.EndUpdate();
            }
        }

        private void ProcessGroundItems(List<GroundItem> collection, List<GroundItem> deletedItems)
        {
            foreach (var item in collection)
            {
                if (item.ShouldBeDeleted)
                {
                    deletedItems.Add(item);
                }
                else
                {
                    item.ShouldBeDeleted = true;
                }
            }
        }

        private void RemoveGroundItems(ListViewPanel listViewPanel, List<GroundItem> items)
        {
            foreach (var item in items)
            {
                listViewPanel.listView.Items.Remove(item.Listitem);
                item.Listitem = null;
                itemcollection.Remove(item);
            }
        }

        private void RemoveDeadEntries(ListViewPanel SpawnList, List<Spawninfo> deletedItems, List<Spawninfo> delListItems)
        {
            foreach (var sp in deletedItems)
            {
                SpawnList.listView.Items.Remove(sp.listitem);
                sp.listitem = null;

                mobsHashTable.Remove(sp.SpawnID); // Assuming mobsHashTable uses SpawnID as the key
            }

            foreach (var sp in delListItems)
            {
                SpawnList.listView.Items.Remove(sp.listitem);
            }
        }

        public void ProcessGroundItems(Spawninfo si)
        {
            try
            {
                var existingItem = itemcollection
                   .FirstOrDefault(gi => gi.Name == si.Name && gi.X == si.X && gi.Y == si.Y && gi.Z == si.Z);

                if (existingItem != null)
                {
                    // If the item exists, reset the 'Gone' property
                    existingItem.ShouldBeDeleted = false;
                }
                else
                {
                    var newItem = new GroundItem(si)
                    {
                        Desc = GetItemDescription(si.Name)
                    };
                    CheckGrounditemForAlerts(newItem, newItem.Desc.ToLower());
                    var listItem = new ListViewItem(newItem.Desc)
                    {
                        SubItems =
                        {
                            si.Name,
                            DateTime.Now.ToLongTimeString(),
                            si.X.ToString("#.000"),
                            si.Y.ToString("#.000"),
                            si.Z.ToString("#.000")
                        }
                    };
                    newItem.Listitem = listItem;
                    itemcollection.Add(newItem);
                    // Add it to the ground item list
                    NewGroundItems.Add(listItem);
                }
            }
            catch (Exception ex) { LogLib.WriteLine("Error in ProcessGroundItems(): ", ex); }
        }

        public string GetItemDescription(string ActorDef)
        {//sample:  IT0_ACTORDEF
            var lookupid = int.Parse(ActorDef.Remove(0, 2).Split('_')[0]);

            foreach (var item in GroundSpawn)
            {
                if (item.ID.Equals(lookupid))
            {
                return item.Name;
            }
            }
            return ActorDef;
        }

        public void ProcessTarget(Spawninfo si)
        {
            try
            {
                if (Settings.Default.AutoSelectEQTarget && EQSelectedID != si.SpawnID)
                {
                    EQSelectedID = SelectedID = si.SpawnID;

                    DefaultSpawnLoc();

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

        public void ProcessWorld(Spawninfo si)
        {
            /*  yy/mm/dd/hh/min
            gameYear = si.Race
            gameMonth = si.Hide
            gameDay = si.Levelafk
            gameHour = si.Type - 1
            gameMin = si.Class
           */
            Gametime = new DateTime(si.Race, si.Hide, si.Level, si.Type - 1, si.Class, 0);
        }

        public void ProcessSpawns(Spawninfo si, bool update_hidden)
        {
            try
            {
                var listReAdd = false;

                var found = false;

                // Converted mob collection to a hashtable so we can do
                // a fast lookup to see if a mob already exists

                if (mobsHashTable.ContainsKey(si.SpawnID))
                {
                    found = true;

                    Spawninfo mob = (Spawninfo)mobsHashTable[si.SpawnID];

                    mob.ShouldBeDeleted = false;

                    if (update_hidden)
                    {
                        mob.refresh = 12;
                    }

                    // some of these should not change often, so only check every 10 times through
                    if (mob.refresh >= 10)
                    {
                        if (mob.Type != si.Type)
                        {
                            mob.Type = si.Type;
                            UpdateMobTypes(mob);
                            SetListColors(mob);
                        }
                        if (mob.Level != si.Level)
                        {
                            mob.Level = si.Level;
                            mob.listitem.SubItems[1].Text = mob.Level.ToString();
                            MobLevelSetColor(mob);
                        }
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
                        Tainted_Egg(si);
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

                        if (mob.OwnerID != 0)
                        {
                            mob.OwnerID = si.OwnerID;
                            MobHasOwner(mob);
                            mob.Hide = si.Hide;
                            mob.listitem.SubItems[9].Text = si.Hide.GetHideStatus();
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
                    }

                    mob.refresh++;

                    // Set variables we dont want to trigger list update
                    UpdateMobPosition(si, mob);

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
                        PopulateListview(si, mob);
                    }

                    if (listReAdd)
                    {
                        NewSpawns.Add(mob.listitem);
                    }
                } // end of if found

                // If it's not already in there, add it

                if (!found && !string.IsNullOrEmpty(si.Name))
                {
                    SpawnNotFound(si);
                }
            }
            catch (Exception ex) { LogLib.WriteLine("Error in ProcessSpawns(): ", ex); }
        }

        private void UpdateMobPosition(Spawninfo si, Spawninfo mob)
        {
            if (SelectedID != mob.SpawnID)
            {
                if (mob.X != si.X)
                {
                    // ensure that map is big enough to show all spawns.
                    CheckBigMap(si);

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

        private void SpawnNotFound(Spawninfo si)
        {
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

            MobsTimers.Spawn(si);

            IsSpawnInFilterLists(si);
            mobsHashTable.Add(si.SpawnID, si);
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
            if (mobsHashTable.ContainsKey(mob.OwnerID))
            {
                Spawninfo owner = (Spawninfo)mobsHashTable[mob.OwnerID];

                if (owner.IsPlayer)
                {
                    mob.isPet = true;
                    mob.listitem.ForeColor = Color.Gray;
                }
                mob.listitem.SubItems[6].Text = owner.Name.FixMobName();
            }
            else
            {
                mob.listitem.SubItems[6].Text = mob.OwnerID.ToString();
                mob.isPet = false;
            }
        }

        private void MobLevelSetColor(Spawninfo mob)
        {
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
                SetListColors(mob);
            }
        }

        private void CheckBigMap(Spawninfo si)
        {
            if (MapPane.scale.Value == 100M && Settings.Default.AutoExpand)
            {
                if ((MinmapX > si.X) && (si.X > -20000))
                {
                    MinmapX = si.X;
                }

                if ((MaxMapX < si.X) && (si.X < 20000))
                {
                    MaxMapX = si.X;
                }

                if ((MinMapY > si.Y) && (si.Y > -20000))
                {
                    MinMapY = si.Y;
                }

                if ((MaxMapY < si.Y) && (si.Y < 20000))
                {
                    MaxMapY = si.Y;
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

            si.IsSpawnController(si);
            si.IsSpawnLDON(si);

            // Mercenary Identification - Only do it once now

            if (!string.IsNullOrEmpty(si.Lastname) && si.Lastname.IsMerc())
            {
                si.isMerc = true;
            }
            else if (si.Name.IsMount()) // Mounts
            {
                si.isMount = true;

                if (!Settings.Default.ShowMounts)
                {
                    si.hidden = true;
                }
            }
            else if (si.Name.IsFamiliar())
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

        private void PopulateListview(Spawninfo si, Spawninfo mob)
        {
            mob.X = si.X;
            mob.listitem.SubItems[13].Text = si.X.ToString();

            mob.Y = si.Y;
            mob.listitem.SubItems[14].Text = si.Y.ToString();

            mob.Z = si.Z;
            mob.listitem.SubItems[15].Text = si.Z.ToString();

            //if (Settings.Default.FollowOption == FollowOption.Target)
            //{
            //    f1.ReAdjust();
            //}

            mob.listitem.SubItems[16].Text = si.SpawnDistance(si, GamerInfo).ToString("#.00");
        }

        private void UpdateMobTypes(Spawninfo mob)
        {
            mob.listitem.SubItems[8].Text = mob.Type.GetSpawnType();

            if (mob.Type == 2 || mob.Type == 3)
            {
                mob.isCorpse = true;

                if (!CorpseAlerts)
                {
                    mob.isHunt = false;

                    mob.isCaution = false;

                    mob.isDanger = false;

                    mob.isAlert = false;
                }
            }
        }

        private void SetListColors(Spawninfo mob)
        {
            mob.listitem.ForeColor = SpawnColors.ConColors[mob.Level].Color;

            if (mob.listitem.ForeColor == Color.Maroon)
            {
                mob.listitem.ForeColor = Color.Red;
            }
            else if (Settings.Default.ListBackColor == Color.White)
            {
                // Change the colors to be more visible on white if the background is white

                mob.MakeVisOnWhite();
            }
        }

        private static void UpdateHidden(Spawninfo si, Spawninfo mob)
        {
            if (mob.isCorpse)
            {
                if (mob.IsPlayer)
                {
                    // My Corpse or Other Players' Corpses
                    si.hidden = mob.IsMyCorpse
                                ? !Settings.Default.ShowMyCorpse
                                : !Settings.Default.ShowPCCorpses;
                }
                else
                {
                    // Non-player Corpses
                    si.hidden = !Settings.Default.ShowCorpses;
                }
            }
            else if (mob.IsPlayer)
            {
                // Player Spawns
                si.hidden = !Settings.Default.ShowPlayers;
            }
            else
            {
                // Non-corpse, Non-player spawn (e.g., NPC)
                si.hidden = !Settings.Default.ShowNPCs // Hide all NPCs
                            || (si.isEventController && !Settings.Default.ShowInvis) // Invis Men
                            || (mob.isMount && !Settings.Default.ShowMounts) // Mounts
                            || (mob.isPet && !Settings.Default.ShowPets) // Pets
                            || (mob.isFamiliar && !Settings.Default.ShowFamiliars); // Familiars
            }

            if (si.hidden && !mob.hidden)
            {
                mob.delFromList = true;
            }

            mob.hidden = si.hidden;
        }

        private void IsSpawnInFilterLists(Spawninfo si)
        {
            var mobname = si.isMerc ? si.Name.FixMobNameMatch() : si.Name.FixMobName();

            var matchmobname = mobname.FixMobNameMatch();
            if (matchmobname.Length < 2)
            {
                matchmobname = mobname;
            }

            var mobnameWithInfo = mobname;

            SetWieldedNames(si);
            // Don't do alert matches for controllers, Ldon objects, pets, mercs, mounts, or familiars
            if (!(si.isLDONObject || si.IsPlayer || si.isEventController || si.isFamiliar || si.isMount || (si.isMerc && si.OwnerID != 0)))
            {
                AssignAlertStatus(si, matchmobname, ref mobnameWithInfo);

                //FormMethods.LookupBoxMatch(si, f1);
            }

            PlayAudioMatch(si, mobnameWithInfo);
            if (!si.hidden)
            {
                NewSpawns.Add(AddDetailsToList(si, mobnameWithInfo));
            }
        }

        private void SetWieldedNames(Spawninfo si)
        {
            if (si.Primary > 0 || si.Offhand > 0)
            {
                si.PrimaryName = ItemNumToString(si.Primary);
                si.OffhandName = ItemNumToString(si.Offhand);
            }
        }

        private ListViewItem AddDetailsToList(Spawninfo si, string mobnameWithInfo)
        {
            var listView = new ListViewItem(mobnameWithInfo);

            listView.SubItems.Add(si.Level.ToString());

            listView.SubItems.Add(GetClass(si.Class));

            listView.SubItems.Add(si.PrimaryName);
            listView.SubItems.Add(si.OffhandName);

            listView.SubItems.Add(GetRace(si.Race));

            listView.SubItems.Add(OwnerFlag(si));

            listView.SubItems.Add(si.Lastname);

            listView.SubItems.Add(si.Type.GetSpawnType());

            listView.SubItems.Add(si.Hide.GetHideStatus());

            listView.SubItems.Add(si.SpeedRun.ToString());

            listView.SubItems.Add(si.SpawnID.ToString());

            listView.SubItems.Add(DateTime.Now.ToLongTimeString());

            listView.SubItems.Add(si.X.ToString("#.00"));

            listView.SubItems.Add(si.Y.ToString("#.00"));

            listView.SubItems.Add(si.Z.ToString("#.00"));

            listView.SubItems.Add(si.SpawnDistance(si, GamerInfo).ToString("#.00"));

            //            item1.SubItems.Add(GuildNumToString(si.Guild));

            listView.SubItems.Add(si.Name.FixMobName());

            listView.ForeColor = GetSpawnListColors(si);

            si.ShouldBeDeleted = false;

            si.refresh = new Random().Next(0, 10);

            si.listitem = listView;
            SetListColors(si);
            return listView;
        }

        private Color GetSpawnListColors(Spawninfo si)
        {
            if (si.Type == 2 || si.Type == 3 || si.isLDONObject)
            {
                return Color.Gray;
            }
            else if (si.isEventController)
            {
                return Color.DarkOrchid;
            }
            else
            {
                return SpawnColors.ConColors[si.Level].Color;
            }
        }

        private void NameChngOrDead(Spawninfo si, Spawninfo mob)
        {
            var newname = si.Name.FixMobName();
            var oldname = mob.Name.FixMobName();

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
                MobsTimers.Kill(mob);
            }
            mob.Name = si.Name;
        }

        private string OwnerFlag(Spawninfo si)
        {
            if (si.OwnerID > 0)
            {
                if (mobsHashTable.ContainsKey(si.OwnerID))
                {
                    return ((Spawninfo)mobsHashTable[si.OwnerID]).Name.FixMobName();
                }
                else
                {
                    return si.OwnerID.ToString();
                }
            }
            else
            {
                return "";
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
                        MobLevelSetColor(si);
                    }
                }
            }
        }

        private bool CheckMyCorpse(string mobname) => (mobname.Length < (GamerInfo.Name.Length + 14)) && (mobname.IndexOf(GamerInfo.Name) == 0);

        public void SaveMobs()
        {
            var dt = DateTime.Now;

            var filename = $"{Longname} - {dt:MM-dd-yyyy-HH}.txt";

            // Use 'using' statement to ensure the StreamWriter is properly disposed of
            using (var sw = new StreamWriter(filename, false))
            {
                // Write header line
                sw.WriteLine("Name\t\tLevel\tClass\t\tRace\tLastname\t\tType\tInvis\tSpawnID\tX\tY\tZ");

                // Iterate over mobs and write their details to the file
                foreach (Spawninfo spawn in mobsHashTable.Values)
                {
                    var line = $"{spawn.Name}\t\t{spawn.Level}\t\t{GetClass(spawn.Class)}\t{GetRace(spawn.Race)}\t{spawn.Lastname}\t{spawn.Type.GetSpawnType()}\t{spawn.Hide.GetHideStatus()}\t{spawn.SpawnID}\t{spawn.Y}\t{spawn.X}\t{spawn.Z}";
                    sw.WriteLine(line);
                }
            }
        }

        public void SetSelectedID(int id)
        {
            SelectedID = id;
            DefaultSpawnLoc();
        }

        public void SetSelectedTimer(float x, float y)
        {
            Spawntimer st = FindTimer(x, y, 1.0f);

            if (st != null)
            {
                Spawninfo sp = FindMobTimer(st.SpawnLoc);

                SelectedID = sp == null ? 99999 : sp.SpawnID;

                SpawnX = st.X;

                SpawnY = st.Y;
            }
        }

        public void SetSelectedGroundItem(float x, float y)
        {
            GroundItem gi = FindGroundItem(x, y, 1.0f); // was FindGroundItemNoFilter

            if (gi != null)
            {
                SetGroundID(gi);
            }
        }

        public string GetClass(int num) => ArrayIndextoStr(Classes, num);

        private string ItemNumToString(int num)
        {
            for (int i = 0; i < GroundSpawn.Count; i++)
            {
                if (GroundSpawn[i].ID == num)
            {
                    return GroundSpawn[i].Name;
                }
            }
            return num.ToString();
        }

        private string ArrayIndextoStr(string[] source, int index) => index < source.GetLowerBound(0) || index > source.GetUpperBound(0) ? $"{index}: Unknown" : source[index];

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
                UpdateGamerInfo(si, f1);
                AdjustConLevel(si);
                UpdateConColorsIfNeeded();
            }
            catch (Exception ex)
            {
                LogLib.WriteLine("Error in ProcessGamer(): ", ex);
            }
        }

        private void UpdateGamerInfo(Spawninfo si, MainForm f1)
        {
            GamerInfo.SpawnID = si.SpawnID;
            GamerInfo.Name = si.Name;
            f1.SetCharSelection(GamerInfo.Name);
            f1.SetTitle();

            GamerInfo.Lastname = si.Lastname;
            GamerInfo.X = si.X;
            GamerInfo.Y = si.Y;
            GamerInfo.Z = si.Z;
            GamerInfo.Heading = si.Heading;
            GamerInfo.Hide = si.Hide;
            GamerInfo.SpeedRun = si.SpeedRun;
        }

        private void AdjustConLevel(Spawninfo si)
        {
            if (GamerInfo.Level != si.Level && GConBaseName.Length > 1)
            {
                gconLevel = Settings.Default.LevelOverride;
                int levelDifference = si.Level - GamerInfo.Level;
                gconLevel = MathClamp(gconLevel + levelDifference, 1, 125);

                Settings.Default.LevelOverride = gconLevel;
                GamerInfo.Level = si.Level;
                spawnColor.FillConColors(GamerInfo);
                UpdateMobListColors();
            }
        }

        private int MathClamp(int value, int min, int max)  // in lieu of having C# 8.0 code...
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        private void UpdateConColorsIfNeeded()
        {
            if (gLastconLevel != gconLevel)
            {
                gLastconLevel = gconLevel;
                spawnColor.FillConColors(GamerInfo);
                UpdateMobListColors();
            }
        }

        #endregion ProcessGamer

        public void Clear()
        {
            mobsHashTable.Clear();
            itemcollection.Clear();
        }

        public void ModKeyControl(MapCon mapCon, float x, float y)
        {
            var delta = 5.0f / mapCon.Ratio;

            Spawninfo sp = FindMob(x, y, delta, true) ?? FindMob(x, y, delta);

            if (sp != null)
            {
                Spawninfo st = FindMobTimer(sp.SpawnLoc);

                if (st == null)
                {
                    SetSelectedID(sp.SpawnID);

                    DefaultSpawnLoc();
                }
                else
                {
                    SetSelectedID(st.SpawnID);
                    Spawntimer spawntimer = FindTimer(st.X, st.Y, 1.0f);
                    if (spawntimer?.ItmSpawnTimerList != null)
                    {
                        spawntimer.ItmSpawnTimerList.Selected = true;
                        spawntimer.ItmSpawnTimerList.EnsureVisible();
                    }

                    SpawnX = st.X;
                    SpawnY = st.Y;
                }
            }
            else if (!SelectTimer(x, y, delta))
            {
                SelectGroundItem(x, y, delta);
            }
        }

        public void DefaultSpawnLoc()
        {
            SpawnX = -1.0f;
            SpawnY = -1.0f;
        }

        private bool AffixStars = true;
        private string HuntPrefix = "";
        private string AlertPrefix = "";
        private string DangerPrefix = "";
        private string CautionPrefix = "";

        private bool FullTxtA;
        private bool FullTxtC;
        private bool FullTxtD;
        private bool FullTxtH;
        private bool Prefix = true;

        private static void AudioMatch(string mobname, string TalkDescr, bool TalkOnMatch, bool PlayOnMatch, bool BeepOnMatch, string AudioFile)
        {
            if (TalkOnMatch)
            {
                ThreadStart threadDelegate = new ThreadStart(new Talker
                {
                    SpeakingText = $"{TalkDescr}, {mobname.SearchName()}, is up."
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

        private bool FindMatches(List<string> filterlist, string mobname, bool MatchFullText)
        {
            foreach (string str in filterlist)
            {
                var matched = false;

                // if "match full text" is ON...

                if (MatchFullText)
                {
                    if (string.Compare(mobname, str, true) == 0)
                    {
                        matched = true;
                    }
                }
                else if (RegexHelper.IsSubstring(mobname, str))
                {
                    matched = true;
                }
                // if item has been matched...

                if (matched)
                {
                    return true;
                }
            }

            return false;
        }

        private string PrefixAffixLabel(string mname, string prefix)
        {
            if (Prefix)
            {
                mname = $"{prefix} {mname}";
            }

            if (AffixStars)
            {
                mname += $" {prefix}";
            }

            return mname;
        }

        private void AssignAlertStatus(Spawninfo si, string matchmobname, ref string mobnameWithInfo)
        {
            if (!si.isCorpse || CorpseAlerts)
            {
                if (FindMatches(Filters.Hunt, matchmobname, FullTxtH) || FindMatches(Filters.GlobalHunt, matchmobname, FullTxtH))
                {
                    si.isHunt = true;
                    mobnameWithInfo = PrefixAffixLabel(mobnameWithInfo, HuntPrefix);
                }

                // [caution]
                if (FindMatches(Filters.Caution, matchmobname, FullTxtC) || FindMatches(Filters.GlobalCaution, matchmobname, FullTxtC))
                {
                    si.isCaution = true;
                    mobnameWithInfo = PrefixAffixLabel(mobnameWithInfo, CautionPrefix);
                }

                // [danger]
                if (((!si.isCorpse || CorpseAlerts) && FindMatches(Filters.Danger, matchmobname, FullTxtD)) || FindMatches(Filters.GlobalDanger, matchmobname, FullTxtD))
                {
                    si.isDanger = true;
                    mobnameWithInfo = PrefixAffixLabel(mobnameWithInfo, DangerPrefix);
                }

                // [rare]
                if (FindMatches(Filters.Alert, matchmobname, FullTxtA) || FindMatches(Filters.GlobalAlert, matchmobname, FullTxtA))
                {
                    si.isAlert = true;
                    mobnameWithInfo = PrefixAffixLabel(mobnameWithInfo, AlertPrefix);
                }
                // [Email]
                //if (filters.EmailAlert.Count > 0 && !si.isCorpse && FindMatches(filters.EmailAlert, matchmobname, true))
                //{
                //    // Flag on map as an alert mob
                //    si.isAlert = true;
                //}

                // [Wielded Items]
                // Acts like a hunt mob.
                if (FindMatches(Filters.WieldedItems, si.PrimaryName, FullTxtH) || FindMatches(Filters.WieldedItems, si.OffhandName, FullTxtH))
                {
                    si.isHunt = true;
                    mobnameWithInfo = PrefixAffixLabel(mobnameWithInfo, HuntPrefix);
                }
            }
        }

        private void CheckGrounditemForAlerts(GroundItem gi, string itemname)
        {
            // [hunt]
            if (FindMatches(Filters.Hunt, itemname, FullTxtH) || FindMatches(Filters.GlobalHunt, itemname, FullTxtH))
            {
                gi.IsHunt = true;
            }

            // [caution]
            if (FindMatches(Filters.Caution, itemname, FullTxtC) || FindMatches(Filters.GlobalCaution, itemname, FullTxtC))
            {
                gi.IsCaution = true;
            }

            // [danger]
            if (FindMatches(Filters.Danger, itemname, FullTxtD) || FindMatches(Filters.GlobalDanger, itemname, FullTxtD))
            {
                gi.IsDanger = true;
            }

            // [rare]
            if (FindMatches(Filters.Alert, itemname, FullTxtA) || FindMatches(Filters.GlobalAlert, itemname, FullTxtA))
            {
                gi.IsAlert = true;
            }
        }

        public void LoadSpawnInfo()
        {
            // Used to improve packet processing speed

            Prefix = Settings.Default.PrefixStars;

            AffixStars = Settings.Default.AffixStars;

            CorpseAlerts = Settings.Default.CorpseAlerts;

            FullTxtH = Settings.Default.MatchFullTextH;

            FullTxtC = Settings.Default.MatchFullTextC;

            FullTxtD = Settings.Default.MatchFullTextD;

            FullTxtA = Settings.Default.MatchFullTextA;

            HuntPrefix = Settings.Default.HuntPrefix;

            CautionPrefix = Settings.Default.CautionPrefix;

            DangerPrefix = Settings.Default.DangerPrefix;

            AlertPrefix = Settings.Default.AlertPrefix;
        }

        private void PlayAudioMatch(Spawninfo si, string matchmobname)
        {
            if (Settings.Default.playAlerts)
            {
                if (si.isHunt && !Settings.Default.NoneOnHunt)
                {
                    AudioMatch(matchmobname, "Hunt Mob", Settings.Default.TalkOnHunt, Settings.Default.PlayOnHunt, Settings.Default.BeepOnHunt, Settings.Default.HuntAudioFile);
                }
                if (si.isCaution && !Settings.Default.NoneOnCaution)
                {
                    AudioMatch(matchmobname, "Caution Mob", Settings.Default.TalkOnCaution, Settings.Default.PlayOnCaution, Settings.Default.BeepOnCaution, Settings.Default.CautionAudioFile);
                }
                if (si.isDanger && !Settings.Default.NoneOnDanger)
                {
                    AudioMatch(matchmobname, "Danger Mob", Settings.Default.TalkOnDanger, Settings.Default.PlayOnDanger, Settings.Default.BeepOnDanger, Settings.Default.DangerAudioFile);
                }
                if (si.isAlert && !Settings.Default.NoneOnAlert)
                {
                    AudioMatch(matchmobname, "Rare Mob", Settings.Default.TalkOnAlert, Settings.Default.PlayOnAlert, Settings.Default.BeepOnAlert, Settings.Default.AlertAudioFile);
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
                    maptxt.Draw_color = Settings.Default.ForceDistinctText ? GetDistinctColor(new SolidBrush(Color.Black)) : GetDistinctColor(maptxt.LineColor);
                    maptxt.Draw_pen = new Pen(maptxt.Draw_color.Color);
                }
            }
        }

        private void SetMaplineColor(int alpha, MapLine mapline)
        {
            if (Settings.Default.ForceDistinct)
            {
                mapline.Draw_color = GetDistinctColor(new Pen(Color.Black));
                mapline.Fade_color = new Pen(Color.FromArgb(alpha, mapline.Draw_color.Color));
            }
            else
            {
                mapline.Draw_color = GetDistinctColor(new Pen(mapline.LineColor.Color));
                mapline.Fade_color = new Pen(Color.FromArgb(alpha, mapline.Draw_color.Color));
            }
        }

        public SolidBrush GetDistinctColor(SolidBrush curBrush)
        {
            curBrush.Color = GetDistinctColor(curBrush.Color, Settings.Default.BackColor);

            return curBrush;
        }

        private Color GetDistinctColor(Color foreColor, Color backColor)
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

        #endregion ColorOperations
    }
}