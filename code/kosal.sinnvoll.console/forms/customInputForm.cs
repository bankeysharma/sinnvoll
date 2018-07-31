using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace kosal.sinnvoll.console.forms {
    public partial class customInputForm : Form {
        public customInputForm() {
            InitializeComponent();

            this.lblHeaderText.Text = "Provide comma (,) delimited contact number(s) to send message.\r\n" +
                                        "Given message will be send to each delimited number.\r\n" +
                                        "e.g. 98xxx12xx1, 98xxx33xx6";
        }

        private void btnSubmit_Click(object sender, EventArgs e) {
            if(this.TextToSendTextBox.Text.Trim() == string.Empty) {
                MessageBox.Show("Please provide some text message to send.", "Info..", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.TextToSendTextBox.Focus();

            } else if (this.delimitedContactTextBox.Text.Replace(',', ' ').Trim() == string.Empty) {
                MessageBox.Show("Please provide valid contact number to send sms.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.delimitedContactTextBox.Focus();

            } else {
                library.publisherManager.mannualSubmission(this.delimitedContactTextBox.Text, this.TextToSendTextBox.Text);
                this.DialogResult = System.Windows.Forms.DialogResult.OK;

            }
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}
