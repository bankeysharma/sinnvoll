using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Ports;

using kosal.sinnvoll.comm.entity;
using kosal.sinnvoll.comm.consts;

namespace kosal.sinnvoll.comm.modem {
    public interface IModem {

        event dlgModemGenericHandler initialised;
        event dlgModemGenericHandler opened;
        event dlgModemErrorHandler errored;
        event dlgModemGenericHandler closed;
        event dlgModemSmsHandler smsSent;

        long id { get; set; }
        string name { get; set; }

        modemDirection direction { get; set; }

        modem.modemSettings settings { get; set; }

        void open();

        void close();

        void sendSms(sms sms);

        sms[] readAllSms(bool deleteOnRead);

        void deleteAllSms();

    }
}
