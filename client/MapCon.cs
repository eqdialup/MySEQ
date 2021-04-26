// Class Files

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using myseq.Properties;
using Structures;

namespace myseq
{
    public class MapCon : UserControl
    {
        // Events
        public delegate void SelectPointHandler(Spawninfo playerinfo, double selectedX, double selectedY);

        public event SelectPointHandler SelectPoint; // Fires when the user clicks the map (without a mob)

        protected void OnSelectPoint(Spawninfo playerinfo, double selectedX, double selectedY) => SelectPoint?.Invoke(playerinfo, selectedX, selectedY);

        private readonly System.ComponentModel.Container components;

        public Label lblMobInfo;

        public Font drawFont = Settings.Default.MapLabel; //size 2
        public Font drawFont1 = new Font(Settings.Default.MapLabel.Name, Settings.Default.MapLabel.Size * 0.9f, Settings.Default.MapLabel.Style);
        public Font drawFont3 = new Font(Settings.Default.MapLabel.Name, Settings.Default.MapLabel.Size * 1.1f, Settings.Default.MapLabel.Style);

        // Hand relocation variables

        private Cursor hCurs;

        private bool m_dragging;

        private bool m_rangechange;

        private int m_dragStartX;

        private int m_dragStartY;

        private float m_dragStartPanX;

        private float m_dragStartPanY;

        private bool lookup_set;

        private Color gridColor;

        private Color gridLabelColor;

        private SolidBrush gridBrush;

        private int skittle;

        private int flash_count;

        // m_zoom - factor by which map has been zoomed.

        private float m_zoom = 1.0f;

        // m_panOffset define how far map has been dragged.

        public float m_panOffsetX;
        //public PointF panOffset;
        public float m_panOffsetY;
        //public float SetpanOffsety(float offset) => panOffset.Y = offset;
        //public float SetpanOffsetx(float offset) => panOffset.X = offset;
        // m_ratio - adjustment factor required to convert map->screen size.

        public float m_ratio = 1.0f;

        // m_mapCenter - centre point of screen in Map Units.
        private PointF mapCenter;

        //private float m_mapCenterX;
        //private float m_mapCenterY;

        // m_screenCenter - centre point of screen in Screen Units.
        private PointF screenCenter;
        //private float m_screenCenterX;
        //private float m_screenCenterY;

        //        private float x_adjust;
        private PointF adjustment;
        //        private float y_adjust;
        private PointF gamerPos;
        // Spawn Sizes
        private int SettingsSpawnSize = 3;

        private float SpawnSize = 5.0f;

        private float SpawnSizeOffset = 2.5f;

        private float SpawnPlusSize = 7.0f;

        private float PlusSzOZ = 3.5f;

        private float SelectSize = 9.0f;

        private float SelectSizeOffset = 4.5f;

        private float LookupRingSize = 7.0f;

        private float LookupRingOffset = 3.5f;

        public float scale = 1.0f;

        public int filterpos;

        public int filterneg;

        //private float selectedX = -1;          
        //private float selectedY = -1;          
        private PointF selectedPoint = new PointF(-1, -1);// [42!] Mark an arbitrary spot on the map
        // Don't set this directly, but use SetSelectedPoint/ClearSelectedPoint

        private string curTarget = "";

        private BufferedGraphicsContext gfxManager;

        private BufferedGraphics bkgBuffer;

        private ToolTip toolTip;

        public bool flash; // used for flashing warning lights

        private SolidBrush textBrush;

        private readonly Pen PCBorder = new Pen(new SolidBrush(Settings.Default.PCBorderColor));

        private MainForm f1;          // Caution: this may be null

        private MapPane mapPane;     // Caution: this may be null

        private EQData eq;
        private EQMap map;

        private SpawnColors con;

        private DateTime LastTTtime;

        private int fpsCount;

        private DateTime fpsLastReadTime = new DateTime();

        private TableLayoutPanel tableLayoutPanel1;

        public Label lblGameClock;

        private bool ShowPCName = Settings.Default.ShowPCNames;

        public double FpsValue;

        private float[] xSin = new float[512];

        private float[] xCos = new float[512];

        public int UpdateSteps { get; set; } = 5;

        public int UpdateTicks { get; set; } = 1;

        public MapCon()
        {
            InitializeComponent();
            InitializeVariables();
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            gfxManager = BufferedGraphicsManager.Current;
        }

        public void SetComponents(MainForm f1, MapPane mapPane, EQData eq, EQMap map)
        {
            this.f1 = f1;
            this.mapPane = mapPane;
            this.eq = eq;
            con = eq.spawnColor;
            this.map = map;
            map.EnterMap += new EQMap.EnterMapHandler(MapChanged);
            Invalidate();
        }

        public void OnResize()
        {
            if (Width > 0 && Height > 0)
            {
                bkgBuffer?.Dispose();

                gfxManager.MaximumBuffer = new Size(Width + 1, Height + 1);

                bkgBuffer = gfxManager.Allocate(CreateGraphics(), new Rectangle(0, 0, Width + 1, Height + 1));

                bkgBuffer.Graphics.SmoothingMode = SmoothingMode.None;

                Refresh();
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnPaint(PaintEventArgs e)

        {
            if (bkgBuffer != null)
            {
                bkgBuffer.Render(e.Graphics);

                base.OnPaint(e);

                CalculateFPS();
                f1.toolStripFPS.Text = $"FPS: {FpsValue}";
            }
        }

        private void CalculateFPS()
        {
            double fpsTimeDelta;
            // Calculate FPS

            if ((fpsTimeDelta = (DateTime.Now - fpsLastReadTime).Seconds) > 0.5)

            {
                FpsValue = Math.Round(fpsCount / fpsTimeDelta, 2);

                fpsLastReadTime = DateTime.Now;

                fpsCount = 0;
            }
            else
            {
                fpsCount++;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()

        {
            this.lblMobInfo = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblGameClock = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMobInfo
            // 
            this.lblMobInfo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom
            | System.Windows.Forms.AnchorStyles.Left
            | System.Windows.Forms.AnchorStyles.Right;
            this.lblMobInfo.BackColor = System.Drawing.Color.White;
            this.lblMobInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMobInfo.Font = Settings.Default.TargetInfoFont;
            this.lblMobInfo.Location = new System.Drawing.Point(0, 20);
            this.lblMobInfo.Margin = new System.Windows.Forms.Padding(0);
            this.lblMobInfo.Name = "lblMobInfo";
            this.lblMobInfo.Size = new System.Drawing.Size(163, 80);
            this.lblMobInfo.TabIndex = 0;
            this.lblMobInfo.Text = "Spawn Information Window";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.lblGameClock, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblMobInfo, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(163, 100);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // lblGameClock
            // 
            this.lblGameClock.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom
            | System.Windows.Forms.AnchorStyles.Left
            | System.Windows.Forms.AnchorStyles.Right;
            this.lblGameClock.BackColor = System.Drawing.Color.BlueViolet;
            this.lblGameClock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblGameClock.Font = new Font(Settings.Default.TargetInfoFont, FontStyle.Bold);
            this.lblGameClock.ForeColor = System.Drawing.Color.White;
            this.lblGameClock.Location = new System.Drawing.Point(0, 0);
            this.lblGameClock.Margin = new System.Windows.Forms.Padding(0);
            this.lblGameClock.Name = "lblGameClock";
            this.lblGameClock.Size = new System.Drawing.Size(163, 20);
            this.lblGameClock.TabIndex = 2;
            this.lblGameClock.Text = "12:12 AM 1/01/0000";
            this.lblGameClock.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MapCon
            // 
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Location = new System.Drawing.Point(3, 3);
            this.Name = "MapCon";
            this.Size = new System.Drawing.Size(224, 147);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MapCon_Paint);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MapCon_KeyPress);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.MapCon_MouseDoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MapCon_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MapCon_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MapCon_MouseUp);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.MapCon_MouseScroll);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void MapCon_KeyPress(object sender, KeyPressEventArgs e) => mapPane.MapCon_KeyPress(sender, e);

        #endregion Component Designer generated code

        private void InitializeVariables()
        {
            hCurs = Cursors.Hand;//new Cursor("Hand.cur");

            // Initialize DragVariables to 0,0 and set semiphore false
            m_dragging = false;
            m_rangechange = false;
            m_dragStartX = m_dragStartY = 0;
            m_panOffsetX = m_panOffsetY = 0;
            ClearSelectedPoint();

            toolTip = new ToolTip
            {
                AutomaticDelay = 500
            };
            toolTip.SetToolTip(this, "ABCD\nEFGH");
            toolTip.Active = true;

            // Set sine and cosine values to use with headings
            for (var p = 0; p < 512; p++)
            {
                xCos[p] = (float)Math.Cos(p / 512.0f * 2.0f * Math.PI);
                xSin[p] = (float)Math.Sin(p / 512.0f * 2.0f * Math.PI);
            }

            textBrush = new SolidBrush(Color.LightGray);
        }

        public void ClearSelectedPoint()

        {
            SetSelectedPoint(-1, -1);
        }

        private void SetSelectedPoint(float x, float y)
        {
            selectedPoint.X = x;
            selectedPoint.Y = y;

            if (eq != null)
            {
                SelectPoint?.Invoke(eq.gamerInfo, x, y);
            }
        }

        private void MapCon_MouseScroll(object sender, MouseEventArgs me)
        {
            if (mapPane == null)
            {
                return;
            }

            var newScale = scale + (me.Delta / 600.0f);

            if (newScale >= 0.1)
            {
                f1.mapPane.scale.Value = (decimal)(newScale * 100);
            }

            ReAdjust();

            Invalidate();
        }

        private void MapCon_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Range Circle Checks
                if (Settings.Default.RangeCircle > 0)
                {
                    float rCircleRadius = Settings.Default.RangeCircle;

                    var upperRadius = rCircleRadius + (4 * SpawnSize);

                    var lowerRadius = rCircleRadius - (4 * SpawnSize);

                    if (lowerRadius < 0)
                    {
                        lowerRadius = 0;
                    }

                    // Calc the proper loc for the mouse

                    MouseMapLoc(e, out var mousex, out var mousey);

                    // if within approximately one mob radius of the Range Circle
                    // then we are resizing range circle, and not dragging.

                    var sd = MouseDistance(mousex, mousey);

                    if (Settings.Default.ColorRangeCircle && (sd > lowerRadius) && (sd < upperRadius))
                    {
                        // changing range cirlce size

                        m_rangechange = true;
                    }
                }

                if (!m_rangechange)

                {
                    Cursor.Current = hCurs;

                    // Remember the mouse down loc and set semiphore

                    m_dragging = true;

                    m_dragStartX = e.X;

                    m_dragStartY = e.Y;

                    // remember the original PanOffsets...

                    m_dragStartPanX = m_panOffsetX;

                    m_dragStartPanY = m_panOffsetY;
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                // right click of mouse.
                // see if there is a mob located there.
                RightMouseButton(e);
            }
        }

        private float MouseDistance(float mousex, float mousey)
        {
            return (float)Math.Sqrt(((mousey - eq.gamerInfo.Y) * (mousey - eq.gamerInfo.Y)) +

                ((mousex - eq.gamerInfo.X) * (mousex - eq.gamerInfo.X)));
        }

        private void MouseMapLoc(MouseEventArgs e, out float mousex, out float mousey)
        {
            mousex = mapCenter.X + ((m_panOffsetX + screenCenter.X - e.X) / m_ratio);
            mousey = mapCenter.Y + ((m_panOffsetY + screenCenter.Y - e.Y) / m_ratio);
        }

