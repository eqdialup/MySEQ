using System;
using System.Collections.Generic;
using System.Linq;
using Structures;

namespace myseq
{
public interface IMarkLookup
{
    void MarkLookups(string name, ref bool filterMob);
}

public class MarkLookup : IMarkLookup
{
    private EQData eq;
    
    // A list to hold search terms and their corresponding filters.
    private List<LookupEntry> lookups;

    public MarkLookup()
    {
        // Initialize with 5 entries for ranks 1-5
        lookups = Enumerable.Range(1, 5).Select(rank => new LookupEntry(rank.ToString())).ToList();
    }

    public void SetComponents(EQData eq)
    {
        this.eq = eq;
    }

    public void MarkLookups(string name, ref bool filterMob)
    {
        // Update lookup entries with the current name and filter
        foreach (var entry in lookups)
        {
            entry.UpdateLookup(name, ref filterMob);
        }

        // Process each mob in the EQData
        foreach (Spawninfo sp in eq.GetMobsReadonly().Values)
        {
            sp.isLookup = false;
            sp.lookupNumber = "";
            
            foreach (var entry in lookups)
            {
                entry.ApplyLookup(sp);
            }
        }
    }

    // Class to encapsulate search and filter logic for each rank
    private class LookupEntry
    {
        public string Rank { get; }
        private string search;
        private bool filter;

        public LookupEntry(string rank)
        {
            Rank = rank;
            search = "";
            filter = false;
        }

        public void UpdateLookup(string name, ref bool filterMob)
        {
            if (name.StartsWith(Rank + ":"))
            {
                if (name.Length > Rank.Length + 1)
                {
                    search = name.Substring(Rank.Length + 1);
                    filter = filterMob;
                }
                else
                {
                    search = "";
                    filter = false;
                }
            }
        }

        public void ApplyLookup(Spawninfo sp)
        {
            if (search.Length > 1)
            {
                SubLookup(sp, search, filter, Rank);
            }
        }

        private static void SubLookup(Spawninfo sp, string search, bool filter, string ln)
        {
            var levelCheck = false;

            if (search.StartsWith("L:", StringComparison.OrdinalIgnoreCase))
            {
                int.TryParse(search.Substring(2), out var searchLevel);
                levelCheck = LevelCheck(sp, searchLevel);
            }

            if (levelCheck || search.GetRegex().Match(sp.Name).Success)
            {
                sp.isLookup = true;
                sp.lookupNumber = ln;
                sp.hidden = false;
                FilterHidden(sp, filter);
            }
        }

        private static bool LevelCheck(Spawninfo sp, int searchLevel)
        {
            return searchLevel != 0 && sp.Level == searchLevel;
        }

        private static void FilterHidden(Spawninfo sp, bool filter)
        {
            if (filter) sp.hidden = true;
        }
    }
}

}