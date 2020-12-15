using System;

using System.IO;

using SpeechLib;

using System.Data;

using System.Text;

using System.Drawing;

using System.Collections;

using System.Globalization;

using System.Windows.Forms;

using System.ComponentModel;

using System.Drawing.Drawing2D;

using System.Text.RegularExpressions;

using System.Runtime.InteropServices;



using Structures;



namespace myseq {



    #region SPAWNTIMER class

    //[StructLayout(LayoutKind.Sequential, Pack=1)]

    public class SPAWNTIMER {

        public SPAWNTIMER(SPAWNINFO si, DateTime dt) 

        {

            this.SpawnLoc=si.SpawnLoc;

            this.ZoneSpawnLoc = si.ZoneSpawnLoc;

            this.zone = "";

            this.X = si.X;

            this.Y = si.Y;

            this.Z = si.Z;

            this.SpawnTimeDT = dt;

            this.SpawnTimeStr = dt.ToLongTimeString() + " " + dt.ToShortDateString();

            this.SpawnCount = 1;

            this.LastSpawnName = si.Name;

            this.AllNames = Regex.Replace(si.Name.Replace("_", " "), "[0-9]", "").Trim();

        }



        public SPAWNTIMER(string str)

        {

            string[] parts = str.Split(';');

            SpawnLoc = parts[0];

            SpawnCount = int.Parse(parts[1]);

            SpawnTimer = int.Parse(parts[2]);

            SpawnTimeStr = parts[3];

            if (SpawnTimeStr.Length>0) SpawnTimeDT = Convert.ToDateTime(SpawnTimeStr);

            KillTimeStr = parts[4];

            if (KillTimeStr.Length>0) KillTimeDT = Convert.ToDateTime(KillTimeStr);

            NextSpawnStr = parts[5];

            if (NextSpawnStr.Length>0) NextSpawnDT = Convert.ToDateTime(NextSpawnStr);

            LastSpawnName = parts[6];

            AllNames = parts[7];

            // split up the All Names, and parse out spawn numbers in parenthesis
            string[] subparts = AllNames.Split(',');

            int jj = subparts.GetUpperBound(0);

            X = float.Parse(parts[8]);

            Y = float.Parse(parts[9]);

            Z = float.Parse(parts[10]);

        }

        

        // Returns the data in a format which can be used with the (string) constructor.

        public string GetAsString()

        {

            return SpawnLoc+";"+SpawnCount+";"+SpawnTimer+";"+SpawnTimeStr+";"+KillTimeStr+";"+

                NextSpawnStr+";"+LastSpawnName+";"+AllNames+";"+X+";"+Y+";"+Z;

        }



        // st has been loaded from a file, and is the same spawn as "this" one. 

        // Glean all useful information.

        public void Merge(SPAWNTIMER st)

        {

            LogLib.WriteLine("Merging spawn timers:",LogLevel.Debug);

                

            LogLib.WriteLine("  Old: "+GetAsString(),LogLevel.Debug);

            LogLib.WriteLine("  Other: "+st.GetAsString(),LogLevel.Debug);

            

            this.SpawnCount = st.SpawnCount; // usually makes it > 1

            this.SpawnTimer = st.SpawnTimer; // woot!

            //this.SpawnTimer =   // NOT!

            if (this.KillTimeDT == DateTime.MinValue) // woot!

            {

                this.KillTimeStr = st.KillTimeStr;

                

                TimeSpan Diff = new TimeSpan(0, 0, 0, Convert.ToInt32(SpawnTimer));                                       

                NextSpawnDT = KillTimeDT.Add(Diff);

                NextSpawnStr = NextSpawnDT.ToLongTimeString() + " " + NextSpawnDT.ToShortDateString();

            }

            else

            {

                // Enable the timer to start on first kill

                if (st.SpawnTimer > 10)

                {

                    //TimeSpan Diff = new TimeSpan(0, 0, 0, Convert.ToInt32(SpawnTimer));

                    //this.KillTimeDT = this.SpawnTimeDT.Subtract(Diff);

                    TimeSpan Diff = new TimeSpan(0, 0, 0, Convert.ToInt32(SpawnTimer));

                    if (DateTime.Now.Subtract(Diff) < st.SpawnTimeDT)

                    {

                        this.SpawnTimeDT = st.SpawnTimeDT;

                        this.SpawnTimeStr = st.SpawnTimeStr;

                    }



                    if (DateTime.Now.Subtract(Diff) > st.KillTimeDT)

                    {

                        this.KillTimeDT = DateTime.MinValue;

                    }

                    else

                    {

                        this.KillTimeDT = st.KillTimeDT;

                        this.KillTimeStr = st.KillTimeStr;

                    }

                    //this.KillTimeStr = "";

                    if (DateTime.Now > st.NextSpawnDT)

                    {

                        this.NextSpawnDT = DateTime.MinValue;

                        this.NextSpawnStr = "";

                    }

                    else

                    {

                        this.NextSpawnDT = st.NextSpawnDT;

                        this.NextSpawnStr = st.NextSpawnStr;

                        this.KillTimeDT = st.KillTimeDT.Subtract(Diff);

                    }

                }

            }

            int namecount = 1;            

            string[] names = st.AllNames.Split(',');

            foreach (string name in names)

            {

                string bname = Regex.Replace(name.Replace("_", " "), "[0-9]", "").Trim();

                if (AllNames.IndexOf(bname) < 0)

                {

                    if (namecount < 11)

                    {

                        AllNames += ", " + bname;

                        namecount++;

                    }

                }

            }

            // update last spawn name to be what looks like named mobs
            string[] bnames = AllNames.Split(',');
            foreach (string tname in bnames)
            {
                string mname = Regex.Replace(tname.Replace("_", " "), "[0-9]", "").Trim();
                if (Regex.IsMatch(mname, "^[A-Z#]"))
                {
                    this.LastSpawnName = mname;
                    break;
                }
            }

            listNeedsUpdate = true;

            

            LogLib.WriteLine("  New: "+GetAsString(),LogLevel.Debug);

            

        }

        

