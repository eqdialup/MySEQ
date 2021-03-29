using myseq.Properties;
using Structures;
using System;
using System.Drawing;
using System.IO;

namespace myseq
{
    public class ConColors : EQData
    {
        // player details
        public SPAWNINFO playerinfo = new SPAWNINFO();

        // Blech, some UI stuff... but at least it's shareable between several users

        public SolidBrush[] conColors = new SolidBrush[1000];
        public int GreenRange { get; set; }

        public int CyanRange { get; set; }

        public int GreyRange { get; set; }
        public int YellowRange { get; set; } = 3;

        public void FillConColors(FrmMain f1)
        {
            try
            {
                int c = 0;

                int level;

                if (Settings.Default.LevelOverride == -1)
                {
                    f1.toolStripLevel.Text = "Auto";
                    level = playerinfo.Level;
                }
                else
                {
                    level = Settings.Default.LevelOverride;
                    f1.toolStripLevel.Text = level.ToString();
                }
                YellowRange = 3;

                CyanRange = -5;

                GreenRange = (-1) * level;

                GreyRange = (-1) * level;

                // If using SoD/Titanium Con Colors
                VersionColorVariation(level);

                // Set the Grey Cons
                for (c = 0; c < (GreyRange + level); c++)
                {
                    conColors[c] = new SolidBrush(Color.Gray);
                }

                // Set the Green Cons
                for (; c < (GreenRange + level); c++)
                {
                    conColors[c] = new SolidBrush(Color.Lime);
                }

                // Set the Light Blue Cons
                for (; c < (CyanRange + level); c++)
                {
                    conColors[c] = new SolidBrush(Color.Aqua);
                }

                // Set the Dark Blue Cons
                for (; c < level; c++)
                {
                    conColors[c] = new SolidBrush(Color.Blue);
                }

                // Set the Same Level Con
                conColors[c++] = new SolidBrush(Color.White);

                // Yellow Cons
                for (; c < (level + YellowRange + 1); c++)
                {
                    conColors[c] = new SolidBrush(Color.Yellow);
                }

                // 4 levels of bright red
                conColors[c++] = new SolidBrush(Color.Red);
                conColors[c++] = new SolidBrush(Color.Red);
                conColors[c++] = new SolidBrush(Color.Red);
                conColors[c++] = new SolidBrush(Color.Red);

                // Set the remaining levels to dark red
                for (; c < 1000; c++)
                {
                    conColors[c] = new SolidBrush(Color.Maroon);
                }
            }
            catch { }
        }

