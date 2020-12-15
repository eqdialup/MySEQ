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



// Class Files

using Structures;



namespace myseq {

    public enum FollowOption{None, Player, Target};



    public class MapCon : System.Windows.Forms.UserControl {

    

        // Events

        public delegate void SelectPointHandler(SPAWNINFO playerinfo,double selectedX,double selectedY);

        public event SelectPointHandler SelectPoint; // Fires when the user clicks the map (without a mob)

        

        protected void OnSelectPoint(SPAWNINFO playerinfo,double selectedX,double selectedY)        

        {

            if (SelectPoint!=null)

            {

                SelectPoint(playerinfo,selectedX,selectedY);

            }

        }

        

        private System.ComponentModel.Container components = null;

        public System.Windows.Forms.Label lblMobInfo;



        public Font drawFont;
        public Font drawFont1;
        public Font drawFont3;



        // Hand relocation variables

        private Cursor hCurs = null;

        private bool m_dragging;

        private bool m_rangechange;

        private int m_dragStartX = 0;

        private int m_dragStartY = 0;

        private float m_dragStartPanX = 0;

        private float m_dragStartPanY = 0;

        private bool lookup_set = false;

        private Color gridColor;

        private Color gridLabelColor;

        private SolidBrush gridBrush;

        private Pen gPen;

        private Pen cPen = new Pen(new SolidBrush(Color.White));

        private Pen drawPen;

        private int skittle = 0;

        private int flash_count = 0;

        // m_zoom - factor by which map has been zoomed.

        private float m_zoom = 1.0f;



        // m_panOffset define how far map has been dragged.

        public float m_panOffsetX = 0;

        public float m_panOffsetY = 0;

        private int collect_mobtrails_count = 0;

        // m_ratio - adjustment factor required to convert map->screen size.

        float m_ratio = 1.0f;



        // m_mapCenter - centre point of screen in Map Units.

        private float m_mapCenterX = 0;

        private float m_mapCenterY = 0;



        // m_screenCenter - centre point of screen in Screen Units.

        private float m_screenCenterX = 0;

        private float m_screenCenterY = 0;

        private float x_adjust = 0;

        private float y_adjust = 0;

        // Spawn Sizes
        private int SettingsSpawnSize = 3;

        private float SpawnSize = 5.0f;

        private float SpawnSizeOffset = 2.5f;

        private float SpawnPlusSize = 7.0f;

        private float SpawnPlusSizeOffset = 3.5f;

        private float SelectSize = 9.0f;

        private float SelectSizeOffset = 4.5f;

        private float LookupRingSize = 7.0f;

        private float LookupRingOffset = 3.5f;

        public float scale = 1.0f;



        public int filterpos = 0;

        public int filterneg = 0;

        private float selectedX = -1;          // [42!] Mark an arbitrary spot on the map

        private float selectedY = -1;          // Don't set these directly, but use SetSelectedPoint/ClearSelectedPoint

        private string curTarget = "";



        BufferedGraphicsContext gfxManager = null;

        public BufferedGraphics bkgBuffer = null;



        private ToolTip tt = null;

        

        public bool flash; // used for flashing warning lights  



        private Pen redPen = new Pen(new SolidBrush(Color.Red));
        private Pen red2Pen = new Pen(new SolidBrush(Color.Red),2);

        private Pen topRedPen = new Pen(Color.FromArgb(64, Color.Red));

        private Pen orangePen = new Pen(new SolidBrush(Color.Orange));

        private Pen topOrangePen = new Pen(Color.FromArgb(64, Color.Orange));

        private Pen bluePen = new Pen(new SolidBrush(Color.Blue));
        private Pen blue2Pen = new Pen(new SolidBrush(Color.Blue), 2);

        private Pen whitePen = new Pen(new SolidBrush(Color.White));
        private Pen white2Pen = new Pen(new SolidBrush(Color.White),2);

        private Pen greenPen = new Pen(new SolidBrush(Color.Green));

        private Pen green2Pen = new Pen(new SolidBrush(Color.LimeGreen),2);

        private Pen yellowPen = new Pen(new SolidBrush(Color.Yellow));
        private Pen yellow2Pen = new Pen(new SolidBrush(Color.Yellow), 2);

        private Pen topYelloPen = new Pen(Color.FromArgb(64, Color.Yellow));

        private Pen ltgrayPen = new Pen(new SolidBrush(Color.LightGray));

        private Pen topLtgrayPen = new Pen(Color.FromArgb(64, Color.LightGray));

        private Pen thickWhitePen = new Pen(new SolidBrush(Color.White),2);

        private Pen ltbluePen = new Pen(new SolidBrush(Color.LightSteelBlue), 1);

        private Pen cyanPen = new Pen(new SolidBrush(Color.Cyan));

        private Pen pinkPen = new Pen(new SolidBrush(Color.Fuchsia));

        private Pen purplePen = new Pen(new SolidBrush(Color.Purple));

        private Pen lightPen = new Pen(new SolidBrush(Color.Gray));

        private Pen darkPen = new Pen(new SolidBrush(Color.Gray));

        private Pen grayPen = new Pen(new SolidBrush(Color.Gray));

        private Pen PCBorder = new Pen(new SolidBrush(Color.Fuchsia));

        private SolidBrush distinctBrush = new SolidBrush(Color.Black);

        private SolidBrush darkBrush = new SolidBrush(Color.Black);

        private SolidBrush whiteBrush = new SolidBrush(Color.White);

        private SolidBrush purpleBrush = new SolidBrush(Color.Purple);

        private SolidBrush greyBrush = new SolidBrush(Color.LightGray);

        private SolidBrush grayBrush = new SolidBrush(Color.Gray);

        private SolidBrush redBrush = new SolidBrush(Color.Red);

        private SolidBrush yellowBrush = new SolidBrush(Color.Yellow);

        private SolidBrush textBrush = new SolidBrush(Color.LightGray);



        //private ArrayList MobTrails {get{return mobtrails;}}

        //private ArrayList Items {get{return items;}}

        //private SPAWNINFO PlayerInfo {get{return eq.playerinfo;} set{eq.playerinfo = value;}}

        //private Hashtable Mobs {get{return mobs;}}

        //private Hashtable MobsTimer {get{return mobsTimer;}}



       

        private frmMain f1 = null;          // Caution: this may be null

        private MapPane mapPane = null;     // Caution: this may be null

        

        private EQData eq;



        private DateTime LastTTtime = DateTime.Now;



        private int fpsCount = 0;

        private DateTime fpsLastReadTime = new DateTime();

        

        private TableLayoutPanel tableLayoutPanel1;

        public Label lblGameClock;

        private double fpsValue = 0;

        private float []xSin = new float[512];

        private float []xCos = new float[512];

        private int update_steps = 5;

        private int update_ticks = 1;

        public int UpdateSteps { get { return update_steps; } set { update_steps = value; } }

        public int UpdateTicks { get { return update_ticks; } set { update_ticks = value; } }

        public MapCon() {

               

            InitializeComponent();

            InitializeVariables();



            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            this.SetStyle(ControlStyles.UserPaint, true);

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);



            gfxManager = BufferedGraphicsManager.Current;

            

            drawPen = darkPen;

        }

        

        public void SetComponents(frmMain f1,MapPane mapPane,EQData eq,EQMap map)

        {

            this.f1 = f1;

            this.mapPane = mapPane;

            this.eq = eq;

            map.EnterMap += new EQMap.EnterMapHandler(MapChanged);

            Invalidate();

        }