        // When will the mob spawn next? Returns 0 if not available.

        // TODO: optimize this, as it is called much more often than the mob is being updated

        public int SecondsUntilSpawn(DateTime now)

        {

            int checkTimer=0;

            

            if (NextSpawnDT != DateTime.MinValue) 

            {

                TimeSpan Diff = NextSpawnDT.Subtract(now);

                checkTimer = (Diff.Hours * 3600) + (Diff.Minutes * 60) + Diff.Seconds;

                if (checkTimer<=0)

                {

                    checkTimer=0;

                }

            }       

            

            return checkTimer;

        }               

        

        public String GetDescription()

        {

            String descr = null;

        

            int countTime = 0;

            string countTimer = "";

            if (NextSpawnDT != DateTime.MinValue) {

                TimeSpan Diff = NextSpawnDT.Subtract(DateTime.Now);

                countTimer = Diff.Hours.ToString("00") + ":" + Diff.Minutes.ToString("00") + ":" + Diff.Seconds.ToString("00");

                countTime = (Diff.Hours * 3600) + (Diff.Minutes * 60) + Diff.Seconds;

            }



            if (countTime > 0)

            {

                StringBuilder spawnTimer = new StringBuilder();

                spawnTimer.AppendFormat("Spawn Name: {0}\n", LastSpawnName);

                string names_to_add = "Names encountered: ";
                string[] names = AllNames.Split(',');

                int namecount = 0;

                foreach (string name in names)
                {
                    string namet = Regex.Replace(name.Replace("_", " "), "[0-9]", "").Trim();

                    if (namecount == 0)
                    {
                        names_to_add += namet;
                    }
                    else
                    {
                        if ((namet.Length + names_to_add.Length + 2) < 45)
                        {
                            names_to_add += ", ";
                            names_to_add += namet;
                        }
                        else
                        {
                            spawnTimer.Append(names_to_add);
                            spawnTimer.Append("\n");
                            names_to_add = namet;
                        }
                    }

                    namecount++;

                }
                if (names_to_add.Length > 0)
                {
                    spawnTimer.Append(names_to_add);
                }

                spawnTimer.Append("\n");

                spawnTimer.AppendFormat("Last Spawned At: {0}\n", SpawnTimeStr);

                spawnTimer.AppendFormat("Last Killed At: {0}\n", KillTimeStr);

                spawnTimer.AppendFormat("Next Spawn At: {0}\n", NextSpawnStr);

                spawnTimer.AppendFormat("Spawn Timer: {0} secs\n", SpawnTimer);

                spawnTimer.AppendFormat("Spawning In: {0}\n", countTimer);

                spawnTimer.AppendFormat("Spawn Count: {0}\n", SpawnCount);

                spawnTimer.AppendFormat("Y: {0:f3}  X: {1:f3}  Z: {2:f3}", Y, X, Z);

                descr = spawnTimer.ToString();

            }

            else if (SpawnTimer > 0)

            {

                StringBuilder spawnTimer = new StringBuilder();

                spawnTimer.AppendFormat("Spawn Name: {0}\n", LastSpawnName);

                string names_to_add = "Names encountered: ";
                string[] names = AllNames.Split(',');

                int namecount = 0;

                foreach (string name in names)
                {
                    string namet = Regex.Replace(name.Replace("_", " "), "[0-9]", "").Trim();

                    if (namecount == 0)
                    {
                        names_to_add += namet;
                    }
                    else
                    {
                        if ((namet.Length + names_to_add.Length + 2) < 45)
                        {
                            names_to_add += ", ";
                            names_to_add += namet;
                        }
                        else
                        {
                            spawnTimer.Append(names_to_add);
                            spawnTimer.Append("\n");

                            names_to_add = namet;
                        }
                    }

                    namecount++;

                }
                if (names_to_add.Length > 0)
                {
                    spawnTimer.Append(names_to_add);
                }

                spawnTimer.Append("\n");

                spawnTimer.AppendFormat("Last Spawned At: {0}\n", SpawnTimeStr);

                spawnTimer.AppendFormat("Last Killed At: {0}\n", KillTimeStr);

                spawnTimer.AppendFormat("Next Spawn At: {0}\n", "");

                spawnTimer.AppendFormat("Spawn Timer: {0} secs\n", SpawnTimer);

                spawnTimer.AppendFormat("Spawning In: {0}\n", "");

                spawnTimer.AppendFormat("Spawn Count: {0}\n", SpawnCount);

                spawnTimer.AppendFormat("Y: {0:f3}  X: {1:f3}  Z: {2:f3}", Y, X, Z);

                descr = spawnTimer.ToString();

            }

            else

            {

                StringBuilder spawnTimer = new StringBuilder();

                spawnTimer.AppendFormat("Spawn Name: {0}\n", LastSpawnName);

                string names_to_add = "Names encountered: ";
                string[] names = AllNames.Split(',');

                int namecount = 0;

                foreach (string name in names)
                {
                    string namet = Regex.Replace(name.Replace("_", " "), "[0-9]", "").Trim();

                    if (namecount == 0)
                    {
                        names_to_add += namet;
                    }
                    else
                    {
                        if ((namet.Length + names_to_add.Length + 2) < 45)
                        {
                            names_to_add += ", ";
                            names_to_add += namet;
                        }
                        else
                        {
                            spawnTimer.Append(names_to_add);
                            spawnTimer.Append("\n");

                            names_to_add = namet;
                        }
                    }

                    namecount++;

                }
                if (names_to_add.Length > 0)
                {
                    spawnTimer.Append(names_to_add);
                }

                spawnTimer.Append("\n");

                spawnTimer.AppendFormat("Last Spawned At: {0}\n", SpawnTimeStr);

                spawnTimer.AppendFormat("Last Killed At: {0}\n", KillTimeStr);

                spawnTimer.AppendFormat("Next Spawn At: {0}\n", "");

                spawnTimer.AppendFormat("Spawn Timer: {0} secs\n", "0");

                spawnTimer.AppendFormat("Spawning In: {0}\n", "");

                spawnTimer.AppendFormat("Spawn Count: {0}\n", SpawnCount);

                spawnTimer.AppendFormat("Y: {0:f3}  X: {1:f3}  Z: {2:f3}", Y, X, Z);

                descr = spawnTimer.ToString();

            }

            return descr;

        }



