using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;

using MySEQ.Structures;

namespace MySEQ
{
    public class MapPoint
    {
        public int x = 0;
        public int y = 0;
        public int z = 0;
        public MapPoint(){}
        public MapPoint(MapPoint mp)
        {
            x = mp.x;
            y = mp.y;
            z = mp.z;
        }
    }

    // A MapFileLine is an object defining how to draw a Line
    // as given by an entry in a map file.
    public class MapFileLine
    {
        public MapPoint from = new MapPoint();
        public MapPoint to = new MapPoint();
        public Pen pen;
        public MapFileLine() {}
        public MapFileLine(MapFileLine mfl)
        {
            from = new MapPoint(mfl.from);
            to = new MapPoint(mfl.to);
            pen = mfl.pen;
        }
    }

    // A MapFileLabel is an object defining how to draw a Label
    // as given by an entry in a map file.
    public class MapFileLabel
    {
        public MapPoint at = new MapPoint();
        public SolidBrush brush;
        public int fontSize = 0;
        public string text;
        public MapFileLabel() {}
        public MapFileLabel(MapFileLabel mfl)
        {
            at = new MapPoint(mfl.at);
            brush = mfl.brush;
            text = mfl.text;
            fontSize = mfl.fontSize;
        }
    }

    public enum MapFileLineType
    {
        MLT_INVALID,
        MLT_EOF,
        MLT_EMPTY,
        MLT_LINE,
        MLT_LABEL
    }

    public class MapData
    {
        public ArrayList lines = new ArrayList();
        public ArrayList labels = new ArrayList();
        public MapExtents extents = new MapExtents();
        public int emptyLines = 0;

        // MapExtents can be viewed as an imaginary box surrounding every point defined
        // in a map file. Every line and label would lie inside this box. We use it to
        // determine how much to scale (zoom) the image so the entire map shows in any
        // given window.
        public class MapExtents
        {
            // Map renderer will peek at this to see if we need should first alter
            // the rendering buffers transforms to fit the entire map into view.
            public bool newMap = false;
            
            public int minX;
            public int minY;
            public int maxX;
            public int maxY;
            
            public MapExtents()
            {
                Reset();
            }
            
            public void Reset()
            {
                minX = int.MaxValue;
                minY = int.MaxValue;
                maxX = int.MinValue;
                maxY = int.MinValue;
            }
        }
        public void Reset()
        {
            lines.Clear();
            labels.Clear();
            extents.Reset();
            emptyLines = 0;
        }
    }

    // This static class will parse a line of text and fill in a 'Working' structure
    // which can later be pulled. This keeps all the smarts inside this MapReader file.
    public static class MapReader
    {
        public static MapFileLine WorkingLine = new MapFileLine();
        public static MapFileLabel WorkingLabel = new MapFileLabel();

        public static MapFileLineType ParseLine(string line)
        {
            string [] tokens;
            int r, g, b;
            
            if (line == null)
                return MapFileLineType.MLT_EOF;

            if (line.Length == 0)
                return MapFileLineType.MLT_EMPTY;

            if (line.Length < 10)
                return MapFileLineType.MLT_INVALID;
            
            try
            {
                tokens = line.Remove(0,1).Split(',');
                switch (line[0])
                {
                    // Line parsing
                    case 'L':
                        WorkingLine.from.x = (int)float.Parse(tokens[0]);
                        WorkingLine.from.y = (int)float.Parse(tokens[1]);
                        WorkingLine.from.z = (int)float.Parse(tokens[2]);
                        WorkingLine.to.x = (int)float.Parse(tokens[3]);
                        WorkingLine.to.y = (int)float.Parse(tokens[4]);
                        WorkingLine.to.z = (int)float.Parse(tokens[5]);
                        r = int.Parse(tokens[6]);
                        g = int.Parse(tokens[7]);
                        b = int.Parse(tokens[8]);
                        WorkingLine.pen = ColorManager.FindNearestSystemPen(Color.FromArgb(r, g, b));
                        return MapFileLineType.MLT_LINE;
                        
                    // Label parsing
                    case 'P':
                        WorkingLabel.at.x = (int)float.Parse(tokens[0]);
                        WorkingLabel.at.y = (int)float.Parse(tokens[1]);
                        WorkingLabel.at.z = (int)float.Parse(tokens[2]);
                        r = int.Parse(tokens[3]);
                        g = int.Parse(tokens[4]);
                        b = int.Parse(tokens[5]);
                        WorkingLabel.brush = ColorManager.FindNearestSystemBrush(Color.FromArgb(r, g, b));
                        WorkingLabel.fontSize = int.Parse(tokens[6]);
                        WorkingLabel.text = tokens[7];
                        return MapFileLineType.MLT_LABEL;

                    // Anything else is invalid
                    default:
                        return MapFileLineType.MLT_INVALID;
                }
            }
            catch (Exception ex)
            {
                Logger.Debug("MapReader:ParseLine - " + ex.Message);
                return MapFileLineType.MLT_INVALID;
            }
        }
        
        public static void UpdateExtentsFromLine(MapData md)
        {
            md.extents.minX = Math.Min(md.extents.minX, WorkingLine.from.x);
            md.extents.minY = Math.Min(md.extents.minY, WorkingLine.from.y);
            md.extents.maxX = Math.Max(md.extents.maxX, WorkingLine.from.x);
            md.extents.maxY = Math.Max(md.extents.maxY, WorkingLine.from.y);

            md.extents.minX = Math.Min(md.extents.minX, WorkingLine.to.x);
            md.extents.minY = Math.Min(md.extents.minY, WorkingLine.to.y);
            md.extents.maxX = Math.Max(md.extents.maxX, WorkingLine.to.x);
            md.extents.maxY = Math.Max(md.extents.maxY, WorkingLine.to.y);
        }
        
        public static void UpdateExtentsFromLabel(MapData md)
        {
            md.extents.minX = Math.Min(md.extents.minX, WorkingLabel.at.x);
            md.extents.minY = Math.Min(md.extents.minY, WorkingLabel.at.y);
            md.extents.maxX = Math.Max(md.extents.maxX, WorkingLabel.at.x);
            md.extents.maxY = Math.Max(md.extents.maxY, WorkingLabel.at.y);
        }
    }
}
