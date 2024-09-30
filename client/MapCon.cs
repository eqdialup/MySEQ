// Class Files

using myseq.Properties;
using Structures;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace myseq
{
    public class MapCon : UserControl
    {
        // Events
        public delegate void SelectPointHandler(Spawninfo playerinfo, double selectedX, double selectedY);

        private Timer tooltipTimer;

        public event SelectPointHandler SelectPoint; // Fires when the user clicks the map (without a mob)

        protected void OnSelectPoint(Spawninfo playerinfo, double selectedX, double selectedY) => SelectPoint?.Invoke(playerinfo, selectedX, selectedY);

        private readonly System.ComponentModel.Container components;

        private Label MobInfoLabel;

        private Font drawFont = Settings.Default.MapLabel;
        private Font drawFont1 = new Font(Settings.Default.MapLabel.Name, Settings.Default.MapLabel.Size * 0.9f, Settings.Default.MapLabel.Style);
        private Font drawFont3 = new Font(Settings.Default.MapLabel.Name, Settings.Default.MapLabel.Size * 1.1f, Settings.Default.MapLabel.Style);

        // Hand relocation variables

        private Cursor CursorHand;

        private bool MouseDragging;

        private bool m_rangechange;

        private float m_dragStartX;

        private float m_dragStartY;

        private float m_dragStartPanX;

        private float m_dragStartPanY;

        private bool canShowTooltip = true;

        private int skittle;

        private int flash_count;

        // m_mapCenter - centre point of screen in Map Units.
        private PointF mapCenter;

        // screenCenter - centre point of screen in Screen Units.
        private PointF screenCenter;

        private PointF adjustment;
        private PointF gamerPos;

        // Spawn Sizes
        private int SettingsSpawnSize = 3;

        private float SpawnSize = 5.0f;

        private float SpawnSizeOffset = 2.5f;

        private float SpawnPlusSize = 7.0f;

        private float PlusSzOZ = 3.5f;

        private float SelectSize = 9.0f;

        private float SelectSizeOffset = 4.5f;

        public float scale = 1.0f;

        public float MinmapX { get; set; } = -1000;
        public float MaxMapX { get; set; } = 1000;
        public float MinMapY { get; set; } = -1000;
        public float MaxMapY { get; set; } = 1000;

        private int filterpos;

        private int filterneg;

        private PointF selectedPoint = new PointF(-1, -1);// [42!] Mark an arbitrary spot on the map

        private string curTarget = "";

        private readonly BufferedGraphicsContext gfxManager;

        private BufferedGraphics bkgBuffer;

        private ToolTip toolTip;

        private bool flash; // used for flashing warning lights

        private SolidBrush textBrush;

        private readonly Pen PCBorder = new Pen(new SolidBrush(Settings.Default.PCBorderColor));

        private MainForm f1;          // Caution: this may be null

        private MapPane mapPane;     // Caution: this may be null

        private EQData eq;
        private EQMap map;
        private MapData mapData;
        private SpawnColors conColor;

        private int fpsCount;

        private TableLayoutPanel tableLayoutPanel1;

        public Label lblGameClock;

        private bool ShowPCName = Settings.Default.ShowPCNames;

        private float[] xSin = new float[512];

        private float[] xCos = new float[512];

        public int UpdateSteps { get; set; } = 5;

        public int UpdateTicks { get; set; } = 1;
        public float PanOffsetX { get; set; }
        public float PanOffsetY { get; set; }
        public float Ratio { get; set; } = 1.0f;

        private Stopwatch fpsStopwatch = new Stopwatch();
        private double fpsValue;

        public MapCon()
        {
            InitializeComponent();
            InitializeVariables();
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            gfxManager = BufferedGraphicsManager.Current;
        }

        public void SetComponents(MainForm f1, MapPane mapPane, EQData eq, EQMap map, MapData mapData)
        {
            this.f1 = f1;
            this.mapPane = mapPane;
            this.eq = eq;
            conColor = eq.spawnColor;
            this.map = map;
            this.mapData = mapData;
            map.EnterMap += MapChanged;

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
            // Override Method intentionally left empty.
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (bkgBuffer != null)
            {
                bkgBuffer.Render(e.Graphics);

                base.OnPaint(e);

                CalculateFPS();
                f1.toolStripFPS.Text = $"FPS: {fpsValue}";
            }
        }

        public void Tick()
        {
            // Increment animation and flash counters
            skittle++;
            flash_count++;

            // Re-adjust the screen/map view (optional: call less frequently if performance is an issue)
            ReAdjust();

            // Reset skittle animation step if it exceeds the number of update steps
            if (skittle >= UpdateSteps) // Consider using >= for clarity and to avoid off-by-one issues
            {
                skittle = 0;
            }

            // Toggle flash status when the flash count reaches the update tick threshold
            if (flash_count >= UpdateTicks)
            {
                flash_count = 0;  // Reset flash count
                flash = !flash;   // Toggle the flash boolean
            }
        }

        public void ClearPan()
        {
            PanOffsetX = 0;
            PanOffsetY = 0;
            ReAdjust();
        }

        private void CalculateFPS()
        {
            if (!fpsStopwatch.IsRunning)
            {
                // Start the stopwatch if it's not already running
                fpsStopwatch.Start();
            }

            // Check if the elapsed time exceeds the threshold (0.5 seconds)
            if (fpsStopwatch.Elapsed.TotalSeconds > 0.5)
            {
                // Calculate FPS
                fpsValue = Math.Round(fpsCount / fpsStopwatch.Elapsed.TotalSeconds, 2);

                // Reset for the next interval
                fpsStopwatch.Restart();
                fpsCount = 0;
            }
            else
            {
                // Increment frame count
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
            MobInfoLabel = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            lblGameClock = new Label();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            tooltipTimer = new System.Windows.Forms.Timer
            {
                Interval = 250 // 250 milliseconds = 0.25 seconds
            };
            tooltipTimer.Tick += TooltipTimer_Tick;
            tooltipTimer.Start();

            //
            // lblMobInfo
            //
            MobInfoLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
            | AnchorStyles.Left
            | AnchorStyles.Right;
            MobInfoLabel.BackColor = Color.White;
            MobInfoLabel.BorderStyle = BorderStyle.FixedSingle;
            MobInfoLabel.Font = Settings.Default.TargetInfoFont;
            MobInfoLabel.Location = new Point(0, 20);
            MobInfoLabel.Margin = new Padding(0);
            MobInfoLabel.Name = "lblMobInfo";
            MobInfoLabel.Size = new Size(163, 80);
            MobInfoLabel.TabIndex = 0;
            MobInfoLabel.Text = "Spawn Information Window";
            //
            // tableLayoutPanel1
            //
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(lblGameClock, 0, 0);
            tableLayoutPanel1.Controls.Add(MobInfoLabel, 0, 1);
            tableLayoutPanel1.Location = new Point(3, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
            tableLayoutPanel1.Size = new Size(163, 100);
            tableLayoutPanel1.TabIndex = 2;
            //
            // lblGameClock
            //
            lblGameClock.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
            | AnchorStyles.Left
            | AnchorStyles.Right;
            lblGameClock.BackColor = Color.BlueViolet;
            lblGameClock.BorderStyle = BorderStyle.FixedSingle;
            lblGameClock.Font = Settings.Default.TargetInfoFont;/// new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            lblGameClock.ForeColor = Color.White;
            lblGameClock.Location = new Point(0, 0);
            lblGameClock.Margin = new Padding(0);
            lblGameClock.Name = "lblGameClock";
            lblGameClock.Size = new Size(163, 20);
            lblGameClock.TabIndex = 2;
            lblGameClock.Text = "12:12 AM 1/01/0000";
            lblGameClock.TextAlign = ContentAlignment.MiddleCenter;
            //
            // MapCon
            //
            AutoScroll = true;
            BackColor = SystemColors.Control;
            Controls.Add(tableLayoutPanel1);
            Location = new Point(3, 3);
            Name = "MapCon";
            Size = new Size(227, 154);
            Paint += new PaintEventHandler(MapCon_Paint);
            KeyPress += new KeyPressEventHandler(MapCon_KeyPress);
            MouseDoubleClick += new MouseEventHandler(MapCon_MouseDoubleClick);
            MouseDown += new MouseEventHandler(MapCon_MouseDown);
            MouseMove += new MouseEventHandler(MapCon_MouseMove);
            MouseUp += new MouseEventHandler(MapCon_MouseUp);
            MouseWheel += new MouseEventHandler(MapCon_MouseScroll);
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        private void MapCon_KeyPress(object sender, KeyPressEventArgs e) => mapPane.MapCon_KeyPress(sender, e);

        #endregion Component Designer generated code

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

                        lblGameClock.Text = $"{eq.Gametime:MMM d, yyyy} {eq.Gametime:t}";

                        // Set the Spawn Size

                        if (SettingsSpawnSize != Settings.Default.SpawnDrawSize)
                        {
                            SetSpawnSizes();
                        }

                        // Used to help reduce the number of calls to improve speed

                        var pX = eq.GamerInfo.X;

                        var pY = eq.GamerInfo.Y;

                        var pZ = eq.GamerInfo.Z;

                        PointF playerF = new PointF
                        {
                            X = CalcScreenCoordX(pX),
                            Y = CalcScreenCoordY(pY)
                        };

                        var dx = ((PanOffsetX + screenCenter.X) / -Ratio) - mapCenter.X;

                        var dy = ((PanOffsetY + screenCenter.Y) / -Ratio) - mapCenter.Y;
                        GraphicsState tState = bkgBuffer.Graphics.Save();

                        bkgBuffer.Graphics.ScaleTransform(-Ratio, -Ratio);

                        bkgBuffer.Graphics.TranslateTransform(dx, dy);

                        DrawMapLines(DrawOpts);

                        bkgBuffer.Graphics.Restore(tState);

                        DrawMap(DrawOpts);

                        if ((DrawOpts & DrawOptions.SpawnTrails) != DrawOptions.None)
                        {
                            DrawSpawnTrails();
                        }

                        if (!eq.Zoning)
                        {
                            if ((DrawOpts & DrawOptions.Player) != DrawOptions.None)
                            {
                                DrawGamer(playerF, SpawnSize, SpawnSizeOffset, DrawOpts);
                            }
                            DrawSpawns(DrawOpts, pX, pY, pZ, playerF);
                            DrawGroundItems(DrawOpts, pZ);
                            DrawSpawntimers(DrawOpts);
                            SmoothMode();
                            DrawCorpses(DrawOpts, pZ);
                        }
                        else
                        {
                            DrawGamer(playerF, SpawnSize, SpawnSizeOffset, DrawOpts);
                        }

                        bkgBuffer.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                        // [42!] Draw a line to an arbitrary spot.
                        DrawDashLine(DrawOpts, playerF);

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

        private void InitializeVariables()
        {
            CursorHand = Cursors.Hand;

            // Initialize DragVariables to 0,0 and set semiphore false
            ResetDragState();
            PanOffsetX = PanOffsetY = 0;
            SetSelectedPoint(-1, -1);

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

        private void ResetDragState()
        {
            MouseDragging = false;
            m_rangechange = false;
            m_dragStartX = m_dragStartY = 0;
        }

        private void SetSelectedPoint(float x, float y)
        {
            selectedPoint.X = x;
            selectedPoint.Y = y;

            if (eq != null)
            {
                SelectPoint?.Invoke(eq.GamerInfo, x, y);
            }
        }

        public void Offset_X_ValueChanged(NumericUpDown offsetx)
        {
            PanOffsetX = -(int)offsetx.Value;
            ReAdjust();
            Invalidate();
        }

        public void Offset_Y_ValueChanged(NumericUpDown offsety)
        {
            PanOffsetY = -(int)offsety.Value;

            ReAdjust();
            Invalidate();
        }

        #region mouseops

        private void MapCon_MouseScroll(object sender, MouseEventArgs me)
        {
            if (mapPane == null)
            {
                return;
            }

            var newScale = scale + (me.Delta / 600.0f);

            if (newScale >= 0.1)
            {
                MapPane.scale.Value = (decimal)(newScale * 100);
            }

            ReAdjust();

            Invalidate();
        }

        private void MapCon_MouseDown(object sender, MouseEventArgs mouse)
        {
            if (mouse.Button == MouseButtons.Left)
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

                    MouseMapLoc(mouse, out var mousex, out var mousey);

                    // if within approximately one mob radius of the Range Circle
                    // then we are resizing range circle, and not dragging.

                    var sd = MouseDistance(mousex, mousey);

                    if (Settings.Default.AlertInsideRangeCircle && (sd > lowerRadius) && (sd < upperRadius))
                    {
                        // changing range cirlce size

                        m_rangechange = true;
                    }
                }

                if (!m_rangechange)

                {
                    Cursor.Current = CursorHand;

                    // Remember the mouse down loc and set semiphore

                    MouseDragging = true;

                    m_dragStartX = mouse.X;

                    m_dragStartY = mouse.Y;

                    // remember the original PanOffsets...

                    m_dragStartPanX = PanOffsetX;

                    m_dragStartPanY = PanOffsetY;
                }
            }
            else if (mouse.Button == MouseButtons.Right)
            {
                // right click of mouse.
                // see if there is a mob located there.
                RightMouseButton(mouse);
            }
        }

        private float MouseDistance(float mousex, float mousey)
        {
            return (float)Math.Sqrt(((mousey - eq.GamerInfo.Y) * (mousey - eq.GamerInfo.Y)) +

                ((mousex - eq.GamerInfo.X) * (mousex - eq.GamerInfo.X)));
        }

        private void MouseMapLoc(MouseEventArgs e, out float mousex, out float mousey)
        {
            mousex = mapCenter.X + ((PanOffsetX + screenCenter.X - e.X) / Ratio);
            mousey = mapCenter.Y + ((PanOffsetY + screenCenter.Y - e.Y) / Ratio);
        }

        private void RightMouseButton(MouseEventArgs e)
        {
            // Convert mouse coordinates to map coordinates.
            MouseMapLoc(e, out var mousex, out var mousey);

            //Proximity threshold.
            float delta = 5.0f / Ratio;

            // Try to find a mob at the clicked location.
            Spawninfo sp = eq.FindMob(mousex, mousey, delta, true, true, true);
            if (TryProcessEntity(sp)) return;

            // Try to find a ground item if no mob is found.
            GroundItem gi = eq.FindGroundItem(mousex, mousey, delta);
            if (TryProcessEntity(gi)) return;

            // Try to find a spawn timer if no ground item is found.
            Spawntimer st = eq.FindTimer(mousex, mousey, 5.0f);
            if (TryProcessEntity(st)) return;

            // Fallback: Try to find a general mob with less strict search criteria.
            sp = eq.FindMob(mousex, mousey, delta, true, true);
            if (TryProcessEntity(sp)) return;

            // If no entity is found, clear the alert and update context menu.
            ClearAlert();
            f1.SetContextMenu();
        }

        // Helper method to handle entity processing based on type.
        private bool TryProcessEntity(object entity)
        {
            switch (entity)
            {
                case Spawninfo sp when sp.Name.Length > 0:
                    f1.alertAddmobname = sp.Name.FilterMobName().Replace("_", " ").TrimEnd(' ');
                    SetAlertCoordinates(sp.X, sp.Y, sp.Z);
                    f1.SetContextMenu();
                    return true;

                case GroundItem gi when gi.Name.Length > 0:
                    f1.alertAddmobname = eq.GetItemDescription(gi.Name);
                    SetAlertCoordinates(gi.X, gi.Y, gi.Z);
                    f1.SetContextMenu();
                    return true;

                case Spawntimer st:
                    f1.alertAddmobname = GetSpawnTimerName(st);
                    SetAlertCoordinates(st.X, st.Y, st.Z);
                    f1.SetContextMenu();
                    return true;

                default:
                    return false;
            }
        }

        private void SetAlertCoordinates(float x, float y, float z)
        {
            f1.alertX = x;
            f1.alertY = y;
            f1.alertZ = z;
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
                    SetSelectedPoint(-1, -1);
                }
            }
            else if (e.X == m_dragStartX && e.Y == m_dragStartY)
            {
                // allow a small margin of error in coordinates
                // value of 5 screen units in terms of mapcoords
                // try to select mob, if not then do timer

                var delta = 5.0f / Ratio;
                if (!eq.SelectMob(mousex, mousey, delta) && !eq.SelectTimer(mousex, mousey, delta))
                {
                    eq.SelectGroundItem(mousex, mousey, delta);
                }

                Invalidate();
            }

            ResetDragState();
            Cursor.Current = Cursors.Default;
        }

        private void MapCon_MouseMove(object sender, MouseEventArgs e)
        {
            if (mapPane != null && f1 != null)
            {
                // Limit TT popups to four times a sec
                if (canShowTooltip)
                {
                    // Show the tooltip
                    canShowTooltip = false;
                    tooltipTimer.Start(); // Start the timer to reset the flag after the interval
                }

                // Calc the proper loc for the mouse
                MouseMapLoc(e, out var mousex, out var mousey);

                // Range
                var sd = MouseDistance(mousex, mousey);

                f1.toolStripMouseLocation.Text = $"Map /loc: {mousey:f2}, {mousex:f2}";
                f1.toolStripDistance.Text = $"Distance: {sd:f1}";

                // If we are dragging, then change the origin.

                if (MouseDragging)
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

        private string GetSpawnTimerName(Spawntimer st)
        {
            foreach (var name in st.AllNames.Split(','))
            {
                var trimmedName = name.TrimName();
                if (trimmedName.Length > 0)
                {
                    if (trimmedName.RegexMatch())
                    {
                        return trimmedName;
                    }

                    if (string.IsNullOrEmpty(f1.alertAddmobname))
                    {
                        f1.alertAddmobname = trimmedName;
                    }
                }
            }

            return f1.alertAddmobname;
        }

        private void ClearAlert()
        {
            f1.alertAddmobname = "";
            f1.alertX = 0.0f;
            f1.alertY = 0.0f;
            f1.alertZ = 0.0f;
        }

        private void MapCon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // if double click in range circle, turn it on/off
            MouseMapLoc(e, out var mousex, out var mousey);
            if (MouseDistance(mousex, mousey) < Settings.Default.RangeCircle)
            {
                Settings.Default.AlertInsideRangeCircle = !Settings.Default.AlertInsideRangeCircle;
            }
        }

        #endregion mouseops

        #region MapMath

        public void ReAdjust()
        {
            // Calculate the dimensions of the map
            float mapWidth = Math.Abs(MaxMapX - MinmapX);
            float mapHeight = Math.Abs(MaxMapY - MinMapY);

            // Calculate screen dimensions with a padding of 30
            float screenWidth = Width - 30;
            float screenHeight = Height - 30;

            // Calculate screen center only once
            screenCenter = new PointF(Width / 2f, Height / 2f);

            // Calculate the smaller ratio to fit the map within the screen
            float xRatio = screenWidth / mapWidth;
            float yRatio = screenHeight / mapHeight;
            Ratio = xRatio < yRatio ? xRatio * scale : yRatio * scale;

            // Determine the map center based on the follow option
            UpdateMapCenter(mapWidth, mapHeight);

            // Adjust for follow options and apply offsets
            AdjustWhileFollow(mapWidth, mapHeight);
            adjustment.X = PanOffsetX + screenCenter.X + (mapCenter.X * Ratio);
            adjustment.Y = PanOffsetY + screenCenter.Y + (mapCenter.Y * Ratio);
        }

        internal int SetFilterPos(int value) => filterneg = value;

        internal int SetFilterNeg(int value) => filterpos = value;

        private void UpdateMapCenter(float mapWidth, float mapHeight)
        {
            switch (Settings.Default.FollowOption)
            {
                case FollowOption.None:
                    // Center of the map
                    mapCenter.X = MinmapX + (mapWidth / 2);
                    mapCenter.Y = MinMapY + (mapHeight / 2);
                    break;

                case FollowOption.Player:
                    // Follow player coordinates
                    mapCenter.X = eq.GamerInfo.X;
                    mapCenter.Y = eq.GamerInfo.Y;
                    break;

                case FollowOption.Target:
                    // Follow target coordinates if available
                    Spawninfo target = eq.GetSelectedMob();
                    if (target != null)
                    {
                        mapCenter.X = target.X;
                        mapCenter.Y = target.Y;
                    }
                    break;
            }
        }

        private void AdjustWhileFollow(float mapWidth, float mapHeight)
        {
            if (!Settings.Default.KeepCentered && Settings.Default.FollowOption != FollowOption.None)
            {
                // Calculate screen edges in map coordinates.
                float screenMinY = ScreenToMapCoordY(Height - 15);
                float screenMaxY = ScreenToMapCoordY(15);
                float screenMapHeight = Math.Abs(screenMaxY - screenMinY);

                float screenMinX = ScreenToMapCoordX(Width - 15);
                float screenMaxX = ScreenToMapCoordX(15);
                float screenMapWidth = Math.Abs(screenMaxX - screenMinX);

                // Center map horizontally if it fits within screen width.
                if (mapWidth <= screenMapWidth)
                {
                    mapCenter.X = MinmapX + (mapWidth / 2);
                }
                else
                {
                    // Adjust the X center point to avoid blank space.
                    AdjustCenterX(screenMinX, screenMaxX);
                }

                // Center map vertically if it fits within screen height.
                if (mapHeight <= screenMapHeight)
                {
                    mapCenter.Y = MinMapY + (mapHeight / 2);
                }
                else
                {
                    // Adjust the Y center point to avoid blank space.
                    AdjustCenterY(screenMinY, screenMaxY);
                }
            }
        }

        private void AdjustCenterX(float screenMinX, float screenMaxX)
        {
            // Adjust map center X based on screen boundaries.
            if (screenMinX < MinmapX)
            {
                mapCenter.X += MinmapX - screenMinX;
            }
            else if (screenMaxX > MaxMapX)
            {
                mapCenter.X -= screenMaxX - MaxMapX;
            }
        }

        private void AdjustCenterY(float screenMinY, float screenMaxY)
        {
            // Adjust map center Y based on screen boundaries.
            if (screenMinY < MinMapY)
            {
                mapCenter.Y += MinMapY - screenMinY;
            }
            else if (screenMaxY > MaxMapY)
            {
                mapCenter.Y -= screenMaxY - MaxMapY;
            }
        }

        public float CalcScreenCoordX(float mapCoordinateX) => adjustment.X - (mapCoordinateX * Ratio);

        // Formula Should be
        // Screen X =CenterScreenX + ((mapCoordinateX - MapCenterX) * m_ratio)

        // However Eq's Map coordinates are in the oposite sense to the screen
        // so we have to multiply the second portion by -1, which is the same
        // as changing the plus to a minus...

        //m_ratio = (ScreenWidth/MapWidth) * zoom (Calculated ahead of time in ReAdjust)

        public float CalcScreenCoordY(float mapCoordinateY) => adjustment.Y - (mapCoordinateY * Ratio);

        private float ScreenToMapCoordX(float screenCoordX) => mapCenter.X + ((screenCenter.X - screenCoordX) / Ratio);

        private float ScreenToMapCoordY(float screenCoordY) => mapCenter.Y + ((screenCenter.Y - screenCoordY) / Ratio);

        private void MapChanged(EQMap map)
        {
            DrawOptions DrawOpts = f1.DrawOpts;

            // if the autoexpand is not checked, scale is not at 100, then maintain the map scale
            if (eq.Longname.Length > 0 && mapPane != null && !Settings.Default.AutoExpand &&
                MapPane.scale.Value != 100)
            {
                GetRatioAndSetScale();
                ClearPan();
            }
            else if (Settings.Default.KeepCentered)
            {
                ClearPan();
            }
            else
            {
                MapPane.scale.Value = 100M;
                SetScale_1();
                ClearPan();
            }

            // check that map text doesn't change extents

            if ((DrawOpts & DrawOptions.ZoneText) != DrawOptions.None)
            {
                VerifyTextExtents(DrawOpts);
            }

            Invalidate();
        }

        private void GetRatioAndSetScale()
        {
            var mapWidth = Math.Abs(MaxMapX - MinmapX);
            var mapHeight = Math.Abs(MaxMapY - MinMapY);

            var screenWidth = Width - 30;  // 2 * 15
            var screenHeight = Height - 30; // 2 * 15

            var ratio = Math.Min(screenWidth / mapWidth, screenHeight / mapHeight);

            SetScale(ratio);
        }

        private void SetScale(float ratio)
        {
            scale = ratio > 0.0f ? Ratio / ratio : 1.0f;

            if (scale < 0.1f)
            {
                SetScale_1();
            }

            MapPane.scale.Value = (decimal)(Math.Round(scale, 1) * 100);
        }

        public void SetScale_1() => scale = 1.0f;

        private void VerifyTextExtents(DrawOptions drawOpts)
        {
            float xlabelOffset = 0, ylabelOffset = 0;
            float factor = 1 / Ratio; // Scaling factor based on the map's zoom ratio.

            // Adjust for grid line labels if enabled
            if ((drawOpts & DrawOptions.GridLines) != 0)
            {
                ylabelOffset = drawFont.GetHeight(); // Height of the text font
                xlabelOffset = bkgBuffer.Graphics.MeasureString("10000", drawFont).Width; // Example width for labels
            }

            foreach (MapLabel label in mapData.Labels)
            {
                SizeF textSize = bkgBuffer.Graphics.MeasureString(label.Text, drawFont);

                float labelMinX = label.Position.X - ((textSize.Width + xlabelOffset) * factor);
                MinmapX = Math.Min(MinmapX, labelMinX);
                MaxMapX = Math.Max(MaxMapX, label.Position.X);

                float labelMinY = label.Position.Y - ((textSize.Height + ylabelOffset) * factor);
                MinMapY = Math.Min(MinMapY, labelMinY);
                MaxMapY = Math.Max(MaxMapY, label.Position.Y);
            }
            ReAdjust();
        }

        #endregion MapMath

        #region DrawShapes

        private void DrawCross(Pen pen, PointF center, float offset)
        {
            // Calculate start and end points for each line of the cross
            PointF left = new PointF(center.X - offset, center.Y);
            PointF right = new PointF(center.X + offset, center.Y);
            PointF top = new PointF(center.X, center.Y - offset);
            PointF bottom = new PointF(center.X, center.Y + offset);

            // Draw the cross
            bkgBuffer.Graphics.DrawLine(pen, left, right);
            bkgBuffer.Graphics.DrawLine(pen, top, bottom);
        }

        private void DrawBigX(Pen pen, PointF drawPoint, float offset)
        {
            // Diagonal lines forming the "X"
            bkgBuffer.Graphics.DrawLine(pen, drawPoint.X - offset, drawPoint.Y - offset, drawPoint.X + offset, drawPoint.Y + offset);
            bkgBuffer.Graphics.DrawLine(pen, drawPoint.X - offset, drawPoint.Y + offset, drawPoint.X + offset, drawPoint.Y - offset);
        }

        private void DrawLine(Pen pen, float x1, float y1, float x2, float y2)
        {
            try
            {
                bkgBuffer.Graphics.DrawLine(pen, x1, y1, x2, y2);
            }
            catch (Exception ex) { LogLib.WriteLine($"Error with DrawLine({x1}, {y1}, {x2}, {y2}): ", ex); }
        }

        private void DrawLines(Color pen, Point3D start, Point3D end)
        {
            try
            {
                bkgBuffer.Graphics.DrawLine(new Pen(pen), start.X, start.Y, end.X, end.Y);
            }
            catch (Exception ex) { LogLib.WriteLine("Error with DrawLines: ", ex); }
        }

        private void FillEllipse(Brush brush, float x1, float y1, float width, float height)
        {
            try
            {
                bkgBuffer.Graphics.FillEllipse(brush, x1, y1, width, height);
            }
            catch (Exception ex) { LogLib.WriteLine($"Error with Fill Ellipse({x1}, {y1}, {width}, {height}): ", ex); }
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
        {
            // Defining constants for better readability
            const float cos30 = 0.866025f; // Approximation of sqrt(3) / 2
            const float sin30 = 0.5f;      // Sine of 30 degrees

            return new PointF[]
            {
        new PointF(x1 + (radius * cos30), y1 + (radius * sin30)),  // Right vertex
        new PointF(x1 - (radius * cos30), y1 + (radius * sin30)),  // Left vertex
        new PointF(x1, y1 - radius)                                // Top vertex
            };
        }

        private void FillRectangle(Brush brush, float x1, float y1, float width, float height)
        {
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
            try
            {
                if (pen is null) return;
                else
                    bkgBuffer.Graphics.DrawRectangle(pen, x1, y1, width, height);
            }
            catch (Exception ex) { LogLib.WriteLine($"Error with DrawRectangle({x1}, {y1}, {width}, {height}): ", ex); }
        }

        private void DrawDashLine(DrawOptions drawOpts, PointF player)
        {
            if (selectedPoint.X == -1 || (drawOpts & DrawOptions.SpotLine) == DrawOptions.None)
                return;

            var endX = CalcScreenCoordX(selectedPoint.X);
            var endY = CalcScreenCoordY(selectedPoint.Y);

            DrawLine(new Pen(new SolidBrush(Color.White))
            {
                DashStyle = DashStyle.Dash,
                DashPattern = new float[] { 8, 4 }
            }, player.X, player.Y, endX, endY);
        }

        #endregion DrawShapes

        #region TooltipWindows

        private void TooltipTimer_Tick(object sender, EventArgs e)
        {
            canShowTooltip = true;
            tooltipTimer.Stop(); // Stop the timer until the next tooltip is shown
        }

        private void PopulateToolTip(MouseEventArgs e)
        {
            MouseMapLoc(e, out var mousex, out var mousey);
            var delta = 5.0f / Ratio;

            Spawninfo sp = eq.FindMob(mousex, mousey, delta, true) ?? eq.FindMob(mousex, mousey, delta);

            bool found;
            if (sp == null)
            {
                found = false;
            }
            else
            {
                found = true;
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
                Spawntimer st = eq.MobsTimers.Find(delta, mousex, mousey);
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
                found = true;
            }

            return found;
        }

        private string MobInfo(Spawninfo si, bool SetColor, bool ChangeSize)
        {
            if (f1 == null)
            { return ""; }

            if (si == null)
            {
                return NoSpawnInfo(ChangeSize);
            }

            StringBuilder mobInfo = SpawnInfoWindow(si);

            if (SetColor)
            {
                InfoSetColor(si);
            }

            tableLayoutPanel1.ColumnStyles[0].SizeType = SizeType.Absolute;
            return MobshowInfo(ChangeSize, mobInfo);
        }

        private string NoSpawnInfo(bool ChangeSize)
        {
            if (ChangeSize)
            {
                tableLayoutPanel1.RowStyles[0].SizeType = SizeType.Absolute;

                tableLayoutPanel1.RowStyles[1].SizeType = SizeType.Absolute;

                if (Settings.Default.ShowTargetInfo)
                {
                    MeasureStrings("Spawn Information Window", out SizeF gt, out SizeF gf);

                    tableLayoutPanel1.ColumnStyles[0].SizeType = SizeType.Absolute;

                    tableLayoutPanel1.Width = (int)gf.Width + 40;
                    tableLayoutPanel1.ColumnStyles[0].Width = (int)gf.Width + 40;

                    tableLayoutPanel1.RowStyles[0].Height = (int)gt.Height + 7;
                }
            }
            MobInfoLabel.BackColor = Color.White;
            return "Spawn Information Window";
        }

        private void MeasureStrings(string label, out SizeF gt, out SizeF gf)
        {
            Graphics graphics = MobInfoLabel.CreateGraphics();

            gt = graphics.MeasureString(lblGameClock.Text, lblGameClock.Font);
            gf = graphics.MeasureString(label, MobInfoLabel.Font);
            graphics.Dispose();
        }

        private string MobshowInfo(bool ChangeSize, StringBuilder mobInfo)
        {
            MeasureStrings(lblGameClock.Text, out SizeF sc, out SizeF sf);

            tableLayoutPanel1.RowStyles[0].SizeType = SizeType.Absolute;
            tableLayoutPanel1.RowStyles[1].SizeType = SizeType.Absolute;
            if (Settings.Default.ShowTargetInfo)
            {
                MobInfoLabel.Visible = true;

                if (ChangeSize)
                {
                    var panel_width = sc.Width > sf.Width ? (int)sc.Width : (int)sf.Width;
                    tableLayoutPanel1.Width = panel_width + (Settings.Default.SmallTargetInfo ? 40 : 10);
                    tableLayoutPanel1.ColumnStyles[0].Width = panel_width + (Settings.Default.SmallTargetInfo ? 40 : 70);
                    tableLayoutPanel1.RowStyles[0].Height = (int)sc.Height + 7;
                    tableLayoutPanel1.RowStyles[1].Height = (int)sf.Height + (Settings.Default.SmallTargetInfo ? 40 : 120);
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
            if (si.Level < (conColor.GreyRange + eq.GamerInfo.Level))
            {
                MobInfoLabel.BackColor = Color.LightGray;
            }
            else if (si.Level < (conColor.GreenRange + eq.GamerInfo.Level))
            {
                MobInfoLabel.BackColor = Color.PaleGreen;
            }
            else if (si.Level < (conColor.CyanRange + eq.GamerInfo.Level))
            {
                MobInfoLabel.BackColor = Color.PowderBlue;
            }
            else if (si.Level < eq.GamerInfo.Level)
            {
                MobInfoLabel.BackColor = Color.DeepSkyBlue;
            }
            else if (si.Level == eq.GamerInfo.Level)
            {
                MobInfoLabel.BackColor = Color.White;
            }
            else
            {
                MobInfoLabel.BackColor = si.Level <= eq.GamerInfo.Level + conColor.YellowRange ? Color.Yellow : Color.Red;
            }

            if (si.isEventController)
            {
                MobInfoLabel.BackColor = Color.Violet;
            }

            if (si.isLDONObject)
            {
                MobInfoLabel.BackColor = Color.LightGray;
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

            mobInfo.AppendFormat("Visibility: {0}\n", si.Hide.GetHideStatus());

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

            mobInfo.AppendFormat("Level {0} / {1}\n", si.Level.ToString(), si.Hide.GetHideStatus());

            mobInfo.AppendFormat("{0} / {1}\n", eq.GetRace(si.Race), eq.GetClass(si.Class));

            mobInfo.AppendFormat("Speed: {0:f3}  Dist: {1:f0}\n", si.SpeedRun, SpawnDistance(si));

            mobInfo.AppendFormat("Y: {0:f1} X: {1:f1} Z: {2:f1}", si.Y, si.X, si.Z);
        }

        public void ResetInfoWindow()
        {
            MobInfoLabel.Text = "Spawn Information Window";

            MobInfoLabel.BackColor = Color.White;

            MobInfoLabel.Visible = true;
        }

        private float SpawnDistance(Spawninfo si)
        {
            var dx = si.X - eq.GamerInfo.X;
            var dy = si.Y - eq.GamerInfo.Y;
            var dz = si.Z - eq.GamerInfo.Z;

            return (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        #endregion TooltipWindows

        #region DrawSpawns

        private bool NPCDepthFilter = Settings.Default.DepthFilter && Settings.Default.FilterNPCs;
        private bool FilterPlayers = Settings.Default.DepthFilter && Settings.Default.FilterPlayers;

        private void DrawSpawns(DrawOptions DrawOpts, float pX, float pY, float pZ, PointF player)
        {
            if ((DrawOpts & DrawOptions.Spawns) != DrawOptions.None)
            {
                SolidBrush WhiteBrush = new SolidBrush(Color.White);
                //            string gName;
                var ShowRings = (DrawOpts & DrawOptions.SpawnRings) != DrawOptions.None;

                var DrawDirection = (DrawOpts & DrawOptions.DirectionLines) != DrawOptions.None;
                var colorRangeCircle = Settings.Default.AlertInsideRangeCircle;
                if ((eq.SelectedID == 99999) && (eq.SpawnX == -1))
                {
                    MobInfoLabel.Text = MobInfo(null, true, true);
                }

                // Collect mob trails, every 8th pass - approx once every 1 sec
                map.trails.CountMobTrails(eq);
                // Draw Spawns

                foreach (Spawninfo sp in eq.GetMobsReadonly().Values)
                {
                    var sPoint = new PointF(
                    (float)Math.Round(CalcScreenCoordX(sp.X), 0),
                    (float)Math.Round(CalcScreenCoordY(sp.Y), 0));
                    //                gName = eq.GuildNumToString(sp.Guild);
                    // Draw Line from Gamer to the Selected Spawn

                    if (eq.SelectedID == sp.SpawnID)
                    {
                        LineGamerToSelected(player.X, player.Y, sp, sPoint.X, sPoint.Y);
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

                    if (sp.flags == 0 && sp.Name.Length > 0)
                    {
                        if (curTarget == sp.Name)
                        {
                            FillRectangle(WhiteBrush, sPoint.X - PlusSzOZ - 1, sPoint.Y - PlusSzOZ - 1, SpawnPlusSize + 2, SpawnPlusSize + 2);
                        }

                        if (sp.isEventController)
                        {
                            DrawSpecialMobs(pZ, sp, sPoint.X, sPoint.Y, Color.Purple);
                        }
                        else if (sp.isLDONObject)
                        {
                            DrawSpecialMobs(pZ, sp, sPoint.X, sPoint.Y, Color.Gray);
                        }
                        else if (sp.Type == 0)
                        {
                            // Draw Other Players

                            DrawOtherPlayers(DrawOpts, pZ, sp, sPoint.X, sPoint.Y);
                        }
                        else if (sp.Type == 1 || sp.Type == 4)
                        {
                            DrawNPCs(pZ, DrawDirection, sp, sPoint.X, sPoint.Y);
                        }
                        DrawRings(sPoint.X, sPoint.Y, sp);
                        DrawFlashes(pZ,sPoint.X, sPoint.Y, sp);
                        MarkSpecial(pZ, sPoint.X, sPoint.Y, ShowRings, sp);
                    }
                }
            }
        }

        internal Font GetdrawFont() => drawFont;

        internal void SetDrawFont(Font newfont)
        {
            drawFont = newfont;
            if (drawFont != null)
            {
                drawFont1 = new Font(drawFont.Name, drawFont.Size * 0.9f, drawFont.Style);
                drawFont3 = new Font(drawFont.Name, drawFont.Size * 1.1f, drawFont.Style);
            }
        }

        private void DrawNPCs(float pZ, bool DrawDirection, Spawninfo sp, float x, float y)
        {
            if ((!Settings.Default.DepthFilter && Settings.Default.FilterNPCs) || IsWithinDepthFilter(sp.Z, pZ))
            {
                SolidBrush GrayBrush = new SolidBrush(Color.Gray);
                Pen purplePen = new Pen(new SolidBrush(Color.Purple));
                sp.filtered = false;

                if (sp.Name.Length > 0)
                {
                    if (Settings.Default.ShowNPCNames)
                    {
                        DrawSpawnNames(textBrush, sp.Name, sp.X, sp.Y);//, gName);
                    }
                    if (Settings.Default.ShowNPCLevels)
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
                else if (SpawnColors.ConColors[sp.Level] != null)
                {
                    FillEllipse(SpawnColors.ConColors[sp.Level], x - SpawnSizeOffset, y - SpawnSizeOffset, SpawnSize, SpawnSize);
                }

                // Draw PC color border around Mercenary

                if (sp.isMerc)
                {
                    DrawEllipse(PCBorder, x - SpawnSizeOffset, y - SpawnSizeOffset, SpawnSize, SpawnSize);
                }

                // Draw Purple border around invis mobs

                PurpleBorder(sp, x, y, purplePen);
            }
            else
            {
                sp.filtered = true;
            }
        }

        private void PurpleBorder(Spawninfo sp, float x, float y, Pen purplePen)
        {
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

        private void DrawOtherPlayers(DrawOptions DrawOpts, float pZ, Spawninfo sp, float x, float y)
        {
            if (!FilterPlayers || IsWithinDepthFilter(sp.Z, pZ))
            {
                sp.filtered = false;
                if (SpawnColors.ConColors[sp.Level] != null)
                {
                    if ((DrawOpts & DrawOptions.DirectionLines) != DrawOptions.None)
                    {
                        DrawDirectionLines(sp, x, y);
                    }

                    // Draw Other Players

                    if (Settings.Default.ShowPVP)
                    {
                        if ((Math.Abs(eq.GamerInfo.Level - sp.Level) <= Settings.Default.PVPLevels) || (Settings.Default.PVPLevels == -1))
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
            FillRectangle(SpawnColors.ConColors[sp.Level], x - PlusSzOZ + 0.5f, y - PlusSzOZ + 0.5f, SpawnPlusSize, SpawnPlusSize);

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
            // Draw the filled triangle and border
            FillTriangle(SpawnColors.ConColors[sp.Level], x, y, SelectSizeOffset);
            DrawTriangle(PCBorder, x, y, SelectSizeOffset);

            // Handle name drawing based on settings
            if (ShowPCName && !string.IsNullOrEmpty(sp.Name))
            {
                DrawSpawnNames(textBrush, $"{sp.Level}: {sp.Name}", sp.X, sp.Y);
            }
            else if (Settings.Default.ShowPVPLevel)
            {
                DrawSpawnNames(textBrush, sp.Level.ToString(), sp.X, sp.Y);
            }

            // Draw flashing ellipse if enabled
            if (flash)
            {
                using (Pen cPen = new Pen(eq.GetDistinctColor(Color.White)))
                {
                    DrawEllipse(cPen, x - SelectSizeOffset, y - SelectSizeOffset, SelectSize, SelectSize);
                }
            }
        }

        private void DrawCorpses(DrawOptions drawOpts, float pZ)
        {
            if ((drawOpts & DrawOptions.Spawns) == DrawOptions.None)
            {
                return; // No need to proceed if spawns shouldn't be drawn
            }

            var pcCorpseDepthFilter = Settings.Default.DepthFilter && Settings.Default.FilterPlayerCorpses;
            var npcCorpseDepthFilter = Settings.Default.DepthFilter && Settings.Default.FilterNPCCorpses;

            // Iterate through all spawns
            foreach (Spawninfo sp in eq.GetMobsReadonly().Values)
            {
                if (!sp.isCorpse || sp.hidden)
                {
                    continue; // Skip non-corpse or hidden spawns
                }

                var corpsePoint = new PointF(
                            (float)Math.Round(CalcScreenCoordX(sp.X), 0),
                            (float)Math.Round(CalcScreenCoordY(sp.Y), 0));

                sp.proxAlert = false;

                if (sp.IsPlayer)
                {
                    HandleCorpseDrawing(sp, corpsePoint, pcCorpseDepthFilter, pZ, Color.Yellow, Settings.Default.ShowPlayerCorpseNames);
                }
                else
                {
                    HandleCorpseDrawing(sp, corpsePoint, npcCorpseDepthFilter, pZ, Color.Cyan, Settings.Default.ShowNPCCorpseNames);
                }
            }
        }

        private void HandleCorpseDrawing(Spawninfo sp, PointF corpsePoint, bool depthFilter, float pZ, Color color, bool showNames)
        {
            if (depthFilter && !IsWithinDepthFilter(sp.Z, pZ))
            {
                sp.filtered = true;
                return;
            }

            sp.filtered = false;
            DrawCorpseShape(corpsePoint, color);

            if (showNames && !string.IsNullOrEmpty(sp.Name))
            {
                DrawSpawnNames(textBrush, $"{sp.Level}: {sp.Name}", sp.X, sp.Y);
            }
        }

        private bool IsWithinDepthFilter(float z, float pZ)
        {
            return z > pZ - filterneg && z < pZ + filterpos;
        }

        private bool IsWithinDepthFilter(float z, float minZ, float maxZ)
        {
            // Check if the Z value is within the specified depth range.
            return z >= minZ && z <= maxZ;
        }

        private void DrawCorpseShape(PointF corpsePoint, Color color)
        {
            using (Pen pen = new Pen(new SolidBrush(color)))
            {
                if (color == Color.Yellow)
                {
                    DrawRectangle(pen, corpsePoint.X - PlusSzOZ + 0.5f, corpsePoint.Y - PlusSzOZ + 0.5f, SpawnPlusSize, SpawnPlusSize);
                }
                else if (color == Color.Cyan)
                {
                    DrawCross(pen, corpsePoint, PlusSzOZ - 0.5f);
                }
            }
        }

        private void DrawSpecialMobs(float pZ, Spawninfo sp, float x, float y, Color color)
        {
            if (!NPCDepthFilter || IsWithinDepthFilter(sp.Z, pZ))
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
                MobInfoLabel.Text = MobInfo(sp, true, true);

                DrawLine(pinkPen, playerx, playery, x, y);
            }

            sp.proxAlert = false;
        }

        private void ProxAlert(float pX, float pY, float pZ, Spawninfo sp)
        {
            // Validate if the spawn should be alertable or if it's a non-alertable type.
            if (!sp.alertMob || sp.Type == 2)
            {
                sp.proxAlert = false;
                return;
            }

            // Determine the minimum level required for an alert.
            int minLevel = (Settings.Default.MinAlertLevel == -1)
                ? eq.GamerInfo.Level + conColor.GreyRange
                : Settings.Default.MinAlertLevel;

            // Exit early if the spawn level is below the minimum alert level.
            if (sp.Level < minLevel)
            {
                sp.proxAlert = false;
                return;
            }

            // Calculate the Z-axis range for proximity checks.
            float range = Settings.Default.RangeCircle;
            float adjustedMinZ = pZ - range;
            float adjustedMaxZ = pZ + range;

            // Apply a depth filter adjustment if required.
            if (NPCDepthFilter)
            {
                adjustedMinZ = pZ - filterpos;
                adjustedMaxZ = pZ + filterpos;
            }

            // Determine proximity state and handle enter or exit accordingly.
            if (sp.proxAlert)
            {
                HandleProximityChange(pX, pY, sp, adjustedMinZ, adjustedMaxZ, range, isEntering: false);
            }
            else
            {
                HandleProximityChange(pX, pY, sp, adjustedMinZ, adjustedMaxZ, range, isEntering: true);
            }
        }

        private void HandleProximityChange(float pX, float pY, Spawninfo sp, float minZ, float maxZ, float range, bool isEntering)
        {
            // Adjust range and depth multipliers based on whether we're handling entering or exiting.
            float rangeMultiplier = isEntering ? 1.0f : 1.4f;
            float depthMultiplier = isEntering ? 1.0f : 1.2f;

            // Check if the spawn is within the desired depth and range.
            if (IsWithinDepthFilter(sp.Z, minZ * depthMultiplier, maxZ * depthMultiplier) &&
                IsWithinRange(pX, pY, sp.X, sp.Y, range * rangeMultiplier))
            {
                if (isEntering)
                {
                    // Enter proximity: enable proximity alert and trigger sound settings.
                    sp.proxAlert = true;
                    new FormMethods().SwitchOnSoundSettings();
                }
            }
            else if (!isEntering)
            {
                // Exit proximity: disable proximity alert if leaving the range.
                sp.proxAlert = false;
            }
        }

        private bool IsWithinRange(float pX, float pY, float spX, float spY, float range)
        {
            // Calculate the squared distance and check against the squared range to avoid using sqrt.
            var distanceSquared = (pX - spX) * (pX - spX) + (pY - spY) * (pY - spY);
            return distanceSquared < (range * range);
        }

        private void MarkSpecial(float pZ, float x, float y, bool ShowRings, Spawninfo sp)
        {
            if (ShowRings && (!NPCDepthFilter || !IsWithinDepthFilter(sp.Z, pZ)))
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
            if (!sp.isLookup || (sp.isCorpse && !Settings.Default.CorpseAlerts))
            {
                return;
            }

            float lookupRingSize = SpawnPlusSize + (skittle / (float)UpdateSteps * SelectSize);
            DrawEllipse(new Pen(new SolidBrush(Color.White)), x - lookupRingSize / 2.0f, y - lookupRingSize / 2.0f, lookupRingSize, lookupRingSize);

            if (Settings.Default.ShowLookupText)
            {
                string displayName = Settings.Default.ShowLookupNumber ? sp.lookupNumber : sp.Name;
                DrawSpawnNames(textBrush, displayName, sp.X, sp.Y);
            }
        }

        public void DrawSpawnTrails()
        {
            try
            {
                // Draw Mob Trails only if there are any
                if (map.trails.GetMobTrailsReadonly().Any())
                {
                    foreach (MobTrailPoint mtp in map.trails.GetMobTrailsReadonly())
                    {
                        FillEllipse(new SolidBrush(Color.White), CalcScreenCoordX(mtp.X) - 2, CalcScreenCoordY(mtp.Y) - 2, 2, 2);
                    }
                }
            }
            catch (Exception ex)
            {
                LogLib.WriteLine("Error in DrawSpawnTrails(): ", ex);
            }
        }

        #endregion DrawSpawns

        #region DrawMap

        public void DrawMapLines(DrawOptions DrawOpts)
        {
            try
            {
                // Draw Zone Map
                if (eq.Longname != "" && ((DrawOpts & DrawOptions.DrawMap) != DrawOptions.None))
                {
                    if (!Settings.Default.DepthFilter || (Settings.Default.DepthFilter && !Settings.Default.FilterMapLines))
                    // No depth filtering
                    {
                        foreach (LineSegment mapLine in mapData.LineSegments)
                        {
                            DrawLines(mapLine.LineColor, mapLine.Start, mapLine.End);
                        }
                    }
                    else
                    {
                        MinMaxFilter(out var minZ, out var maxZ);

                        foreach (LineSegment mapLine in mapData.LineSegments)
                        {
                            // All the points in this set of lines are good
                            if (mapLine.Start.Z > minZ || mapLine.Start.Z < maxZ || mapLine.End.Z > minZ || mapLine.End.Z < maxZ)
                            {
                                DrawLines(mapLine.LineColor, mapLine.Start, mapLine.End);
                            }
                            else if (mapLine.Start.Z < minZ || mapLine.Start.Z > maxZ || mapLine.End.Z < minZ || mapLine.End.Z < maxZ)
                            {
                                DrawLines(mapLine.LineColor, mapLine.Start, mapLine.End);
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

        private void AlphaFiltering(float minZ, float maxZ, LineSegment mapLine)
        {
            bool curValid, lastValid;

            float curX, curY, curZ, lastX, lastY, lastZ;

            lastX = mapLine.Start.X;

            lastY = mapLine.Start.Y;

            lastZ = mapLine.Start.Z;

            lastValid = (lastZ > minZ) && (lastZ < maxZ);

            for (var d = 1; d < mapData.LineSegments.Count; d++)
            {
                curX = mapLine.Start.X;

                curY = mapLine.Start.Y;

                curZ = mapLine.Start.Z;

                curValid = (curZ > minZ) && (curZ < maxZ);

                // Original Depth Filter method (use z-axis values only)

                // instead of not drawing filtered lines, we draw light ones

                if (!curValid && !lastValid)
                {
                    if (Settings.Default.UseDynamicAlpha)
                    {
                        var alpha = Settings.Default.FadedLines * 255 / 100;
                        using (Pen Fade_color = new Pen(Color.FromArgb(alpha, mapLine.LineColor)))
                        { DrawLine(Fade_color, lastX, lastY, curX, curY); }
                    }
                }
                else
                {
                    DrawLine(new Pen(mapLine.LineColor), lastX, lastY, curX, curY);
                }

                lastX = curX;

                lastY = curY;

                lastValid = curValid;
            }
        }

        private void MinMaxFilter(out float minZ, out float maxZ)
        {
            minZ = eq.GamerInfo.Z - filterneg;
            maxZ = eq.GamerInfo.Z + filterpos;
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
            var gridInterval = Settings.Default.GridInterval;
            var curGridColor = Settings.Default.GridColor;
            var gridLabelColor = Settings.Default.GridLabelColor;

            using (var gridPen = new Pen(curGridColor))
            using (var gridBrush = new SolidBrush(gridLabelColor))
            {
                // Cache label font height for consistent spacing and avoid repeated method calls.
                float labelHeight = drawFont.GetHeight() + 5;
                DrawGridLinesInternal(gridInterval, gridPen, gridBrush, labelHeight, true);
                DrawGridLinesInternal(gridInterval, gridPen, gridBrush, labelHeight, false);
            }
        }

        private void DrawGridLinesInternal(int gridInterval, Pen gridPen, Brush gridBrush, float labelHeight, bool isHorizontal)
        {
            int start, end, screenLimit;
            Func<int, float> CalcScreenCoord;
            Func<int, string> GetLabelStr = label => label.ToString();

            if (isHorizontal)
            {
                // Calculate start and end for horizontal lines.
                start = (int)(MinmapX / gridInterval) - 2;
                end = (int)(MaxMapX / gridInterval) + 2;
                screenLimit = Height;
                CalcScreenCoord = label => (float)Math.Round(CalcScreenCoordX(label), 0);
            }
            else
            {
                // Calculate start and end for vertical lines.
                start = (int)(MinMapY / gridInterval) - 1;
                end = (int)(MaxMapY / gridInterval) + 2;
                screenLimit = Width;
                CalcScreenCoord = label => (float)Math.Round(CalcScreenCoordY(label), 0);
            }

            for (int i = start; i < end; i++)
            {
                int label = i * gridInterval;
                string labelStr = GetLabelStr(label);
                float screenCoord = CalcScreenCoord(label);

                if (isHorizontal)
                {
                    // Draw vertical grid lines for horizontal labels.
                    DrawLine(gridPen, screenCoord, 0, screenCoord, screenLimit);
                    DrawLabel(labelStr, gridBrush, screenCoord, screenLimit - labelHeight);
                }
                else
                {
                    // Draw horizontal grid lines for vertical labels.
                    DrawLine(gridPen, 0, screenCoord, screenLimit, screenCoord);
                    float labelWidth = bkgBuffer.Graphics.MeasureString(labelStr, drawFont).Width;
                    DrawLabel(labelStr, gridBrush, screenLimit - (labelWidth + 5), screenCoord);
                }
            }
        }

        private void DrawLabel(string text, Brush brush, float x, float y)
        {
            bkgBuffer.Graphics.DrawString(text, drawFont, brush, x, y);
        }

        private void DepthfilterText()
        {
            // Draw Zone Text
            if (Settings.Default.DepthFilter && Settings.Default.FilterMapText)
            {
                // Depth Filter
                MinMaxFilter(out var minZ, out var maxZ);

                foreach (MapLabel label in mapData.Labels)
                {
                    if (label.Position.Z != -99999 && label.Position.Z > minZ && label.Position.Z < maxZ)
                    {
                        AddTextToDrawnMap(label);
                    }
                }
            }
            else
            {
                // No Depth Filtering
                foreach (MapLabel text in mapData.Labels)
                {
                    AddTextToDrawnMap(text);
                }
            }
        }

        private void AddTextToDrawnMap(MapLabel label)
        {
            try
            {
                var x_cord = (int)CalcScreenCoordX(label.Position.X);
                var y_cord = (int)CalcScreenCoordY(label.Position.Y);
                if (Settings.Default.MapLabel.Size == 2)
                {// check for null
                    bkgBuffer.Graphics.DrawString(label.Text, drawFont, new SolidBrush(label.TextColor), x_cord, y_cord);
                }
                else if (Settings.Default.MapLabel.Size == 1)
                {
                    bkgBuffer.Graphics.DrawString(label.Text, drawFont1, new SolidBrush(label.TextColor), x_cord, y_cord);
                }
                else
                {
                    bkgBuffer.Graphics.DrawString(label.Text, drawFont3, new SolidBrush(label.TextColor), x_cord, y_cord);
                }
                using (Pen pen = new Pen(label.TextColor))
                {
                    bkgBuffer.Graphics.DrawLine(pen, x_cord - 1, y_cord, x_cord + 1, y_cord);
                    bkgBuffer.Graphics.DrawLine(pen, x_cord, y_cord - 1, x_cord, y_cord + 1);
                }
            }
            catch (Exception ex)
            {
                LogLib.WriteLine("Error in AddText" + ex.StackTrace, ex);
            }
        }

        #endregion DrawMap

        #region DrawGamer

        public void DrawGamer(PointF gamer, float SpawnSize, float SpawnSizeOffset, DrawOptions DrawOpts)
        {
            try
            {
                var xHead = (int)eq.GamerInfo.Heading;

                // Draw Range Circle

                if (Settings.Default.RangeCircle > 0)

                {
                    var rCircleRadius = Settings.Default.RangeCircle * Ratio;
                    MakeRangeCircle(gamer.X, gamer.Y, rCircleRadius);

                    // Draw Red V in the Range Circle

                    if (Settings.Default.DrawFoV && xHead >= 0 && xHead < 512)

                    {
                        DrawFoV(gamer.X, gamer.Y, xHead, rCircleRadius);
                    }

                    if (m_rangechange)

                    {
                        if (flash)
                        {
                            DrawEllipse(eq.GetDistinctColor(new Pen(Settings.Default.RangeCircleColor)), gamer.X - rCircleRadius, gamer.Y - rCircleRadius, rCircleRadius * 2, rCircleRadius * 2);
                        }
                    }
                    else
                    {
                        DrawEllipse(eq.GetDistinctColor(new Pen(Settings.Default.RangeCircleColor)), gamer.X - rCircleRadius, gamer.Y - rCircleRadius, rCircleRadius * 2, rCircleRadius * 2);
                    }
                }

                // Draw Player  (only if we actually have a player)

                if (eq.GamerInfo.SpawnID != 0)

                {
                    // Draw Player Heading Line

                    if ((DrawOpts & DrawOptions.DirectionLines) != DrawOptions.None && xHead >= 0 && xHead < 512)

                    {
                        var y1 = -(xCos[xHead] * (eq.GamerInfo.SpeedRun * Ratio * 100));

                        var x1 = -(xSin[xHead] * (eq.GamerInfo.SpeedRun * Ratio * 100));

                        DrawLine(new Pen(new SolidBrush(Color.White)), gamer.X, gamer.Y, gamer.X + x1, gamer.Y + y1);
                    }

                    FillRectangle(new SolidBrush(Color.White), gamer.X - SpawnSizeOffset, gamer.Y - SpawnSizeOffset, SpawnSize, SpawnSize);

                    DrawRectangle(PCBorder, gamer.X - SpawnSizeOffset - 0.5f, gamer.Y - SpawnSizeOffset - 0.5f, SpawnSize + 1.0f, SpawnSize + 1.0f);
                }
            }
            catch (Exception ex) { LogLib.WriteLine("Error in DrawPlayer(): ", ex); }
        }

        private void DrawFoV(float gamerX, float gamerY, int xHead, float rCircleRadius)
        {
            float x, y;

            if (xHead < 448)
            {
                y = -(xCos[xHead + 64] * rCircleRadius * 1.05f);
                x = -(xSin[xHead + 64] * rCircleRadius * 1.05f);
            }
            else
            {
                y = -(float)(xCos[xHead + 64 - 512] * rCircleRadius * 1.05);

                x = -(float)(xSin[xHead + 64 - 512] * rCircleRadius * 1.05);
            }

            DrawLine(new Pen(new SolidBrush(Color.Red)), gamerX, gamerY, gamerX + x, gamerY + y);

            if (xHead >= 64)

            {
                y = -(xCos[xHead - 64] * rCircleRadius * 1.05f);

                x = -(xSin[xHead - 64] * rCircleRadius * 1.05f);
            }
            else
            {
                y = -(xCos[xHead - 64 + 512] * rCircleRadius * 1.05f);

                x = -(xSin[xHead - 64 + 512] * rCircleRadius * 1.05f);
            }

            DrawLine(new Pen(new SolidBrush(Color.Red)), gamerX, gamerY, gamerX + x, gamerY + y);

            // Draw Heading Line

            y = -(xCos[xHead] * rCircleRadius);

            x = -(xSin[xHead] * rCircleRadius);

            DrawLine(new Pen(new SolidBrush(Color.Yellow)), gamerX, gamerY, gamerX + x, gamerY + y);
        }

        private void MakeRangeCircle(float gamerx, float gamery, float rCircleRadius)
        {
            if (Settings.Default.AlertInsideRangeCircle)
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

                    foreach (Spawntimer st in eq.MobsTimers.GetRespawned().Values)
                    {
                        if (st.zone == eq.Shortname)
                        {
                            PointF timerPoint = new PointF((float)Math.Round(CalcScreenCoordX(st.X), 0), (float)Math.Round(CalcScreenCoordY(st.Y), 0));

                            var stOffset = PlusSzOZ - 0.5f;

                            var checkTimer = st.SecondsUntilSpawn(DateTime.Now);

                            var canDraw = false;

                            CheckTimer(ref pen, checkTimer, ref canDraw);

                            // if depth filter on make adjustments to spawn points
                            canDraw = CheckDepthFilter(minZ, maxZ, st, canDraw);
                            if (canDraw)
                            {
                                DrawCross(pen, timerPoint, stOffset);

                                if (Settings.Default.SpawnCountdown && (checkTimer > 0) && (checkTimer < 120))
                                {
                                    DrawSpawnNames(textBrush, checkTimer.ToString(), st.X, st.Y);
                                }
                            }

                            // Draw Blue Line to selected spawn location

                            if ((st.X == eq.SpawnX) && (st.Y == eq.SpawnY))
                            {
                                GetGamerPoint();

                                pen = new Pen(new SolidBrush(Color.Blue));

                                bkgBuffer.Graphics.DrawLine(pen, gamerPos, timerPoint);

                                // Update the Spawn Information Window

                                MobInfoLabel.Text = TimerInfo(st);
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

        private string GroundItemInfo(GroundItem gi)
        {
            if (f1 == null) { return ""; }

            StringBuilder grounditemInfo = new StringBuilder();

            grounditemInfo.AppendFormat("Ground Item: {0}\n", gi.Desc);

            grounditemInfo.AppendFormat("ActorDef: {0}\n", gi.Name);

            grounditemInfo.AppendFormat("Y: {0:f3}  X: {1:f3}  Z: {2:f3}", gi.Y, gi.X, gi.Z);

            MobInfoLabel.BackColor = Color.White;

            MeasureStrings(grounditemInfo.ToString(), out SizeF sc, out SizeF sf);

            sf.ToPointF();

            sc.ToPointF();

            AdjustTableLayout(9, sf, sc);

            return grounditemInfo.ToString();
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
            gamerPos.X = CalcScreenCoordX(eq.GamerInfo.X);
            gamerPos.Y = CalcScreenCoordY(eq.GamerInfo.Y);
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

                    if (!GroundItemDepthFilter || IsWithinDepthFilter(gi.Z, pZ))
                    {
                        Pen yellowPen = new Pen(new SolidBrush(Color.Yellow));
                        gi.Filtered = false;
                        PointF giPoint = new PointF(x, y);
                        DrawBigX(yellowPen, giPoint, PlusSzOZ);
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
            if (eq.SpawnX == gi.X && eq.SpawnY == gi.Y && eq.SelectedID == 99999)
            {
                GetGamerPoint();

                DrawLine(new Pen(new SolidBrush(Color.Yellow)), gamerPos.X, gamerPos.Y, x, y);

                DrawEllipse(new Pen(new SolidBrush(Color.Fuchsia)), x - SelectSizeOffset, y - SelectSizeOffset, SelectSize, SelectSize);

                // Update the Spawn Information Window

                MobInfoLabel.Text = GroundItemInfo(gi);
            }
        }

        private void FlashAlertGroundSpawns(float pZ, bool GroundItemDepthFilter, float x, float y, GroundItem gi)
        {
            var x1 = x - PlusSzOZ - 1;
            var y1 = y - PlusSzOZ - 1;
            var width = SpawnPlusSize + 2;
            var height = SpawnPlusSize + 2;

            // Draw Yellow Ring around Caution Ground Items
            if (!GroundItemDepthFilter || IsWithinDepthFilter(gi.Z, pZ))
            {
                if (gi.IsCaution)
                {
                    DrawEllipse(new Pen(new SolidBrush(Color.Yellow), 2), x1, y1, width, height);
                }
                // Draw Red Ring around Danger Ground Items
                if (gi.IsDanger)
                {
                    DrawEllipse(new Pen(new SolidBrush(Color.Red), 2), x1, y1, width, height);
                }

                // Draw White Ring around Rare Ground Items
                if (gi.IsAlert)
                {
                    DrawEllipse(new Pen(new SolidBrush(Color.White), 2), x1, y1, width, height);
                }

                // Draw Cyan Ring around Hunt Ground Items
                if (gi.IsHunt)
                {
                    DrawEllipse(new Pen(new SolidBrush(Color.Green), 2), x1, y1, width, height);
                }
            }
        }

        #endregion DrawGroundItems

        #region DrawDirectionLines

        public void DrawDirectionLines(Spawninfo sp, float x, float y)
        {
            try
            {
                float x1, y1;

                // Draw NPCs Direction Lines if heading > 0
                if (sp.Heading >= 0 && sp.Heading < 512)
                {
                    y1 = -(xCos[(int)sp.Heading] * (sp.SpeedRun * Ratio * 150));

                    x1 = -(xSin[(int)sp.Heading] * (sp.SpeedRun * Ratio * 150));

                    DrawLine(new Pen(new SolidBrush(Color.White)), x, y, x + x1, y + y1);
                }
            }
            catch (Exception ex) { LogLib.WriteLine("Error in DrawDirectionLines(): ", ex); }
        }

        #endregion DrawDirectionLines

        public void SetUpdateSteps()
        {
            // Define constants for minimum values
            const int MinUpdateSteps = 3;
            const int MinUpdateTicks = 1;

            // Cache the UpdateDelay value and prevent division by zero
            int updateDelay = Settings.Default.UpdateDelay;
            if (updateDelay <= 0)
            {
                // Set to a sensible default value to avoid division by zero
                updateDelay = 1;
            }

            // Calculate the number of update steps per second based on UpdateDelay
            // Add 1 to ensure that the calculation doesn't result in too few steps
            int updateSteps = (1000 / updateDelay) + 1;
            UpdateSteps = Math.Max(updateSteps, MinUpdateSteps);

            // Calculate the number of ticks per 250 ms timeframe based on UpdateDelay
            // Ensure there is at least 1 tick (to avoid zero or negative values)
            int updateTicks = 250 / updateDelay;
            UpdateTicks = Math.Max(updateTicks, MinUpdateTicks);
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

        private string TimerInfo(Spawntimer st)
        {
            // Exit early if the main form reference is not available.
            if (f1 == null || st == null) return string.Empty;

            int heightAdder = 20;

            // Get the spawn timer information as a formatted string.
            string timerInfo = BuildTimerInfoString(st, ref heightAdder);

            // Set the background color of the label.
            MobInfoLabel.BackColor = Color.White;

            // Measure the size of the string for layout purposes.
            MeasureStrings(timerInfo, out SizeF stringContentSize, out SizeF labelSize);

            // Adjust the layout based on measured sizes.
            AdjustTableLayout(heightAdder, stringContentSize, labelSize);

            return timerInfo;
        }

        private static string BuildTimerInfoString(Spawntimer st, ref int heightAdder)
        {
            var stringBuilder = new StringBuilder();

            // Add spawn name information.
            stringBuilder.AppendLine($"Spawn Name: {st.LastSpawnName}");

            // Process and append all names encountered.
            AppendEncounteredNames(st, stringBuilder, ref heightAdder);

            // Append other relevant information from the spawn timer description.
            stringBuilder.AppendLine(st.GetDescription());

            return stringBuilder.ToString();
        }

        private static void AppendEncounteredNames(Spawntimer st, StringBuilder stringBuilder, ref int heightAdder)
        {
            var namesToAdd = new StringBuilder("Names encountered: ");
            var names = st.AllNames.Split(',');

            const int MaxLineLength = 45; // Define a maximum line length for wrapping.
            var lineLength = namesToAdd.Length;

            foreach (var name in names.Select(n => n.TrimName()))
            {
                var nameLength = name.Length;

                // Check if adding the next name exceeds the maximum line length.
                if (lineLength + nameLength + 2 >= MaxLineLength)
                {
                    stringBuilder.AppendLine(namesToAdd.ToString());
                    heightAdder += 2; // Adjust height for the new line.

                    namesToAdd.Clear();
                    lineLength = 0;
                }
                else if (lineLength > 0) // Add a comma separator if it's not the first name.
                {
                    namesToAdd.Append(", ");
                    lineLength += 2;
                }

                namesToAdd.Append(name);
                lineLength += nameLength;
            }

            // Append any remaining names that haven't been added.
            if (namesToAdd.Length > 0)
            {
                stringBuilder.AppendLine(namesToAdd.ToString());
            }
        }

        private void AdjustTableLayout(int heightAdder, SizeF stringContentSize, SizeF labelSize)
        {
            tableLayoutPanel1.ColumnStyles[0].SizeType = SizeType.Absolute;

            if (Settings.Default.ShowTargetInfo)
            {
                // Determine the necessary width based on content sizes.
                int maxWidth = (int)Math.Max(stringContentSize.Width, labelSize.Width) + 10;

                tableLayoutPanel1.Width = maxWidth;
                tableLayoutPanel1.ColumnStyles[0].Width = maxWidth;

                tableLayoutPanel1.RowStyles[0].SizeType = SizeType.Absolute;
                tableLayoutPanel1.RowStyles[1].SizeType = SizeType.Absolute;

                tableLayoutPanel1.RowStyles[0].Height = (int)labelSize.Height + 7;
                tableLayoutPanel1.RowStyles[1].Height = (int)stringContentSize.Height + heightAdder;
            }
            else
            {
                HideTableRows();
            }
        }

        private void HideTableRows()
        {
            // Hide rows by setting their heights to zero.
            tableLayoutPanel1.RowStyles[0].Height = 0;
            tableLayoutPanel1.RowStyles[1].Height = 0;
        }
    }
}