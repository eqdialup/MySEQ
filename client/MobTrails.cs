using System.Collections.Generic;

using myseq.Properties;
using Structures;

namespace myseq
{
    public class MobTrails
    {
        private readonly List<MobTrailPoint> mobtrails = new List<MobTrailPoint>();

        private int collect_mobtrails_count;

        public List<MobTrailPoint> GetMobTrailsReadonly() => mobtrails;

        private void CollectMobTrails(EQData eq)
        {
            // Collect Mob Trails
            foreach (Spawninfo sp in eq.GetMobsReadonly().Values)
            {
                if (sp.Type == 1)
                {
                    MobTrailPoint work = new MobTrailPoint(sp);
                    if (!mobtrails.Contains(work))
                    {
                        mobtrails.Add(work);
                    }
                }
            }
        }

        public void CountMobTrails(EQData eq)
        {
            if (Settings.Default.CollectMobTrails)
            {
                if (collect_mobtrails_count > 12)
                {
                    collect_mobtrails_count = 0;
                    CollectMobTrails(eq);
                }
                collect_mobtrails_count++;
            }
        }

        public void Clear()
        {
            mobtrails.Clear();
        }
    }
}
