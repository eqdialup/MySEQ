using System;
using System.Collections;
using System.Text;
using System.Drawing;

using MySEQ.Structures;

namespace MySEQ
{
    public class ColorManager
    {
        
        private static ArrayList AllColorTools = new ArrayList();
        
        public class ColorTool
        {
            public Color color;
            public Pen pen;
            public SolidBrush brush;
        }
        
        public ColorManager()
        {
            // Walk thru all the System defined colors and keep a list of
            // them to compare against requested colors. To save resources,
            // we don't want to make any new colors if possible, just use the
            // closest available System defined color (there are ~140 of them).
            Array AllKnownColors = Enum.GetValues(typeof(KnownColor));
            
            foreach (KnownColor kc in AllKnownColors)
            {
                Color c = Color.FromKnownColor(kc);
                if (!c.IsSystemColor && c.Name != "Transparent")
                {   
                    ColorTool ct = new ColorTool();
                    ct.color = c;
                    ct.pen = new Pen(c);
                    ct.brush = new SolidBrush(c);
                    AllColorTools.Add(ct);
                }
            }
        }

        public static Color FindNearestSystemColor(Color rc)
        {
            return FindNearestSystemColorTool(rc).color;
        }
        
        public static Pen FindNearestSystemPen(Color rc)
        {
            return FindNearestSystemColorTool(rc).pen;
        }

        public static SolidBrush FindNearestSystemBrush(Color rc)
        {
            return FindNearestSystemColorTool(rc).brush;
        }
        
        private static ColorTool FindNearestSystemColorTool(Color rc)
        {
            double hd, sd, bd, td, ltd;
            ColorTool rtn = null;
            
            ltd = double.MaxValue;
            foreach (ColorTool gct in AllColorTools)
            {
                Color gc = gct.color;
                
                // Use a simple Euclidean formula to determine which of the system colors is 
                // closest to the requested color. Weight Hue twice as much as Saturation and
                // Brightness.
                hd = (2) * (1000/360) * (Math.Pow((rc.GetHue() - gc.GetHue()), 2));
                sd = (1) * (1000/1)   * (Math.Pow((rc.GetSaturation() - gc.GetSaturation()), 2));
                bd = (1) * (1000/1)   * (Math.Pow((rc.GetBrightness() - gc.GetBrightness()), 2));
                td = hd + sd + bd;
                
                // As the total difference gets lower, the color is getting closer. Keep
                // track of the lowest total difference, and it's owning color.
                if (td < ltd)
                {
                    ltd = td;
                    rtn = gct;
                    // Exact color match, we are done
                    if (td == 0)
                        break;
                }
            }
            return rtn;
        }
    }
}
