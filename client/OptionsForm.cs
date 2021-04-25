using System;
using System.Windows.Forms;
using myseq.Properties;

namespace myseq
{
    public partial class OptionsForm : Form
    {
        private readonly FormMethods formMethod = new FormMethods();
        public OptionsForm()
        {
            InitializeComponent();
            FormMethods.SetOptions(this);
        }

        #region ClickResponders
        private void CmdCommand_Click(object sender, EventArgs e)
        {
            if (chkSaveOnExit.Checked)
            {
                Settings.Default.Save();
            }
            Hide();
        }

        private void CmdMapBackgroundColor_Click(object sender, EventArgs e)
        {
            colorOptionPicker.Color = picMapBackgroundColor.BackColor;

            if (colorOptionPicker.ShowDialog() != DialogResult.Cancel)
            {
                picMapBackgroundColor.BackColor = colorOptionPicker.Color;
            }
        }

        private void CmdListBackgroundColor_Click(object sender, EventArgs e)

        {
            colorOptionPicker.Color = picListBackgroundColor.BackColor;

            if (colorOptionPicker.ShowDialog() != DialogResult.Cancel)

            {
                Settings.Default.ListBackColor = colorOptionPicker.Color;

                picListBackgroundColor.BackColor = colorOptionPicker.Color;
            }
        }

        private void CmdGridColor_Click(object sender, EventArgs e)

        {
            colorOptionPicker.Color = picGridColor.BackColor;

            if (colorOptionPicker.ShowDialog() != DialogResult.Cancel)

            {
                Settings.Default.GridColor = colorOptionPicker.Color;

                picGridColor.BackColor = colorOptionPicker.Color;
            }
        }

        private void CmdRangeCircleColor_Click(object sender, EventArgs e)
        {
            colorOptionPicker.Color = picRangeCircleColor.BackColor;

            if (colorOptionPicker.ShowDialog() != DialogResult.Cancel)

            {
                Settings.Default.RangeCircleColor = colorOptionPicker.Color;

                picRangeCircleColor.BackColor = colorOptionPicker.Color;
            }
        }

        private void CmdMapDirBrowse_Click(object sender, EventArgs e)
        {
            txtMapDir.Text = formMethod.FolderBrowser("Map Directory", Settings.Default.MapDir);
            if (!string.IsNullOrEmpty(txtMapDir.Text)) Settings.Default.MapDir = txtMapDir.Text;
        }

        private void CmdCfgDirBrowse_Click(object sender, EventArgs e)
        {
            txtCfgDir.Text = formMethod.FolderBrowser("Config Directory", Settings.Default.CfgDir);
            if (!string.IsNullOrEmpty(txtCfgDir.Text))Settings.Default.CfgDir = txtCfgDir.Text;
        }

        private void CmdFilterDirBrowse_Click(object sender, EventArgs e)
        {
            txtFilterDir.Text = formMethod.FolderBrowser("Filter Directory", Settings.Default.FilterDir);
            if (!string.IsNullOrEmpty(txtFilterDir.Text))Settings.Default.FilterDir = txtFilterDir.Text;
        }

        private void CmdLogDir_Click(object sender, EventArgs e)
        {
            txtLogDir.Text = formMethod.FolderBrowser("Log Directory", Settings.Default.LogDir);
            if (!string.IsNullOrEmpty(txtLogDir.Text)) Settings.Default.LogDir = txtLogDir.Text;
        }

        private void CmdSpawnTimers_Click(object sender, EventArgs e)
        {
            txtTimerDir.Text = formMethod.FolderBrowser("Timers log Directory", Settings.Default.TimerDir);
            if (!string.IsNullOrEmpty(txtTimerDir.Text)) Settings.Default.TimerDir = txtTimerDir.Text;
        }

        private void CmdGridLabelColor_Click(object sender, EventArgs e)
        {
            colorOptionPicker.Color = picGridLabelColor.BackColor;

            if (colorOptionPicker.ShowDialog() != DialogResult.Cancel)
            {
                Settings.Default.GridLabelColor = colorOptionPicker.Color;

                picGridLabelColor.BackColor = colorOptionPicker.Color;
            }
        }

        private void ButPlayerBorder_Click(object sender, EventArgs e)
        {
            colorOptionPicker.Color = picPlayerBorder.BackColor;

            if (colorOptionPicker.ShowDialog() != DialogResult.Cancel)
            {
                picPlayerBorder.BackColor = colorOptionPicker.Color;
            }
        }

        private void CmbAlertSound_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Settings.Default.AlertSound = cmbAlertSound.SelectedItem.ToString();
        }

        private void SpnSpawnSize_ValueChanged(object sender, EventArgs e) => Settings.Default.SpawnDrawSize = (int)spnSpawnSize.Value;

        private void PvpLevels_ValueChanged(object sender, EventArgs e) => Settings.Default.PVPLevels = (int)pvpLevels.Value;

        private void ChkShowZoneName_CheckedChanged(object sender, EventArgs e) => Settings.Default.ShowZoneName = chkShowZoneName.Checked;

        private void ChkShowCharName_CheckedChanged(object sender, EventArgs e) => Settings.Default.ShowCharName = chkShowZoneName.Checked;

        private void ChkSaveOnExit_CheckedChanged(object sender, EventArgs e) => Settings.Default.SaveOnExit = chkSaveOnExit.Checked;
        #endregion
    }
}
