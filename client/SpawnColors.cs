using System.Drawing;
using myseq;
using myseq.Properties;

namespace Structures
{
    public class SpawnColors
    {
        public int GreenRange { get; set; } = -4;

        public int CyanRange { get; set; } = -5;

        public int GreyRange { get; set; } = -3;

        public int YellowRange { get; set; } = 3;

        public SolidBrush[] ConColors { get; set; } = new SolidBrush[500];

        public void FillConColors(MainForm f1, Spawninfo GamerInfo)//, SolidBrush[] conColors
        {
            int level = GamerInfo.Level;
            f1.toolStripLevel.Text = "Auto";

            if (Settings.Default.LevelOverride != -1)
            {
                level = Settings.Default.LevelOverride;
                f1.toolStripLevel.Text = level.ToString();
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
            for (; c < 500; c++)
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
                DefaultCon(level);
            }
            else
            {
                ConLevelFile(level);
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
            //if (level < 16) // verified
            //{
            //    GreyRange = -5;
            //    GreenRange = -5;
            //}
            //else if (level < 19) // verified
            //{
            //    GreyRange = -6;
            //    GreenRange = -5;
            //}
            //else if (level < 21) // verified
            //{
            //    GreyRange = -7;
            //    GreenRange = -5;
            //}
            //else if (level < 22) // verified
            //{
            //    GreyRange = -7;
            //    GreenRange = -6;
            //}
            //else if (level < 25) // verified
            //{
            //    GreyRange = -8;
            //    GreenRange = -6;
            //}
            //else if (level < 28) // verified
            //{
            //    GreyRange = -9;
            //    GreenRange = -7;
            //}
            //else if (level < 29) // verified
            //{
            //    GreyRange = -10;
            //    GreenRange = -7;
            //}
            //else if (level < 31) // verified
            //{
            //    GreyRange = -10;
            //    GreenRange = -8;
            //}
            //else if (level < 33) // verified
            //{
            //    GreyRange = -11;
            //    GreenRange = -8;
            //}
            //else if (level < 34) // verified
            //{
            //    GreyRange = -11;
            //    GreenRange = -9;
            //}
            //else if (level < 37) // verified
            //{
            //    GreyRange = -12;
            //    GreenRange = -9;
            //}
            //else if (level < 40) // verified
            //{
            //    GreyRange = -13;
            //    GreenRange = -10;
            //}
            //else if (level < 41) // Verified
            //{
            //    GreyRange = -14;
            //    GreenRange = -10;
            //}
            //else if (level < 43) // Verified
            //{
            //    GreyRange = -14;
            //    GreenRange = -11;
            //}
            //else if (level < 45)  // Verified
            //{
            //    GreyRange = -15;
            //    GreenRange = -11;
            //}
            //else if (level < 46)  // Verified
            //{
            //    GreyRange = -15;
            //    GreenRange = -12;
            //}
            //else if (level < 49)  // Verified
            //{
            //    GreyRange = -16;
            //    GreenRange = -12;
            //}
            //else if (level < 51) // Verified at 50
            //{
            //    GreyRange = -17;
            //    GreenRange = -13;
            //}
            //else if (level < 53)
            //{
            //    GreyRange = -18;
            //    GreenRange = -14;
            //}
            //else if (level < 57)
            //{
            //    GreyRange = -20;
            //    GreenRange = -15;
            //}
            //else
            //{
            //    GreyRange = -21;
            //    GreenRange = -16;
            //}
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
            //else if (level < 10)
            //{
            //    GreyRange = -4;

            //    GreenRange = -3;
            //}
            //else if (level < 13)
            //{
            //    GreyRange = -5;

            //    GreenRange = -3;
            //}
            //else if (level < 17)
            //{
            //    GreyRange = -6;

            //    GreenRange = -4;
            //}
            //else if (level < 21)
            //{
            //    GreyRange = -7;

            //    GreenRange = -5;
            //}
            //else if (level < 25)
            //{
            //    GreyRange = -8;

            //    GreenRange = -6;
            //}
            //else if (level < 29)
            //{
            //    GreyRange = -9;

            //    GreenRange = -7;
            //}
            //else if (level < 31)
            //{
            //    GreyRange = -10;

            //    GreenRange = -8;
            //}
            //else if (level < 33)
            //{
            //    GreyRange = -11;

            //    GreenRange = -8;
            //}
            //else if (level < 37)
            //{
            //    GreyRange = -12;

            //    GreenRange = -9;
            //}
            //else if (level < 41)
            //{
            //    GreyRange = -13;

            //    GreenRange = -10;
            //}
            //else if (level < 45)
            //{
            //    GreyRange = -15;

            //    GreenRange = -11;
            //}
            //else if (level < 49)
            //{
            //    GreyRange = -16;

            //    GreenRange = -12;
            //}
            //else if (level < 53)
            //{
            //    GreyRange = -17;

            //    GreenRange = -13;
            //}
            //else if (level < 55)
            //{
            //    GreyRange = -18;

            //    GreenRange = -14;
            //}
            //else if (level < 57)
            //{
            //    GreyRange = -19;

            //    GreenRange = -14;
            //}
            //else
            //{
            //    GreyRange = -20;

            //    GreenRange = -15;
            //}
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
            //else if (level < 13)
            //{
            //    GreenRange = -5;

            //    CyanRange = -3;
            //}
            //else if (level < 17)
            //{
            //    GreenRange = -6;

            //    CyanRange = -4;
            //}
            //else if (level < 21)
            //{
            //    GreenRange = -7;

            //    CyanRange = -5;
            //}
            //else if (level < 25)
            //{
            //    GreenRange = -8;

            //    CyanRange = -6;
            //}
            //else if (level < 29)
            //{
            //    GreenRange = -9;

            //    CyanRange = -7;
            //}
            //else if (level < 31)
            //{
            //    GreenRange = -10;

            //    CyanRange = -8;
            //}
            //else if (level < 33)
            //{
            //    GreenRange = -11;

            //    CyanRange = -8;
            //}
            //else if (level < 37)
            //{
            //    GreenRange = -12;

            //    CyanRange = -9;
            //}
            //else if (level < 41)
            //{
            //    GreenRange = -13;

            //    CyanRange = -10;
            //}
            //else if (level < 45)
            //{
            //    GreenRange = -15;

            //    CyanRange = -11;
            //}
            //else if (level < 49)
            //{
            //    GreenRange = -16;

            //    CyanRange = -12;
            //}
            //else if (level < 53)
            //{
            //    GreenRange = -17;

            //    CyanRange = -13;
            //}
            //else if (level < 55)
            //{
            //    GreenRange = -18;

            //    CyanRange = -14;
            //}
            //else if (level < 57)
            //{
            //    GreenRange = -19;

            //    CyanRange = -14;
            //}
            //else
            //{
            //    GreenRange = -20;

            //    CyanRange = -15;
            //}
        }
    }
}