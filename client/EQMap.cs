using Structures;
using System;
using System.IO;

namespace myseq
{
    // Does the map loading etc.

    public class EQMap
    {
        private MapCon mapCon;
        private MapData mapData;
        public EQData eq;

        public MobTrails trails = new MobTrails();

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

        public void SetComponents(MapCon mapCon, EQData eq, MapData mapData)
        {
            this.mapCon = mapCon;
            this.eq = eq;
            this.mapData = mapData;
        }

        public void AddMapText(MapLabel work)
        {
            mapData.Labels.Add(work);
        }

        public void DeleteMapText(MapLabel work)
        {
            mapData.Labels.Remove(work);
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
                // Remove duplicates
                mapData.RemoveDuplicateLineSegments();
                mapData.RemoveDuplicateLabels();
                eq?.CalculateMapLinePens(mapData.LineSegments, mapData.Labels); // Pre-calculate all pen colors used for map drawing.
                OnEnterMap();
            }

            return isLoaded;
        }

        internal bool LoadLoYMapInternal(string filename) // In-game EQ format
        {
            LogLib.WriteLine($"Loading Zone Map (LoY): {filename}", LogLevel.Debug);
            try
            {
                mapData.LoadMapData(filename);
            }
            catch (Exception ex)
            {
                LogLib.WriteLine($"Error reading lines from file '{filename}': {ex.Message}", LogLevel.Error);
                return false;
            }

            LogLib.WriteLine($"Loaded {filename}", LogLevel.Debug);
            return true;
        }

        private void ResetMap()
        {
            eq.MobsTimers.ResetTimers();
            OnExitMap();
            trails.Clear();
            mapData.Update();
        }

        public void LoadDummyMap()
        {
            LogLib.WriteLine($"Loading Dummy Map", LogLevel.Debug);
            OnExitMap();
            trails.Clear();
            mapData.Update();
            OnEnterMap();
        }
    }
}