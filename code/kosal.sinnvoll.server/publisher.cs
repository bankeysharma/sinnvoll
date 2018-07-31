using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.IO.Ports;

using kosal.sinnvoll.comm.modem;
using kosal.sinnvoll.comm.entity;

namespace kosal.sinnvoll.server {
    public class publisher {
        private List<comm.entity.sms>   smsBench = null;
        private Thread                  workerThead = null;
        private ManualResetEvent        instMre = null;

        public event dlgPublishedSMS        publishingSMS;
        public event dlgPublisherStatus     shareStatus;
        public event dlgModemSignalHandler  signalShanged;

        public publisher() {

        }

        /// <summary>
        /// 
        /// </summary>
        public bool startPublisher() {
            this.doShareStatus("Initialising Sinnvoll publisher...", common.logger.logLevel.Information);

            try {
                //* Preparing modem pool
                if(transports.modemPool.prepareModelPool(new dlgModemGenericHandler(this.modemGenericEventHandler)
                                                        , new dlgModemSmsHandler(this.modemSmsEventHandler)
                                                        , new dlgModemErrorHandler(this.modemErrorReceived)
                                                        , new dlgModemSignalHandler(this.modemSignalChanged))) {

                    this.smsBench = new List<comm.entity.sms>();
                    this.workerThead = new Thread(new ThreadStart(this.publishFromBench));
                    this.instMre = new ManualResetEvent(true);
                    this.workerThead.Start();

                    this.doShareStatus("Sinnvoll publisher initialised", common.logger.logLevel.Information);

                    return true;

                } else {
                    return false;

                }

            } catch {
                this.doShareStatus("Sinnvoll publisher errored and not initialised", common.logger.logLevel.Information);
            
            }

            return false;
        
        }

        /// <summary>
        /// 
        /// </summary>
        public void stopPublisher() {
            this.doShareStatus("Stopping Sinnvoll publisher...", common.logger.logLevel.Information);

            if(this.smsBench != null) this.smsBench.Clear();
            this.smsBench = null;

            try {
                this.workerThead.Abort();

            } catch {
            
            } finally {
                this.workerThead = null;

            }

            if(this.instMre != null) this.instMre.Close();
            this.instMre = null;

            this.doShareStatus("Unmounting all transport agents (modem)...", common.logger.logLevel.Information);

            transports.modemPool.closeAll();

            this.doShareStatus("Sinnvoll publisher stopped!", common.logger.logLevel.Information);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="smsToSend"></param>
        public void submit(List<comm.entity.sms> smsToSend) {
            lock(this.smsBench) {
                this.smsBench.AddRange(smsToSend);
                this.instMre.Set();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void publishFromBench() {
            this.doShareStatus("Starting publishing...", common.logger.logLevel.Information); 

            sms[] localSetOfSMSToSend = null;

            while(true) {
                if(!this.instMre.WaitOne(new TimeSpan(0,0,5))) {
                    continue;
                }
                
                //* Collecting set of sms to publish
                lock (this.smsBench) {
                    if(this.smsBench.Count == 0) {
                        this.doShareStatus("SMS bench found empty", common.logger.logLevel.Information);
                        this.instMre.Reset();
                        this.doShareStatus("SMS publishing halted!", common.logger.logLevel.Information);
                        localSetOfSMSToSend = null;
                        continue;

                    } else {
                        if(localSetOfSMSToSend == null) {
                            this.doShareStatus("SMS publishing started...", common.logger.logLevel.Information);
                        }

                        int presentBatchSize = this.smsBench.Count >= 10 ? 10 : this.smsBench.Count;
                        localSetOfSMSToSend = new sms[ presentBatchSize ];
                        this.smsBench.CopyTo(0, localSetOfSMSToSend, 0, presentBatchSize);
                        this.smsBench.RemoveRange(0, presentBatchSize);

                    }
                }

                if(localSetOfSMSToSend != null) {
                    IModem instModem = null;

                    for(int indx = 0; indx < localSetOfSMSToSend.Length; indx++) {
                        instModem = transports.modemPool.getModem();

                        instModem.sendSms(localSetOfSMSToSend[ indx ]);

                        //this.doPublishedSMS(instModem, localSetOfSMSToSend[ indx ]);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modem"></param>
        /// <param name="state"></param>
        private void modemGenericEventHandler(IModem modem, comm.consts.modemState state) {
            //if (this.shareStatus != null) {
            //    this.shareStatus(this, string.Format("{0} - {1}", modem.name, state.ToString()), common.logger.logLevel.Information);
            //}
            this.doShareStatus(string.Format("{0} - {1}", modem.name, state.ToString()), common.logger.logLevel.Information);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modem"></param>
        /// <param name="state"></param>
        private void modemSmsEventHandler(IModem modem, sms smsEntity) {
            this.doPublishedSMS(modem, smsEntity);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void modemErrorReceived(IModem modem, modemErrorEventArgs e) {
            if(modem != null) {
                this.doShareStatus(string.Format("{0} - {1}: {2}", modem.name, e.modemState.ToString(), e.errorMessage)
                                    , common.logger.logLevel.Information);
            } else {
                this.doShareStatus(string.Format("{0} - {1}: {2}", "#Not Configured#", e.modemState.ToString(), e.errorMessage)
                                    , common.logger.logLevel.Information);
                                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modem"></param>
        /// <param name="e"></param>
        private void modemSignalChanged(IModem modem, modemSingnalChangesEventArgs e) {
            if(this.signalShanged != null) {
                this.signalShanged(modem, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modem"></param>
        /// <param name="status"></param>
        /// <param name="publishedSms"></param>
        private void doShareStatus(string status, common.logger.logLevel level) {
            if(this.shareStatus != null) {
                this.shareStatus(this, status, level);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modem"></param>
        /// <param name="smsPublished"></param>
        private void doPublishedSMS(IModem modem, sms smsPublished) {
            if(this.publishingSMS != null) {
                this.publishingSMS(this, modem, smsPublished);
            }
        }
    }
}