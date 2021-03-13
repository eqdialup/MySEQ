using myseq.Properties;
using Structures;
using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;

namespace myseq
{
    public class MobsTimers

    {
        private Hashtable mobsTimer = new Hashtable();    // All spawns

        public Hashtable mobsTimer2 = new Hashtable();   // Only those that re-spawned at least once^

        public Hashtable mobsTimer3 = new Hashtable();

        private string mapName;

        public bool MustSave;

        private DateTime LastSaveTime = DateTime.Now;

        public void SetComponents(EQMap map)

        {
            // Reset the Mob Timers for the New Zone
            map.EnterMap += new EQMap.EnterMapHandler(EnterMap);
            map.ExitMap += new EQMap.ExitMapHandler(ExitMap);
        }

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
            ResetTimers();
            mapName = "";
        }

        // Remove all spawns

        public void EnterMap(EQMap map)

        {
            ResetTimers();

            mapName = map.eq.shortname;

            LoadTimers();
        }

        // kill the timers

        public void ResetTimers()

        {
            mobsTimer.Clear();

            ArrayList delTimerItems = new ArrayList();

            foreach (SPAWNTIMER st in mobsTimer2.Values)
            {
                if (!st.sticky)
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

            string timerpath = Settings.Default.TimerDir;

            string timerfile = Path.Combine(timerpath, $"spawns-{mapName}.txt");

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

                if (si.IsPlayer() || (si.Race == 141) || (si.Race == 533) || (si.Race == 376) || si.Type == 2 || si.Type == 3)
                    return;

                // ignore ldon objects, mounts, pets, mercs, and familiars

                if (si.isLDONObject || si.isEventController || si.isPet || si.isMerc || si.isFamiliar || si.isMount)
                    return;

                // ignore any mobs where name starts with "_"

                if (si.Name.IndexOf("_") == 0)
                    return;

                // If made it this far, then it is a mob that can perform alerts for proximity checks.

                si.alertMob = true;

                si.SpawnLoc = $"{si.Y:f3},{si.X:f3}";

                si.ZoneSpawnLoc = $"{mapName.ToLower()}{si.Y:f3},{si.X:f3}";

                if (!mobsTimer.ContainsKey(si.ZoneSpawnLoc))

                {
                    // First spawn ever

                    SPAWNTIMER st = new SPAWNTIMER(si, DateTime.Now)
                    {
                        zone = mapName
                    };

                    try

                    {
                        mobsTimer.Add(si.ZoneSpawnLoc, st);

                        if (Settings.Default.MaxLogLevel > 0)
                            SpawnTimerLog($"Added Spawn: {si.SpawnLoc} Name: {si.Name}");
                    }
                    catch (Exception ex) {LogLib.WriteLine("Error adding new SPAWNTIMER for " + si.Name + ": ", ex);}
                }
                else
                {
                    // Process a true re-spawn

                    LogSpawns($"[SPAWN] Loc: {si.SpawnLoc} Name: {si.Name}");

                    SPAWNTIMER st = (SPAWNTIMER)mobsTimer[si.ZoneSpawnLoc];

                    SPAWNTIMER st2 = null;

                    if (mobsTimer2.ContainsKey(si.ZoneSpawnLoc))
                        st2 = (SPAWNTIMER)mobsTimer2[si.ZoneSpawnLoc];

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
                        st.AllNames = st2.AllNames;
                        st.KillTimeDT = st2.KillTimeDT;
                        st.KillTimeStr = st2.KillTimeStr;
                    }
                    else
                    {
                        log = st.ReSpawn(si.Name);
                    }

                    if (Settings.Default.MaxLogLevel > 0)
                    {
                        SpawnTimerLog($"Found Spawn: {si.SpawnLoc} Name: {si.Name}");

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
                if (Settings.Default.SaveSpawnLogs || (mapName.Length > 0))
                    LogSpawns($"[KILL] Loc: {mob.SpawnLoc} Name: {mob.Name}");

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

                    if (log != string.Empty && Settings.Default.MaxLogLevel > 0)
                    {
                        SpawnTimerLog($"Updating Kill Time for Spawn: {mob.SpawnLoc} Name: {mob.Name} Killed: {log}");
                    }
                }
            }
            catch (Exception ex) { LogLib.WriteLine($"Error updating the SPAWNTIMER for {mob.Name}: ", ex); }

            MustSave=true;
        }

        // Add new spawns to the list, or update changed spawns.

        // Also misused to save the spawn list occasionally

        public void UpdateList(ListViewPanel SpawnTimerList) {
            _ = DateTime.Now;
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
                        catch (Exception ex) { LogLib.WriteLine($"Error in ProcessSpawnTimer() List Add: {itmSpawnTimerList} - ", ex); }
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
        }

        private void SpawnTimerLog(string msg)