        // A true re-spawn has been detected        

        public string ReSpawn(string name)

        {

            string log = "";

            try 

            {

                this.SpawnCount++;

                // if it looks like a named, leave last spawn name alone
                if (this.LastSpawnName.Length > 0)
                {
                    // See if mob name starts with capital letter or #
                    if (!Regex.IsMatch(this.LastSpawnName, "^[A-Z#]"))
                    {

                        this.LastSpawnName = name;

                    }
                }
                else
                {

                    this.LastSpawnName = name;

                }
                this.NextSpawnStr = ""; 

                this.NextSpawnDT = DateTime.MinValue;

                this.SpawnTimeDT = DateTime.Now;

                this.SpawnTimeStr = SpawnTimeDT.ToLongTimeString() + " " + SpawnTimeDT.ToShortDateString();



                // put name at beginning of list of AllNames

                

                string namet = Regex.Replace(name.Replace("_", " "), "[0-9]", "");      

                string newnames = namet;

                int namecount = 1;

                string[] names = AllNames.Split(',');

                foreach (string tname in names)

                {

                    string bname = Regex.Replace(tname.Replace("_", " "), "[0-9]", "").Trim();

                    if (newnames.IndexOf(bname) < 0)

                    {

                        if (namecount < 12)

                        {

                            newnames += ", " + bname;

                            namecount++;

                        }

                    }

                }

                AllNames = newnames;



                if (this.KillTimeDT != DateTime.MinValue) 

                {

                    // This mob has been killed already - now we can calculate

                    // the respawn time


                    int last_Timer = this.SpawnTimer;

                    TimeSpan Diff = this.SpawnTimeDT.Subtract(this.KillTimeDT);

                    this.SpawnTimer = (Diff.Hours * 3600) + (Diff.Minutes * 60) + Diff.Seconds;

                    if (Settings.Instance.MaxLogLevel > 0)
                    {
                        
                        string spawnTimer = String.Format("{0}:{1}:{2}", Diff.Hours.ToString("00"), Diff.Minutes.ToString("00"), Diff.Seconds.ToString("00"));

                        log = String.Format("Setting Timer for Spawn: {0} Name: {1} Count: {2} Last Kill Time: {3} Current Spawn Time: {4} Timer: {5} = {6} secs Old: {7} secs",

                            SpawnLoc, name, this.SpawnCount, this.KillTimeStr, this.SpawnTimeStr, spawnTimer, this.SpawnTimer, last_Timer);

                    }

                    // ... and forget about the kill



                    this.KillTimeDT = DateTime.MinValue;

                    this.KillTimeStr = "";                

                }

            }

            catch (Exception ex) {LogLib.WriteLine("Error updating Timer SPAWNTIMER for " + name + ": ", ex);}        



            listNeedsUpdate = true;

            

            return log;

        }

        

