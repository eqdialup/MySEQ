// Class Files
using System.Drawing;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.IO;

namespace myseq
{
    public class Point3D
    {
        public float X { get; }
        public float Y { get; }
        public float Z { get; }

        public Point3D(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }

    // Class representing a label or text object
    public class MapLabel
    {
        public Point3D Position { get; }
        public Color TextColor { get; set; }
        public string Text { get; }

        public MapLabel(Point3D position, Color textColor, string text)
        {
            Position = position;
            TextColor = textColor;
            Text = text;
        }
    }

    public class LineSegment
    {
        public Point3D Start { get; }
        public Point3D End { get; }
        public Color LineColor { get; set; }

        public LineSegment(Point3D start, Point3D end, Color lineColor)
        {
            Start = start;
            End = end;
            LineColor = lineColor;
        }
    }

    // Class that processes the map file and stores the elements
    public class MapData
    {
        public List<LineSegment> LineSegments { get; private set; } = new List<LineSegment>();
        public List<MapLabel> Labels { get; private set; } = new List<MapLabel>();
        public float MinX { get; private set; } = -1000;
        public float MaxX { get; private set; } = 1000;
        public float MinY { get; private set; } = -1000;
        public float MaxY { get; private set; } = 1000;
        public float MinZ { get; private set; } = -1000;
        public float MaxZ { get; private set; } = 1000;

        public void LoadMapData(string filePath)
        {
            foreach (var line in File.ReadLines(filePath))
            {
                if (line.StartsWith("L "))
                {
                    ParseLineSegment(line);
                }
                else if (line.StartsWith("P "))
                {
                    ParseLabel(line);
                }
            }
        }

        private void ParseLineSegment(string line)
        {
            IFormatProvider numFormat = new CultureInfo("en-US");
            // Example line: L -100,-200,0,100,-200,0,0,0,0
            var parts = line.Substring(2).Split(',');

            Point3D start = new Point3D(
                -float.Parse(parts[0], numFormat),
                -float.Parse(parts[1], numFormat),
                float.Parse(parts[2], numFormat)
            );
            Point3D end = new Point3D(
                -float.Parse(parts[3], numFormat),
                -float.Parse(parts[4], numFormat),
                float.Parse(parts[5], numFormat)
            );
            Color color = Color.FromArgb(
                int.Parse(parts[6], numFormat),  // Red
                int.Parse(parts[7], numFormat),  // Green
                int.Parse(parts[8], numFormat)   // Blue
            );

            LineSegments.Add(new LineSegment(start, end, color));
        }

        private void ParseLabel(string line)
        {
            IFormatProvider numFormat = new CultureInfo("en-US");
            // Example line: P -75,-150,0,125,0,125,3, THIS IS A MAP!
            var parts = line.Substring(2).Split(',');

            Point3D position = new Point3D(
                -float.Parse(parts[0], numFormat),
                -float.Parse(parts[1], numFormat),
                float.Parse(parts[2], numFormat)
            );
            Color textColor = Color.FromArgb(
                int.Parse(parts[3], numFormat),  // Red
                int.Parse(parts[4], numFormat),  // Green
                int.Parse(parts[5], numFormat)   // Blue
            );
            string text = parts[7].Trim(); // Assumes text starts from the 7th index

            Labels.Add(new MapLabel(position, textColor, text));
        }

