// Class Files

using System.Collections;
using System.Drawing;
using Structures;
using System.Globalization;
using System;

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
        public MobTrailPoint(Spawninfo sp)
        {
            x = (int)sp.X;
            y = (int)sp.Y;
        }
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

        public MapText(string line)
        {
            IFormatProvider NumFormat = new CultureInfo("en-US");
            var dataRecord = line.Remove(0, 1);
            var parsedline = dataRecord.Split(",".ToCharArray());

            if (parsedline.Length >= 7)
            {
                x = -(int)float.Parse(parsedline[0], NumFormat);
                y = -(int)float.Parse(parsedline[1], NumFormat);
                z = (int)float.Parse(parsedline[2], NumFormat);
                var r = int.Parse(parsedline[3], NumFormat);
                var g = int.Parse(parsedline[4], NumFormat);
                var b = int.Parse(parsedline[5], NumFormat);
                color = new SolidBrush(Color.FromArgb(r, g, b));
                size = int.Parse(parsedline[6], NumFormat);
                for (var i = 7; i < parsedline.Length; i++)
                {
                    label = parsedline[i];
                }
            }
            else
            {
                LogLib.WriteLine("Warning - Label-line has an invalid format and will be ignored.", LogLevel.Warning);
            }
        }
    }

    public class MapLine
    {
        public int maxZ;

        public int minZ;

        public string name = "";

        public Pen color;

        public Pen draw_color;

        public Pen fade_color;

        public ArrayList aPoints;

        public PointF[] linePoints;

        public MapLine(string line)
        {
            IFormatProvider NumFormat = new CultureInfo("en-US");
            MapPoint point1 = new MapPoint();
            MapPoint point2 = new MapPoint();
            aPoints = new ArrayList();

            var parsedLine = line.Remove(0, 1).Split(",".ToCharArray());

            if (parsedLine.Length == 9)
            {
                point1.x = -(int)float.Parse(parsedLine[0], NumFormat);
                point1.y = -(int)float.Parse(parsedLine[1], NumFormat);
                point1.z = (int)float.Parse(parsedLine[2], NumFormat);

                point2.x = -(int)float.Parse(parsedLine[3], NumFormat);
                point2.y = -(int)float.Parse(parsedLine[4], NumFormat);
                point2.z = -(int)float.Parse(parsedLine[5], NumFormat);

                var R = int.Parse(parsedLine[6].PadRight(4).Substring(0, 3));
                var G = int.Parse(parsedLine[7].PadRight(4).Substring(0, 3));
                var B = int.Parse(parsedLine[8].PadRight(4).Substring(0, 3));
                color = new Pen(new SolidBrush(Color.FromArgb(R, G, B)));

                aPoints.Add(point1);
                aPoints.Add(point2);

                linePoints = new PointF[2];

                linePoints[0] = new PointF(point1.x, point1.y);

                linePoints[1] = new PointF(point2.x, point2.y);
            }
            else
            {
                LogLib.WriteLine("Warning - Line has an invalid format and will be ignored.", LogLevel.Warning);
            }
        }

        public MapPoint Point(int index)
        {
            return (MapPoint)aPoints[index];
        }
    }
}