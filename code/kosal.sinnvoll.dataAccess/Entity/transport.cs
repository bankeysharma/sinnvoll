using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Ports;

namespace kosal.sinnvoll.dataAccess.Entity {
    public class transport {
        public long idTransport { get; set; }
        public string transportName { get; set; }
        public string transportType { get; set; }
        public string httpApipUrl { get; set; }
        public string commPort { get; set; }
        public long baudRate { get; set; }
        public Parity parity { get; set; }
        public int dataBits { get; set; }
        public StopBits stopBits { get; set; }
        public Handshake handShake { get; set; }
        public int readTimeOut { get; set; }
        public int writeTimeOut { get; set; }
        public bool dtrEnable { get; set; }
        public bool rtsEnable { get; set; }
        public byte direction { get; set; }
        public bool isTestModem { get; set; }
        public bool isValid { get; set; }
        public bool isDeleted { get; set; }
        public DateTime createdOn { get; set; }
        public string createdBy { get; set; }
        public DateTime updatedOn { get; set; }
        public string updatedBy { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public transport() {
            this.readTimeOut = -1;
            this.writeTimeOut = -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idTransport"></param>
        /// <param name="transportName"></param>
        /// <param name="commPort"></param>
        /// <param name="baudRate"></param>
        /// <param name="parity"></param>
        /// <param name="dataBits"></param>
        /// <param name="stopBits"></param>
        /// <param name="handShake"></param>
        /// <param name="readTimeOut"></param>
        /// <param name="writeTimeOut"></param>
        /// <param name="dtrEnable"></param>
        /// <param name="rtsEnable"></param>
        /// <param name="direction"></param>
        /// <param name="isTestModem"></param>
        /// <param name="isValid"></param>
        /// <param name="isDeleted"></param>
        /// <param name="createdOn"></param>
        /// <param name="createdBy"></param>
        /// <param name="updatedOn"></param>
        /// <param name="updatedBy"></param>
        public transport(long idTransport, string transportName, string transportType, string commPort, long baudRate, int parity, int dataBits, int stopBits, int handShake,
                            int readTimeOut, int writeTimeOut, bool dtrEnable, bool rtsEnable, byte direction,
                            bool isTestModem, bool isValid, bool isDeleted, DateTime createdOn, string createdBy, DateTime updatedOn, string updatedBy) {

        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idTransport"></param>
        /// <param name="transportName"></param>
        /// <param name="transportType"></param>
        /// <param name="httpApiUrl"></param>
        /// <param name="direction"></param>
        /// <param name="isTestModem"></param>
        /// <param name="isValid"></param>
        /// <param name="isDeleted"></param>
        /// <param name="createdOn"></param>
        /// <param name="createdBy"></param>
        /// <param name="updatedOn"></param>
        /// <param name="updatedBy"></param>
        public transport(long idTransport, string transportName, string transportType, string httpApiUrl, byte direction,
                            bool isTestModem, bool isValid, bool isDeleted, DateTime createdOn, string createdBy, DateTime updatedOn, string updatedBy) {


        }
    }
}