        {
            if (Settings.Default.MaxLogLevel == 0)
                return;

            string logpath = Settings.Default.LogDir;

            if (!Directory.Exists(logpath)) Directory.CreateDirectory(logpath);

            FileStream fs = new FileStream(Path.Combine(logpath, "SpawnTimer.txt"), FileMode.Append, FileAccess.Write, FileShare.ReadWrite);

            StreamWriter outLog = new StreamWriter(fs);

            outLog.WriteLine($"{DateTime.Now:MM/dd/yyyy HH:mm:ss.ff} - {msg}");

            outLog.Close();

            fs.Close();
        }

        private void LogSpawns(string msg)

        {
            if ((!Settings.Default.SaveSpawnLogs) || (mapName.Length < 3))
                return;

            try
            {
                string logpath = Settings.Default.LogDir;
                string logfile = string.Format($"spawns-{{0}}-{mapName}.txt", DateTime.Now.ToString("MM-dd-yyyy"));

                if (!Directory.Exists(logpath)) Directory.CreateDirectory(logpath);

                FileStream fs = new FileStream(Path.Combine(logpath, logfile),FileMode.Append,FileAccess.Write,FileShare.ReadWrite);
                StreamWriter spawnLog = new StreamWriter(fs);
                spawnLog.WriteLine($"[{DateTime.Now:MM/dd/yyyy HH:mm:ss.ff}] {msg}");
                spawnLog.Close();
                fs.Close();
            }
            catch (Exception ex) { LogLib.WriteLine("Error in LogSpawns():", ex); }
        }

        // Loads timers from a file for the current map

        public void LoadTimers()
        {
            if (!string.IsNullOrEmpty(mapName))
                {
            if (!Settings.Default.saveSpawnTimers  || mapName == "clz" || mapName == "default" || mapName == "bazaar" || mapName.Contains("guild") || mapName == "poknowledge" || mapName == "nexus")
            {
                return;
            }
            try

            {
                string timerpath = Settings.Default.TimerDir;

                string timerfile = Path.Combine(timerpath, "spawns-" + mapName + ".txt");

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
                    string line;

                    int count = 0;

                    while ((line = sr.ReadLine()) != null)

                    {
                        try

                        {
                            SPAWNTIMER st = new SPAWNTIMER(line)
                            {
                                zone = mapName.ToLower()
                            };

                            st.ZoneSpawnLoc = st.zone + st.SpawnLoc;

                            count++;

                            if (mobsTimer.ContainsKey(st.ZoneSpawnLoc))

                            {
                                // We already know about this mob. Copy some of the information.

                                SPAWNTIMER stold = (SPAWNTIMER)mobsTimer[st.ZoneSpawnLoc];

                                // check if we add names in the merge.  If so, make sure we save.
                                int startlen = stold.AllNames.Length;
                                stold.Merge(st);
                                if (stold.AllNames.Length > startlen)
                                    MustSave = true;

                                if (stold.SpawnCount > 1 && stold.SpawnTimer > 10 && !mobsTimer2.ContainsKey(stold.ZoneSpawnLoc))
                                {
                                    mobsTimer2.Add(stold.ZoneSpawnLoc, stold);
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
                        catch (Exception ex) { LogLib.WriteLine("Error in LoadTimers(), processing line:\r\n" + line, ex); }
                    }

                    LogLib.WriteLine($"Spawns read: {count}", LogLevel.Debug);
                }

                finally

                {
                    sr.Close();
                }
            }
            catch (Exception ex) { LogLib.WriteLine("Error in LoadTimers():", ex); }
            }
        }

        // Saves timers to a file for the current map

        private void SaveTimers()

        {
            if ((!Settings.Default.saveSpawnTimers) || (mapName.Length < 3))
            {
                MustSave = false;
                LastSaveTime = DateTime.Now;
                return;
            }
            if (mapName == "clz" || mapName == "default" || mapName == "bazaar" || mapName.Contains("guild") || mapName == "poknowledge" || mapName == "nexus")
            {
                MustSave = false;
                LastSaveTime = DateTime.Now;

                // We are not saving timers for these zone.  If they exist, then delete them.
                string timerpath = Settings.Default.TimerDir;

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
                        string timerpath = Settings.Default.TimerDir;

                        if (!Directory.Exists(timerpath))
                        {
                            Directory.CreateDirectory(timerpath);
                        }

                        string timerfile = Path.Combine(timerpath, $"spawns-{mapName}.txt");

                        LogLib.WriteLine($"File name is '{timerfile}'", LogLevel.Debug);

                        StreamWriter sw = new StreamWriter(File.Open(timerfile, FileMode.Create));

                        foreach (SPAWNTIMER st in mobsTimer2.Values)

                        {
                            if (st.SpawnTimer > 10 && (string.Compare(st.zone, mapName,true) == 0))

                            {
                                string str = st.GetAsString();

                                sw.WriteLine(str);
                            }
                        }

                        LogLib.WriteLine($"Spawns written: {count}", LogLevel.Debug);

                        sw.Close();
                    }
                }
            }
            catch (Exception ex) {LogLib.WriteLine("Error in SaveTimers():",ex);}
        }
    }
}

