using System.Drawing;
using myseq.Properties;

namespace Structures
{
    public class SpawnColors
    {
        public int GreenRange { get; set; } = -4;

        public int CyanRange { get; set; } = -5;

        public int GreyRange { get; set; } = -3;

        public int YellowRange { get; set; } = 3;

        public static readonly SolidBrush[] ConColors = new SolidBrush[200];

        public void FillConColors(Spawninfo GamerInfo)
        {
            int level = GamerInfo.Level;

            if (Settings.Default.LevelOverride != -1)
            {
                level = Settings.Default.LevelOverride;
            }

            GreenRange = -1 * level;

            GreyRange = -1 * level;

            // Variation on level depending on EQ version (SOD, SOF, LIVE)
            VersionColorVariation(level);

            int c;
            // Set the Grey Cons
            for (c = 0; c < GreyRange + level; c++)
            {
                ConColors[c] = new SolidBrush(Color.Gray);
            }

            // Set the Green Cons
            for (; c < GreenRange + level; c++)
            {
                ConColors[c] = new SolidBrush(Color.Lime);
            }

            // Set the Light Blue Cons
            for (; c < CyanRange + level; c++)
            {
                ConColors[c] = new SolidBrush(Color.Aqua);
            }

            // Set the Dark Blue Cons
            for (; c < level; c++)
            {
                ConColors[c] = new SolidBrush(Color.Blue);
            }

            // Set the Same Level Con
            ConColors[c++] = new SolidBrush(Color.White);

            // Yellow Cons
            for (; c < level + YellowRange + 1; c++)
            {
                ConColors[c] = new SolidBrush(Color.Yellow);
            }

            // 4 levels of bright red
            ConColors[c++] = new SolidBrush(Color.Red);
            ConColors[c++] = new SolidBrush(Color.Red);
            ConColors[c++] = new SolidBrush(Color.Red);
            ConColors[c++] = new SolidBrush(Color.Red);

            // Set the remaining levels to dark red
            for (; c < 200; c++)
            {
                ConColors[c] = new SolidBrush(Color.Maroon);
            }
        }

        private void VersionColorVariation(int level)
        // Check for SoD, SoF or Live EQ con levels in use
        {
            if (Settings.Default.SoDCon)
            {
                SoDCon(level);
            }
            else if (Settings.Default.SoFCon)
            {
                SoFCon(level);
            }
            else if (Settings.Default.DefaultCon)
            {
                ConLevelFile(level);
            }
            else
            {
                DefaultCon(level);
            }
        }

        private void ConLevelFile(int level)
        {
            var Ini = new IniFile("ConLevels.Ini");

            var sIniValue = Ini.ReadValue("Con Levels", level.ToString(), "0/0/0");
            var yellowLevels = Ini.ReadValue("Con Levels", "0", "3");
            var ConLevels = sIniValue.Split('/');

            GreyRange = int.Parse(ConLevels[0]) - level + 1;

            GreenRange = int.Parse(ConLevels[1]) - level + 1;

            CyanRange = int.Parse(ConLevels[2]) - level + 1;

            YellowRange = int.Parse(yellowLevels);
        }

        private void DefaultCon(int level)
        {
            GreyRange = -(int)(level / 2.94);
            GreenRange = -(int)(level / 3.80);
        }

        private void SoFCon(int level)
        {
            if (level < 9)
            {
                GreyRange = -3;
                GreenRange = -7;
            }
            else
            {
                GreyRange = -(int)(level / 2.99);
                GreenRange = -(int)(level / 4.06);
            }
        }

        private void SoDCon(int level)
        {
            YellowRange = 2;
            GreyRange = -1 * level;

            if (level < 9)
            {
                GreenRange = -3;
                CyanRange = -7;
            }
            else
            {
                GreenRange = -(int)(level / 2.94);
                CyanRange = -(int)(level / 3.97);
            }
        }
    }
}