using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using kosal.sinnvoll.comm.consts;

namespace kosal.sinnvoll.comm.entity {
    public class sms {
        public string mobileNo { get; set; }
        public string message { get; set; }
        public smsStatus status { get; set; }

        public sms(string mobileNo, string message) {
            this.mobileNo = mobileNo;
            this.message = message;
            this.status = smsStatus.New;
        }
    }
}
