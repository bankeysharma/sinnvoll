using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Ports;

using kosal.common.logger;
using kosal.sinnvoll.comm.modem;
using kosal.sinnvoll.dataAccess;

namespace kosal.sinnvoll.server.transports {
    /// <summary>
    /// 
    /// </summary>
    public static class modemPool {
        private static List<IModem> modems = null;

        /// <summary>
        /// 
        /// </summary>
        static modemPool() {
            modems = new List<IModem>(1);

        }

        /// <summary>
        /// 
        /// </summary>
        public static int initialisedModem {
            get {
                return modemPool.modems.Count;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool prepareModelPool(dlgModemGenericHandler genericEventCallback
                                            , dlgModemSmsHandler smsEventCallback
                                            , dlgModemErrorHandler modemErrorReceived
                                            , dlgModemSignalHandler modemSignalChanged) {

            usbModem instUsbModem = null;

            try {
                instUsbModem = new usbModem(transportDAC.getTransport("USB Modem"));

                if(instUsbModem == null) {
                    if(modemErrorReceived != null) {
                        modemErrorReceived(null, new modemErrorEventArgs("Transport pool not ready, no transport found configured.", comm.consts.modemState.Errored, null, null));
                    }   

                    return false;

                }

                if(genericEventCallback != null) {
                    instUsbModem.initialised += genericEventCallback;
                    instUsbModem.opened += genericEventCallback;
                    instUsbModem.closed += genericEventCallback;

                }

                if(smsEventCallback != null) {
                    instUsbModem.smsSent += smsEventCallback;
                }

                if(modemErrorReceived != null) {
                    instUsbModem.errored += modemErrorReceived;
                }

                if(modemSignalChanged != null) {
                    instUsbModem.signalChanged += modemSignalChanged;
                }

                instUsbModem.open();

                modems.Add(instUsbModem);

                return true;

            } catch (Exception ex) {
                if(modemErrorReceived != null) {
                    modemErrorReceived(instUsbModem, new modemErrorEventArgs(string.Format("Unable to prepare transport pool - {0}", ex.Message), comm.consts.modemState.Errored, null, ex));
                }
            }

            return false;

        }

        /// <summary>
        /// 
        /// </summary>
        public static void closeAll() {
            for(int indx = 0; indx < modems.Count; indx++) {
                try {
                    modems[indx].close();
                } catch {  }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modem"></param>
        public static void close(IModem modem) {
            throw new NotImplementedException();        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IModem getModem() {
            return modemPool.getModem(comm.consts.modemDirection.Both);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IModem getModem(comm.consts.modemDirection direction) {
            return modems[ 0 ];
        }
    }
}