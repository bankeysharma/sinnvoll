using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

namespace kosal.sinnvoll.console.library {
    public static class transportManager {
        private static object instLockMe = new object();
        private static forms.transportViewer instTransport = null; 

        static transportManager() {
            //transportManager.instTransport = new forms.transport();
        }

        public static void showFormToManageTransport() {
            lock(transportManager.instLockMe) {
                if (transportManager.instTransport != null && !transportManager.instTransport.IsDisposed) return;
                if (transportManager.instTransport == null || transportManager.instTransport.IsDisposed) transportManager.instTransport = new forms.transportViewer();

                transportManager.instTransport.Show();

            }
        }
    }
}