        private void RightMouseButton(MouseEventArgs e)
        {
            MouseMapLoc(e, out var mousex, out var mousey);

            float delta = 5.0f / m_ratio;
            Spawninfo sp = eq.FindMobNoPetNoPlayerNoCorpse(mousex, mousey, delta);

            if (sp?.Name.Length > 0)
            {
                f1.alertAddmobname = RegexHelper.FilterMobName(sp.Name);
                f1.alertAddmobname = f1.alertAddmobname.Replace("_", " ").TrimEnd(' ');
                f1.alertX = sp.X;
                f1.alertY = sp.Y;
                f1.alertZ = sp.Z;
            }
            else
            {
                GroundItem gi = eq.FindGroundItem(mousex, mousey, delta);
                if (gi?.Name.Length > 0)
                {
                    f1.alertAddmobname = gi.GetItemDescription(gi.Name, eq);
                    f1.alertX = gi.X;
                    f1.alertY = gi.Y;
                    f1.alertZ = gi.Z;
                    //                        emailmenu = false;
                }
                else
                {
                    f1.alertAddmobname = "";
                    Spawntimer st = eq.FindTimer(mousex, mousey, 5.0f);
                    if (st != null)
                    {
                        foreach (var name in st.AllNames.Split(','))
                        {
                            var bname = RegexHelper.TrimName(name);
                            if (bname.Length > 0 && f1.alertAddmobname.Length == 0)
                            {
                                f1.alertAddmobname = bname;
                            }

                            if (RegexHelper.RegexMatch(bname))
                            {
                                f1.alertAddmobname = bname;
                                break;
                            }
                        }

                        if (f1.alertAddmobname.Length > 0)
                        {
                            f1.alertX = st.X;
                            f1.alertY = st.Y;
                            f1.alertZ = st.Z;
                        }
                    }
                    else
                    {
                        sp = eq.FindMobNoPetNoPlayer(mousex, mousey, delta);

                        if (sp?.Name.Length > 0)
                        {
                            f1.alertAddmobname = RegexHelper.FilterMobName(sp.Name);
                            f1.alertAddmobname = RegexHelper.FixMobName(f1.alertAddmobname);
                            f1.alertX = sp.X;
                            f1.alertY = sp.Y;
                            f1.alertZ = sp.Z;
                        }
                        else
                        {
                            f1.alertAddmobname = "";
                            f1.alertX = 0.0f;
                            f1.alertY = 0.0f;
                            f1.alertZ = 0.0f;
                        }
                    }
                }
            }

            f1.SetContextMenu();
        }

        private void MapCon_MouseUp(object sender, MouseEventArgs e)

