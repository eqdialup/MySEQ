using System;

using System.Data;

using System.Drawing;

using System.Collections;

using System.Windows.Forms;

using WeifenLuo.WinFormsUI.Docking;

using System.ComponentModel;

using System.Text.RegularExpressions;



using Structures;



namespace myseq

{

    public class ListViewPanel : DockContent
    {

        public bool curDescend = false;

        private myseq.MapPane mapPane;

        private EQData eq;

        private MapCon mapCon;

        private Filters filters;

        private frmMain f1;



        private System.Windows.Forms.TextBox txtSpawnList;

        private System.Windows.Forms.Button cmdReset;

        public System.Windows.Forms.ListView listView;

        private System.Windows.Forms.ContextMenuStrip mnuContext;
        private IContainer components;

        private ToolStripMenuItem mnuAddGlobalFilter;

        private ToolStripMenuItem mnuAddHuntFilter;

        private ToolStripMenuItem mnuAddCautionFilter;

        private ToolStripMenuItem mnuAddAlertFilter;

        private ToolStripMenuItem mnuAddZoneFilter;

        private ToolStripMenuItem mnuAddZoneHuntFilter;

        private ToolStripMenuItem mnuAddZoneCautionFilter;

        private ToolStripMenuItem mnuAddZoneDangerFilter;

        private ToolStripMenuItem mnuAddZoneRareFilter;

        private ToolStripMenuItem addZoneEmailAlertFilter;

        private ToolStripMenuItem mnuEditGlobalFilters;

        private ToolStripMenuItem mnuEditZoneFilters;

        public string mobname = "";

        public string smoblevel = "1";

        public int moblevel = 1;
        
        private ToolStripMenuItem mnuAddDangerFilter;

        private ToolStripMenuItem mnuReloadZoneFilters;

        private ToolStripMenuItem mnuSearchAllakhazam;
        private ToolStripSeparator menuItem3;
        private ToolStripSeparator menuItem2;
        private ToolStripSeparator mnuSep1;
        private ToolStripSeparator mnuSep2;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem addMapLabelToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem mnuStickyTimer;
        private ToolStripMenuItem toolStriConcolor;

        private int ListType;



        // if 0, it's the SpawnList, 1 SpawnTimerList, 2 GroundItemList

