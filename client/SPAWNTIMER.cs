using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using myseq.Properties;
using Structures;

namespace myseq
{
    public class Spawntimer
    {
        public Spawntimer(Spawninfo si, DateTime dt)

        {
            SpawnLoc = si.SpawnLoc;

            ZoneSpawnLoc = si.ZoneSpawnLoc;

            zone = "";

            X = si.X;

            Y = si.Y;

            Z = si.Z;

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

            X = float.Parse(parts[8]);

            Y = float.Parse(parts[9]);

            Z = float.Parse(parts[10]);
        }

        private DateTime GetDateTime(string input)
        {
            return input.Length > 0 ? Convert.ToDateTime(input) : default;
        }

        // Returns the data in a format which can be used with the (string) constructor.

        public string GetAsString() => $"{SpawnLoc};{SpawnCount};{SpawnTimer};{SpawnTimeStr};{KillTimeStr};{NextSpawnStr};{LastSpawnName};{AllNames};{X};{Y};{Z}";

        // st has been loaded from a file, and is the same spawn as "this" one.
        // Glean all useful information.

        public void Merge(Spawntimer st)
        {
            LogLib.WriteLine("Merging spawn timers:", LogLevel.Debug);

            SpawnCount = st.SpawnCount; // usually makes it > 1

            SpawnTimer = st.SpawnTimer; // woot!

            if (KillTimeDT == DateTime.MinValue) // woot!

            {
                KillTimeStr = st.KillTimeStr;

                NextSpawnDT = KillTimeDT.Add(new TimeSpan(0, 0, 0, SpawnTimer));

                NextSpawnStr = $"{NextSpawnDT.ToLongTimeString()} {NextSpawnDT.ToShortDateString()}";
            }
            else
            {
                // Enable the timer to start on first kill

                EnableTimer(st);
            }

            var namecount = 1;
            StringBuilder builder = new StringBuilder(AllNames);
            foreach (var name in st.AllNames.Split(','))
            {
                var bname = name.TrimName();
                if (AllNames.IndexOf(bname) < 0 && namecount < 11)
                {
                    builder.Append(", ").Append(bname);
                    namecount++;
                }
            }
            AllNames = builder.ToString();

            // update last spawn name to be what looks like named mobs
            foreach (var tname in AllNames.Split(','))
            {
                var mname = tname.TrimName();
                if (mname.RegexMatch())
                {
                    LastSpawnName = mname;
                    break;
                }
            }

            listNeedsUpdate = true;
        }

        private void EnableTimer(Spawntimer st)
        {
            if (st.SpawnTimer > 10)

            {
                TimeSpan Diff = new TimeSpan(0, 0, 0, SpawnTimer);

                if (DateTime.Now.Subtract(Diff) < st.SpawnTimeDT)

                {
                    SpawnTimeDT = st.SpawnTimeDT;

                    SpawnTimeStr = st.SpawnTimeStr;
                }

                if (DateTime.Now.Subtract(Diff) > st.KillTimeDT)

                {
                    KillTimeDT = DateTime.MinValue;
                }
                else
                {
                    KillTimeDT = st.KillTimeDT;

                    KillTimeStr = st.KillTimeStr;
                }

                if (DateTime.Now > st.NextSpawnDT)

                {
                    NextSpawnDT = DateTime.MinValue;

                    NextSpawnStr = "";
                }
                else
                {
                    NextSpawnDT = st.NextSpawnDT;

                    NextSpawnStr = st.NextSpawnStr;

                    KillTimeDT = st.KillTimeDT.Subtract(Diff);
                }
            }
        }

        // When will the mob spawn next? Returns 0 if not available.
        // TO DO: optimize this, as it is called much more often than the mob is being updated

        public int SecondsUntilSpawn(DateTime now)
        {
            var checkTimer = 0;

            if (NextSpawnDT != DateTime.MinValue)

            {
                TimeSpan Diff = NextSpawnDT.Subtract(now);

                checkTimer = (Diff.Hours * 3600) + (Diff.Minutes * 60) + Diff.Seconds;

                if (checkTimer <= 0)
                {
                    checkTimer = 0;
                }
            }

            return checkTimer;
        }

        public string GetDescription()
        {
            //var countTimer = GetCountTimer();
            StringBuilder spawnTimer = StBuilder();

            spawnTimer.Append("\n")
            .AppendFormat("Last Spawned At: {0}\n", SpawnTimeStr)
            .AppendFormat("Last Killed At: {0}\n", KillTimeStr)
            .AppendFormat("Next Spawn At: {0}\n", NextSpawnStr)
            .AppendFormat("Spawn Timer: {0} secs\n", SpawnTimer)
            .AppendFormat("Spawning In: {0}\n", GetCountdown().ToString("hh':'mm':'ss"))//countTimer)
            .AppendFormat("Spawn Count: {0}\n", SpawnCount)
            .AppendFormat("Y: {0:f3}  X: {1:f3}  Z: {2:f3}", Y, X, Z);
            return spawnTimer.ToString();
        }

        //private string GetCountTimer()
        //{
        //    var countTimer = "";

