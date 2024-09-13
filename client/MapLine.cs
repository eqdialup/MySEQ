// Class Files

using System.Drawing;
using Structures;
using System.Globalization;
using System;
using System.Collections.Generic;

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
        public string Label { get; set; } = "";

        public int Offset { get; set; }

        public SolidBrush LineColor { get; private set; }

        public SolidBrush Draw_color { get; set; }

        public Pen Draw_pen { get; set; }

        public int X { get; private set; }

        public int Y { get; private set; }

        public int Z { get; private set; }

        public int Size { get; private set; } = 2;

        public MapText(string line)
        {
            IFormatProvider NumFormat = new CultureInfo("en-US");
            var dataRecord = line.Remove(0, 1);
            var parsedLine = dataRecord.Split(",".ToCharArray());

            if (parsedLine.Length >= 7)
            {
                X = -(int)float.Parse(parsedLine[0], NumFormat);
                Y = -(int)float.Parse(parsedLine[1], NumFormat);
                Z = (int)float.Parse(parsedLine[2], NumFormat);
                InitializeColor(parsedLine[3], parsedLine[4], parsedLine[5]);
                Size = int.Parse(parsedLine[6], NumFormat);

                Label = string.Join(",", parsedLine, 7, parsedLine.Length - 7);
            }
            else
            {
                LogLib.WriteLine("Warning - Label-line has an invalid format and will be ignored.", LogLevel.Warning);
            }
        }

        private void InitializeColor(string r, string g, string b)
        {
            try
            {
                int red = int.Parse(r);
                int green = int.Parse(g);
                int blue = int.Parse(b);
                LineColor = new SolidBrush(Color.FromArgb(red, green, blue));
            }
            catch (Exception ex)
            {
                LogLib.WriteLine($"Error initializing color: {ex.Message}", LogLevel.Warning);
                LineColor = new SolidBrush(Color.Black);  // Default to black on error
            }
        }
    }

    public class MapLine
    {
        public int MaxZ { get; private set; }
        public int MinZ { get; private set; }
        public Pen LineColor { get; private set; }
        public Pen Draw_color { get; set; }
        public Pen Fade_color { get; set; }
        public List<MapPoint> APoints { get; set; }
        public PointF[] LinePoints { get; set; }

        public MapLine(string line)
        {
            IFormatProvider numFormat = new CultureInfo("en-US");
            APoints?.Clear();
            APoints = new List<MapPoint>();

            var parsedLine = line.Remove(0, 1).Split(',');

            if (parsedLine.Length == 9)
            {
                MapPoint point1 = ParsePoint(parsedLine, 0, numFormat);
                MapPoint point2 = ParsePoint(parsedLine, 3, numFormat);
                LineColor = ParseColor(parsedLine, 6);

                APoints.Add(point1);
                APoints.Add(point2);

                LinePoints = new PointF[]
                {
                new PointF(point1.X, point1.Y),
                new PointF(point2.X, point2.Y)
                };

                NormalizeZ(); // Normalize Z values during initialization
            }
            else
            {
                LogLib.WriteLine("Warning - Line has an invalid format and will be ignored.", LogLevel.Warning);
            }
        }

        private MapPoint ParsePoint(string[] parsedLine, int startIndex, IFormatProvider numFormat)
        {
            return new MapPoint
            {
                X = -(int)float.Parse(parsedLine[startIndex], numFormat),
                Y = -(int)float.Parse(parsedLine[startIndex + 1], numFormat),
                Z = (int)float.Parse(parsedLine[startIndex + 2], numFormat)
            };
        }

        private Pen ParseColor(string[] parsedLine, int startIndex)
        {
            int R = int.Parse(parsedLine[startIndex].Trim());
            int G = int.Parse(parsedLine[startIndex + 1].Trim());
            int B = int.Parse(parsedLine[startIndex + 2].Trim());

            return new Pen(new SolidBrush(Color.FromArgb(R, G, B)));
        }

        public MapPoint Point(int index)
        {
            return (MapPoint)APoints[index];
        }

        // Normalize the Z values for the line
        private void NormalizeZ()
        {
            if (APoints.Count == 0)
                return;

            MapPoint firstPoint = Point(0);
            MinZ = MaxZ = firstPoint.Z;

            foreach (MapPoint point in APoints)
            {
                if (point.Z < MinZ)
                {
                    MinZ = point.Z;
                }
                if (point.Z > MaxZ)
                {
                    MaxZ = point.Z;
                }
            }
        }


    }

public class MapExtents
{
    public float MinX { get; private set; } = -1000;
    public float MaxX { get; private set; } = 1000;
    public float MinY { get; private set; } = -1000;
    public float MaxY { get; private set; } = 1000;
    public float MinZ { get; private set; } = -1000;
    public float MaxZ { get; private set; } = 1000;

    public MapExtents(float minX, float maxX, float minY, float maxY, float minZ, float maxZ)
    {
        MinX = minX;
        MaxX = maxX;
        MinY = minY;
        MaxY = maxY;
        MinZ = minZ;
        MaxZ = maxZ;
    }

    public MapExtents()
    {
        Reset();
    }

    public void Reset()
    {
        MinX = MinY = MinZ = float.MaxValue;
        MaxX = MaxY = MaxZ = float.MinValue;
    }

    public void Update(float x, float y, float z)
    {
        MinX = Math.Min(MinX, x);
        MaxX = Math.Max(MaxX, x);
        MinY = Math.Min(MinY, y);
        MaxY = Math.Max(MaxY, y);
        MinZ = Math.Min(MinZ, z);
        MaxZ = Math.Max(MaxZ, z);
    }
}
}