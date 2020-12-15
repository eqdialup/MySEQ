using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace myseq
{
	public class ListBoxPanel : System.Windows.Forms.UserControl {
		public System.Windows.Forms.ListView lstSpawnList;

		private System.ComponentModel.Container components = null;

		public ListBoxPanel() {
			InitializeComponent();
		}

		protected override void Dispose(bool disposing) {
			if (disposing && components != null)
				components.Dispose();

			base.Dispose( disposing );
		}

		#region Component Designer generated code

		private void InitializeComponent()
		{
			this.lstSpawnList = new System.Windows.Forms.ListView();
			this.SuspendLayout();
			// 
			// lstSpawnList
			// 
			this.lstSpawnList.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lstSpawnList.GridLines = true;
			this.lstSpawnList.HideSelection = false;
			this.lstSpawnList.Location = new System.Drawing.Point(0, 0);
			this.lstSpawnList.Name = "lstSpawnList";
			this.lstSpawnList.Size = new System.Drawing.Size(320, 432);
			this.lstSpawnList.TabIndex = 0;
			this.lstSpawnList.View = System.Windows.Forms.View.Details;
			// 
			// ListBoxPanel
			// 
			this.Controls.Add(this.lstSpawnList);
			this.Name = "ListBoxPanel";
			this.Size = new System.Drawing.Size(320, 432);
			this.ResumeLayout(false);
		}
		#endregion
	}
}
