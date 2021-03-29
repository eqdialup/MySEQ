using myseq.Properties;
using Structures;
using System;
using System.Collections;
using System.Drawing;

namespace myseq
{
    // Does the map loading etc.

    public class EQMap

    {
        private MapCon mapCon;

        private ListViewPanel SpawnList;

        private ListViewPanel SpawnTimerList;

        private ListViewPanel GroundItemList;

        private bool initialized;

        public MapPane mapPane;

        public EQData eq;

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

        public void SetComponents(MapCon mapCon,ListViewPanel SpawnList,ListViewPanel SpawnTimerList,ListViewPanel GroundItemList,MapPane mapPane,EQData eq)

        {
            this.mapCon=mapCon;

            this.SpawnList=SpawnList;

            this.SpawnTimerList=SpawnTimerList;

            this.GroundItemList = GroundItemList;

            this.eq=eq;

            initialized = true;
        }

        public void ClearMap()
        {
            try {
                if (!initialized) { throw new Exception("EQMapManager not initialized yet"); }

                eq.Clear();

                SpawnList.listView.BeginUpdate();
                SpawnTimerList.listView.BeginUpdate();
                GroundItemList.listView.BeginUpdate();

                SpawnList.listView.Items.Clear();

                SpawnTimerList.listView.Items.Clear();

                if (eq.mobsTimers.mobsTimer2.Count > 0)
                {
                    foreach (SPAWNTIMER st in eq.mobsTimers.mobsTimer2.Values)
                    {
                        st.itmSpawnTimerList = null;
                    }
                }

                GroundItemList.listView.Items.Clear();

                SpawnList.listView.EndUpdate();
                SpawnTimerList.listView.EndUpdate();
                GroundItemList.listView.EndUpdate();

                eq.mobsTimers.ResetTimers();
            }
            catch (Exception ex) {LogLib.WriteLine("Error with ClearMap:", ex);}
        }

        public void LoadDummyMap(string mapname)

        {
            OnExitMap();

            ClearMap();

            eq.ClearMapStructures();

            eq.shortname = mapname;

            if (mapPane != null)
                mapPane.scale.Value = 100M;

            OnEnterMap();
        }

        public bool LoadMap(string filename)
        {
            eq.mobsTimers.ResetTimers();

            OnExitMap();

            ClearMap();

            eq.ClearMapStructures();

            bool rc = eq.LoadMapInternal(filename);

            if (rc)
            {
                OptimizeMap();

                eq.CalculateMapLinePens(); // pre-calculate all pen colors used for map drawing.
                OnEnterMap();
            }

            return rc;
        }

        public bool LoadLoYMap(string filename, bool resetmap)
        {
            if (resetmap)

            {
                eq.mobsTimers.ResetTimers();

                OnExitMap();

                ClearMap();

                eq.ClearMapStructures();
            }

            bool rc = eq.LoadLoYMapInternal(filename);

            if (rc)
            {
                OptimizeMap();

                eq.CalculateMapLinePens(); // pre-calculate all pen colors used for map drawing.

                OnEnterMap();
            }

            return rc;
        }

