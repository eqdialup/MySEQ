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
        public int X { get; set; }

        public int Y { get; set; }

        public int Z { get; set; }
    }

    public struct MobTrailPoint
    {
        public int X { get; set; }
        public int Y { get; set; }
        public MobTrailPoint(Spawninfo sp)
        {
            X = (int)sp.X;
            Y = (int)sp.Y;
        }
    }

    public class MapText
    {
        public string label {get; set; } = "";

        public int offset {get; set; }

        public SolidBrush color {get; set; }

        public SolidBrush draw_color {get; set; }

        public Pen draw_pen {get; set; }

        public int x {get; set;}

        public int y {get; set;}

        public int z {get; set;}

        public int size {get; set;} = 2;

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
        public int MaxZ {get; set; }

        public int MinZ {get; set; }

        //public string name {get; set; } = "";

        public Pen LineColor {get; set; }

        public Pen Draw_color {get; set; }

        public Pen Fade_color {get; set; }

        public ArrayList APoints {get; set; }

        public PointF[] LinePoints {get; set; }

        public MapLine(string line)
        {
            IFormatProvider NumFormat = new CultureInfo("en-US");
            MapPoint point1 = new MapPoint();
            MapPoint point2 = new MapPoint();
            APoints = new ArrayList();

            var parsedLine = line.Remove(0, 1).Split(",".ToCharArray());

            if (parsedLine.Length == 9)
            {
                point1.X = -(int)float.Parse(parsedLine[0], NumFormat);
                point1.Y = -(int)float.Parse(parsedLine[1], NumFormat);
                point1.Z = (int)float.Parse(parsedLine[2], NumFormat);

                point2.X = -(int)float.Parse(parsedLine[3], NumFormat);
                point2.Y = -(int)float.Parse(parsedLine[4], NumFormat);
                point2.Z = -(int)float.Parse(parsedLine[5], NumFormat);

                var R = int.Parse(parsedLine[6].PadRight(4).Substring(0, 3));
                var G = int.Parse(parsedLine[7].PadRight(4).Substring(0, 3));
                var B = int.Parse(parsedLine[8].PadRight(4).Substring(0, 3));
                LineColor = new Pen(new SolidBrush(Color.FromArgb(R, G, B)));

                APoints.Add(point1);
                APoints.Add(point2);

                LinePoints = new PointF[2];

                LinePoints[0] = new PointF(point1.X, point1.Y);

                LinePoints[1] = new PointF(point2.X, point2.Y);
            }
            else
            {
                LogLib.WriteLine("Warning - Line has an invalid format and will be ignored.", LogLevel.Warning);
            }
        }

        public MapPoint Point(int index)
        {
            return (MapPoint)APoints[index];
        }
    }
}