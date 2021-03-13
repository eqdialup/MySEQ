namespace Structures
{
    public static class PrettyNames
    {
        private static readonly string[] s_Spawntypes = new[]
                                        { "Player", "NPC", "Corpse", "Any", "Pet" };

        private static readonly string[] s_HideTypes = new[]
                                        { "Visible", "Invisible", "Hidden/Stealth", "Invis to Undead", "Invis to Animals" };

        private static string ArrayIndextoStr(string[] source, int index)
        {
            return index < source.GetLowerBound(0) || index > source.GetUpperBound(0) ? $"{index}: Unknown" : source[index];
        }

        //private static bool IsActorDef(string line)
        //{
        //    return line.Contains("_ACTORDEF=");
        //}

                // PUBLIC ACCESSORS
        //public static string GetItemName(string ActorDef)
        //{
        //    return ItemsList.ContainsKey(ActorDef) ? ItemsList[ActorDef] : ActorDef;
        //}

        //public static string GetClass(int index) => s_Classes[index];

        //public static string GetRace(int index) => s_Races[index];

        public static string GetSpawnType(byte index)
        {
            return ArrayIndextoStr(s_Spawntypes, index);
        }

        public static string GetHideStatus(byte index)
        {
            return ArrayIndextoStr(s_HideTypes, index);
        }
    }
}
