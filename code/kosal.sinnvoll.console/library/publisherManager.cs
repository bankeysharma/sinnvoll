using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using kosal.sinnvoll.server;
using kosal.common.logger;

using kosal.sinnvoll.comm.modem;
using kosal.sinnvoll.comm.entity;

namespace kosal.sinnvoll.console.library {
    public static class publisherManager {
        private static object instLockOn = new object();

        private static publisher instPublisher = null;

        /// <summary>
        /// 
        /// </summary>
        public static bool isPublisherOn { 
            get { 
                lock (publisherManager.instLockOn) {
                    return publisherManager.instPublisher != null;
                }
            }
        }

        public static void mannualSubmission(string delimitedContects, string message) {
            string[] arrContects = delimitedContects.Split( new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            List<comm.entity.sms> smsToPublish = new List<comm.entity.sms>(arrContects.Length);

            for(int indx = 0; indx < arrContects.Length; indx++) {
                smsToPublish.Add(new comm.entity.sms(arrContects[ indx ].Trim(), message));
            }

            publisherManager.instPublisher.submit(smsToPublish);

        }

        /// <summary>
        /// 
        /// </summary>
        public static bool startPublisher() {
            lock(instLockOn) {
                try {
                    if(publisherManager.instPublisher == null) {
                        publisherManager.instPublisher = new publisher();
                        instPublisher.publishingSMS += new dlgPublishedSMS(instPublisher_publishingSMS);
                        instPublisher.shareStatus += new dlgPublisherStatus(instPublisher_shareStatus);
                        instPublisher.signalShanged += new dlgModemSignalHandler(instPublisher_signalChanged);

                        if(publisherManager.instPublisher.startPublisher()) {
                            return true;
                        } else {
                            publisherManager.instPublisher.stopPublisher();
                            publisherManager.instPublisher = null;
                        }
                    }
                } catch {
                    if(publisherManager.instPublisher != null) {
                        publisherManager.instPublisher.stopPublisher();
                    }

                    publisherManager.instPublisher = null;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modem"></param>
        /// <param name="e"></param>
        static void instPublisher_signalChanged(IModem modem, modemSingnalChangesEventArgs e) {
            parentFormManager.signalChanged(modem, e);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void stopPublisher() {
            lock (instLockOn) {
                try {
                    if (publisherManager.instPublisher != null) {
                        publisherManager.instPublisher.stopPublisher();
                        publisherManager.instPublisher = null;
                    }
                } catch {
                    publisherManager.instPublisher = null;
                }
            }
        } 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="publisher"></param>
        /// <param name="status"></param>
        /// <param name="level"></param>
        private static void instPublisher_shareStatus(publisher publisher, string status, logLevel level) {
            screenManager.appendMessage(status, level);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="publisher"></param>
        /// <param name="modem"></param>
        /// <param name="publishedSms"></param>
        private static void instPublisher_publishingSMS(publisher publisher, comm.modem.IModem modem, comm.entity.sms publishedSms) {
            if(publishedSms.status == comm.consts.smsStatus.Sent) {
                screenManager.appendMessage(string.Format("{0} ({1}) - sent message at {2}", modem.name, modem.settings.portName, publishedSms.mobileNo), logLevel.Information);

            } else {
                screenManager.appendMessage(string.Format("{0} ({1}) - failed to send message at {2}", modem.name, modem.settings.portName, publishedSms.mobileNo), logLevel.Information);
            
            }
        }
    }
}