        public string Kill(DateTime dt)

        {        

            KillTimeDT = dt;

            KillTimeStr = dt.ToLongTimeString() + " " + dt.ToShortDateString();



            TimeSpan Diff = new TimeSpan(0, 0, 0, Convert.ToInt32(SpawnTimer));                                       

            NextSpawnDT = KillTimeDT.Add(Diff);

            NextSpawnStr = NextSpawnDT.ToLongTimeString() + " " + NextSpawnDT.ToShortDateString();



            listNeedsUpdate = true;



            return KillTimeStr;

        }    

        

        public ListViewItem GetListItem()

        {

            bool isInList = true;

        

            if (itmSpawnTimerList == null)

            {

                itmSpawnTimerList = new ListViewItem(EQData.FixMobName(this.LastSpawnName));

                isInList = false;

                listNeedsUpdate = true;               

                

                for (int t=0; t<10; t++)

                {

                    itmSpawnTimerList.SubItems.Add("");

                }

            }

            SpawnTimeRemaining = SecondsUntilSpawn(DateTime.Now);

            if (SpawnTimeRemaining < 1 || SpawnTimeRemaining > 120)

                itmSpawnTimerList.ForeColor = Color.Black;

            else if (SpawnTimeRemaining < 30)

                itmSpawnTimerList.ForeColor = Color.Red;

            else if (SpawnTimeRemaining < 60)

                itmSpawnTimerList.ForeColor = Color.IndianRed;

            else if (SpawnTimeRemaining < 90)

                itmSpawnTimerList.ForeColor = Color.Orange;

            else

                itmSpawnTimerList.ForeColor = Color.Goldenrod;

            if (listNeedsUpdate)
            {

                listNeedsUpdate = false;

                itmSpawnTimerList.SubItems[1].Text = this.SpawnTimeRemaining.ToString();

                itmSpawnTimerList.SubItems[2].Text = this.SpawnTimer.ToString();

                itmSpawnTimerList.SubItems[3].Text = this.zone.ToString();

                itmSpawnTimerList.SubItems[4].Text = this.X.ToString();

                itmSpawnTimerList.SubItems[5].Text = this.Y.ToString();

                itmSpawnTimerList.SubItems[6].Text = this.Z.ToString();

                itmSpawnTimerList.SubItems[7].Text = this.SpawnCount.ToString();

                itmSpawnTimerList.SubItems[8].Text = this.SpawnTimeStr;

                itmSpawnTimerList.SubItems[9].Text = this.KillTimeStr;

                itmSpawnTimerList.SubItems[10].Text = this.NextSpawnStr;

            }
            else
            {
                if (this.SpawnTimeRemaining.ToString() != itmSpawnTimerList.SubItems[1].Text.ToString())
                    itmSpawnTimerList.SubItems[1].Text = this.SpawnTimeRemaining.ToString();
            }


            if (!isInList)

            {

                return itmSpawnTimerList;

            }

            else

            {

                return null; // it already is in the list - don't add it again

            }

        }

        public string ZoneSpawnLoc;

        public string SpawnLoc;            // x,y = primary key, set on first spawn

        public string zone;

        public bool sticky = false;

        public float Y = 0;                 

        public float X = 0;

        public float Z = 0;

        public bool filtered = false;

        public int SpawnCount = 0;          // Updated on true re-spawn

        public int SpawnTimeRemaining = 0;

