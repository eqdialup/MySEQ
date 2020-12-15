using System;
using System.Drawing;
using System.Collections;
using System.IO;

namespace MySEQ
{

#if false
#region ColorConverter
    /// <summary>
    /// Summary description for ColorConverter.
    /// Class to convert a ShowEQ line color into a c# color.
    /// Optionally can load a unix style RGB.txt to increase the number of
    /// named colors above that which .NET natively supports
    /// </summary>
    public class ColorConverter
    {
        private Hashtable NamedColors;

        public ColorConverter() 
        {
            NamedColors = new Hashtable();
        }

        private class NamedColor
        {
            public string Name;
            public Color Color;
        }

        private string FixName(string sInput)
        {
            string sFixed = sInput;
            sFixed = sFixed.ToLower().Replace(" ", "");
            sFixed = sFixed.Replace("grey", "gray");
            return sFixed;
        }

        private bool IsKnownColor(string sName)
        {
            if ( NamedColors.ContainsKey(sName) )
                return true;
            else
                return Color.FromName(sName).IsKnownColor;
        }

        private void ProcessFileLine(string sLine)
        {
            int start = sLine.IndexOf("\t\t");
            if ( start != -1 )
            {
                string colorName = FixName(sLine.Substring(start + 2));
                if ( !IsKnownColor(colorName) )
                {
                    string colorValue = sLine.Substring(0, start).Trim();
                    colorValue = colorValue.Replace("  ", " ");
                    colorValue = colorValue.Replace("  ", " ");
                    colorValue = colorValue.Replace("  ", " ");
                    string[] colorValues = colorValue.Split(' ');
                    if ( (colorValues.Length == 3) && (colorName.Length > 0) )
                    {
                        NamedColor nc = new NamedColor();
                        nc.Name = (colorName);
                        byte red   = byte.Parse(colorValues[0]);
                        byte green = byte.Parse(colorValues[1]);
                        byte blue  = byte.Parse(colorValues[2]);
                        nc.Color   = Color.FromArgb(red, green, blue);
                        if ( !NamedColors.ContainsKey(nc.Name) )
                            NamedColors.Add(nc.Name, nc);
                    }
                }
            }

        }

        public void Initialise(string filename)
        {
            NamedColors = new Hashtable();
            if ( File.Exists(filename) )
            {
                using (StreamReader sr = new StreamReader(filename)) 
                {
                    while ( sr.Peek() >= 0 )
                    {
                        string sLine = sr.ReadLine();
                        ProcessFileLine(sLine);
                    }
                }       
            }

        }

        public Color StringToColor(string sInput)
        {
            Color cColor;
            string sFixed = FixName(sInput);
            if ( sFixed.StartsWith("#") && sFixed.Length == 7 ) // an RGB value in the form #RRGGBB 
            {
                int red   = System.Convert.ToInt32(sFixed.Substring(1, 2), 16);
                int green = System.Convert.ToInt32(sFixed.Substring(3, 2), 16);
                int blue  = System.Convert.ToInt32(sFixed.Substring(5, 2), 16);
                return Color.FromArgb(red, green, blue);;
            }
            else if ( NamedColors.ContainsKey(sFixed) ) // a named Color
            {
                NamedColor nc = (NamedColor)NamedColors[sFixed];
                return nc.Color;
            }
            else
            {
                cColor = Color.FromName(sFixed);
                if ( cColor.IsKnownColor )
                    return cColor;
                else
                {
                    return Color.PaleGreen;
                }
            }
        }
    }
#endregion
#endif
    public class ProcessInfo
    {
        public uint pid;
        public string gamerName;
        public System.Windows.Forms.MenuItem menuItem;

        public ProcessInfo(uint pid, string gamerName)
        {
            this.pid = pid;
            this.gamerName = gamerName;
            this.menuItem = new System.Windows.Forms.MenuItem();
        }
        
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            ProcessInfo p = (ProcessInfo) obj;

            return (pid == p.pid);
        }
        
        public override int GetHashCode()
        {
            return pid.GetHashCode();
        }
    }


}
