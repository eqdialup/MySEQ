﻿using myseq.Properties;
using Structures;
using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace myseq
{
    public class Spawntimer
    {
        public Spawntimer(Spawninfo si, DateTime dt)

        {
            SpawnLoc = si.SpawnLoc;

            ZoneSpawnLoc = si.ZoneSpawnLoc;

            Zone = "";

            Location = new Point3D(si.X, si.Y, si.Z);

            SpawnTimeDT = dt;

            SpawnTimeStr = $"{dt.ToLongTimeString()} {dt.ToShortDateString()}";

            SpawnCount = 1;

            LastSpawnName = si.Name;

            AllNames = si.Name.TrimName();
        }

        public Spawntimer(string str)
        {
            var parts = str.Split(';');

            SpawnLoc = parts[0];

            SpawnCount = int.Parse(parts[1]);

            SpawnTimer = int.Parse(parts[2]);

            SpawnTimeStr = parts[3];

            SpawnTimeDT = GetDateTime(SpawnTimeStr);

            KillTimeStr = parts[4];

            KillTimeDT = GetDateTime(KillTimeStr);

            NextSpawnStr = parts[5];

            NextSpawnDT = GetDateTime(NextSpawnStr);

            LastSpawnName = parts[6];

            AllNames = parts[7];

            // split up the All Names, and parse out spawn numbers in parenthesis
            _ = AllNames.Split(',').GetUpperBound(0);

            Location = Point3D.Parse(parts, 8);
        }

        private DateTime GetDateTime(string input)
        {
            return input.Length > 0 ? Convert.ToDateTime(input) : default;
        }

        // Returns the data in a format which can be used with the (string) constructor.

        public string GetAsString() => $"{SpawnLoc};{SpawnCount};{SpawnTimer};{SpawnTimeStr};{KillTimeStr};{NextSpawnStr};{LastSpawnName};{AllNames};{Location.ToString()}";

        // st has been loaded from a file, and is the same spawn as "this" one.
        // Glean all useful information.

        public void Merge(Spawntimer st)
        {
            LogLib.WriteLine("Merging spawn timers:", LogLevel.Debug);

            // Update spawn count and timer
            SpawnCount = st.SpawnCount;
            SpawnTimer = st.SpawnTimer;

            if (KillTimeDT == DateTime.MinValue)
            {
                // If no previous kill, set new kill time and next spawn time
                KillTimeStr = st.KillTimeStr;
                NextSpawnDT = DateTime.Now.AddSeconds(SpawnTimer);
                NextSpawnStr = $"{NextSpawnDT.ToLongTimeString()} {NextSpawnDT.ToShortDateString()}";
            }
            else
            {
                // Otherwise, enable the timer based on the provided spawntimer
                EnableTimer(st);
            }

            // Merge the names, ensuring no duplicates and limiting the total to 10 names
            MergeNames(st.AllNames);

            // Update last spawn name to match a named mob (capital letter or #)
            UpdateLastSpawnName();

            listNeedsUpdate = true;
        }

        private void MergeNames(string newNames)
        {
            var builder = new StringBuilder(AllNames);
            var existingNames = AllNames.Split(',')
                                        .Select(n => n.TrimName())
                                        .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var addedCount = existingNames.Count;
            foreach (var name in newNames.Split(',').Select(n => n.TrimName()))
            {
                if (!existingNames.Contains(name) && addedCount < 10)  // Limit to 10 names
                {
                    builder.Append($", {name}");
                    addedCount++;
                    existingNames.Add(name);
                }
            }
            AllNames = builder.ToString();
        }

        private void UpdateLastSpawnName()
        {
            foreach (var name in AllNames.Split(',').Select(n => n.TrimName()))
            {
                if (name.RegexMatch())
                {
                    LastSpawnName = name;
                    break;
                }
            }
        }

        private void EnableTimer(Spawntimer st)
        {
            if (st.SpawnTimer <= 10) return;

            var diff = TimeSpan.FromSeconds(st.SpawnTimer);

            // Check if spawn time is still valid
            if (DateTime.Now.Subtract(diff) < st.SpawnTimeDT)
            {
                SpawnTimeDT = st.SpawnTimeDT;
                SpawnTimeStr = st.SpawnTimeStr;
            }

            // Update kill time
            KillTimeDT = DateTime.Now.Subtract(diff) > st.KillTimeDT ? DateTime.MinValue : st.KillTimeDT;
            KillTimeStr = KillTimeDT == DateTime.MinValue ? string.Empty : st.KillTimeStr;

            // Update next spawn time if not passed
            NextSpawnDT = DateTime.Now > st.NextSpawnDT ? DateTime.MinValue : st.NextSpawnDT;
            NextSpawnStr = NextSpawnDT == DateTime.MinValue ? string.Empty : st.NextSpawnStr;
        }

        public int SecondsUntilSpawn(DateTime now)
        {
            if (NextSpawnDT == DateTime.MinValue)
            {
                return 0;
            }

            TimeSpan diff = NextSpawnDT - now;
            int checkTimer = (int)diff.TotalSeconds;

            return checkTimer > 0 ? checkTimer : 0;
        }

        public string GetDescription()
        {
            var countTimer = GetCountTimer();
            var spawnTimer = BuildSpawnDescription();

            spawnTimer.AppendLine()
                      .AppendFormat("Last Spawned At: {0}\n", SpawnTimeStr)
                      .AppendFormat("Last Killed At: {0}\n", KillTimeStr)
                      .AppendFormat("Next Spawn At: {0}\n", NextSpawnStr)
                      .AppendFormat("Spawn Timer: {0} secs\n", SpawnTimer)
                      .AppendFormat("Spawning In: {0}\n", countTimer)
                      .AppendFormat("Spawn Count: {0}\n", SpawnCount)
                      .AppendFormat(Location.ToString());

            return spawnTimer.ToString();
        }

        private string GetCountTimer()
        {
            if (NextSpawnDT == DateTime.MinValue)
                return string.Empty;

            TimeSpan diff = NextSpawnDT.Subtract(DateTime.Now);
            return $"{diff.Hours:00}:{diff.Minutes:00}:{diff.Seconds:00}";
        }

        private StringBuilder BuildSpawnDescription()
        {
            var spawnTimer = new StringBuilder();

            spawnTimer.AppendFormat("Spawn Name: {0}\n", LastSpawnName);

            var names = AllNames.Split(',');
            AppendNames(spawnTimer, "Names encountered: ", names);

            return spawnTimer;
        }

        private static void AppendNames(StringBuilder spawnTimer, string namesPrefix, string[] names)
        {
            var currentLine = new StringBuilder(namesPrefix);
            const int maxLineLength = 45;

            foreach (var name in names)
            {
                var trimmedName = name.TrimName();
                var potentialLength = currentLine.Length + trimmedName.Length + 2; // +2 for ", "

                if (potentialLength < maxLineLength)
                {
                    if (currentLine.Length > namesPrefix.Length)
                    {
                        currentLine.Append(", ");
                    }
                    currentLine.Append(trimmedName);
                }
                else
                {
                    spawnTimer.AppendLine(currentLine.ToString());
                    currentLine.Clear();
                    currentLine.Append(trimmedName);
                }
            }

            // Append remaining names
            if (currentLine.Length > 0)
            {
                spawnTimer.AppendLine(currentLine.ToString());
            }
        }

        // A true re-spawn has been detected

        public string ReSpawn(string name)
        {
            string log = "";

            try
            {
                SpawnCount++;
                if (LastSpawnName.Length == 0 || !LastSpawnName.RegexMatch())
                {
                    LastSpawnName = name;
                }

                NextSpawnStr = "";
                NextSpawnDT = DateTime.MinValue;

                // Update spawn time
                SpawnTimeDT = DateTime.Now;
                SpawnTimeStr = $"{SpawnTimeDT.ToLongTimeString()} {SpawnTimeDT.ToShortDateString()}";

                AllNames = UpdateAllNames(name);
                log = GetRespawnTime(name, log);
            }
            catch (Exception ex)
            {
                LogLib.WriteLine($"Error updating Timer SPAWNTIMER for {name}: ", ex);
            }

            listNeedsUpdate = true;
            return log;
        }

        private string UpdateAllNames(string name)
        {
            var trimmedName = name.TrimName();
            var nameCount = 1;
            StringBuilder builder = new StringBuilder(trimmedName);

            foreach (var tname in AllNames.Split(','))
            {
                var trimmedTName = tname.TrimName();
                if (!trimmedName.Contains(trimmedTName) && nameCount < 8)
                {
                    builder.Append(", ").Append(trimmedTName);
                    nameCount++;
                }
            }

            return builder.ToString();
        }

        private string GetRespawnTime(string name, string log)
        {
            if (KillTimeDT != DateTime.MinValue)
            {
                // This mob has been killed already - now we can calculate the respawn time
                var last_Timer = SpawnTimer;

                TimeSpan Diff = SpawnTimeDT.Subtract(KillTimeDT);

                SpawnTimer = (Diff.Hours * 3600) + (Diff.Minutes * 60) + Diff.Seconds;

                if (Settings.Default.MaxLogLevel > 0)
                {
                    var spawnTimer = $"{Diff.Hours:00}:{Diff.Minutes:00}:{Diff.Seconds:00}";

                    log = $"Setting Timer for Spawn: {SpawnLoc} Name: {name} Count: {SpawnCount} Last Kill Time: {KillTimeStr} Current Spawn Time: {SpawnTimeStr} Timer: {spawnTimer} = {SpawnTimer} secs Old: {last_Timer} secs";
                }
                // ... and forget about the kill
                KillTimeDT = DateTime.MinValue;
                KillTimeStr = "";
            }

            return log;
        }

        public string Kill(DateTime dt)
        {
            KillTimeDT = dt;

            KillTimeStr = $"{dt.ToLongTimeString()} {dt.ToShortDateString()}";

            NextSpawnDT = KillTimeDT.Add(new TimeSpan(0, 0, 0, Convert.ToInt32(SpawnTimer)));

            NextSpawnStr = $"{NextSpawnDT.ToLongTimeString()} {NextSpawnDT.ToShortDateString()}";

            listNeedsUpdate = true;

            return KillTimeStr;
        }

        public ListViewItem GetListItem()
        {
            // If the ListViewItem does not exist, create and initialize it
            if (ItmSpawnTimerList == null)
            {
                ItmSpawnTimerList = new ListViewItem(LastSpawnName.FixMobName());
                listNeedsUpdate = true;

                // Initialize the ListViewItem with 10 subitems
                for (var t = 0; t < 10; t++)
                {
                    ItmSpawnTimerList.SubItems.Add("");
                }
            }
            SpawnTimeRemaining = SecondsUntilSpawn(DateTime.Now);

            // Assign the appropriate color based on remaining time
            if (SpawnTimeRemaining < 1 || SpawnTimeRemaining > 120)
            {
                ItmSpawnTimerList.ForeColor = Color.Black;
            }
            else if (SpawnTimeRemaining < 30)
            {
                ItmSpawnTimerList.ForeColor = Color.Red;
            }
            else if (SpawnTimeRemaining < 60)
            {
                ItmSpawnTimerList.ForeColor = Color.IndianRed;
            }
            else if (SpawnTimeRemaining < 90)
            {
                ItmSpawnTimerList.ForeColor = Color.Orange;
            }
            else
            {
                ItmSpawnTimerList.ForeColor = Color.Goldenrod;
            }

            UpdateList();
            return ItmSpawnTimerList;
        }

        private void UpdateList()
        {
            if (!listNeedsUpdate) return;

            string[] subItems = { SpawnTimeRemaining.ToString(), SpawnTimer.ToString(), Zone, Location.ToString(), SpawnCount.ToString(), SpawnTimeStr, KillTimeStr, NextSpawnStr };
            for (int i = 0; i < subItems.Length; i++)
            {
                ItmSpawnTimerList.SubItems[i + 1].Text = subItems[i];
            }

            listNeedsUpdate = false;
        }

        public string ZoneSpawnLoc { get; set; }

        public string SpawnLoc { get; set; }            // x,y = primary key, set on first spawn

        public string Zone { get; set; }

        public bool Sticky { get; set; }
        public Point3D Location { get; set; } // Using Point3D for X, Y, Z coordinates

        public bool Filtered { get; set; }

        public int SpawnCount { get; set; } = 0;          // Updated on true re-spawn

        public int SpawnTimeRemaining { get; set; }

        public int SpawnTimer { get; set; }          // Updated on true re-spawn

        public string SpawnTimeStr { get; set; }    // Update on spawn (last spawn time)

        public DateTime SpawnTimeDT { get; set; }

        public string KillTimeStr { get; set; } = "";     // Updated on each kill, erased on spawn

        public DateTime KillTimeDT { get; set; } = DateTime.MinValue;

        public string NextSpawnStr { get; set; } = "";    // Updated on each kill, erased on spawn

        public DateTime NextSpawnDT { get; set; } = DateTime.MinValue;

        public string LastSpawnName { get; set; }   // Updated on each spawn

        public ListViewItem ItmSpawnTimerList { get; set; }

        private bool listNeedsUpdate;

        public string AllNames { get; set; }
    }
}