        {
            MouseMapLoc(e, out var mousex, out var mousey);

            if ((ModifierKeys & Keys.Control) == Keys.Control)
            {
                eq.ModKeyControl(this, mousex, mousey);
            }
            else if ((ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                // [42!] Mark an arbitrary spot on the map, or turn it off if a spot was marked already.

                if (selectedPoint.X == -1)
                {
                    SetSelectedPoint(mousex, mousey);
                }
                else
                {
                    ClearSelectedPoint();
                }
            }
            else if (e.X == m_dragStartX && e.Y == m_dragStartY)
            {
                // allow a small margin of error in coordinates
                // value of 5 screen units in terms of mapcoords
                // try to select mob, if not then do timer

                var delta = 5.0f / m_ratio;
                if (!eq.SelectMob(mousex, mousey, delta) && !eq.SelectTimer(mousex, mousey, delta))
                {
                    eq.SelectGroundItem(mousex, mousey, delta);
                }

                Invalidate();
            }

            m_dragging = false;

            m_rangechange = false;

            m_dragStartX = m_dragStartY = 0;

            Cursor.Current = Cursors.Default;
        }

        private void MapCon_MouseMove(object sender, MouseEventArgs e)
        {
            if (mapPane != null && f1 != null)
            {
                // Limit TT popups to four times a sec
                if (DateTime.Now.Subtract(LastTTtime).TotalSeconds < 0.25)
                {
                    return;
                }

                // Calc the proper loc for the mouse
                MouseMapLoc(e, out var mousex, out var mousey);

                // Range
                var sd = MouseDistance(mousex, mousey);

                f1.toolStripMouseLocation.Text = $"Map /loc: {mousey:f2}, {mousex:f2}";
                f1.toolStripDistance.Text = $"Distance: {sd:f1}";

                // If we are dragging, then change the origin.

                if (m_dragging)
                {
                    // Compute delta x,y from original click
                    var dx = m_dragStartX - e.X;
                    var dy = m_dragStartY - e.Y;
                    mapPane.offsetx.Value = -(decimal)(m_dragStartPanX - dx);
                    mapPane.offsety.Value = -(decimal)(m_dragStartPanY - dy);
                    ReAdjust();
                    Invalidate();
                }
                else
                {
                    PopulateToolTip(e);
                }
            }
        }

        private void PopulateToolTip(MouseEventArgs e)
        {
            MouseMapLoc(e, out var mousex, out var mousey);
            var delta = 5.0f / m_ratio;

            Spawninfo sp = eq.FindMobNoPet(mousex, mousey, delta) ?? eq.FindMob(mousex, mousey, delta);

            bool found;
            if (sp == null)
            {
                found = false;
            }
            else
            {
                found = true;
                LastTTtime = DateTime.Now;
                toolTip.SetToolTip(this, MobInfo(sp, false, false));
                toolTip.AutomaticDelay = 0;
                toolTip.Active = true;
            }

            if (!found)
            {
                GroundItem gi = eq.FindGroundItem(mousex, mousey, delta);

                found = ToolTipGroundItem(found, gi);
            }

            if (!found)
            {
                Spawntimer st = eq.mobsTimers.Find(delta, mousex, mousey);
                found = ToolTipSpawnTimer(st, found);
            }

            if (!found)
            {
                toolTip.SetToolTip(this, "");
            }
        }

        private bool ToolTipGroundItem(bool found, GroundItem gi)
        {
            if (gi != null)

            {
                var ItemName = gi.Name;

                foreach (ListItem listItem in eq.GroundSpawn)
                {
                    if (gi.Name == listItem.ActorDef)
                    {
                        ItemName = listItem.Name;
                    }
                }

                var s = $"Name: {ItemName}\n{gi.Name}";

                toolTip.SetToolTip(this, s);

                toolTip.AutomaticDelay = 0;

                toolTip.Active = true;

                LastTTtime = DateTime.Now;

                found = true;
            }

            return found;
        }

        private bool ToolTipSpawnTimer(Spawntimer st, bool found)
        {
            if (st != null)
            {
                var description = st.GetDescription();
                if (description != null)
                {
                    toolTip.SetToolTip(this, description);
                    toolTip.AutomaticDelay = 0;
                    toolTip.Active = true;
                }
                LastTTtime = DateTime.Now;
                found = true;
            }

            return found;
        }

        public void ClearPan()
        {
            //panOffset.X = 0;
            //panOffset.Y = 0;
            m_panOffsetX = 0;
            m_panOffsetY = 0;
            ReAdjust();
        }

        public void ReAdjust()
        {
            var mapWidth = Math.Abs(eq.maxx - eq.minx);

            var mapHeight = Math.Abs(eq.maxy - eq.miny);

            var ScreenWidth = Width - 30;

            var ScreenHeight = Height - 30;

            screenCenter.X = Width / 2;

            screenCenter.Y = Height / 2;

            var zoom = scale;

            if (m_zoom > 32)
            {
                m_zoom = 32;
            }

            var xratio = ScreenWidth / mapWidth;

            var yratio = ScreenHeight / mapHeight;

            // Use the smaller scale ratio so that the map fits in the screen at a zoom of 1.

            m_ratio = xratio < yratio ? xratio * zoom : yratio * zoom;

            // Calculate the Map Center
            if (Settings.Default.FollowOption == FollowOption.None)
            {
                mapCenter.X = eq.minx + (mapWidth / 2);
                mapCenter.Y = eq.miny + (mapHeight / 2);
                //                m_mapCenterX = eq.minx + (mapWidth / 2);
                //                m_mapCenterY = eq.miny + (mapHeight / 2);
            }
            else if (Settings.Default.FollowOption == FollowOption.Player)
            {
                //                m_mapCenterX = eq.gamerInfo.X;
                //                m_mapCenterY = eq.gamerInfo.Y;
                mapCenter.X = eq.gamerInfo.X;
                mapCenter.Y = eq.gamerInfo.Y;
            }
            else if (Settings.Default.FollowOption == FollowOption.Target)
            {
                Spawninfo siTarget = eq.GetSelectedMob();

                if (siTarget != null)
                {
                    mapCenter.X = siTarget.X;
                    mapCenter.Y = siTarget.Y;
                    //                    m_mapCenterX = siTarget.X;
                    //                    m_mapCenterY = siTarget.Y;
                }
            }

            // When Following a player or spawn and KeepCentered is not selected

            // adjust the map center so as to minimise the amount of blank space in the map window.

            if (!Settings.Default.KeepCentered && Settings.Default.FollowOption != FollowOption.None)

            {
                // Calculate the MapCordinates of the Screen Edges

                float ScreenMaxY, ScreenMinY, ScreenMinX, ScreenMaxX;

                float ScreenMapWidth, ScreenMapHeight;

                ScreenMaxY = ScreenToMapCoordY(15);

                ScreenMinY = ScreenToMapCoordY(Height - (float)15);

                ScreenMapHeight = Math.Abs(ScreenMaxY - ScreenMinY);

                // X sense is wrong way round...

                ScreenMinX = ScreenToMapCoordX(Width - (float)15);

                ScreenMaxX = ScreenToMapCoordX(15);

                ScreenMapWidth = Math.Abs(ScreenMaxX - ScreenMinX);

                if (mapWidth <= ScreenMapWidth)
                {
                    // If map fits in window set center to center of map

                    //                    m_mapCenterX = eq.minx + (mapWidth / 2);
                    mapCenter.X = eq.minx + (mapWidth / 2);
                }
                else
                {
                    // if we have blank space to the left or right repostion the center point appropriately
                    ReposCenter(ScreenMinX, ScreenMaxX);
                }

                if (mapHeight <= ScreenMapHeight)

                {
                    // If map fits in window set center to center of map

                    //                    m_mapCenterY = eq.miny + (mapHeight / 2);
                    mapCenter.Y = eq.miny + (mapHeight / 2);
                }
                else
                {
                    // if we have blank space at the top or botton repostion the center point appropriately
                    ReposCenter(ScreenMinX, ScreenMaxX);
                }
                LogLib.WriteLine("Readjust Done");
            }
            adjustment.X = m_panOffsetX + screenCenter.X + (float)(mapCenter.X * m_ratio);
            adjustment.Y = m_panOffsetY + screenCenter.Y + (float)(mapCenter.Y * m_ratio);
        }

        private void ReposCenter(float ScreenMinX, float ScreenMaxX)
        {
            if (ScreenMinX < eq.minx)
            {
                //                m_mapCenterX += eq.minx - ScreenMinX;
                mapCenter.X += eq.minx - ScreenMinX;
            }
            else if (ScreenMaxX > eq.maxx)
            {
                //                m_mapCenterX -= ScreenMaxX - eq.maxx;
                mapCenter.X -= ScreenMaxX - eq.maxx;
            }
        }

        public float CalcScreenCoordX(float mapCoordinateX) => adjustment.X - (float)(mapCoordinateX * m_ratio);

        // Formula Should be
        // Screen X =CenterScreenX + ((mapCoordinateX - MapCenterX) * m_ratio)

        // However Eq's Map coordinates are in the oposite sense to the screen
        // so we have to multiply the second portion by -1, which is the same
        // as changing the plus to a minus...

        //m_ratio = (ScreenWidth/MapWidth) * zoom (Calculated ahead of time in ReAdjust)

        //return m_panOffsetX + m_screenCenterX - ((mapCoordinateX - m_mapCenterX) * m_ratio);
        public float CalcScreenCoordY(float mapCoordinateY) => adjustment.Y - (float)(mapCoordinateY * m_ratio);

        private float ScreenToMapCoordX(float screenCoordX) => mapCenter.X + ((screenCenter.X - screenCoordX) / m_ratio);

        private float ScreenToMapCoordY(float screenCoordY) => mapCenter.Y + ((screenCenter.Y - screenCoordY) / m_ratio);

        private void DrawCross(Pen pen, PointF drawPoint, float offset)
        {
            PointF startpos1 = new PointF(drawPoint.X - offset, drawPoint.Y);
            PointF endpos1 = new PointF(drawPoint.X + offset, drawPoint.Y);
            PointF startpos2 = new PointF(drawPoint.X, drawPoint.Y - offset);
            PointF endpos2 = new PointF(drawPoint.X, drawPoint.Y + offset);
            bkgBuffer.Graphics.DrawLine(pen, startpos1, endpos1);
            bkgBuffer.Graphics.DrawLine(pen, startpos2, endpos2);
        }
        private void DrawLine(Pen pen, float x1, float y1, float x2, float y2)
        {
            try
            {
                bkgBuffer.Graphics.DrawLine(pen, x1, y1, x2, y2);
            }
            catch (Exception ex) { LogLib.WriteLine($"Error with DrawLine({x1}, {y1}, {x2}, {y2}): ", ex); }
        }

        private void DrawLines(Pen pen, PointF[] points)
        {
            try
            {
                bkgBuffer.Graphics.DrawLines(pen, points);
            }
            catch (Exception ex) { LogLib.WriteLine("Error with DrawLines: ", ex); }
        }

        private void FillEllipse(Brush brush, float x1, float y1, float width, float height)
        {
            try
            {
                bkgBuffer.Graphics.FillEllipse(brush, x1, y1, width, height);
            }
            catch (Exception ex) { LogLib.WriteLine($"Error with FillEllipse({x1}, {y1}, {width}, {height}): ", ex); }
        }

        private void DrawEllipse(Pen pen, float x1, float y1, float width, float height)
        {
            try
            {
                bkgBuffer.Graphics.DrawEllipse(pen, x1, y1, width, height);
            }
            catch (Exception ex) { LogLib.WriteLine($"Error with DrawEllipse({x1}, {y1}, {width}, {height}): ", ex); }
        }

        private void DrawTriangle(Pen pen, float x1, float y1, float radius)
        {
            PointF[] points = TrianglePoints(x1, y1, radius);
            try
            {
                bkgBuffer.Graphics.DrawLines(pen, points);
            }
            catch (Exception ex) { LogLib.WriteLine($"Error with DrawTriangle({x1}, {y1}, {radius}): ", ex); }
        }

        private void FillTriangle(Brush brush, float x1, float y1, float radius)
        {
            PointF[] points = TrianglePoints(x1, y1, radius);

            try
            {
                bkgBuffer.Graphics.FillPolygon(brush, points);
            }
            catch (Exception ex) { LogLib.WriteLine($"Error with FillTriangle({x1}, {y1}, {radius}): ", ex); }
        }

        private static PointF[] TrianglePoints(float x1, float y1, float radius)
            => new PointF[]{
                new PointF(x1 + (radius * 0.866025f), y1 + (radius * 0.5f)),
                new PointF(x1 + (radius * -0.866025f), y1 + (radius * 0.5f)),
                new PointF(x1, y1 - radius)
            };

        private void FillRectangle(Brush brush, float x1, float y1, float width, float height)
        {
            //if (x1 != x1 || y1 != y1 || width != width || height != height) return;

            try { bkgBuffer.Graphics.FillRectangle(brush, x1, y1, width, height); }
            catch (Exception ex) { LogLib.WriteLine($"Error with FillRectangle({x1}, {y1}, {width}, {height}): ", ex); }
        }

        private void DrawSpawnNames(Brush dBrush, string tName, float x1, float y1)//, string gName)
        {
            var xoffset = bkgBuffer.Graphics.MeasureString(tName, drawFont).Width * 0.5f;
            //            float goffset = bkgBuffer.Graphics.MeasureString(gName, drawFont).Width * 0.5f;

            try
            {
                bkgBuffer.Graphics.DrawString(tName, drawFont, dBrush, CalcScreenCoordX(x1) - xoffset, CalcScreenCoordY(y1) - SpawnSize - drawFont.GetHeight());
                //if (gName != "") bkgBuffer.Graphics.DrawString(gName, drawFont, dBrush, CalcScreenCoordX(x1) - goffset, CalcScreenCoordY(y1) - SpawnSize - drawFont.GetHeight());
            }
            catch (Exception ex) { LogLib.WriteLine($"Error with DrawSpawnNames({tName}, {x1}, {y1}): ", ex); }
        }

        private void DrawRectangle(Pen pen, float x1, float y1, float width, float height)
        {
            //if (x1 != x1 || y1 != y1 || width != width || height != height) return;

            try
            {
                if (pen is null) return;
                else
                    bkgBuffer.Graphics.DrawRectangle(pen, x1, y1, width, height);
            }
            catch (Exception ex) { LogLib.WriteLine($"Error with DrawRectangle({x1}, {y1}, {width}, {height}): ", ex); }
        }

        private string MobInfo(Spawninfo si, bool SetColor, bool ChangeSize)
        {
            try
            {
                if (f1 == null) { return ""; }

                Graphics graphics;

                if (si == null)

                {
                    if (ChangeSize)
                    {
                        tableLayoutPanel1.RowStyles[0].SizeType = SizeType.Absolute;

                        tableLayoutPanel1.RowStyles[1].SizeType = SizeType.Absolute;

                        if (Settings.Default.ShowTargetInfo)
                        {
                            graphics = lblMobInfo.CreateGraphics();

                            SizeF gt = new SizeF();
                            SizeF gf = new SizeF();

                            gt = graphics.MeasureString(lblGameClock.Text, lblGameClock.Font);

                            gf = graphics.MeasureString("Spawn Information Window", lblMobInfo.Font);

                            graphics.Dispose();

                            tableLayoutPanel1.ColumnStyles[0].SizeType = SizeType.Absolute;

                            if (gt.Width > gf.Width)
                            {
                                tableLayoutPanel1.Width = (int)gt.Width + 40;
                                tableLayoutPanel1.ColumnStyles[0].Width = (int)gt.Width + 40;
                            }
                            else
                            {
                                tableLayoutPanel1.Width = (int)gf.Width + 40;
                                tableLayoutPanel1.ColumnStyles[0].Width = (int)gf.Width + 40;
                            }

                            tableLayoutPanel1.RowStyles[0].Height = (int)gt.Height + 7;
                        }
                        else
                        {
                            tableLayoutPanel1.RowStyles[0].Height = 0;

                            tableLayoutPanel1.RowStyles[1].Height = 0;
                        }
                    }

                    lblMobInfo.BackColor = Color.White;

                    return "Spawn Information Window";
                }

                StringBuilder mobInfo = SpawnInfoWindow(si);

                if (SetColor)
                {
                    InfoSetColor(si);
                }

                graphics = lblMobInfo.CreateGraphics();

                SizeF sf = new SizeF();
                SizeF sc = new SizeF();

                sf = graphics.MeasureString(mobInfo.ToString(), lblMobInfo.Font);

                sc = graphics.MeasureString(lblGameClock.Text, lblGameClock.Font);

                graphics.Dispose();

                tableLayoutPanel1.ColumnStyles[0].SizeType = SizeType.Absolute;
                return MobshowInfo(ChangeSize, mobInfo, ref sf, ref sc);
            }
            catch (Exception ex)
            {
                LogLib.WriteLine("Error with MobInfo(): ", ex);
                return "";
            }
        }

        private string MobshowInfo(bool ChangeSize, StringBuilder mobInfo, ref SizeF sf, ref SizeF sc)
        {
            tableLayoutPanel1.RowStyles[0].SizeType = SizeType.Absolute;
            tableLayoutPanel1.RowStyles[1].SizeType = SizeType.Absolute;
            if (Settings.Default.ShowTargetInfo)
            {
                lblMobInfo.Visible = true;

                if (ChangeSize)
                {
                    var panel_width = sc.Width > sf.Width ? (int)sc.Width : (int)sf.Width;
                    tableLayoutPanel1.Width = panel_width + (Settings.Default.SmallTargetInfo ? 40 : 10);
                    tableLayoutPanel1.ColumnStyles[0].Width = panel_width + (Settings.Default.SmallTargetInfo ? 40 : 10);
                    tableLayoutPanel1.RowStyles[0].Height = (int)sc.Height + 7;
                    tableLayoutPanel1.RowStyles[1].Height = (int)sf.Height + (Settings.Default.SmallTargetInfo ? 11 : 17);
                }
            }
            else if (ChangeSize)
            {
                tableLayoutPanel1.Width = (int)sc.Width + 10;
                tableLayoutPanel1.ColumnStyles[0].Width = (int)sc.Width + 10;
                tableLayoutPanel1.RowStyles[0].Height = 0;
                tableLayoutPanel1.RowStyles[1].Height = 0;
            }

            return mobInfo.ToString();
        }

        private void InfoSetColor(Spawninfo si)
        {
            if (si.Level < (con.GreyRange + eq.gamerInfo.Level))
            {
                lblMobInfo.BackColor = Color.LightGray;
            }
            else if (si.Level < (con.GreenRange + eq.gamerInfo.Level))
            {
                lblMobInfo.BackColor = Color.PaleGreen;
            }
            else if (si.Level < (con.CyanRange + eq.gamerInfo.Level))
            {
                lblMobInfo.BackColor = Color.PowderBlue;
            }
            else if (si.Level < eq.gamerInfo.Level)
            {
                lblMobInfo.BackColor = Color.DeepSkyBlue;
            }
            else if (si.Level == eq.gamerInfo.Level)
            {
                lblMobInfo.BackColor = Color.White;
            }
            else
            {
                lblMobInfo.BackColor = si.Level <= eq.gamerInfo.Level + con.YellowRange ? Color.Yellow : Color.Red;
            }

            if (si.isEventController)
            {
                lblMobInfo.BackColor = Color.Violet;
            }

            if (si.isLDONObject)
            {
                lblMobInfo.BackColor = Color.LightGray;
            }
        }

        private StringBuilder SpawnInfoWindow(Spawninfo si)
        {
            StringBuilder mobInfo = new StringBuilder();

            if (Settings.Default.SmallTargetInfo)
            {
                SmallWindow(si, mobInfo);
            }
            else
            {
                // long target window version
                LargeWindow(si, mobInfo);
            }

            return mobInfo;
        }

        private void LargeWindow(Spawninfo si, StringBuilder mobInfo)
        {
            mobInfo.AppendFormat("Name: {0} ({1})\n", si.Name, si.SpawnID);

            if (si.isMerc)
            {
                mobInfo.AppendFormat("Level: {0} (Mercenary)\n", si.Level.ToString());
            }
            else if (si.isPet)
            {
                mobInfo.AppendFormat("Level: {0} (Pet)\n", si.Level.ToString());
            }
            else if (si.isFamiliar)
            {
                mobInfo.AppendFormat("Level: {0} (Familiar)\n", si.Level.ToString());
            }
            else if (si.isMount)
            {
                mobInfo.AppendFormat("Level: {0} (Mount)\n", si.Level.ToString());
            }
            else
            {
                mobInfo.AppendFormat("Level: {0}\n", si.Level.ToString());
            }

            if (si.Primary > 0)
            {
                mobInfo.AppendFormat("Class: {0}    Primary: {1} ({2})\n", eq.GetClass(si.Class), si.PrimaryName, si.Primary);
            }
            else
            {
                mobInfo.AppendFormat("Class: {0}\n", eq.GetClass(si.Class));
            }

            if (si.Offhand > 0)
            {
                mobInfo.AppendFormat("Race: {0}    Offhand: {1} ({2})\n", eq.GetRace(si.Race), si.OffhandName, si.Offhand);
            }
            else
            {
                mobInfo.AppendFormat("Race: {0}\n", eq.GetRace(si.Race));
            }

            mobInfo.AppendFormat("Speed: {0:f3}\n", si.SpeedRun);

            mobInfo.AppendFormat("Visibility: {0}\n", PrettyNames.GetHideStatus(si.Hide));

            mobInfo.AppendFormat("Distance: {0:f3}\n", SpawnDistance(si));

            mobInfo.AppendFormat("Y: {0:f3}  X: {1:f3}  Z: {2:f3}", si.Y, si.X, si.Z);
        }

        private void SmallWindow(Spawninfo si, StringBuilder mobInfo)
        {
            // small target window version
            if (si.isMerc)
            {
                mobInfo.AppendFormat("Mercenary: {0}\n", si.Name);
            }
            else if (si.isPet)
            {
                mobInfo.AppendFormat("Pet: {0}\n", si.Name);
            }
            else if (si.isFamiliar)
            {
                mobInfo.AppendFormat("Familiar: {0}\n", si.Name);
            }
            else if (si.isMount)
            {
                mobInfo.AppendFormat("Mount: {0}\n", si.Name);
            }
            else if (si.IsPlayer)
            {
                mobInfo.AppendFormat("Player: {0}\n", si.Name);
            }
            else if (si.isCorpse)
            {
                mobInfo.AppendFormat("Corpse: {0}\n", si.Name);
            }
            else
            {
                mobInfo.AppendFormat("NPC: {0}\n", si.Name);
            }

            mobInfo.AppendFormat("Level {0} / {1}\n", si.Level.ToString(), PrettyNames.GetHideStatus(si.Hide));

            mobInfo.AppendFormat("{0} / {1}\n", eq.GetRace(si.Race), eq.GetClass(si.Class));

            mobInfo.AppendFormat("Speed: {0:f3}  Dist: {1:f0}\n", si.SpeedRun, SpawnDistance(si));

            mobInfo.AppendFormat("Y: {0:f1} X: {1:f1} Z: {2:f1}", si.Y, si.X, si.Z);
        }

        private float SpawnDistance(Spawninfo si)
        {
            return (float)Math.Sqrt(((si.X - eq.gamerInfo.X) * (si.X - eq.gamerInfo.X)) +

                                ((si.Y - eq.gamerInfo.Y) * (si.Y - eq.gamerInfo.Y)) +

                                ((si.Z - eq.gamerInfo.Z) * (si.Z - eq.gamerInfo.Z)));
        }

        private string TimerInfo(Spawntimer st)
        {
            int height_adder = 20;
            if (f1 == null) { return ""; }

            int countTime = 0;

            string countTimer = "";

            GetSpawnTimeDiff(st, ref countTime, ref countTimer);

            string Timerinfo;
            if (countTime > 0)
            {
                Timerinfo = AppendSpawnInfo(st, countTimer, ref height_adder);
            }
            else if (st.SpawnTimer > 0)
            {
                Timerinfo = AppendSpawnInfo(st, "", ref height_adder);
            }
            else
            {
                Timerinfo = AppendSpawnInfo(st, "", ref height_adder);
            }

            lblMobInfo.BackColor = Color.White;

            Graphics g = lblMobInfo.CreateGraphics();

            SizeF sf = g.MeasureString(Timerinfo, lblMobInfo.Font);

            Graphics gt = lblGameClock.CreateGraphics();

            SizeF sc = gt.MeasureString(lblGameClock.Text, lblGameClock.Font);

            g.Dispose();

            gt.Dispose();

            sf.ToPointF();

            sc.ToPointF();

            TableLayout(height_adder, ref sf, ref sc);

            return Timerinfo;
        }

        private static string AppendSpawnInfo(Spawntimer st, string countTimer, ref int height_adder)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("Spawn Name: {0}\n", st.LastSpawnName);

            string names_to_add = "Names encountered: ";
            string[] names = st.AllNames.Split(',');

            int namecount = 0;

            foreach (string name in names)
            {
                var namet = RegexHelper.TrimName(name);

                if (namecount == 0)
                {
                    names_to_add += namet;
                }
                else
                {
                    if ((namet.Length + names_to_add.Length + 2) < 45)
                    {
                        names_to_add += ", ";
                        names_to_add += namet;
                    }
                    else
                    {
                        stringBuilder.Append(names_to_add);
                        stringBuilder.Append("\n");
                        height_adder++;
                        height_adder++;

                        names_to_add = namet;
                    }
                }

                namecount++;
            }
            if (names_to_add.Length > 0)
            {
                stringBuilder.Append(names_to_add);
            }

            return TimerFormat(st, countTimer, stringBuilder);
        }

