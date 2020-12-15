using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace MySEQ
{
    public class MapControls
    {
        private UserControl uc;
        private MapData mapData;
        private Button mapInfo = new Button();
        private Button spInfo = new Button();
        private Label pop1 = new Label();

        public MapControls(UserControl uc, MapData mapData)
        {
            this.uc = uc;
            this.mapData = mapData;
            InitializeWidgets();
        }

        private void PreStyleButton(Button b, String name)
        {
            b.BackColor = Color.Transparent;
            b.Cursor = Cursors.Hand;
            b.FlatStyle = FlatStyle.Flat;
            b.Name = name;
            b.Size = new Size(60, 20);
            b.Text = name;
            b.UseVisualStyleBackColor = false;
        }

        private void InitializeWidgets()
        {
            PreStyleButton(mapInfo, "MapInfo");
            PreStyleButton(spInfo, "Spawns");
            
            pop1.Size = new Size(100, 150);

            UpdateControls();

            mapInfo.MouseEnter += new EventHandler(mapInfo_MouseEnter);
            mapInfo.MouseLeave += new EventHandler(mapInfo_MouseLeave);

            uc.Controls.Add(mapInfo);
            uc.Controls.Add(spInfo);
            uc.Controls.Add(pop1);
        }

        void mapInfo_MouseEnter(object sender, EventArgs e)
        {
            Point p = ((Button) sender).Location;
            p.Offset(-105, 0);
            pop1.Location = p;
            pop1.Text  = mapData.lines.Count.ToString() + " lines\n";
            pop1.Text += mapData.emptyLines.ToString() + " empty lines\n";
            pop1.Text += mapData.labels.Count.ToString() + " labels\n";
            pop1.Show();
        }
        
        void mapInfo_MouseLeave(object sender, EventArgs e)
        {
            pop1.Hide();
        }

        public void UpdateControls()
        {
            mapInfo.Location = new Point(uc.Width - 60, 10);
            spInfo.Location = new Point(uc.Width - 60, 30);
        }
    }
}
