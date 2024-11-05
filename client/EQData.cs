using myseq.Properties;
using Structures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myseq
{
    // This is the "model" part - no UI related things in here, only hard EQ data.

    public class EQData
    {
        private static readonly Spawninfo spawninformation = new Spawninfo();

        public readonly SpawnColors spawnColor = new SpawnColors();

        // player details
        public Spawninfo GamerInfo { get; set; } = spawninformation;

        // Map details
        public string Longname { get; set; } = "";

        public string Shortname { get; set; } = "";

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

                SpawnX = st.Location.X;
                SpawnY = st.Location.Y;

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

            SpawnX = gi.ItemLocation.X;

            SpawnY = gi.ItemLocation.Y;
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

                if (Math.Abs(sp.X - x) <= delta && Math.Abs(sp.Y - y) <= delta)
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
                if (st.Location.X < x + delta && st.Location.X > x - delta && st.Location.Y < y + delta && st.Location.Y > y - delta)
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
                if (gi.ItemLocation.X < x + delta && gi.ItemLocation.X > x - delta && gi.ItemLocation.Y < y + delta && gi.ItemLocation.Y > y - delta)
                {
                    return gi;
                }
            }
            return null;
        }

        public Spawninfo GetSelectedMob() => (Spawninfo)mobsHashTable[SelectedID];

        public void InitLookups()
        {
            Classes = FileOps.GetStrArrayFromTextFile("Classes.txt");
            Races = FileOps.GetStrArrayFromTextFile("Races.txt");

            GroundSpawn.Clear();
            FileOps.ReadIniFile("GroundItems.ini", this);
            //guildList.Clear();

            //FileOps.ReadGuildList("Guilds.txt");
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
                   .FirstOrDefault(gi => gi.Name == si.Name && gi.ItemLocation.X == si.X && gi.ItemLocation.Y == si.Y && gi.ItemLocation.Z == si.Z);

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
                        GetMobLevel(si, mob);
                        // Update Hidden flags
                        listReAdd = GetMobHidden(si, update_hidden, listReAdd, mob);

                        // check if the mob name has changed - eg when a mob dies.
                        if ((si.Name.Length > 0) && (string.Compare(mob.Name, si.Name) != 0))
                        {
                            NameChngOrDead(si, mob);
                        }
                        Tainted_Egg(si);
                        GetMobClass(si, mob);

                        GetMobPrimary(si, mob);

                        GetMobOffhand(si, mob);

                        GetMobRace(si, mob);

                        GetMobOwner(si, mob);

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

        private void GetMobLevel(Spawninfo si, Spawninfo mob)
        {
            if (mob.Level != si.Level)
            {
                mob.Level = si.Level;
                mob.listitem.SubItems[1].Text = mob.Level.ToString();
                MobLevelSetColor(mob);
            }
        }

        private bool GetMobHidden(Spawninfo si, bool update_hidden, bool listReAdd, Spawninfo mob)
        {
            if (update_hidden)
            {
                UpdateHidden(si, mob);
                if (!si.hidden && mob.hidden)
                {
                    listReAdd = true;
                }
            }

            return listReAdd;
        }

        private void GetMobOwner(Spawninfo si, Spawninfo mob)
        {
            if (mob.OwnerID != 0)
            {
                mob.OwnerID = si.OwnerID;
                MobHasOwner(mob);
                mob.Hide = si.Hide;
                mob.listitem.SubItems[9].Text = si.Hide.GetHideStatus();
            }
        }

        private void GetMobRace(Spawninfo si, Spawninfo mob)
        {
            if (mob.Race != si.Race)
            {
                mob.Race = si.Race;
                mob.listitem.SubItems[5].Text = GetRace(si.Race);
            }
        }

        private void GetMobOffhand(Spawninfo si, Spawninfo mob)
        {
            if (mob.Offhand != si.Offhand)
            {
                mob.Offhand = si.Offhand;
                mob.listitem.SubItems[4].Text = si.Offhand > 0 ? ItemNumToString(si.Offhand) : "";
            }
        }

        private void GetMobPrimary(Spawninfo si, Spawninfo mob)
        {
            if (mob.Primary != si.Primary)
            {
                mob.Primary = si.Primary;
                mob.listitem.SubItems[3].Text = si.Primary > 0 ? ItemNumToString(si.Primary) : "";
            }
        }

        private void GetMobClass(Spawninfo si, Spawninfo mob)
        {
            if (mob.Class != si.Class)
            {
                mob.Class = si.Class;
                mob.listitem.SubItems[2].Text = GetClass(si.Class);
            }
        }

        private void UpdateMobPosition(Spawninfo si, Spawninfo mob)
        {
            if (SelectedID != mob.SpawnID)
            {
                if (mob.X != si.X)
                {
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

                si.MyPlayer = true;

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
            // Start with the assumption that mob is not a pet
            mob.isPet = false;

            // Check if mob has an owner in the hash table
            if (mobsHashTable.ContainsKey(mob.OwnerID))
            {
                Spawninfo owner = mobsHashTable[mob.OwnerID] as Spawninfo;

                // Ensure the retrieved owner is not null before proceeding
                if (owner != null)
                {
                    // If the owner is a player, designate the mob as a pet
                    if (owner.IsPlayer)
                    {
                        SetMobAsPet(mob);
                    }

                    // Set the owner's name in the UI
                    mob.listitem.SubItems[6].Text = owner.Name.FixMobName();
                    mob.listitem.SubItems[11].Text = mob.SpawnID.ToString();
                    return;
                }
            }
            // If no valid owner was found, display the OwnerID as a fallback
            mob.listitem.SubItems[6].Text = mob.OwnerID.ToString();
            mob.listitem.SubItems[11].Text = mob.SpawnID.ToString();
        }

        private void SetMobAsPet(Spawninfo mob)
        {
            mob.isPet = true;
            mob.listitem.ForeColor = Color.Gray;
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
                si.MyPlayer = true;

                if (!Settings.Default.ShowPCCorpses)
                {
                    si.hidden = true;
                }

                if (si.Name.Length > 0 && CheckMyCorpse(si.Name))
                {
                    si.MyCorpse = true;

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

        private bool IsSpecialSpawn(Spawninfo si)
        {
            return si.isLDONObject || si.IsPlayer || si.isEventController || si.isFamiliar || si.isMount || (si.isMerc && si.OwnerID != 0);
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
            if (!IsSpecialSpawn(si))
            {
                AssignAlertStatus(si, matchmobname, ref mobnameWithInfo);
            }

            PlayAudioMatch(si, mobnameWithInfo);
            if (!si.hidden)
            {
                NewSpawns.Add(AddDetailsToList(si, mobnameWithInfo));
            }
        }

        private void SetWieldedNames(Spawninfo si)
        {
            // Set names based on whether there are valid items in Primary and Offhand
            if (si.Primary > 0)
            {
                si.PrimaryName = ItemNumToString(si.Primary);
            }
            else
            {
                si.PrimaryName = string.Empty; // Reset if no valid Primary item
            }

            if (si.Offhand > 0)
            {
                si.OffhandName = ItemNumToString(si.Offhand);
            }
            else
            {
                si.OffhandName = string.Empty; // Reset if no valid Offhand item
            }
        }

        private ListViewItem AddDetailsToList(Spawninfo si, string mobnameWithInfo)
        {
            // Create a new ListViewItem with the main text (mob name with info)
            var listViewItem = new ListViewItem(mobnameWithInfo)
            {
                ForeColor = GetSpawnListColors(si) // Set the initial color for the list item
            };

            // Create a list of subitems with formatted values and add them in one go
            var subItems = new List<string>
    {
        si.Level.ToString(),                   // Level
        GetClass(si.Class),                    // Class
        si.PrimaryName,                        // Primary weapon name
        si.OffhandName,                        // Offhand weapon name
        GetRace(si.Race),                      // Race
        OwnerFlag(si),                         // Ownership flag
        si.Lastname,                           // Last name or additional name
        si.Type.GetSpawnType(),                // Spawn type description
        si.Hide.GetHideStatus(),               // Hide status (visible, hidden, etc.)
        si.SpeedRun.ToString(),                // Running speed of the spawn
        si.SpawnID.ToString(),                 // Spawn ID
        DateTime.Now.ToLongTimeString(),       // Current time
        si.X.ToString("#.00"),                 // X coordinate formatted
        si.Y.ToString("#.00"),                 // Y coordinate formatted
        si.Z.ToString("#.00"),                 // Z coordinate formatted
        si.SpawnDistance(si, GamerInfo).ToString("#.00"), // Distance to gamer
        si.Name.FixMobName()                   // Fixed or adjusted mob name
                                               // ,GuildNumToString(si.Guild);
    };

            // Add all the subitems at once to reduce repetitive code
            listViewItem.SubItems.AddRange(subItems.ToArray());

            // Update Spawninfo state if necessary; otherwise, move this to another method
            PrepareSpawninfoForList(si, listViewItem);

            return listViewItem;
        }

        // Refactored helper method to encapsulate Spawninfo state updates
        private void PrepareSpawninfoForList(Spawninfo si, ListViewItem listViewItem)
        {
            si.ShouldBeDeleted = false;
            si.refresh = new Random().Next(0, 10);  // Use a shared Random instance instead of creating a new one
            si.listitem = listViewItem;
            SetListColors(si);  // Additional logic to set colors, if needed
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
            if (si.OwnerID <= 0)
            {
                return "";
            }

            if (mobsHashTable.ContainsKey(si.OwnerID))
            {
                return ((Spawninfo)mobsHashTable[si.OwnerID]).Name.FixMobName();
            }

            return si.OwnerID.ToString();
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

                SpawnX = st.Location.X;

                SpawnY = st.Location.Y;
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

        public string GetRace(int num) => num == 2250 ? "Interactive Object" : ArrayIndextoStr(Races, num);

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

        public void BeginProcessPacket()
        {
            NewSpawns.Clear();
            NewGroundItems.Clear();
        }

        public void ProcessSpawnList(ListViewPanel SpawnList)
        {
            if (NewSpawns.Count == 0) return;
            try
            {
                if (Zoning)
                {
                    SpawnList.listView.BeginUpdate();
                }

                var items = new List<ListViewItem>(NewSpawns.Count);
                foreach (var spawn in NewSpawns)
                {
                    if (spawn != null)
                    {
                        items.Add(spawn);
                    }
                }
                if (items.Count > 0)
                { SpawnList.listView.Items.AddRange(items.ToArray()); }
                NewSpawns.Clear();
            }
            catch (Exception ex) { LogLib.WriteLine("Error in ProcessSpawnList(): ", ex); }
            finally
            {
                // Always end the update if Zoning was active
                if (Zoning)
                {
                    SpawnList.listView.EndUpdate();
                }
            }
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

        private bool Prefix = true;
        private bool Affix = true;
        private string HuntPrefix = "";
        private string AlertPrefix = "";
        private string DangerPrefix = "";
        private string CautionPrefix = "";

        private bool FullTxtA;
        private bool FullTxtC;
        private bool FullTxtD;
        private bool FullTxtH;

        private static void AudioMatch(string mobname, string TalkDescr, bool TalkOnMatch, bool PlayOnMatch, bool BeepOnMatch, string AudioFile)
        {
            if (TalkOnMatch)
            {
                var talker = new Talker($"{TalkDescr}, {mobname.SearchName()}, is up.");
                Task.Run(() => talker.SpeakText());
            }
            else if (PlayOnMatch)
            {
                if (!string.IsNullOrEmpty(AudioFile) && File.Exists(AudioFile))
                {
                    using (SoundPlayer player = new SoundPlayer(AudioFile))
                    {
                        player.Play(); // Play the WAV file asynchronously
                    }
                }
            }
            else if (BeepOnMatch)
            {
                Console.Beep(300, 100);
            }
        }

        private bool FindMatches(List<string> filterList, string mobName, bool matchFullText)
        {
            if (!string.IsNullOrEmpty(mobName))
            {
                // If matchFullText is true, use case-insensitive string comparison.
                // Otherwise, check if any filter item is a substring within mobName.
                var mobNameLower = mobName.ToLower();
                return filterList.Any(filter =>
                    matchFullText ? mobNameLower.Equals(filter.ToLower())
                                  : mobNameLower.Contains(filter.ToLower()));
            }
            else return false;
        }

        private string PrefixAffixLabel(string mname, string prefix)
        {
            if (Prefix)
            {
                mname = $"{prefix} {mname}";
            }

            if (Affix)
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

            Affix = Settings.Default.AffixStars;

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

        private static void PlayAudioMatch(Spawninfo si, string matchmobname)
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

        public void CalculateMapLinePens(List<LineSegment> lines, List<MapLabel> texts)
        {
            if (lines == null) return;

            foreach (LineSegment mapline in lines)
            {
                SetMaplineColor(mapline);
            }
            foreach (MapLabel maptxt in texts)
            {
                maptxt.TextColor = GetDistinctColor(maptxt.TextColor);
            }
        }

        private void SetMaplineColor(LineSegment mapline)
        {
            if (Settings.Default.ForceDistinct)
            {
                mapline.LineColor = GetDistinctColor(Color.Black);
            }
            else
            {
                mapline.LineColor = GetDistinctColor(mapline.LineColor);
                ////TODO make alpha filtering.
            }
        }

        private Color GetDistinctColor(Color foreColor, Color backColor)
        {
            const int ColorThreshold = 55;

            if (GetColorDiff(foreColor, backColor) >= ColorThreshold)
            {
                return foreColor;
            }

            var inverseColor = GetInverseColor(foreColor);

            if (GetColorDiff(inverseColor, backColor) > ColorThreshold)
            {
                return inverseColor;
            }

            return Color.Black;
        }

        public Pen GetDistinctColor(Pen curPen)
        {
            curPen.Color = GetDistinctColor(curPen.Color, Settings.Default.BackColor);
            return curPen;
        }

        public Color GetDistinctColor(Color curColor) => GetDistinctColor(curColor, Settings.Default.BackColor);

        private int GetColorDiff(Color color1, Color color2)
        {
            int diffR = Math.Abs(color1.R - color2.R);
            int diffG = Math.Abs(color1.G - color2.G);
            int diffB = Math.Abs(color1.B - color2.B);

            return Math.Max(diffR, Math.Max(diffG, diffB));
        }

        private Color GetInverseColor(Color color)
        {
            return Color.FromArgb(
                (int)(192 - color.R * 0.75),
                (int)(192 - color.G * 0.75),
                (int)(192 - color.B * 0.75));
        }

        #endregion ColorOperations
    }
}