        public int SpawnTimer = 0;          // Updated on true re-spawn

        public string SpawnTimeStr = "";    // Update on spawn (last spawn time)

        public DateTime SpawnTimeDT = DateTime.MinValue;

        public string KillTimeStr = "";     // Updated on each kill, erased on spawn

        public DateTime KillTimeDT = DateTime.MinValue;

        public string NextSpawnStr = "";    // Updated on each kill, erased on spawn

        public DateTime NextSpawnDT = DateTime.MinValue;

        public string LastSpawnName = "";   // Updated on each spawn

        public ListViewItem itmSpawnTimerList = null;

        private bool listNeedsUpdate = false;

        private String AllNames = "";       // All names known to spawn here

        public String allNames { get { return this.AllNames; } set { this.AllNames = value; } }

    }

    #endregion



    public class MobsTimers 

    {

        private Hashtable mobsTimer = new Hashtable();    // All spawns

        public Hashtable mobsTimer2 = new Hashtable();   // Only those that re-spawned at least once^

        public Hashtable mobsTimer3 = new Hashtable();

        private String mapName = null;

        public bool MustSave = false;

        private DateTime LastSaveTime = DateTime.Now;

              

        public void SetComponents(EQMap map)

        {

            // Reset the Mob Timers for the New Zone

            //eq.mobsTimers.Reset();                        

            

            map.EnterMap += new EQMap.EnterMapHandler(this.EnterMap);

            map.ExitMap += new EQMap.ExitMapHandler(this.ExitMap);

        }

        

        // TODO: make sure it is not changed... and don't really use this, please!

        // SpawnCount is guaranteed to be > 1

        public Hashtable GetRespawned()

        {

            return mobsTimer2;

        }    

        

        // Used when the mouse is hovered over a timer (for the detail display)

        public SPAWNTIMER Find(float delta,float x,float y)

        {

            foreach (SPAWNTIMER st in mobsTimer2.Values) {

                if (!st.filtered && st.X < x+delta && st.X > x-delta && st.Y < y+delta && st.Y > y-delta) {

                    return st;

                }

            }

            return null;

        }



        // Remove all spawns

        private void ExitMap(EQMap map)

        {

            LogLib.WriteLine("Entering MobTimers.ExitMap",LogLevel.Trace);

            

            //mobsTimer.Clear();

            //mobsTimer2.Clear();
            ResetTimers();

            mapName = "";

            

            LogLib.WriteLine("Exiting MobTimers.ExitMap",LogLevel.Trace);

        }



        // Remove all spawns

        public void EnterMap(EQMap map)

        {

            LogLib.WriteLine("Entering MobTimers.EnterMap",LogLevel.Trace);

            

            ResetTimers();

            mapName = map.eq.shortname;

            LoadTimers();

            // Add items to list

            

            LogLib.WriteLine("Exiting MobTimers.EnterMap",LogLevel.Trace);

        }



        // kill the timers

        public void ResetTimers()

        {

            mobsTimer.Clear();

            ArrayList delTimerItems = new ArrayList();

            foreach (SPAWNTIMER st in mobsTimer2.Values)
            {
                if (st.sticky == false)
                    delTimerItems.Add(st);
            }

            foreach (SPAWNTIMER sp in delTimerItems)
            {

                mobsTimer2.Remove(sp.ZoneSpawnLoc);

            }

        }

        public void ResetAllTimers()
        {
            mobsTimer.Clear();
            mobsTimer2.Clear();
        }

        public void ClearSavedTimers()

        {

            ResetAllTimers();

            string timerpath = Settings.Instance.TimerDir;

            string timerfile = Path.Combine(timerpath,"spawns-" + mapName + ".txt");

            if (!Directory.Exists(timerpath))

                return;

            if (File.Exists(timerfile))

            {

                File.Delete(timerfile);

            }

        }



        // Add a new spawn, or do a re-spawn

