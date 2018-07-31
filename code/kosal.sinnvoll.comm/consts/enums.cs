using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kosal.sinnvoll.comm.consts {

    /// <summary>
    /// 
    /// </summary>
    public enum smsStatus: byte {
        None = 0,
        New,
        Sent,
        FailedToSent,
        FailedToRead,
        Deleted

    }

    /// <summary>
    /// 
    /// </summary>
    public enum modemState : byte {
        Closed = 0,
        Opened,
        Initialised,
        Working,
        Errored
    }

    /// <summary>
    /// 
    /// </summary>
    public enum modemDirection: byte {
        None = 0,
        Sent,
        Receive,
        Both
    }
}
