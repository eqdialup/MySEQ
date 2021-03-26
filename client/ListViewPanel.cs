using myseq.Properties;
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

        private FrmMain f1;

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

        private readonly int ListType;

        // if 0, it's the SpawnList, 1 SpawnTimerList, 2 GroundItemList

        public ListViewPanel(int listType)
        {
            ListType = listType; // 0 = spawn list, 1 = spawn timer list, 2 = ground spawn list

            InitializeComponent();

            //this.DoubleBuffered = true;

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            // Use reflection to set the ListView control to being double buffered.  This stops the blinking.
            System.Reflection.PropertyInfo listProperty = typeof(Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            listProperty.SetValue(listView, true, null);
        }

        protected override string GetPersistString() => ListType == 0 ? "SpawnList" : ListType == 1 ? "SpawnTimerList" : "GroundSpawnList";

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

        public void SetComponents(EQData eq, MapCon mapCon, Filters filters, FrmMain f1)

        {
            this.eq = eq;

            this.mapCon = mapCon;

            this.filters = filters;

            this.f1 = f1;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();

            base.Dispose(disposing);
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
            txtSpawnList.TextChanged += new EventHandler(TxtSpawnList_TextChanged);
            // 
            // cmdReset
            // 
            cmdReset.Location = new Point(0, 0);
            cmdReset.Name = "cmdReset";
            cmdReset.Size = new Size(48, 20);
            cmdReset.TabIndex = 0;
            cmdReset.Text = "Reset";
            cmdReset.Click += new EventHandler(CmdReset_Click);
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
            listView.ColumnClick += new ColumnClickEventHandler(ListView_ColumnClick);
            listView.SelectedIndexChanged += new EventHandler(ListView_SelectedIndexChanged);
            listView.VisibleChanged += new EventHandler(ListView_VisibleChanged);
            listView.MouseEnter += new EventHandler(ListView_MouseEnter);
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
            mnuContext.Opened += new EventHandler(MnuContext_Opened);
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
            mnuStickyTimer.Click += new EventHandler(MnuStickyTimer_Click);
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
            mnuAddZoneHuntFilter.Click += new EventHandler(MnuAddZoneHuntFilter_Click);
            // 
            // mnuAddZoneCautionFilter
            // 
            mnuAddZoneCautionFilter.Name = "mnuAddZoneCautionFilter";
            mnuAddZoneCautionFilter.Size = new Size(307, 22);
            mnuAddZoneCautionFilter.Text = "Add Zone Caution Alert Filter";
            mnuAddZoneCautionFilter.Click += new EventHandler(MnuAddZoneCautionFilter_Click);
            // 
            // mnuAddZoneDangerFilter
            // 
            mnuAddZoneDangerFilter.Name = "mnuAddZoneDangerFilter";
            mnuAddZoneDangerFilter.Size = new Size(307, 22);
            mnuAddZoneDangerFilter.Text = "Add Zone Danger Alert Filter";
            mnuAddZoneDangerFilter.Click += new EventHandler(MnuAddZoneDangerFilter_Click);
            // 
            // mnuAddZoneRareFilter
            // 
            mnuAddZoneRareFilter.Name = "mnuAddZoneRareFilter";
            mnuAddZoneRareFilter.Size = new Size(307, 22);
            mnuAddZoneRareFilter.Text = "Add Zone Rare Alert Filter";
            mnuAddZoneRareFilter.Click += new EventHandler(MnuAddZoneAlertFilter_Click);
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
            mnuAddHuntFilter.Click += new EventHandler(MnuAddHuntFilter_Click);
            // 
            // mnuAddCautionFilter
            // 
            mnuAddCautionFilter.Name = "mnuAddCautionFilter";
            mnuAddCautionFilter.Size = new Size(124, 22);
            mnuAddCautionFilter.Text = "Caution";
            mnuAddCautionFilter.Click += new EventHandler(MnuAddCautionFilter_Click);
            // 
            // mnuAddDangerFilter
            // 
            mnuAddDangerFilter.Name = "mnuAddDangerFilter";
            mnuAddDangerFilter.Size = new Size(124, 22);
            mnuAddDangerFilter.Text = "Danger";
            mnuAddDangerFilter.Click += new EventHandler(MnuAddDangerFilter_Click);
            // 
            // mnuAddAlertFilter
            // 
            mnuAddAlertFilter.Name = "mnuAddAlertFilter";
            mnuAddAlertFilter.Size = new Size(124, 22);
            mnuAddAlertFilter.Text = "Rare";
            mnuAddAlertFilter.Click += new EventHandler(MnuAddAlertFilter_Click);
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
            mnuEditGlobalFilters.Click += new EventHandler(MnuEditGlobalFilters_Click);
            // 
            // mnuEditZoneFilters
            // 
            mnuEditZoneFilters.Name = "mnuEditZoneFilters";
            mnuEditZoneFilters.Size = new Size(307, 22);
            mnuEditZoneFilters.Text = "Edit Z&one Alert Filters";
            mnuEditZoneFilters.Click += new EventHandler(MnuEditZoneFilters_Click);
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
            mnuReloadZoneFilters.Click += new EventHandler(MnuReloadFilters_Click);
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
            addMapLabelToolStripMenuItem.Click += new EventHandler(AddMapLabelToolStripMenuItem_Click);
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
            toolStriConcolor.Click += new EventHandler(ToolStriConcolor_Click);
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

        private void MnuContext_Opened(object sender, EventArgs e)

        {
            mobname = "";

            ListView.SelectedIndexCollection sel = listView.SelectedIndices;
            mnuStickyTimer.Visible = ListType == 1;
            if (sel.Count > 0)
            {
                if (ListType == 0)
                {
                    ListType0(sel);
                }
                else if (ListType == 1)
                {
                    ListType1(sel);
                }
                else
                {
                    mobname = listView.Items[sel[0]].SubItems[0].Text;
                }
            }

            if ((sel.Count > 0) && (mobname.Length > 0))

            {
                mnuAddGlobalFilter.Text = $"Add '{mobname}' &Global Alert Filter";

                mnuAddZoneFilter.Text = $"'{mobname}'";

                toolStriConcolor.Text = $"Base Concolor on '{mobname}' ({smoblevel})";
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

                    if (st != null)
                    {
                        StickyTimer(st);
                    }
                }
            }
            else
            {
                NoSelection();
            }
        }

        private void NoSelection()
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

        private void ListType0(ListView.SelectedIndexCollection sel)
        {
            mobname = RegexHelper.FilterMobName(listView.Items[sel[0]].SubItems[17].Text);
            mobname = mobname.Replace("_", " ");
            mobname = mobname.Trim();
            //smoblevel = "";
            smoblevel = listView.Items[sel[0]].SubItems[1].Text;
            if (smoblevel.Length > 0)
            {
                GetMobLevel();
            }
        }

        private void ListType1(ListView.SelectedIndexCollection sel)
        {
            smoblevel = "";
            smoblevel = listView.Items[sel[0]].SubItems[1].Text;
            if (smoblevel.Length > 0)
            {
                GetMobLevel();
            }
            mobname = RegexHelper.FixMobNameMatch(listView.Items[sel[0]].SubItems[0].Text);
            mobname = mobname.Trim();
        }

        private void StickyTimer(SPAWNTIMER st)
        {
            mnuStickyTimer.Checked = st.sticky;
            foreach (string name in st.AllNames.Split(','))
            {
                var bname = RegexHelper.TrimName(name);
                if (RegexHelper.RegexMatch(bname))
                {
                    mobname = bname;
                    mnuAddZoneFilter.Text = $"'{mobname}'";
                    f1.alertX = st.X;
                    f1.alertY = st.Y;
                    f1.alertZ = st.Z;
                    break;
                }
            }
        }

        private void GetMobLevel()
        {
            if (int.TryParse(smoblevel, out var Num))
            {
                moblevel = Num;
            }
        }

        private void ListViewPanel_Resize(object sender, EventArgs e)
        {
            try
            {
                txtSpawnList.Width = Width - txtSpawnList.Left;

                listView.Width = Width;

                listView.Height = Height - listView.Top;
            }
            catch (Exception ex) { LogLib.WriteLine("Error in ListViewPanel.ListViewPanel_Resize: ", ex); }
        }

        private void CmdReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtSpawnList.Text = "";

                txtSpawnList.Focus();
            }
            catch (Exception ex) { LogLib.WriteLine("Error in ListViewPanel.cmdReset_Click: ", ex); }
        }

        private void TxtSpawnList_TextChanged(object sender, EventArgs e) => SearchName(txtSpawnList.Text);

        public void SearchName(string name)

        {
            try
            {
                foreach (ListViewItem lstItem in listView.Items)
                {
                    // Match the regular expression pattern against a text string.
                    if (RegexHelper.GetRegex(name).Match(lstItem.Text).Success)
                    {
                        lstItem.EnsureVisible();

                        lstItem.Selected = true;

                        break;
                    }
                }
            }
            catch (Exception ex) { LogLib.WriteLine("Error in ListViewPanel.txtSpawnList_TextChanged: ", ex); }
        }

        private void ListView_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                if (!f1.toolStripScale.Focused && !f1.toolStripZPos.Focused && !f1.toolStripZNeg.Focused && !f1.toolStripLookupBox.Focused)
                    listView.Focus();
            }
            catch (Exception ex) { LogLib.WriteLine("Error in ListViewPanel.listView_MouseEnter: ", ex); }
        }

        private void ListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
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
            catch (Exception ex) { LogLib.WriteLine("Error in ListViewPanel.listView_ColumnClick: ", ex); }
        }

        private void ListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection sel = listView.SelectedIndices;
            listView.HideSelection = false;

            if (sel.Count > 0)
            {
                try
                {
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
                catch (Exception ex) { LogLib.WriteLine("Error in ListViewPanel.listView_SelectedIndexChanged: ", ex); }
            }
        }

        public void ColumnsAdd(string ColumnName, int ColumnWidth, HorizontalAlignment CoulumnAlign)

        {
            try { listView.Columns.Add(ColumnName, ColumnWidth, CoulumnAlign); }
            catch (Exception ex) { LogLib.WriteLine("Error in ListViewPanel.ColumnsAdd: ", ex); }
        }

        private void MnuAddHuntFilter_Click(object sender, EventArgs e)

        {
            if (f1.DialogBox("Add to Global Alert Filters", "Add name to Hunt list:", mobname))
                AddFilter(filters.GlobalHunt, "global");
        }

        private void AddFilter(ArrayList fltr, string zone)
        {
            filters.AddToAlerts(fltr, mobname);

            filters.WriteAlertFile(zone);

            f1.ReloadAlertFiles();
        }

        private void MnuAddCautionFilter_Click(object sender, EventArgs e)

        {
            if (f1.DialogBox("Add to Global Alert Filters", "Add name to Caution list:", mobname))
                AddFilter(filters.GlobalCaution, "global");
        }

        private void MnuAddDangerFilter_Click(object sender, EventArgs e)

        {
            if (f1.DialogBox("Add to Global Alert Filters", "Add name to Danger list:", mobname))
                AddFilter(filters.GlobalDanger, "global");
        }

        private void MnuAddAlertFilter_Click(object sender, EventArgs e)

        {
            if (f1.DialogBox("Add to Global Alert Filters", "Add name to Rare list:", mobname))
                AddFilter(filters.GlobalAlert, "global");
        }

        private void MnuAddZoneHuntFilter_Click(object sender, EventArgs e)

        {
            if (f1.DialogBox("Add to Zone Hunt Alert Filters", "Add name to Hunt list:", mobname))
                AddFilter(filters.Hunt, f1.curZone);
        }

        private void MnuAddZoneCautionFilter_Click(object sender, EventArgs e)

        {
            if (f1.DialogBox("Add to Zone Caution Alert Filters", "Add name to Caution list:", mobname))
                AddFilter(filters.Caution, f1.curZone);
        }

        private void MnuAddZoneDangerFilter_Click(object sender, EventArgs e)

        {
            if (f1.DialogBox("Add to Zone Danger Alert Filters", "Add name to Danger list:", mobname))
                AddFilter(filters.Danger, f1.curZone);
        }

        private void MnuAddZoneAlertFilter_Click(object sender, EventArgs e)

        {
            if (f1.DialogBox("Add to Zone Rare Alert Filters", "Add name to Rare list:", mobname))
                AddFilter(filters.Alert, f1.curZone);
        }

        private void MnuEditGlobalFilters_Click(object sender, EventArgs e) => filters.EditAlertFile("global");

        private void MnuEditZoneFilters_Click(object sender, EventArgs e) => filters.EditAlertFile(f1.curZone);

        private void MnuReloadFilters_Click(object sender, EventArgs e)

        {
            filters.ClearArrays();

            f1.ReloadAlertFiles();
        }

        private void MnuSearchAllakhazam_Click(object sender, EventArgs e)
        {
            var searchname = RegexHelper.SearchName(mobname);

            if (!string.IsNullOrEmpty(searchname))

            {
                var searchURL = string.Format(Settings.Default.SearchString, searchname);

                System.Diagnostics.Process.Start(searchURL);
            }
        }

        private void AddMapLabelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mobname.Length > 0)
                f1.AddMapText(mobname);
        }

        private void ListView_VisibleChanged(object sender, EventArgs e)
        {
            _ = listView.SelectedItems;
            if (listView.SelectedItems.Count > 0 && listView.Visible)
            {
                listView.EnsureVisible(listView.SelectedItems[0].Index);
            }
        }

        private void MnuStickyTimer_Click(object sender, EventArgs e)
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

        private void ToolStriConcolor_Click(object sender, EventArgs e)
        {
            //mobname;
            Settings.Default.LevelOverride = moblevel;
            //f1.gconLevel = moblevel;
            eq.GConBaseName = mobname;
        }
    }
}