        public void Spawn(SPAWNINFO si) {

            try 

            {

                

                // ignore players boats boxes corpses and invis man races of level 1

                if (si.IsPlayer() || (si.Race == 141) || (si.Race == 533) || (si.Race == 376) || (si.Type == 2 || si.Type == 3))

                    return;



                // ignore ldon objects, mounts, pets, mercs, and familiars

                if (si.isLDONObject || si.isEventController || si.isPet || si.isMerc || si.isFamiliar || si.isMount)

                    return;



                // ignore any mobs where name starts with "_"

                if (si.Name.IndexOf("_") == 0)

                    return;



                // If made it this far, then it is a mob that can perform alerts for proximity checks.

                si.alertMob = true;



                si.SpawnLoc = String.Format("{0:f3},{1:f3}", si.Y, si.X);

                si.ZoneSpawnLoc = String.Format("{0}{1:f3},{2:f3}", mapName.ToLower(), si.Y, si.X);

                

                if (!mobsTimer.ContainsKey(si.ZoneSpawnLoc))

                {

                    // First spawn ever



                    SPAWNTIMER st = new SPAWNTIMER(si,DateTime.Now);

                    st.zone = mapName;

                    try 

                    {

                        mobsTimer.Add(si.ZoneSpawnLoc, st);

                        if (Settings.Instance.MaxLogLevel > 0)
                            SpawnTimerLog(String.Format("Added Spawn: {0} Name: {1}", si.SpawnLoc, si.Name));

                    }

                    catch (Exception ex) {LogLib.WriteLine("Error adding new SPAWNTIMER for " + si.Name + ": ", ex);}                                  

                }

                else

                {                

                    // Process a true re-spawn

                    LogSpawns(String.Format("[SPAWN] Loc: {0} Name: {1}", si.SpawnLoc, si.Name));

                    SPAWNTIMER st = (SPAWNTIMER)mobsTimer[si.ZoneSpawnLoc];

                    SPAWNTIMER st2 = null;

                    if (mobsTimer2.ContainsKey(si.ZoneSpawnLoc))
                        st2 = (SPAWNTIMER)mobsTimer2[si.ZoneSpawnLoc];

                    int last_time = st.SpawnTimer;

                    string log = "";

                    if (st2 != null)
                    {
                        log = st2.ReSpawn(si.Name);
                        // since we updated from st2, update st values
                        st.LastSpawnName = st2.LastSpawnName;
                        st.SpawnCount = st2.SpawnCount;
                        st.SpawnTimer = st2.SpawnTimer;
                        st.NextSpawnDT = st2.NextSpawnDT;
                        st.NextSpawnStr = st2.NextSpawnStr;
                        st.SpawnTimeDT = st2.SpawnTimeDT;
                        st.SpawnTimeStr = st2.SpawnTimeStr;
                        st.allNames = st2.allNames;
                        st.KillTimeDT = st2.KillTimeDT;
                        st.KillTimeStr = st2.KillTimeStr;
                    }
                    else
                    {
                        log = st.ReSpawn(si.Name);
                    }

                    if (Settings.Instance.MaxLogLevel > 0)
                    {
                        SpawnTimerLog(String.Format("Found Spawn: {0} Name: {1}", si.SpawnLoc, si.Name));

                        if (log != string.Empty)

                            SpawnTimerLog(log);
                    }

                    if (st.SpawnCount>1 && st.SpawnTimer>10)

                    {

                        // The mob was known to spawn once, and is now re-spawning for the first

                        // time... so copy it to the other hash
                        if (!mobsTimer2.ContainsKey(si.ZoneSpawnLoc))
                        {
                            mobsTimer2.Add(si.ZoneSpawnLoc, st);
                        }

                        MustSave = true;
                    }

                }                

            }

            catch (Exception ex) {LogLib.WriteLine("Error creating new SPAWNTIMER for " + si.Name + ": ", ex);}

        }



        // We're pretty positive that the mob has been processed before, here.

        // This updates KillTime and NextSpawn.

        public void Kill(SPAWNINFO mob)

        {

            try
            {

                if ((Settings.Instance.SaveSpawnLogs) || (mapName.Length > 0))
                    LogSpawns(String.Format("[KILL] Loc: {0} Name: {1}", mob.SpawnLoc, mob.Name));

                if (mobsTimer.ContainsKey(mob.ZoneSpawnLoc))
                {
                    SPAWNTIMER stold = (SPAWNTIMER)mobsTimer[mob.ZoneSpawnLoc];

                    string log = stold.Kill(DateTime.Now);

                    // update mobsTimer2 also with kill info
                    if (mobsTimer2.ContainsKey(stold.ZoneSpawnLoc))
                    {
                        SPAWNTIMER st2 = (SPAWNTIMER)mobsTimer2[stold.ZoneSpawnLoc];

                        st2.KillTimeDT = stold.KillTimeDT;
                        st2.KillTimeStr = stold.KillTimeStr;
                        st2.NextSpawnDT = stold.NextSpawnDT;
                        st2.NextSpawnStr = stold.NextSpawnStr;
                    }

                    if (log != string.Empty && Settings.Instance.MaxLogLevel > 0)
                    {

                        SpawnTimerLog(String.Format("Updating Kill Time for Spawn: {0} Name: {1} Killed: {2}", mob.SpawnLoc, mob.Name, log));

                    }

                }
            }

            catch (Exception ex) { LogLib.WriteLine("Error updating the SPAWNTIMER for " + mob.Name + ": ", ex); }

            

            MustSave=true;

        }                           



