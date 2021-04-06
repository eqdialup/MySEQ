// Class Files

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Media;
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

        public Font drawFont; //size 2
        public Font drawFont1;
        public Font drawFont3;

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

        public float m_panOffsetY;

        private int collect_mobtrails_count;

        // m_ratio - adjustment factor required to convert map->screen size.

        private float m_ratio = 1.0f;

        // m_mapCenter - centre point of screen in Map Units.

        private float m_mapCenterX;

        private float m_mapCenterY;

        // m_screenCenter - centre point of screen in Screen Units.

        private float m_screenCenterX;

        private float m_screenCenterY;

        private float x_adjust;

        private float y_adjust;

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

        private float selectedX = -1;          // [42!] Mark an arbitrary spot on the map

        private float selectedY = -1;          // Don't set these directly, but use SetSelectedPoint/ClearSelectedPoint

        private string curTarget = "";

        private BufferedGraphicsContext gfxManager;

        private BufferedGraphics bkgBuffer;

        private ToolTip tt;

        public bool flash; // used for flashing warning lights

        private SolidBrush textBrush;

        private MainForm f1;          // Caution: this may be null

        private MapPane mapPane;     // Caution: this may be null

        private EQData eq;

        private DateTime LastTTtime;

        private int fpsCount;

        private DateTime fpsLastReadTime = new DateTime();

        private TableLayoutPanel tableLayoutPanel1;

        public Label lblGameClock;

        private double fpsValue;

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
            double fpsTimeDelta;

            if (bkgBuffer == null)
            {
                return;
            }

            bkgBuffer.Render(e.Graphics);

            base.OnPaint(e);

            // Calculate FPS

            if ((fpsTimeDelta = (DateTime.Now - fpsLastReadTime).Seconds) > 0.5)

            {
                fpsValue = Math.Round(fpsCount / fpsTimeDelta, 2);

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
            lblMobInfo = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            lblGameClock = new Label();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            //
            // lblMobInfo
            //
            lblMobInfo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                        | AnchorStyles.Left
                        | AnchorStyles.Right;
            lblMobInfo.BackColor = Color.White;
            lblMobInfo.BorderStyle = BorderStyle.FixedSingle;
            lblMobInfo.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMobInfo.Location = new Point(0, 20);
            lblMobInfo.Margin = new Padding(0);
            lblMobInfo.Name = "lblMobInfo";
            lblMobInfo.Size = new Size(163, 80);
            lblMobInfo.TabIndex = 0;
            lblMobInfo.Text = "Spawn Information Window";
            //
            // tableLayoutPanel1
            //
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(lblGameClock, 0, 0);
            tableLayoutPanel1.Controls.Add(lblMobInfo, 0, 1);
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
            lblGameClock.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
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
            Size = new Size(224, 147);
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

            tt = new ToolTip
            {
                AutomaticDelay = 250
            };
            tt.SetToolTip(this, "ABCD\nEFGH");
            tt.Active = true;

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
            selectedX = x;
            selectedY = y;

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

        private void MapCon_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (mapPane == null)
            {
                return;
            }

            if (e.KeyChar == '+')
            {
                scale += 0.2f;

                mapPane.scale.Value = (decimal)(scale * 100);

                Invalidate();
            }
            else if (e.KeyChar == '-')
            {
                if (scale - 0.2 >= 0.1)
                {
                    scale -= 0.2f;

                    mapPane.scale.Value = (decimal)(scale * 100);
                }

                Invalidate();
            }
            else if (char.ToLower(e.KeyChar) == 'c' || char.ToLower(e.KeyChar) == '5')
            {
                mapPane.offsetx.Value = 0;

                mapPane.offsety.Value = 0;

                e.Handled = true;
            }
            else if (char.ToLower(e.KeyChar) == '4')
            {
                mapPane.offsetx.Value -= 50;

                e.Handled = true;
            }
            else if (char.ToLower(e.KeyChar) == '7')
            {
                mapPane.offsetx.Value -= 50;

                mapPane.offsety.Value -= 50;

                e.Handled = true;
            }
            else if (char.ToLower(e.KeyChar) == '8')
            {
                mapPane.offsety.Value -= 50;

                e.Handled = true;
            }
            else if (char.ToLower(e.KeyChar) == '9')
            {
                mapPane.offsety.Value -= 50;

                mapPane.offsetx.Value += 50;

                e.Handled = true;
            }
            else if (char.ToLower(e.KeyChar) == '6')
            {
                mapPane.offsetx.Value += 50;

                e.Handled = true;
            }
            else if (char.ToLower(e.KeyChar) == '3')
            {
                mapPane.offsety.Value += 50;

                mapPane.offsetx.Value += 50;

                e.Handled = true;
            }
            else if (char.ToLower(e.KeyChar) == '2')
            {
                mapPane.offsety.Value += 50;

                e.Handled = true;
            }
            else if (char.ToLower(e.KeyChar) == '1')
            {
                mapPane.offsety.Value += 50;

                mapPane.offsetx.Value -= 50;

                e.Handled = true;
            }

            ReAdjust();
        }

        private void MapCon_MouseDown(object sender, MouseEventArgs e)
        {
            //            bool emailmenu = true;
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
            mousex = ScreenToMapCoordX(e.X);
            mousey = ScreenToMapCoordY(e.Y);
        }

        private void RightMouseButton(MouseEventArgs e)
        {
            MouseMapLoc(e, out var mousex, out var mousey);

            var delta = (float)(5.0 / m_ratio);
            Spawninfo sp = eq.FindMobNoPetNoPlayerNoCorpse(delta, mousex, mousey);

            if (sp?.Name.Length > 0)
            {
                f1.alertAddmobname = RegexHelper.FilterMobName(sp.Name);
                f1.alertAddmobname = f1.alertAddmobname.Replace("_", " ");
                f1.alertAddmobname = f1.alertAddmobname.TrimEnd(' ');
                f1.alertX = sp.X;
                f1.alertY = sp.Y;
                f1.alertZ = sp.Z;
            }
            else
            {
                GroundItem gi = eq.FindGroundItem(delta, mousex, mousey);
                if (gi?.Name.Length > 0)
                {
                    eq.GetItemDescription(gi.Name);
                    f1.alertAddmobname = eq.GetItemDescription(gi.Name);
                    f1.alertX = gi.X;
                    f1.alertY = gi.Y;
                    f1.alertZ = gi.Z;
                    //                        emailmenu = false;
                }
                else
                {
                    f1.alertAddmobname = "";
                    Spawntimer st = eq.FindTimer(5.0f, mousex, mousey);
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
                        sp = eq.FindMobNoPetNoPlayer(delta, mousex, mousey);

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
                ModKeyControl(mousex, mousey);
            }
            else if ((ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                // [42!] Mark an arbitrary spot on the map, or turn it off if a spot was marked already.

                if (selectedX == -1)
                {
                    SetSelectedPoint(mousex, mousey);
                }
                else
                {
                    ClearSelectedPoint();
                }
            }
            else
            {
                if (e.X == m_dragStartX && e.Y == m_dragStartY)
                {
                    // allow a small margin of error in coordinates

                    // value of 5 screen units in terms of mapcoords

                    // try to select mob, if not then do timer

                    var delta = (float)(5.0 / m_ratio);
                    if (!eq.SelectMob(delta, mousex, mousey) && !eq.SelectTimer(delta, mousex, mousey))
                    {
                        eq.SelectGroundItem(delta, mousex, mousey);
                    }

                    Invalidate();
                }
            }

            m_dragging = false;

            m_rangechange = false;

            m_dragStartX = m_dragStartY = 0;

            //rclick = false;

            Cursor.Current = Cursors.Default;
        }

        private void ModKeyControl(float x, float y)
        {
            var delta = 5.0f / m_ratio;

            Spawninfo sp = eq.FindMobNoPet(delta, x, y) ?? eq.FindMob(delta, x, y);

            if (sp != null)
            {
                Spawninfo st = eq.FindMobTimer(sp.SpawnLoc);

                if (st == null)
                {
                    eq.SetSelectedID(sp.SpawnID);

                    eq.SpawnX = -1.0f;
                    eq.SpawnY = -1.0f;
                }
                else
                {
                    eq.SetSelectedID(st.SpawnID);
                    Spawntimer spt = eq.FindTimer(1.0f, st.X, st.Y);
                    if (spt?.itmSpawnTimerList != null)
                    {
                        spt.itmSpawnTimerList.Selected = true;
                        spt.itmSpawnTimerList.EnsureVisible();
                    }

                    eq.SpawnX = st.X;
                    eq.SpawnY = st.Y;
                }
            }
            else
            {
                if (!eq.SelectTimer((float)(5.0 / m_ratio), x, y))
                {
                    eq.SelectGroundItem((float)(5.0 / m_ratio), x, y);
                }
            }
        }

        private void MapCon_MouseMove(object sender, MouseEventArgs e)
        {
            if (mapPane == null || f1 == null)
            {
                return;
            }

            // Limit TT popups to four times a sec

            TimeSpan interval = DateTime.Now.Subtract(LastTTtime);

            if (interval.TotalSeconds < 0.25)
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

        private void PopulateToolTip(MouseEventArgs e)
        {
            MouseMapLoc(e, out var mousex, out var mousey);
            var delta = 5.0f / m_ratio;

            Spawninfo sp = eq.FindMobNoPet(delta, mousex, mousey) ?? eq.FindMob(delta, mousex, mousey);

            bool found;
            if (sp == null)
            {
                found = false;
            }
            else
            {
                found = true;
                LastTTtime = DateTime.Now;
                tt.SetToolTip(this, MobInfo(sp, false, false));
                tt.AutomaticDelay = 0;
                tt.Active = true;
            }

            if (!found)

            {
                GroundItem gi = eq.FindGroundItem(delta, mousex, mousey);

                if (gi != null)

                {
                    var ItemName = gi.Name;

                    foreach (ListItem listItem in eq.GroundSpawn)//eq.itemList.Values)

                    {
                        if (gi.Name == listItem.ActorDef)

                        {
                            ItemName = listItem.Name;
                        }
                    }

                    var s = $"Name: {ItemName}\n{gi.Name}";

                    tt.SetToolTip(this, s);

                    tt.AutomaticDelay = 0;

                    tt.Active = true;

                    LastTTtime = DateTime.Now;

                    found = true;
                }
            }

            if (!found)

            {
                Spawntimer st = eq.mobsTimers.Find(delta, mousex, mousey);

                if (st != null)

                {
                    var description = st.GetDescription();

                    if (description != null)

                    {
                        tt.SetToolTip(this, description);

                        tt.AutomaticDelay = 0;

                        tt.Active = true;
                    }

                    LastTTtime = DateTime.Now;

                    found = true;
                }
            }

            if (!found)
            {
                tt.SetToolTip(this, "");
            }
        }

        private void ClearPan()
        {
            m_panOffsetX = 0;
            m_panOffsetY = 0;
            ReAdjust();
        }

        public void ReAdjust()
        {
            var mapWidth = Math.Abs(eq.maxx - eq.minx);

            var mapHeight = Math.Abs(eq.maxy - eq.miny);

            var ScreenWidth = Width - (2 * (float)15);

            var ScreenHeight = Height - (2 * (float)15);

            m_screenCenterX = Width / 2;

            m_screenCenterY = Height / 2;

            var zoom = scale;

            if (m_zoom > 32)
            {
                m_zoom = 32;
            }

            var xratio = (float)ScreenWidth / mapWidth;

            var yratio = (float)ScreenHeight / mapHeight;

            // Use the smaller scale ratio so that the map fits in the screen at a zoom of 1.

            m_ratio = xratio < yratio ? xratio * zoom : yratio * zoom;

            // Calculate the Map Center
            if (Settings.Default.FollowOption == FollowOption.None)
            {
                m_mapCenterX = eq.minx + (mapWidth / 2);
                m_mapCenterY = eq.miny + (mapHeight / 2);
            }
            else if (Settings.Default.FollowOption == FollowOption.Player)
            {
                m_mapCenterX = eq.gamerInfo.X;
                m_mapCenterY = eq.gamerInfo.Y;
            }
            else if (Settings.Default.FollowOption == FollowOption.Target)
            {
                Spawninfo siTarget = eq.GetSelectedMob();

                if (siTarget != null)
                {
                    m_mapCenterX = siTarget.X;
                    m_mapCenterY = siTarget.Y;
                }
            }

            // When Following a player or spawn and KeepCentered is not selected

            // adjust the map center so as to minimise the amount of blank space in the map window.

            if (!Settings.Default.KeepCentered && Settings.Default.FollowOption != FollowOption.None)

            {
                // Calculate the MapCordinates of the Screen Edges

                float ScreenMaxY, ScreenMinY, ScreenMinX, ScreenMaxX;

                float ScreenMapWidth, ScreenMapHeight;

                ScreenMaxY = ScreenToMapCoordY(15, true);

                ScreenMinY = ScreenToMapCoordY(Height - (float)15, true);

                ScreenMapHeight = Math.Abs(ScreenMaxY - ScreenMinY);

                // X sense is wrong way round...

                ScreenMinX = ScreenToMapCoordX(Width - (float)15, true);

                ScreenMaxX = ScreenToMapCoordX(15, true);

                ScreenMapWidth = Math.Abs(ScreenMaxX - ScreenMinX);

                if (mapWidth <= ScreenMapWidth)
                {
                    // If map fits in window set center to center of map

                    m_mapCenterX = eq.minx + (mapWidth / 2);
                }
                else
                {
                    // if we have blank space to the left or right repostion the center point appropriately
                    reposCenter(ScreenMinX, ScreenMaxX);
                }

                if (mapHeight <= ScreenMapHeight)

                {
                    // If map fits in window set center to center of map

                    m_mapCenterY = eq.miny + (mapHeight / 2);
                }
                else
                {
                    // if we have blank space at the top or botton repostion the center point appropriately
                    reposCenter(ScreenMinX, ScreenMaxX);
                }
                LogLib.WriteLine("Readjust Done");
            }
            x_adjust = m_panOffsetX + m_screenCenterX + (float)(m_mapCenterX * m_ratio);
            y_adjust = m_panOffsetY + m_screenCenterY + (float)(m_mapCenterY * m_ratio);

            void reposCenter(float ScreenMinX, float ScreenMaxX)
            {
                if (ScreenMinX < eq.minx)
                {
                    m_mapCenterX += eq.minx - ScreenMinX;
                }
                else if (ScreenMaxX > eq.maxx)
                {
                    m_mapCenterX -= ScreenMaxX - eq.maxx;
                }
            }
        }

        public float CalcScreenCoordX(float mapCoordinateX) => x_adjust - (float)(mapCoordinateX * m_ratio);

        // Formula Should be
        // Screen X =CenterScreenX + ((mapCoordinateX - MapCenterX) * m_ratio)

        // However Eq's Map coordinates are in the oposite sense to the screen
        // so we have to multiply the second portion by -1, which is the same
        // as changing the plus to a minus...

        //m_ratio = (ScreenWidth/MapWidth) * zoom (Calculated ahead of time in ReAdjust)

        //return m_panOffsetX + m_screenCenterX - ((mapCoordinateX - m_mapCenterX) * m_ratio);
        public float CalcScreenCoordY(float mapCoordinateY) => y_adjust - (float)(mapCoordinateY * m_ratio);

        private float ScreenToMapCoordX(float screenCoordX) => m_mapCenterX + ((m_panOffsetX + m_screenCenterX - screenCoordX) / m_ratio);

        private float ScreenToMapCoordY(float screenCoordY) => m_mapCenterY + ((m_panOffsetY + m_screenCenterY - screenCoordY) / m_ratio);

        private float ScreenToMapCoordX(float screenCoordX, bool IgnorePan)
        {
            if (IgnorePan)
            {
                return m_mapCenterX + ((m_screenCenterX - screenCoordX) / m_ratio);
            }
            else
            {
                return ScreenToMapCoordX(screenCoordX);
            }
        }

        private float ScreenToMapCoordY(float screenCoordY, bool IgnorePan)
        {
            if (IgnorePan)
            {
                return m_mapCenterY + ((m_screenCenterY - screenCoordY) / m_ratio);
            }
            else
            {
                return ScreenToMapCoordY(screenCoordY);
            }
        }

        private void DrawLine(Pen pen, float x1, float y1, float x2, float y2)
        {
            try
            {
                if (pen is null) return;
                else
                    bkgBuffer.Graphics.DrawLine(pen, x1, y1, x2, y2);
            }
            catch (Exception ex) { LogLib.WriteLine($"Error with DrawLine({x1}, {y1}, {x2}, {y2}): ", ex); }
        }

        private void DrawLines(Pen pen, PointF[] points)
        {
            try
            {
                if (pen is null) return;
                else
                    bkgBuffer.Graphics.DrawLines(pen, points);
            }
            catch (Exception ex) { LogLib.WriteLine("Error with DrawLines: ", ex); }
        }

        private void FillEllipse(Brush brush, float x1, float y1, float width, float height)
        {
            try
            {
                if (brush is null) return;
                else
                    bkgBuffer.Graphics.FillEllipse(brush, x1, y1, width, height);
            }
            catch (Exception ex) { LogLib.WriteLine($"Error with FillEllipse({x1}, {y1}, {width}, {height}): ", ex); }
        }

        private void DrawEllipse(Pen pen, float x1, float y1, float width, float height)
        {
            //if (x1 != x1 || y1 != y1 || width != width || height != height) return;

            try
            {
                if (pen is null) return;
                else
                    bkgBuffer.Graphics.DrawEllipse(pen, x1, y1, width, height);
            }
            catch (Exception ex) { LogLib.WriteLine($"Error with DrawEllipse({x1}, {y1}, {width}, {height}): ", ex); }
        }

        private void DrawTriangle(Pen pen, float x1, float y1, float radius)
        {
            PointF[] points = TrianglePoints(x1, y1, radius);
            try
            {
                if (pen is null) return;
                else
                    bkgBuffer.Graphics.DrawLines(pen, points);
            }
            catch (Exception ex) { LogLib.WriteLine($"Error with DrawTriangle({x1}, {y1}, {radius}): ", ex); }
        }

        private void FillTriangle(Brush brush, float x1, float y1, float radius)
        {
            PointF[] points = TrianglePoints(x1, y1, radius);

            try
            {
                if (brush is null) return;
                else
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

        //public void drawarc(Pen pen, float x1, float y1, float width, float height, float startangle, float sweepangle)
        //{
        //    try { bkgBuffer.Graphics.DrawArc(pen, x1, y1, width, height, startangle, sweepangle); }
        //    catch (Exception ex) { LogLib.WriteLine($"error with drawarc({x1}, {y1}, {width}, {height}, {startangle}, {sweepangle}): ", ex); }
        //}

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
                    if (si.Level < (eq.GreyRange + eq.gamerInfo.Level))
                    {
                        lblMobInfo.BackColor = Color.LightGray;
                    }
                    else if (si.Level < (eq.GreenRange + eq.gamerInfo.Level))
                    {
                        lblMobInfo.BackColor = Color.PaleGreen;
                    }
                    else if (si.Level < (eq.CyanRange + eq.gamerInfo.Level))
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
                        lblMobInfo.BackColor = si.Level <= eq.gamerInfo.Level + eq.YellowRange ? Color.Yellow : Color.Red;
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

                graphics = lblMobInfo.CreateGraphics();

                SizeF sf = new SizeF();
                SizeF sc = new SizeF();

                sf = graphics.MeasureString(mobInfo.ToString(), lblMobInfo.Font);

                sc = graphics.MeasureString(lblGameClock.Text, lblGameClock.Font);

                graphics.Dispose();

                tableLayoutPanel1.ColumnStyles[0].SizeType = SizeType.Absolute;

                if (Settings.Default.ShowTargetInfo)

                {
                    lblMobInfo.Visible = true;

                    if (ChangeSize)

                    {
                        var panel_width = sc.Width > sf.Width ? (int)sc.Width : (int)sf.Width;
                        tableLayoutPanel1.Width = panel_width + (Settings.Default.SmallTargetInfo ? 40 : 10);

                        tableLayoutPanel1.ColumnStyles[0].Width = panel_width + (Settings.Default.SmallTargetInfo ? 40 : 10);

                        tableLayoutPanel1.RowStyles[1].SizeType = SizeType.Absolute;

                        tableLayoutPanel1.RowStyles[0].SizeType = SizeType.Absolute;

                        tableLayoutPanel1.RowStyles[1].Height = (int)sf.Height + (Settings.Default.SmallTargetInfo ? 11 : 17);

                        tableLayoutPanel1.RowStyles[0].Height = (int)sc.Height + 7;
                    }
                }
                else
                {
                    if (ChangeSize)

                    {
                        tableLayoutPanel1.Width = (int)sc.Width + 10;

                        tableLayoutPanel1.ColumnStyles[0].Width = (int)sc.Width + 10;

                        tableLayoutPanel1.RowStyles[0].SizeType = SizeType.Absolute;

                        tableLayoutPanel1.RowStyles[0].Height = 0;

                        tableLayoutPanel1.RowStyles[1].SizeType = SizeType.Absolute;

                        tableLayoutPanel1.RowStyles[1].Height = 0;
                    }
                }

                return mobInfo.ToString();
            }
            catch (Exception ex)
            {
                LogLib.WriteLine("Error with MobInfo(): ", ex);
                return "";
            }
        }

        private StringBuilder SpawnInfoWindow(Spawninfo si)
        {
            var sd = SpawnDistance(si);

            StringBuilder mobInfo = new StringBuilder();

            if (Settings.Default.SmallTargetInfo)
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
                else if (si.m_isPlayer)
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

                mobInfo.AppendFormat("Speed: {0:f3}  Dist: {1:f0}\n", si.SpeedRun, sd);

                mobInfo.AppendFormat("Y: {0:f1} X: {1:f1} Z: {2:f1}", si.Y, si.X, si.Z);
            }
            else
            {
                // long target window version
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
                    mobInfo.AppendFormat("Class: {0}    Primary: {1} ({2})\n", eq.GetClass(si.Class), eq.ItemNumToString(si.Primary), si.Primary);
                }
                else
                {
                    mobInfo.AppendFormat("Class: {0}\n", eq.GetClass(si.Class));
                }

                if (si.Offhand > 0)
                {
                    mobInfo.AppendFormat("Race: {0}    Offhand: {1} ({2})\n", eq.GetRace(si.Race), eq.ItemNumToString(si.Offhand), si.Offhand);
                }
                else
                {
                    mobInfo.AppendFormat("Race: {0}\n", eq.GetRace(si.Race));
                }

                mobInfo.AppendFormat("Speed: {0:f3}\n", si.SpeedRun);

                mobInfo.AppendFormat("Visibility: {0}\n", PrettyNames.GetHideStatus(si.Hide));

                mobInfo.AppendFormat("Distance: {0:f3}\n", sd);

                mobInfo.AppendFormat("Y: {0:f3}  X: {1:f3}  Z: {2:f3}", si.Y, si.X, si.Z);
            }

            return mobInfo;
        }

        private float SpawnDistance(Spawninfo si)
        {
            return (float)Math.Sqrt(((si.X - eq.gamerInfo.X) * (si.X - eq.gamerInfo.X)) +

                                ((si.Y - eq.gamerInfo.Y) * (si.Y - eq.gamerInfo.Y)) +

                                ((si.Z - eq.gamerInfo.Z) * (si.Z - eq.gamerInfo.Z)));
        }

        private string TimerInfo(Spawntimer st)

        {
            var height_adder = 20;
            try

            {
                if (f1 == null) { return ""; }

                string descr = null;

                var countTime = 0;

                var countTimer = "";

                if (st.NextSpawnDT != DateTime.MinValue)
                {
                    TimeSpan Diff = st.NextSpawnDT.Subtract(DateTime.Now);

                    countTimer = Diff.Hours.ToString("00") + ":" + Diff.Minutes.ToString("00") + ":" + Diff.Seconds.ToString("00");

                    countTime = (Diff.Hours * 3600) + (Diff.Minutes * 60) + Diff.Seconds;
                }

                if (countTime > 0)
                {
                    // StringBuilder moved to new, common method, as equal for all paths.
                    height_adder = Spawnformbuild(st, height_adder, out StringBuilder spawnTimer, out var names_to_add);
                    if (names_to_add.Length > 0)
                    {
                        spawnTimer.Append(names_to_add);
                    }

                    spawnTimer.Append("\n");

                    spawnTimer.AppendFormat("Last Spawned At: {0}\n", st.SpawnTimeStr);

                    spawnTimer.AppendFormat("Last Killed At: {0}\n", st.KillTimeStr);

                    spawnTimer.AppendFormat("Next Spawn At: {0}\n", st.NextSpawnStr);

                    spawnTimer.AppendFormat("Spawn Timer: {0} secs\n", st.SpawnTimer);

                    spawnTimer.AppendFormat("Spawning In: {0}\n", countTimer);

                    spawnTimer.AppendFormat("Spawn Count: {0}\n", st.SpawnCount);

                    spawnTimer.AppendFormat("Y: {0:f3}  X: {1:f3}  Z: {2:f3}", st.Y, st.X, st.Z);

                    descr = spawnTimer.ToString();
                }
                else if (st.SpawnTimer > 0)
                {
                    height_adder = Spawnformbuild(st, height_adder, out StringBuilder spawnTimer, out var names_to_add);
                    descr = SpawnForm(st, spawnTimer, names_to_add);
                }
                else
                {
                    height_adder = Spawnformbuild(st, height_adder, out StringBuilder spawnTimer, out var names_to_add);
                    descr = SpawnForm(st, spawnTimer, names_to_add);
                }

                //return descr;

                var timerInfo = descr;

                //String timerInfo = st.GetDescription();

                lblMobInfo.BackColor = Color.White;

                Graphics g = lblMobInfo.CreateGraphics();

                SizeF sf = g.MeasureString(timerInfo, lblMobInfo.Font);

                Graphics gt = lblGameClock.CreateGraphics();

                SizeF sc = gt.MeasureString(lblGameClock.Text, lblGameClock.Font);

                g.Dispose();

                gt.Dispose();

                sf.ToPointF();

                sc.ToPointF();

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

                    tableLayoutPanel1.RowStyles[1].Height = (int)sf.Height + height_adder;

                    tableLayoutPanel1.RowStyles[0].Height = (int)sc.Height + 7;
                }
                else
                {
                    tableLayoutPanel1.RowStyles[1].SizeType = SizeType.Absolute;

                    tableLayoutPanel1.RowStyles[0].SizeType = SizeType.Absolute;

                    tableLayoutPanel1.RowStyles[1].Height = 0;

                    tableLayoutPanel1.RowStyles[0].Height = 0;
                }

                return timerInfo;
            }
            catch (Exception ex) { LogLib.WriteLine("Error with TimerInfo(): ", ex); return ""; }
        }

        private static int Spawnformbuild(Spawntimer st, int height_adder, out StringBuilder spawnTimer, out string names_to_add)
        {
            spawnTimer = new StringBuilder();
            spawnTimer.AppendFormat("Spawn Name: {0}\n", st.LastSpawnName);

            names_to_add = "Names encountered: ";
            var names = st.AllNames.Split(',');

            var namecount = 0;

            foreach (var name in names)
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
                        spawnTimer.Append(names_to_add);
                        spawnTimer.Append("\n");
                        height_adder++;
                        height_adder++;

                        names_to_add = namet;
                    }
                }

                namecount++;
            }

            return height_adder;
        }

        private static string SpawnForm(Spawntimer st, StringBuilder spawnTimer, string names_to_add)
        {
            if (names_to_add.Length > 0)
            {
                spawnTimer.Append(names_to_add);
            }

            spawnTimer.Append("\n");

            spawnTimer.AppendFormat("Last Spawned At: {0}\n", st.SpawnTimeStr);

            spawnTimer.AppendFormat("Last Killed At: {0}\n", st.KillTimeStr);

            spawnTimer.AppendFormat("Next Spawn At: {0}\n", "");

            spawnTimer.AppendFormat("Spawn Timer: {0} secs\n", "0");

            spawnTimer.AppendFormat("Spawning In: {0}\n", "");

            spawnTimer.AppendFormat("Spawn Count: {0}\n", st.SpawnCount);

            spawnTimer.AppendFormat("Y: {0:f3}  X: {1:f3}  Z: {2:f3}", st.Y, st.X, st.Z);

            return spawnTimer.ToString();
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

                    tableLayoutPanel1.RowStyles[1].Height = (int)sf.Height + 9;

                    tableLayoutPanel1.RowStyles[0].Height = (int)sc.Height + 7;
                }
                else
                {
                    tableLayoutPanel1.RowStyles[1].SizeType = SizeType.Absolute;

                    tableLayoutPanel1.RowStyles[0].SizeType = SizeType.Absolute;

                    tableLayoutPanel1.RowStyles[1].Height = 0;

                    tableLayoutPanel1.RowStyles[0].Height = 0;
                }

                return grounditemInfo.ToString();
            }
            catch (Exception ex) { LogLib.WriteLine("Error with TimerInfo(): ", ex); return ""; }
        }

        private void MapCon_Paint(object sender, PaintEventArgs pe)
        {
            if (mapPane == null || f1 == null)
            {
                return;
            }

            try
            {
                // Check if the Window is not minimized

                if (f1.WindowState != FormWindowState.Minimized)
                {
                    DrawOptions DrawOpts = f1.DrawOpts;

                    Graphics sg = pe.Graphics;

                    // Clear Map

                    bkgBuffer.Graphics.Clear(Settings.Default.BackColor);

                    lblGameClock.Text = $"{eq.gametime:MMM d, yyyy} {eq.gametime:t}";

                    lblGameClock.TextAlign = ContentAlignment.MiddleCenter;

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

                    var dx = ((m_panOffsetX + m_screenCenterX) / -m_ratio) - m_mapCenterX;

                    var dy = ((m_panOffsetY + m_screenCenterY) / -m_ratio) - m_mapCenterY;

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

                    if (eq.Zoning)
                    {
                        DrawPlayer(CalcScreenCoordX(0.0f), CalcScreenCoordY(0.0f), SpawnSize, SpawnSizeOffset, DrawOpts);
                    }
                    else
                    {
                        if ((DrawOpts & DrawOptions.Player) != DrawOptions.None)
                        {
                            DrawPlayer(playerx, playery, SpawnSize, SpawnSizeOffset, DrawOpts);
                        }

                        if ((DrawOpts & DrawOptions.Spawns) != DrawOptions.None)
                        {
                            DrawCorpses(pZ);
                        }

                        if ((DrawOpts & DrawOptions.GroundItems) != DrawOptions.None)
                        {
                            DrawGroundItems(pZ);
                        }

                        if ((DrawOpts & DrawOptions.SpawnTimers) != DrawOptions.None)
                        {
                            DrawSpawnTimers();
                        }

                        if (Settings.Default.SpawnDrawSize > 1)
                        {
                            bkgBuffer.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        }

                        if ((DrawOpts & DrawOptions.Spawns) != DrawOptions.None)
                        {
                            DrawSpawns(pX, pY, pZ, playerx, playery, DrawOpts);
                        }
                    }
                    // Collect mob trails, every 8th pass - approx once every 1 sec
                    if (Settings.Default.CollectMobTrails)
                    {
                        MobTrailCounter();
                    }

                    bkgBuffer.Graphics.SmoothingMode = SmoothingMode.None;

                    // [42!] Draw a line to an arbitrary spot.

                    if ((selectedX != -1) && ((DrawOpts & DrawOptions.SpotLine) != DrawOptions.None))
                    {
                        DrawLine(DashingPen(), playerx, playery, CalcScreenCoordX(selectedX), CalcScreenCoordY(selectedY));
                    }

                    f1.toolStripFPS.Text = $"FPS: {fpsValue}";

                    // Setup GDI Drawing

                    bkgBuffer.Render(sg);
                }
            }
            catch (Exception ex) { LogLib.WriteLine("Error in MapCon_Paint(): ", ex); }
        }

        private void MobTrailCounter()
        {
            if (collect_mobtrails_count > 8)
            {
                collect_mobtrails_count = 0;
                eq.CollectMobTrails();
            }
            collect_mobtrails_count++;
        }

        private static Pen DashingPen()
        {
            return new Pen(new SolidBrush(Color.White))
            {
                DashStyle = DashStyle.Dash,

                DashPattern = new float[] { 8, 4 }
            };
        }

        private void DrawCorpses(float pZ)

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
                    sp.proxAlert = false;

                    // Draw Corpses

                    if (sp.IsPlayer)

                    {
                        if (!PCCorpseDepthFilter || ((sp.Z > pZ - filterneg) && (sp.Z < pZ + filterpos)))
                        {
                            DrawRectangle(new Pen(new SolidBrush(Color.Yellow)), x - PlusSzOZ + 0.5f, y - PlusSzOZ + 0.5f, SpawnPlusSize, SpawnPlusSize);

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
                            DrawLine(new Pen(new SolidBrush(Color.Cyan)), x - drawOffset, y, x + drawOffset, y);

                            DrawLine(new Pen(new SolidBrush(Color.Cyan)), x, y - drawOffset, x, y + drawOffset);

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

        private void GetSpawnLoc(out float x, out float y, Spawninfo sp)
        {
            x = (float)Math.Round(CalcScreenCoordX(sp.X), 0);

            y = (float)Math.Round(CalcScreenCoordY(sp.Y), 0);
        }

        #region DrawSpawns

        private void DrawSpawns(float pX, float pY, float pZ, float playerx, float playery, DrawOptions DrawOpts)
        {
            var playerSpawnID = eq.gamerInfo.SpawnID;

            var RangeCircle = Settings.Default.RangeCircle;

            var NPCDepthFilter = Settings.Default.DepthFilter && Settings.Default.FilterNPCs;

            var PCDepthFilter = Settings.Default.DepthFilter && Settings.Default.FilterPlayers;

            //            string gName;

            var ShowRings = (DrawOpts & DrawOptions.SpawnRings) != DrawOptions.None;

            var DrawDirection = (DrawOpts & DrawOptions.DirectionLines) != DrawOptions.None;

            if ((eq.selectedID == 99999) && (eq.SpawnX == -1))
            {
                lblMobInfo.Text = MobInfo(null, true, true);
            }

            // Draw Spawns

            foreach (Spawninfo sp in eq.GetMobsReadonly().Values)
            {
                GetSpawnLoc(out var x, out var y, sp);

                //                gName = eq.GuildNumToString(sp.Guild);

                // Draw Line from Player to the Selected Spawn

                if (eq.selectedID == sp.SpawnID)
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
                else if (Settings.Default.ColorRangeCircle && RangeCircle > 0)
                {
                    // do checks for proximity alert

                    ProxAlert(pX, pY, pZ, NPCDepthFilter, sp);
                }
                else
                {
                    sp.proxAlert = false;
                }

                // Draw All Other Spawns

                if (sp.SpawnID != playerSpawnID && sp.flags == 0 && sp.Name.Length > 0)
                {
                    // Draw Spawn if not Hidden

                    if (!sp.hidden)
                    {
                        // Draw Spawn if the Spawn is within the Players Depth Filter

                        // Highlight Current Target

                        if (curTarget == sp.Name)
                        {
                            FillRectangle(new SolidBrush(Color.White), x - PlusSzOZ - 1, y - PlusSzOZ - 1, SpawnPlusSize + 2, SpawnPlusSize + 2);
                        }

                        // Draw Invisible Mob - these are EQ trigger events

                        if (sp.isEventController)
                        {
                            if (!NPCDepthFilter || ((sp.Z > pZ - filterneg) && (sp.Z < pZ + filterpos)))
                            {
                                FillEllipse(new SolidBrush(Color.Purple), x - SpawnSizeOffset, y - SpawnSizeOffset, SpawnSize, SpawnSize);
                                sp.filtered = false;
                            }
                            else
                            {
                                sp.filtered = true;
                            }
                        }
                        else if (sp.isLDONObject)
                        {
                            if (!NPCDepthFilter || ((sp.Z > pZ - filterneg) && (sp.Z < pZ + filterpos)))
                            {
                                FillEllipse(new SolidBrush(Color.Gray), x - SpawnSizeOffset, y - SpawnSizeOffset, SpawnSize, SpawnSize);
                                sp.filtered = false;
                            }
                            else
                            {
                                sp.filtered = true;
                            }
                        }
                        else if (sp.Type == 0)
                        {
                            // Draw Other Players

                            if (!PCDepthFilter || ((sp.Z > pZ - filterneg) && (sp.Z < pZ + filterpos)))
                            {
                                DrawOtherPlayers(DrawOpts, x, y, /*gName,*/ sp);
                            }
                            else
                            {
                                sp.filtered = true;
                            }
                        }
                        else if (sp.Type == 1 || sp.Type == 4)
                        {
                            if (!NPCDepthFilter || ((sp.Z > pZ - filterneg) && (sp.Z < pZ + filterpos)))
                            {
                                DrawNPCs(x, y, /*gName,*/ DrawDirection, sp);
                            }
                            else
                            {
                                sp.filtered = true;
                            }
                        }
                        DrawRings(x, y, sp);
                        DrawFlashes(pZ, NPCDepthFilter, x, y, sp);
                        MarkSpecial(pZ, NPCDepthFilter, x, y, ShowRings, sp);
                    }
                }
            }

            lookup_set = false;
        }

        private void DrawNPCs(float x, float y, /*string gName,*/ bool DrawDirection, Spawninfo sp)
        {
            sp.filtered = false;

            if (Settings.Default.ShowNPCNames && (sp.Name.Length > 0))
            {
                DrawSpawnNames(textBrush, sp.Name, sp.X, sp.Y);//, gName);
            }
            else if (Settings.Default.ShowNPCLevels && (sp.Name.Length > 0))
            {
                DrawSpawnNames(textBrush, sp.Level.ToString(), sp.X, sp.Y);//, gName);
            }

            if (DrawDirection)
            {
                DrawDirectionLines(sp, x, y);
            }

            // Draw NPCs
            if ((sp.isPet && !Settings.Default.ShowPVP) || sp.isFamiliar || sp.isMount)
            {
                FillEllipse(new SolidBrush(Color.Gray), x - SpawnSizeOffset, y - SpawnSizeOffset, SpawnSize, SpawnSize);
            }
            else if (eq.ConColors[sp.Level] != null)
            {
                FillEllipse(eq.ConColors[sp.Level], x - SpawnSizeOffset, y - SpawnSizeOffset, SpawnSize, SpawnSize);
            }

            // Draw PC color border around Mercenary

            if (sp.isMerc)
            {
                DrawEllipse(new Pen(new SolidBrush(Settings.Default.PCBorderColor)), x - SpawnSizeOffset, y - SpawnSizeOffset, SpawnSize, SpawnSize);
            }

            // Draw Purple border around invis mobs

            if (sp.Hide != 0)
            {
                Pen purplePen = new Pen(new SolidBrush(Color.Purple));
                // Flashing purple ring around SoS mobs

                if (sp.Hide == 2)
                {
                    if (flash)
                    {
                        DrawEllipse(purplePen, x - SelectSizeOffset, y - SelectSizeOffset, SelectSize, SelectSize);
                    }
                }
                else
                {
                    DrawEllipse(purplePen, x - SelectSizeOffset, y - SelectSizeOffset, SelectSize, SelectSize);
                }
            }
        }

        private void DrawOtherPlayers(DrawOptions DrawOpts, float x, float y, /*string gName,*/ Spawninfo sp)
        {
            sp.filtered = false;
            if (eq.ConColors[sp.Level] != null)
            {
                if ((DrawOpts & DrawOptions.DirectionLines) != DrawOptions.None)
                {
                    DrawDirectionLines(sp, x, y);
                }

                // Draw Other Players

                var levelRange = ((Math.Abs(eq.gamerInfo.Level - sp.Level) <= Settings.Default.PVPLevels) || (Settings.Default.PVPLevels == -1));
                if (Settings.Default.ShowPVP && levelRange)
                {
                    FillTriangle(eq.ConColors[sp.Level], x, y, SelectSizeOffset);

                    DrawTriangle(new Pen(new SolidBrush(Settings.Default.PCBorderColor)), x, y, SelectSizeOffset);

                    if (Settings.Default.ShowPCNames && (sp.Name.Length > 0))
                    //                        && Settings.Default.ShowPCGuild && (gName.Length > 0))
                    {
                        DrawSpawnNames(textBrush, $"{sp.Level}: {sp.Name}", sp.X, sp.Y);//, gName);
                    }
                    else
                        if (Settings.Default.ShowPCNames && (sp.Name.Length > 0))
                    {
                        DrawSpawnNames(textBrush, $"{sp.Level}: {sp.Name}", sp.X, sp.Y);//, gName);
                    }
                    else
                    {
                        if (Settings.Default.ShowPVPLevel)
                        {
                            DrawSpawnNames(textBrush, sp.Level.ToString(), sp.X, sp.Y);//, gName);
                        }
                    }

                    if (flash)
                    {
                        Pen cPen = new Pen(eq.GetDistinctColor(Color.White));

                        DrawEllipse(cPen, x - SelectSizeOffset, y - SelectSizeOffset, SelectSize, SelectSize);
                    }
                }
                else
                {
                    FillRectangle(eq.ConColors[sp.Level], x - PlusSzOZ + 0.5f, y - PlusSzOZ + 0.5f, SpawnPlusSize, SpawnPlusSize);

                    DrawRectangle(new Pen(new SolidBrush(Settings.Default.PCBorderColor)), x - PlusSzOZ + 0.5f, y - PlusSzOZ + 0.5f, SpawnPlusSize, SpawnPlusSize);

                    // draw purple border around players

                    if (sp.Hide != 0)
                    {
                        Pen purplePen = new Pen(new SolidBrush(Color.Purple));

                        if (sp.Hide == 2)
                        {
                            // SoS Players

                            if (flash)
                            {
                                DrawRectangle(purplePen, x - PlusSzOZ - 0.5f, y - PlusSzOZ - 0.5f, SpawnPlusSize + 2.0f, SpawnPlusSize + 2.0f);
                            }
                        }
                        else
                        {
                            // Player is invis

                            DrawRectangle(purplePen, x - PlusSzOZ - 0.5f, y - PlusSzOZ - 0.5f, SpawnPlusSize + 2.0f, SpawnPlusSize + 2.0f);
                        }
                    }

                    if (Settings.Default.ShowPCNames && (sp.Name.Length > 0))
                    {
                        DrawSpawnNames(textBrush, $"{sp.Level}: {sp.Name}", sp.X, sp.Y);//, gName);
                    }
                    //else if (Settings.Default.ShowPCGuild && (gName.Length > 0))
                    //{ DrawSpawnNames(textBrush, gName, sp.X, sp.Y); }//, gName); }
                }
            }
        }

        private void ProxAlert(float pX, float pY, float pZ, bool NPCDepthFilter, Spawninfo sp)
        {
            if (sp.alertMob && (sp.Type != 2))
            {
                // if alertmob - use to identify mobs that are ok to do alerts

                var minlevel = Settings.Default.MinAlertLevel;

                if (minlevel == -1)
                {
                    minlevel = eq.gamerInfo.Level + eq.GreyRange;
                }

                if (sp.Level >= minlevel)
                {
                    float rRange = Settings.Default.RangeCircle;

                    //                    float rsRange = rRange * 1.1f;

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

                                PlayAlertSound();
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

        public void PlayAlertSound()
        {
            if (Settings.Default.AlertSound == "Asterisk")
            {
                SystemSounds.Asterisk.Play();
            }
            else if (Settings.Default.AlertSound == "Beep")
            {
                SystemSounds.Beep.Play();
            }
            else if (Settings.Default.AlertSound == "Exclamation")
            {
                SystemSounds.Exclamation.Play();
            }
            else if (Settings.Default.AlertSound == "Hand")
            {
                SystemSounds.Hand.Play();
            }
            else if (Settings.Default.AlertSound == "Question")
            {
                SystemSounds.Question.Play();
            }
        }

        private void MarkSpecial(float pZ, bool NPCDepthFilter, float x, float y, bool ShowRings, Spawninfo sp)
        {
            if (ShowRings && (!NPCDepthFilter || ((sp.Z > pZ - filterneg) && (sp.Z < pZ + filterpos))))
            {
                // Draw Ring around Bankers

                if (sp.Class == 40)
                {
                    DrawEllipse(new Pen(new SolidBrush(Color.White)), x - SpawnSizeOffset, y - SpawnSizeOffset, SpawnSize, SpawnSize);

                    DrawEllipse(new Pen(new SolidBrush(Color.Green)), x - PlusSzOZ, y - PlusSzOZ, SpawnPlusSize, SpawnPlusSize);
                }

                // Draw Ring around Guild Master

                if (sp.Class > 19 && sp.Class < 35)
                {
                    DrawEllipse(new Pen(new SolidBrush(Color.White)), x - SpawnSizeOffset, y - SpawnSizeOffset, SpawnSize, SpawnSize);

                    DrawEllipse(new Pen(new SolidBrush(Color.Red)), x - PlusSzOZ, y - PlusSzOZ, SpawnPlusSize, SpawnPlusSize);
                }

                // Draw Ring around Shopkeepers

                if (sp.Class == 41)
                {
                    DrawEllipse(new Pen(new SolidBrush(Color.White)), x - SpawnSizeOffset, y - SpawnSizeOffset, SpawnSize, SpawnSize);

                    DrawEllipse(new Pen(new SolidBrush(Color.Blue)), x - PlusSzOZ, y - PlusSzOZ, SpawnPlusSize, SpawnPlusSize);
                }
            }
        }

        private void DrawFlashes(float pZ, bool NPCDepthFilter, float x, float y, Spawninfo sp)
        {
            if (flash)
            {
                var x1 = x - PlusSzOZ;
                var y1 = y - PlusSzOZ;
                var above = (sp.Z < pZ + filterpos);
                var below = (sp.Z > pZ - filterneg);

                // Draw Ring around Hunted Mobs
                var notdepthfiltered = (!NPCDepthFilter || (below && above));

                if ((sp.isHunt || sp.proxAlert) && notdepthfiltered)
                {
                    DrawEllipse(new Pen(new SolidBrush(Color.LimeGreen), 2), x1, y1, SpawnPlusSize, SpawnPlusSize);
                }

                // Draw Ring around Caution Mobs

                if (sp.isCaution && notdepthfiltered)
                {
                    DrawEllipse(new Pen(new SolidBrush(Color.Yellow), 2), x1, y1, SpawnPlusSize, SpawnPlusSize);
                }

                // Draw Ring around Danger Mobs

                if (sp.isDanger && notdepthfiltered)
                {
                    DrawEllipse(new Pen(new SolidBrush(Color.Red), 2), x1, y1, SpawnPlusSize, SpawnPlusSize);
                }

                // Draw Ring around Rare Mobs

                if (sp.isAlert && notdepthfiltered)
                {
                    DrawEllipse(new Pen(new SolidBrush(Color.White), 2), x1, y1, SpawnPlusSize, SpawnPlusSize);
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

        private void SetLookupValues()
        {
            LookupRingSize = SpawnPlusSize + (skittle / (float)UpdateSteps * SelectSize);
            LookupRingOffset = LookupRingSize / 2.0f;
            lookup_set = true;
        }

        #region DrawMap

        //public void SetDistinctPens()
        //{
        //    // setup the distinct pens

        //    // the goal is to create as few pens as possible at runtime for performance reasons

        //    // Always set up distinct colors.  But will only use map colors if not going with the few.
        //    darkPen = eq.GetDistinctColor(new Pen(Color.Black));
        //    lightPen = eq.GetDistinctColor(new Pen(Color.FromArgb(32, darkPen.Color)));

        //    drawPen = darkPen;

        //    darkBrush = eq.GetDistinctColor(distinctBrush);
        //}

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
                        foreach (MapLine mapLine in eq.GetLinesReadonly())
                        {
                            DrawLines(mapLine.draw_color, mapLine.linePoints);
                        }
                    }
                    else
                    {
                        MinMaxFilter(out var minZ, out var maxZ);

                        foreach (MapLine mapLine in eq.GetLinesReadonly())
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

                                    lastZ = curZ;

                                    lastValid = curValid;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { LogLib.WriteLine("Error in DrawMapLines(): ", ex); }
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

                foreach (MapText t in eq.GetTextsReadonly())
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
                foreach (MapText t in eq.GetTextsReadonly())
                {
                    AddTextToDrawnMap(t);
                }
            }
        }

        private void AddTextToDrawnMap(MapText t)
        {
            var x_cord = (int)CalcScreenCoordX(t.x);
            var y_cord = (int)CalcScreenCoordY(t.y);
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

        #region DrawPlayer

        public void DrawPlayer(float gamerX, float gamerY, float SpawnSize, float SpawnSizeOffset, DrawOptions DrawOpts)
        {
            try
            {
                var xHead = (int)eq.gamerInfo.Heading;

                // Draw Range Circle

                if (Settings.Default.RangeCircle > 0)

                {
                    var rCircleRadius = Settings.Default.RangeCircle * m_ratio;

                    if (Settings.Default.ColorRangeCircle)

                    {
                        MakeRangeCircle(gamerX, gamerY, rCircleRadius);
                    }

                    // Draw Red V in the Range Circle

                    if (Settings.Default.DrawFoV && xHead >= 0 && xHead < 512)

                    {
                        DrawFoV(gamerX, gamerY, out var x, out var y, xHead, rCircleRadius);
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
                    Pen PCBorder = new Pen(new SolidBrush(Settings.Default.PCBorderColor));

                    FillRectangle(new SolidBrush(Color.White), gamerX - SpawnSizeOffset, gamerY - SpawnSizeOffset, SpawnSize, SpawnSize);

                    DrawRectangle(PCBorder, gamerX - SpawnSizeOffset - 0.5f, gamerY - SpawnSizeOffset - 0.5f, SpawnSize + 1.0f, SpawnSize + 1.0f);
                }
            }
            catch (Exception ex) { LogLib.WriteLine("Error in DrawPlayer(): ", ex); }
        }

        private void DrawFoV(float gamerX, float gamerY, out float x, out float y, int xHead, float rCircleRadius)
        {
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
            HatchStyle hs = (HatchStyle)Enum.Parse(typeof(HatchStyle), Settings.Default.HatchIndex, true);

            HatchBrush hatchBrush = new HatchBrush(hs, Settings.Default.RangeCircleColor, Color.Transparent);

            FillEllipse(hatchBrush, gamerx - rCircleRadius, gamery - rCircleRadius, rCircleRadius * 2, rCircleRadius * 2);
        }

        #endregion DrawPlayer

        #region DrawSpawnTimers

        public void DrawSpawnTimers()
        {
            try
            {
                MinMaxFilter(out var minZ, out var maxZ);

                // Draw Spawn Timers

                Pen pen = new Pen(new SolidBrush(Color.LightGray));

                foreach (Spawntimer st in eq.mobsTimers.GetRespawned().Values)
                {
                    if (st.zone != eq.shortname)
                    {
                        continue;
                    }

                    var stX = (float)Math.Round(CalcScreenCoordX(st.X), 0);

                    var stY = (float)Math.Round(CalcScreenCoordY(st.Y), 0);

                    var stOffset = PlusSzOZ - 0.5f;

                    var checkTimer = st.SecondsUntilSpawn(DateTime.Now);

                    var canDraw = false;

                    if (checkTimer == 0)
                    {
                        canDraw = true;
                    }

                    if (checkTimer > 0)

                    {
                        canDraw = true;

                        // Set Pen Colors

                        if (checkTimer < 30)

                        {
                            if (flash)
                            {
                                pen = new Pen(new SolidBrush(Color.Red));
                            }
                        }
                        else if (checkTimer < 60)

                        {
                            pen = new Pen(new SolidBrush(Color.Red));
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

                    // if depth filter on make adjustments to spawn points

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

                    if (canDraw)

                    {
                        DrawLine(pen, stX - stOffset, stY, stX + stOffset, stY);
                        DrawLine(pen, stX, stY - stOffset, stX, stY + stOffset);

                        if (Settings.Default.SpawnCountdown && (checkTimer > 0) && (checkTimer < 120))
                        {
                            DrawSpawnNames(textBrush, checkTimer.ToString(), st.X, st.Y);//, "");
                        }
                    }

                    // Draw Blue Line to selected spawn location

                    if ((st.X == eq.SpawnX) && (st.Y == eq.SpawnY))

                    {
                        GamerMapPos(out var gamerx, out var gamery);

                        pen = new Pen(new SolidBrush(Color.Blue));

                        DrawLine(pen, gamerx, gamery, stX, stY);

                        // Update the Spawn Information Window

                        lblMobInfo.Text = TimerInfo(st);
                    }
                }
            }
            catch (Exception ex) { LogLib.WriteLine("Error in DrawSpawnTimers(): ", ex); }
        }

        private void GamerMapPos(out float gamerx, out float gamery)
        {
            gamerx = CalcScreenCoordX(eq.gamerInfo.X);
            gamery = CalcScreenCoordY(eq.gamerInfo.Y);
        }

        #endregion DrawSpawnTimers

        #region DrawGroundItems

        public void DrawGroundItems(float pZ)

        {
            var GroundItemDepthFilter = Settings.Default.DepthFilter && Settings.Default.FilterGroundItems;

            try

            {
                float x, y;

                // Draw Ground Spawns

                foreach (GroundItem gi in eq.GetItemsReadonly())

                {
                    x = (float)Math.Round(CalcScreenCoordX(gi.X), 0);

                    y = (float)Math.Round(CalcScreenCoordY(gi.Y), 0);

                    if (!GroundItemDepthFilter || ((gi.Z > pZ - filterneg) && (gi.Z < pZ + filterpos)))
                    {
                        Pen yellowPen = new Pen(new SolidBrush(Color.Yellow));
                        gi.filtered = false;

                        DrawLine(yellowPen, x - PlusSzOZ, y - PlusSzOZ, x + PlusSzOZ, y + PlusSzOZ);

                        DrawLine(yellowPen, x - PlusSzOZ, y + PlusSzOZ, x + PlusSzOZ, y - PlusSzOZ);
                    }
                    else
                    {
                        gi.filtered = true;
                    }

                    // Draw Yellow Line to selected ground item location
                    PointF GIpos = new PointF(gi.X, gi.Y);
                    var spawnXY = eq.SpawnX == gi.X && eq.SpawnY == gi.Y;
                    if (spawnXY && eq.selectedID == 99999)
                    {
                        GamerMapPos(out var playerx, out var playery);

                        DrawLine(new Pen(new SolidBrush(Color.Yellow)), playerx, playery, x, y);

                        DrawEllipse(new Pen(new SolidBrush(Color.Fuchsia)), x - SelectSizeOffset, y - SelectSizeOffset, SelectSize, SelectSize);

                        // Update the Spawn Information Window

                        lblMobInfo.Text = GroundItemInfo(gi);
                    }

                    if (flash)
                    {
                        FlashAlertGroundSpawns(pZ, GroundItemDepthFilter, x, y, gi);
                    }
                }
            }
            catch (Exception ex) { LogLib.WriteLine("Error in DrawGroundItems(): ", ex); }
        }

        private void FlashAlertGroundSpawns(float pZ, bool GroundItemDepthFilter, float x, float y, GroundItem gi)
        {
            var Depthfilter = (!GroundItemDepthFilter || ((gi.Z > pZ - filterneg) && (gi.Z < pZ + filterpos)));
            var x1 = x - PlusSzOZ - 1;
            var y1 = y - PlusSzOZ - 1;
            var width = SpawnPlusSize + 2;
            var height = SpawnPlusSize + 2;

            // Draw Yellow Ring around Caution Ground Items
            if (gi.isCaution && Depthfilter)
            {
                DrawEllipse(new Pen(new SolidBrush(Color.Yellow), 2), x1, y1, width, height);
            }
            // Draw Red Ring around Danger Ground Items
            if (gi.isDanger && Depthfilter)
            {
                DrawEllipse(new Pen(new SolidBrush(Color.Red), 2), x1, y1, width, height);
            }

            // Draw White Ring around Rare Ground Items
            if (gi.isAlert && Depthfilter)
            {
                DrawEllipse(new Pen(new SolidBrush(Color.White), 2), x1, y1, width, height);
            }

            // Draw Cyan Ring around Hunt Ground Items
            if (gi.isHunt && Depthfilter)
            {
                DrawEllipse(new Pen(new SolidBrush(Color.Green), 2), x1, y1, width, height);
            }
        }

        #endregion DrawGroundItems

        #region DrawSpawnTrails

        public void DrawSpawnTrails()
        {
            try

            {
                // Draw Mob Trails

                foreach (MobTrailPoint mtp in eq.GetMobTrailsReadonly())
                {
                    FillEllipse(new SolidBrush(Color.White), CalcScreenCoordX(mtp.x) - 2, CalcScreenCoordY(mtp.y) - 2, 2, 2);
                }
            }
            catch (Exception ex) { LogLib.WriteLine("Error in DrawSpawnTrails(): ", ex); }
        }

        #endregion DrawSpawnTrails

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

        public void MapReset()

        {
            f1.mapPane.scale.Value = 100M;

            scale = 1.0f;

            f1.mapPane.offsetx.Value = 0;

            f1.mapPane.offsety.Value = 0;

            f1.mapPane.filterzneg.Value = 75;

            f1.mapPane.filterzpos.Value = 75;

            ClearPan();
        }

        //public float XAadjust => x_adjust;

        //public float YAadjust => y_adjust;

        private void MapChanged(EQMap map)

        {
            DrawOptions DrawOpts = f1.DrawOpts;

            // if the autoexpand is not checked, scale is not at 100, then maintain the map scale
            if (eq.longname.Length > 0 && mapPane != null && !Settings.Default.AutoExpand &&
                mapPane.scale.Value != 100)
            {
                var mapWidth = Math.Abs(eq.maxx - eq.minx);
                var mapHeight = Math.Abs(eq.maxy - eq.miny);
                var ScreenWidth = Width - (2.0f * 15);
                var ScreenHeight = Height - (2.0f * 15);
                var xratio = (float)ScreenWidth / mapWidth;
                var yratio = (float)ScreenHeight / mapHeight;
                var r_ratio = xratio < yratio ? xratio : yratio;

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
                        //scale = (float)mapPane.scale.Value / 100.0f;
                    }
                }
                else
                {
                    f1.mapPane.scale.Value = 100M;
                    scale = 1.0f;
                }
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

        private void VerifyTextExtents(DrawOptions DrawOpts)
        {
            float factor = 1;

            float xlabel = 0;

            float ylabel = 0;

            if (m_ratio > 0)
            {
                factor = 1 / m_ratio;
            }

            if ((DrawOpts & DrawOptions.GridLines) != DrawOptions.None)

            {
                // drawing gridlines, so account for grid labels

                ylabel = drawFont.GetHeight() + 0;

                xlabel = bkgBuffer.Graphics.MeasureString("10000", drawFont).Width;
            }

            foreach (MapText t in eq.GetTextsReadonly())

            {
                SizeF tf = bkgBuffer.Graphics.MeasureString(t.label, drawFont);
                if (t.size == 1)
                {
                    bkgBuffer.Graphics.MeasureString(t.label, drawFont1);
                }
                else if (t.size == 3)
                {
                    bkgBuffer.Graphics.MeasureString(t.label, drawFont3);
                }

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

        //public void UpdatePCBorder()
        //{
        //    Color curPCBorder = Settings.Default.PCBorderColor;

        //    if (curPCBorder != PCBorder.Color)
        //    {
        //        PCBorder = new Pen(new SolidBrush(Settings.Default.PCBorderColor));
        //    }
        //}

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