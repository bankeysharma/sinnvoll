using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Collections;
using kosal.sinnvoll.dataAccess;
using kosal.sinnvoll.dataAccess.consts.enums;

namespace kosal.sinnvoll.console.forms {
    /// <summary>
    /// 
    /// </summary>
    public partial class transportSettings : Form {
        private operationPermission operationPerissions = operationPermission.readOnly;
        private dataAccess.Entity.transport instOldValues = null;

        /// <summary>
        /// 
        /// </summary>
        public transportSettings(): this(operationPermission.readOnly, string.Empty, string.Empty) {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationType"></param>
        public transportSettings(operationPermission operationType, string modemName, string headertext) {
            this.operationPerissions = operationType;

            InitializeComponent();
            this.initialiseInputControls();
            this.lblHeader.Text = headertext;
            this.instOldValues = this.loadTransportDetails(modemName);

        }

        /// <summary>
        /// 
        /// </summary>
        private void initialiseInputControls() {
            this.cmbTransportType.Items.Clear();
            this.cmbTransportType.Items.AddRange(new string[] { "USB Modem" });
            this.cmbTransportType.SelectedIndex = 0;

            this.cmbDirection.ValueMember = "Value";
            this.cmbDirection.DisplayMember = "Key";
            this.cmbDirection.DataSource = new KeyValuePair<string, int>[] {
                                                new KeyValuePair<string, int>("None", 0)
                                                , new KeyValuePair<string, int>("SendOnly", 1)
                                                , new KeyValuePair<string, int>("ReceiveOnly", 2)
                                                , new KeyValuePair<string, int>("Both", 3) };
            this.cmbDirection.SelectedIndex = 0;

            this.cmbDTREnabled.ValueMember = "Value";
            this.cmbDTREnabled.DisplayMember = "Key";
            this.cmbDTREnabled.DataSource = new KeyValuePair<string, int>[] {
                                                new KeyValuePair<string, int>("True", 1)
                                                , new KeyValuePair<string, int>("False", 0)
                                            };
            this.cmbDTREnabled.SelectedIndex = 0;

            this.cmbRTSEnabled.ValueMember = "Value";
            this.cmbRTSEnabled.DisplayMember = "Key";
            this.cmbRTSEnabled.DataSource = new KeyValuePair<string, int>[] {
                                                new KeyValuePair<string, int>("True", 1)
                                                , new KeyValuePair<string, int>("False", 0)
                                            };
            this.cmbRTSEnabled.SelectedIndex = 0;

            this.cmbParity.ValueMember = "Value";
            this.cmbParity.DisplayMember = "Key";
            this.cmbParity.DataSource = new KeyValuePair<string, int>[] {
                                                new KeyValuePair<string, int>("None", 0)
                                                , new KeyValuePair<string, int>("Odd", 1)
                                                , new KeyValuePair<string, int>("Even", 2)
                                                , new KeyValuePair<string, int>("Mark", 3)
                                                , new KeyValuePair<string, int>("Space", 4)
                                            };
            this.cmbParity.SelectedIndex = 0;

            this.cmbStopBits.ValueMember = "Value";
            this.cmbStopBits.DisplayMember = "Key";
            this.cmbStopBits.DataSource = new KeyValuePair<string, int>[] {
                                                new KeyValuePair<string, int>("None", 0)
                                                , new KeyValuePair<string, int>("One", 1)
                                                , new KeyValuePair<string, int>("Two", 2)
                                                , new KeyValuePair<string, int>("OnePointFive", 3)
                                            };
            this.cmbStopBits.SelectedIndex = 0;

            this.cmbHandShake.ValueMember = "Value";
            this.cmbHandShake.DisplayMember = "Key";
            this.cmbHandShake.DataSource = new KeyValuePair<string, int>[] {
                                                new KeyValuePair<string, int>("None", 0)
                                                , new KeyValuePair<string, int>("XOnXOff", 1)
                                                , new KeyValuePair<string, int>("RequestToSend", 2)
                                                , new KeyValuePair<string, int>("RequestToSendXOnXOff", 3)
                                            };
            this.cmbHandShake.SelectedIndex = 0;

            this.txtReadTimeout.Text = "-1";
            this.txtWriteTimeout.Text = "-1";
            this.txttransportCode.Text = string.Empty;
            this.txtBaudRate.Text = string.Empty;
            this.txtCommPort.Text = string.Empty;
            this.txtDataBits.Text = string.Empty;

            switch (this.operationPerissions) {
                case operationPermission.delete:
                case operationPermission.readOnly:
                    this.cmbTransportType.Enabled = false;
                    this.cmbDirection.Enabled = false;
                    this.cmbDTREnabled.Enabled = false;
                    this.cmbRTSEnabled.Enabled = false;
                    this.cmbParity.Enabled = false;
                    this.cmbStopBits.Enabled = false;
                    this.cmbHandShake.Enabled = false;
                    this.txtReadTimeout.Enabled = false;
                    this.txtWriteTimeout.Enabled = false;
                    this.txttransportCode.Enabled = false;
                    this.txtBaudRate.Enabled = false;
                    this.txtCommPort.Enabled = false;
                    this.txtDataBits.Enabled = false;
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private dataAccess.Entity.transport loadTransportDetails(string name) {
            dataAccess.Entity.transport instTransport = transportDAC.getTransport(name);

            if(instTransport != null) {
                this.txttransportCode.Text = instTransport.transportName;
                this.cmbTransportType.SelectedText = instTransport.transportType;
                this.txtBaudRate.Text = instTransport.baudRate.ToString();
                this.txtCommPort.Text = instTransport.commPort;
                this.txtDataBits.Text = instTransport.dataBits.ToString();
                this.txtReadTimeout.Text = instTransport.readTimeOut.ToString();
                this.txtWriteTimeout.Text = instTransport.writeTimeOut.ToString();

                this.cmbParity.SelectedValue = (int)instTransport.parity;
                this.cmbDirection.SelectedValue = (int)instTransport.direction;
                this.cmbDTREnabled.SelectedValue = instTransport.dtrEnable ? 1 : 0;
                this.cmbHandShake.SelectedValue = (int) instTransport.handShake;
                this.cmbRTSEnabled.SelectedValue = instTransport.rtsEnable ? 1 : 0;
                this.cmbStopBits.SelectedValue = (int) instTransport.stopBits;

            } else {
                this.initialiseInputControls();
            }

            return instTransport;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e) {
            if(this.instOldValues != null) {
                switch(this.operationPerissions) {
                    case operationPermission.update:
                        transportDAC.updateTransport(this.instOldValues.idTransport
                                                        , this.txttransportCode.Text
                                                        , this.cmbTransportType.Text
                                                        , string.Empty
                                                        , this.txtCommPort.Text
                                                        , Convert.ToInt64(this.txtBaudRate.Text)
                                                        , (System.IO.Ports.Parity) Convert.ToInt32(this.cmbParity.SelectedValue)
                                                        , Convert.ToInt32(this.txtDataBits.Text)
                                                        , (System.IO.Ports.StopBits) Convert.ToInt32(this.cmbStopBits.SelectedValue)
                                                        , (System.IO.Ports.Handshake) Convert.ToInt32(this.cmbHandShake.SelectedValue)
                                                        , Convert.ToInt32(this.txtReadTimeout.Text)
                                                        , Convert.ToInt32(this.txtWriteTimeout.Text)
                                                        , Convert.ToBoolean(this.cmbDTREnabled.SelectedValue)
                                                        , Convert.ToBoolean(this.cmbRTSEnabled.SelectedValue)
                                                        , Convert.ToByte(this.cmbDirection.SelectedValue)
                                                        , false // Not test modem
                                                        , true // is valid
                                                        , System.Environment.UserName);

                            this.DialogResult = System.Windows.Forms.DialogResult.OK;
                            break;
                        case operationPermission.delete:
                            transportDAC.deleteTransport(this.instOldValues.idTransport
                                                            , Environment.UserName);
                            this.DialogResult = System.Windows.Forms.DialogResult.OK;
                            break;

                }

            } else {
                MessageBox.Show("Failed to execute operation on database, please try again.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e) {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}
