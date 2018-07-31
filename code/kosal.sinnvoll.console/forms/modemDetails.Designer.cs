namespace kosal.sinnvoll.console.forms {
    partial class modemDetails {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(modemDetails));
            this.pictureBoxModemIcon = new System.Windows.Forms.PictureBox();
            this.panelDetailArea = new System.Windows.Forms.Panel();
            this.pnlSignalBase = new System.Windows.Forms.Panel();
            this.pnlSignalIndicator = new System.Windows.Forms.Panel();
            this.lblDetails = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxModemIcon)).BeginInit();
            this.panelDetailArea.SuspendLayout();
            this.pnlSignalBase.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBoxModemIcon
            // 
            this.pictureBoxModemIcon.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBoxModemIcon.BackgroundImage")));
            this.pictureBoxModemIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBoxModemIcon.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBoxModemIcon.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxModemIcon.Name = "pictureBoxModemIcon";
            this.pictureBoxModemIcon.Size = new System.Drawing.Size(60, 53);
            this.pictureBoxModemIcon.TabIndex = 0;
            this.pictureBoxModemIcon.TabStop = false;
            // 
            // panelDetailArea
            // 
            this.panelDetailArea.Controls.Add(this.pnlSignalBase);
            this.panelDetailArea.Controls.Add(this.lblDetails);
            this.panelDetailArea.Controls.Add(this.lblName);
            this.panelDetailArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetailArea.Location = new System.Drawing.Point(60, 0);
            this.panelDetailArea.Name = "panelDetailArea";
            this.panelDetailArea.Size = new System.Drawing.Size(126, 53);
            this.panelDetailArea.TabIndex = 1;
            this.panelDetailArea.Paint += new System.Windows.Forms.PaintEventHandler(this.panelDetailArea_Paint);
            // 
            // pnlSignalBase
            // 
            this.pnlSignalBase.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pnlSignalBase.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlSignalBase.Controls.Add(this.pnlSignalIndicator);
            this.pnlSignalBase.Location = new System.Drawing.Point(8, 36);
            this.pnlSignalBase.Name = "pnlSignalBase";
            this.pnlSignalBase.Size = new System.Drawing.Size(114, 14);
            this.pnlSignalBase.TabIndex = 3;
            // 
            // pnlSignalIndicator
            // 
            this.pnlSignalIndicator.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pnlSignalIndicator.Location = new System.Drawing.Point(12, 3);
            this.pnlSignalIndicator.Name = "pnlSignalIndicator";
            this.pnlSignalIndicator.Size = new System.Drawing.Size(35, 16);
            this.pnlSignalIndicator.TabIndex = 0;
            // 
            // lblDetails
            // 
            this.lblDetails.AutoSize = true;
            this.lblDetails.Location = new System.Drawing.Point(7, 20);
            this.lblDetails.Name = "lblDetails";
            this.lblDetails.Size = new System.Drawing.Size(35, 13);
            this.lblDetails.TabIndex = 1;
            this.lblDetails.Text = "label1";
            this.lblDetails.Visible = false;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(7, 3);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(41, 13);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "label1";
            // 
            // modemDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.panelDetailArea);
            this.Controls.Add(this.pictureBoxModemIcon);
            this.Name = "modemDetails";
            this.Size = new System.Drawing.Size(186, 53);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxModemIcon)).EndInit();
            this.panelDetailArea.ResumeLayout(false);
            this.panelDetailArea.PerformLayout();
            this.pnlSignalBase.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxModemIcon;
        private System.Windows.Forms.Panel panelDetailArea;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblDetails;
        private System.Windows.Forms.Panel pnlSignalBase;
        private System.Windows.Forms.Panel pnlSignalIndicator;
    }
}
