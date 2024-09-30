using myseq.Properties;
using Structures;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace myseq
{
    public class ListViewPanel : DockContent
    {
        private bool curDescend;

        private EQData eq;

        private Filters filters;

        private MainForm f1;

        private TextBox txtSpawnList;

        private Button SearchBoxReset;

        public ListView listView;

        private IContainer components;

        private ToolStripMenuItem mnuAddGlobalFilter;

        private ToolStripMenuItem mnuAddZoneFilter;

        private ToolStripMenuItem mnuAddZoneHuntFilter;

        private ToolStripMenuItem mnuAddZoneCautionFilter;

        private ToolStripMenuItem mnuAddZoneDangerFilter;

        private ToolStripMenuItem mnuAddZoneRareFilter;

        private ToolStripMenuItem mnuEditGlobalFilters;

        private ToolStripMenuItem mnuEditZoneFilters;

        private ToolStripMenuItem mnuReloadZoneFilters;

        private ToolStripMenuItem mnuSearchAllakhazam;

        private ToolStripMenuItem addMapLabelToolStripMenuItem;

        private ToolStripMenuItem mnuStickyTimer;

        private ToolStripMenuItem toolStriConcolor;

        private ToolStripSeparator mnuSep3;

        public string Mobname { get; set; } = "";

        private int moblevel = 1;

        //private int moblevel = 1;

        private readonly int ListType;
        private int lastSortedColumn = -1;

        // if 0, it's the SpawnList, 1 SpawnTimerList, 2 GroundItemList

        public ListViewPanel(int listType)
        {
            ListType = listType; // 0 = spawn list, 1 = spawn timer list, 2 = ground spawn list

            InitializeComponent();
            Font = new Font(Settings.Default.ListFont.FontFamily, Settings.Default.ListFont.Size, Settings.Default.ListFont.Style);

            if (listType == 0) // Add Columns to Spawnlist window
            {
                ColumnsAdd("Name", Settings.Default.c1w, HorizontalAlignment.Left);
                ColumnsAdd("Level", Settings.Default.c2w, HorizontalAlignment.Left);
                ColumnsAdd("Class", Settings.Default.c3w, HorizontalAlignment.Left);
                ColumnsAdd("Primary", Settings.Default.c3w, HorizontalAlignment.Left);
                ColumnsAdd("Offhand", Settings.Default.c3w, HorizontalAlignment.Left);
                ColumnsAdd("Race", Settings.Default.c4w, HorizontalAlignment.Left);
                ColumnsAdd("Owner", Settings.Default.c4w, HorizontalAlignment.Left);
                ColumnsAdd("Last Name", Settings.Default.c5w, HorizontalAlignment.Left);
                ColumnsAdd("Type", Settings.Default.c6w, HorizontalAlignment.Left);
                ColumnsAdd("Invis", Settings.Default.c7w, HorizontalAlignment.Left);
                ColumnsAdd("Run Speed", Settings.Default.c8w, HorizontalAlignment.Left);
                ColumnsAdd("SpawnID", Settings.Default.c9w, HorizontalAlignment.Left);
                ColumnsAdd("Spawn Time", Settings.Default.c10w, HorizontalAlignment.Left);
                ColumnsAdd("X", Settings.Default.c11w, HorizontalAlignment.Left);
                ColumnsAdd("Y", Settings.Default.c12w, HorizontalAlignment.Left);
                ColumnsAdd("Z", Settings.Default.c13w, HorizontalAlignment.Left);
                ColumnsAdd("Distance", Settings.Default.c14w, HorizontalAlignment.Left);
                //            SpawnList.ColumnsAdd("Guild", Settings.Default.c14w, HorizontalAlignment.Left); //17
            }
            else if (listType == 1)     // Add the Columns to the Spawn Timer Window
            {
                ColumnsAdd("Spawn Name", Settings.Default.c1w, HorizontalAlignment.Left);
                ColumnsAdd("Remain", Settings.Default.c10w, HorizontalAlignment.Left);
                ColumnsAdd("Interval", Settings.Default.c10w, HorizontalAlignment.Left);
                ColumnsAdd("Zone", Settings.Default.c10w, HorizontalAlignment.Left);
                ColumnsAdd("X", Settings.Default.c12w, HorizontalAlignment.Left);
                ColumnsAdd("Y", Settings.Default.c11w, HorizontalAlignment.Left);
                ColumnsAdd("Z", Settings.Default.c13w, HorizontalAlignment.Left);
                ColumnsAdd("Count", Settings.Default.c9w, HorizontalAlignment.Left);
                ColumnsAdd("Spawn Time", Settings.Default.c10w, HorizontalAlignment.Left);
                ColumnsAdd("Kill Time", Settings.Default.c10w, HorizontalAlignment.Left);
                ColumnsAdd("Next Spawn", Settings.Default.c10w, HorizontalAlignment.Left);
            }
            else if (listType == 2)     // Add Columns to Ground Items window
            {
                ColumnsAdd("Description", Settings.Default.c1w, HorizontalAlignment.Left);
                ColumnsAdd("Name", Settings.Default.c1w, HorizontalAlignment.Left);
                ColumnsAdd("Spawn Time", Settings.Default.c10w, HorizontalAlignment.Left);
                ColumnsAdd("X", Settings.Default.c12w, HorizontalAlignment.Left);
                ColumnsAdd("Y", Settings.Default.c11w, HorizontalAlignment.Left);
                ColumnsAdd("Z", Settings.Default.c13w, HorizontalAlignment.Left);
            }

            DoubleBuffered = true;

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
            {
                return "SpawnList";
            }
            else if (ListType == 1)
            {
                return "SpawnTimerList";
            }
            else
            {
                return "GroundSpawnList";
            }
        }

        public void HideSearchBox()
        {
            SearchBoxReset.Visible = false;
            txtSpawnList.Visible = false;
            txtSpawnList.Text = "";
            listView.Location = new Point(0, 0);
        }

        public void ShowSearchBox()
        {
            SearchBoxReset.Visible = true;
            txtSpawnList.Visible = true;
            listView.Location = new Point(0, 24);
        }

        public void SetComponents(EQData eq, Filters filters, MainForm f1)
        {
            this.eq = eq;

            this.filters = filters;

            this.f1 = f1;
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
            components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(ListViewPanel));
            txtSpawnList = new TextBox();
            SearchBoxReset = new Button();
            listView = new ListView();
            ContextMenuStrip mnuContext = new ContextMenuStrip(components);
            mnuAddZoneFilter = new ToolStripMenuItem();
            mnuStickyTimer = new ToolStripMenuItem();
            toolStriConcolor = new ToolStripMenuItem();
            mnuAddZoneHuntFilter = new ToolStripMenuItem();
            mnuAddZoneCautionFilter = new ToolStripMenuItem();
            mnuAddZoneDangerFilter = new ToolStripMenuItem();
            mnuAddZoneRareFilter = new ToolStripMenuItem();
            mnuAddGlobalFilter = new ToolStripMenuItem();
            ToolStripMenuItem mnuAddHuntFilter = new ToolStripMenuItem();
            ToolStripMenuItem mnuAddCautionFilter = new ToolStripMenuItem();
            ToolStripMenuItem mnuAddDangerFilter = new ToolStripMenuItem();
            ToolStripMenuItem mnuAddAlertFilter = new ToolStripMenuItem();
            mnuEditGlobalFilters = new ToolStripMenuItem();
            mnuEditZoneFilters = new ToolStripMenuItem();
            mnuReloadZoneFilters = new ToolStripMenuItem();
            addMapLabelToolStripMenuItem = new ToolStripMenuItem();
            ToolStripSeparator mnuSep1 = new ToolStripSeparator();
            ToolStripSeparator mnuSep2 = new ToolStripSeparator();
            mnuSep3 = new ToolStripSeparator();
            ToolStripSeparator mnuSep4 = new ToolStripSeparator();
            ToolStripSeparator mnuSep5 = new ToolStripSeparator();
            ToolStripSeparator mnuSep6 = new ToolStripSeparator();
            mnuSearchAllakhazam = new ToolStripMenuItem();
            mnuContext.SuspendLayout();
            SuspendLayout();
            //
            // txtSpawnList
            //
            txtSpawnList.Location = new Point(48, 0);
            txtSpawnList.Name = "txtSpawnList";
            txtSpawnList.Size = new Size(152, 20);
            txtSpawnList.TabIndex = 1;
            txtSpawnList.TextChanged += (TxtSpawnList_TextChanged);
            //
            // SearchBoxReset
            //
            SearchBoxReset.Location = new Point(0, 0);
            SearchBoxReset.Name = "SearchBoxReset";
            SearchBoxReset.Size = new Size(48, 20);
            SearchBoxReset.TabIndex = 0;
            SearchBoxReset.Text = "Reset";
            SearchBoxReset.Click += (SearchboxReset_Click);
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
            mnuSep3,
            mnuAddZoneHuntFilter,
            mnuAddZoneCautionFilter,
            mnuAddZoneDangerFilter,
            mnuAddZoneRareFilter,
            mnuSep4,
            mnuAddGlobalFilter,
            mnuSep1,
            mnuEditGlobalFilters,
            mnuEditZoneFilters,
            mnuSep2,
            mnuReloadZoneFilters,
            mnuSep5,
            addMapLabelToolStripMenuItem,
            mnuSep6,
            mnuSearchAllakhazam});
            mnuContext.Name = "mnuContext";
            mnuContext.Size = new Size(297, 326);
            mnuContext.Opened += new EventHandler(MnuContext_Opened);
            //
            // mnuAddZoneFilter
            //
            mnuAddZoneFilter.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            mnuAddZoneFilter.Name = "mnuAddZoneFilter";
            mnuAddZoneFilter.Size = new Size(296, 22);
            mnuAddZoneFilter.Text = "\'mob name placeholder\'";
            //
            // mnuStickyTimer
            //
            mnuStickyTimer.Name = "mnuStickyTimer";
            mnuStickyTimer.Size = new Size(296, 22);
            mnuStickyTimer.Text = "Sticky Timer";
            mnuStickyTimer.Click += new EventHandler(MnuStickyTimer_Click);
            //
            // toolStriConcolor
            //
            toolStriConcolor.CheckOnClick = true;
            toolStriConcolor.Font = new Font("Tahoma", 8.400001F, FontStyle.Bold);
            toolStriConcolor.Image = Resources.BlackX;
            toolStriConcolor.ImageTransparentColor = Color.Magenta;
            toolStriConcolor.Name = "toolStriConcolor";
            toolStriConcolor.ShortcutKeys = (Keys.Alt | Keys.C);
            toolStriConcolor.Size = new Size(296, 22);
            toolStriConcolor.Text = "Base Concolor on this Spawn";
            toolStriConcolor.Click += new EventHandler(ToolStriConcolor_Click);
            //
            // mnuSep3
            //
            mnuSep3.Name = "mnuSep3";
            mnuSep3.Size = new Size(293, 6);
            //
            // mnuAddZoneHuntFilter
            //
            mnuAddZoneHuntFilter.Name = "mnuAddZoneHuntFilter";
            mnuAddZoneHuntFilter.Size = new Size(296, 22);
            mnuAddZoneHuntFilter.Text = "Add Zone Hunt Alert Filter";
            mnuAddZoneHuntFilter.Click += new EventHandler(MnuAddZoneHuntFilter_Click);
            //
            // mnuAddZoneCautionFilter
            //
            mnuAddZoneCautionFilter.Name = "mnuAddZoneCautionFilter";
            mnuAddZoneCautionFilter.Size = new Size(296, 22);
            mnuAddZoneCautionFilter.Text = "Add Zone Caution Alert Filter";
            mnuAddZoneCautionFilter.Click += new EventHandler(MnuAddZoneCautionFilter_Click);
            //
            // mnuAddZoneDangerFilter
            //
            mnuAddZoneDangerFilter.Name = "mnuAddZoneDangerFilter";
            mnuAddZoneDangerFilter.Size = new Size(296, 22);
            mnuAddZoneDangerFilter.Text = "Add Zone Danger Alert Filter";
            mnuAddZoneDangerFilter.Click += new EventHandler(MnuAddZoneDangerFilter_Click);
            //
            // mnuAddZoneRareFilter
            //
            mnuAddZoneRareFilter.Name = "mnuAddZoneRareFilter";
            mnuAddZoneRareFilter.Size = new Size(296, 22);
            mnuAddZoneRareFilter.Text = "Add Zone Rare Alert Filter";
            mnuAddZoneRareFilter.Click += new EventHandler(MnuAddZoneAlertFilter_Click);
            //
            // mnuSep4
            //
            mnuSep4.Name = "mnuSep4";
            mnuSep4.Size = new Size(293, 6);
            //
            // mnuAddGlobalFilter
            //
            mnuAddGlobalFilter.DropDownItems.AddRange(new ToolStripItem[] {
            mnuAddHuntFilter,
            mnuAddCautionFilter,
            mnuAddDangerFilter,
            mnuAddAlertFilter});
            mnuAddGlobalFilter.Name = "mnuAddGlobalFilter";
            mnuAddGlobalFilter.Size = new Size(296, 22);
            mnuAddGlobalFilter.Text = "Add \'\' &Global Alert Filter";
            //
            // mnuAddHuntFilter
            //
            mnuAddHuntFilter.Name = "mnuAddHuntFilter";
            mnuAddHuntFilter.Size = new Size(180, 22);
            mnuAddHuntFilter.Text = "Hunt";
            mnuAddHuntFilter.Click += new EventHandler(MnuAddHuntFilter_Click);
            //
            // mnuAddCautionFilter
            //
            mnuAddCautionFilter.Name = "mnuAddCautionFilter";
            mnuAddCautionFilter.Size = new Size(180, 22);
            mnuAddCautionFilter.Text = "Caution";
            mnuAddCautionFilter.Click += new EventHandler(MnuAddCautionFilter_Click);
            //
            // mnuAddDangerFilter
            //
            mnuAddDangerFilter.Name = "mnuAddDangerFilter";
            mnuAddDangerFilter.Size = new Size(180, 22);
            mnuAddDangerFilter.Text = "Danger";
            mnuAddDangerFilter.Click += new EventHandler(MnuAddDangerFilter_Click);
            //
            // mnuAddAlertFilter
            //
            mnuAddAlertFilter.Name = "mnuAddAlertFilter";
            mnuAddAlertFilter.Size = new Size(180, 22);
            mnuAddAlertFilter.Text = "Rare";
            mnuAddAlertFilter.Click += new EventHandler(MnuAddAlertFilter_Click);
            //
            // mnuSep1
            //
            mnuSep1.Name = "mnuSep1";
            mnuSep1.Size = new Size(293, 6);
            //
            // mnuEditGlobalFilters
            //
            mnuEditGlobalFilters.Name = "mnuEditGlobalFilters";
            mnuEditGlobalFilters.Size = new Size(296, 22);
            mnuEditGlobalFilters.Text = "Edit Global &Alert Filters";
            mnuEditGlobalFilters.Click += new EventHandler(MnuEditGlobalFilters_Click);
            //
            // mnuEditZoneFilters
            //
            mnuEditZoneFilters.Name = "mnuEditZoneFilters";
            mnuEditZoneFilters.Size = new Size(296, 22);
            mnuEditZoneFilters.Text = "Edit Z&one Alert Filters";
            mnuEditZoneFilters.Click += new EventHandler(MnuEditZoneFilters_Click);
            //
            // mnuSep2
            //
            mnuSep2.Name = "mnuSep2";
            mnuSep2.Size = new Size(293, 6);
            //
            // mnuReloadZoneFilters
            //
            mnuReloadZoneFilters.Image = ((Image)(resources.GetObject("mnuReloadZoneFilters.Image")));
            mnuReloadZoneFilters.Name = "mnuReloadZoneFilters";
            mnuReloadZoneFilters.Size = new Size(296, 22);
            mnuReloadZoneFilters.Text = "&Reload Alert Filters";
            mnuReloadZoneFilters.Click += new EventHandler(MnuReloadFilters_Click);
            //
            // mnuSep5
            //
            mnuSep5.Name = "mnuSep5";
            mnuSep5.Size = new Size(293, 6);
            //
            // addMapLabelToolStripMenuItem
            //
            addMapLabelToolStripMenuItem.Name = "addMapLabelToolStripMenuItem";
            addMapLabelToolStripMenuItem.Size = new Size(296, 22);
            addMapLabelToolStripMenuItem.Text = "Add Map Label";
            addMapLabelToolStripMenuItem.Click += new EventHandler(AddMapLabelToolStripMenuItem_Click);
            //
            // mnuSep6
            //
            mnuSep6.Name = "mnuSep6";
            mnuSep6.Size = new Size(293, 6);
            //
            // mnuSearchAllakhazam
            //
            mnuSearchAllakhazam.Image = ((Image)(resources.GetObject("mnuSearchAllakhazam.Image")));
            mnuSearchAllakhazam.ImageTransparentColor = Color.Magenta;
            mnuSearchAllakhazam.Name = "mnuSearchAllakhazam";
            mnuSearchAllakhazam.Size = new Size(296, 22);
            mnuSearchAllakhazam.Text = "Search Allakhazam";
            mnuSearchAllakhazam.Click += new EventHandler(MnuSearchAllakhazam_Click);
            //
            // ListViewPanel
            //
            BackColor = SystemColors.Window;
            ClientSize = new Size(200, 191);
            Controls.Add(listView);
            Controls.Add(SearchBoxReset);
            Controls.Add(txtSpawnList);
            Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Icon = ((Icon)(resources.GetObject("$this.Icon")));
            Name = "ListViewPanel";
            Resize += new EventHandler(ListViewPanel_Resize);
            mnuContext.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion Component Designer generated code

        private void MnuContext_Opened(object sender, EventArgs e)
        {
            Mobname = "";

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
                    Mobname = listView.Items[sel[0]].SubItems[0].Text;
                }
            }

            if ((sel.Count > 0) && (Mobname.Length > 0))

            {
                mnuAddGlobalFilter.Text = $"Add '{Mobname}' &Global Alert Filter";

                mnuAddZoneFilter.Text = $"'{Mobname}'";

                toolStriConcolor.Text = $"Base Concolor on '{Mobname}' ({moblevel})";
                mnuAddZoneFilter.Visible = true;

                mnuSep3.Visible = true;

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
                    Spawntimer st = eq.FindListViewTimer(listView.Items[sel[0]]);

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

            mnuSep3.Visible = false;

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
            Mobname = listView.Items[sel[0]].SubItems[17].Text.FilterMobName();
            Mobname = Mobname.Replace("_", " ");
            Mobname = Mobname.Trim();
            //smoblevel = "";
            MobLevel(sel);
        }

        private void MobLevel(ListView.SelectedIndexCollection sel)
        {
            var smoblevelText = listView.Items[sel[0]].SubItems[1].Text;
            if (int.TryParse(smoblevelText, out var Num))
            {
                moblevel = Num;
            }
            else
            {
                moblevel = 1;  // Default value or handle error accordingly
            }
        }

        private void ListType1(ListView.SelectedIndexCollection sel)
        {
            moblevel = 1;
            MobLevel(sel);
            Mobname = listView.Items[sel[0]].SubItems[0].Text.FixMobNameMatch();
            Mobname = Mobname.Trim();
        }

        private void StickyTimer(Spawntimer st)
        {
            mnuStickyTimer.Checked = st.sticky;
            foreach (var name in st.AllNames.Split(','))
            {
                var bname = name.TrimName();
                if (bname.RegexMatch())
                {
                    Mobname = bname;
                    mnuAddZoneFilter.Text = $"'{Mobname}'";
                    f1.alertX = st.X;
                    f1.alertY = st.Y;
                    f1.alertZ = st.Z;
                    break;
                }
            }
        }

        private void ListViewPanel_Resize(object sender, EventArgs e)
        {
            txtSpawnList.Width = Width - txtSpawnList.Left;
            listView.Width = Width;
            listView.Height = Height - listView.Top;
        }

        private void SearchboxReset_Click(object sender, EventArgs e)
        {
            txtSpawnList.Text = "";
            txtSpawnList.Focus();
        }

        private void TxtSpawnList_TextChanged(object sender, EventArgs e) => SearchName(txtSpawnList.Text);

        public void SearchName(string name)
        {
            try
            {
                foreach (ListViewItem listItem in listView.Items)
                {
                    // Match the regular expression pattern against a text string.
                    if (name.GetRegex().Match(listItem.Text).Success)
                    {
                        listItem.EnsureVisible();

                        listItem.Selected = true;

                        break;
                    }
                }
            }
            catch (Exception ex) { LogLib.WriteLine("Error in ListViewPanel.txtSpawnList_TextChanged: ", ex); }
        }

        private void ListView_MouseEnter(object sender, EventArgs e)
        {
            if (!f1.toolStripScale.Focused && !f1.toolStripZPos.Focused && !f1.toolStripZNeg.Focused && !f1.toolStripLookupBox1.Focused)
            {
                listView.Focus();
            }
        }

        private void ListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Toggle sort order if the same column is clicked, otherwise reset to ascending.
            if (e.Column == lastSortedColumn)
            {
                curDescend = !curDescend;
            }
            else
            {
                curDescend = false;  // Default to ascending when a new column is clicked.
            }

            lastSortedColumn = e.Column;
            SortOrder sortOrder = curDescend ? SortOrder.Ascending : SortOrder.Descending;
            // Ensure listView is not null.
            if (listView != null)
            {
                listView.ListViewItemSorter = new ListViewComparer(e.Column, sortOrder);
                listView.Sort();  // Trigger the sorting immediately.
            }
        }

        private void ListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection sel = listView.SelectedIndices;
            listView.HideSelection = false;

            if (sel.Count > 0)
            {
                if (listView.Visible)
                {
                    listView.Focus();
                }

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

                f1.MapConInvalidate();
            }
        }

        public void ColumnsAdd(string ColumnName, int ColumnWidth, HorizontalAlignment CoulumnAlign)
        {
            listView.Columns.Add(ColumnName, ColumnWidth, CoulumnAlign);
        }

        private void AddFilter(List<string> fltr, string zone)
        {
            filters.AddToAlerts(fltr, Mobname);

            filters.WriteAlertFile(zone);

            f1.ReloadAlertFiles();
        }

        private void MnuAddHuntFilter_Click(object sender, EventArgs e)
        {
            if (f1.DialogBox("Add to Global Alert Filters", "Add name to Hunt list:", Mobname))
            {
                AddFilter(Filters.GlobalHunt, "global");
            }
        }

        private void MnuAddCautionFilter_Click(object sender, EventArgs e)
        {
            if (f1.DialogBox("Add to Global Alert Filters", "Add name to Caution list:", Mobname))
            {
                AddFilter(Filters.GlobalCaution, "global");
            }
        }

        private void MnuAddDangerFilter_Click(object sender, EventArgs e)

        {
            if (f1.DialogBox("Add to Global Alert Filters", "Add name to Danger list:", Mobname))
            {
                AddFilter(Filters.GlobalDanger, "global");
            }
        }

        private void MnuAddAlertFilter_Click(object sender, EventArgs e)

        {
            if (f1.DialogBox("Add to Global Alert Filters", "Add name to Rare list:", Mobname))
            {
                AddFilter(Filters.GlobalAlert, "global");
            }
        }

        private void MnuAddZoneHuntFilter_Click(object sender, EventArgs e)

        {
            if (f1.DialogBox("Add to Zone Hunt Alert Filters", "Add name to Hunt list:", Mobname))
            {
                AddFilter(Filters.Hunt, f1.CurZone);
            }
        }

        private void MnuAddZoneCautionFilter_Click(object sender, EventArgs e)

        {
            if (f1.DialogBox("Add to Zone Caution Alert Filters", "Add name to Caution list:", Mobname))
            {
                AddFilter(Filters.Caution, f1.CurZone);
            }
        }

        private void MnuAddZoneDangerFilter_Click(object sender, EventArgs e)

        {
            if (f1.DialogBox("Add to Zone Danger Alert Filters", "Add name to Danger list:", Mobname))
            {
                AddFilter(Filters.Danger, f1.CurZone);
            }
        }

        private void MnuAddZoneAlertFilter_Click(object sender, EventArgs e)

        {
            if (f1.DialogBox("Add to Zone Rare Alert Filters", "Add name to Rare list:", Mobname))
            {
                AddFilter(Filters.Alert, f1.CurZone);
            }
        }

        private void MnuEditGlobalFilters_Click(object sender, EventArgs e) => filters.EditAlertFile("global");

        private void MnuEditZoneFilters_Click(object sender, EventArgs e) => filters.EditAlertFile(f1.CurZone);

        private void MnuReloadFilters_Click(object sender, EventArgs e) =>
            f1.ReloadAlertFiles();

        private void MnuSearchAllakhazam_Click(object sender, EventArgs e)
        {
            Mobname.StartSearch();
        }

        private void AddMapLabelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Mobname.Length > 0)
            {
                f1.AddMapText(Mobname);
            }
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
            // We only do this for the Spawn Timer List
            if (sel.Count > 0 && ListType == 1)
            {
                // This returns mobsTimer2
                Spawntimer st = eq.FindListViewTimer(listView.Items[sel[0]]);
                if (st != null)
                {
                    mnuStickyTimer.Checked = st.sticky;
                    st.sticky = !st.sticky;
                }
            }
        }

        private void ToolStriConcolor_Click(object sender, EventArgs e)
        {
            Settings.Default.LevelOverride = moblevel;
            eq.GConBaseName = Mobname;
        }
    }
}