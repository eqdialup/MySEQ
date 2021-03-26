// Class Files

using System.Collections;
using System.Drawing;

namespace myseq
{
    public struct MapPoint
    {
        public int x;

        public int y;

        public int z;
    }

    public struct MobTrailPoint
    {
        public int x;
        public int y;
    }

    public class MapText
    {
        public string label = "";

        public int offset;

        public SolidBrush color;

        public SolidBrush draw_color;

        public Pen draw_pen;

        public int x;

        public int y;

        public int z;

        public int size = 2;
    }

    public class MapLine
    {
        public MapLine()
        {
            aPoints = new ArrayList();
        }

        public int maxZ;

        public int minZ;

        public string name = "";

        public Pen color;

        public Pen draw_color;

        public Pen fade_color;

        public ArrayList aPoints;

        public PointF[] linePoints;

        public MapPoint Point(int index)

        {
            return (MapPoint)aPoints[index];
        }
    }
}