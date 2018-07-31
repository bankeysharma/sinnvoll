using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.IO.Ports;
using System.Threading;

using kosal.sinnvoll.comm.entity;
using kosal.sinnvoll.comm.consts;

using kosal.common.logger;
using kosal.sinnvoll.server;
using kosal.sinnvoll.comm.modem;

namespace kosal.sinnvoll.console.forms {
    public partial class screenForm : Form {

        /// <summary>
        /// 
        /// </summary>
        public screenForm() {
            InitializeComponent();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void screenForm_FormClosed(object sender, FormClosedEventArgs e) {
            library.parentFormManager.updateScreenMenuStatus(false);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modem"></param>
        /// <param name="state"></param>
        public void addModemDetails(IModem modem, modemState state) {
        
        }

        /// <summary>
        /// 
        /// </summary>
        public void arrangeScreen() {
            this.groupBoxContent.Visible = false;
            this.textBoxContent.Dock = DockStyle.Fill;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        public void appendMessage(publisher publisher, string message, logLevel level) {
            if(!this.textBoxContent.InvokeRequired) {
                this.textBoxContent.AppendText(string.Format("{0}: {1}\r\n", DateTime.Now.ToString("MMM dd HH.mm.ss"), message));
            } else {
                this.textBoxContent.Invoke(new dlgPublisherStatus(this.appendMessage), new object[] { publisher, message, level });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modem"></param>
        /// <param name="e"></param>
        public void signalChanged(IModem modem, modemSingnalChangesEventArgs e) {
            lock(this) {
                (this.MdiParent as parentForm).signalChanged(modem, e);
            }
        }
    }
}