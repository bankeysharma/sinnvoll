using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using kosal.common.logger;
using kosal.sinnvoll.comm.modem;
using kosal.sinnvoll.comm.entity;

namespace kosal.sinnvoll.console.library {
    public static class parentFormManager {
        private static forms.parentForm instParentForm = null;

        static parentFormManager() {
            parentFormManager.instParentForm = new forms.parentForm();

        }

        public static forms.parentForm parentFormSinnvoll {
            get {
                return parentFormManager.instParentForm;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public static void updateScreenMenuStatus(bool value) {
            parentFormManager.instParentForm.updateScreenMenuCheckStatus(value);
        }


        /// <summary>
        /// 
        /// </summary>
        public static void showManualSubmissionForm() {
            forms.customInputForm instManualSubmissionForm = new forms.customInputForm();

            instManualSubmissionForm.ShowDialog();
            
            instManualSubmissionForm.Dispose();

        }

        public static void signalChanged(IModem modem, modemSingnalChangesEventArgs e) {
            lock (instParentForm) {
                instParentForm.signalChanged(modem, e);
            }
        }
    }
}