        // Add new spawns to the list, or update changed spawns.

        // Also misused to save the spawn list occasionally

        public void UpdateList(ListViewPanel SpawnTimerList) {        

            LogLib.WriteLine("Entering UpdateList()", LogLevel.Trace);
            DateTime now = DateTime.Now;
            try 

            {                

                foreach (SPAWNTIMER st in mobsTimer2.Values) 

                {

                    ListViewItem itmSpawnTimerList = st.GetListItem();


                    if (itmSpawnTimerList != null)

                    {

                        st.itmSpawnTimerList = itmSpawnTimerList;

                        try 

                        {

                            SpawnTimerList.listView.Items.Add(itmSpawnTimerList);

                        }

                        catch (Exception ex) {LogLib.WriteLine("Error in ProcessSpawnTimer() List Add: " + itmSpawnTimerList + " - ", ex);}                        

                    }

                }

            }

            catch (Exception ex) {LogLib.WriteLine("Error in ProcessSpawnTimer(): ", ex);}          

            if (MustSave)            

            {

                TimeSpan interval = DateTime.Now.Subtract(LastSaveTime);

                if (interval.TotalSeconds > 10)

                {

                    SaveTimers();

                }

            }

            

            LogLib.WriteLine("Exiting UpdateList()", LogLevel.Trace);

        }        



        private void SpawnTimerLog(string msg) 

