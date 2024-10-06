using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using myseq.Properties;

namespace myseq
{
public partial class FrmAddMapText : Form
{
    public FrmAddMapText() : this(string.Empty, string.Empty) { }

    // New constructor to initialize text and map name
    public FrmAddMapText(string initialText, string mapName)
    {
        InitializeComponent();
        txtColr = Settings.Default.SelectedAddMapText;
        txtBkg = Settings.Default.BackColor;
        txtAdd = initialText.Length > 0 ? initialText : "Enter Text Label";
        this.mapName = !string.IsNullOrWhiteSpace(mapName) && mapName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase)
                       ? $"Add to Map: {mapName.GetLastSlash()}"
                       : "Add to Map: ";
    }

    private void BtnAddTxtOk_Click(object sender, EventArgs e)
    {
        txtColr = Settings.Default.SelectedAddMapText;
        Close();
    }

    private void BtnAddTxtCancel_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void Button1_Click(object sender, EventArgs e)
    {
        if (colorDialog1.ShowDialog() == DialogResult.OK)
        {
            selectedColor = colorDialog1.Color;
            Settings.Default.SelectedAddMapText = colorDialog1.Color;
            pictureBox1.BackColor = colorDialog1.Color;
        }
    }

    public bool TryAddMapText(MainForm f1)
    {
        if (string.IsNullOrWhiteSpace(txtAdd))
        {
            MessageBox.Show("The text label is empty or invalid. Please enter a valid text label.", "Invalid Text", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        // Trim any unnecessary trailing characters from the text
        txtAdd = txtAdd.Trim();

        // Add it to the map and handle saving it
        SOEMapTextAdd(txtAdd, f1);

        return true;
    }

    public void SOEMapTextAdd(string new_text, MainForm f1)
    {
        // Create the map label with the necessary details
        Color color = txtColr;
        Point3D position = new Point3D(f1.alertX, f1.alertY, f1.alertZ);
        MapLabel work = new MapLabel(position, color, new_text);

        f1.map.AddMapText(work);

        // Prompt the user to save the label to the map file
        if (DialogResult.Yes == MessageBox.Show($"Do you want to write the label to {mapName}?" +
                Environment.NewLine + Environment.NewLine + work, "Write label to map",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
        {
            try
            {
                IFormatProvider NumFormat = new CultureInfo("en-US");
                var soe_maptext = string.Format(NumFormat, "P {0:f3}, {1:f3}, {2}, {3}, {4}, {5}, {6}, {7}", 
                                                f1.alertX * -1, f1.alertY * -1, f1.alertZ, txtColr.R, txtColr.G, txtColr.B, txtSize, new_text);
                File.AppendAllText(f1.MapnameWithLabels, soe_maptext);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Access Violation: {ex.Message}", "Add Text to Map Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        else
        {
            f1.map.DeleteMapText(work);
        }
    }
}

}