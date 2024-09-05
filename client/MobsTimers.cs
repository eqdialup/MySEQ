using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using myseq.Properties;
using Structures;

namespace myseq
{
    public class MobsTimers
    {
        private readonly Hashtable mobsTimer = new Hashtable();    // All spawns

        public Hashtable mobsTimer2 = new Hashtable();   // Only those that re-spawned at least once^

        private string mapName;

        internal bool MustSave;

        private DateTime LastSaveTime = DateTime.Now;

        public void SetComponents(EQMap map)
        {
            // Reset the Mob Timers for the New Zone
            map.EnterMap += EnterMap;
            map.ExitMap += ExitMap;
        }

        public Hashtable GetRespawned() => mobsTimer2;

        // Used when the mouse is hovered over a timer (for the detail display)

        public Spawntimer Find(float delta, float x, float y)
        {
            foreach (Spawntimer st in mobsTimer2.Values)
            {
                var stXdelta = (st.X < x + delta) && (st.X > x - delta);
                var stY_delta = (st.Y < y + delta) && (st.Y > y - delta);

                if (!st.filtered && stXdelta && stY_delta)
                {
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
            mapName = map.eq.Shortname;
            LoadTimers();
        }

        // kill the timers

        public void ResetTimers()
        {
            mobsTimer.Clear();

            ArrayList delTimerItems = new ArrayList();

            foreach (Spawntimer st in mobsTimer2.Values)
            {
                if (!st.sticky)
                {
                    delTimerItems.Add(st);
                }
            }

            foreach (Spawntimer sp in delTimerItems)
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

            var timerfile = FileOps.CombineTimer(mapName);

            if (!Directory.Exists(Settings.Default.TimerDir))
            {
                return;
            }

            FileOps.DeleteFile(timerfile);
        }

        // Add a new spawn, or do a re-spawn
        public void Spawn(Spawninfo si)
        {
            try

            {
                // ignore players boats boxes corpses and invis man races of level 1
                // ignore ldon objects, mounts, pets, mercs, and familiars
                // ignore any mobs where name starts with "_"
                var various = si.IsPlayer || (si.Race == 141) || (si.Race == 533) || si.Race == 376 || si.Type == 2 || si.Type == 3;
                var oddtypes = si.isLDONObject || si.isEventController || si.isPet || si.isMerc || si.isFamiliar || si.isMount;
                if (various || oddtypes || si.Name.IndexOf("_") == 0)
                {
                    return;
                }
                // If made it this far, then it is a mob that can perform alerts for proximity checks.

                si.alertMob = true;

                si.SpawnLoc = $"{si.Y:f3},{si.X:f3}";

                si.ZoneSpawnLoc = $"{mapName.ToLower()}{si.Y:f3},{si.X:f3}";

                if (mobsTimer.ContainsKey(si.ZoneSpawnLoc))
                {
                    // Process a true re-spawn
                    TrueRespawn(si);
                }
                else
                {
                    // First spawn ever
                    FirstSpawn(si);
                }
            }
            catch (Exception ex) { LogLib.WriteLine($"Error creating new SPAWNTIMER for {si.Name}: ", ex); }
        }

        private void FirstSpawn(Spawninfo si)
        {
            Spawntimer st = new Spawntimer(si, DateTime.Now)
            {
                zone = mapName
            };

            try
            {
                mobsTimer.Add(si.ZoneSpawnLoc, st);

                if (Settings.Default.MaxLogLevel > 0)
                {
                    SpawnTimerLog($"Added Spawn: {si.SpawnLoc} Name: {si.Name}");
                }
            }
            catch (Exception ex)
            {
                LogLib.WriteLine($"Error adding new SPAWNTIMER for {si.Name}: ", ex);
                throw;
            }
        }

        private void TrueRespawn(Spawninfo si)
        {
            LogSpawns($"[SPAWN] Loc: {si.SpawnLoc} Name: {si.Name}");

            Spawntimer st = (Spawntimer)mobsTimer[si.ZoneSpawnLoc];

            Spawntimer st2 = null;

            if (mobsTimer2.ContainsKey(si.ZoneSpawnLoc))
            {
                st2 = (Spawntimer)mobsTimer2[si.ZoneSpawnLoc];
            }

            string log;

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
                SpawnTimerLog($"Found Spawn: {si.SpawnLoc} Name: {si.Name} Timer {log} ");
            }

            AddRespawned(si, st);
        }

        private void AddRespawned(Spawninfo si, Spawntimer st)
        {
            if (st.SpawnCount > 1 && st.SpawnTimer > 10)

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

        // We're pretty positive that the mob has been processed before, here.
        // This updates KillTime and NextSpawn.
        public void Kill(Spawninfo mob)

        {
            if (Settings.Default.SaveSpawnLogs || (mapName.Length > 0))
            {
                LogSpawns($"[KILL] Loc: {mob.SpawnLoc} Name: {mob.Name}");
            }

            if (mobsTimer.ContainsKey(mob.ZoneSpawnLoc))
            {
                Spawntimer stold = (Spawntimer)mobsTimer[mob.ZoneSpawnLoc];

                var log = stold.Kill(DateTime.Now);

                // update mobsTimer2 also with kill info
                if (mobsTimer2.ContainsKey(stold.ZoneSpawnLoc))
                {
                    Spawntimer st2 = (Spawntimer)mobsTimer2[stold.ZoneSpawnLoc];

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

            MustSave = true;
        }

        // Add new spawns to the list, or update changed spawns.
        // Also misused to save the spawn list occasionally
        public void UpdateList(ListViewPanel SpawnTimerList)
        {
            _ = DateTime.Now;
            try

            {
                foreach (Spawntimer st in mobsTimer2.Values)
                {
                    ListViewItem itmSpawnTimerList = st.GetListItem();

                    if (itmSpawnTimerList != null)
                    {
                        st.ItmSpawnTimerList = itmSpawnTimerList;
                        SpawnTimerList.listView.Items.Add(itmSpawnTimerList);
                    }
                }
            }
            catch (Exception ex) { LogLib.WriteLine("Error in ProcessSpawnTimer(): ", ex); }

            if (MustSave && DateTime.Now.Subtract(LastSaveTime).TotalSeconds > 60)
            {
                SaveTimers();
            }
        }

        private void SpawnTimerLog(string msg)
        {
            if (Settings.Default.MaxLogLevel == 0)
            {
                return;
            }

            using (FileStream fs = new FileStream(FileOps.CombineLog("SpawnTimer.txt"), FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            using (StreamWriter outLog = new StreamWriter(fs))
            {
                outLog.WriteLine($"{DateTime.Now:MM/dd/yyyy HH:mm:ss.ff} - {msg}");
            }
        }

        private void LogSpawns(string msg)
        {
            if (!Settings.Default.SaveSpawnLogs || mapName.Length < 3)
            {
                return;
            }

            var logfile = $"spawns-{DateTime.Now:MM-dd-yyyy}-{mapName}.txt";

            using (FileStream fs = new FileStream(FileOps.CombineLog(logfile), FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            using (StreamWriter spawnLog = new StreamWriter(fs))
            {
                spawnLog.WriteLine($"[{DateTime.Now:MM/dd/yyyy HH:mm:ss.ff}] {msg}");
            }
        }

        // Loads timers from a file for the current map

        public void LoadTimers()
        {
            if (!string.IsNullOrEmpty(mapName) && Settings.Default.saveSpawnTimers && !Voidmaps)
            {
                var timerfile = FileOps.CombineTimer(mapName);

                if (File.Exists(timerfile))
                {
                    foreach (var line in File.ReadAllLines(timerfile))
                    {
                        TimersFileLines(line);
                    }
                }
            }
        }

        private void TimersFileLines(string line)
        {
            var count = 0;
            try
            {
                Spawntimer st = new Spawntimer(line)
                {
                    zone = mapName.ToLower()
                };

                st.ZoneSpawnLoc = st.zone + st.SpawnLoc;

                count++;

                if (mobsTimer.ContainsKey(st.ZoneSpawnLoc))

                {
                    // We already know about this mob. Copy some of the information.

                    GetKnownMobInfo(st);
                }
                else if (st.SpawnCount > 1)
                {
                    SetNewMobTimeInfo(st);

                    mobsTimer.Add(st.ZoneSpawnLoc, st);
                }
            }
            catch (Exception ex) { LogLib.WriteLine($"Error in LoadTimers(), processing line:\r\n{line}", ex); }

            LogLib.WriteLine($"Spawns read: {count}", LogLevel.Debug);
        }

        private void SetNewMobTimeInfo(Spawntimer st)
        {
            if (st.SpawnTimer > 10)
            {
                TimeSpan Diff = new TimeSpan(0, 0, 0, st.SpawnTimer);

                if (DateTime.Now > st.KillTimeDT.Add(Diff))
                {
                    st.KillTimeDT = DateTime.MinValue;
                }

                if (DateTime.Now > st.NextSpawnDT)

                {
                    st.NextSpawnDT = DateTime.MinValue;

                    st.KillTimeDT = DateTime.MinValue;

                    st.NextSpawnStr = "";
                }

                mobsTimer2.Add(st.ZoneSpawnLoc, st);
            }
        }

        private void GetKnownMobInfo(Spawntimer st)
        {
            Spawntimer oldTimer = (Spawntimer)mobsTimer[st.ZoneSpawnLoc];

            // check if we add names in the merge.  If so, make sure we save.
            var startlen = oldTimer.AllNames.Length;
            oldTimer.Merge(st);
            if (oldTimer.AllNames.Length > startlen)
            {
                MustSave = true;
            }

            if (oldTimer.SpawnCount > 1 && oldTimer.SpawnTimer > 10 && !mobsTimer2.ContainsKey(oldTimer.ZoneSpawnLoc))
            {
                mobsTimer2.Add(oldTimer.ZoneSpawnLoc, oldTimer);
            }
        }

        // Saves timers to a file for the current map

        private void SaveTimers()
        {
            var timerfile = FileOps.CombineTimer(mapName);
            if (!Settings.Default.saveSpawnTimers || mapName.Length < 3)
            {
                return;
            }
            if (Voidmaps)
            {
                MustSave = false;
                FileOps.DeleteFile(timerfile);
                return;
            }

            try
            {
                MustSave = false;
                LastSaveTime = DateTime.Now;

                if (mobsTimer2.Count != 0)
                {
                    var count = 0;

                    foreach (Spawntimer st in mobsTimer2.Values)
                    {
                        if (st.SpawnTimer > 10 && (string.Compare(st.zone, mapName, true) == 0))
                        {
                            count++;
                        }
                    }

                    if (count > 0)
                    {
                        using (StreamWriter sw = new StreamWriter(File.Open(timerfile, FileMode.Create)))
                        {
                            foreach (Spawntimer st in mobsTimer2.Values)
                            {
                                if (st.SpawnTimer > 10 && (string.Compare(st.zone, mapName, true) == 0))
                                {
                                    sw.WriteLine(st.GetAsString());
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { LogLib.WriteLine("Error in SaveTimers():", ex); }
        }

        internal bool Voidmaps => mapName == "clz" || mapName == "default" || mapName == "bazaar" || mapName.Contains("guild") || mapName == "poknowledge" || mapName == "nexus";
    }
}