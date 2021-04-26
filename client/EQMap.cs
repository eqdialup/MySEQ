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

                mapCon.lblMobInfo.Text = "Spawn Information Window";

                mapCon.lblMobInfo.BackColor = Color.White;

                mapCon.lblMobInfo.Visible = true;

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

            if (numtexts > 0 || Lines.Count > 0)
            {
                return true;
            }
            return false;
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
                    var thiscount = thisline.aPoints.Count;
                    var lastcount = lastline.aPoints.Count;

                    MapPoint thispoint = (MapPoint)thisline.aPoints[0];
                    MapPoint thisnext = (MapPoint)thisline.aPoints[1];
                    MapPoint lastpoint = (MapPoint)lastline.aPoints[lastcount - 1];
                    MapPoint lastprev = (MapPoint)lastline.aPoints[lastcount - 2];

                    Pen thisColor = thisline.color;
                    Pen lastColor = lastline.color;

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

                        lastline.linePoints = new PointF[thiscount + lastcount - 1 - droppoint];

                        for (var p = 0; p < (lastcount - droppoint); p++)
                        {
                            MapPoint tmp = (MapPoint)lastline.aPoints[p];

                            lastline.linePoints[p] = new PointF(tmp.x, tmp.y);
                        }

                        if (droppoint == 1)
                        {
                            lastline.aPoints.RemoveAt(lastcount - 1);
                        }

                        for (var p = 1; p < thiscount; p++)
                        {
                            MapPoint temp = GetMapPoint(thisline, p);
                            lastline.linePoints[p + lastcount - 1 - droppoint] = new PointF(temp.x, temp.y);

                            lastline.aPoints.Add(temp);
                        }
                        linesToRemove.Add(thisline);
                        thisline = lastline;
                    }
                    else
                    {
                        droppoint = 0;

                        thispoint = (MapPoint)thisline.aPoints[thiscount - 1];

                        MapPoint thisprev = (MapPoint)thisline.aPoints[thiscount - 2];
                        lastpoint = (MapPoint)lastline.aPoints[0];

                        MapPoint lastnext = (MapPoint)lastline.aPoints[1];

                        if (lastpoint.x == thispoint.x && lastpoint.y == thispoint.y && lastpoint.z == thispoint.z && thisColor.Color == lastColor.Color)
                        {
                            prod = CalcDotProduct(thisprev, thispoint, lastnext);

                            if (prod > 0.9999f)
                            {
                                droppoint = 1;
                            }

                            // Second Line Starts at End of First Line

                            lastline.linePoints = new PointF[thiscount + lastcount - 1 - droppoint];

                            if (droppoint == 1)
                            {
                                lastline.aPoints.RemoveAt(0);
                            }

                            for (var p = 0; p < (thiscount - 1); p++)
                            {
                                MapPoint temp = GetMapPoint(thisline, p);

                                lastline.aPoints.Insert(p, temp);
                            }

                            thiscount = lastline.aPoints.Count;

                            for (var p = 0; p < thiscount; p++)
                            {
                                MapPoint tmp = (MapPoint)lastline.aPoints[p];

                                lastline.linePoints[p] = new PointF(tmp.x, tmp.y);
                            }

                            linesToRemove.Add(thisline);

                            thisline = lastline;
                        }
                    }
                }

                lastline = thisline;
            }
        }

        internal static bool PointsAreEqual(ref MapPoint thispoint, ref MapPoint lastpoint, Pen thisColor, Pen lastColor) => lastpoint.x == thispoint.x && lastpoint.y == thispoint.y && lastpoint.z == thispoint.z && thisColor.Color == lastColor.Color;

        internal static MapPoint GetMapPoint(MapLine thisline, int p)
        {
            MapPoint tmp = (MapPoint)thisline.aPoints[p];
            return new MapPoint
            {
                x = tmp.x,
                y = tmp.y,
                z = tmp.z
            };
        }

        internal void NormalizeMaxMinZ()
        {
            foreach (MapLine line in Lines)
            {
                line.maxZ = line.minZ = line.Point(0).z;
                for (var j = 1; j < line.aPoints.Count; j++)
                {
                    if (line.minZ > line.Point(j).z)
                    {
                        line.minZ = line.Point(j).z;
                    }

                    if (line.maxZ < line.Point(j).z)
                    {
                        line.maxZ = line.Point(j).z;
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
            float x1 = lastprev.x;
            float y1 = lastprev.y;
            float z1 = lastprev.z;
            float x2 = thispoint.x;
            float y2 = thispoint.y;
            float z2 = thispoint.z;
            float x3 = thisnext.x;
            float y3 = thisnext.y;
            float z3 = thisnext.z;

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