        public void OptimizeMap()
        {
            if (eq.lines == null)
                return;

            ArrayList linesToRemove = new ArrayList();
            MapLine lastline = null;
            float prod;
            var pointsdrop = 0;

            foreach (MapLine line in eq.lines)
            {
                MapLine thisline = line;
                if (thisline != null && lastline != null)
                {
                    var thiscount = thisline.aPoints.Count;
                    MapPoint thispoint = (MapPoint)thisline.aPoints[0];

                    MapPoint thisnext = (MapPoint)thisline.aPoints[1];

                    Pen thisColor = thisline.color;
                    var lastcount = lastline.aPoints.Count;
                    MapPoint lastpoint = (MapPoint)lastline.aPoints[lastcount - 1];
                    MapPoint lastprev = (MapPoint)lastline.aPoints[lastcount - 2];
                    Pen lastColor = lastline.color;

                    int droppoint;
                    if (lastpoint.x == thispoint.x && lastpoint.y == thispoint.y && lastpoint.z == thispoint.z && thisColor.Color == lastColor.Color)
                    {
                        droppoint = 0;

                        // Take Dot Product to see if lines have 0 degrees between angle

                        // Basic Dot Product, where varies from -1 at 180 degrees to 1 at 0 degrees

                        if ((thiscount > 1) && (lastcount > 1))
                        {
                            prod = CalcDotProduct(lastprev.x, lastprev.y, lastprev.z, thispoint.x, thispoint.y, thispoint.z, thisnext.x, thisnext.y, thisnext.z);

                            if (prod > 0.9999f)
                            {
                                pointsdrop++;

                                droppoint = 1;
                            }
                        }

                        // Second Line Starts at End of First Line

                        lastline.linePoints = new PointF[thiscount + lastcount - 1 - droppoint];

                        for (int p = 0; p < (lastcount - droppoint); p++)
                        {
                            MapPoint tmp = (MapPoint)lastline.aPoints[p];

                            lastline.linePoints[p] = new PointF(tmp.x, tmp.y);
                        }

                        if (droppoint == 1)
                            lastline.aPoints.RemoveAt(lastcount - 1);

                        for (int p = 1; p < thiscount; p++)
                        {
                            MapPoint tmp = (MapPoint)thisline.aPoints[p];

                            lastline.linePoints[p + lastcount - 1 - droppoint] = new PointF(tmp.x, tmp.y);

                            MapPoint temp = new MapPoint
                            {
                                x = tmp.x,

                                y = tmp.y,

                                z = tmp.z
                            };

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
                            prod = CalcDotProduct(thisprev.x, thisprev.y, thisprev.z, thispoint.x, thispoint.y, thispoint.z, lastnext.x, lastnext.y, lastnext.z);

                            if (prod > 0.9999f)
                            {
                                pointsdrop++;

                                droppoint = 1;

                                // look here

                            }

                            // Second Line is at beginning of first line

                            lastline.linePoints = new PointF[thiscount + lastcount - 1 - droppoint];

                            if (droppoint == 1)
                                lastline.aPoints.RemoveAt(0);

                            for (int p = 0; p < (thiscount - 1); p++)
                            {
                                MapPoint tmp = (MapPoint)thisline.aPoints[p];

                                MapPoint temp = new MapPoint
                                {
                                    x = tmp.x,

                                    y = tmp.y,

                                    z = tmp.z
                                };

                                lastline.aPoints.Insert(p, temp);
                            }

                            thiscount = lastline.aPoints.Count;

                            for (int p = 0; p < thiscount; p++)
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

            foreach (MapLine lineToRemove in linesToRemove) eq.lines.Remove(lineToRemove);
            foreach (MapLine line in eq.lines)
            {
                line.maxZ = line.minZ = line.Point(0).z;
                for (int j = 1; j < line.aPoints.Count; j++)
                {
                    if (line.minZ > line.Point(j).z)
                        line.minZ = line.Point(j).z;
                    if (line.maxZ < line.Point(j).z)
                        line.maxZ = line.Point(j).z;
                }
            }
            // Put in offsets for use when drawing text on map, for duplicate text at same location
            OptimizeText();
        }

        private void OptimizeText()
        {
            int index = 0;
            foreach (MapText tex1 in eq.texts)
            {
                int index2 = 0;
                foreach (MapText tex2 in eq.texts)
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

        public float CalcDotProduct(float x1, float y1, float z1, float x2, float y2, float z2, float x3, float y3, float z3)
        {
            if ((x1 == x2 && y1 == y2 && z1 == z2) || (x2 == x3 && y2 == y3 && z2 == z3))
                return 1.0f;

            double lenV1 = Math.Sqrt(((x2 - x1) * (x2 - x1)) + ((y2 - y1) * (y2 - y1)) + ((z2 - z1) * (z2 - z1)));

            double lenV2 = Math.Sqrt(((x3 - x2) * (x3 - x2)) + ((y3 - y2) * (y3 - y2)) + ((z3 - z2) * (z3 - z2)));

            double lenV3 = Math.Sqrt(((x3 - x1) * (x3 - x1)) + ((y3 - y1) * (y3 - y1)) + ((z3 - z1) * (z3 - z1)));

            return (float)(lenV3 / (lenV1 + lenV2));
        }
    }
}