        private void VersionColorVariation(int level)
        // Check for SoD, SoF or Real EQ con levels in use
        {
            var ConColorsFile = System.IO.Path.Combine(Settings.Default.CfgDir, "ConLevels.Ini");
            if (Settings.Default.SoDCon)
            {
                YellowRange = 2;

                GreyRange = (-1) * level;

                if (level < 9)
                {
                    GreenRange = -3;

                    CyanRange = -7;
                }
                else if (level < 13)
                {
                    GreenRange = -5;

                    CyanRange = -3;
                }
                else if (level < 17)
                {
                    GreenRange = -6;

                    CyanRange = -4;
                }
                else if (level < 21)
                {
                    GreenRange = -7;

                    CyanRange = -5;
                }
                else if (level < 25)
                {
                    GreenRange = -8;

                    CyanRange = -6;
                }
                else if (level < 29)
                {
                    GreenRange = -9;

                    CyanRange = -7;
                }
                else if (level < 31)
                {
                    GreenRange = -10;

                    CyanRange = -8;
                }
                else if (level < 33)
                {
                    GreenRange = -11;

                    CyanRange = -8;
                }
                else if (level < 37)
                {
                    GreenRange = -12;

                    CyanRange = -9;
                }
                else if (level < 41)
                {
                    GreenRange = -13;

                    CyanRange = -10;
                }
                else if (level < 45)
                {
                    GreenRange = -15;

                    CyanRange = -11;
                }
                else if (level < 49)
                {
                    GreenRange = -16;

                    CyanRange = -12;
                }
                else if (level < 53)
                {
                    GreenRange = -17;

                    CyanRange = -13;
                }
                else if (level < 55)
                {
                    GreenRange = -18;

                    CyanRange = -14;
                }
                else if (level < 57)
                {
                    GreenRange = -19;

                    CyanRange = -14;
                }
                else
                {
                    GreenRange = -20;

                    CyanRange = -15;
                }
            }

            // If using SoF Con Colors
            else if (Settings.Default.SoFCon)
            {
                YellowRange = 3;

                CyanRange = -5;

                if (level < 9)
                {
                    GreyRange = -3;

                    GreenRange = -7;
                }
                else if (level < 10)
                {
                    GreyRange = -4;

                    GreenRange = -3;
                }
                else if (level < 13)
                {
                    GreyRange = -5;

                    GreenRange = -3;
                }
                else if (level < 17)
                {
                    GreyRange = -6;

                    GreenRange = -4;
                }
                else if (level < 21)
                {
                    GreyRange = -7;

                    GreenRange = -5;
                }
                else if (level < 25)
                {
                    GreyRange = -8;

                    GreenRange = -6;
                }
                else if (level < 29)
                {
                    GreyRange = -9;

                    GreenRange = -7;
                }
                else if (level < 31)
                {
                    GreyRange = -10;

                    GreenRange = -8;
                }
                else if (level < 33)
                {
                    GreyRange = -11;

                    GreenRange = -8;
                }
                else if (level < 37)
                {
                    GreyRange = -12;

                    GreenRange = -9;
                }
                else if (level < 41)
                {
                    GreyRange = -13;

                    GreenRange = -10;
                }
                else if (level < 45)
                {
                    GreyRange = -15;

                    GreenRange = -11;
                }
                else if (level < 49)
                {
                    GreyRange = -16;

                    GreenRange = -12;
                }
                else if (level < 53)
                {
                    GreyRange = -17;

                    GreenRange = -13;
                }
                else if (level < 55)
                {
                    GreyRange = -18;

                    GreenRange = -14;
                }
                else if (level < 57)
                {
                    GreyRange = -19;

                    GreenRange = -14;
                }
                else
                {
                    GreyRange = -20;

                    GreenRange = -15;
                }
            }
            else if (File.Exists(ConColorsFile))
            {
                IniFile Ini = new IniFile(ConColorsFile);

                string sIniValue = Ini.ReadValue("Con Levels", level.ToString(), "0/0/0");
                var yellowLevels = Ini.ReadValue("Con Levels", "0", "3");
                string[] ConLevels = sIniValue.Split('/');

                GreyRange = Convert.ToInt32(ConLevels[0]) - level + 1;

                GreenRange = Convert.ToInt32(ConLevels[1]) - level + 1;

                CyanRange = Convert.ToInt32(ConLevels[2]) - level + 1;

                YellowRange = Convert.ToInt32(yellowLevels);
            }
            else if (Settings.Default.DefaultCon)
            {
                // Using Default Con Colors

                CyanRange = -5;

                if (level < 16) // verified
                {
                    GreyRange = -5;

                    GreenRange = -5;
                }
                else if (level < 19) // verified
                {
                    GreyRange = -6;

                    GreenRange = -5;
                }
                else if (level < 21) // verified
                {
                    GreyRange = -7;

                    GreenRange = -5;
                }
                else if (level < 22) // verified
                {
                    GreyRange = -7;

                    GreenRange = -6;
                }
                else if (level < 25) // verified
                {
                    GreyRange = -8;

                    GreenRange = -6;
                }
                else if (level < 28) // verified
                {
                    GreyRange = -9;

                    GreenRange = -7;
                }
                else if (level < 29) // verified
                {
                    GreyRange = -10;

                    GreenRange = -7;
                }
                else if (level < 31) // verified
                {
                    GreyRange = -10;

                    GreenRange = -8;
                }
                else if (level < 33) // verified
                {
                    GreyRange = -11;

                    GreenRange = -8;
                }
                else if (level < 34) // verified
                {
                    GreyRange = -11;

                    GreenRange = -9;
                }
                else if (level < 37) // verified
                {
                    GreyRange = -12;

                    GreenRange = -9;
                }
                else if (level < 40) // verified
                {
                    GreyRange = -13;

                    GreenRange = -10;
                }
                else if (level < 41) // Verified
                {
                    GreyRange = -14;

                    GreenRange = -10;
                }
                else if (level < 43) // Verified
                {
                    GreyRange = -14;

                    GreenRange = -11;
                }
                else if (level < 45)  // Verified
                {
                    GreyRange = -15;

                    GreenRange = -11;
                }
                else if (level < 46)  // Verified
                {
                    GreyRange = -15;

                    GreenRange = -12;
                }
                else if (level < 49)  // Verified
                {
                    GreyRange = -16;

                    GreenRange = -12;
                }
                else if (level < 51) // Verified at 50
                {
                    GreyRange = -17;

                    GreenRange = -13;
                }
                else if (level < 53)
                {
                    GreyRange = -18;

                    GreenRange = -14;
                }
                else if (level < 57)
                {
                    GreyRange = -20;

                    GreenRange = -15;
                }
                else
                {
                    GreyRange = -21;

                    GreenRange = -16;
                }
            }
        }

        public void UpdateMobListColors()
        {
            if (mobs != null)
            {
                foreach (SPAWNINFO si in mobs.Values)
                {
                    if (si.listitem != null)
                    {
                        if (si.Type == 2 || si.Type == 3 || si.isLDONObject)
                        {
                            si.listitem.ForeColor = Color.Gray;
                        }
                        else if (si.isEventController)
                        {
                            si.listitem.ForeColor = Color.DarkOrchid;
                        }
                        else
                        {
                            si.listitem.ForeColor = conColors[si.Level].Color;

                            if (si.listitem.ForeColor == Color.Maroon)
                                si.listitem.ForeColor = Color.Red;

                            // Change the colors to be more visible on white if the background is white

                            if (Settings.Default.ListBackColor == Color.White)
                            {
                                if (si.listitem.ForeColor == Color.White)
                                    si.listitem.ForeColor = Color.Black;
                                else if (si.listitem.ForeColor == Color.Yellow)
                                    si.listitem.ForeColor = Color.Goldenrod;
                            }

                            if (Settings.Default.ListBackColor == Color.Black && si.listitem.ForeColor == Color.Black)
                            {
                                si.listitem.ForeColor = Color.White;
                            }
                        }
                    }
                }
            }
        }
    }
}