        private static string TimerFormat(Spawntimer st, string countTimer, StringBuilder spawnTimer)
        {
            spawnTimer.Append("\n")
                .AppendFormat("Last Spawned At: {0}\n", st.SpawnTimeStr)
                .AppendFormat("Last Killed At: {0}\n", st.KillTimeStr)
                .AppendFormat("Next Spawn At: {0}\n", st.NextSpawnStr)
                .AppendFormat("Spawn Timer: {0} secs\n", st.SpawnTimer)
                .AppendFormat("Spawning In: {0}\n", countTimer)
                .AppendFormat("Spawn Count: {0}\n", st.SpawnCount)
                .AppendFormat("Y: {0:f3}  X: {1:f3}  Z: {2:f3}", st.Y, st.X, st.Z);

            return spawnTimer.ToString();
        }

        private static void GetSpawnTimeDiff(Spawntimer st, ref int countTime, ref string countTimer)
        {
            if (st.NextSpawnDT != DateTime.MinValue)
            {
                TimeSpan Diff = st.NextSpawnDT.Subtract(DateTime.Now);

                countTimer = $"{Diff.Hours:00}:{Diff.Minutes:00}:{Diff.Seconds:00}";

                countTime = (Diff.Hours * 3600) + (Diff.Minutes * 60) + Diff.Seconds;
            }
        }

        private string GroundItemInfo(GroundItem gi)
        {
            try
            {
                if (f1 == null) { return ""; }

                StringBuilder grounditemInfo = new StringBuilder();

                grounditemInfo.AppendFormat("Ground Item: {0}\n", gi.Desc);

                grounditemInfo.AppendFormat("ActorDef: {0}\n", gi.Name);

                grounditemInfo.AppendFormat("Y: {0:f3}  X: {1:f3}  Z: {2:f3}", gi.Y, gi.X, gi.Z);

                lblMobInfo.BackColor = Color.White;

                Graphics g = lblMobInfo.CreateGraphics();

                SizeF sf = g.MeasureString(grounditemInfo.ToString(), lblMobInfo.Font);

                Graphics gt = lblGameClock.CreateGraphics();

                SizeF sc = gt.MeasureString(lblGameClock.Text, lblGameClock.Font);

                g.Dispose();

                gt.Dispose();

                sf.ToPointF();

                sc.ToPointF();

                TableLayout(9, ref sf, ref sc);

                return grounditemInfo.ToString();
            }
            catch (Exception ex) { LogLib.WriteLine("Error with TimerInfo(): ", ex); return ""; }
        }

        private void TableLayout(int height, ref SizeF sf, ref SizeF sc)
        {
            tableLayoutPanel1.ColumnStyles[0].SizeType = SizeType.Absolute;

            if (Settings.Default.ShowTargetInfo)
            {
                lblMobInfo.Visible = true;

                if (sf.Width > sc.Width)
                {
                    tableLayoutPanel1.Width = (int)sf.Width + 10;

                    tableLayoutPanel1.ColumnStyles[0].Width = (int)sf.Width + 10;
                }
                else
                {
                    tableLayoutPanel1.Width = (int)sc.Width + 10;

                    tableLayoutPanel1.ColumnStyles[0].Width = (int)sc.Width + 10;
                }

                tableLayoutPanel1.RowStyles[1].SizeType = SizeType.Absolute;

                tableLayoutPanel1.RowStyles[0].SizeType = SizeType.Absolute;

                tableLayoutPanel1.RowStyles[1].Height = (int)sf.Height + height;

                tableLayoutPanel1.RowStyles[0].Height = (int)sc.Height + 7;
            }
            else
            {
                tableLayoutPanel1.RowStyles[1].SizeType = SizeType.Absolute;

                tableLayoutPanel1.RowStyles[0].SizeType = SizeType.Absolute;

                tableLayoutPanel1.RowStyles[1].Height = 0;

                tableLayoutPanel1.RowStyles[0].Height = 0;
            }
        }

        private void MapCon_Paint(object sender, PaintEventArgs pe)
        {
            if (mapPane != null && f1 != null)
            {
                try
                {
                    // Check if the Window is not minimized
                    if (f1.WindowState != FormWindowState.Minimized)
                    {
                        DrawOptions DrawOpts = f1.DrawOpts;

                        // Clear Map
                        bkgBuffer.Graphics.Clear(Settings.Default.BackColor);

                        lblGameClock.Text = $"{eq.gametime:MMM d, yyyy} {eq.gametime:t}";

                        // Set the Spawn Size

                        if (SettingsSpawnSize != Settings.Default.SpawnDrawSize)
                        {
                            SetSpawnSizes();
                        }

                        // Used to help reduce the number of calls to improve speed

                        var pX = eq.gamerInfo.X;

                        var pY = eq.gamerInfo.Y;

                        var pZ = eq.gamerInfo.Z;

                        var playerx = CalcScreenCoordX(pX);

                        var playery = CalcScreenCoordY(pY);

                        var realhead = eq.CalcRealHeading(eq.gamerInfo);

                        //var dx = ((m_panOffsetX + m_screenCenterX) / -m_ratio) - m_mapCenterX;

                        //var dy = ((m_panOffsetY + m_screenCenterY) / -m_ratio) - m_mapCenterY;

                        var dx = ((m_panOffsetX + screenCenter.X) / -m_ratio) - mapCenter.X;

                        var dy = ((m_panOffsetY + screenCenter.Y) / -m_ratio) - mapCenter.Y;
                        GraphicsState tState = bkgBuffer.Graphics.Save();

                        bkgBuffer.Graphics.ScaleTransform(-m_ratio, -m_ratio);

                        bkgBuffer.Graphics.TranslateTransform(dx, dy);

                        DrawMapLines(DrawOpts);

                        bkgBuffer.Graphics.Restore(tState);

                        //bkgBuffer.Graphics.ResetTransform();

                        DrawMap(DrawOpts);

                        if ((DrawOpts & DrawOptions.SpawnTrails) != DrawOptions.None)
                        {
                            DrawSpawnTrails();
                        }

                        if (!eq.Zoning)
                        {
                            if ((DrawOpts & DrawOptions.Player) != DrawOptions.None)
                            {
                                DrawGamer(playerx, playery, SpawnSize, SpawnSizeOffset, DrawOpts);
                            }
                            DrawSpawns(DrawOpts, pX, pY, pZ, playerx, playery);
                            DrawGroundItems(DrawOpts, pZ);
                            DrawSpawntimers(DrawOpts);
                            SmoothMode();
                            DrawCorpses(DrawOpts, pZ);
                        }
                        else
                        {
                            DrawGamer(CalcScreenCoordX(0.0f), CalcScreenCoordY(0.0f), SpawnSize, SpawnSizeOffset, DrawOpts);
                        }

                        bkgBuffer.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                        // [42!] Draw a line to an arbitrary spot.
                        DrawDashLine(DrawOpts, playerx, playery);

                        //f1.toolStripFPS.Text = $"FPS: {fpsValue}";

                        // Setup GDI Drawing
                        bkgBuffer.Render(pe.Graphics);
                    }
                }
                catch (Exception ex) { LogLib.WriteLine("Error in MapCon_Paint(): ", ex); }
            }
        }

