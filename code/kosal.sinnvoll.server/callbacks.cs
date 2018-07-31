using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using kosal.sinnvoll.comm.modem;
using kosal.sinnvoll.comm.entity;

namespace kosal.sinnvoll.server {

    public delegate void dlgPublishedSMS(publisher publisher, IModem modem, sms publishedSms);
    public delegate void dlgPublisherStatus(publisher publisher, string status, common.logger.logLevel level);

}
