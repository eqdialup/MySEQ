namespace Structures
{
    public static class PrettyNames
    {
//        private static readonly string[] Classes = GetArrayFromFile("Classes.txt");
//        private static readonly string[] Races = GetArrayFromFile("Races.txt");

        private static readonly string[] s_Spawntypes = new[]
                                        { "Player", "NPC", "Corpse", "Any", "Pet" };

        private static readonly string[] s_HideTypes = new[]
                                        { "Visible", "Invisible", "Hidden/Stealth", "Invis to Undead", "Invis to Animals" };

        private static string ArrayIndextoStr(string[] source, int index)
        {
            return index < source.GetLowerBound(0) || index > source.GetUpperBound(0) ? $"{index}: Unknown" : source[index];
        }
//        private static string CombineCfgFile(string fileName) => Path.Combine(Settings.Default.CfgDir, fileName);
        //private static string[] GetArrayFromFile(string file)
        //{
        //    ArrayList arList = new ArrayList();

        //    string line;

        //    if (File.Exists(CombineCfgFile(file)))
        //    {
        //        FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);

        //        StreamReader sr = new StreamReader(fs);
        //        do
        //        {
        //            line = sr.ReadLine();

        //            if (line != null)
        //            {
        //                line = line.Trim();

        //                if (line != "" && line.Substring(0, 1) != "#")
        //                    arList.Add(line);
        //            }
        //        } while (line != null);

        //        sr.Close();

        //        fs.Close();
        //    }

        //    return (string[])arList.ToArray(Type.GetType("System.String"));
        //}

        //public static string GetClass(int num) => ArrayIndextoStr(Classes, num);
        //public static string GetRace(int num)
        //{
        //    if (num == 2250)
        //        return "Interactive Object";

        //    return ArrayIndextoStr(Races, num);
        //}

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