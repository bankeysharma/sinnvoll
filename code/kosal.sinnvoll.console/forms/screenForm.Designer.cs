namespace kosal.sinnvoll.console.forms {
    partial class screenForm {
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.textBoxContent = new System.Windows.Forms.RichTextBox();
            this.groupBoxContent = new System.Windows.Forms.GroupBox();
            this.SuspendLayout();
            // 
            // textBoxContent
            // 
            this.textBoxContent.BackColor = System.Drawing.Color.Black;
            this.textBoxContent.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxContent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.textBoxContent.Location = new System.Drawing.Point(268, 153);
            this.textBoxContent.Name = "textBoxContent";
            this.textBoxContent.ReadOnly = true;
            this.textBoxContent.Size = new System.Drawing.Size(100, 96);
            this.textBoxContent.TabIndex = 0;
            this.textBoxContent.Text = "";
            // 
            // groupBoxContent
            // 
            this.groupBoxContent.Location = new System.Drawing.Point(397, 92);
            this.groupBoxContent.Name = "groupBoxContent";
            this.groupBoxContent.Size = new System.Drawing.Size(225, 209);
            this.groupBoxContent.TabIndex = 1;
            this.groupBoxContent.TabStop = false;
            // 
            // screenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(897, 459);
            this.Controls.Add(this.textBoxContent);
            this.Controls.Add(this.groupBoxContent);
            this.Name = "screenForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Screen";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.screenForm_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox textBoxContent;
        private System.Windows.Forms.GroupBox groupBoxContent;

    }
}