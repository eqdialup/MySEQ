using myseq.Properties;
using Structures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;

namespace myseq
{
    // Does the map loading etc.

    public class EQMap
    {
        private MapCon mapCon;
        private MapLine mapLine;
        public EQData eq;

        public MobTrails trails = new MobTrails();

        private readonly List<MapLine> mapLines = new List<MapLine>();
        private readonly List<MapText> mapTexts = new List<MapText>();
        public List<MapLine> Lines => mapLines;
        public List<MapText> Texts => mapTexts;

        // Events
        public event Action<EQMap> ExitMap;  // Fires when the map is unloaded

        public event Action<EQMap> EnterMap; // Fires when the map is loaded

        // Trigger ExitMap event and reset necessary components
        protected void OnExitMap()
        {
            ExitMap?.Invoke(this);
            mapCon?.ResetInfoWindow();
            eq?.DefaultSpawnLoc();
            eq.SelectedID = 99999;
        }

        protected void OnEnterMap()
        {
            EnterMap?.Invoke(this);
        }

        public void SetComponents(MapCon mapCon, EQData eq)
        {
            this.mapCon = mapCon;
            this.eq = eq;
        }

        public void AddMapText(MapText work)
        {
            Texts.Add(work);
        }

        public void DeleteMapText(MapText work)
        {
            Texts.Remove(work);
        }

        internal void ClearMapStructures()
        {
            mapLine?.APoints.Clear();
            mapLines?.Clear();
            mapTexts?.Clear();
            Lines?.Clear();
            Texts?.Clear();
            mapCon.CalcExtents(Lines);
        }

        public bool LoadMap(string filename)
        {
            if (!File.Exists(filename))
            {
                LogLib.WriteLine($"File not found: {filename}", LogLevel.Error);
                return false;
            }

            try
            {
                bool isLoYMap = filename.EndsWith("_1.txt") || filename.EndsWith("_2.txt") || filename.EndsWith("_3.txt");
                return LoadLoYMap(filename, !isLoYMap);
            }
            catch (Exception ex)
            {
                var msg = $"Failed to load map {filename}: {ex.Message}";
                LogLib.WriteLine(msg, LogLevel.Error);
                return false;
            }
        }

        private bool LoadLoYMap(string filename, bool resetMap)
        {
            if (resetMap)
            {
                ResetMap();
            }

            bool isLoaded = LoadLoYMapInternal(filename);

            if (isLoaded)
            {
                OptimizeMap();
                eq?.CalculateMapLinePens(Lines, Texts); // Pre-calculate all pen colors used for map drawing.
                OnEnterMap();
            }

            return isLoaded;
        }