        {

            if (Settings.Instance.MaxLogLevel == 0)

                return;



            string logpath = Settings.Instance.LogDir;

            string logfile = "SpawnTimer.txt";

                    

            if (!Directory.Exists(logpath)) Directory.CreateDirectory(logpath);

                    

            FileStream fs = new FileStream(Path.Combine(logpath, logfile),FileMode.Append,FileAccess.Write,FileShare.ReadWrite);

            StreamWriter outLog = new StreamWriter(fs);

            outLog.WriteLine(String.Format("{0} - {1}", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.ff"), msg));

            outLog.Close();

            fs.Close();     

        }

        private void LogSpawns(string msg)

        {

            LogLib.WriteLine("Entering LogSpawns", LogLevel.Trace);



            if ((!Settings.Instance.SaveSpawnLogs) || (mapName.Length < 3))

                return;



            try
            {
                string logpath = Settings.Instance.LogDir;
                string logfile = String.Format("spawns-{0}-" + mapName + ".txt",DateTime.Now.ToString("MM-dd-yyyy"));



                if (!Directory.Exists(logpath)) Directory.CreateDirectory(logpath);

                       
                FileStream fs = new FileStream(Path.Combine(logpath, logfile),FileMode.Append,FileAccess.Write,FileShare.ReadWrite);
                StreamWriter spawnLog = new StreamWriter(fs);
                spawnLog.WriteLine(String.Format("[{0}] {1}", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.ff"), msg));
                spawnLog.Close();
                fs.Close();
            }

            catch (Exception ex) { LogLib.WriteLine("Error in LogSpawns():", ex); }



            LogLib.WriteLine("Exiting SaveTimers", LogLevel.Trace);

        }



        // Loads timers from a file for the current map

        public void LoadTimers()

        {

            LogLib.WriteLine("Entering LoadTimers",LogLevel.Trace);

            

            if ((!Settings.Instance.saveSpawnTimers) || (mapName.Length ==0))

                return;

            // Dont load timers for these zones
            if (mapName == "clz" || mapName == "default" || mapName == "bazaar" ||(mapName.IndexOf("guild") == 0) || mapName == "poknowledge" || mapName == "nexus")

                return;


            try

            {

                string timerpath = Settings.Instance.TimerDir;

                string timerfile = Path.Combine(timerpath,"spawns-" + mapName + ".txt");

                if (!Directory.Exists(timerpath))
                {
                    Directory.CreateDirectory(timerpath);
                    return;
                }

                if (!File.Exists(timerfile)) 

                {

                    return;

                }

                StreamReader sr;

                

                try

                {

                    sr = new StreamReader(File.Open(timerfile, FileMode.Open));

                }

                catch (Exception) 

                {

                    return;

                }

                

                try

                {

                    String line;

                    int count=0;

                    while ((line = sr.ReadLine()) != null) 

                    {

                        try

                        {

                            SPAWNTIMER st = new SPAWNTIMER(line);

                            st.zone = mapName.ToLower();

                            st.ZoneSpawnLoc = st.zone + st.SpawnLoc;

                            count++;

                            if (mobsTimer.ContainsKey(st.ZoneSpawnLoc))                        

                            {

                                // We already know about this mob. Copy some of the information.

                                SPAWNTIMER stold = (SPAWNTIMER)mobsTimer[st.ZoneSpawnLoc];

                                // check if we add names in the merge.  If so, make sure we save.
                                int startlen = stold.allNames.Length;
                                stold.Merge(st);
                                if (stold.allNames.Length > startlen)
                                    MustSave = true;

                                if (stold.SpawnCount > 1 && stold.SpawnTimer > 10)
                                {
                                    if (!mobsTimer2.ContainsKey(stold.ZoneSpawnLoc))
                                    {

                                        mobsTimer2.Add(stold.ZoneSpawnLoc, stold);

                                    }

                                }

                            }

                            else

                            {

                                if (st.SpawnCount > 1)

                                {

                                    if (st.SpawnTimer > 10)

                                    {

                                        TimeSpan Diff = new TimeSpan(0, 0, 0, Convert.ToInt32(st.SpawnTimer));                                       

                                        if (DateTime.Now > st.KillTimeDT.Add(Diff))

                                            st.KillTimeDT = DateTime.MinValue;

                                        if (DateTime.Now > st.NextSpawnDT)

                                        {

                                            st.NextSpawnDT = DateTime.MinValue;

                                            st.KillTimeDT = DateTime.MinValue;

                                            st.NextSpawnStr = "";

                                        }

                                        mobsTimer2.Add(st.ZoneSpawnLoc, st);

                                    }

                                    mobsTimer.Add(st.ZoneSpawnLoc, st);

                                }

                            }

                        }

                        catch (Exception ex) {LogLib.WriteLine("Error in LoadTimers(), processing line:\r\n"+line,ex);}                        

                    }

                    

                    LogLib.WriteLine("Spawns read: "+count,LogLevel.Debug);

                }       

                finally

                {

                    sr.Close();

                }

            }

            catch (Exception ex) {LogLib.WriteLine("Error in LoadTimers():",ex);}                        

        

            LogLib.WriteLine("Exiting LoadTimers",LogLevel.Trace);

        }

        

        // Saves timers to a file for the current map

        private void SaveTimers()

        {

            LogLib.WriteLine("Entering SaveTimers",LogLevel.Trace);


            if ((!Settings.Instance.saveSpawnTimers) || (mapName.Length < 3))
            {

                MustSave = false;
                LastSaveTime = DateTime.Now;
                return;
                
            }
            if (mapName == "clz" || mapName == "default" || mapName == "bazaar" || (mapName.IndexOf("guild") == 0) || mapName == "poknowledge" || mapName == "nexus")
            {
                MustSave = false;
                LastSaveTime = DateTime.Now;

                // We are not saving timers for these zone.  If they exist, then delete them.
                string timerpath = Settings.Instance.TimerDir;

                if (!Directory.Exists(timerpath))
                {
                    return;
                }

                string timerfile = Path.Combine(timerpath, "spawns-" + mapName + ".txt");

                if (File.Exists(timerfile))
                    File.Delete(timerfile);

                return;

            }

            try

            {

                MustSave = false;

                LastSaveTime = DateTime.Now;

                if (mobsTimer2.Count==0)

                {

                    LogLib.WriteLine("No spawns to write.",LogLevel.Debug);

                }

                else

                {

                    int count = 0;

                    foreach (SPAWNTIMER st in mobsTimer2.Values)

                    {

                        if (st.SpawnTimer > 10 && (string.Compare(st.zone, mapName,true) == 0))

                        {

                            count++;

                        }

                    }

                    if (count > 0)

                    {

                        string timerpath = Settings.Instance.TimerDir;

                        if (!Directory.Exists(timerpath))
                        {
                            Directory.CreateDirectory(timerpath);
                        }

                        string timerfile = Path.Combine(timerpath, "spawns-" + mapName + ".txt");

                        LogLib.WriteLine("File name is '" + timerfile + "'", LogLevel.Debug);



                        StreamWriter sw = new StreamWriter(File.Open(timerfile, FileMode.Create));

                        foreach (SPAWNTIMER st in mobsTimer2.Values)

                        {

                            if (st.SpawnTimer > 10 && (string.Compare(st.zone, mapName,true) == 0))

                            {

                                string str = st.GetAsString();

                                sw.WriteLine(str);

                            }

                        }



                        LogLib.WriteLine("Spawns written: " + count, LogLevel.Debug);

                        sw.Close();

                    }

                }

            }

            catch (Exception ex) {LogLib.WriteLine("Error in SaveTimers():",ex);}                        

            

            LogLib.WriteLine("Exiting SaveTimers",LogLevel.Trace);

        }

        

    }        

}        

