using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.IO.Ports;

using kosal.sinnvoll.comm;
using kosal.sinnvoll.dataAccess;

namespace kosal.sinnvoll.comm.modem {
    public class modemSettings {
        public string modemCode {get; set; }
        public string portName { get; set; }
        public long baudRate { get; set; }
        public Parity parity { get; set; }
        public int dataBits { get; set; }
        public StopBits stopBits { get; set; }
        public Handshake handshake { get; set; }
        public int readTimeOut { get; set; }
        public int writeTimeOut { get; set; }
        public bool dtrEnable { get; set; }
        public bool rtsEnable { get; set; }
        public consts.modemDirection modemDirection { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="portName"></param>
        /// <param name="baudRate"></param>
        /// <param name="parity"></param>
        /// <param name="dataBits"></param>
        /// <param name="stopBits"></param>
        /// <param name="handshake"></param>
        /// <param name="readTimeout"></param>
        /// <param name="writeTimeout"></param>
        /// <param name="readBufferSize"></param>
        /// <param name="writeBufferSize"></param>
        public modemSettings(string portName, long baudRate, Parity parity, int dataBits, StopBits stopBits, Handshake handshake
                            , int readTimeout, int writeTimeout, int readBufferSize, int writeBufferSize
                            , bool dtrEnable, bool rtsEnable
                            , comm.consts.modemDirection modemDirection) {
            this.portName = portName;
            this.baudRate = baudRate;
            this.parity = parity;
            this.stopBits = stopBits;
            this.dataBits = dataBits;
            this.handshake = handshake;
            this.readTimeOut = readTimeout;
            this.writeTimeOut = writeTimeout;
            this.dtrEnable = dtrEnable;
            this.rtsEnable = rtsEnable;
            this.modemDirection = (comm.consts.modemDirection) modemDirection;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static modemSettings getStandardUSBModemSetting() {
            SerialPort instSP = new SerialPort();
            modemSettings instUsbModemSettings = new modemSettings("COM3"
                                                                    , 9600 //instSP.BaudRate
                                                                    , Parity.None // instSP.Parity
                                                                    , 8 //instSP.DataBits
                                                                    , StopBits.One // instSP.StopBits
                                                                    , Handshake.RequestToSend // instSP.Handshake
                                                                    , instSP.ReadTimeout
                                                                    , instSP.WriteTimeout
                                                                    , instSP.ReadBufferSize
                                                                    , instSP.WriteBufferSize
                                                                    , true //instSP.RtsEnable
                                                                    , true //instSP.DtrEnable
                                                                    , consts.modemDirection.Both
                                                                    );
            return instUsbModemSettings;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instTransport"></param>
        /// <returns></returns>
        public static modemSettings parse(dataAccess.Entity.transport instTransport) {
            modemSettings instModemSettings = null;

            instModemSettings = new modemSettings(instTransport.commPort
                                                    , instTransport.baudRate
                                                    , instTransport.parity
                                                    , instTransport.dataBits
                                                    , instTransport.stopBits
                                                    , instTransport.handShake
                                                    , instTransport.readTimeOut
                                                    , instTransport.writeTimeOut
                                                    , instTransport.readTimeOut
                                                    , instTransport.writeTimeOut
                                                    , instTransport.dtrEnable
                                                    , instTransport.rtsEnable
                                                    , (consts.modemDirection) instTransport.direction);
        
            return instModemSettings;

        } 
    }
}