        public void onResize() {

            if (Width > 0 && Height > 0) {

                if (bkgBuffer != null)

                    bkgBuffer.Dispose();

                gfxManager.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);

                bkgBuffer = gfxManager.Allocate(this.CreateGraphics(), new Rectangle(0, 0, this.Width + 1, this.Height + 1));

                bkgBuffer.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

                this.Refresh();

            }

        }

        protected override void OnPaintBackground(PaintEventArgs e)

        {

        }

        protected override void OnPaint(PaintEventArgs e)

        {

            double fpsTimeDelta;

            if (bkgBuffer == null)

                return;

            bkgBuffer.Render(e.Graphics);

            base.OnPaint(e);

            

            // Calculate FPS

            if ((fpsTimeDelta = ((TimeSpan)(DateTime.Now - fpsLastReadTime)).Seconds) > 0.5)

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



        protected override void Dispose(bool disposing) {

            if( disposing && components != null) 

                components.Dispose();



            base.Dispose( disposing );

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
            this.lblMobInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMobInfo.BackColor = System.Drawing.Color.White;
            this.lblMobInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMobInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.lblGameClock.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGameClock.BackColor = System.Drawing.Color.BlueViolet;
            this.lblGameClock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblGameClock.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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

        #endregion

       

        private void InitializeVariables() {

            hCurs = Cursors.Hand;//new Cursor("Hand.cur");

            // Initialize DragVariables to 0,0 and set semiphore false

            m_dragging = false;

            m_rangechange = false;

            m_dragStartX = m_dragStartY = 0;

            m_panOffsetX = m_panOffsetY = 0;

            

            ClearSelectedPoint();



            tt = new ToolTip();

            tt.AutomaticDelay = 250;

            tt.SetToolTip(this, "ABCD\nEFGH");

            tt.Active = true;



            // Set sine and cosine values to use with headings

            for (int p = 0; p < 512; p++)

            {

                xCos[p]= (float)Math.Cos( (float) p / 512.0f * 2.0f * Math.PI );

                xSin[p]= (float)Math.Sin( (float) p / 512.0f * 2.0f * Math.PI );

            }

            textBrush = greyBrush;

        }

        public void SetInitialParams()

        {

            ReAdjust();            

        }

       

        public void ClearSelectedPoint()

        {

            SetSelectedPoint(-1,-1);            

        }

        

        private void SetSelectedPoint(float x,float y)

        {

            this.selectedX = x;

            this.selectedY = y;

            

            if (eq!=null)

            {

                OnSelectPoint(eq.playerinfo,x,y);

            }

        }

        

        private void MapCon_MouseScroll(object sender, MouseEventArgs me) {

        

            if (mapPane == null) return;

            

            float newScale = scale + me.Delta / 600.0f;

            if (newScale >= 0.1) 

                f1.mapPane.scale.Value = (decimal)(newScale*100);



            ReAdjust();



            Invalidate();

        }



        private void MapCon_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e) {

            if (mapPane == null) return;

            

            if (e.KeyChar == '+') {

                scale = scale+0.2f;

                mapPane.scale.Value = (decimal)(scale*100);

                Invalidate();

            } else if (e.KeyChar == '-') {

                if (scale-0.2 >= 0.1) {

                    scale = scale-0.2f;

                    mapPane.scale.Value = (decimal)(scale*100);

                }

                Invalidate();

            } else if (char.ToLower(e.KeyChar) == 'c') {

                mapPane.offsetx.Value=0;

                mapPane.offsety.Value=0;

                e.Handled = true;

            } else if (char.ToLower(e.KeyChar) == '4') {

                mapPane.offsetx.Value-=50;

                e.Handled = true;

            } else if (char.ToLower(e.KeyChar) == '7') {

                mapPane.offsetx.Value-=50;

                mapPane.offsety.Value-=50;

                e.Handled = true;

            } else if (char.ToLower(e.KeyChar) == '8') {

                mapPane.offsety.Value-=50;

                e.Handled = true;

            } else if (char.ToLower(e.KeyChar) == '9') {

                mapPane.offsety.Value-=50;

                mapPane.offsetx.Value+=50;

                e.Handled = true;

            } else if (char.ToLower(e.KeyChar) == '6') {

                mapPane.offsetx.Value+=50;

                e.Handled = true;

            } else if (char.ToLower(e.KeyChar) == '3') {

                mapPane.offsety.Value+=50;

                mapPane.offsetx.Value+=50;

                e.Handled = true;

            } else if (char.ToLower(e.KeyChar) == '2') {

                mapPane.offsety.Value+=50;

                e.Handled = true;

            } else if (char.ToLower(e.KeyChar) == '1') {

                mapPane.offsety.Value+=50;

                mapPane.offsetx.Value-=50;

                e.Handled = true;

            } else if (char.ToLower(e.KeyChar) == '5') {

                mapPane.offsetx.Value=0;

                mapPane.offsety.Value=0;

                e.Handled = true;

            }

            ReAdjust();

        }



        private void MapCon_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
            bool emailmenu = true;
            if (e.Button == MouseButtons.Left) {

                // Range Circle Checks

                if (Settings.Instance.RangeCircle > 0)
                {

                    float rCircleRadius = Settings.Instance.RangeCircle;

                    float upperRadius = rCircleRadius + (4 * SpawnSize);

                    float lowerRadius = rCircleRadius - (4 * SpawnSize);

                    if (lowerRadius < 0)

                        lowerRadius = 0;



                    // Calc the proper loc for the mouse

                    float mousex = ScreenToMapCoordX((float)e.X);

                    float mousey = ScreenToMapCoordY((float)e.Y);



                    // if within approximately one mob radius of the Range Circle

                    // then we are resizing range circle, and not dragging.



                    float sd = (float)Math.Sqrt(((mousey - eq.playerinfo.Y) * (mousey - eq.playerinfo.Y)) +

                        ((mousex - eq.playerinfo.X) * (mousex - eq.playerinfo.X)));



                    if ((Settings.Instance.ColorRangeCircle) && (sd > lowerRadius) && (sd < upperRadius))
                    {

                        // changing range cirlce size

                        m_rangechange = true;

                    }
                }

                if (!m_rangechange)

                {

                    System.Windows.Forms.Cursor.Current = hCurs;



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
                float x = ScreenToMapCoordX((float)e.X);

                float y = ScreenToMapCoordY((float)e.Y);

                SPAWNINFO sp = eq.FindMobNoPetNoPlayerNoCorpse((float)(5.0 / m_ratio), x, y);

                if ((sp != null) && (sp.Name.Length > 0))
                {
                    f1.alertAddmobname = filterMobName(sp.Name);
                    f1.alertAddmobname = f1.alertAddmobname.Replace("_", " ");
                    f1.alertAddmobname = f1.alertAddmobname.TrimEnd(' ');
                    f1.alertX = sp.X;
                    f1.alertY = sp.Y;
                    f1.alertZ = sp.Z;
                }
                else
                {
                    GroundItem gi = eq.FindGroundItem((float)(5.0 / m_ratio), x, y);
                    if ((gi != null) && (gi.Name.Length > 0))
                    {
                        eq.GetItemDescription(gi.Name);
                        f1.alertAddmobname = eq.GetItemDescription(gi.Name);
                        f1.alertX = gi.X;
                        f1.alertY = gi.Y;
                        f1.alertZ = gi.Z;
                        emailmenu = false;
                    }
                    else
                    {
                        f1.alertAddmobname = "";
                        SPAWNTIMER st = eq.FindTimer(5.0f, x, y);
                        if (st != null)
                        {
                            string[] names = st.allNames.Split(',');
                            foreach (string name in names)
                            {
                                string bname = Regex.Replace(name.Replace("_", " "), "[0-9]", "").Trim();
                                if (bname.Length > 0 && f1.alertAddmobname.Length == 0)
                                    f1.alertAddmobname = bname;
                                if (Regex.IsMatch(bname, "^[A-Z#]"))
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
                            sp = eq.FindMobNoPetNoPlayer((float)(5.0 / m_ratio), x, y);

                            if ((sp != null) && (sp.Name.Length > 0))
                            {
                                f1.alertAddmobname = filterMobName(sp.Name);
                                f1.alertAddmobname = f1.alertAddmobname.Replace("_", " ");
                                f1.alertAddmobname = f1.alertAddmobname.TrimEnd(' ');
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

                f1.SetContextMenu(emailmenu);
            }

        }

        private static string filterMobName(string name)
        {

            return Regex.Replace(name, "^*[^a-zA-Z_ #'`]", "");

        }

        public static string FixMobName(string name)
        {

            // if first char is "_" dont replace

            if (name.IndexOf("_") == 0)

                return name;

            else

                return name.Replace("_", " ");

        }

        private void MapCon_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e) 

        {

       

            float x = ScreenToMapCoordX((float)e.X);

            float y = ScreenToMapCoordY((float)e.Y);

            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                float delta = 5.0f / m_ratio;

                SPAWNINFO sp = eq.FindMobNoPet(delta, x, y);

                if (sp == null)
                    sp = eq.FindMob(delta, x, y);

                if (sp != null)
                {

                    SPAWNINFO st = eq.FindMobTimer(sp.SpawnLoc);

                    if (st == null)
                    {
                        eq.SetSelectedID((int)sp.SpawnID);

                        eq.SpawnX = -1.0f;

                        eq.SpawnY = -1.0f;
                    }
                    else
                    {
                        eq.SetSelectedID((int)st.SpawnID);
                        SPAWNTIMER spt = eq.FindTimer(1.0f, st.X, st.Y);
                        if (spt != null && spt.itmSpawnTimerList != null)
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
                        eq.SelectGroundItem((float)(5.0 / m_ratio), x, y);
                }
            }
            else if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            {

                // [42!] Mark an arbitrary spot on the map, or turn it off if a spot was marked already.

                if (this.selectedX == -1)
                {

                    SetSelectedPoint(x, y);

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

                    if (!eq.SelectMob((float)(5.0 / m_ratio), x, y))
                        if (!eq.SelectTimer((float)(5.0 / m_ratio), x, y))
                            eq.SelectGroundItem((float)(5.0 / m_ratio), x, y);

                    Invalidate();

                }

            }

            m_dragging = false;

            m_rangechange = false;

            m_dragStartX = m_dragStartY = 0;

            //rclick = false;

            

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

        }



        private void MapCon_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)

        {

            if (mapPane == null || f1 == null) return;



            // Limit TT popups to four times a sec

            TimeSpan interval = DateTime.Now.Subtract(LastTTtime);

            if (interval.TotalSeconds < 0.25)

            {

                return;

            }

            // Calc the proper loc for the mouse

            float mousex = ScreenToMapCoordX((float)e.X);

            float mousey = ScreenToMapCoordY((float)e.Y);

            // Range

            float sd = (float)Math.Sqrt(((mousey - eq.playerinfo.Y) * (mousey - eq.playerinfo.Y)) +

                ((mousex - eq.playerinfo.X) * (mousex - eq.playerinfo.X)));

            f1.toolStripMouseLocation.Text = String.Format("Map /loc: {0:f2}, {1:f2}", mousey, mousex);

            f1.toolStripDistance.Text = String.Format("Distance: {0:f1}", sd);

            // If we are dragging, then change the origin.

            if (m_dragging)

            {

                // Compute delta x,y from original click

                int dx = m_dragStartX - e.X;

                int dy = m_dragStartY - e.Y;

                mapPane.offsetx.Value = -(decimal)(m_dragStartPanX - (float)dx);

                mapPane.offsety.Value = -(decimal)(m_dragStartPanY - (float)dy);

                ReAdjust();

                Invalidate();

            }

            else

            {

                float x = ScreenToMapCoordX((float)e.X);

                float y = ScreenToMapCoordY((float)e.Y);

                bool found = false;

                float delta;

                delta = 5.0f / m_ratio;

                SPAWNINFO sp = eq.FindMobNoPet(delta, x, y);

                if (sp == null)

                    sp = eq.FindMob(delta, x, y);

                if (sp == null)

                {

                    found = false;

                }

                else

                {

                    found = true;

                    LastTTtime = DateTime.Now;

                    tt.SetToolTip(this, MobInfo(sp, false,false));

                    tt.AutomaticDelay = 0;

                    tt.Active = true;

                }



                if (!found)

                {

                    GroundItem gi = eq.FindGroundItem(delta, x, y);

                    if (gi != null)

                    {

                        string ItemName = gi.Name;

                        foreach (ListItem li in eq.itemList.Values)

                        {

                            if (gi.Name == li.ActorDef)

                            {

                                ItemName = li.Name;

                            }

                        }

                        string s = "Name: " + ItemName + "\n" + gi.Name;

                        tt.SetToolTip(this, s);

                        tt.AutomaticDelay = 0;

                        tt.Active = true;

                        LastTTtime = DateTime.Now;

                        found = true;

                    }

                }



                if (!found)

                {

                    SPAWNTIMER st = eq.mobsTimers.Find(delta, x, y);

                    if (st != null)

                    {

                        String description = st.GetDescription();

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

                    tt.SetToolTip(this, "");

            }

        }





        private void ClearPan() { 

            m_panOffsetX = 0; 

            m_panOffsetY = 0; 

            ReAdjust();

        }

        

        public void ReAdjust() {

            float mapWidth = Math.Abs(eq.maxx - eq.minx);

            float mapHeight = Math.Abs(eq.maxy - eq.miny);

            

            float border = 15;



            float ScreenWidth = Width - (2*border);

            float ScreenHeight = Height - (2*border);

            m_screenCenterX =  Width/2;

            m_screenCenterY = Height/2;

            

            float zoom = (float)scale;

            if (m_zoom > 32)

                m_zoom = 32;



            float xratio = (float)ScreenWidth / mapWidth;

            float yratio = (float)ScreenHeight / mapHeight;



            // Use the smaller scale ratio so that the map fits in the screen at a zoom of 1.

            if (xratio < yratio) {

                m_ratio = xratio * zoom;

            } else {

                m_ratio = yratio * zoom;

            }



            // Calculate the Map Center

            switch (Settings.Instance.FollowOption)

            {

                case FollowOption.None:

                    m_mapCenterX = eq.minx + (mapWidth/2);

                    m_mapCenterY = eq.miny + (mapHeight/2);

                    break;

                case FollowOption.Player:

                    m_mapCenterX = eq.playerinfo.X;

                    m_mapCenterY = eq.playerinfo.Y;

                    break;

                case FollowOption.Target:

                    SPAWNINFO siTarget = eq.GetSelectedMob();

                    if (siTarget != null)

                    {

                        m_mapCenterX = siTarget.X;

                        m_mapCenterY = siTarget.Y;

                        break;

                    }

                    else

                        goto case FollowOption.Player;



                    default:

                        goto case FollowOption.None;

            }



            // When Following a player or spawn and KeepCentered is not selected

            // adjust the map center so as to minimise the amount of blank space in the map window.



            if (Settings.Instance.KeepCentered==false && Settings.Instance.FollowOption!=FollowOption.None)

            {

                // Calculate the MapCordinates of the Screen Edges

                float ScreenMaxY, ScreenMinY, ScreenMinX, ScreenMaxX;

                float ScreenMapWidth, ScreenMapHeight;



                ScreenMaxY = ScreenToMapCoordY(border, true);

                ScreenMinY = ScreenToMapCoordY(Height - border, true);

                ScreenMapHeight = Math.Abs(ScreenMaxY - ScreenMinY);

                

                // X sense is wrong way round...

                ScreenMinX = ScreenToMapCoordX(Width - border, true);

                ScreenMaxX = ScreenToMapCoordX(border, true);

                ScreenMapWidth = Math.Abs(ScreenMaxX - ScreenMinX);

                

                if (mapWidth <= ScreenMapWidth)

                {

                    // If map fits in window set center to center of map

                    m_mapCenterX = eq.minx + (mapWidth/2);

                }

                else

                {

                    // if we have blank space to the left or right repostion the center point appropriately 

                    if (ScreenMinX<eq.minx)

                        m_mapCenterX += (eq.minx-ScreenMinX);

                    else if (ScreenMaxX>eq.maxx)

                        m_mapCenterX -= (ScreenMaxX-eq.maxx);

                }



                if (mapHeight <= ScreenMapHeight)

                {

                    // If map fits in window set center to center of map

                    m_mapCenterY = eq.miny + (mapHeight/2);

                }

                else

                {

                    // if we have blank space at the top or botton repostion the center point appropriately 

                    if (ScreenMinY<eq.miny)

                        m_mapCenterY += (eq.miny-ScreenMinY);

                    else if (ScreenMaxY>eq.maxy)

                        m_mapCenterY -= (ScreenMaxY-eq.maxy);

                }

            }
            x_adjust = m_panOffsetX + m_screenCenterX + (float)(m_mapCenterX * m_ratio);
            y_adjust = m_panOffsetY + m_screenCenterY + (float)(m_mapCenterY * m_ratio);

        }



        public float CalcScreenCoordX(float mapCoordinateX) {

            

            // Formula Should be 

            // Screen X =CenterScreenX + ((mapCoordinateX - MapCenterX) * m_ratio)

            // However Eq's Map coordinates are in the oposite sense to the screen

            // so we have to multiply the second portion by -1, which is the same

            // as changing the plus to a minus...



            //m_ratio = (ScreenWidth/MapWidth) * zoom (Calculated ahead of time in ReAdjust)

            //return m_panOffsetX + m_screenCenterX - ((mapCoordinateX - m_mapCenterX) * m_ratio);
            return x_adjust - (float)(mapCoordinateX * m_ratio);
        }



        public float CalcScreenCoordY(float mapCoordinateY) {

            //return m_panOffsetY + m_screenCenterY - ((mapCoordinateY - m_mapCenterY) * m_ratio);
            return y_adjust - (float)(mapCoordinateY * m_ratio);
        }



        private float ScreenToMapCoordX(float screenCoordX) 

        {

            return m_mapCenterX + ((m_panOffsetX + m_screenCenterX - screenCoordX) / m_ratio);

        }

        

        private float ScreenToMapCoordY(float screenCoordY) 

        {

            return m_mapCenterY + ((m_panOffsetY + m_screenCenterY - screenCoordY) / m_ratio);

        }

        private float ScreenToMapCoordX(float screenCoordX, bool IgnorePan) 

        {

            if (IgnorePan)

                return m_mapCenterX + ((m_screenCenterX - screenCoordX) / m_ratio);

            else

                return ScreenToMapCoordX(screenCoordX);

        }

        

        private float ScreenToMapCoordY(float screenCoordY, bool IgnorePan) 

        {

            if (IgnorePan)

                return m_mapCenterY + ((m_screenCenterY - screenCoordY) / m_ratio);

            else

                return ScreenToMapCoordY(screenCoordY);

        }

        private void DrawLine(Pen pen, float x1, float y1, float x2, float y2) 

        {

            try { bkgBuffer.Graphics.DrawLine(pen, x1, y1, x2, y2); }

            catch (Exception ex) {LogLib.WriteLine(String.Format("Error with DrawLine({0}, {1}, {2}, {3}): ", x1, y1, x2, y2), ex);}

        }

        private void DrawIntLine(Pen pen, int x1, int y1, int x2, int y2)
        {

            try { bkgBuffer.Graphics.DrawLine(pen, x1, y1, x2, y2); }

            catch (Exception ex) { LogLib.WriteLine(String.Format("Error with DrawIntLine({0}, {1}, {2}, {3}): ", x1, y1, x2, y2), ex); }

        }

        private void DrawLines(Pen pen, PointF[] points)

        {

            try { bkgBuffer.Graphics.DrawLines(pen,points); }

            catch (Exception ex) { LogLib.WriteLine("Error with DrawLines: ", ex); }

        }



        private void FillEllipse(Brush brush, float x1, float y1, float width, float height) {

             try {bkgBuffer.Graphics.FillEllipse(brush, x1, y1, width, height); }

            catch (Exception ex) {LogLib.WriteLine(String.Format("Error with FillEllipse({0}, {1}, {2}, {3}): ", x1, y1, width, height), ex);}

        }



        private void DrawEllipse(Pen pen, float x1, float y1, float width, float height) {

            //if (x1 != x1 || y1 != y1 || width != width || height != height) return;

            try {bkgBuffer.Graphics.DrawEllipse(pen, x1, y1, width, height); }

            catch (Exception ex) {LogLib.WriteLine(String.Format("Error with DrawEllipse({0}, {1}, {2}, {3}): ", x1, y1, width, height), ex);}

        }



        private void DrawTriangle(Pen pen, float x1, float y1, float radius){

            PointF[] points = { 

                new PointF (x1 + radius * 0.866025f ,y1 + radius * 0.5f),

                new PointF (x1 + radius * -0.866025f ,y1 + radius * 0.5f),

                new PointF (x1  ,y1 - radius),

                new PointF (x1 + radius * 0.866025f ,y1 + radius * 0.5f)};

            try {bkgBuffer.Graphics.DrawLines(pen,points); }

            catch (Exception ex) {LogLib.WriteLine(String.Format("Error with DrawTriangle({0}, {1}, {2}): ", x1, y1, radius), ex);}

        }



        private void FillTriangle(Brush brush, float x1, float y1, float radius){

            PointF[] points = { 

                new PointF (x1 + radius * 0.866025f ,y1 + radius * 0.5f),

                new PointF (x1 + radius * -0.866025f ,y1 + radius * 0.5f),

                new PointF (x1 ,y1 - radius)};

            try {bkgBuffer.Graphics.FillPolygon(brush,points); }

            catch (Exception ex) {LogLib.WriteLine(String.Format("Error with FillTriangle({0}, {1}, {2}): ", x1, y1, radius), ex);}

        }



        private void FillRectangle(Brush brush, float x1, float y1, float width, float height) {

            //if (x1 != x1 || y1 != y1 || width != width || height != height) return;



            try { bkgBuffer.Graphics.FillRectangle(brush, x1, y1, width, height); }

            catch (Exception ex) {LogLib.WriteLine(String.Format("Error with FillRectangle({0}, {1}, {2}, {3}): ", x1, y1, width, height), ex);}

        }

        

        private void DrawSpawnNames(Brush dBrush, String tName, float x1, float y1) {

            float xoffset;

            xoffset = bkgBuffer.Graphics.MeasureString(tName, drawFont).Width * 0.5f;

            try { bkgBuffer.Graphics.DrawString(tName, drawFont, dBrush, CalcScreenCoordX(x1) - xoffset, CalcScreenCoordY(y1) - SpawnSize - drawFont.GetHeight()); }

            catch (Exception ex) {LogLib.WriteLine(String.Format("Error with DrawSpawnNames({0}, {1}, {2}): ", tName, x1, y1 ), ex);}

        }            



        private void DrawRectangle(Pen pen, float x1, float y1, float width, float height) {

            //if (x1 != x1 || y1 != y1 || width != width || height != height) return;



            try { bkgBuffer.Graphics.DrawRectangle(pen, x1, y1, width, height); }

            catch (Exception ex) {LogLib.WriteLine(String.Format("Error with DrawRectangle({0}, {1}, {2}, {3}): ", x1, y1, width, height), ex);}

        }



        public void DrawArc(Pen pen, float x1, float y1, float width, float height, float startAngle, float sweepAngle) {

            try { bkgBuffer.Graphics.DrawArc(pen, x1, y1, width, height, startAngle, sweepAngle); }

            catch (Exception ex) {LogLib.WriteLine(String.Format("Error with DrawArc({0}, {1}, {2}, {3}, {4}, {5}): ", x1, y1, width, height, startAngle, sweepAngle), ex);}

        }



        private string MobInfo(SPAWNINFO si, bool SetColor, bool ChangeSize) {

            try {

            

                if (f1 == null) { return ""; }

                Graphics g;

                

                if (si == null)

                {

                    if (ChangeSize)

                    {

                        tableLayoutPanel1.RowStyles[0].SizeType = SizeType.Absolute;

                        tableLayoutPanel1.RowStyles[1].SizeType = SizeType.Absolute;



                        if (Settings.Instance.ShowTargetInfo)

                        {

                            g = lblMobInfo.CreateGraphics();

                            SizeF gt = new SizeF();
                            SizeF gf = new SizeF();

                            gt = g.MeasureString(lblGameClock.Text.ToString(), lblGameClock.Font);

                            gf = g.MeasureString("Spawn Information Window", lblMobInfo.Font);

                            g.Dispose();

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

                    lblMobInfo.BackColor = System.Drawing.Color.White;

                    return "Spawn Information Window";

                }



                float sd = (float)Math.Sqrt((si.X - eq.playerinfo.X)* (si.X - eq.playerinfo.X) + 

                    (si.Y - eq.playerinfo.Y) * (si.Y - eq.playerinfo.Y) + 

                    (si.Z - eq.playerinfo.Z) * (si.Z - eq.playerinfo.Z)); 

                                

                StringBuilder mobInfo = new StringBuilder();

                if (Settings.Instance.SmallTargetInfo)
                {
                    // small target window version
                    if (si.isMerc)

                        mobInfo.AppendFormat("Mercenary: {0}\n", si.Name);

                    else if (si.isPet)

                        mobInfo.AppendFormat("Pet: {0}\n", si.Name);

                    else if (si.isFamiliar)

                        mobInfo.AppendFormat("Familiar: {0}\n", si.Name);

                    else if (si.isMount)

                        mobInfo.AppendFormat("Mount: {0}\n", si.Name);

                    else if (si.m_isPlayer)

                        mobInfo.AppendFormat("Player: {0}\n", si.Name);

                    else if (si.isCorpse)

                        mobInfo.AppendFormat("Corpse: {0}\n", si.Name);

                    else

                        mobInfo.AppendFormat("NPC: {0}\n", si.Name);

                    mobInfo.AppendFormat("Level {0} / {1}\n", si.Level.ToString(), eq.hideNumToString(si.Hide) );

                    mobInfo.AppendFormat("{0} / {1}\n", eq.raceNumtoString(si.Race), eq.classNumToString(si.Class));

                    mobInfo.AppendFormat("Speed: {0:f3}  Dist: {1:f0}\n", si.SpeedRun, sd);

                    mobInfo.AppendFormat("Y: {0:f1} X: {1:f1} Z: {2:f1}", si.Y, si.X, si.Z);

                } else {
                    // long target window version
                    mobInfo.AppendFormat("Name: {0} ({1})\n", si.Name, si.SpawnID);

                    if (si.isMerc)

                        mobInfo.AppendFormat("Level: {0} (Mercenary)\n", si.Level.ToString());

                    else if (si.isPet)

                        mobInfo.AppendFormat("Level: {0} (Pet)\n", si.Level.ToString());

                    else if (si.isFamiliar)

                        mobInfo.AppendFormat("Level: {0} (Familiar)\n", si.Level.ToString());

                    else if (si.isMount)

                        mobInfo.AppendFormat("Level: {0} (Mount)\n", si.Level.ToString());

                    else

                        mobInfo.AppendFormat("Level: {0}\n", si.Level.ToString());

                    if (si.Primary > 0)

                        mobInfo.AppendFormat("Class: {0}    Primary: {1} ({2})\n", eq.classNumToString(si.Class), eq.itemNumToString(si.Primary), si.Primary);

                    else

                        mobInfo.AppendFormat("Class: {0}\n", eq.classNumToString(si.Class));

                    if (si.Offhand > 0)

                        mobInfo.AppendFormat("Race: {0}    Offhand: {1} ({2})\n", eq.raceNumtoString(si.Race), eq.itemNumToString(si.Offhand), si.Offhand);

                    else

                        mobInfo.AppendFormat("Race: {0}\n", eq.raceNumtoString(si.Race));

                    mobInfo.AppendFormat("Speed: {0:f3}\n", si.SpeedRun);

                    mobInfo.AppendFormat("Visibility: {0}\n", eq.hideNumToString(si.Hide));

                    mobInfo.AppendFormat("Distance: {0:f3}\n", sd);

                    mobInfo.AppendFormat("Y: {0:f3}  X: {1:f3}  Z: {2:f3}", si.Y, si.X, si.Z);

                }
                
                
                if (SetColor) {
                    
                    if (si.Level < (eq.greyRange + eq.playerinfo.Level)) lblMobInfo.BackColor = Color.LightGray;

                    else if (si.Level < (eq.greenRange + eq.playerinfo.Level)) lblMobInfo.BackColor = Color.PaleGreen;

                    else if (si.Level < (eq.cyanRange + eq.playerinfo.Level)) lblMobInfo.BackColor = Color.PowderBlue;

                    else if (si.Level < eq.playerinfo.Level) lblMobInfo.BackColor = Color.DeepSkyBlue;

                    else if (si.Level == eq.playerinfo.Level) lblMobInfo.BackColor = Color.White;

                    else if (si.Level <= eq.playerinfo.Level + eq.yellowRange) lblMobInfo.BackColor = Color.Yellow;

                    else lblMobInfo.BackColor = Color.Red;

                    if (si.isEventController)

                        lblMobInfo.BackColor = Color.Violet;

                    if (si.isLDONObject)

                        lblMobInfo.BackColor = Color.LightGray;

                }



                g = lblMobInfo.CreateGraphics();

                SizeF sf = new SizeF();
                SizeF sc = new SizeF();

                sf = g.MeasureString(mobInfo.ToString(), lblMobInfo.Font);

                sc = g.MeasureString(lblGameClock.Text.ToString(), lblGameClock.Font);

                g.Dispose();

                tableLayoutPanel1.ColumnStyles[0].SizeType = SizeType.Absolute;

                if (Settings.Instance.ShowTargetInfo)

                {

                    lblMobInfo.Visible = true;

                    if (ChangeSize)

                    {
                        int panel_width;
                        if (sc.Width > sf.Width)
                            panel_width = (int)sc.Width;
                        else
                            panel_width = (int)sf.Width;
                        tableLayoutPanel1.Width = panel_width + (Settings.Instance.SmallTargetInfo ? 40 : 10);

                        tableLayoutPanel1.ColumnStyles[0].Width = panel_width + (Settings.Instance.SmallTargetInfo ? 40 : 10);

                        tableLayoutPanel1.RowStyles[1].SizeType = SizeType.Absolute;

                        tableLayoutPanel1.RowStyles[0].SizeType = SizeType.Absolute;

                        tableLayoutPanel1.RowStyles[1].Height = (int)sf.Height + (Settings.Instance.SmallTargetInfo ? 11 : 17);

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

            catch (Exception ex) {LogLib.WriteLine("Error with MobInfo(): ", ex); return "";}

        }

        private string TimerInfo(SPAWNTIMER st)

        {
            int height_adder = 20;
            try

            {

                if (f1 == null) { return ""; }

                String descr = null;



                int countTime = 0;

                string countTimer = "";

                if (st.NextSpawnDT != DateTime.MinValue)
                {

                    TimeSpan Diff = st.NextSpawnDT.Subtract(DateTime.Now);

                    countTimer = Diff.Hours.ToString("00") + ":" + Diff.Minutes.ToString("00") + ":" + Diff.Seconds.ToString("00");

                    countTime = (Diff.Hours * 3600) + (Diff.Minutes * 60) + Diff.Seconds;

                }



                if (countTime > 0)
                {

                    StringBuilder spawnTimer = new StringBuilder();

                    spawnTimer.AppendFormat("Spawn Name: {0}\n", st.LastSpawnName);

                    string names_to_add = "Names encountered: ";
                    string[] names = st.allNames.Split(',');

                    int namecount = 0;

                    foreach (string name in names)
                    {
                        string namet = Regex.Replace(name.Replace("_", " "), "[0-9]", "").Trim();

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

                    StringBuilder spawnTimer = new StringBuilder();

                    spawnTimer.AppendFormat("Spawn Name: {0}\n", st.LastSpawnName);

                    //spawnTimer.Append("Names encountered: ");
                    string names_to_add = "Names encountered: ";
                    string[] names = st.allNames.Split(',');

                    int namecount = 0;

                    foreach (string name in names)
                    {
                        string namet = Regex.Replace(name.Replace("_", " "), "[0-9]", "").Trim();

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
                    if (names_to_add.Length > 0)
                    {
                        spawnTimer.Append(names_to_add);
                    }

                    spawnTimer.Append("\n");

                    spawnTimer.AppendFormat("Last Spawned At: {0}\n", st.SpawnTimeStr);

                    spawnTimer.AppendFormat("Last Killed At: {0}\n", st.KillTimeStr);

                    spawnTimer.AppendFormat("Next Spawn At: {0}\n", "");

                    spawnTimer.AppendFormat("Spawn Timer: {0} secs\n", st.SpawnTimer);

                    spawnTimer.AppendFormat("Spawning In: {0}\n", "");

                    spawnTimer.AppendFormat("Spawn Count: {0}\n", st.SpawnCount);

                    spawnTimer.AppendFormat("Y: {0:f3}  X: {1:f3}  Z: {2:f3}", st.Y, st.X, st.Z);



                    descr = spawnTimer.ToString();

                }

                else
                {

                    StringBuilder spawnTimer = new StringBuilder();

                    spawnTimer.AppendFormat("Spawn Name: {0}\n", st.LastSpawnName);

                    //spawnTimer.Append("Names encountered: ");
                    string names_to_add = "Names encountered: ";
                    string[] names = st.allNames.Split(',');

                    int namecount = 0;

                    foreach (string name in names)
                    {
                        string namet = Regex.Replace(name.Replace("_", " "), "[0-9]", "").Trim();

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

                    descr = spawnTimer.ToString();

                }





                //return descr;


                String timerInfo = descr;

                //String timerInfo = st.GetDescription();

                lblMobInfo.BackColor = System.Drawing.Color.White;

                Graphics g = lblMobInfo.CreateGraphics();

                SizeF sf = g.MeasureString(timerInfo, lblMobInfo.Font);

                Graphics gt = lblGameClock.CreateGraphics();

                SizeF sc = gt.MeasureString(lblGameClock.Text.ToString(), lblGameClock.Font);

                g.Dispose();

                gt.Dispose();

                sf.ToPointF();

                sc.ToPointF();

                tableLayoutPanel1.ColumnStyles[0].SizeType = SizeType.Absolute;

                if (Settings.Instance.ShowTargetInfo)

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

        private string GroundItemInfo(GroundItem gi)
        {

            try
            {



                if (f1 == null) { return ""; }

                StringBuilder grounditemInfo = new StringBuilder();

                grounditemInfo.AppendFormat("Ground Item: {0}\n", gi.Desc);

                grounditemInfo.AppendFormat("ActorDef: {0}\n", gi.Name);

                grounditemInfo.AppendFormat("Y: {0:f3}  X: {1:f3}  Z: {2:f3}", gi.Y, gi.X, gi.Z);

                lblMobInfo.BackColor = System.Drawing.Color.White;

                Graphics g = lblMobInfo.CreateGraphics();

                SizeF sf = g.MeasureString(grounditemInfo.ToString(), lblMobInfo.Font);

                Graphics gt = lblGameClock.CreateGraphics();

                SizeF sc = gt.MeasureString(lblGameClock.Text.ToString(), lblGameClock.Font);

                g.Dispose();

                gt.Dispose();

                sf.ToPointF();

                sc.ToPointF();

                tableLayoutPanel1.ColumnStyles[0].SizeType = SizeType.Absolute;

                if (Settings.Instance.ShowTargetInfo)
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


        private void MapCon_Paint(object sender, System.Windows.Forms.PaintEventArgs pe) {

        

            if (mapPane == null || f1 == null) return;



            try {

                // Check if the Window is not minimized

                if (f1.WindowState != FormWindowState.Minimized) {

                

                    DrawOptions DrawOpts=f1.DrawOpts;
                    

                    // Setup GDI Drawing        

                    Graphics sg = pe.Graphics;



                    // Clear Map

                    bkgBuffer.Graphics.Clear(Settings.Instance.BackColor);



                    string gameTime = "";

                    gameTime = String.Format("{0:MMM d, yyyy} {1:t}", eq.gametime, eq.gametime);

                    lblGameClock.Text = gameTime;

                    lblGameClock.TextAlign = ContentAlignment.MiddleCenter;



                    // Set the Spawn Size

                    if (SettingsSpawnSize != Settings.Instance.SpawnDrawSize)
                    {
                        SetSpawnSizes();
                    }

                    // Used to help reduce the number of calls to improve speed

                    float pX = eq.playerinfo.X;

                    float pY = eq.playerinfo.Y;

                    float pZ = eq.playerinfo.Z;

                    float playerx = CalcScreenCoordX(pX);

                    float playery = CalcScreenCoordY(pY);

                    float realhead = eq.CalcRealHeading(eq.playerinfo);

                    float dx = (m_panOffsetX + m_screenCenterX)/-m_ratio - m_mapCenterX;

                    float dy = (m_panOffsetY + m_screenCenterY)/-m_ratio - m_mapCenterY;

                    GraphicsState tState = bkgBuffer.Graphics.Save();



                    bkgBuffer.Graphics.ScaleTransform(-m_ratio, -m_ratio);

                    bkgBuffer.Graphics.TranslateTransform(dx, dy);

                    
                    DrawMapLines(DrawOpts);



                    bkgBuffer.Graphics.Restore(tState);

                    //bkgBuffer.Graphics.ResetTransform();

                    

                    DrawMap(DrawOpts);


                    if ((DrawOpts & DrawOptions.SpawnTrails) != DrawOptions.DrawNone)

                        DrawSpawnTrails();

                    if (eq.Zoning)
                    {
                        DrawPlayer(CalcScreenCoordX(0.0f), CalcScreenCoordY(0.0f), 0.0f, SpawnSize, SpawnSizeOffset, DrawOpts);

                    }
                    else
                    {
                        if ((DrawOpts & DrawOptions.Player) != DrawOptions.DrawNone)

                            DrawPlayer(playerx, playery, realhead, SpawnSize, SpawnSizeOffset, DrawOpts);
                    }

                    if ((DrawOpts & DrawOptions.Spawns) != DrawOptions.DrawNone)

                        DrawCorpses(pX, pY, pZ, playerx, playery, DrawOpts);

                    if ((DrawOpts & DrawOptions.GroundItems) != DrawOptions.DrawNone)

                        DrawGroundItems(pZ);

                    if ((DrawOpts & DrawOptions.SpawnTimers) != DrawOptions.DrawNone)

                        DrawSpawnTimers();

                    if (Settings.Instance.SpawnDrawSize > 1)

                        bkgBuffer.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    if ((DrawOpts & DrawOptions.Spawns) != DrawOptions.DrawNone)

                        DrawSpawns(pX, pY, pZ, playerx, playery, DrawOpts);
                    

                    // Collect mob trails, every 8th pass - approx once every 1 sec
                    if (Settings.Instance.CollectMobTrails)
                    {
                        if (collect_mobtrails_count > 8)
                        {
                            collect_mobtrails_count = 0;
                            CollectMobTrails();
                        }
                        collect_mobtrails_count++;
                    }


                    bkgBuffer.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;                   

                    // [42!] Draw a line to an arbitrary spot.

                    if ((selectedX != -1) && ((DrawOpts & DrawOptions.SpotLine)!=DrawOptions.DrawNone)) 

                    {

                        Pen myPen = new Pen(new SolidBrush(Color.White));

                        myPen.DashStyle = DashStyle.Dash;

                        myPen.DashPattern = new float[] {8, 4};

                        DrawLine(myPen, playerx,  playery, CalcScreenCoordX(selectedX), CalcScreenCoordY(selectedY));

                    }

                    f1.toolStripFPS.Text = String.Format("FPS: {0}", fpsValue);

                    bkgBuffer.Render(sg);

                }

            }

            catch (Exception ex) {LogLib.WriteLine("Error in MapCon_Paint(): ", ex);}

        }



        private void DrawCorpses(float pX, float pY, float pZ, float playerx, float playery, DrawOptions DrawOpts)

        {


            bool PCCorpseDepthFilter = Settings.Instance.DepthFilter && Settings.Instance.FilterPlayerCorpses;

            bool NPCCorpseDepthFilter = Settings.Instance.DepthFilter && Settings.Instance.FilterNPCCorpses;


            float x, y;

            float drawOffset = SpawnPlusSizeOffset - 0.5f;

            // Draw Spawns

            foreach (SPAWNINFO sp in eq.GetMobsReadonly().Values)

            {


                x = (float)Math.Round(CalcScreenCoordX(sp.X), 0);

                y = (float)Math.Round(CalcScreenCoordY(sp.Y), 0);



                // Draw Corpses

                if (sp.isCorpse && !sp.hidden)

                {

                    sp.proxAlert = false;

                    // Draw Corpses

                    if (sp.IsPlayer())

                    {

                        if (!PCCorpseDepthFilter || ((sp.Z > pZ - filterneg) && (sp.Z < pZ + filterpos)))
                        {

                            DrawRectangle(yellowPen, x - SpawnPlusSizeOffset + 0.5f, y - SpawnPlusSizeOffset + 0.5f, SpawnPlusSize, SpawnPlusSize);

                            if (Settings.Instance.ShowPlayerCorpseNames && (sp.Name.Length > 0))

                                DrawSpawnNames(textBrush, sp.Level.ToString() + ": " + sp.Name, sp.X, sp.Y);

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

                            DrawLine(cyanPen, x - drawOffset, y, x + drawOffset, y);

                            DrawLine(cyanPen, x, y - drawOffset, x, y + drawOffset);

                            if (Settings.Instance.ShowNPCCorpseNames && (sp.Name.Length > 0))

                                DrawSpawnNames(textBrush, sp.Level.ToString() + ": " + sp.Name, sp.X, sp.Y);

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

        private void DrawSpawns(float pX, float pY, float pZ, float playerx, float playery, DrawOptions DrawOpts)
        {

            uint playerSpawnID = eq.playerinfo.SpawnID;

            int RangeCircle = Settings.Instance.RangeCircle;

            bool ShowTargetInfo = Settings.Instance.ShowTargetInfo;

            bool NPCDepthFilter = Settings.Instance.DepthFilter && Settings.Instance.FilterNPCs;

            bool PCDepthFilter = Settings.Instance.DepthFilter && Settings.Instance.FilterPlayers;

            bool PCCorpseDepthFilter = Settings.Instance.DepthFilter && Settings.Instance.FilterPlayerCorpses;

            bool NPCCorpseDepthFilter = Settings.Instance.DepthFilter && Settings.Instance.FilterNPCCorpses;

            float x, y;

            bool ShowRings = (DrawOpts & DrawOptions.SpawnRings) != DrawOptions.DrawNone;

            bool DrawDirection = (DrawOpts & DrawOptions.DirectionLines) != DrawOptions.DrawNone;

            if ((eq.selectedID == 99999) && (eq.SpawnX == -1))
            {

                lblMobInfo.Text = MobInfo(null, true, true);

            }

            // Draw Spawns

            foreach (SPAWNINFO sp in eq.GetMobsReadonly().Values)
            {

                x = (float)Math.Round(CalcScreenCoordX(sp.X), 0);

                y = (float)Math.Round(CalcScreenCoordY(sp.Y), 0);

                // Draw Line from Player to the Selected Spawn

                if (eq.selectedID == sp.SpawnID)
                {

                    DrawEllipse(pinkPen, x - SelectSizeOffset, y - SelectSizeOffset, SelectSize, SelectSize);

                    // Update the Spawn Information Window if not based on selected timer

                    if (eq.SpawnX == -1)
                    {

                        lblMobInfo.Text = MobInfo(sp, true, true);

                        DrawLine(pinkPen, playerx, playery, x, y);

                    }

                    sp.proxAlert = false;

                }

                else if (Settings.Instance.ColorRangeCircle && RangeCircle > 0)
                {

                    // do checks for proximity alert

                    if ((sp.alertMob) && (sp.Type != 2))
                    {

                        // if alertmob - use to identify mobs that are ok to do alerts

                        int minlevel = Settings.Instance.MinAlertLevel;

                        if (minlevel == -1)

                            minlevel = (int)eq.playerinfo.Level + eq.greyRange;

                        if (sp.Level >= minlevel)
                        {

                            float rRange = Settings.Instance.RangeCircle;

                            float rsRange = rRange * 1.1f;

                            float maxZ = pZ + rRange;

                            float minZ = pZ - rRange;

                            if (NPCDepthFilter)
                            {

                                maxZ = pZ + filterpos;

                                minZ = pZ - filterpos;

                            }

                            if (!sp.proxAlert)
                            {

                                if ((sp.Z > minZ) && (sp.Z < maxZ))
                                {

                                    float sd = (pX - sp.X) * (pX - sp.X) + (pY - sp.Y) * (pY - sp.Y);

                                    if (sd < (rRange * rRange))
                                    {

                                        sp.proxAlert = true;

                                        eq.PlayAlertSound();

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

                                    float sd = (pX - sp.X) * (pX - sp.X) + (pY - sp.Y) * (pY - sp.Y);

                                    if (sd > (1.4 * rRange * rRange))
                                    {

                                        sp.proxAlert = false;

                                    }

                                }

                            }

                        }

                        else

                            sp.proxAlert = false;

                    }

                    else

                        sp.proxAlert = false;

                }

                else

                    sp.proxAlert = false;



                // Draw All Other Spawns

                if (sp.SpawnID != playerSpawnID && sp.flags == 0 && sp.Name.Length > 0)
                {

                    // Draw Spawn if not Hidden

                    if (!sp.hidden)
                    {

                        // Draw Spawn if the Spawn is within the Players Depth Filter

                        // Highlite Current Target

                        if (curTarget == sp.Name)

                            FillRectangle(whiteBrush, x - SpawnPlusSizeOffset - 1, y - SpawnPlusSizeOffset-1, SpawnPlusSize+2, SpawnPlusSize+2);



                        // Draw Invisible Mob - these are EQ trigger events



                        if (sp.isEventController)
                        {

                            if (!NPCDepthFilter || ((sp.Z > pZ - filterneg) && (sp.Z < pZ + filterpos)))
                            {

                                FillEllipse(purpleBrush, x - SpawnSizeOffset, y - SpawnSizeOffset, SpawnSize, SpawnSize);
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

                                FillEllipse(grayBrush, x - SpawnSizeOffset, y - SpawnSizeOffset, SpawnSize, SpawnSize);
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
                                sp.filtered = false;
                                if (eq.conColors[sp.Level] != null)
                                {

                                    if ((DrawOpts & DrawOptions.DirectionLines) != DrawOptions.DrawNone)

                                        DrawDirectionLines(sp, x, y);



                                    // Draw Other Players 

                                    if ((Settings.Instance.ShowPVP) && ((Math.Abs(eq.playerinfo.Level - sp.Level) <= Settings.Instance.PVPLevels) || (Settings.Instance.PVPLevels == -1)))
                                    {

                                        FillTriangle(eq.conColors[sp.Level], x, y, SelectSizeOffset);

                                        DrawTriangle(PCBorder, x, y, SelectSizeOffset);

                                        if (Settings.Instance.ShowPCNames && (sp.Name.Length > 0))
                                        {

                                            DrawSpawnNames(textBrush, sp.Level.ToString() + ": " + sp.Name, sp.X, sp.Y);

                                        }

                                        else
                                        {

                                            if (Settings.Instance.ShowPVPLevel)

                                                DrawSpawnNames(textBrush, sp.Level.ToString(), sp.X, sp.Y);

                                        }

                                        if (flash)
                                        {

                                            cPen = new Pen(eq.GetDistinctColor(Color.White));

                                            DrawEllipse(cPen, x - SelectSizeOffset, y - SelectSizeOffset, SelectSize, SelectSize);

                                        }

                                    }

                                    else
                                    {

                                        FillRectangle(eq.conColors[sp.Level], x - SpawnPlusSizeOffset + 0.5f, y - SpawnPlusSizeOffset + 0.5f, SpawnPlusSize, SpawnPlusSize);

                                        DrawRectangle(PCBorder, x - SpawnPlusSizeOffset + 0.5f, y - SpawnPlusSizeOffset + 0.5f, SpawnPlusSize, SpawnPlusSize);



                                        // draw purple border around players

                                        if (sp.Hide != 0)
                                        {

                                            if (sp.Hide == 2)
                                            {

                                                // SoS Players

                                                if (flash)

                                                    DrawRectangle(purplePen, x - SpawnPlusSizeOffset - 0.5f, y - SpawnPlusSizeOffset - 0.5f, SpawnPlusSize + 2.0f, SpawnPlusSize + 2.0f);

                                            }

                                            else
                                            {

                                                // Player is invis

                                                DrawRectangle(purplePen, x - SpawnPlusSizeOffset - 0.5f, y - SpawnPlusSizeOffset - 0.5f, SpawnPlusSize + 2.0f, SpawnPlusSize + 2.0f);

                                            }

                                        }

                                        if (Settings.Instance.ShowPCNames && (sp.Name.Length > 0))

                                            DrawSpawnNames(textBrush, sp.Level.ToString() + ": " + sp.Name, sp.X, sp.Y);

                                    }

                                }

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
                                sp.filtered = false;

                                if (Settings.Instance.ShowNPCNames && (sp.Name.Length > 0))

                                    DrawSpawnNames(textBrush, sp.Level.ToString() + ": " + sp.Name, sp.X, sp.Y);

                                else if ((Settings.Instance.ShowNPCLevels) && (sp.Name.Length > 0))

                                    DrawSpawnNames(textBrush, sp.Level.ToString(), sp.X, sp.Y);



                                if (DrawDirection)

                                    DrawDirectionLines(sp, x, y);



                                // Draw NPCs
                                if ((sp.isPet == true && !Settings.Instance.ShowPVP) || sp.isFamiliar || sp.isMount)
                                {
                                    FillEllipse(grayBrush, x - SpawnSizeOffset, y - SpawnSizeOffset, SpawnSize, SpawnSize);
                                }
                                else if (eq.conColors[sp.Level] != null)
                                {
                                    FillEllipse(eq.conColors[sp.Level], x - SpawnSizeOffset, y - SpawnSizeOffset, SpawnSize, SpawnSize);
                                }


                                // Draw PC color border around Mercenary

                                if (sp.isMerc)

                                    DrawEllipse(PCBorder, x - SpawnSizeOffset, y - SpawnSizeOffset, SpawnSize, SpawnSize);



                                // Draw Purple border around invis mobs

                                if (sp.Hide != 0)
                                {

                                    // Flashing purple ring around SoS mobs

                                    if (sp.Hide == 2)
                                    {

                                        if (flash)

                                            DrawEllipse(purplePen, x - SelectSizeOffset, y - SelectSizeOffset, SelectSize, SelectSize);

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

                        // Draw Ring around Lookup'ed Mobs

                        if (sp.isLookup && (!sp.isCorpse || Settings.Instance.CorpseAlerts))
                        {
                            if (!lookup_set)
                            {
                                SetLookupValues();
                            }

                            DrawEllipse(whitePen, x - LookupRingOffset, y - LookupRingOffset, LookupRingSize, LookupRingSize);

                        }
                        // Draw flashes

                        if (flash)
                        {
                            // Draw Ring around Hunted Mobs

                            if ((sp.isHunt || sp.proxAlert) && (!NPCDepthFilter || ((sp.Z > pZ - filterneg) && (sp.Z < pZ + filterpos))))

                                DrawEllipse(green2Pen, x - SpawnPlusSizeOffset, y - SpawnPlusSizeOffset, SpawnPlusSize, SpawnPlusSize);

                            // Draw Ring around Caution Mobs

                            if (sp.isCaution && (!NPCDepthFilter || ((sp.Z > pZ - filterneg) && (sp.Z < pZ + filterpos))))

                                DrawEllipse(yellow2Pen, x - SpawnPlusSizeOffset, y - SpawnPlusSizeOffset, SpawnPlusSize, SpawnPlusSize);

                            // Draw Ring around Danger Mobs

                            if (sp.isDanger && (!NPCDepthFilter || ((sp.Z > pZ - filterneg) && (sp.Z < pZ + filterpos))))

                                DrawEllipse(red2Pen, x - SpawnPlusSizeOffset, y - SpawnPlusSizeOffset, SpawnPlusSize, SpawnPlusSize);

                            // Draw Ring around Rare Mobs

                            if (sp.isAlert && (!NPCDepthFilter || ((sp.Z > pZ - filterneg) && (sp.Z < pZ + filterpos))))

                                DrawEllipse(white2Pen, x - SpawnPlusSizeOffset, y - SpawnPlusSizeOffset, SpawnPlusSize, SpawnPlusSize);

                        }

                        if (ShowRings && (!NPCDepthFilter || ((sp.Z > pZ - filterneg) && (sp.Z < pZ + filterpos))))
                        {

                            // Draw Ring around Bankers

                            if (sp.Class == 40)
                            {

                                DrawEllipse(whitePen, x - SpawnSizeOffset, y - SpawnSizeOffset, SpawnSize, SpawnSize);

                                DrawEllipse(greenPen, x - SpawnPlusSizeOffset, y - SpawnPlusSizeOffset, SpawnPlusSize, SpawnPlusSize);

                            }



                            // Draw Ring around Guild Master

                            if (sp.Class > 19 && sp.Class < 35)
                            {

                                DrawEllipse(whitePen, x - SpawnSizeOffset, y - SpawnSizeOffset, SpawnSize, SpawnSize);

                                DrawEllipse(redPen, x - SpawnPlusSizeOffset, y - SpawnPlusSizeOffset, SpawnPlusSize, SpawnPlusSize);

                            }



                            // Draw Ring around Shopkeepers

                            if (sp.Class == 41)
                            {

                                DrawEllipse(whitePen, x - SpawnSizeOffset, y - SpawnSizeOffset, SpawnSize, SpawnSize);

                                DrawEllipse(bluePen, x - SpawnPlusSizeOffset, y - SpawnPlusSizeOffset, SpawnPlusSize, SpawnPlusSize);

                            }

                        }

                    }

                }

            }

            lookup_set = false;

        }

        #region DrawMap

        public void SetDistinctPens()

        {

            // setup the distinct pens

            // the goal is to create as few pens as possible at runtime for performance reasons

            // Always set up distinct colors.  But will only use map colors if not going with the few.
            darkPen = eq.GetDistinctColor(new Pen(Color.Black));
            lightPen = eq.GetDistinctColor(new Pen(Color.FromArgb(32, darkPen.Color)));

            drawPen = darkPen;

            darkBrush = eq.GetDistinctColor(distinctBrush);

        }

        public void DrawMapLines(DrawOptions DrawOpts)

        {

            LogLib.WriteLine("Entering DrawMapLines", LogLevel.Trace);

            try

            {

                // Draw Zone Map

                if (eq.longname != "" && ((DrawOpts & DrawOptions.DrawMap) != DrawOptions.DrawNone))

                {
                    if (!Settings.Instance.DepthFilter || (Settings.Instance.DepthFilter && !Settings.Instance.FilterMapLines))
                    // No depth filtering
                    {
                            foreach (MapLine l in eq.GetLinesReadonly())
                                DrawLines(l.draw_color, l.linePoints);
                    } 
                    
                    else

                    {

                        float minZ = eq.playerinfo.Z - filterneg;

                        float maxZ = eq.playerinfo.Z + filterpos;

                        foreach (MapLine l in eq.GetLinesReadonly())
                        {
                            // All the points in this set of lines are good
                            if (l.maxZ < maxZ && l.minZ > minZ)
                            {
                                DrawLines(l.draw_color, l.linePoints);
                            }
                            else if (l.maxZ < minZ || l.minZ > maxZ)
                            {
                                DrawLines(l.fade_color, l.linePoints);
                            }
                            else
                            {

                                bool curValid, lastValid;

                                float curX, curY, curZ, lastX, lastY, lastZ;


                                lastX = l.Point(0).x;

                                lastY = l.Point(0).y;

                                lastZ = l.Point(0).z;

                                lastValid = (lastZ > minZ) && (lastZ < maxZ);

                                for (int d = 1; d < l.aPoints.Count; d++)
                                {

                                    curX = l.Point(d).x;

                                    curY = l.Point(d).y;

                                    curZ = l.Point(d).z;

                                    curValid = (curZ > minZ) && (curZ < maxZ);

                                    // Original Depth Filter method (use z-axis values only)

                                    // instead of not drawing filtered lines, we draw light ones

                                    if (!curValid && !lastValid)
                                    {
                                        if (Settings.Instance.UseDynamicAlpha)
                                        {
                                            DrawLine(l.fade_color, lastX, lastY, curX, curY);
                                        }
                                    }

                                    else
                                    {

                                        DrawLine(l.draw_color, lastX, lastY, curX, curY);

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

            LogLib.WriteLine("Exiting DrawMapLines", LogLevel.Trace);

        }

        public void DrawMap(DrawOptions DrawOpts) 

        {

            LogLib.WriteLine("Entering DrawMap",LogLevel.Trace);



            try {

                //bkgBuffer.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Draw gridline...

                if ((DrawOpts & DrawOptions.GridLines) != DrawOptions.DrawNone) 

                {

                    int gx, gy, label;

                    float sx,sy;

                    int gridInterval = Settings.Instance.GridInterval;

                    Color curGridColor = Settings.Instance.GridColor;

                    Color curGridLabelColor = Settings.Instance.GridLabelColor;

                    if (curGridLabelColor != gridLabelColor)

                    {

                        gridLabelColor = curGridLabelColor;

                        gridBrush = new SolidBrush(gridLabelColor);

                    }



                    if (curGridColor != gridColor)

                    {

                        gridColor = curGridColor;

                        gPen = new Pen(gridColor);

                    }



                    // Draw Horizontal Grid Lines

                    for (gx = ((int)(eq.minx / (float)gridInterval)) - 1; (float)gx < (eq.maxx / gridInterval) + 1; gx++)

                    {

                        label = (gx * gridInterval);

                        sx = (float)Math.Round(CalcScreenCoordX((float)label), 0);

                        DrawLine(gPen, sx, 0, sx, Height);

                        bkgBuffer.Graphics.DrawString(label.ToString(), drawFont, gridBrush, sx, Height - (drawFont.GetHeight() + 5));

                    }



                    // Draw Vertical Grid Lines

                    for (gy = ((int)(eq.miny / gridInterval)) - 1; gy < (int)(eq.maxy / gridInterval) + 1; gy++)

                    {

                        label = (gy * gridInterval);

                        sy = (float)Math.Round(CalcScreenCoordY((float)label), 0);

                        DrawLine(gPen, 0, sy, Width, sy);

                        bkgBuffer.Graphics.DrawString(label.ToString(), drawFont, gridBrush, Width - (bkgBuffer.Graphics.MeasureString(label.ToString(), drawFont).Width + 5), sy);

                    }

                }             

                if ((DrawOpts & DrawOptions.ZoneText)!=DrawOptions.DrawNone)

                {

                    // Draw Zone Text
                    if (Settings.Instance.DepthFilter && Settings.Instance.FilterMapText)
                    {
                        // Depth Filter
                        float minZ = eq.playerinfo.Z - filterneg;
                        float maxZ = eq.playerinfo.Z + filterpos;

                        foreach (MapText t in eq.GetTextsReadonly())
                        {
                            if (t.z != -99999 && t.z > minZ && t.z < maxZ)
                            {
                                int x_cord = (int)CalcScreenCoordX(t.x);
                                int y_cord = (int)CalcScreenCoordY(t.y);
                                if (t.size == 2)
                                    bkgBuffer.Graphics.DrawString(t.text, drawFont, t.draw_color, x_cord, y_cord - t.offset);
                                else if (t.size == 1)
                                    bkgBuffer.Graphics.DrawString(t.text, drawFont1, t.draw_color, x_cord, y_cord - t.offset);
                                else
                                    bkgBuffer.Graphics.DrawString(t.text, drawFont3, t.draw_color, x_cord, y_cord - t.offset);
                                DrawIntLine(t.draw_pen, x_cord - 1, y_cord, x_cord + 1, y_cord);
                                DrawIntLine(t.draw_pen, x_cord, y_cord - 1, x_cord, y_cord + 1);
                            }
                        }

                    } else {

                        // No Depth Filtering
                        foreach (MapText t in eq.GetTextsReadonly())
                        {
                            int x_cord = (int)CalcScreenCoordX(t.x);
                            int y_cord = (int)CalcScreenCoordY(t.y);
                            if (t.size == 2)
                                bkgBuffer.Graphics.DrawString(t.text, drawFont, t.draw_color, x_cord, y_cord - t.offset);
                            else if (t.size == 1)
                                bkgBuffer.Graphics.DrawString(t.text, drawFont1, t.draw_color, x_cord, y_cord - t.offset);
                            else
                                bkgBuffer.Graphics.DrawString(t.text, drawFont3, t.draw_color, x_cord, y_cord - t.offset);
                            DrawIntLine(t.draw_pen, x_cord - 1, y_cord, x_cord + 1, y_cord);
                            DrawIntLine(t.draw_pen, x_cord, y_cord - 1, x_cord, y_cord + 1);
                        }

                    }

                }

            }

            catch (Exception ex) {LogLib.WriteLine("Error in DrawMap(): ", ex);}

            

            LogLib.WriteLine("Exiting DrawMap",LogLevel.Trace);            

        }

        #endregion



        #region DrawPlayer

        public void DrawPlayer(float playerx, float playery, float realhead, float SpawnSize, float SpawnSizeOffset, DrawOptions DrawOpts)

        {

            try 

            {

                float x,y,x1,y1;

                int xHead = (int)eq.playerinfo.Heading;

                // Draw Range Circle

                if (Settings.Instance.RangeCircle > 0) 

                {

                    float rCircleRadius = Settings.Instance.RangeCircle * m_ratio;  

                    if (Settings.Instance.ColorRangeCircle) 

                    {

                        HatchStyle hs = (HatchStyle)Enum.Parse(typeof(HatchStyle), Settings.Instance.HatchIndex, true);

                        HatchBrush hatchBrush = new HatchBrush(hs, Settings.Instance.RangeCircleColor,Color.Transparent);

                        FillEllipse(hatchBrush, playerx-rCircleRadius, playery-rCircleRadius, (rCircleRadius*2), (rCircleRadius*2));

                    }



                    // Draw Red V in the Range Circle

                    if (Settings.Instance.DrawFoV && xHead >=0 && xHead < 512) 

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

                        DrawLine(redPen, playerx, playery, playerx+x, playery+y);

                        

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

                        //y = -(float)(Math.Cos((realhead - 45) * 0.017453f) * rCircleRadius * 1.05);

                        //x = -(float)(Math.Sin((realhead - 45) * 0.017453f) * rCircleRadius * 1.05);

                        DrawLine(redPen, playerx, playery, playerx+x, playery+y);

                        

                        // Draw Heading Line

                        y = -(float)(xCos[xHead] * rCircleRadius);

                        x = -(float)(xSin[xHead] * rCircleRadius);

                        DrawLine(yellowPen, playerx, playery, playerx + x, playery + y);

                    }

                    if (m_rangechange)

                    {

                        if (flash)

                            DrawEllipse(eq.GetDistinctColor(new Pen(Settings.Instance.RangeCircleColor)), playerx - rCircleRadius, playery - rCircleRadius, (rCircleRadius * 2), (rCircleRadius * 2));

                    }

                    else

                        DrawEllipse(eq.GetDistinctColor(new Pen(Settings.Instance.RangeCircleColor)), playerx - rCircleRadius, playery - rCircleRadius, (rCircleRadius * 2), (rCircleRadius * 2));

                }



                // Draw Player  (only if we actually have a player)

                if (eq.playerinfo.SpawnID != 0) 

                {

                    // Draw Player Heading Line

                    if ((DrawOpts & DrawOptions.DirectionLines) != DrawOptions.DrawNone)

                    {

                        if (xHead >= 0 && xHead < 512)

                        {

                            y1 = -(float)(xCos[xHead] * ((eq.playerinfo.SpeedRun * m_ratio) * 100));

                            x1 = -(float)(xSin[xHead] * ((eq.playerinfo.SpeedRun * m_ratio) * 100));

                            DrawLine(whitePen, playerx, playery, playerx + x1, playery + y1);

                        }

                    }

                    

                    FillRectangle(whiteBrush, playerx-SpawnSizeOffset, playery-SpawnSizeOffset, SpawnSize, SpawnSize);  

                    DrawRectangle(PCBorder, playerx-SpawnSizeOffset - 0.5f, playery-SpawnSizeOffset - 0.5f, SpawnSize + 1.0f, SpawnSize + 1.0f);

                }

            }

            catch (Exception ex) {LogLib.WriteLine("Error in DrawPlayer(): ", ex);}

        }

        #endregion



        #region DrawSpawnTimers

        public void DrawSpawnTimers() 

        {

            try 

            {

                // Draw Spawn Timers

                Pen pen;

                DateTime now=DateTime.Now;



                float minZ = eq.playerinfo.Z - filterneg;

                float maxZ = eq.playerinfo.Z + filterpos;



                pen = ltgrayPen;



                foreach (SPAWNTIMER st in eq.mobsTimers.GetRespawned().Values) 

                {
                    if (st.zone != eq.shortname)
                        continue;

                    float stX = (float)Math.Round(CalcScreenCoordX(st.X),0);

                    float stY = (float)Math.Round(CalcScreenCoordY(st.Y),0);



                    float stOffset = SpawnPlusSizeOffset - 0.5f;

                    int checkTimer = st.SecondsUntilSpawn(now);



                    pen = ltgrayPen;



                    bool canDraw = false;



                    if (checkTimer == 0)

                        canDraw = true;

                    if (checkTimer > 0)

                    {

                        canDraw = true;

                        // Set Pen Colors

                        if ((checkTimer < 30))

                        {

                            if (flash)

                                pen = redPen;

                        }

                        else if (checkTimer < 60)

                        {

                            pen = redPen;

                        }

                        else if (checkTimer < 90)

                        {

                            pen = orangePen;

                        }

                        else if (checkTimer < 120)

                        {

                            pen = yellowPen;

                        }

                    }

                        

                        // if depth filter on make adjustments to spawn points

                    if (Settings.Instance.DepthFilter && Settings.Instance.FilterSpawnPoints)
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

                        if ((Settings.Instance.SpawnCountdown) && (checkTimer > 0) && (checkTimer < 120))

                            DrawSpawnNames(textBrush, checkTimer.ToString(), st.X, st.Y);

                    }



                    // Draw Blue Line to selected spawn location

                    if ((st.X == eq.SpawnX) && (st.Y == eq.SpawnY))

                    {

                        float playerx = CalcScreenCoordX(eq.playerinfo.X);

                        float playery = CalcScreenCoordY(eq.playerinfo.Y);

                        pen = bluePen;

                        DrawLine(pen, playerx, playery, stX, stY);

                        // Update the Spawn Information Window

                        lblMobInfo.Text = TimerInfo(st);

                    }

                }

            }

            catch (Exception ex) {LogLib.WriteLine("Error in DrawSpawnTimers(): ", ex);}

        }

        #endregion



        #region DrawGroundItems

        public void DrawGroundItems(float pZ) 

        {
            bool GroundItemDepthFilter = Settings.Instance.DepthFilter && Settings.Instance.FilterGroundItems;
            
            try 

            {

                float x,y;

                // Draw Ground Spawns

                foreach (GroundItem gi in eq.GetItemsReadonly()) 

                {
                    x = (float)Math.Round(CalcScreenCoordX(gi.X), 0);

                    y = (float)Math.Round(CalcScreenCoordY(gi.Y), 0);

                    

                    if (!GroundItemDepthFilter || ((gi.Z > pZ - filterneg) && (gi.Z < pZ + filterpos)))
                    {
                        gi.filtered = false;

                        DrawLine(yellowPen, x - SpawnPlusSizeOffset, y - SpawnPlusSizeOffset, x + SpawnPlusSizeOffset, y + SpawnPlusSizeOffset);

                        DrawLine(yellowPen, x - SpawnPlusSizeOffset, y + SpawnPlusSizeOffset, x + SpawnPlusSizeOffset, y - SpawnPlusSizeOffset);
                    }
                    else
                    {
                        gi.filtered = true;
                    }

                    // Draw Yellow Line to selected ground item location

                    if (eq.SpawnX == gi.X && eq.SpawnY == gi.Y && eq.selectedID == 99999)
                    {

                        float playerx = CalcScreenCoordX(eq.playerinfo.X);

                        float playery = CalcScreenCoordY(eq.playerinfo.Y);

                        DrawLine(yellowPen, playerx, playery, x, y);

                        DrawEllipse(pinkPen, x - SelectSizeOffset, y - SelectSizeOffset, SelectSize, SelectSize);

                        // Update the Spawn Information Window

                        lblMobInfo.Text = GroundItemInfo(gi);

                    }

                    if (flash)
                    {

                        // Draw Yellow Ring around Caution Ground Items

                        if (gi.isCaution && (!GroundItemDepthFilter || ((gi.Z > pZ - filterneg) && (gi.Z < pZ + filterpos))))

                            DrawEllipse(yellow2Pen, x - SpawnPlusSizeOffset - 1, y - SpawnPlusSizeOffset - 1, SpawnPlusSize + 2, SpawnPlusSize + 2);

                        // Draw Red Ring around Danger Ground Items

                        if (gi.isDanger && (!GroundItemDepthFilter || ((gi.Z > pZ - filterneg) && (gi.Z < pZ + filterpos))))

                            DrawEllipse(red2Pen, x - SpawnPlusSizeOffset - 1, y - SpawnPlusSizeOffset - 1, SpawnPlusSize + 2, SpawnPlusSize + 2);

                        // Draw White Ring around Rare Ground Items

                        if (gi.isAlert && (!GroundItemDepthFilter || ((gi.Z > pZ - filterneg) && (gi.Z < pZ + filterpos))))

                            DrawEllipse(white2Pen, x - SpawnPlusSizeOffset - 1, y - SpawnPlusSizeOffset - 1, SpawnPlusSize + 2, SpawnPlusSize + 2);

                        // Draw Cyan Ring around Hunt Ground Items 

                        if (gi.isHunt && (!GroundItemDepthFilter || ((gi.Z > pZ - filterneg) && (gi.Z < pZ + filterpos))))

                            DrawEllipse(green2Pen, x - SpawnPlusSizeOffset - 1, y - SpawnPlusSizeOffset - 1, SpawnPlusSize + 2, SpawnPlusSize + 2);
                    }

                }

            }

            catch (Exception ex) {LogLib.WriteLine("Error in DrawGroundItems(): ", ex);}

        }

        #endregion



        #region DrawSpawnTrails

        public void DrawSpawnTrails() {

            try 

            {

                // Draw Mob Trails

                foreach (MobTrailPoint mtp in eq.GetMobTrailsReadonly()) 

                    FillEllipse(whiteBrush, CalcScreenCoordX(mtp.x)-2, CalcScreenCoordY(mtp.y)-2, 2,2);

            }

            catch (Exception ex) {LogLib.WriteLine("Error in DrawSpawnTrails(): ", ex);}

        }

        #endregion



        #region DrawDirectionLines

        public void DrawDirectionLines(SPAWNINFO sp, float x, float y) {

            try 

            {

                float x1, y1;



                // Draw NPCs Direction Lines if heading > 0

                if (sp.Heading >= 0 && sp.Heading < 512)

                {

                    y1 = -(float)(xCos[(int)sp.Heading] * ((sp.SpeedRun * m_ratio) * 150));

                    x1 = -(float)(xSin[(int)sp.Heading] * ((sp.SpeedRun * m_ratio) * 150));

                    DrawLine(whitePen, x, y, x + x1, y + y1);

                }

            }

            catch (Exception ex) {LogLib.WriteLine("Error in DrawDirectionLines(): ", ex);}

        }

        #endregion


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

        public float GetXAadjust()
        {
            return x_adjust;
        }

        public float GetYAadjust()
        {
            return y_adjust;
        }

        private void MapChanged(EQMap map)

        {

            DrawOptions DrawOpts = f1.DrawOpts;

            // if the autoexpand is not checked, scale is not at 100, then maintain the map scale
            if (eq.longname.Length > 0 && mapPane != null && Settings.Instance.AutoExpand == false &&
                mapPane.scale.Value != 100)
            {
                float r_ratio = 1.0f;
                float mapWidth = Math.Abs(eq.maxx - eq.minx);
                float mapHeight = Math.Abs(eq.maxy - eq.miny);
                float border = 15;
                float ScreenWidth = Width - (2.0f * border);
                float ScreenHeight = Height - (2.0f * border);
                float xratio = (float)ScreenWidth / mapWidth;
                float yratio = (float)ScreenHeight / mapHeight;
                if (xratio < yratio)
                {
                    r_ratio = xratio;
                }
                else
                {
                    r_ratio = yratio;
                }

                if (r_ratio > 0.0f)
                {
                    scale = (float)m_ratio / r_ratio;
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

            else if (Settings.Instance.KeepCentered)

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

            if ((DrawOpts & DrawOptions.ZoneText) != DrawOptions.DrawNone)

            {

                float factor = 1;

                float xlabel = 0;

                float ylabel = 0;

                if (m_ratio > 0)

                    factor = 1 / m_ratio;

                if ((DrawOpts & DrawOptions.GridLines) != DrawOptions.DrawNone)

                {

                    // drawing gridlines, so account for grid labels

                    ylabel = drawFont.GetHeight() + 0;

                    xlabel = bkgBuffer.Graphics.MeasureString("10000", drawFont).Width;



                }

                foreach (MapText t in eq.GetTextsReadonly())

                {

                    SizeF tf = bkgBuffer.Graphics.MeasureString(t.text.ToString(), drawFont);
                    if (t.size == 1)
                        bkgBuffer.Graphics.MeasureString(t.text.ToString(), drawFont1);
                    else if (t.size == 3)
                        bkgBuffer.Graphics.MeasureString(t.text.ToString(), drawFont3);



                    if ((t.x - ((tf.Width + xlabel) * factor)) < eq.minx)

                        eq.minx = t.x - ((tf.Width + xlabel) / m_ratio);

                    else if (t.x > eq.maxx)

                        eq.maxx = t.x;



                    if ((t.y + (t.offset * factor)) > eq.maxy)

                        eq.maxy = t.y + (t.offset * factor);

                    else if ((t.y - ((tf.Height + ylabel) * factor)) < eq.miny)

                        eq.miny = t.y - ((tf.Height + ylabel) * factor);

                }

                ReAdjust();

            }





            Invalidate();        

        }


        private float Calculate3DDistanceSquared(MapPoint mp)

        {

            float deltaX, deltaY, deltaZ;



            deltaX = (float) eq.playerinfo.X - mp.x;

            if (deltaX > 300) return 90000.0f;

            deltaY = (float) eq.playerinfo.Y - mp.y;

            if (deltaY > 300) return 90000.0f;

            deltaZ = (float) eq.playerinfo.Z - mp.z;

            return (float)(deltaX * deltaX) + (deltaY * deltaY) + (deltaZ * deltaZ);

        }

        public void Tick()
        {
            // This is set up for flashes and animated skittles
            skittle = skittle + 1;
            flash_count = flash_count + 1;

            if (skittle > update_steps)
                skittle = 0;

            
            if (flash_count >= update_ticks)
            {
                flash_count = 0;
                flash = !flash;
            }
        }

        private void SetSpawnSizes()
        {
            SettingsSpawnSize = Settings.Instance.SpawnDrawSize;

            SpawnSize = ((float)SettingsSpawnSize * 2.0f) - 1.0f;

            SpawnSizeOffset = (SpawnSize / 2.0f);

            SpawnPlusSize = SpawnSize + 2.0f;

            SpawnPlusSizeOffset = SpawnPlusSize / 2.0f;

            SelectSize = SpawnSize + 4.0f;

            SelectSizeOffset = SelectSize / 2.0f;

        }

        public void UpdatePCBorder()
        {
            Color curPCBorder = Settings.Instance.PCBorderColor;

            if (curPCBorder != PCBorder.Color)
            {

                PCBorder = new Pen(new SolidBrush(Settings.Instance.PCBorderColor));

            }
        }

        private void SetLookupValues()
        {
            LookupRingSize = SpawnPlusSize + ((float) skittle / (float) update_steps * SelectSize);
            LookupRingOffset = LookupRingSize / 2.0f;
            lookup_set = true;
        }

        private void MapCon_MouseDoubleClick(object sender, MouseEventArgs e)

        {
            // If a spawn point is found at location, select the spawn point.
            // else if mob is found at current location, select that mobs spawn
            // point if it exists, otherwise select the mob.
            if (Settings.Instance.RangeCircle > 0)
            {

                float RangeCircleRadius = Settings.Instance.RangeCircle;

                // Calc the proper loc for the mouse

                float mousex = ScreenToMapCoordX((float)e.X);

                float mousey = ScreenToMapCoordY((float)e.Y);

                // Range

                float sd = (float)Math.Sqrt(((mousey - eq.playerinfo.Y) * (mousey - eq.playerinfo.Y)) +

                    ((mousex - eq.playerinfo.X) * (mousex - eq.playerinfo.X)));

                if (sd < (RangeCircleRadius))
                {

                    // if double click in range circle, turn it on/off

                    Settings.Instance.ColorRangeCircle = !Settings.Instance.ColorRangeCircle;

                }
            }

        }

        private void CollectMobTrails()
        {

            // Collect Mob Trails

            foreach (SPAWNINFO sp in eq.GetMobsReadonly().Values)
            {

                if (sp.Type == 1)
                {
                    // Setup NPCs Trails


                    //add point to mobtrails arraylist if not already there

                    MobTrailPoint work = new MobTrailPoint();

                    work.x = (int)(sp.X);

                    work.y = (int)(sp.Y);

                    eq.AddMobTrailPoint(work);

                }
            }
        }

    }



    #region Map Structures

    public class MapPoint 

    {

        public int x = 0;

        public int y = 0;

        public int z = 0;

    }       



    public class MobTrailPoint {

        public int x = 0;

        public int y = 0;

    }



    public class MapLine 

    {

        public MapLine() {aPoints = new ArrayList();}

        public int maxZ;

        public int minZ;

        public string name = "";

        public Pen color = null;

        public Pen draw_color = null;

        public Pen fade_color = null;

        public ArrayList aPoints;

        public PointF[] linePoints;

        public MapPoint Point(int index)

        {

            return (MapPoint) aPoints[index];

        }



    }





    public class MapText {

        public string text = "";

        public int offset = 0;

        public SolidBrush color = null;

        public SolidBrush draw_color = null;

        public Pen draw_pen = null;

        public int x = 0;

        public int y = 0;

        public int z = 0;

        public int size = 2;

    }

    #endregion

}