        internal bool LoadLoYMapInternal(string filename) // In-game EQ format
        {
            if (!File.Exists(filename))
            {
                LogLib.WriteLine($"File not found: {filename}", LogLevel.Error);
                return false;
            }

            LogLib.WriteLine($"Loading Zone Map (LoY): {filename}", LogLevel.Debug);

            int numTexts = 0;
            int numLines = 0;

            try
            {
                foreach (var line in File.ReadLines(filename))
                {
                    if (line.StartsWith("L") || line.StartsWith("P"))
                    {
                        ParseLP(line, ref numTexts, ref numLines);
                    }
                    else
                    {
                        LogLib.WriteLine($"Warning: Ignoring invalid format line in map '{filename}': {line}", LogLevel.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                LogLib.WriteLine($"Error reading lines from file '{filename}': {ex.Message}", LogLevel.Error);
                return false;
            }

            LogLib.WriteLine($"Loaded {Lines.Count} lines and {numTexts} texts", LogLevel.Debug);
            return numTexts > 0 || Lines.Count > 0;
        }

        internal void ParseLP(string line, ref int numTexts, ref int numLines)
        {
            if (line.StartsWith("L"))
            {
                Lines.Add(new MapLine(line));
                numLines++;
            }
            else if (line.StartsWith("P"))
            {
                Texts.Add(new MapText(line));
                numTexts++;
            }
        }

        private void ResetMap()
        {
            eq.MobsTimers.ResetTimers();
            OnExitMap();
            trails.Clear();
            ClearMapStructures();
        }

        public void LoadDummyMap()
        {
            LogLib.WriteLine($"Loading Dummy Map", LogLevel.Debug);
            OnExitMap();
            trails.Clear();
            ClearMapStructures();
            OnEnterMap();
        }

        #region Optimize Map

        internal void OptimizeMap()
        {
            if (Lines == null) return;
            
            List<MapLine> linesToRemove = new List<MapLine>();
            linesToRemove?.Clear();
            MapLine lastline = null;
            FindVoidLines(linesToRemove, lastline);
            RemoveLines(linesToRemove);
            // Put in offsets for use when drawing text on map, for duplicate text at same location
            //OptimizeText();
        }

        private static int ProdPoint(float prod, int droppoint)
        {
            if (prod > 0.9999f)
            {
                droppoint = 1;
            }

            return droppoint;
        }

        internal void FindVoidLines(List<MapLine> linesToRemove, MapLine lastLine)
        {
            foreach (var currentLine in Lines)
            {
                if (currentLine == null || lastLine == null) continue;

                int currentCount = currentLine.APoints.Count;
                int lastCount = lastLine.APoints.Count;

                if (currentCount < 2 || lastCount < 2) continue;

                // Check if the current line starts where the last line ends
                if (AreLinesConnected(currentLine, lastLine, out bool isReversed, out int dropPoint))
                {
                    MergeLines(lastLine, currentLine, dropPoint, isReversed);
                    linesToRemove.Add(currentLine);
                }

                lastLine = currentLine;
            }
        }

        private bool AreLinesConnected(MapLine currentLine, MapLine lastLine, out bool isReversed, out int dropPoint)
        {
            dropPoint = 0;
            isReversed = false;

            var currentStart = GetMapPoint(currentLine, 0);
            var currentEnd = GetMapPoint(currentLine, currentLine.APoints.Count - 1);

            var lastEnd = GetMapPoint(lastLine, lastLine.APoints.Count - 1);
            var lastStart = GetMapPoint(lastLine, 0);

            var currentColor = currentLine.LineColor;
            var lastColor = lastLine.LineColor;

            // Check if the lines are connected at the start or end
            if (PointsAreEqual(ref currentStart, ref lastEnd, currentColor, lastColor))
            {
                dropPoint = ShouldDropPoint(currentLine, lastLine, reverse: false);
                return true;
            }
            else if (PointsAreEqual(ref currentEnd, ref lastStart, currentColor, lastColor))
            {
                dropPoint = ShouldDropPoint(currentLine, lastLine, reverse: true);
                isReversed = true;
                return true;
            }

            return false;
        }

        internal static bool PointsAreEqual(ref MapPoint thispoint, ref MapPoint lastpoint, Pen thisColor, Pen lastColor) => lastpoint.X == thispoint.X && lastpoint.Y == thispoint.Y && lastpoint.Z == thispoint.Z && thisColor.Color == lastColor.Color;

        private void MergeLines(MapLine lastLine, MapLine currentLine, int dropPoint, bool isReversed)
        {
            int lastCount = lastLine.APoints.Count;
            int currentCount = currentLine.APoints.Count;

            var mergedPoints = new List<PointF>();

            // Add points from the last line, skipping the last point if needed
            for (int i = 0; i < lastCount - dropPoint; i++)
            {
                var point = GetMapPoint(lastLine, i);
                mergedPoints.Add(new PointF(point.X, point.Y));
            }

            // Add points from the current line, reversing order if necessary
            if (isReversed)
            {
                for (int i = currentCount - 1; i > 0; i--)
                {
                    var point = GetMapPoint(currentLine, i);
                    mergedPoints.Add(new PointF(point.X, point.Y));
                }
            }
            else
            {
                for (int i = 1; i < currentCount; i++)
                {
                    var point = GetMapPoint(currentLine, i);
                    mergedPoints.Add(new PointF(point.X, point.Y));
                }
            }

            // Update the last line with the merged points
            lastLine.LinePoints = mergedPoints.ToArray();
            lastLine.APoints = mergedPoints.Select(p => new MapPoint { X = (int)p.X, Y = (int)p.Y }).ToList();
        }

        private int ShouldDropPoint(MapLine currentLine, MapLine lastLine, bool reverse)
        {
            int dropPoint = 0;

            var prev = reverse ? GetMapPoint(currentLine, currentLine.APoints.Count - 2) : GetMapPoint(lastLine, lastLine.APoints.Count - 2);
            var current = reverse ? GetMapPoint(currentLine, currentLine.APoints.Count - 1) : GetMapPoint(currentLine, 0);
            var next = reverse ? GetMapPoint(lastLine, 1) : GetMapPoint(currentLine, 1);

            float dotProduct = CalcDotProduct(prev, current, next);

            return ProdPoint(dotProduct, dropPoint);
        }

        private static MapPoint GetMapPoint(MapLine line, int index)
        {
            return line.APoints[index];
        }

        internal void RemoveLines(List<MapLine> linesToRemove)
        {
            foreach (MapLine lineToRemove in linesToRemove)
            {
                Lines.Remove(lineToRemove);
            }
        }

        internal void OptimizeText()
        {
            var index = 0;
            foreach (MapText tex1 in Texts)
            {
                var index2 = 0;
                foreach (MapText tex2 in Texts)
                {
                    if (index2 > index && tex1.X == tex2.X && tex1.Y == tex2.Y && tex1.Z == tex2.Z && tex1.Label != tex2.Label)
                    {
                        tex2.Offset = tex1.Offset + (int)(2.0f * Settings.Default.MapLabel.Size);
                    }
                    index2++;
                }
                index++;
            }
        }

        internal float CalcDotProduct(MapPoint lastprev, MapPoint thispoint, MapPoint thisnext)
        {
            // Convert MapPoint to vectors
            var v1 = new Vector3(lastprev.X, lastprev.Y, lastprev.Z);
            var v2 = new Vector3(thispoint.X, thispoint.Y, thispoint.Z);
            var v3 = new Vector3(thisnext.X, thisnext.Y, thisnext.Z);

            // Check if any points are identical
            if (v1.Equals(v2) || v2.Equals(v3))
            {
                return 1.0f;
            }

            // Calculate vectors
            var vec1 = v2 - v1;
            var vec2 = v3 - v2;

            // Calculate dot product
            var dotProduct = Vector3.Dot(vec1, vec2);

            // Calculate magnitudes
            var lenVec1 = vec1.Length();
            var lenVec2 = vec2.Length();

            // Avoid division by zero
            if (lenVec1 == 0 || lenVec2 == 0)
            {
                return 0;
            }

            // Calculate cosine of the angle
            var cosTheta = dotProduct / (lenVec1 * lenVec2);

            // Clamp the value to avoid potential issues with floating-point precision
            cosTheta = Math.Max(-1.0f, Math.Min(1.0f, cosTheta));

            // Return the result
            return cosTheta;
        }

        internal float CalcDotProducts(MapPoint lastprev, MapPoint thispoint, MapPoint thisnext)
        {
            float x1 = lastprev.X;
            float y1 = lastprev.Y;
            float z1 = lastprev.Z;
            float x2 = thispoint.X;
            float y2 = thispoint.Y;
            float z2 = thispoint.Z;
            float x3 = thisnext.X;
            float y3 = thisnext.Y;
            float z3 = thisnext.Z;

            if ((x1 == x2 && y1 == y2 && z1 == z2) || (x2 == x3 && y2 == y3 && z2 == z3))
            {
                return 1.0f;
            }

            var lenV1 = Math.Sqrt(((x2 - x1) * (x2 - x1)) + ((y2 - y1) * (y2 - y1)) + ((z2 - z1) * (z2 - z1)));

            var lenV2 = Math.Sqrt(((x3 - x2) * (x3 - x2)) + ((y3 - y2) * (y3 - y2)) + ((z3 - z2) * (z3 - z2)));

            var lenV3 = Math.Sqrt(((x3 - x1) * (x3 - x1)) + ((y3 - y1) * (y3 - y1)) + ((z3 - z1) * (z3 - z1)));

            return (float)(lenV3 / (lenV1 + lenV2));
        }

        #endregion Optimize Map
    }
}