        private void SmoothMode()
        {
            if (Settings.Default.SpawnDrawSize > 1)
            {
                bkgBuffer.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            }
        }

        private void DrawDashLine(DrawOptions DrawOpts, float playerx, float playery)
        {
            if ((selectedPoint.X != -1) && ((DrawOpts & DrawOptions.SpotLine) != DrawOptions.None))
            {
                DrawLine(new Pen(new SolidBrush(Color.White))
                {
                    DashStyle = DashStyle.Dash,

                    DashPattern = new float[] { 8, 4 }
                }, playerx, playery, CalcScreenCoordX(selectedPoint.X), CalcScreenCoordY(selectedPoint.Y));
            }
        }

        #region DrawSpawns

        private readonly bool NPCDepthFilter = Settings.Default.DepthFilter && Settings.Default.FilterNPCs;
        private readonly bool FilterPlayers = Settings.Default.DepthFilter && Settings.Default.FilterPlayers;
        private readonly bool showPVP = Settings.Default.ShowPVP;
        private readonly bool ShowNames = Settings.Default.ShowNPCNames;
        private readonly bool ShowLevel = Settings.Default.ShowNPCLevels;
        private void DrawSpawns(DrawOptions DrawOpts, float pX, float pY, float pZ, float playerx, float playery)
        {
            if ((DrawOpts & DrawOptions.Spawns) != DrawOptions.None)
            {
                SolidBrush WhiteBrush = new SolidBrush(Color.White);

                //            string gName;

                var ShowRings = (DrawOpts & DrawOptions.SpawnRings) != DrawOptions.None;

                var DrawDirection = (DrawOpts & DrawOptions.DirectionLines) != DrawOptions.None;
                var colorRangeCircle = Settings.Default.ColorRangeCircle;
                if ((eq.selectedID == 99999) && (eq.SpawnX == -1))
                {
                    lblMobInfo.Text = MobInfo(null, true, true);
                }

                // Collect mob trails, every 8th pass - approx once every 1 sec
                map.trails.CountMobTrails(eq);
                // Draw Spawns

                foreach (Spawninfo sp in eq.GetMobsReadonly().Values)
                {
                    GetSpawnLoc(out var x, out var y, sp);
                    //                gName = eq.GuildNumToString(sp.Guild);
                    // Draw Line from Gamer to the Selected Spawn

                    if (eq.selectedID == sp.SpawnID)
                    {
                        LineGamerToSelected(playerx, playery, sp, x, y);
                    }
                    else if (colorRangeCircle && Settings.Default.RangeCircle > 0)
                    {
                        ProxAlert(pX, pY, pZ, sp);
                    }
                    else
                    {
                        sp.proxAlert = false;
                    }

                    // Draw All Other Spawns

                    if (sp.SpawnID != eq.gamerInfo.SpawnID && sp.flags == 0 && sp.Name.Length > 0)
                    {
                        if (curTarget == sp.Name)
                        {
                            FillRectangle(WhiteBrush, x - PlusSzOZ - 1, y - PlusSzOZ - 1, SpawnPlusSize + 2, SpawnPlusSize + 2);
                        }

                        if (sp.isEventController)
                        {
                            DrawSpecialMobs(pZ, sp, x, y, Color.Purple);
                        }
                        else if (sp.isLDONObject)
                        {
                            DrawSpecialMobs(pZ, sp, x, y, Color.Gray);
                        }
                        else if (sp.Type == 0)
                        {
                            // Draw Other Players

                            DrawOtherPlayers(DrawOpts, pZ, sp, x, y);
                        }
                        else if (sp.Type == 1 || sp.Type == 4)
                        {
                            DrawNPCs(pZ, NPCDepthFilter, DrawDirection, sp, x, y);
                        }
                        DrawRings(x, y, sp);
                        DrawFlashes(pZ, x, y, sp);
                        MarkSpecial(pZ, x, y, ShowRings, sp);
                    }
                }

                lookup_set = false;
            }
        }

        private void DrawNPCs(float pZ, bool NPCDepthFilter, bool DrawDirection, Spawninfo sp, float x, float y)
        {
            if (!NPCDepthFilter || ((sp.Z > pZ - filterneg) && (sp.Z < pZ + filterpos)))
            {
                SolidBrush GrayBrush = new SolidBrush(Color.Gray);
                Pen purplePen = new Pen(new SolidBrush(Color.Purple));
                sp.filtered = false;

                if (sp.Name.Length > 0)
                {
                    if (ShowNames)
                    {
                        DrawSpawnNames(textBrush, sp.Name, sp.X, sp.Y);//, gName);
                    }
                    if (ShowLevel)
                    {
                        DrawSpawnNames(textBrush, sp.Level.ToString(), sp.X, sp.Y);//, gName);
                    }
                }
                if (DrawDirection)
                {
                    DrawDirectionLines(sp, x, y);
                }

                // Draw NPCs
                if ((sp.isPet && !Settings.Default.ShowPVP) || sp.isFamiliar || sp.isMount)
                {
                    FillEllipse(GrayBrush, x - SpawnSizeOffset, y - SpawnSizeOffset, SpawnSize, SpawnSize);
                }
                else if (con.ConColors[sp.Level] != null)
                {
                    FillEllipse(con.ConColors[sp.Level], x - SpawnSizeOffset, y - SpawnSizeOffset, SpawnSize, SpawnSize);
                }

                // Draw PC color border around Mercenary

                if (sp.isMerc)
                {
                    DrawEllipse(PCBorder, x - SpawnSizeOffset, y - SpawnSizeOffset, SpawnSize, SpawnSize);
                }

                // Draw Purple border around invis mobs

                if (sp.Hide != 0)
                {
                    // Flashing purple ring around SoS mobs

                    if (sp.Hide == 2)
                    {
                        if (flash)
                        {
                            DrawEllipse(new Pen(new SolidBrush(Color.White)), x - SelectSizeOffset, y - SelectSizeOffset, SelectSize, SelectSize);
                        }
                    }
                    else
                    {
                        DrawEllipse(purplePen, x - SelectSizeOffset, y - SelectSizeOffset, SelectSize, SelectSize);
                    }
                }
            }
            else
            {
                sp.filtered = true;
            }
        }

        private void DrawOtherPlayers(DrawOptions DrawOpts, float pZ, Spawninfo sp, float x, float y)
        {
            if (!FilterPlayers || ((sp.Z > pZ - filterneg) && (sp.Z < pZ + filterpos)))
            {
                sp.filtered = false;
                if (con.ConColors[sp.Level] != null)
                {
                    if ((DrawOpts & DrawOptions.DirectionLines) != DrawOptions.None)
                    {
                        DrawDirectionLines(sp, x, y);
                    }

                    // Draw Other Players

                    if (Settings.Default.ShowPVP)
                    {
                        if ((Math.Abs(eq.gamerInfo.Level - sp.Level) <= Settings.Default.PVPLevels) || (Settings.Default.PVPLevels == -1))
                        {
                            DrawPVP(sp, x, y);
                        }
                    }
                    else
                    {
                        DrawPlayer(sp, x, y);
                    }
                }
            }
            else
            {
                sp.filtered = true;
            }
        }

        private void DrawPlayer(Spawninfo sp, float x, float y)
        {
            FillRectangle(con.ConColors[sp.Level], x - PlusSzOZ + 0.5f, y - PlusSzOZ + 0.5f, SpawnPlusSize, SpawnPlusSize);

            DrawRectangle(PCBorder, x - PlusSzOZ + 0.5f, y - PlusSzOZ + 0.5f, SpawnPlusSize, SpawnPlusSize);

            // draw purple border around players

            DrawStealthorInvisPlayer(sp, x, y);

            if (ShowPCName && (sp.Name.Length > 0))
            {
                DrawSpawnNames(textBrush, $"{sp.Level}: {sp.Name}", sp.X, sp.Y);//, gName);
            }
            //else if (Settings.Default.ShowPCGuild && (gName.Length > 0))
            //{ DrawSpawnNames(textBrush, gName, sp.X, sp.Y); }//, gName); }
        }

        private void DrawStealthorInvisPlayer(Spawninfo sp, float x, float y)
        {
            if (sp.Hide != 0)
            {
                Pen purplePen = new Pen(new SolidBrush(Color.Purple));
                if (sp.Hide == 2)
                {
                    // SoS Players

                    if (flash)
                    {
                        DrawRectangle(new Pen(new SolidBrush(Color.White)), x - PlusSzOZ - 0.5f, y - PlusSzOZ - 0.5f, SpawnPlusSize + 2.0f, SpawnPlusSize + 2.0f);
                    }
                }
                else
                {
                    // Player is invis

                    DrawRectangle(purplePen, x - PlusSzOZ - 0.5f, y - PlusSzOZ - 0.5f, SpawnPlusSize + 2.0f, SpawnPlusSize + 2.0f);
                }
            }
        }

        private void DrawPVP(Spawninfo sp, float x, float y)
        {
            FillTriangle(con.ConColors[sp.Level], x, y, SelectSizeOffset);

            DrawTriangle(PCBorder, x, y, SelectSizeOffset);

            if (ShowPCName && (sp.Name.Length > 0))
            //                        && Settings.Default.ShowPCGuild && (gName.Length > 0))
            {
                DrawSpawnNames(textBrush, $"{sp.Level}: {sp.Name}", sp.X, sp.Y);//, gName);
            }
            else
                if (ShowPCName && (sp.Name.Length > 0))
            {
                DrawSpawnNames(textBrush, $"{sp.Level}: {sp.Name}", sp.X, sp.Y);//, gName);
            }
            else if (Settings.Default.ShowPVPLevel)
            {
                DrawSpawnNames(textBrush, sp.Level.ToString(), sp.X, sp.Y);//, gName);
            }

            if (flash)
            {
                Pen cPen = new Pen(eq.GetDistinctColor(Color.White));

                DrawEllipse(cPen, x - SelectSizeOffset, y - SelectSizeOffset, SelectSize, SelectSize);
            }
        }

