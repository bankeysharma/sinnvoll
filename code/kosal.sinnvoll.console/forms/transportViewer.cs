using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using kosal.sinnvoll.dataAccess;
using kosal.sinnvoll.dataAccess.Entity;

namespace kosal.sinnvoll.console.forms {
    public partial class transportViewer : Form {
        public transportViewer() {
            InitializeComponent();

            this.loadTransportDataGrid();

            this.dataGridViewTransport.Dock = DockStyle.Fill;
        }

        public void loadTransportDataGrid() {
            List<dataAccess.Entity.transport> listTransports = null;

            listTransports = dataAccess.transportDAC.getTransport();

            this.dataGridViewTransport.DataSource = listTransports; 
            
        }

        private void dataGridViewTransport_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e) {
            this.loadTransportOperationForm(dataAccess.consts.enums.operationPermission.update, this.dataGridViewTransport.SelectedRows[ 0 ]);

        }

        private void loadTransportOperationForm(dataAccess.consts.enums.operationPermission operationPermission, DataGridViewRow dataRow) {
            string modemName = string.Empty;

            try {
                if(dataRow != null) {
                    modemName = dataRow.Cells[ 1 ].Value.ToString();

                    if(modemName == string.Empty) {
                        MessageBox.Show("Can not open for editing.");
                        return;
                    }

                }

                transportSettings instOpForm = new transportSettings(operationPermission, modemName, string.Empty);
                if (instOpForm.ShowDialog() == DialogResult.OK) {
                    this.loadTransportDataGrid();
                }

            } catch (Exception ex) {
                MessageBox.Show(string.Format("Unable to open:\r\n{0}", ex.Message));

            }
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void btnInsert_Click(object sender, EventArgs e) {
            this.loadTransportOperationForm(dataAccess.consts.enums.operationPermission.insert, null);
        }

        private void btnSettings_Click(object sender, EventArgs e) {
            switch(this.dataGridViewTransport.SelectedRows.Count) {
                case 0:
                    MessageBox.Show("Please select transport in above grid to modify settings.\r\nYou should select by clicking row header.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 1:
                    this.loadTransportOperationForm(dataAccess.consts.enums.operationPermission.update, this.dataGridViewTransport.SelectedRows[ 0 ]);
                    break;
                default:
                    MessageBox.Show("Please select only one transport in above grid to modify settings.\r\nYou should select by clicking row header.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                    
            }
        }

        private void btnDelete_Click(object sender, EventArgs e) {
            switch (this.dataGridViewTransport.SelectedRows.Count) {
                case 0:
                    MessageBox.Show("Please select transport in above grid to delete.\r\nYou should select by clicking row header.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 1:
                    string modemName = string.Empty;
                    modemName = this.dataGridViewTransport.SelectedRows[ 0 ].Cells[ 1 ].Value.ToString();

                    transportSettings instTS = new transportSettings(dataAccess.consts.enums.operationPermission.delete
                                                                     , modemName
                                                                     , string.Format("You are going to delete '{0}' transport.\r\nDo you want to proceed with your action?", modemName));
                    
                    if(instTS.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                        this.loadTransportDataGrid();
                    
                    }

                    break;

                default:
                    MessageBox.Show("Please select only one transport in above grid to delete.\r\nYou should select by clicking row header.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

            }
        }
    }
}
