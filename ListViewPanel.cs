using Structures;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace myseq

{
    public class ListViewPanel : DockContent
    {
        public bool curDescend = false;

        private EQData eq;

        private MapCon mapCon;

        private Filters filters;

        private frmMain f1;

        private TextBox txtSpawnList;

        private Button cmdReset;

        public ListView listView;

        private ContextMenuStrip mnuContext;
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

//        private ToolStripMenuItem addZoneEmailAlertFilter;

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
            ListType=listType; // 0 = spawn list, 1 = spawn timer list, 2 = ground spawn list

            InitializeComponent();

            //this.DoubleBuffered = true;

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            // Use reflection to set the ListView control to being double buffered.  This stops the blinking.
            System.Reflection.PropertyInfo listProperty = typeof(Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            listProperty.SetValue(listView, true, null);
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
            cmdReset.Visible = false;
            txtSpawnList.Visible = false;
            txtSpawnList.Text = "";
            listView.Location = new Point(0, 0);
        }

        public void ShowSearchBox()
        {
            cmdReset.Visible = true;
            txtSpawnList.Visible = true;
            listView.Location = new Point(0, 24);
        }

        public void SetComponents(EQData eq,MapCon mapCon, Filters filters,frmMain f1)

        {
            this.eq = eq;

            this.mapCon = mapCon;

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
            components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(ListViewPanel));
            txtSpawnList = new TextBox();
            cmdReset = new Button();
            listView = new ListView();
            mnuContext = new ContextMenuStrip(components);
            mnuAddZoneFilter = new ToolStripMenuItem();
            mnuStickyTimer = new ToolStripMenuItem();
            menuItem3 = new ToolStripSeparator();
            mnuAddZoneHuntFilter = new ToolStripMenuItem();
            mnuAddZoneCautionFilter = new ToolStripMenuItem();
            mnuAddZoneDangerFilter = new ToolStripMenuItem();
            mnuAddZoneRareFilter = new ToolStripMenuItem();
//            addZoneEmailAlertFilter = new ToolStripMenuItem();
            menuItem2 = new ToolStripSeparator();
            mnuAddGlobalFilter = new ToolStripMenuItem();
            mnuAddHuntFilter = new ToolStripMenuItem();
            mnuAddCautionFilter = new ToolStripMenuItem();
            mnuAddDangerFilter = new ToolStripMenuItem();
            mnuAddAlertFilter = new ToolStripMenuItem();
            mnuSep1 = new ToolStripSeparator();
            mnuEditGlobalFilters = new ToolStripMenuItem();
            mnuEditZoneFilters = new ToolStripMenuItem();
            mnuSep2 = new ToolStripSeparator();
            mnuReloadZoneFilters = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            addMapLabelToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            mnuSearchAllakhazam = new ToolStripMenuItem();
            toolStriConcolor = new ToolStripMenuItem();
            mnuContext.SuspendLayout();
            SuspendLayout();
            // 
            // txtSpawnList
            // 
            txtSpawnList.Location = new Point(48, 0);
            txtSpawnList.Name = "txtSpawnList";
            txtSpawnList.Size = new Size(152, 23);
            txtSpawnList.TabIndex = 1;
            txtSpawnList.TextChanged += new EventHandler(txtSpawnList_TextChanged);
            // 
            // cmdReset
            // 
            cmdReset.Location = new Point(0, 0);
            cmdReset.Name = "cmdReset";
            cmdReset.Size = new Size(48, 20);
            cmdReset.TabIndex = 0;
            cmdReset.Text = "Reset";
            cmdReset.Click += new EventHandler(cmdReset_Click);
            // 
            // listView
            // 
            listView.AllowColumnReorder = true;
            listView.ContextMenuStrip = mnuContext;
            listView.FullRowSelect = true;
            listView.GridLines = true;
            listView.HideSelection = false;
            listView.LabelWrap = false;
            listView.Location = new Point(0, 24);
            listView.MultiSelect = false;
            listView.Name = "listView";
            listView.Size = new Size(200, 168);
            listView.Sorting = SortOrder.Ascending;
            listView.TabIndex = 2;
            listView.UseCompatibleStateImageBehavior = false;
            listView.View = View.Details;
            listView.ColumnClick += new ColumnClickEventHandler(listView_ColumnClick);
            listView.SelectedIndexChanged += new EventHandler(listView_SelectedIndexChanged);
            listView.VisibleChanged += new EventHandler(listView_VisibleChanged);
            listView.MouseEnter += new EventHandler(listView_MouseEnter);
            // 
            // mnuContext
            // 
            mnuContext.Items.AddRange(new ToolStripItem[] {
            mnuAddZoneFilter,
            mnuStickyTimer,
            toolStriConcolor,
            menuItem3,
            mnuAddZoneHuntFilter,
            mnuAddZoneCautionFilter,
            mnuAddZoneDangerFilter,
            mnuAddZoneRareFilter,
//            addZoneEmailAlertFilter,
            menuItem2,
            mnuAddGlobalFilter,
            mnuSep1,
            mnuEditGlobalFilters,
            mnuEditZoneFilters,
            mnuSep2,
            mnuReloadZoneFilters,
            toolStripSeparator1,
            addMapLabelToolStripMenuItem,
            toolStripSeparator2,
            mnuSearchAllakhazam});
            mnuContext.Name = "mnuContext";
            mnuContext.Size = new Size(342, 370);
            mnuContext.Opened += new EventHandler(mnuContext_Opened);
            // 
            // mnuAddZoneFilter
            // 
            mnuAddZoneFilter.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            mnuAddZoneFilter.Name = "mnuAddZoneFilter";
            mnuAddZoneFilter.Size = new Size(307, 22);
            mnuAddZoneFilter.Text = "\'mob name placeholder\'";
            // 
            // mnuStickyTimer
            // 
            mnuStickyTimer.Name = "mnuStickyTimer";
            mnuStickyTimer.Size = new Size(307, 22);
            mnuStickyTimer.Text = "Sticky Timer";
            mnuStickyTimer.Click += new EventHandler(mnuStickyTimer_Click);
            // 
            // menuItem3
            // 
            menuItem3.Name = "menuItem3";
            menuItem3.Size = new Size(304, 6);
            // 
            // mnuAddZoneHuntFilter
            // 
            mnuAddZoneHuntFilter.Name = "mnuAddZoneHuntFilter";
            mnuAddZoneHuntFilter.Size = new Size(307, 22);
            mnuAddZoneHuntFilter.Text = "Add Zone Hunt Alert Filter";
            mnuAddZoneHuntFilter.Click += new EventHandler(mnuAddZoneHuntFilter_Click);
            // 
            // mnuAddZoneCautionFilter
            // 
            mnuAddZoneCautionFilter.Name = "mnuAddZoneCautionFilter";
            mnuAddZoneCautionFilter.Size = new Size(307, 22);
            mnuAddZoneCautionFilter.Text = "Add Zone Caution Alert Filter";
            mnuAddZoneCautionFilter.Click += new EventHandler(mnuAddZoneCautionFilter_Click);
            // 
            // mnuAddZoneDangerFilter
            // 
            mnuAddZoneDangerFilter.Name = "mnuAddZoneDangerFilter";
            mnuAddZoneDangerFilter.Size = new Size(307, 22);
            mnuAddZoneDangerFilter.Text = "Add Zone Danger Alert Filter";
            mnuAddZoneDangerFilter.Click += new EventHandler(mnuAddZoneDangerFilter_Click);
            // 
            // mnuAddZoneRareFilter
            // 
            mnuAddZoneRareFilter.Name = "mnuAddZoneRareFilter";
            mnuAddZoneRareFilter.Size = new Size(307, 22);
            mnuAddZoneRareFilter.Text = "Add Zone Rare Alert Filter";
            mnuAddZoneRareFilter.Click += new EventHandler(mnuAddZoneAlertFilter_Click);
            //// 
            //// addZoneEmailAlertFilter
            //// 
            //addZoneEmailAlertFilter.Name = "addZoneEmailAlertFilter";
            //addZoneEmailAlertFilter.Size = new Size(307, 22);
            //addZoneEmailAlertFilter.Text = "Add Email Alert Filter";
            //addZoneEmailAlertFilter.Click += new EventHandler(addZoneEmailAlertFilter_Click);
            // 
            // menuItem2
            // 
            menuItem2.Name = "menuItem2";
            menuItem2.Size = new Size(304, 6);
            // 
            // mnuAddGlobalFilter
            // 
            mnuAddGlobalFilter.DropDownItems.AddRange(new ToolStripItem[] {
            mnuAddHuntFilter,
            mnuAddCautionFilter,
            mnuAddDangerFilter,
            mnuAddAlertFilter});
            mnuAddGlobalFilter.Name = "mnuAddGlobalFilter";
            mnuAddGlobalFilter.Size = new Size(307, 22);
            mnuAddGlobalFilter.Text = "Add \'\' &Global Alert Filter";
            // 
            // mnuAddHuntFilter
            // 
            mnuAddHuntFilter.Name = "mnuAddHuntFilter";
            mnuAddHuntFilter.Size = new Size(124, 22);
            mnuAddHuntFilter.Text = "Hunt";
            mnuAddHuntFilter.Click += new EventHandler(mnuAddHuntFilter_Click);
            // 
            // mnuAddCautionFilter
            // 
            mnuAddCautionFilter.Name = "mnuAddCautionFilter";
            mnuAddCautionFilter.Size = new Size(124, 22);
            mnuAddCautionFilter.Text = "Caution";
            mnuAddCautionFilter.Click += new EventHandler(mnuAddCautionFilter_Click);
            // 
            // mnuAddDangerFilter
            // 
            mnuAddDangerFilter.Name = "mnuAddDangerFilter";
            mnuAddDangerFilter.Size = new Size(124, 22);
            mnuAddDangerFilter.Text = "Danger";
            mnuAddDangerFilter.Click += new EventHandler(mnuAddDangerFilter_Click);
            // 
            // mnuAddAlertFilter
            // 
            mnuAddAlertFilter.Name = "mnuAddAlertFilter";
            mnuAddAlertFilter.Size = new Size(124, 22);
            mnuAddAlertFilter.Text = "Rare";
            mnuAddAlertFilter.Click += new EventHandler(mnuAddAlertFilter_Click);
            // 
            // mnuSep1
            // 
            mnuSep1.Name = "mnuSep1";
            mnuSep1.Size = new Size(304, 6);
            // 
            // mnuEditGlobalFilters
            // 
            mnuEditGlobalFilters.Name = "mnuEditGlobalFilters";
            mnuEditGlobalFilters.Size = new Size(307, 22);
            mnuEditGlobalFilters.Text = "Edit Global &Alert Filters";
            mnuEditGlobalFilters.Click += new EventHandler(mnuEditGlobalFilters_Click);
            // 
            // mnuEditZoneFilters
            // 
            mnuEditZoneFilters.Name = "mnuEditZoneFilters";
            mnuEditZoneFilters.Size = new Size(307, 22);
            mnuEditZoneFilters.Text = "Edit Z&one Alert Filters";
            mnuEditZoneFilters.Click += new EventHandler(mnuEditZoneFilters_Click);
            // 
            // mnuSep2
            // 
            mnuSep2.Name = "mnuSep2";
            mnuSep2.Size = new Size(304, 6);
            // 
            // mnuReloadZoneFilters
            // 
            mnuReloadZoneFilters.Image = (Image)resources.GetObject("mnuReloadZoneFilters.Image");
            mnuReloadZoneFilters.Name = "mnuReloadZoneFilters";
            mnuReloadZoneFilters.Size = new Size(341, 22);
            mnuReloadZoneFilters.Text = "&Reload Alert Filters";
            mnuReloadZoneFilters.Click += new EventHandler(mnuReloadFilters_Click);
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(304, 6);
            // 
            // addMapLabelToolStripMenuItem
            // 
            addMapLabelToolStripMenuItem.Name = "addMapLabelToolStripMenuItem";
            addMapLabelToolStripMenuItem.Size = new Size(307, 22);
            addMapLabelToolStripMenuItem.Text = "Add Map Label";
            addMapLabelToolStripMenuItem.Click += new EventHandler(addMapLabelToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(304, 6);
            // 
            // mnuSearchAllakhazam
            // 
            mnuSearchAllakhazam.Image = (Image)resources.GetObject("mnuSearchAllakhazam.Image");
            mnuSearchAllakhazam.ImageTransparentColor = Color.Magenta;
            mnuSearchAllakhazam.Name = "mnuSearchAllakhazam";
            mnuSearchAllakhazam.Size = new Size(307, 22);
            mnuSearchAllakhazam.Text = "Search Allakhazam";
            mnuSearchAllakhazam.Click += new EventHandler(MnuSearchAllakhazam_Click);
            // 
            // toolStriConcolor
            // 
            toolStriConcolor.CheckOnClick = true;
            toolStriConcolor.Font = new Font("Tahoma", 8.400001F, FontStyle.Bold);
            toolStriConcolor.Image = Properties.Resources.BlackX;
            toolStriConcolor.ImageTransparentColor = Color.Magenta;
            toolStriConcolor.Name = "toolStriConcolor";
            toolStriConcolor.ShortcutKeys = Keys.Alt | Keys.C;
            toolStriConcolor.Size = new Size(341, 22);
            toolStriConcolor.Text = "Base Concolor on this Spawn";
            toolStriConcolor.Click += new EventHandler(toolStriConcolor_Click);
            // 
            // ListViewPanel
            // 
            BackColor = SystemColors.Window;
            ClientSize = new Size(200, 191);
            Controls.Add(listView);
            Controls.Add(cmdReset);
            Controls.Add(txtSpawnList);
            Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "ListViewPanel";
            Resize += new EventHandler(ListViewPanel_Resize);
            mnuContext.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private void mnuContext_Opened(object sender, EventArgs e)

        {
            mobname = "";

            ListView.SelectedIndexCollection sel = listView.SelectedIndices;
            mnuStickyTimer.Visible = ListType == 1;
            if (sel.Count > 0)
            {
                if (ListType == 0)
                {
                    mobname = RegexHelper.FilterMobName(listView.Items[sel[0]].SubItems[18].Text);
                    mobname = mobname.Replace("_", " ");
                    mobname = mobname.Trim();
                    smoblevel = "";
                    smoblevel = listView.Items[sel[0]].SubItems[1].Text;
                    if (smoblevel.Length > 0)
                    {
                        bool isNum = long.TryParse(smoblevel, out var Num);

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
                        bool isNum = long.TryParse(smoblevel, out var Num);

                        if (isNum)
                        {
                            moblevel = (int)Num;
                        }
                    }
                    mobname = RegexHelper.FixMobNameMatch(listView.Items[sel[0]].SubItems[0].Text);
                    mobname = mobname.Trim();
                }
                else
                {
                    mobname = listView.Items[sel[0]].SubItems[0].Text;
                }
            }

            if ((sel.Count > 0) && (mobname.Length > 0))

            {
                mnuAddGlobalFilter.Text = "Add '" + mobname + "' &Global Alert Filter";

                mnuAddZoneFilter.Text = "'" + mobname + "'";

                toolStriConcolor.Text = "Base Concolor on '" + mobname + "' (" + smoblevel + ")";
                mnuAddZoneFilter.Visible = true;

                menuItem3.Visible = true;

                mnuAddZoneFilter.Enabled = true;

                mnuAddGlobalFilter.Enabled = true;

                mnuAddZoneHuntFilter.Enabled = true;

                mnuAddZoneCautionFilter.Enabled = true;

                mnuAddZoneDangerFilter.Enabled = true;

                mnuAddZoneRareFilter.Enabled = true;

//                addZoneEmailAlertFilter.Enabled = ListType != 2; // Not for ground items

                mnuEditZoneFilters.Enabled = true;

                mnuEditGlobalFilters.Enabled = true;

                mnuReloadZoneFilters.Enabled = true;

                mnuSearchAllakhazam.Enabled = true;
                if (ListType == 0)
                {
                    addMapLabelToolStripMenuItem.Enabled = true;
                    f1.alertX = float.Parse(listView.Items[sel[0]].SubItems[13].Text);
                    f1.alertY = float.Parse(listView.Items[sel[0]].SubItems[14].Text);
                    f1.alertZ = float.Parse(listView.Items[sel[0]].SubItems[15].Text);
                }
                else if (ListType == 1)
                {
                    // add what is in the menu showing
                    addMapLabelToolStripMenuItem.Enabled = true;
                    f1.alertX = float.Parse(listView.Items[sel[0]].SubItems[4].Text);
                    f1.alertY = float.Parse(listView.Items[sel[0]].SubItems[5].Text);
                    f1.alertZ = float.Parse(listView.Items[sel[0]].SubItems[6].Text);
                    // search for a better name to use for this spawn point
                    SPAWNTIMER st = eq.FindListViewTimer(listView.Items[sel[0]]);
                    //SPAWNTIMER st = eq.FindTimer(1.0f, float.Parse(listView.Items[sel[0]].SubItems[4].Text), float.Parse(listView.Items[sel[0]].SubItems[5].Text));
                    if (st != null)
                    {
                        mnuStickyTimer.Checked = st.sticky;
                        foreach (string name in st.AllNames.Split(','))
                        {
                            var bname = RegexHelper.TrimName(name);
                            if (RegexHelper.RegexMatch(bname))
                            {
                                mobname = bname;
                                mnuAddZoneFilter.Text = "'" + mobname + "'";
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

                mnuAddGlobalFilter.Text = "Add '' &Global Filter";

                mnuAddZoneFilter.Text = "''";

                mnuStickyTimer.Enabled = false;

                mnuAddZoneFilter.Visible = false;

                menuItem3.Visible = false;

                mnuAddGlobalFilter.Enabled = false;

                mnuAddZoneFilter.Enabled = false;

                mnuAddZoneHuntFilter.Enabled = false;

                mnuAddZoneCautionFilter.Enabled = false;

                mnuAddZoneDangerFilter.Enabled = false;

                mnuAddZoneRareFilter.Enabled = false;

//                addZoneEmailAlertFilter.Enabled = false;

                mnuEditZoneFilters.Enabled = true;

                mnuEditGlobalFilters.Enabled = true;

                mnuReloadZoneFilters.Enabled = true;

                mnuSearchAllakhazam.Enabled = false;

                addMapLabelToolStripMenuItem.Enabled = false;
            }
        }

        //private static string filterMobName(string name)

        //{

        //    return Regex.Replace(name, "^*[^a-zA-Z_ #'`]", "");

        //}

        private void ListViewPanel_Resize(object sender, EventArgs e) {
            try {
                txtSpawnList.Width = Width - txtSpawnList.Left;

                listView.Width = Width;

                listView.Height = Height - listView.Top;
            }
            catch (Exception ex) {LogLib.WriteLine("Error in ListViewPanel.ListViewPanel_Resize: ", ex);}
        }

        private void cmdReset_Click(object sender, EventArgs e) {
            try {
                txtSpawnList.Text = "";

                txtSpawnList.Focus();
            }
            catch (Exception ex) {LogLib.WriteLine("Error in ListViewPanel.cmdReset_Click: ", ex);}
        }

        private void txtSpawnList_TextChanged(object sender, EventArgs e) {
            SearchName(txtSpawnList.Text);
        }

        public void SearchName(string name)

        {
            try {
                foreach (ListViewItem lstItem in listView.Items) {
                    // Match the regular expression pattern against a text string.
                    if (RegexHelper.GetRegex(name).Match(lstItem.Text).Success) {
                        lstItem.EnsureVisible();

                        lstItem.Selected = true;

                        break;
                    }
                }
            }
            catch (Exception ex) {LogLib.WriteLine("Error in ListViewPanel.txtSpawnList_TextChanged: ", ex);}
        }

        private void listView_MouseEnter(object sender, EventArgs e) {
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

        private void listView_SelectedIndexChanged(object sender, EventArgs e) {
            ListView.SelectedIndexCollection sel = listView.SelectedIndices;
            listView.HideSelection = false;

            if (sel.Count > 0) {
                try {
                    if (listView.Visible)
                        listView.Focus();

                    if (ListType == 0)

                    {
                        eq?.SetSelectedID(int.Parse(listView.Items[sel[0]].SubItems[11].Text));
                    }
                    else if (ListType == 1)
                    {
                        eq?.SetSelectedTimer(float.Parse(listView.Items[sel[0]].SubItems[4].Text), float.Parse(listView.Items[sel[0]].SubItems[5].Text));
                    }
                    else if (ListType == 2)
                    {
                        eq?.SetSelectedGroundItem(float.Parse(listView.Items[sel[0]].SubItems[3].Text), float.Parse(listView.Items[sel[0]].SubItems[4].Text));
                    }

                    mapCon?.Invalidate();
                }
                catch (Exception ex) {LogLib.WriteLine("Error in ListViewPanel.listView_SelectedIndexChanged: ", ex);}
            }
        }

        public void ColumnsAdd(string ColumnName, int ColumnWidth, HorizontalAlignment CoulumnAlign)

        {
            try {listView.Columns.Add(ColumnName, ColumnWidth, CoulumnAlign);}
            catch (Exception ex) { LogLib.WriteLine("Error in ListViewPanel.ColumnsAdd: ", ex); }
        }

        private void mnuAddHuntFilter_Click(object sender, EventArgs e)

        {
            if (f1.dialogBox("Add to Global Alert Filters", "Add name to Hunt list:", mobname))
                AddFilter(filters.GlobalHunt, "global");
        }

        private void AddFilter(ArrayList fltr, string zone)
        {
            filters.AddToAlerts(fltr, mobname);

            filters.WriteAlertFile(zone);

            f1.reloadAlertFiles();
        }

        private void mnuAddCautionFilter_Click(object sender, EventArgs e)

        {
            if (f1.dialogBox("Add to Global Alert Filters", "Add name to Caution list:", mobname))
                AddFilter(filters.GlobalCaution, "global");
        }

        private void mnuAddDangerFilter_Click(object sender, EventArgs e)

        {
            if (f1.dialogBox("Add to Global Alert Filters", "Add name to Danger list:", mobname))
                AddFilter(filters.GlobalDanger, "global");
        }

        private void mnuAddAlertFilter_Click(object sender, EventArgs e)

        {
            if (f1.dialogBox("Add to Global Alert Filters", "Add name to Rare list:", mobname))
                AddFilter(filters.GlobalAlert, "global");
        }

        private void mnuAddZoneHuntFilter_Click(object sender, EventArgs e)

        {
            if (f1.dialogBox("Add to Zone Hunt Alert Filters", "Add name to Hunt list:", mobname))
                AddFilter(filters.Hunt, f1.curZone);
        }

        private void mnuAddZoneCautionFilter_Click(object sender, EventArgs e)

        {
            if (f1.dialogBox("Add to Zone Caution Alert Filters", "Add name to Caution list:", mobname))
                AddFilter(filters.Caution, f1.curZone);
        }

        private void mnuAddZoneDangerFilter_Click(object sender, EventArgs e)

        {
            if (f1.dialogBox("Add to Zone Danger Alert Filters", "Add name to Danger list:", mobname))
                AddFilter(filters.Danger, f1.curZone);
        }

        private void mnuAddZoneAlertFilter_Click(object sender, EventArgs e)

        {
            if (f1.dialogBox("Add to Zone Rare Alert Filters", "Add name to Rare list:", mobname))
                AddFilter(filters.Alert, f1.curZone);
        }

        private void mnuEditGlobalFilters_Click(object sender, EventArgs e)

        {
            filters.EditAlertFile("global");
        }

        private void mnuEditZoneFilters_Click(object sender, EventArgs e)

        {
            filters.EditAlertFile(f1.curZone);
        }

        private void mnuReloadFilters_Click(object sender, EventArgs e)

        {
            filters.ClearArrays();

            f1.reloadAlertFiles();
        }

        private void MnuSearchAllakhazam_Click(object sender, EventArgs e)
        {
            var searchname = RegexHelper.SearchName(mobname);

            if (searchname.Length > 0)

            {
                var searchURL = string.Format(Settings.Instance.SearchString, searchname);

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
                        mnuStickyTimer.Checked = st.sticky;
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
            {
                res = string.Compare(sa.Text, sb.Text);
            }
            else if (Column == 1)   // Level

            {
                int ia = int.Parse(sa.SubItems[1].Text);

                int ib = int.Parse(sb.SubItems[1].Text);

                if (ia < ib) res = -1;
                else res = ia > ib ? 1 : 0;
            }
            else if ((Column == 2) ||   // Class 

                (Column == 3) ||    // Primary

                (Column == 4) ||    // Offhand

                (Column == 5) ||    // Race

                (Column == 6) ||    // Owner

                (Column == 7) ||    // Last Name

                (Column == 8) ||    // Type

                (Column == 9) ||    // Invis

                (Column == 17))     // Guild


            {
                res = string.Compare(sa.SubItems[Column].Text, sb.SubItems[Column].Text);
            }
            else if ((Column == 10) ||   // Run Speed

               (Column == 13) ||   // X

               (Column == 14) ||   // Y

               (Column == 15) ||   // Z

               (Column == 16))     // Distance

            {
                float fa = float.Parse(sa.SubItems[Column].Text);

                float fb = float.Parse(sb.SubItems[Column].Text);

                if (fa < fb) res = -1;
                else res = fa > fb ? 1 : 0;
            }
            else if (Column == 11)

            { // SpawnID
                uint ia = uint.Parse(sa.SubItems[11].Text);

                uint ib = uint.Parse(sb.SubItems[11].Text);

                if (ia < ib) res = -1;
                else res = ia > ib ? 1 : 0;
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
            {
                res = string.Compare(sa.Text, sb.Text);
            }
            else if (Column == 1 ||   // Countdown
               Column == 2 || // SpawnTimer
               Column == 7)   // SpawnCount

            {
                int ia = int.Parse(sa.SubItems[1].Text);

                int ib = int.Parse(sb.SubItems[1].Text);

                if (ia < ib) res = -1;
                else res = ia > ib ? 1 : 0;
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
                else res = fa > fb ? 1 : 0;
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
            {
                res = string.Compare(sa.Text, sb.Text);
            }
            else if ((Column == 1) ||   // ActorDef

               (Column == 2))    // Spawn Time
            {
                res = string.Compare(sa.SubItems[Column].Text, sb.SubItems[Column].Text);
            }
            else if ((Column == 3) ||   // X

               (Column == 4) ||   // Y

               (Column == 5)) // Z
            {
                float fa = float.Parse(sa.SubItems[Column].Text);

                float fb = float.Parse(sb.SubItems[Column].Text);

                if (fa < fb) res = -1;
                else res = fa > fb ? 1 : 0;
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