        private void DrawCorpses(DrawOptions DrawOpts, float pZ)
        {
            if ((DrawOpts & DrawOptions.Spawns) != DrawOptions.None)
            {
                var PCCorpseDepthFilter = Settings.Default.DepthFilter && Settings.Default.FilterPlayerCorpses;

                var NPCCorpseDepthFilter = Settings.Default.DepthFilter && Settings.Default.FilterNPCCorpses;

                var drawOffset = PlusSzOZ - 0.5f;

                // Draw Spawns

                foreach (Spawninfo sp in eq.GetMobsReadonly().Values)

                {
                    GetSpawnLoc(out var x, out var y, sp);

                    // Draw Corpses

                    if (sp.isCorpse && !sp.hidden)

                    {
                        PointF corpsePoint = new PointF(x, y);
                        sp.proxAlert = false;

                        // Draw Corpses

                        if (sp.IsPlayer)

                        {
                            if (!PCCorpseDepthFilter || ((sp.Z > pZ - filterneg) && (sp.Z < pZ + filterpos)))
                            {
                                DrawRectangle(new Pen(new SolidBrush(Color.Yellow)), corpsePoint.X - PlusSzOZ + 0.5f, corpsePoint.Y - PlusSzOZ + 0.5f, SpawnPlusSize, SpawnPlusSize);

                                if (Settings.Default.ShowPlayerCorpseNames && (sp.Name.Length > 0))
                                {
                                    DrawSpawnNames(textBrush, $"{sp.Level}: {sp.Name}", sp.X, sp.Y); //, gName);
                                }

                                sp.filtered = false;
                            }
                            else
                            {
                                sp.filtered = true;
                            }
                        }
                        else
                        {
                            if (!NPCCorpseDepthFilter || ((sp.Z > pZ - filterneg) && (sp.Z < pZ + filterpos)))
                            {
                                Pen cyanPen = new Pen(new SolidBrush(Color.Cyan));
                                DrawCross(cyanPen, corpsePoint, drawOffset);

                                if (Settings.Default.ShowNPCCorpseNames && (sp.Name.Length > 0))
                                {
                                    DrawSpawnNames(textBrush, $"{sp.Level}: {sp.Name}", sp.X, sp.Y); //, gName);
                                }

                                sp.filtered = false;
                            }
                            else
                            {
                                sp.filtered = true;
                            }
                        }
                    }
                }
            }
        }

        private void GetSpawnLoc(out float x, out float y, Spawninfo sp)
        {
            x = (float)Math.Round(CalcScreenCoordX(sp.X), 0);

            y = (float)Math.Round(CalcScreenCoordY(sp.Y), 0);
        }
        private void SetLookupValues()
        {
            LookupRingSize = SpawnPlusSize + (skittle / (float)UpdateSteps * SelectSize);
            LookupRingOffset = LookupRingSize / 2.0f;
            lookup_set = true;
        }

        private void DrawSpecialMobs(float pZ, Spawninfo sp, float x, float y, Color color)
        {
            if (!NPCDepthFilter || ((sp.Z > pZ - filterneg) && (sp.Z < pZ + filterpos)))
            {
                FillEllipse(new SolidBrush(color), x - SpawnSizeOffset, y - SpawnSizeOffset, SpawnSize, SpawnSize);
                sp.filtered = false;
            }
            else
            {
                sp.filtered = true;
            }
        }

        private void LineGamerToSelected(float playerx, float playery, Spawninfo sp, float x, float y)
        {
            Pen pinkPen = new Pen(new SolidBrush(Color.Fuchsia));
            DrawEllipse(pinkPen, x - SelectSizeOffset, y - SelectSizeOffset, SelectSize, SelectSize);

            // Update the Spawn Information Window if not based on selected timer

            if (eq.SpawnX == -1)
            {
                lblMobInfo.Text = MobInfo(sp, true, true);

                DrawLine(pinkPen, playerx, playery, x, y);
            }

            sp.proxAlert = false;
        }

        private void ProxAlert(float pX, float pY, float pZ, Spawninfo sp)
        {
            if (sp.alertMob && (sp.Type != 2))
            {
                // if alertmob - use to identify mobs that are ok to do alerts

                var minlevel = Settings.Default.MinAlertLevel;

                if (minlevel == -1)
                {
                    minlevel = eq.gamerInfo.Level + con.GreyRange;
                }

                if (sp.Level >= minlevel)
                {
                    float rRange = Settings.Default.RangeCircle;

                    var maxZ = pZ + rRange;

                    var minZ = pZ - rRange;

                    if (NPCDepthFilter)
                    {
                        maxZ = pZ + filterpos;

                        minZ = pZ - filterpos;
                    }

                    if (!sp.proxAlert)
                    {
                        if ((sp.Z > minZ) && (sp.Z < maxZ))
                        {
                            var sd = ((pX - sp.X) * (pX - sp.X)) + ((pY - sp.Y) * (pY - sp.Y));

                            if (sd < (rRange * rRange))
                            {
                                sp.proxAlert = true;
                                FormMethods.SwitchOnSoundSettings();
                            }
                        }
                    }
                    else
                    {
                        if ((sp.Z < (1.2f * minZ)) || (sp.Z > (1.2f * maxZ)))
                        {
                            sp.proxAlert = false;
                        }
                        else
                        {
                            var sd = ((pX - sp.X) * (pX - sp.X)) + ((pY - sp.Y) * (pY - sp.Y));

                            if (sd > (1.4 * rRange * rRange))
                            {
                                sp.proxAlert = false;
                            }
                        }
                    }
                }
                else
                {
                    sp.proxAlert = false;
                }
            }
            else
            {
                sp.proxAlert = false;
            }
        }

        private void MarkSpecial(float pZ, float x, float y, bool ShowRings, Spawninfo sp)
        {
            if (ShowRings && (!NPCDepthFilter || ((sp.Z > pZ - filterneg) && (sp.Z < pZ + filterpos))))
            {
                Pen WhitePen = new Pen(new SolidBrush(Color.White));

                if (sp.Class == 40)// Draw Ring around Bankers
                {
                    DrawEllipse(WhitePen, x - SpawnSizeOffset, y - SpawnSizeOffset, SpawnSize, SpawnSize);

                    DrawEllipse(new Pen(new SolidBrush(Color.Green)), x - PlusSzOZ, y - PlusSzOZ, SpawnPlusSize, SpawnPlusSize);
                }
                if (sp.Class > 19 && sp.Class < 35)                // Draw Ring around Guild Master
                {
                    DrawEllipse(WhitePen, x - SpawnSizeOffset, y - SpawnSizeOffset, SpawnSize, SpawnSize);

                    DrawEllipse(new Pen(new SolidBrush(Color.Red)), x - PlusSzOZ, y - PlusSzOZ, SpawnPlusSize, SpawnPlusSize);
                }

                if (sp.Class == 41)                // Draw Ring around Shopkeepers
                {
                    DrawEllipse(WhitePen, x - SpawnSizeOffset, y - SpawnSizeOffset, SpawnSize, SpawnSize);

                    DrawEllipse(new Pen(new SolidBrush(Color.Blue)), x - PlusSzOZ, y - PlusSzOZ, SpawnPlusSize, SpawnPlusSize);
                }
            }
        }

        private void DrawFlashes(float pZ, float x, float y, Spawninfo sp)
        {
            if (flash)
            {
                var x1 = x - PlusSzOZ;
                var y1 = y - PlusSzOZ;
                var above = sp.Z < pZ + filterpos;
                var below = sp.Z > pZ - filterneg;

                // Draw Ring around Hunted Mobs

                if (!NPCDepthFilter || (below && above))
                {
                    if (sp.isHunt || sp.proxAlert)
                    {
                        DrawEllipse(new Pen(new SolidBrush(Color.LimeGreen), 2), x1, y1, SpawnPlusSize, SpawnPlusSize);
                    }

                    // Draw Ring around Caution Mobs

                    if (sp.isCaution)
                    {
                        DrawEllipse(new Pen(new SolidBrush(Color.Yellow), 2), x1, y1, SpawnPlusSize, SpawnPlusSize);
                    }

                    // Draw Ring around Danger Mobs

                    if (sp.isDanger)
                    {
                        DrawEllipse(new Pen(new SolidBrush(Color.Red), 2), x1, y1, SpawnPlusSize, SpawnPlusSize);
                    }

                    // Draw Ring around Rare Mobs

                    if (sp.isAlert)
                    {
                        DrawEllipse(new Pen(new SolidBrush(Color.White), 2), x1, y1, SpawnPlusSize, SpawnPlusSize);
                    }
                }
            }
        }

        private void DrawRings(float x, float y, Spawninfo sp)
        {
            //            string gName = eq.GuildNumToString(sp.Guild);
            if (sp.isLookup && (!sp.isCorpse || Settings.Default.CorpseAlerts))
            {
                if (!lookup_set)
                {
                    SetLookupValues();
                }

                DrawEllipse(new Pen(new SolidBrush(Color.White)), x - LookupRingOffset, y - LookupRingOffset, LookupRingSize, LookupRingSize);

                if (Settings.Default.ShowLookupText)
                {
                    if (Settings.Default.ShowLookupNumber)
                    {
                        DrawSpawnNames(textBrush, sp.lookupNumber, sp.X, sp.Y);//, gName);
                    }
                    else
                    {
                        DrawSpawnNames(textBrush, sp.Name, sp.X, sp.Y);//, gName);
                    }
                }
            }
        }

        #endregion DrawSpawns

        #region DrawMap

