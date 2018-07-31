using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Drawing;
using System.Diagnostics;

using kosal.sinnvoll.comm.entity;
using kosal.sinnvoll.comm.consts;

using kosal.sinnvoll.dataAccess;
using kosal.sinnvoll.dataAccess.Entity;

namespace kosal.sinnvoll.comm.modem {
    /// <summary>
    /// 
    /// </summary>
    public class usbModem: IModem, IDisposable {

        #region Modem interface events 

        public event dlgModemGenericHandler initialised = null;
        public event dlgModemGenericHandler opened = null;
        public event dlgModemErrorHandler errored = null;
        public event dlgModemGenericHandler closed = null;
        public event dlgModemSmsHandler smsSent = null;
        public event dlgModemSignalHandler signalChanged = null;

        #endregion

        #region Member Variables 

        private StringBuilder modemResponseLocalBuffer = null;

        private ManualResetEvent mreModemOperation = null;
        private ManualResetEvent mreModemDataResponseReading = null;

        private object instSyncOperationLock = new object();

        private string newLineSequence = "\r\n";

        /// <summary>
        /// Timeout in milliSeconds
        /// </summary>
        private int modemOperationTimeOut = 0;

        private int modemResponseTimeout = 30; //* Seconds

        private bool disposed = false;

        private string OK = "OK";
        private string modemResponseBuffer = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        private SerialPort instPort = null;

