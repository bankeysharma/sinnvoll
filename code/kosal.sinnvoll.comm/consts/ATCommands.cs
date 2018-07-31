using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kosal.sinnvoll.comm.consts {
    public static class ATCommands {
        
        /// <summary>
        /// 
        /// </summary>
        public static string AT {
            get {
                return "AT" + (char) (13);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string ATCMGF_TextMode {
            get {
                return "AT+CMGF=1" + (char)(13);
            
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string ATCMGS_SendSms { 
            get {
                return "AT+CMGS=\"{0}\"{1}" + (char)(26);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string ATCSQ_CheckSignals {
            get {
                return "AT+CSQ" + (char) 13;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string ATCNMI_MessageReceptionMode {
            get {
                return "AT+CNMI=2,1,0,0,0" + (char) 13;  
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mobileNo"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string getSendSmsATCmd(string mobileNo, string message) {
            return string.Format(ATCommands.ATCMGS_SendSms, mobileNo, message);
        }
    }
}
