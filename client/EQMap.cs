using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using myseq.Properties;
using Structures;


namespace myseq
{
    // Does the map loading etc.

    public class EQMap
    {
        private MapCon mapCon;
        public EQData eq;

        public MobTrails trails = new MobTrails();

        private static readonly List<MapLine> mapLines = new List<MapLine>();
        private static readonly List<MapText> mapTexts = new List<MapText>();
        public List<MapLine> Lines { get; } = mapLines;
        public List<MapText> Texts { get; } = mapTexts;

        // Events

        public delegate void ExitMapHandler(EQMap Map);

        public event ExitMapHandler ExitMap; // Fires when the map is unloaded

        public delegate void EnterMapHandler(EQMap Map);


        public event EnterMapHandler EnterMap; // Fires when the map is loaded

        protected void OnExitMap()
        {
            ExitMap?.Invoke(this);

            if (mapCon != null)
            {
                // reset spawn information window

                mapCon.ResetInfoWindow();

                eq.selectedID = 99999;

                eq.SpawnX = -1.0f;

                eq.SpawnY = -1.0f;
            }
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
            Lines.Clear();
            Texts.Clear();
            eq.CalcExtents(Lines);
        }

        public bool Loadmap(string filename)
        {
            try
            {
                if (File.Exists(filename))
                {
                    if (filename.EndsWith("_1.txt") || filename.EndsWith("_2.txt") || filename.EndsWith("_3.txt"))
                    {
                        if (!LoadLoYMap(filename, false))
                        {
                            return false;
                        }
                    }
                    else if (!LoadLoYMap(filename, true))
                    {
                        return false;
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                var msg = $"Failed to load map {filename}: {ex.Message}";
                LogLib.WriteLine(msg);
                return false;
            }
        }

        private bool LoadLoYMap(string filename, bool resetmap)
        {
            if (resetmap)
            {
                eq.mobsTimers.ResetTimers();
                OnExitMap();
                trails.Clear();
                ClearMapStructures();
            }

            var rc = LoadLoYMapInternal(filename);

            if (rc)
            {
                OptimizeMap();

                eq.CalculateMapLinePens(Lines, Texts); // pre-calculate all pen colors used for map drawing.

                OnEnterMap();
            }

            return rc;
        }

        internal bool LoadLoYMapInternal(string filename) //ingame EQ format
        {
            var numtexts = 0;
            var numlines = 0;
            var curLine = 0;

            if (!File.Exists(filename))
            {
                LogLib.WriteLine($"File not found loading {filename} in loadLoYMap", LogLevel.Error);
                return false;
            }

            LogLib.WriteLine($"Loading Zone Map (LoY): {filename}");

            foreach (var line in File.ReadAllLines(filename))
            {
                if (line.StartsWith("L") || line.StartsWith("P"))
                {
                    ParseLP(line, ref numtexts, ref numlines);
                    curLine++;
                }
                else
                {
                    LogLib.WriteLine($"Warning - {line} in map '{filename}' has an invalid format and will be ignored.", LogLevel.Warning);
                }
            }
            LogLib.WriteLine($"{curLine} lines processed.", LogLevel.Debug);
            LogLib.WriteLine($"Loaded {Lines.Count} lines", LogLevel.Debug);

            return numtexts > 0 || Lines.Count > 0;
        }

        internal void ParseLP(string line, ref int numtexts, ref int numlines)
        {
            if (line.StartsWith("L"))
            {
                MapLine work = new MapLine(line);
                Lines.Add(work);
                numlines++;
            }
            else if (line.StartsWith("P"))
            {
                MapText work = new MapText(line);
                Texts.Add(work);
                numtexts++;
            }
        }

        public void LoadDummyMap()
        {
            OnExitMap();
            trails.Clear();
            ClearMapStructures();
            OnEnterMap();
        }

        #region Optimize Map

        internal void OptimizeMap()
        {
            if (Lines != null)
            {
                List<MapLine> linesToRemove = new List<MapLine>();
                MapLine lastline = null;
                FindvoidLines(linesToRemove, lastline);
                RemoveLines(linesToRemove);
                NormalizeMaxMinZ();
                // Put in offsets for use when drawing text on map, for duplicate text at same location
                OptimizeText();
            }
        }

        internal void FindvoidLines(List<MapLine> linesToRemove, MapLine lastline)
        {
            float prod;
            foreach (MapLine line in Lines)
            {
                MapLine thisline = line;
                if (thisline != null && lastline != null)
                {
                    var thiscount = thisline.APoints.Count;
                    var lastcount = lastline.APoints.Count;

                    MapPoint thispoint = (MapPoint)thisline.APoints[0];
                    MapPoint thisnext = (MapPoint)thisline.APoints[1];
                    MapPoint lastpoint = (MapPoint)lastline.APoints[lastcount - 1];
                    MapPoint lastprev = (MapPoint)lastline.APoints[lastcount - 2];

                    Pen thisColor = thisline.LineColor;
                    Pen lastColor = lastline.LineColor;

                    int droppoint;
                    if (PointsAreEqual(ref thispoint, ref lastpoint, thisColor, lastColor))
                    {
                        droppoint = 0;

                        // Take Dot Product to see if lines have 0 degrees between angle

                        // Basic Dot Product, where varies from -1 at 180 degrees to 1 at 0 degrees

                        if ((thiscount > 1) && (lastcount > 1))
                        {
                            prod = CalcDotProduct(lastprev, thispoint, thisnext);

                            if (prod > 0.9999f)
                            {
                                droppoint = 1;
                            }
                        }

                        // Second Line Starts at End of First Line

                        lastline.LinePoints = new PointF[thiscount + lastcount - 1 - droppoint];

                        for (var p = 0; p < (lastcount - droppoint); p++)
                        {
                            MapPoint tmp = (MapPoint)lastline.APoints[p];

                            lastline.LinePoints[p] = new PointF(tmp.X, tmp.Y);
                        }

                        if (droppoint == 1)
                        {
                            lastline.APoints.RemoveAt(lastcount - 1);
                        }

                        for (var p = 1; p < thiscount; p++)
                        {
                            MapPoint temp = GetMapPoint(thisline, p);
                            lastline.LinePoints[p + lastcount - 1 - droppoint] = new PointF(temp.X, temp.Y);

                            lastline.APoints.Add(temp);
                        }
                        linesToRemove.Add(thisline);
                        thisline = lastline;
                    }
                    else
                    {
                        droppoint = 0;

                        thispoint = (MapPoint)thisline.APoints[thiscount - 1];

                        MapPoint thisprev = (MapPoint)thisline.APoints[thiscount - 2];
                        lastpoint = (MapPoint)lastline.APoints[0];

                        MapPoint lastnext = (MapPoint)lastline.APoints[1];

                        if (lastpoint.X == thispoint.X && lastpoint.Y == thispoint.Y && lastpoint.Z == thispoint.Z && thisColor.Color == lastColor.Color)
                        {
                            prod = CalcDotProduct(thisprev, thispoint, lastnext);

                            if (prod > 0.9999f)
                            {
                                droppoint = 1;
                            }

                            // Second Line Starts at End of First Line

                            lastline.LinePoints = new PointF[thiscount + lastcount - 1 - droppoint];

                            if (droppoint == 1)
                            {
                                lastline.APoints.RemoveAt(0);
                            }

                            for (var p = 0; p < (thiscount - 1); p++)
                            {
                                MapPoint temp = GetMapPoint(thisline, p);

                                lastline.APoints.Insert(p, temp);
                            }

                            thiscount = lastline.APoints.Count;

                            for (var p = 0; p < thiscount; p++)
                            {
                                MapPoint tmp = (MapPoint)lastline.APoints[p];

                                lastline.LinePoints[p] = new PointF(tmp.X, tmp.Y);
                            }

                            linesToRemove.Add(thisline);

                            thisline = lastline;
                        }
                    }
                }

                lastline = thisline;
            }
        }

        internal static bool PointsAreEqual(ref MapPoint thispoint, ref MapPoint lastpoint, Pen thisColor, Pen lastColor) => lastpoint.X == thispoint.X && lastpoint.Y == thispoint.Y && lastpoint.Z == thispoint.Z && thisColor.Color == lastColor.Color;

        internal static MapPoint GetMapPoint(MapLine thisline, int p)
        {
            MapPoint tmp = (MapPoint)thisline.APoints[p];
            return new MapPoint
            {
                X = tmp.X,
                Y = tmp.Y,
                Z = tmp.Z
            };
        }

        internal void NormalizeMaxMinZ()
        {
            foreach (MapLine line in Lines)
            {
                line.MaxZ = line.MinZ = line.Point(0).Z;
                for (var j = 1; j < line.APoints.Count; j++)
                {
                    if (line.MinZ > line.Point(j).Z)
                    {
                        line.MinZ = line.Point(j).Z;
                    }

                    if (line.MaxZ < line.Point(j).Z)
                    {
                        line.MaxZ = line.Point(j).Z;
                    }
                }
            }
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
                    if (index2 > index && tex1.x == tex2.x && tex1.y == tex2.y && tex1.z == tex2.z && tex1.label != tex2.label)
                    {
                        tex2.offset = tex1.offset + (int)(2.0f * Settings.Default.MapLabel.Size);
                    }
                    index2++;
                }
                index++;
            }
        }

        internal float CalcDotProduct(MapPoint lastprev, MapPoint thispoint, MapPoint thisnext)
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