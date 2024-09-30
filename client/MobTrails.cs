using System;
using System.Collections.Generic;
using Structures;

namespace myseq
{
    public struct MobTrailPoint
    {
        public int X { get; }
        public int Y { get; }

        public MobTrailPoint(Spawninfo sp)
        {
            X = (int)sp.X;
            Y = (int)sp.Y;
        }

        public override bool Equals(object obj)
        {
            if (obj is MobTrailPoint other)
            {
                return X == other.X && Y == other.Y;
            }
            return false;
        }

        public override int GetHashCode()
        { return (X * 397) ^ Y; } // A common pattern for combining hash codes
    }

    public class MobTrails
    {
        private readonly HashSet<MobTrailPoint> mobtrails = new HashSet<MobTrailPoint>();
        private int collect_mobtrails_count;

        public IReadOnlyCollection<MobTrailPoint> GetMobTrailsReadonly() => mobtrails;

        public void Clear()
        {
            mobtrails.Clear();
        }

        public void CountMobTrails(EQData eq)
        {
            if (++collect_mobtrails_count > 12)
            {
                collect_mobtrails_count = 0;
                CollectMobTrails(eq);
            }
        }

        private void CollectMobTrails(EQData eq)
        {
            foreach (Spawninfo sp in eq.GetMobsReadonly().Values)
            {
                if (sp.Type == 1)
                {
                    // Automatically handles duplicates since HashSet ignores duplicates
                    mobtrails.Add(new MobTrailPoint(sp));
                }
            }
        }
    }
}