        public (float minX, float maxX, float minY, float maxY, float minZ, float maxZ) GetBounds()
        {
            if (LineSegments.Count == 0)
            {
                return (MinX, MaxX, MinY, MaxY, MinZ, MaxZ);
            }

            // Initialize min/max values using the first line segment's coordinates
            float minX = LineSegments[0].Start.X;
            float maxX = LineSegments[0].Start.X;
            float minY = LineSegments[0].Start.Y;
            float maxY = LineSegments[0].Start.Y;
            float minZ = LineSegments[0].Start.Z;
            float maxZ = LineSegments[0].Start.Z;

            // Iterate through all line segments to find the min/max values
            foreach (var line in LineSegments)
            {
                // Compare start point
                minX = Math.Min(minX, line.Start.X);
                maxX = Math.Max(maxX, line.Start.X);
                minY = Math.Min(minY, line.Start.Y);
                maxY = Math.Max(maxY, line.Start.Y);
                minZ = Math.Min(minZ, line.Start.Z);
                maxZ = Math.Max(maxZ, line.Start.Z);

                // Compare end point
                minX = Math.Min(minX, line.End.X);
                maxX = Math.Max(maxX, line.End.X);
                minY = Math.Min(minY, line.End.Y);
                maxY = Math.Max(maxY, line.End.Y);
                minZ = Math.Min(minZ, line.End.Z);
                maxZ = Math.Max(maxZ, line.End.Z);
            }

            // Return the bounds as a tuple (minX, maxX, minY, maxY, minZ, maxZ)
            return (minX, maxX, minY, maxY, minZ, maxZ);
        }

        public void Update()
        {
            var bounds = GetBounds();
            MinX = bounds.minX;
            MaxX = bounds.maxX;
            MinY = bounds.minY;
            MaxY = bounds.maxY;
            MinZ = bounds.minZ;
            MaxZ = bounds.maxZ;
        }

        public void RemoveDuplicateLineSegments()
        {
            var uniqueLines = new HashSet<LineSegment>(new LineSegmentComparer());
            var nonDuplicateLines = new List<LineSegment>();

            foreach (var line in LineSegments)
            {
                if (uniqueLines.Add(line))
                {
                    nonDuplicateLines.Add(line); // If the line is unique, add to the list
                }
            }

            LineSegments = nonDuplicateLines; // Replace original list with non-duplicate lines
        }

        // Method to remove duplicate labels
        public void RemoveDuplicateLabels()
        {
            var uniqueLabels = new HashSet<MapLabel>(new LabelComparer());
            var nonDuplicateLabels = new List<MapLabel>();

            foreach (var label in Labels)
            {
                if (uniqueLabels.Add(label))
                {
                    nonDuplicateLabels.Add(label); // If the label is unique, add to the list
                }
            }

            Labels = nonDuplicateLabels; // Replace original list with non-duplicate labels
        }
    }

    // Comparer for LineSegment to detect duplicates
    public class LineSegmentComparer : IEqualityComparer<LineSegment>
    {
        public bool Equals(LineSegment x, LineSegment y)
        {
            if (x == null || y == null) return false;

            // Check if the start and end points match (in any direction) and colors are the same
            bool sameDirection = x.Start.X == y.Start.X && x.Start.Y == y.Start.Y && x.End.X == y.End.X && x.End.Y == y.End.Y;
            bool reverseDirection = x.Start.X == y.End.X && x.Start.Y == y.End.Y && x.End.X == y.Start.X && x.End.Y == y.Start.Y;

            return (sameDirection || reverseDirection) && x.LineColor == y.LineColor;
        }

        public int GetHashCode(LineSegment obj)
        {
            // Generate a hash code that accounts for both directions being considered equal
            int startHash = obj.Start.X.GetHashCode() ^ obj.Start.Y.GetHashCode();
            int endHash = obj.End.X.GetHashCode() ^ obj.End.Y.GetHashCode();

            // Combine the hashes in a way that makes (Start, End) and (End, Start) equal
            return (startHash ^ endHash) ^ obj.LineColor.GetHashCode();
        }
    }

    // Comparer for MapLabel to detect duplicates
    public class LabelComparer : IEqualityComparer<MapLabel>
    {
        public bool Equals(MapLabel x, MapLabel y)
        {
            if (x == null || y == null) return false;

            // Check if the position, color, and text are the same
            return x.Position.X == y.Position.X && x.Position.Y == y.Position.Y && x.TextColor == y.TextColor && x.Text == y.Text;
        }

        public int GetHashCode(MapLabel obj)
        {
            // Combine the hashes of the position, color, and text
            return obj.Position.X.GetHashCode() ^ obj.Position.Y.GetHashCode() ^ obj.TextColor.GetHashCode() ^ obj.Text.GetHashCode();
        }
    }
}