        public ListViewPanel(int listType) {

            this.ListType=listType; // 0 = spawn list, 1 = spawn timer list, 2 = ground spawn list

            InitializeComponent();

            //this.DoubleBuffered = true;

            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            // Use reflection to set the ListView control to being double buffered.  This stops the blinking.
            System.Reflection.PropertyInfo listProperty = typeof(System.Windows.Forms.Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            listProperty.SetValue(this.listView, true, null);
          
        }

        protected override string GetPersistString()
        {
            if (ListType == 0)
                return "SpawnList";
            else if (ListType == 1)
                return "SpawnTimerList";
            else
                return "GroundSpawnList";
        }

        public void HideSearchBox()
        {
            this.cmdReset.Visible = false;
            this.txtSpawnList.Visible = false;
            this.txtSpawnList.Text = "";
            this.listView.Location = new System.Drawing.Point(0, 0);
        }

        public void ShowSearchBox()
        {
            this.cmdReset.Visible = true;
            this.txtSpawnList.Visible = true;
            this.listView.Location = new Point(0, 24);
        }

        public void SetComponents(EQData eq,MapCon mapCon,MapPane mapPane, Filters filters,frmMain f1)

        {

            this.eq = eq;

            this.mapCon = mapCon;

            this.mapPane = mapPane;

            this.filters = filters;

            this.f1 = f1;

        }



        protected override void Dispose(bool disposing) {

            if (disposing && components != null)

                components.Dispose();



            base.Dispose( disposing );

        }

        #region Component Designer generated code

        private void InitializeComponent()

        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListViewPanel));
            this.txtSpawnList = new System.Windows.Forms.TextBox();
            this.cmdReset = new System.Windows.Forms.Button();
            this.listView = new System.Windows.Forms.ListView();
            this.mnuContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuAddZoneFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuStickyTimer = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuAddZoneHuntFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAddZoneCautionFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAddZoneDangerFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAddZoneRareFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.addZoneEmailAlertFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuAddGlobalFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAddHuntFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAddCautionFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAddDangerFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAddAlertFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuEditGlobalFilters = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEditZoneFilters = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuReloadZoneFilters = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.addMapLabelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuSearchAllakhazam = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStriConcolor = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuContext.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSpawnList
            // 
            this.txtSpawnList.Location = new System.Drawing.Point(48, 0);
            this.txtSpawnList.Name = "txtSpawnList";
            this.txtSpawnList.Size = new System.Drawing.Size(152, 23);
            this.txtSpawnList.TabIndex = 1;
            this.txtSpawnList.TextChanged += new System.EventHandler(this.txtSpawnList_TextChanged);
            // 
            // cmdReset
            // 
            this.cmdReset.Location = new System.Drawing.Point(0, 0);
            this.cmdReset.Name = "cmdReset";
            this.cmdReset.Size = new System.Drawing.Size(48, 20);
            this.cmdReset.TabIndex = 0;
            this.cmdReset.Text = "Reset";
            this.cmdReset.Click += new System.EventHandler(this.cmdReset_Click);
            // 
            // listView
            // 
            this.listView.AllowColumnReorder = true;
            this.listView.ContextMenuStrip = this.mnuContext;
            this.listView.FullRowSelect = true;
            this.listView.GridLines = true;
            this.listView.HideSelection = false;
            this.listView.LabelWrap = false;
            this.listView.Location = new System.Drawing.Point(0, 24);
            this.listView.MultiSelect = false;
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(200, 168);
            this.listView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView.TabIndex = 2;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView_ColumnClick);
            this.listView.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
            this.listView.VisibleChanged += new System.EventHandler(this.listView_VisibleChanged);
            this.listView.MouseEnter += new System.EventHandler(this.listView_MouseEnter);
            // 
            // mnuContext
            // 
            this.mnuContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAddZoneFilter,
            this.mnuStickyTimer,
            this.toolStriConcolor,
            this.menuItem3,
            this.mnuAddZoneHuntFilter,
            this.mnuAddZoneCautionFilter,
            this.mnuAddZoneDangerFilter,
            this.mnuAddZoneRareFilter,
            this.addZoneEmailAlertFilter,
            this.menuItem2,
            this.mnuAddGlobalFilter,
            this.mnuSep1,
            this.mnuEditGlobalFilters,
            this.mnuEditZoneFilters,
            this.mnuSep2,
            this.mnuReloadZoneFilters,
            this.toolStripSeparator1,
            this.addMapLabelToolStripMenuItem,
            this.toolStripSeparator2,
            this.mnuSearchAllakhazam});
            this.mnuContext.Name = "mnuContext";
            this.mnuContext.Size = new System.Drawing.Size(342, 370);
            this.mnuContext.Opened += new System.EventHandler(this.mnuContext_Opened);
            // 
            // mnuAddZoneFilter
            // 
            this.mnuAddZoneFilter.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.mnuAddZoneFilter.Name = "mnuAddZoneFilter";
            this.mnuAddZoneFilter.Size = new System.Drawing.Size(307, 22);
            this.mnuAddZoneFilter.Text = "\'mob name placeholder\'";
            // 
            // mnuStickyTimer
            // 
            this.mnuStickyTimer.Name = "mnuStickyTimer";
            this.mnuStickyTimer.Size = new System.Drawing.Size(307, 22);
            this.mnuStickyTimer.Text = "Sticky Timer";
            this.mnuStickyTimer.Click += new System.EventHandler(this.mnuStickyTimer_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Name = "menuItem3";
            this.menuItem3.Size = new System.Drawing.Size(304, 6);
            // 
            // mnuAddZoneHuntFilter
            // 
            this.mnuAddZoneHuntFilter.Name = "mnuAddZoneHuntFilter";
            this.mnuAddZoneHuntFilter.Size = new System.Drawing.Size(307, 22);
            this.mnuAddZoneHuntFilter.Text = "Add Zone Hunt Alert Filter";
            this.mnuAddZoneHuntFilter.Click += new System.EventHandler(this.mnuAddZoneHuntFilter_Click);
            // 
            // mnuAddZoneCautionFilter
            // 
            this.mnuAddZoneCautionFilter.Name = "mnuAddZoneCautionFilter";
            this.mnuAddZoneCautionFilter.Size = new System.Drawing.Size(307, 22);
            this.mnuAddZoneCautionFilter.Text = "Add Zone Caution Alert Filter";
            this.mnuAddZoneCautionFilter.Click += new System.EventHandler(this.mnuAddZoneCautionFilter_Click);
            // 
            // mnuAddZoneDangerFilter
            // 
            this.mnuAddZoneDangerFilter.Name = "mnuAddZoneDangerFilter";
            this.mnuAddZoneDangerFilter.Size = new System.Drawing.Size(307, 22);
            this.mnuAddZoneDangerFilter.Text = "Add Zone Danger Alert Filter";
            this.mnuAddZoneDangerFilter.Click += new System.EventHandler(this.mnuAddZoneDangerFilter_Click);
            // 
            // mnuAddZoneRareFilter
            // 
            this.mnuAddZoneRareFilter.Name = "mnuAddZoneRareFilter";
            this.mnuAddZoneRareFilter.Size = new System.Drawing.Size(307, 22);
            this.mnuAddZoneRareFilter.Text = "Add Zone Rare Alert Filter";
            this.mnuAddZoneRareFilter.Click += new System.EventHandler(this.mnuAddZoneAlertFilter_Click);
            // 
            // addZoneEmailAlertFilter
            // 
            this.addZoneEmailAlertFilter.Name = "addZoneEmailAlertFilter";
            this.addZoneEmailAlertFilter.Size = new System.Drawing.Size(307, 22);
            this.addZoneEmailAlertFilter.Text = "Add Email Alert Filter";
            this.addZoneEmailAlertFilter.Click += new System.EventHandler(this.addZoneEmailAlertFilter_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Name = "menuItem2";
            this.menuItem2.Size = new System.Drawing.Size(304, 6);
            // 
            // mnuAddGlobalFilter
            // 
            this.mnuAddGlobalFilter.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAddHuntFilter,
            this.mnuAddCautionFilter,
            this.mnuAddDangerFilter,
            this.mnuAddAlertFilter});
            this.mnuAddGlobalFilter.Name = "mnuAddGlobalFilter";
            this.mnuAddGlobalFilter.Size = new System.Drawing.Size(307, 22);
            this.mnuAddGlobalFilter.Text = "Add \'\' &Global Alert Filter";
            // 
            // mnuAddHuntFilter
            // 
            this.mnuAddHuntFilter.Name = "mnuAddHuntFilter";
            this.mnuAddHuntFilter.Size = new System.Drawing.Size(124, 22);
            this.mnuAddHuntFilter.Text = "Hunt";
            this.mnuAddHuntFilter.Click += new System.EventHandler(this.mnuAddHuntFilter_Click);
            // 
            // mnuAddCautionFilter
            // 
            this.mnuAddCautionFilter.Name = "mnuAddCautionFilter";
            this.mnuAddCautionFilter.Size = new System.Drawing.Size(124, 22);
            this.mnuAddCautionFilter.Text = "Caution";
            this.mnuAddCautionFilter.Click += new System.EventHandler(this.mnuAddCautionFilter_Click);
            // 
            // mnuAddDangerFilter
            // 
            this.mnuAddDangerFilter.Name = "mnuAddDangerFilter";
            this.mnuAddDangerFilter.Size = new System.Drawing.Size(124, 22);
            this.mnuAddDangerFilter.Text = "Danger";
            this.mnuAddDangerFilter.Click += new System.EventHandler(this.mnuAddDangerFilter_Click);
            // 
            // mnuAddAlertFilter
            // 
            this.mnuAddAlertFilter.Name = "mnuAddAlertFilter";
            this.mnuAddAlertFilter.Size = new System.Drawing.Size(124, 22);
            this.mnuAddAlertFilter.Text = "Rare";
            this.mnuAddAlertFilter.Click += new System.EventHandler(this.mnuAddAlertFilter_Click);
            // 
            // mnuSep1
            // 
            this.mnuSep1.Name = "mnuSep1";
            this.mnuSep1.Size = new System.Drawing.Size(304, 6);
            // 
            // mnuEditGlobalFilters
            // 
            this.mnuEditGlobalFilters.Name = "mnuEditGlobalFilters";
            this.mnuEditGlobalFilters.Size = new System.Drawing.Size(307, 22);
            this.mnuEditGlobalFilters.Text = "Edit Global &Alert Filters";
            this.mnuEditGlobalFilters.Click += new System.EventHandler(this.mnuEditGlobalFilters_Click);
            // 
            // mnuEditZoneFilters
            // 
            this.mnuEditZoneFilters.Name = "mnuEditZoneFilters";
            this.mnuEditZoneFilters.Size = new System.Drawing.Size(307, 22);
            this.mnuEditZoneFilters.Text = "Edit Z&one Alert Filters";
            this.mnuEditZoneFilters.Click += new System.EventHandler(this.mnuEditZoneFilters_Click);
            // 
            // mnuSep2
            // 
            this.mnuSep2.Name = "mnuSep2";
            this.mnuSep2.Size = new System.Drawing.Size(304, 6);
            // 
            // mnuReloadZoneFilters
            // 
            this.mnuReloadZoneFilters.Image = ((System.Drawing.Image)(resources.GetObject("mnuReloadZoneFilters.Image")));
            this.mnuReloadZoneFilters.Name = "mnuReloadZoneFilters";
            this.mnuReloadZoneFilters.Size = new System.Drawing.Size(341, 22);
            this.mnuReloadZoneFilters.Text = "&Reload Alert Filters";
            this.mnuReloadZoneFilters.Click += new System.EventHandler(this.mnuReloadFilters_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(304, 6);
            // 
            // addMapLabelToolStripMenuItem
            // 
            this.addMapLabelToolStripMenuItem.Name = "addMapLabelToolStripMenuItem";
            this.addMapLabelToolStripMenuItem.Size = new System.Drawing.Size(307, 22);
            this.addMapLabelToolStripMenuItem.Text = "Add Map Label";
            this.addMapLabelToolStripMenuItem.Click += new System.EventHandler(this.addMapLabelToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(304, 6);
            // 
            // mnuSearchAllakhazam
            // 
            this.mnuSearchAllakhazam.Image = ((System.Drawing.Image)(resources.GetObject("mnuSearchAllakhazam.Image")));
            this.mnuSearchAllakhazam.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuSearchAllakhazam.Name = "mnuSearchAllakhazam";
            this.mnuSearchAllakhazam.Size = new System.Drawing.Size(307, 22);
            this.mnuSearchAllakhazam.Text = "Search bAllakhazam";
            this.mnuSearchAllakhazam.Click += new System.EventHandler(this.mnuSearchAllakhazam_Click);
            // 
            // toolStriConcolor
            // 
            this.toolStriConcolor.CheckOnClick = true;
            this.toolStriConcolor.Font = new System.Drawing.Font("Tahoma", 8.400001F, System.Drawing.FontStyle.Bold);
            this.toolStriConcolor.Image = global::myseq.Properties.Resources.BlackX;
            this.toolStriConcolor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStriConcolor.Name = "toolStriConcolor";
            this.toolStriConcolor.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.C)));
            this.toolStriConcolor.Size = new System.Drawing.Size(341, 22);
            this.toolStriConcolor.Text = "Base Concolor on this Spawn";
            this.toolStriConcolor.Click += new System.EventHandler(this.toolStriConcolor_Click);
            // 
            // ListViewPanel
            // 
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(200, 191);
            this.Controls.Add(this.listView);
            this.Controls.Add(this.cmdReset);
            this.Controls.Add(this.txtSpawnList);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ListViewPanel";
            this.Resize += new System.EventHandler(this.ListViewPanel_Resize);
            this.mnuContext.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private void mnuContext_Opened(object sender, System.EventArgs e)

        {

            mobname = "";

            ListView.SelectedIndexCollection sel = listView.SelectedIndices;
            mnuStickyTimer.Visible = ListType == 1;
            if (sel.Count > 0)
            {
                
                if (ListType == 0)
                {

                    mobname = filterMobName(listView.Items[sel[0]].SubItems[17].Text);
                    mobname = mobname.Replace("_", " ");
                    mobname = mobname.Trim();
                    smoblevel = "";
                    smoblevel = listView.Items[sel[0]].SubItems[1].Text;
                    if (smoblevel.Length > 0)
                    {
                        long Num = 1;
                        bool isNum = long.TryParse(smoblevel, out Num);

                        if (isNum)
                        {
                            moblevel = (int)Num;
                        }
                    }
                }
                else if (ListType == 1)
                {
                    smoblevel = "";
                    smoblevel = listView.Items[sel[0]].SubItems[1].Text;
                    if (smoblevel.Length > 0)
                    {
                        long Num = 1;
                        bool isNum = long.TryParse(smoblevel, out Num);

                        if (isNum)
                        {
                            moblevel = (int)Num;
                        }
                    }
                    mobname = eq.FixMobNameToo(listView.Items[sel[0]].SubItems[0].Text);
                    mobname = mobname.Trim();
                }
                else
                {

                    mobname = listView.Items[sel[0]].SubItems[0].Text;
                }
            }

            if ((sel.Count > 0) && (mobname.Length > 0))

            {

                this.mnuAddGlobalFilter.Text = "Add '" + mobname + "' &Global Alert Filter";

                this.mnuAddZoneFilter.Text = "'" + mobname + "'";

                this.toolStriConcolor.Text = "Base Concolor on '" + mobname + "' (" + smoblevel + ")";
                this.mnuAddZoneFilter.Visible = true;

                this.menuItem3.Visible = true;

                this.mnuAddZoneFilter.Enabled = true;

                this.mnuAddGlobalFilter.Enabled = true;

                this.mnuAddZoneHuntFilter.Enabled = true;

                this.mnuAddZoneCautionFilter.Enabled = true;

                this.mnuAddZoneDangerFilter.Enabled = true;

                this.mnuAddZoneRareFilter.Enabled = true;

                this.addZoneEmailAlertFilter.Enabled = ListType != 2; // Not for ground items

                this.mnuEditZoneFilters.Enabled = true;

                this.mnuEditGlobalFilters.Enabled = true;

                this.mnuReloadZoneFilters.Enabled = true;

                this.mnuSearchAllakhazam.Enabled = true;
                if (ListType == 0)
                {
                    this.addMapLabelToolStripMenuItem.Enabled = true;
                    f1.alertX = float.Parse(listView.Items[sel[0]].SubItems[13].Text);
                    f1.alertY = float.Parse(listView.Items[sel[0]].SubItems[14].Text);
                    f1.alertZ = float.Parse(listView.Items[sel[0]].SubItems[15].Text);


                }
                else if (ListType == 1)
                {
                    // add what is in the menu showing
                    this.addMapLabelToolStripMenuItem.Enabled = true;
                    f1.alertX = float.Parse(listView.Items[sel[0]].SubItems[4].Text);
                    f1.alertY = float.Parse(listView.Items[sel[0]].SubItems[5].Text);
                    f1.alertZ = float.Parse(listView.Items[sel[0]].SubItems[6].Text);
                    // search for a better name to use for this spawn point
                    SPAWNTIMER st = eq.FindListViewTimer(listView.Items[sel[0]]);
                    //SPAWNTIMER st = eq.FindTimer(1.0f, float.Parse(listView.Items[sel[0]].SubItems[4].Text), float.Parse(listView.Items[sel[0]].SubItems[5].Text));
                    if (st != null)
                    {
                        this.mnuStickyTimer.Checked = st.sticky;
                        string[] names = st.allNames.Split(',');
                        foreach (string name in names)
                        {
                            string bname = Regex.Replace(name.Replace("_", " "), "[0-9]", "").Trim();
                            if (Regex.IsMatch(bname, "^[A-Z#]"))
                            {
                                mobname = bname;
                                this.mnuAddZoneFilter.Text = "'" + mobname + "'";
                                f1.alertX = st.X;
                                f1.alertY = st.Y;
                                f1.alertZ = st.Z;
                                break;
                            }
                        }
                    }
                }

            }

            else

            {
                // This is where we update the menu view if no selected item

                this.mnuAddGlobalFilter.Text = "Add '' &Global Filter";

                this.mnuAddZoneFilter.Text = "''";

                this.mnuStickyTimer.Enabled = false;

                this.mnuAddZoneFilter.Visible = false;

                this.menuItem3.Visible = false;

                this.mnuAddGlobalFilter.Enabled = false;

                this.mnuAddZoneFilter.Enabled = false;

                this.mnuAddZoneHuntFilter.Enabled = false;

                this.mnuAddZoneCautionFilter.Enabled = false;

                this.mnuAddZoneDangerFilter.Enabled = false;

                this.mnuAddZoneRareFilter.Enabled = false;

                this.addZoneEmailAlertFilter.Enabled = false;

                this.mnuEditZoneFilters.Enabled = true;

                this.mnuEditGlobalFilters.Enabled = true;

                this.mnuReloadZoneFilters.Enabled = true;

                this.mnuSearchAllakhazam.Enabled = false;

                this.addMapLabelToolStripMenuItem.Enabled = false;

            }

        }

        private static string filterMobName(string name)

        {

            return Regex.Replace(name, "^*[^a-zA-Z_ #'`]", "");

        }

        private void ListViewPanel_Resize(object sender, EventArgs e) {

            try {

                txtSpawnList.Width = this.Width - txtSpawnList.Left;

                listView.Width = this.Width;

                listView.Height = this.Height - listView.Top;

            }

            catch (Exception ex) {LogLib.WriteLine("Error in ListViewPanel.ListViewPanel_Resize: ", ex);}

        }

        private void cmdReset_Click(object sender, System.EventArgs e) {

            try {

                txtSpawnList.Text = "";

                txtSpawnList.Focus();

            }

            catch (Exception ex) {LogLib.WriteLine("Error in ListViewPanel.cmdReset_Click: ", ex);}

        }



        private void txtSpawnList_TextChanged(object sender, EventArgs e) {

            SearchName(txtSpawnList.Text);

        }

        

        public void SearchName(String name)

        {

            try {

                Regex regEx = new Regex(".*" + txtSpawnList.Text + ".*", RegexOptions.IgnoreCase);

                foreach (ListViewItem lstItem in listView.Items) {

                    // Compile the regular expression.

                    // Match the regular expression pattern against a text string.

                    if (regEx.Match(lstItem.Text).Success) {

                        lstItem.EnsureVisible();

                        lstItem.Selected = true;

                        break;

                    }

                }

            }

            catch (Exception ex) {LogLib.WriteLine("Error in ListViewPanel.txtSpawnList_TextChanged: ", ex);}

        }



        private void listView_MouseEnter(object sender, System.EventArgs e) {

            try  { if (!f1.toolStripScale.Focused && !f1.toolStripZPos.Focused && !f1.toolStripZNeg.Focused && !f1.toolStripLookupBox.Focused)
                listView.Focus();}

            catch (Exception ex) {LogLib.WriteLine("Error in ListViewPanel.listView_MouseEnter: ", ex);}

        }

        private void listView_ColumnClick(object sender, ColumnClickEventArgs e) {

            try  {

                curDescend = !curDescend;

                if (ListType == 0)

                {

                    listView.ListViewItemSorter = new ListBoxComparerSpawnList(listView.Items, curDescend, e.Column);

                }

                else if (ListType == 1)

                {

                    listView.ListViewItemSorter = new ListBoxComparerSpawnTimerList(listView.Items, curDescend, e.Column);

                }

                else if (ListType == 2)

                {

                    listView.ListViewItemSorter = new ListBoxComparerGroundItemsList(listView.Items, curDescend, e.Column);

                }

            }

            catch (Exception ex) {LogLib.WriteLine("Error in ListViewPanel.listView_ColumnClick: ", ex);}

        }

        private void listView_SelectedIndexChanged(object sender, System.EventArgs e) {

            ListView.SelectedIndexCollection sel = listView.SelectedIndices;
            listView.HideSelection = false;
            
            if (sel.Count > 0) {
               
                try {
                    if (listView.Visible)
                        listView.Focus();

                    if (ListType == 0)

                    {

                        if (eq != null)

                        {
                            
                            eq.SetSelectedID(int.Parse(listView.Items[sel[0]].SubItems[11].Text));

                        }

                    }

                    else if (ListType == 1)
                    {

                        if (eq != null)
                        {
                            
                            eq.SetSelectedTimer(float.Parse(listView.Items[sel[0]].SubItems[4].Text), float.Parse(listView.Items[sel[0]].SubItems[5].Text));

                        }

                    }

                    else if (ListType == 2)
                    {

                        if (eq != null)
                        {
                            eq.SetSelectedGroundItem(float.Parse(listView.Items[sel[0]].SubItems[3].Text), float.Parse(listView.Items[sel[0]].SubItems[4].Text));

                        }

                    }

                    if (mapCon != null)

                    {

                        mapCon.Invalidate();

                    }

                }

                catch (Exception ex) {LogLib.WriteLine("Error in ListViewPanel.listView_SelectedIndexChanged: ", ex);}

            }

        }

        public void ColumnsAdd(string ColumnName, int ColumnWidth, HorizontalAlignment CoulumnAlign)

        {

            try {listView.Columns.Add(ColumnName, ColumnWidth, CoulumnAlign);}

            catch (Exception ex) {LogLib.WriteLine("Error in ListViewPanel.ColumnsAdd: ", ex);}

        }

        private void mnuAddHuntFilter_Click(object sender, EventArgs e)

        {

            if (f1.dialogBox("Add to Global Alert Filters","Add name to Hunt list:",mobname))

            {

                filters.AddToAlerts(filters.globalHunt,mobname);

                filters.writeAlertFile("global");

                f1.reloadAlertFiles();

            }

        }

        private void mnuAddCautionFilter_Click(object sender, EventArgs e)

        {

            if (f1.dialogBox("Add to Global Alert Filters", "Add name to Caution list:", mobname))

            {

                filters.AddToAlerts(filters.globalCaution, mobname);

                filters.writeAlertFile("global");

                f1.reloadAlertFiles();

            }

        }



        private void mnuAddDangerFilter_Click(object sender, EventArgs e)

        {

            if (f1.dialogBox("Add to Global Alert Filters", "Add name to Danger list:", mobname))

            {

                filters.AddToAlerts(filters.globalDanger, mobname);

                filters.writeAlertFile("global");

                f1.reloadAlertFiles();

            }

        }



        private void mnuAddAlertFilter_Click(object sender, EventArgs e)

        {

            if (f1.dialogBox("Add to Global Alert Filters", "Add name to Rare list:", mobname))

            {

                filters.AddToAlerts(filters.globalAlert, mobname);

                filters.writeAlertFile("global");

                f1.reloadAlertFiles();

            }

        }



        private void mnuAddZoneHuntFilter_Click(object sender, EventArgs e)

        {

            if (f1.dialogBox("Add to Zone Hunt Alert Filters", "Add name to Hunt list:", mobname))

            {

                filters.AddToAlerts(filters.hunt, mobname);

                filters.writeAlertFile(f1.curZone);

                f1.reloadAlertFiles();

            }

        }



        private void mnuAddZoneCautionFilter_Click(object sender, EventArgs e)

        {

            if (f1.dialogBox("Add to Zone Caution Alert Filters", "Add name to Caution list:", mobname))

            {

                filters.AddToAlerts(filters.caution, mobname);

                filters.writeAlertFile(f1.curZone);

                f1.reloadAlertFiles();

            }

        }



        private void mnuAddZoneDangerFilter_Click(object sender, EventArgs e)

        {

            if (f1.dialogBox("Add to Zone Danger Alert Filters", "Add name to Danger list:", mobname))

            {

                filters.AddToAlerts(filters.danger, mobname);

                filters.writeAlertFile(f1.curZone);

                f1.reloadAlertFiles();

            }

        }



        private void mnuAddZoneAlertFilter_Click(object sender, EventArgs e)

        {

            if (f1.dialogBox("Add to Zone Rare Alert Filters", "Add name to Rare list:", mobname))

            {

                filters.AddToAlerts(filters.alert, mobname);

                filters.writeAlertFile(f1.curZone);

                f1.reloadAlertFiles();

            }

        }

        private void addZoneEmailAlertFilter_Click(object sender, EventArgs e)
        {

            if (f1.dialogBox("Add to Zone Email Alert Filters", "Add name to Email list:", mobname))
            {

                filters.AddToAlerts(filters.emailAlert, mobname);

                filters.writeAlertFile(f1.curZone);

                f1.reloadAlertFiles();

            }
        }

        private void mnuEditGlobalFilters_Click(object sender, EventArgs e)

        {

            filters.editAlertFile("global");

        }



        private void mnuEditZoneFilters_Click(object sender, EventArgs e)

        {

            filters.editAlertFile(f1.toolStripShortName.Text);

        }



        private void mnuReloadFilters_Click(object sender, EventArgs e)

        {

            this.filters.ClearArrays();

            f1.reloadAlertFiles();

        }



        private void mnuSearchAllakhazam_Click(object sender, EventArgs e)

        {

            string searchname = Regex.Replace(mobname.Replace("_", " "), "[0-9#]", "").Trim();

            if (searchname.Length > 0)

            {

                string searchURL = String.Format(Settings.Instance.SearchString, searchname);

                System.Diagnostics.Process.Start(searchURL);

            }

        }

        private void addMapLabelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mobname.Length > 0)
                f1.addMapText(mobname);
        }

        private void listView_VisibleChanged(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection selection = listView.SelectedItems;
            if (listView.SelectedItems.Count > 0 && listView.Visible) {
                listView.EnsureVisible(listView.SelectedItems[0].Index);
            }
        }

        private void mnuStickyTimer_Click(object sender, EventArgs e)
        {
            // Set or unset the sticky flag for the timer selected
            ListView.SelectedIndexCollection sel = listView.SelectedIndices;

            if (sel.Count > 0)
            {
                // We only do this for the Spawn Timer List
                if (ListType == 1)
                {
                    // This returns mobsTimer2
                    SPAWNTIMER st = eq.FindListViewTimer(listView.Items[sel[0]]);
                    if (st != null)
                    {
                        this.mnuStickyTimer.Checked = st.sticky;
                        st.sticky = !st.sticky;
                    }
                }
            }
        }

        private void toolStriConcolor_Click(object sender, EventArgs e)
        {
            //mobname;
            Settings.Instance.LevelOverride = moblevel;
            f1.gconLevel = moblevel;
            f1.gConBaseName = mobname;
        }
    }

    public class ListBoxComparerSpawnList : IComparer 

    {

        public int Compare(object a, object b) 

        {

            ListViewItem sa = (ListViewItem)a;

            ListViewItem sb = (ListViewItem)b;

            int res = 0;



            if (Column == 0)    // Name

                res = string.Compare(sa.Text,sb.Text);

            else if (Column == 1)   // Level

            {

                int ia = int.Parse(sa.SubItems[1].Text);

                int ib = int.Parse(sb.SubItems[1].Text);

                if (ia < ib) res = -1;

                else if (ia > ib) res = 1;

                else res = 0;

            } 

            else if ((Column == 2) ||   // Class 

                (Column == 3) ||    // Primary

                (Column == 4) ||    // Offhand

                (Column == 5) ||    // Race

                (Column == 6) ||    // Owner

                (Column == 7) ||    // Last Name

                (Column == 8) ||    // Type

                (Column == 9))      // Invis

                res = string.Compare(sa.SubItems[Column].Text, sb.SubItems[Column].Text);

            else if ((Column == 10) ||   // Run Speed

                (Column == 13) ||   // X

                (Column == 14) ||   // Y

                (Column == 15) ||   // Z

                (Column == 16))     // Distance

            {

                float fa = float.Parse(sa.SubItems[Column].Text);

                float fb = float.Parse(sb.SubItems[Column].Text);

                if (fa < fb) res = -1;

                else if (fa > fb) res = 1;

                else res = 0;

            } 

            else if (Column == 11) 

            { // SpawnID

                uint ia = uint.Parse(sa.SubItems[11].Text);

                uint ib = uint.Parse(sb.SubItems[11].Text);

                if (ia < ib) res = -1;

                else if (ia > ib) res = 1;

                else res = 0;

            } 

            else if (Column == 12) 

            {

                DateTime dta = DateTime.Parse(sa.SubItems[12].Text);

                DateTime dtb = DateTime.Parse(sb.SubItems[12].Text);

                res = DateTime.Compare(dta, dtb);

            }

            

            if (Descending) res = -res;

            

            return res;

        }

        public ListBoxComparerSpawnList(ListView.ListViewItemCollection spawns, bool descending, int column) 

        {

            Spawns = spawns;

            Descending = descending;

            Column = column;

        }



        private ListView.ListViewItemCollection Spawns;

        private bool Descending;

        private int Column;

    }



    public class ListBoxComparerSpawnTimerList : IComparer 

    {

        public int Compare(object a, object b) 

        {

            ListViewItem sa = (ListViewItem)a;

            ListViewItem sb = (ListViewItem)b;

            int res = 0;



            if (Column == 0 || Column == 3)    // Last Spawn = 0  // zone = 3

                res = string.Compare(sa.Text,sb.Text);

            else if (Column == 1 ||   // Countdown
                Column == 2 || // SpawnTimer
                Column == 7)   // SpawnCount

            {

                int ia = int.Parse(sa.SubItems[1].Text);

                int ib = int.Parse(sb.SubItems[1].Text);

                if (ia < ib) res = -1;

                else if (ia > ib) res = 1;

                else res = 0;

            } 

            //else if ()

            //    res = string.Compare(sa.SubItems[Column].Text, sb.SubItems[Column].Text);

            else if (

                (Column == 4) ||   // X

                (Column == 5) ||   // Y

                (Column == 6)    // Z

                )

            {

                float fa = float.Parse(sa.SubItems[Column].Text);

                float fb = float.Parse(sb.SubItems[Column].Text);

                if (fa < fb) res = -1;

                else if (fa > fb) res = 1;

                else res = 0;

            } 



            else if (Column == 8 || // SpawnTime

                Column == 9 || // KillTime

                Column == 10) // NextSpawn

            {

                if (sa.SubItems[Column].Text.Length == 0)

                {

                    res = 1;

                }

                else if (sb.SubItems[Column].Text.Length == 0)

                {

                    res = -1;

                }

                else

                {

                    DateTime dta = DateTime.Parse(sa.SubItems[Column].Text);

                    DateTime dtb = DateTime.Parse(sb.SubItems[Column].Text);

                    res = DateTime.Compare(dta, dtb);

                }

            }

            

            if (Descending) res = -res;

            

            return res;

        }



        public ListBoxComparerSpawnTimerList(ListView.ListViewItemCollection spawns, bool descending, int column) 

        {

            Spawns = spawns;

            Descending = descending;

            Column = column;

        }



        private ListView.ListViewItemCollection Spawns;

        private bool Descending;

        private int Column;

    }

    public class ListBoxComparerGroundItemsList : IComparer
    {

        public int Compare(object a, object b)
        {

            ListViewItem sa = (ListViewItem)a;

            ListViewItem sb = (ListViewItem)b;

            int res = 0;



            if (Column == 0)    // Description

                res = string.Compare(sa.Text, sb.Text);
            
            else if ((Column == 1) ||   // ActorDef

                (Column == 2))    // Spawn Time

                res = string.Compare(sa.SubItems[Column].Text, sb.SubItems[Column].Text);

            else if ((Column == 3) ||   // X

                (Column == 4) ||   // Y

                (Column == 5)) // Z
            {

                float fa = float.Parse(sa.SubItems[Column].Text);

                float fb = float.Parse(sb.SubItems[Column].Text);

                if (fa < fb) res = -1;

                else if (fa > fb) res = 1;

                else res = 0;

            }

            if (Descending) res = -res;

            return res;

        }



        public ListBoxComparerGroundItemsList(ListView.ListViewItemCollection spawns, bool descending, int column)
        {

            Spawns = spawns;

            Descending = descending;

            Column = column;

        }



        private ListView.ListViewItemCollection Spawns;

        private bool Descending;

        private int Column;

    }

}

