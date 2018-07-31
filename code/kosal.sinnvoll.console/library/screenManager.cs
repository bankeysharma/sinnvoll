using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using kosal.common.logger;
using kosal.sinnvoll.comm.modem;
using kosal.sinnvoll.comm.entity;

namespace kosal.sinnvoll.console.library {
    public static class screenManager {
        private static forms.screenForm instScreen = null;

        public static void showScreen() {
            screenManager.instScreen = new forms.screenForm();
            screenManager.instScreen.MdiParent = parentFormManager.parentFormSinnvoll;
            screenManager.instScreen.arrangeScreen();

            screenManager.instScreen.WindowState = FormWindowState.Maximized;
            screenManager.instScreen.Show();
            
        }

        public static void hideScreen() {
            screenManager.instScreen.Close();
            screenManager.instScreen = null;
        }

        public static void appendMessage(string message, logLevel level) {
            screenManager.instScreen.appendMessage(null, message, level);
        }

    }
}
