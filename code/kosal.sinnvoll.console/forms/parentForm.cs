using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using kosal.sinnvoll.comm.entity;
using kosal.sinnvoll.comm.modem;

namespace kosal.sinnvoll.console.forms {
    public partial class parentForm : Form {
        private int childFormNumber = 0;

        public parentForm() {
            InitializeComponent();

        }

        private void ShowNewForm(object sender, EventArgs e) {
            //Form childForm = new Form();
            //childForm.MdiParent = this;
            //childForm.Text = "Window " + childFormNumber++;
            //childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e) {
            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            //openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            //if (openFileDialog.ShowDialog(this) == DialogResult.OK) {
            //    string FileName = openFileDialog.FileName;
            //}
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e) {
            //SaveFileDialog saveFileDialog = new SaveFileDialog();
            //saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            //saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            //if (saveFileDialog.ShowDialog(this) == DialogResult.OK) {
            //    string FileName = saveFileDialog.FileName;
            //}
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e) {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e) {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e) {
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e) {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e) {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e) {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e) {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e) {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e) {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e) {
            foreach (Form childForm in MdiChildren) {
                childForm.Close();
            }
        }

        private void parentForm_Shown(object sender, EventArgs e) {
            library.screenManager.showScreen();
        }

        private void screenToolStripMenuItem_Click(object sender, EventArgs e) {
            if (screenToolStripMenuItem.Checked) {
                library.screenManager.showScreen();
            } else {
                library.screenManager.hideScreen();
            }
        }

        public void updateScreenMenuCheckStatus(bool value) {
            this.screenToolStripMenuItem.Checked = value;
        
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e) {
            if(library.publisherManager.startPublisher()) {
                startToolStripMenuItem.Enabled = false;

                stopToolStripMenuItem.Enabled = true;
                publishSMSToolStripMenuItem.Enabled = true;
            }
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e) {
            library.publisherManager.stopPublisher();
            stopToolStripMenuItem.Enabled = false;
            publishSMSToolStripMenuItem.Enabled = false;

            startToolStripMenuItem.Enabled = true;
        }

        private void ignoreNosToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        private void parentForm_Load(object sender, EventArgs e) {

        }

        private void windowsMenu_Click(object sender, EventArgs e) {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void parentForm_FormClosing(object sender, FormClosingEventArgs e) {
            if(library.publisherManager.isPublisherOn) {
                if(MessageBox.Show("SMS Publisher is ON, please stop publisher before closing application.\r\n" +
                                    "Exiting application without stopping publisher may cause pending sms not to publish.\r\n\r\n" +
                                    "Do you want to continue, anyways?"
                                    , "Warning"
                                    , MessageBoxButtons.YesNo
                                    , MessageBoxIcon.Warning
                                    , MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes) {

                    library.publisherManager.stopPublisher();

                } else {
                    e.Cancel = true;
                }
            }
        }

        private void fromManualInputToolStripMenuItem_Click(object sender, EventArgs e) {
            library.parentFormManager.showManualSubmissionForm();
        }


        private Dictionary<string, modemDetails> modemDetailControl = null;
        public void signalChanged(IModem modem, modemSingnalChangesEventArgs e) {
            if(this.InvokeRequired) {
                this.Invoke(new dlgModemSignalHandler(this.signalChanged), new object[] { modem, e });
            
            } else {
                if (this.modemDetailControl == null) {
                    this.modemDetailControl = new Dictionary<string, modemDetails>();

                    modemDetails instModemDetailControl = new modemDetails();

                    instModemDetailControl.setDetails(modem.name, modem.settings.portName, e.changedStrength, e.maxStrength);
                    instModemDetailControl.Name = string.Format("modemDetail_{0}", modem.id);
                    instModemDetailControl.signalColor = e.signalColor;

                    this.panelStatus.Controls.Add(instModemDetailControl);
                    instModemDetailControl.Dock = DockStyle.Left;
                    instModemDetailControl.BringToFront();

                    this.modemDetailControl.Add(modem.id.ToString(), instModemDetailControl);

                } else {
                    modemDetails instModemDetailControl = null;
                    
                    this.modemDetailControl.TryGetValue(modem.id.ToString(), out instModemDetailControl);

                    //instModemDetailControl.setDetails(modem.name, modem.settings.portName, e.changedStrength, e.maxStrength);
                    //instModemDetailControl.Name = string.Format("modemDetail_{0}", modem.id);
                    if(instModemDetailControl != null) {
                        instModemDetailControl.signalStrength = e.changedStrength;
                        instModemDetailControl.signalColor = e.signalColor;
                    }
                    //this.panelStatus.Controls.Add(instModemDetailControl);
                    //instModemDetailControl.Dock = DockStyle.Left;
                
                }
            }
        }

        private void manageToolStripMenuItem1_Click(object sender, EventArgs e) {
            library.transportManager.showFormToManageTransport();

        }
    }
}
