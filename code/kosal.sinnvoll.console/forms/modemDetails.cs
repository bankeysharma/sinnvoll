using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace kosal.sinnvoll.console.forms {
    public partial class modemDetails : UserControl {

        private int signalStrengthThreshold = 0;
        private int signalStrengthCurrent = 0;

        public modemDetails() {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="details"></param>
        /// <param name="signalStrength"></param>
        /// <param name="signalThreshold"></param>
        public void setDetails(string name, string details, int signalStrength, int signalThreshold) {
            this.setDetails(name, details, signalStrength, signalThreshold, null);
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="details"></param>
        /// <param name="signalStrength"></param>
        /// <param name="signalThreshold"></param>
        /// <param name="icon"></param>
        public void setDetails(string name, string details, int signalStrength, int signalThreshold, Image icon) {
            this.lblName.Text = name;
            this.lblDetails.Text = details;
            this.lblDetails.Visible = this.lblDetails.Text != string.Empty;

            this.pnlSignalIndicator.Dock = DockStyle.Left;
            this.signalStrengthThreshold = signalThreshold;
            this.signalStrengthCurrent = signalStrength;
            this.signalStrength = signalStrength;
            this.pictureBoxModemIcon.Visible = true;

            //if(icon != null) {
            //    this.pictureBoxModemIcon.Image = icon;
            //    this.pictureBoxModemIcon.Visible = true;

            //} else {
            //    this.pictureBoxModemIcon.Visible = false;

            //}
        }

        /// <summary>
        /// 
        /// </summary>
        public PictureBox pictureBox {
            get {
                return this.pictureBoxModemIcon;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int signalStrength {
            get {
                return this.signalStrengthCurrent;
            }
            set {
                this.pnlSignalIndicator.Width = (int) Math.Ceiling(((float) this.pnlSignalBase.Width) * (((float) value) / ((float) this.signalStrengthThreshold)));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Color signalColor {
            get {
                return this.pnlSignalIndicator.BackColor;
            }
            set {
                this.pnlSignalIndicator.BackColor = value;
            }
        }

        private void panelDetailArea_Paint(object sender, PaintEventArgs e) {

        }
    }
}
