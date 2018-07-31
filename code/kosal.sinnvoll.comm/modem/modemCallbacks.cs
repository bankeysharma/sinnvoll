using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

using System.IO;
using System.IO.Ports;
using System.Threading;

using kosal.sinnvoll.comm.entity;
using kosal.sinnvoll.comm.consts;

namespace kosal.sinnvoll.comm.modem {
    public delegate void dlgModemGenericHandler(IModem modem, modemState state);
    public delegate void dlgModemErrorHandler(IModem modem, modemErrorEventArgs e);
    public delegate void dlgModemSmsHandler(IModem modem, sms smsEntity);
    public delegate void dlgModemSignalHandler(IModem modem, modemSingnalChangesEventArgs e);

    /// <summary>
    /// 
    /// </summary>
    public class modemSingnalChangesEventArgs: EventArgs {
        public int maxStrength = 31;
        public int changedStrength = 0;
        public Color signalColor = Color.Gray;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxStrength"></param>
        /// <param name="changedStrength"></param>
        /// <param name="signalColor"></param>
        public modemSingnalChangesEventArgs(int maxStrength, int changedStrength, Color signalColor) {
            this.maxStrength = maxStrength;
            this.changedStrength = changedStrength;
            this.signalColor = signalColor;   
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class modemErrorEventArgs: EventArgs {
        public string errorMessage  = string.Empty;
        public modemState modemState = modemState.Closed;
        public SerialErrorReceivedEventArgs serialError = null;
        public Exception appEx = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="modemState"></param>
        /// <param name="serialError"></param>
        /// <param name="ex"></param>
        public modemErrorEventArgs(string errorMessage, modemState modemState, SerialErrorReceivedEventArgs serialError, Exception ex) {
            this.modemState = modemState;
            this.errorMessage = errorMessage;
            this.serialError = serialError;
            this.appEx = ex;
        }
    }
}