        //    if (NextSpawnDT != DateTime.MinValue)
        //    {
        //        TimeSpan Diff = NextSpawnDT.Subtract(DateTime.Now);

        //        countTimer = $"{Diff.Hours:00}:{Diff.Minutes:00}:{Diff.Seconds:00}";
        //    }

        //    return countTimer;
        //}

        private TimeSpan GetCountdown()
        {
            TimeSpan countdown = TimeSpan.Zero;
            if (NextSpawnDT != DateTime.MinValue)
            {
                countdown = NextSpawnDT.Subtract(DateTime.UtcNow);
            }
            return countdown;
        }

        private StringBuilder StBuilder()
        {
            StringBuilder spawnTimer = new StringBuilder();
            spawnTimer.AppendFormat("Spawn Name: {0}\n", LastSpawnName);

            if (AllNames.Length > 0)
            {
                var names = AllNames.Split(',');
                NameCount(spawnTimer, "Names encountered: ", names);
            }

            return spawnTimer;
        }

        private static void NameCount(StringBuilder spawnTimer, string names_to_add, string[] names)
        {
            foreach (var name in names)
            {
                var namet = name.Trim();

                if ((namet.Length + names_to_add.Length + 2) < 45)
                {
                    names_to_add += ", " + namet;
                }
                else
                {
                    spawnTimer.Append(names_to_add);
                    spawnTimer.Append("\n");
                    names_to_add = namet;
                }
            }
            spawnTimer.Append(names_to_add);
        }

        // A true re-spawn has been detected

        public string ReSpawn(string name)
        {
            var log = "";
            try
            {
                SpawnCount++;
                // if it looks like a named, leave last spawn name alone
                if (LastSpawnName.Length > 0)
                {
                    // See if mob name starts with capital letter or #
                    if (!LastSpawnName.RegexMatch())
                    {
                        LastSpawnName = name;
                    }
                }
                else
                {
                    LastSpawnName = name;
                }
                NextSpawnStr = "";

                NextSpawnDT = DateTime.MinValue;

                SpawnTimeDT = DateTime.Now;

                SpawnTimeStr = $"{SpawnTimeDT.ToLongTimeString()} {SpawnTimeDT.ToShortDateString()}";

                // put name at beginning of list of AllNames

                var newnames = name.TrimName();
                var namecount = 1;
                StringBuilder builder = new StringBuilder(AllNames);
                foreach (var tname in AllNames.Split(','))
                {
                    var bname = tname.TrimName();
                    if (newnames.IndexOf(bname) < 0 && namecount < 8)
                    {
                        builder.Append(", ").Append(bname);
                        namecount++;
                    }
                }
                AllNames = builder.ToString();

                log = GetRespawnTime(name, log);
            }
            catch (Exception ex) { LogLib.WriteLine($"Error updating Timer SPAWNTIMER for {name}: ", ex); }

            listNeedsUpdate = true;

            return log;
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
            var isInList = true;

            if (ItmSpawnTimerList == null)
            {
                ItmSpawnTimerList = new ListViewItem(LastSpawnName.FixMobName());

                isInList = false;

                listNeedsUpdate = true;

                for (var t = 0; t < 10; t++)
                {
                    ItmSpawnTimerList.SubItems.Add("");
                }
            }

            SpawnTimeRemaining = SecondsUntilSpawn(DateTime.Now);

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
            else
            {
                ItmSpawnTimerList.ForeColor = SpawnTimeRemaining < 90 ? Color.Orange : Color.Goldenrod;
            }

            UpdateList();

            if (!isInList)

            {
                return ItmSpawnTimerList;
            }
            else
            {
                return null; // it already is in the list - don't add it again
            }
        }

        private void UpdateList()
        {
            if (listNeedsUpdate)
            {
                ItmSpawnTimerList.SubItems[1].Text = SpawnTimeRemaining.ToString();

                ItmSpawnTimerList.SubItems[2].Text = SpawnTimer.ToString();

                ItmSpawnTimerList.SubItems[3].Text = zone;

                ItmSpawnTimerList.SubItems[4].Text = X.ToString();

                ItmSpawnTimerList.SubItems[5].Text = Y.ToString();

                ItmSpawnTimerList.SubItems[6].Text = Z.ToString();

                ItmSpawnTimerList.SubItems[7].Text = SpawnCount.ToString();

                ItmSpawnTimerList.SubItems[8].Text = SpawnTimeStr;

                ItmSpawnTimerList.SubItems[9].Text = KillTimeStr;

                ItmSpawnTimerList.SubItems[10].Text = NextSpawnStr;

                listNeedsUpdate = false;
            }
            else
            {
                if (SpawnTimeRemaining.ToString() != ItmSpawnTimerList.SubItems[1].Text)
                {
                    ItmSpawnTimerList.SubItems[1].Text = SpawnTimeRemaining.ToString();
                }
            }
        }

        public string ZoneSpawnLoc { get; set; }

        public string SpawnLoc { get; set; }            // x,y = primary key, set on first spawn

        public string zone { get; set; }

        public bool sticky { get; set; }

        public float Y { get; set; }

        public float X { get; set; }

        public float Z { get; set; }

        public bool filtered { get; set; }

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