        public void DrawMapLines(DrawOptions DrawOpts)
        {
            try
            {
                // Draw Zone Map
                if (eq.longname != "" && ((DrawOpts & DrawOptions.DrawMap) != DrawOptions.None))
                {
                    if (!Settings.Default.DepthFilter || (Settings.Default.DepthFilter && !Settings.Default.FilterMapLines))
                    // No depth filtering
                    {
                        foreach (MapLine mapLine in map.Lines)
                        {
                            DrawLines(mapLine.draw_color, mapLine.linePoints);
                        }
                    }
                    else
                    {
                        MinMaxFilter(out var minZ, out var maxZ);

                        foreach (MapLine mapLine in map.Lines)
                        {
                            // All the points in this set of lines are good
                            if (mapLine.maxZ < maxZ && mapLine.minZ > minZ)
                            {
                                DrawLines(mapLine.draw_color, mapLine.linePoints);
                            }
                            else if (mapLine.maxZ < minZ || mapLine.minZ > maxZ)
                            {
                                DrawLines(mapLine.fade_color, mapLine.linePoints);
                            }
                            else
                            {
                                AlphaFiltering(minZ, maxZ, mapLine);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { LogLib.WriteLine("Error in DrawMapLines(): ", ex); }
        }

        private void AlphaFiltering(float minZ, float maxZ, MapLine mapLine)
        {
            bool curValid, lastValid;

            float curX, curY, curZ, lastX, lastY, lastZ;

            lastX = mapLine.Point(0).x;

            lastY = mapLine.Point(0).y;

            lastZ = mapLine.Point(0).z;

            lastValid = (lastZ > minZ) && (lastZ < maxZ);

            for (var d = 1; d < mapLine.aPoints.Count; d++)
            {
                curX = mapLine.Point(d).x;

                curY = mapLine.Point(d).y;

                curZ = mapLine.Point(d).z;

                curValid = (curZ > minZ) && (curZ < maxZ);

                // Original Depth Filter method (use z-axis values only)

                // instead of not drawing filtered lines, we draw light ones

                if (!curValid && !lastValid)
                {
                    if (Settings.Default.UseDynamicAlpha)
                    {
                        DrawLine(mapLine.fade_color, lastX, lastY, curX, curY);
                    }
                }
                else
                {
                    DrawLine(mapLine.draw_color, lastX, lastY, curX, curY);
                }

                lastX = curX;

                lastY = curY;

                lastValid = curValid;
            }
        }

        private void MinMaxFilter(out float minZ, out float maxZ)
        {
            minZ = eq.gamerInfo.Z - filterneg;
            maxZ = eq.gamerInfo.Z + filterpos;
        }

        public void DrawMap(DrawOptions DrawOpts)
        {
            try
            {
                if ((DrawOpts & DrawOptions.GridLines) != DrawOptions.None)
                    DrawGridLines();

                if ((DrawOpts & DrawOptions.ZoneText) != DrawOptions.None)
                {
                    DepthfilterText();
                }
            }
            catch (Exception ex) { LogLib.WriteLine("Error in DrawMap(): ", ex); }
        }

        private void DrawGridLines()
        {
            int gx, gy, label;

            float sx, sy;

            var gridInterval = Settings.Default.GridInterval;

            Color curGridColor = Settings.Default.GridColor;

            Color curGridLabelColor = Settings.Default.GridLabelColor;

            if (curGridLabelColor != gridLabelColor)
            {
                gridLabelColor = curGridLabelColor;
            }
            gridBrush = new SolidBrush(gridLabelColor);

            if (curGridColor != gridColor)
            {
                gridColor = curGridColor;
            }
            // Draw gridline...
            Pen gridPen = new Pen(gridColor);
            // Draw Horizontal Grid Lines

            for (gx = ((int)(eq.minx / gridInterval)) - 1; gx < (eq.maxx / gridInterval) + 1; gx++)

            {
                label = gx * gridInterval;

                sx = (float)Math.Round(CalcScreenCoordX(label), 0);
                DrawLine(gridPen, sx, 0, sx, Height);

                bkgBuffer.Graphics.DrawString(label.ToString(), drawFont, gridBrush, sx, Height - (drawFont.GetHeight() + 5));
            }

            // Draw Vertical Grid Lines

            for (gy = ((int)(eq.miny / gridInterval)) - 1; gy < (int)(eq.maxy / gridInterval) + 1; gy++)

            {
                label = gy * gridInterval;

                sy = (float)Math.Round(CalcScreenCoordY(label), 0);
                DrawLine(gridPen, 0, sy, Width, sy);

                bkgBuffer.Graphics.DrawString(label.ToString(), drawFont, gridBrush, Width - (bkgBuffer.Graphics.MeasureString(label.ToString(), drawFont).Width + 5), sy);
            }
        }

        private void DepthfilterText()
        {
            // Draw Zone Text
            if (Settings.Default.DepthFilter && Settings.Default.FilterMapText)
            {
                // Depth Filter
                MinMaxFilter(out var minZ, out var maxZ);

                foreach (MapText t in map.Texts)
                {
                    if (t.z != -99999 && t.z > minZ && t.z < maxZ)
                    {
                        AddTextToDrawnMap(t);
                    }
                }
            }
            else
            {
                // No Depth Filtering
                foreach (MapText t in map.Texts)
                {
                    AddTextToDrawnMap(t);
                }
            }
        }

        private void AddTextToDrawnMap(MapText t)
        {
            var x_cord = (int)CalcScreenCoordX(t.x);
            var y_cord = (int)CalcScreenCoordY(t.y);
            if (t.draw_color is null) t.draw_color = new SolidBrush(Color.HotPink);
            if (t.draw_pen is null) t.draw_pen = new Pen(Color.HotPink);
            if (t.size == 2)
            {// check for null
                bkgBuffer.Graphics.DrawString(t.label, drawFont, t.draw_color, x_cord, y_cord - t.offset);
            }
            else if (t.size == 1)
            {
                bkgBuffer.Graphics.DrawString(t.label, drawFont1, t.draw_color, x_cord, y_cord - t.offset);
            }
            else
            {
                bkgBuffer.Graphics.DrawString(t.label, drawFont3, t.draw_color, x_cord, y_cord - t.offset);
            }
            bkgBuffer.Graphics.DrawLine(t.draw_pen, x_cord - 1, y_cord, x_cord + 1, y_cord);
            bkgBuffer.Graphics.DrawLine(t.draw_pen, x_cord, y_cord - 1, x_cord, y_cord + 1);
        }

        #endregion DrawMap

        #region DrawGamer

        public void DrawGamer(float gamerX, float gamerY, float SpawnSize, float SpawnSizeOffset, DrawOptions DrawOpts)
        {
            try
            {
                var xHead = (int)eq.gamerInfo.Heading;

                // Draw Range Circle

                if (Settings.Default.RangeCircle > 0)

                {
                    var rCircleRadius = Settings.Default.RangeCircle * m_ratio;
                    MakeRangeCircle(gamerX, gamerY, rCircleRadius);

                    // Draw Red V in the Range Circle

                    if (Settings.Default.DrawFoV && xHead >= 0 && xHead < 512)

                    {
                        DrawFoV(gamerX, gamerY, xHead, rCircleRadius);
                    }

                    if (m_rangechange)

                    {
                        if (flash)
                        {
                            DrawEllipse(eq.GetDistinctColor(new Pen(Settings.Default.RangeCircleColor)), gamerX - rCircleRadius, gamerY - rCircleRadius, rCircleRadius * 2, rCircleRadius * 2);
                        }
                    }
                    else
                    {
                        DrawEllipse(eq.GetDistinctColor(new Pen(Settings.Default.RangeCircleColor)), gamerX - rCircleRadius, gamerY - rCircleRadius, rCircleRadius * 2, rCircleRadius * 2);
                    }
                }

                // Draw Player  (only if we actually have a player)

                if (eq.gamerInfo.SpawnID != 0)

                {
                    // Draw Player Heading Line

                    if ((DrawOpts & DrawOptions.DirectionLines) != DrawOptions.None && xHead >= 0 && xHead < 512)

                    {
                        var y1 = -(float)(xCos[xHead] * (eq.gamerInfo.SpeedRun * m_ratio * 100));

                        var x1 = -(float)(xSin[xHead] * (eq.gamerInfo.SpeedRun * m_ratio * 100));

                        DrawLine(new Pen(new SolidBrush(Color.White)), gamerX, gamerY, gamerX + x1, gamerY + y1);
                    }

                    FillRectangle(new SolidBrush(Color.White), gamerX - SpawnSizeOffset, gamerY - SpawnSizeOffset, SpawnSize, SpawnSize);

                    DrawRectangle(PCBorder, gamerX - SpawnSizeOffset - 0.5f, gamerY - SpawnSizeOffset - 0.5f, SpawnSize + 1.0f, SpawnSize + 1.0f);
                }
            }
            catch (Exception ex) { LogLib.WriteLine("Error in DrawPlayer(): ", ex); }
        }

        private void DrawFoV(float gamerX, float gamerY, int xHead, float rCircleRadius)
        {
            float x, y;

            if (xHead < 448)
            {
                y = -(float)(xCos[xHead + 64] * rCircleRadius * 1.05f);
                x = -(float)(xSin[xHead + 64] * rCircleRadius * 1.05f);
            }
            else
            {
                y = -(float)(xCos[xHead + 64 - 512] * rCircleRadius * 1.05);

                x = -(float)(xSin[xHead + 64 - 512] * rCircleRadius * 1.05);
            }

            DrawLine(new Pen(new SolidBrush(Color.Red)), gamerX, gamerY, gamerX + x, gamerY + y);

            if (xHead >= 64)

            {
                y = -(float)(xCos[xHead - 64] * rCircleRadius * 1.05f);

                x = -(float)(xSin[xHead - 64] * rCircleRadius * 1.05f);
            }
            else
            {
                y = -(float)(xCos[xHead - 64 + 512] * rCircleRadius * 1.05f);

                x = -(float)(xSin[xHead - 64 + 512] * rCircleRadius * 1.05f);
            }

            DrawLine(new Pen(new SolidBrush(Color.Red)), gamerX, gamerY, gamerX + x, gamerY + y);

            // Draw Heading Line

            y = -(float)(xCos[xHead] * rCircleRadius);

            x = -(float)(xSin[xHead] * rCircleRadius);

            DrawLine(new Pen(new SolidBrush(Color.Yellow)), gamerX, gamerY, gamerX + x, gamerY + y);
        }

        private void MakeRangeCircle(float gamerx, float gamery, float rCircleRadius)
        {
            if (Settings.Default.ColorRangeCircle)
            {
                HatchStyle hs = (HatchStyle)Enum.Parse(typeof(HatchStyle), Settings.Default.HatchIndex, true);

                HatchBrush hatchBrush = new HatchBrush(hs, Settings.Default.RangeCircleColor, Color.Transparent);

                FillEllipse(hatchBrush, gamerx - rCircleRadius, gamery - rCircleRadius, rCircleRadius * 2, rCircleRadius * 2);
            }

        }

        #endregion DrawGamer

        #region DrawSpawnTimers
        private void DrawSpawntimers(DrawOptions DrawOpts)
        {
            if ((DrawOpts & DrawOptions.SpawnTimers) != DrawOptions.None)
            {
                try
                {
                    MinMaxFilter(out var minZ, out var maxZ);

                    // Draw Spawn Timers

                    Pen pen = new Pen(new SolidBrush(Color.LightGray));

                    foreach (Spawntimer st in eq.mobsTimers.GetRespawned().Values)
                    {
                        if (st.zone == eq.shortname)
                        {
                            PointF timerPoint = new PointF((float)Math.Round(CalcScreenCoordX(st.X), 0), (float)Math.Round(CalcScreenCoordY(st.Y), 0));
                            //var stX = (float)Math.Round(CalcScreenCoordX(st.X), 0);

                            //var stY = (float)Math.Round(CalcScreenCoordY(st.Y), 0);

                            var stOffset = PlusSzOZ - 0.5f;

                            var checkTimer = st.SecondsUntilSpawn(DateTime.Now);

                            var canDraw = false;

                            CheckTimer(ref pen, checkTimer, ref canDraw);

                            // if depth filter on make adjustments to spawn points
                            canDraw = CheckDepthFilter(minZ, maxZ, st, canDraw);
                            if (canDraw)
                            {
                                DrawCross(pen, timerPoint, stOffset);

                                //DrawLine(pen, timerPoint.X - stOffset, timerPoint.Y, timerPoint.X + stOffset, timerPoint.Y);
                                //DrawLine(pen, timerPoint.X, timerPoint.Y - stOffset, timerPoint.X, timerPoint.Y + stOffset,);

                                if (Settings.Default.SpawnCountdown && (checkTimer > 0) && (checkTimer < 120))
                                {
                                    DrawSpawnNames(textBrush, checkTimer.ToString(), st.X, st.Y);//, "");
                                }
                            }

                            // Draw Blue Line to selected spawn location

                            if ((st.X == eq.SpawnX) && (st.Y == eq.SpawnY))
                            {
                                GetGamerPoint();

                                pen = new Pen(new SolidBrush(Color.Blue));

                                bkgBuffer.Graphics.DrawLine(pen, gamerPos, timerPoint);
                                //DrawLine(pen, gamerPos.X, gamerPos.Y, stX, stY);

                                // Update the Spawn Information Window

                                lblMobInfo.Text = TimerInfo(st);
                            }
                        }
                    }
                }
                catch (Exception ex) { LogLib.WriteLine("Error in DrawSpawnTimers(): ", ex); }
            }
        }

        private static bool CheckDepthFilter(float minZ, float maxZ, Spawntimer st, bool canDraw)
        {
            if (Settings.Default.DepthFilter && Settings.Default.FilterSpawnPoints)
            {
                if ((st.Z > maxZ) || (st.Z < minZ))
                {
                    canDraw = false;
                    st.filtered = true;
                }
                else
                {
                    st.filtered = false;
                }
            }
            else
            {
                st.filtered = false;
            }

            return canDraw;
        }

        private void CheckTimer(ref Pen pen, int checkTimer, ref bool canDraw)
        {
            Pen redPen = new Pen(new SolidBrush(Color.Red));
            if (checkTimer == 0)
                canDraw = true;

            if (checkTimer > 0)
            {
                canDraw = true;
                // Set Pen Colors
                if (checkTimer < 30)

                {
                    if (flash)
                    {
                        pen = redPen;
                    }
                }
                else if (checkTimer < 60)

                {
                    pen = redPen;
                }
                else if (checkTimer < 90)

                {
                    pen = new Pen(new SolidBrush(Color.Orange));
                }
                else if (checkTimer < 120)

                {
                    pen = new Pen(new SolidBrush(Color.Yellow));
                }
            }
        }

        private void GetGamerPoint()
        {
            gamerPos.X = CalcScreenCoordX(eq.gamerInfo.X);
            gamerPos.Y = CalcScreenCoordY(eq.gamerInfo.Y);
        }

        #endregion DrawSpawnTimers

        #region DrawGroundItems
        private void DrawGroundItems(DrawOptions DrawOpts, float pZ)
        {
            if ((DrawOpts & DrawOptions.GroundItems) != DrawOptions.None)
            {
                var GroundItemDepthFilter = Settings.Default.DepthFilter && Settings.Default.FilterGroundItems;

                float x, y;

                // Draw Ground Spawns

                foreach (GroundItem gi in eq.GetItemsReadonly())

                {
                    x = (float)Math.Round(CalcScreenCoordX(gi.X), 0);

                    y = (float)Math.Round(CalcScreenCoordY(gi.Y), 0);

                    if (!GroundItemDepthFilter || ((gi.Z > pZ - filterneg) && (gi.Z < pZ + filterpos)))
                    {
                        Pen yellowPen = new Pen(new SolidBrush(Color.Yellow));
                        gi.Filtered = false;
                        PointF giPoint = new PointF(x, y);
                        DrawCross(yellowPen, giPoint, PlusSzOZ);
                        //                        DrawLine(yellowPen, x - PlusSzOZ, y - PlusSzOZ, x + PlusSzOZ, y + PlusSzOZ);

                        //                        DrawLine(yellowPen, x - PlusSzOZ, y + PlusSzOZ, x + PlusSzOZ, y - PlusSzOZ);
                    }
                    else
                    {
                        gi.Filtered = true;
                    }

                    // Draw Yellow Line to selected ground item location
                    DrawYellowLine(x, y, gi);

                    if (flash)
                    {
                        FlashAlertGroundSpawns(pZ, GroundItemDepthFilter, x, y, gi);
                    }
                }
            }
        }

        private void DrawYellowLine(float x, float y, GroundItem gi)
        {
            if (eq.SpawnX == gi.X && eq.SpawnY == gi.Y && eq.selectedID == 99999)
            {
                GetGamerPoint();

                DrawLine(new Pen(new SolidBrush(Color.Yellow)), gamerPos.X, gamerPos.Y, x, y);

                DrawEllipse(new Pen(new SolidBrush(Color.Fuchsia)), x - SelectSizeOffset, y - SelectSizeOffset, SelectSize, SelectSize);

                // Update the Spawn Information Window

                lblMobInfo.Text = GroundItemInfo(gi);
            }
        }

        private void FlashAlertGroundSpawns(float pZ, bool GroundItemDepthFilter, float x, float y, GroundItem gi)
        {
            var x1 = x - PlusSzOZ - 1;
            var y1 = y - PlusSzOZ - 1;
            var width = SpawnPlusSize + 2;
            var height = SpawnPlusSize + 2;

            // Draw Yellow Ring around Caution Ground Items
            if (!GroundItemDepthFilter || ((gi.Z > pZ - filterneg) && (gi.Z < pZ + filterpos)))
            {
                if (gi.isCaution)
                {
                    DrawEllipse(new Pen(new SolidBrush(Color.Yellow), 2), x1, y1, width, height);
                }
                // Draw Red Ring around Danger Ground Items
                if (gi.isDanger)
                {
                    DrawEllipse(new Pen(new SolidBrush(Color.Red), 2), x1, y1, width, height);
                }

                // Draw White Ring around Rare Ground Items
                if (gi.isAlert)
                {
                    DrawEllipse(new Pen(new SolidBrush(Color.White), 2), x1, y1, width, height);
                }

                // Draw Cyan Ring around Hunt Ground Items
                if (gi.isHunt)
                {
                    DrawEllipse(new Pen(new SolidBrush(Color.Green), 2), x1, y1, width, height);
                }
            }
        }

        #endregion DrawGroundItems

        public void DrawSpawnTrails()
        {
            try
            {
                // Draw Mob Trails
                foreach (MobTrailPoint mtp in map.trails.GetMobTrailsReadonly())
                {
                    FillEllipse(new SolidBrush(Color.White), CalcScreenCoordX(mtp.x) - 2, CalcScreenCoordY(mtp.y) - 2, 2, 2);
                }
            }
            catch (Exception ex) { LogLib.WriteLine("Error in DrawSpawnTrails(): ", ex); }
        }

        #region DrawDirectionLines

        public void DrawDirectionLines(Spawninfo sp, float x, float y)
        {
            try

            {
                float x1, y1;

                // Draw NPCs Direction Lines if heading > 0

                if (sp.Heading >= 0 && sp.Heading < 512)

                {
                    y1 = -(float)(xCos[(int)sp.Heading] * (sp.SpeedRun * m_ratio * 150));

                    x1 = -(float)(xSin[(int)sp.Heading] * (sp.SpeedRun * m_ratio * 150));

                    DrawLine(new Pen(new SolidBrush(Color.White)), x, y, x + x1, y + y1);
                }
            }
            catch (Exception ex) { LogLib.WriteLine("Error in DrawDirectionLines(): ", ex); }
        }

        #endregion DrawDirectionLines

        private void MapChanged(EQMap map)
        {
            DrawOptions DrawOpts = f1.DrawOpts;

            // if the autoexpand is not checked, scale is not at 100, then maintain the map scale
            if (eq.longname.Length > 0 && mapPane != null && !Settings.Default.AutoExpand &&
                mapPane.scale.Value != 100)
            {
                GetRatioSetScale();
                ClearPan();
            }
            else if (Settings.Default.KeepCentered)
            {
                ClearPan();
            }
            else
            {
                f1.mapPane.scale.Value = 100M;
                scale = 1.0f;
                ClearPan();
            }

            // check that map text doesn't change extents

            if ((DrawOpts & DrawOptions.ZoneText) != DrawOptions.None)
            {
                VerifyTextExtents(DrawOpts);
            }

            Invalidate();
        }

        private void GetRatioSetScale()
        {
            var mapWidth = Math.Abs(eq.maxx - eq.minx);
            var mapHeight = Math.Abs(eq.maxy - eq.miny);
            var ScreenWidth = Width - (2.0f * 15);
            var ScreenHeight = Height - (2.0f * 15);
            var xratio = (float)ScreenWidth / mapWidth;
            var yratio = (float)ScreenHeight / mapHeight;
            var r_ratio = xratio < yratio ? xratio : yratio;

            SetScale(r_ratio);
        }

        private void SetScale(float r_ratio)
        {
            if (r_ratio > 0.0f)
            {
                scale = m_ratio / r_ratio;
                if (scale < 0.1)
                {
                    f1.mapPane.scale.Value = 100M;
                    scale = 1.0f;
                }
                else
                {
                    mapPane.scale.Value = (decimal)Math.Round(scale, 1) * 100;
                }
            }
            else
            {
                f1.mapPane.scale.Value = 100M;
                scale = 1.0f;
            }
        }

        private void VerifyTextExtents(DrawOptions DrawOpts)
        {
            float xlabel = 0;
            float ylabel = 0;
            var factor = 1 / m_ratio;

            if ((DrawOpts & DrawOptions.GridLines) != DrawOptions.None)
            {
                // drawing gridlines, so account for grid labels

                ylabel = drawFont.GetHeight();

                xlabel = bkgBuffer.Graphics.MeasureString("10000", drawFont).Width;
            }

            foreach (MapText t in map.Texts)
            {
                SizeF tf = bkgBuffer.Graphics.MeasureString(t.label, drawFont);

                GetTextSize(t);

                if ((t.x - ((tf.Width + xlabel) * factor)) < eq.minx)
                {
                    eq.minx = t.x - ((tf.Width + xlabel) / m_ratio);
                }
                else if (t.x > eq.maxx)
                {
                    eq.maxx = t.x;
                }

                if ((t.y + (t.offset * factor)) > eq.maxy)
                {
                    eq.maxy = t.y + (t.offset * factor);
                }
                else if ((t.y - ((tf.Height + ylabel) * factor)) < eq.miny)
                {
                    eq.miny = t.y - ((tf.Height + ylabel) * factor);
                }
            }

            ReAdjust();
        }

        private void GetTextSize(MapText t)
        {
            if (t.size == 1)
            {
                bkgBuffer.Graphics.MeasureString(t.label, drawFont1);
            }
            else if (t.size == 3)
            {
                bkgBuffer.Graphics.MeasureString(t.label, drawFont3);
            }
        }

        public void SetUpdateSteps()
        {
            var update_steps = (1000 / Settings.Default.UpdateDelay) + 1;
            if (update_steps < 3)
            {
                update_steps = 3;
            }

            var update_ticks = 250 / Settings.Default.UpdateDelay;
            if (update_ticks < 1)
            {
                update_ticks = 1;
            }

            UpdateSteps = update_steps;
            UpdateTicks = update_ticks;
        }

        public void Tick()
        {
            // This is set up for flashes and animated skittles
            skittle++;
            flash_count++;

            if (skittle > UpdateSteps)
            {
                skittle = 0;
            }

            if (flash_count >= UpdateTicks)
            {
                flash_count = 0;
                flash = !flash;
            }
        }

        private void SetSpawnSizes()
        {
            SettingsSpawnSize = Settings.Default.SpawnDrawSize;

            SpawnSize = (SettingsSpawnSize * 2.0f) - 1.0f;

            SpawnSizeOffset = SpawnSize / 2.0f;

            SpawnPlusSize = SpawnSize + 2.0f;

            PlusSzOZ = SpawnPlusSize / 2.0f;

            SelectSize = SpawnSize + 4.0f;

            SelectSizeOffset = SelectSize / 2.0f;
        }

        private void MapCon_MouseDoubleClick(object sender, MouseEventArgs e)

        {
            // If a spawn point is found at location, select the spawn point.
            // else if mob is found at current location, select that mobs spawn
            // point if it exists, otherwise select the mob.
            if (Settings.Default.RangeCircle > 0)
            {
                float RangeCircleRadius = Settings.Default.RangeCircle;

                // Calc the proper loc for the mouse
                MouseMapLoc(e, out var mousex, out var mousey);

                // Range
                if (MouseDistance(mousex, mousey) < RangeCircleRadius)
                {
                    // if double click in range circle, turn it on/off
                    Settings.Default.ColorRangeCircle = !Settings.Default.ColorRangeCircle;
                }
            }
        }
    }
}