        public long id { 
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string name {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public modemDirection direction {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public modemSettings settings {
            get; 
            set;
        }

        #endregion

        #region Initialisers 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settings"></param>
        public usbModem(transport instTransport) {
            modemSettings settings = modemSettings.parse(instTransport);
             
            this.name = instTransport.transportName;
            this.id = instTransport.idTransport;
            this.settings = settings;
            this.modemOperationTimeOut = (30 * 1000); //* Timeout in milliseconds
            this.modemResponseTimeout = 30; //* Seconds
            this.initialise();
            //this.instPort.ErrorReceived += new SerialErrorReceivedEventHandler(instPort_ErrorReceived);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settings"></param>
        private void initialise() {
            this.instPort = new SerialPort(settings.portName);
            this.instPort.BaudRate = (int) settings.baudRate;
            this.instPort.Parity = settings.parity;
            this.instPort.StopBits = settings.stopBits;
            this.instPort.DataBits = settings.dataBits;
            this.instPort.Handshake = settings.handshake;
            this.instPort.ReadTimeout = settings.readTimeOut;
            this.instPort.WriteTimeout = settings.writeTimeOut;
            this.instPort.DtrEnable = settings.dtrEnable;
            this.instPort.RtsEnable = settings.rtsEnable;
            this.instPort.NewLine = this.newLineSequence; //System.Environment.NewLine;
            //this.instPort.ReadBufferSize = this.instPort.ReadBufferSize * this.instPort.ReadBufferSize;

            if (this.initialised != null) {
                this.initialised(this, modemState.Initialised);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        ~usbModem() {
            this.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose() {
            Dispose(true);
            // This object will be cleaned up by the Dispose method. 
            // Therefore, you should call GC.SupressFinalize to 
            // take this object off the finalization queue 
            // and prevent finalization code for this object 
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios. 
        // If disposing equals true, the method has been called directly 
        // or indirectly by a user's code. Managed and unmanaged resources 
        // can be disposed. 
        // If disposing equals false, the method has been called by the 
        // runtime from inside the finalizer and you should not reference 
        // other objects. Only unmanaged resources can be disposed. 
        protected virtual void Dispose(bool disposing) {
            // Check to see if Dispose has already been called. 
            if(!this.disposed) {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources. 
                if(disposing && this.instPort != null) {
                    // Dispose managed resources.
                    
                    this.close();

                }

                // Note disposing has been done.
                disposed = true;

            }
        }

        #endregion

        #region Control Subroutines 

        /// <summary>
        /// 
        /// </summary>
        private void initialiseSyncObjectOfModemResponse() {
            lock(this.instSyncOperationLock) {
                this.mreModemOperation = new ManualResetEvent(false);
                this.modemResponseLocalBuffer = new StringBuilder();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void disposeSyncObjectOfModemResponse() {
            lock(this.instSyncOperationLock) {
                try {
                    if(this.mreModemOperation != null) {
                        this.mreModemOperation.Set();
                        this.mreModemOperation.Close();
                    }
                } catch { }
                this.mreModemOperation = null;
                this.modemResponseLocalBuffer = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void disposeSyncObjectOfDataReceiving() {
            try {
                if (this.mreModemDataResponseReading != null) this.mreModemDataResponseReading.Close();
            } catch { }

            this.mreModemDataResponseReading = null;

            try {
                this.thrdReadModemResponseWorker.Abort();
            } catch { }

            this.thrdReadModemResponseWorker = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clearLocalBuffer"></param>
        /// <returns></returns>
        private string fetchRespondedData(bool clearLocalBuffer) {
            if(this.modemResponseLocalBuffer == null) return string.Empty;
            string str;
            try {
                return(str = this.modemResponseLocalBuffer.ToString());
            } finally {
                if (clearLocalBuffer) {
                    this.modemResponseLocalBuffer = new StringBuilder();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void open() {
            lock(this) {
                if(this.instPort == null) {
                    this.initialise();
                }

                if(!this.instPort.IsOpen) {
                    this.initialiseSyncObjectOfModemResponse();

                    this.disposeSyncObjectOfDataReceiving();
                    this.mreModemDataResponseReading = new ManualResetEvent(false);

                    this.thrdReadModemResponseWorker = new Thread(new ThreadStart(this.readModemResponse));
                    this.thrdReadModemResponseWorker.Start();

                    try {
                        this.instPort.DataReceived += new SerialDataReceivedEventHandler(instPort_DataReceived);
                        this.instPort.ErrorReceived +=new SerialErrorReceivedEventHandler(instPort_ErrorReceived);
                        this.instPort.Open();
                        
                        Thread.Sleep(100);
                        this.executeATCommand(ATCommands.AT, false);

                        if(this.mreModemOperation.WaitOne(this.modemOperationTimeOut)) {
                            this.disposeSyncObjectOfModemResponse();
                            if(this.executeATCommandWithoutLock(ATCommands.ATCNMI_MessageReceptionMode, this.OK, out this.modemResponseBuffer)) {
                                if(this.executeATCommandWithoutLock(ATCommands.ATCMGF_TextMode, this.OK, out this.modemResponseBuffer)) {
                                    if (this.opened != null) {
                                        this.opened(this, modemState.Opened);
                                    }
                            
                                } else {
                                    if(this.modemResponseBuffer != string.Empty) {
                                        this.raiseErrorEvent(string.Format("Modem open attempt failed - {0}", this.modemResponseBuffer));
                                    }
                                    throw new Exception(string.Format("Unable to set text mode on transport '{0}' at '{1}'. Please unplug modem and replug again.", this.name, this.settings.portName));
                            
                                }

                            } else {
                                if (this.modemResponseBuffer != string.Empty) {
                                    this.raiseErrorEvent(string.Format("Modem open attempt failed - {0}", this.modemResponseBuffer));
                                }
                                throw new Exception(string.Format("Unable to set text mode on transport '{0}' at '{1}'. Please unplug modem and replug again.", this.name, this.settings.portName));
                            }

                            this.resetSignalCheckerTimer(true);

                        } else {
                            if (this.modemResponseBuffer != string.Empty) {
                                this.raiseErrorEvent(string.Format("Modem open attempt failed - {0}", this.modemResponseBuffer));
                            }
                            throw new Exception(string.Format("Unable to mount transport '{0}' at '{1}'. Please unplug modem and replug again.", this.name, this.settings.portName));

                        }

                    } catch (Exception ex) {
                        if(this.errored != null) {
                            this.errored(this, new modemErrorEventArgs(ex.Message, modemState.Errored, null, ex));
                        }

                        this.disposeSyncObjectOfModemResponse();
                        this.disposeSyncObjectOfDataReceiving();

                        this.instPort = null;

                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void close() {
            lock (this) {
                if (this.instPort != null && this.instPort.IsOpen) {
                    try {
                        this.instPort.Close();
                    } catch (Exception ex) { 
                        if(this.errored != null) {
                            this.errored(this, new modemErrorEventArgs(ex.Message, modemState.Errored, null, ex));
                        }
                    }

                    try {
                        this.resetSignalCheckerTimer(false);

                        this.disposeSyncObjectOfModemResponse();
                        this.disposeSyncObjectOfDataReceiving();

                    } catch (Exception ex) {
                        if (this.errored != null) {
                            this.errored(this, new modemErrorEventArgs(ex.Message, modemState.Errored, null, ex));
                        }
                    
                    }

                    if (this.closed != null) {
                        this.closed(this, modemState.Closed);
                    }
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void raiseErrorEvent(string message) {
            if(this.errored != null) {
                this.errored(this, new modemErrorEventArgs(message, modemState.Errored, null, null));
            
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentStrength"></param>
        public void raiseSignalChange(int currentStrength) {
            if(this.signalChanged != null) {

                Color signalColor = Color.Gray;

                if(currentStrength <= 9) {
                    signalColor = Color.Red;

                } else if(currentStrength <= 14) {
                    signalColor = Color.Yellow;

                } else if (currentStrength <= 19) {
                    signalColor = Color.Orange;

                } else if (currentStrength <= 31) {
                    signalColor = Color.ForestGreen;
                }

                this.signalChanged(this, new modemSingnalChangesEventArgs(31, currentStrength, signalColor));
            }
        }
        #endregion
        
        #region Modem signal handlers 

        private System.Threading.Timer instSignalCheckerTimer = null;

        private void resetSignalCheckerTimer(bool shouldCreateNew) {
            if (this.instSignalCheckerTimer != null) {
                try {
                    this.instSignalCheckerTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                    this.instSignalCheckerTimer.Dispose();
                } catch { }
            }

            if(shouldCreateNew) {
                this.instSignalCheckerTimer = new Timer(new TimerCallback(this.signalChecker_interval), null, 10 * 1000, 10 * 1000);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        private void signalChecker_interval(object sender) {
            this.resetSignalCheckerTimer(false);

            string strResponse = string.Empty;

            if(this.executeATCommandWithLock(ATCommands.ATCSQ_CheckSignals, this.OK, out this.modemResponseBuffer)) {
                this.resetSignalCheckerTimer(true);

            }

        }

        Thread thrdReadModemResponseWorker = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void instPort_DataReceived(object sender, SerialDataReceivedEventArgs e) {
            if(e.EventType == SerialData.Eof) {
                string s = string.Empty;

            }

            if (this.instPort != null && this.mreModemDataResponseReading != null) {
                this.mreModemDataResponseReading.Set();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private void readModemResponse() {
            while(true) {
                try {
                    if(this.mreModemDataResponseReading == null) break;
                 
                    this.mreModemDataResponseReading.WaitOne();
                    this.mreModemDataResponseReading.Reset();

                    string currentLine = string.Empty;

                    Stopwatch instSw = new Stopwatch();
                    instSw.Start();

                    while (this.instPort != null 
                            && this.modemResponseLocalBuffer != null 
                            && instSw.Elapsed.TotalSeconds <= this.modemResponseTimeout) {

                        if(this.instPort.BytesToRead != 0) {
                            this.modemResponseLocalBuffer.Append((currentLine = this.instPort.ReadExisting()));

                            if (currentLine == string.Empty 
                                    || currentLine.Contains(">")
                                    || currentLine.Contains("OK")
                                    || currentLine.Contains("ERROR")) {

                                break;

                            }

                        }
                        
                        System.Threading.Thread.Sleep(100);

                    }

                    instSw.Stop();
                    instSw = null;

                    if(this.modemResponseLocalBuffer != null && (currentLine = this.modemResponseLocalBuffer.ToString().Trim()) != string.Empty) {
                        new Thread(new ParameterizedThreadStart(this.interpretModemResponse)).Start(currentLine);
                    }

                } catch { 

                } finally {
                    if (this.mreModemOperation != null) this.mreModemOperation.Set();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        private void interpretModemResponse(object response) {
            if (response == null || response.ToString().Trim() == string.Empty) return;

            this.extractNetworkSignal(response.ToString());
        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void instPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e) {
            if (this.errored != null) {
                this.errored(this, new modemErrorEventArgs(string.Empty
                                                            , modemState.Errored
                                                            , e
                                                            , null));
            }
        }
        #endregion

        #region Modem interface operations 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="atCommand"></param>
        /// <param name="acquireLock"></param>
        /// <returns></returns>
        private void executeATCommand(string atCommand, bool acquireLock) {
            if(acquireLock) {
                this.executeATCommandWithLock(atCommand);

            } else {
                this.executeATCommandWithoutLock(atCommand);
            
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="atCommand"></param>
        private void executeATCommandWithLock(string atCommand) {
            lock(this) {
                this.executeATCommandWithoutLock(atCommand);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="atCommand"></param>
        /// <param name="successPattern"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        private bool executeATCommandWithLock(string atCommand, string successPattern, out string response) {
            lock(this) {
                return this.executeATCommandWithoutLock(atCommand, successPattern, out response);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="atCommand"></param>
        private void executeATCommandWithoutLock(string atCommand) {
            try {
                if (this.instPort == null || !this.instPort.IsOpen) return;

                this.instPort.WriteLine(atCommand);

                System.Windows.Forms.Application.DoEvents();

                Thread.Sleep(100);

            } catch {
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="atCommand"></param>
        /// <param name="successPattern"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        private bool executeATCommandWithoutLock(string atCommand, string successPattern, out string response) {
            response = string.Empty;

            try {
                this.initialiseSyncObjectOfModemResponse();

                this.executeATCommandWithoutLock(atCommand);

                if(this.mreModemOperation.WaitOne(this.modemOperationTimeOut)) {
                    response = this.fetchRespondedData(true);
                    this.disposeSyncObjectOfModemResponse();

                    return response.Contains(successPattern);

                }

            } catch {

            }

            return false;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modemResponse"></param>
        private void extractNetworkSignal(string modemResponse) {
            object value = null;
            string errorMessage = string.Empty;

            //int indxCSQ = 0;
            int indxCmd = 0;
            int indxDelimeter = 0;
            int intBuffer = 0;

            bool isErrorResponse = false;

            try {

                //if((indxRSSI = modemResponse.LastIndexOf("^RSSI")) != -1) {
                //    indxDelimeter = modemResponse.IndexOf(':', indxRSSI);
                //    value = Convert.ToInt32(modemResponse.Substring(indxDelimeter + 1, modemResponse.IndexOf('\r', indxDelimeter) - indxDelimeter - 1));
                //} 

                if((indxCmd = modemResponse.LastIndexOf("+CSQ")) != -1 /*&& indxCSQ > indxRSSI*/) {
                    if (!(isErrorResponse = modemResponse.Contains("ERROR"))) {
                        indxDelimeter = modemResponse.IndexOf(':', indxCmd);
                        value = modemResponse.Substring(indxDelimeter + 1, modemResponse.IndexOf(',', indxDelimeter) - indxDelimeter - 1);

                    } else {
                        if((indxCmd = modemResponse.LastIndexOf("+CME ERROR")) != -1) {
                            indxDelimeter   = modemResponse.IndexOf(':', indxCmd);
                            errorMessage    = modemResponse.Substring(indxDelimeter + 1, (((intBuffer = modemResponse.IndexOf('\n', indxDelimeter)) == -1) ? modemResponse.Length : intBuffer) - indxDelimeter - 1);

                            this.raiseErrorEvent(string.Format("Network signal issue: {0}", errorMessage.Trim()));

                        }

                    }

                    this.raiseSignalChange(Convert.ToInt32(value));

                }

            } catch {

            }

        }

        //private bool setModemTextSMSReady() {
        //    lock(this) {
        //        if(this.instPort != null) {
        //            //this.mreModemOperation.Set();

        //            //this.instPort.WriteLine("AT" + (char)(13));
        //            //Thread.Sleep(4);

        //            //this.instPort.WriteLine("AT+CMGF=1" + (char)(13));
        //            //Thread.Sleep(5);

                    
        //            this.instPort.WriteLine("AT+CMGF=1" + (char)(13));
        //            Thread.Sleep(5);

        //            //string strData = string.Empty;
        //            //if(this.mreModemOperation.WaitOne(this.modemOperationTimeOut)) {
        //            //    strData = this.instPort.ReadExisting();
        //            //}

        //            //if(strData == string.Empty) {
        //            //    return false;
        //            //}
        //        }
        //    }

        //    return false;
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sms"></param>
        public void sendSms(sms sms) {

            lock (this) {
                try {
                    //this.instPort.RtsEnable = false; //* Disabling SMS Receiving

                    this.resetSignalCheckerTimer(false);

                    if (this.instPort != null && this.instPort.IsOpen) {
                        string strData = string.Empty;

                        sms.status = smsStatus.FailedToSent;
                        this.modemResponseBuffer = this.instPort.ReadExisting();
                        if(this.executeATCommandWithoutLock(ATCommands.AT, this.OK, out this.modemResponseBuffer)) {
                            this.modemResponseBuffer = this.instPort.ReadExisting();
                            if(this.executeATCommandWithoutLock(ATCommands.ATCMGF_TextMode, this.OK, out this.modemResponseBuffer)) {
                                this.modemResponseBuffer = this.instPort.ReadExisting();
                                if (this.executeATCommandWithoutLock("AT+CMGS=\"" + sms.mobileNo + "\"" + (char) (13), ">", out this.modemResponseBuffer)) {
                                    if (this.executeATCommandWithoutLock((char) (8) + sms.message + (char)(26) + (char)(13), this.OK, out this.modemResponseBuffer)) {
                                        sms.status = smsStatus.Sent;
                                    }
                                }
                            }
                        }
                    } else {
                        sms.status = smsStatus.None;
                    }

                } finally {
                    if (this.smsSent != null) {
                        this.smsSent(this, sms);

                    }

                    this.disposeSyncObjectOfModemResponse();
                    this.resetSignalCheckerTimer(true);
                    //this.instPort.RtsEnable = true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sms"></param>
        //public void sendSms(sms sms) {

        //    lock(this) {
        //        try {
        //            this.resetSignalCheckerTimer(false);

        //            if(this.instPort != null && this.instPort.IsOpen) {
        //                string strData = string.Empty;

        //                this.instPort.WriteLine("AT" + (char)(13));
        //                Thread.Sleep(4);

        //                this.initialiseSyncObjectOfModemResponse();

        //                this.instPort.WriteLine("AT+CMGF=1" + (char)(13));
        //                Thread.Sleep(5);

        //                if (this.mreModemOperation == null || !this.mreModemOperation.WaitOne(this.modemOperationTimeOut)) {
        //                    sms.status = smsStatus.FailedToSent;

        //                } else {

        //                    string presentBuffer = this.instPort.ReadExisting();
        //                    string presentResponse = this.fetchRespondedData(true);

        //                    this.initialiseSyncObjectOfModemResponse();

        //                    this.instPort.WriteLine("AT+CMGS=\"" + sms.mobileNo + "\"");
        //                    Thread.Sleep(10);

        //                    this.instPort.WriteLine(sms.message + (char)(26) + (char)(13));
                    
        //                    //this.instPort.Write("AT+CMGS=\"" + sms.mobileNo + "\"\r" + sms.message + (char)(26) + (char)(13));

        //                    Thread.Sleep(10);
        //                    //this.executeATCommand(ATCommands.AT, false);

        //                    if (this.mreModemOperation != null && this.mreModemOperation.WaitOne(this.modemOperationTimeOut)) {
        //                        sms.status = smsStatus.Sent;
        //                    } else {
        //                        sms.status = smsStatus.FailedToSent;
        //                    }
        //                }
        //                if (this.smsSent != null) {
        //                    this.smsSent(this, sms);

        //                }
        //            }

        //        } finally {
        //            this.disposeSyncObjectOfModemResponse();
        //            this.resetSignalCheckerTimer(true);
        //        }
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deleteOnRead"></param>
        /// <returns></returns>
        public sms[] readAllSms(bool deleteOnRead) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public void deleteAllSms() {
            throw new NotImplementedException();
        }

        #endregion

    }
}