using System;

using System.IO;

using SpeechLib;

using System.Data;

using System.Text;

using System.Drawing;

using System.Collections;

using System.Globalization;

using System.Windows.Forms;

using System.ComponentModel;

using System.Drawing.Drawing2D;

using System.Text.RegularExpressions;

using System.Runtime.InteropServices;



using Structures;



namespace myseq {



    // Does the map loading etc.

    public class EQMap

    {

        private MapCon mapCon = null;              // TODO: replace with a notification mechanism

        private ListViewPanel SpawnList = null;

        private ListViewPanel SpawnTimerList = null;

        private ListViewPanel GroundItemList = null;

        private bool initialized = false;

        public MapPane mapPane = null;             // TODO: replace with a notification mechanism

        public EQData eq = null;

        

        // Events

        public delegate void ExitMapHandler(EQMap Map);

        public event ExitMapHandler ExitMap; // Fires when the map is unloaded



        public delegate void EnterMapHandler(EQMap Map);

        public event EnterMapHandler EnterMap; // Fires when the map is loaded



        protected void OnExitMap()        

        {

            LogLib.WriteLine("Entering OnExitMap",LogLevel.Trace);

            if (ExitMap!=null)

            {

                ExitMap(this);

            }

            if (mapCon != null)
            {
                // reset spawn information window

                mapCon.lblMobInfo.Text = "Spawn Information Window";

                mapCon.lblMobInfo.BackColor = System.Drawing.Color.White;

                mapCon.lblMobInfo.Visible = true;

                eq.selectedID = 99999;

                eq.SpawnX = -1.0f;

                eq.SpawnY = -1.0f;
            }

            LogLib.WriteLine("Exiting OnExitMap",LogLevel.Trace);

        }

        

        protected void OnEnterMap()        

        {

            LogLib.WriteLine("Entering OnEnterMap",LogLevel.Trace);

            

            if (EnterMap!=null)

            {

                EnterMap(this);

            }

            LogLib.WriteLine("Exiting OnEnterMap",LogLevel.Trace);

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

        public void NewMap()
        {
            OnEnterMap();
        }

    

        public void ClearMap() 

        {

            LogLib.WriteLine("Entering EQMap.ClearMap()", LogLevel.Trace);

            try {                           

                if (!initialized) { throw new Exception("EQMapManager not initialized yet"); }

                eq.Clear();

                SpawnList.listView.BeginUpdate();
                SpawnTimerList.listView.BeginUpdate();
                GroundItemList.listView.BeginUpdate();

                SpawnList.listView.Items.Clear();

                SpawnTimerList.listView.Items.Clear();

                if (eq.mobsTimers.mobsTimer2.Count > 0)
                    foreach (SPAWNTIMER st in eq.mobsTimers.mobsTimer2.Values)
                        st.itmSpawnTimerList = null;

                GroundItemList.listView.Items.Clear();

                SpawnList.listView.EndUpdate();
                SpawnTimerList.listView.EndUpdate();
                GroundItemList.listView.EndUpdate();

                eq.mobsTimers.ResetTimers();

            }

            catch (Exception ex) {LogLib.WriteLine("Error with ClearMap:", ex);}

            LogLib.WriteLine("Exiting EQMap.ClearMap()", LogLevel.Trace);

        }



        public void loadDummyMap(string mapname) 

        {

            LogLib.WriteLine("Entering EQMap.loadDummyMap()", LogLevel.Trace);

            OnExitMap();           

            ClearMap();

            eq.ClearMapStructures();

            mapCon.SetDistinctPens();

            eq.shortname = mapname;

            if (mapPane != null)
                mapPane.scale.Value = 100M;            

            //if (mapCon != null)
            //    mapCon.scale = 1.0f;
            //eq.NewZone = true;
            OnEnterMap();

            LogLib.WriteLine("Exiting EQMap.loadDummyMap()", LogLevel.Trace);

        }



        public bool loadMap(string filename) {

            LogLib.WriteLine("Entering EQMap.loadMap(filename='"+filename+"')", LogLevel.Trace);

            eq.mobsTimers.ResetTimers();



            OnExitMap();

            ClearMap();



            eq.ClearMapStructures();

            mapCon.SetDistinctPens();

            bool rc = eq.loadMapInternal(filename);

            if (rc)
            {

                eq.OptimizeMap();

                eq.CalculateMapLinePens(); // pre-calculate all pen colors used for map drawing.

                //eq.NewZone = true;
                OnEnterMap();
            }
            

            LogLib.WriteLine("Exiting EQMap.loadMap(), rc="+rc, LogLevel.Trace);

            return rc;

        }



        public bool loadLoYMap(string filename, bool resetmap) 

        {

            LogLib.WriteLine("Entering EQMap.loadLoYMap(filename='"+filename+"')", LogLevel.Trace);



            if (resetmap == true)

            {

                eq.mobsTimers.ResetTimers();

                OnExitMap();

                ClearMap();

                eq.ClearMapStructures();

                mapCon.SetDistinctPens();

            }

            bool rc = eq.loadLoYMapInternal(filename);

            if (rc)
            {

                eq.OptimizeMap();

                eq.CalculateMapLinePens(); // pre-calculate all pen colors used for map drawing.

                //eq.NewZone = true;
                OnEnterMap();
            }



            LogLib.WriteLine("Exiting EQMap.loadLoYMap(), rc="+rc, LogLevel.Trace);

            return rc;

        }

    }

}