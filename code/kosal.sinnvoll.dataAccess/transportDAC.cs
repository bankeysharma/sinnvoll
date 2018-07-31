using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.Common;
using System.Data.OleDb;

using System.IO;
using System.IO.Ports;

namespace kosal.sinnvoll.dataAccess {
    public static class transportDAC {

        /// <summary>
        /// 
        /// </summary>
        private static string sqlAllTransports {
            get {
                return "SELECT [idTransport], [transportName], [transportType], [httpApiUrl], [commPort], [baudRate], [parity], [dataBits], [stopBits], [handShake], [readTimeOut], [writeTimeOut]" +
                        ", [dtrEnable], [rtsEnable], [direction], [isTestModem], [isValid], [createdOn], [createdBy], [updatedOn], [updatedBy] " +
                        " FROM Transports " +
                        " WHERE [isDeleted] = 0 " + 
                        "{0};";
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private static string sqlInsertTransport {
            get {
                return "INSERT INTO Transports ([transportName], [transportType], [httpApiUrl], [commPort], [baudRate], [parity], [dataBits], [stopBits], [handShake], [readTimeOut], [writeTimeOut]" +
                        ", [dtrEnable], [rtsEnable], [direction], [isTestModem], [isValid], [createdBy], [updatedBy]) " +
                        "  ('{0}', '{1}', '{2}', '{3}', {4}, {5}, {6}, {7}, {8}, {9}, {10}" +
                          " , {11}, {12}, {13}, {14}, {15}, {16}, {17});";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static string sqlUpdateTransport {
            get {
                return "UPDATE Transports " +
                            " SET [transportName] = '{0}', [transportType] = '{1}', [httpApiUrl] = '{2}', [commPort] = '{3}', [baudRate] = {4}, [parity] = {5}, " +
                            " [dataBits] = {6}, [stopBits] = {7}, [handShake] = {8}, [readTimeOut] = {9}, [writeTimeOut] = {10}, " +
                            " [dtrEnable] = {11}, [rtsEnable] = {12}, [direction] = {13}, [isTestModem] = {14}, [isValid] = {15}, [updatedBy] = '{16}' " +
                        "WHERE [idTransport] = {17}";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static string sqlShoftDeleteTransport {
            get {
                return "UPDATE Transports " +
                            " SET [isDeleted] = {0}, [updatedBy] = '{1}' " +
                        "WHERE [idTransport] = {2}";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Entity.transport> getTransport() {
            return transportDAC.getTransportFromDB(string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modemName"></param>
        /// <returns></returns>
        public static Entity.transport getTransport(long idTransport) {
            List<Entity.transport> transportList = transportDAC.getTransportFromDB(string.Format(" AND [idTransport] = {0}", idTransport));

            if(transportList != null && transportList.Count != 0) {
                return transportList[ 0 ];

            } else {
                return null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="idTransport"></param>
        /// <returns></returns>
        public static Entity.transport getTransport(string modemName) {
            List<Entity.transport> transportList = transportDAC.getTransportFromDB(string.Format(" AND [transportName] = '{0}'", modemName));

            if (transportList != null && transportList.Count != 0) {
                return transportList[0];

            } else {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static List<Entity.transport> getTransportFromDB(string filterCriteria) {
            List<Entity.transport>  transportsList  = null;
            Entity.transport        instTransport   = null;

            using (IDbConnection instConn = dacFactory.getConnection()) {
                using (IDbCommand instCmd = dacFactory.getCommand(string.Format(transportDAC.sqlAllTransports, filterCriteria), instConn)) {
                    using(IDataReader instDR = instCmd.ExecuteReader(CommandBehavior.CloseConnection)) {
                        while(instDR.Read()) {
                            instTransport = new Entity.transport();

                            instTransport.idTransport = Convert.ToInt64(instDR["idTransport"]);
                            instTransport.baudRate = Convert.ToInt64(instDR["baudRate"]);
                            instTransport.commPort = Convert.ToString(instDR["commPort"]);
                            instTransport.createdBy = Convert.ToString(instDR["createdBy"]);
                            instTransport.createdOn = Convert.ToDateTime(instDR["createdOn"]);
                            instTransport.dataBits = Convert.ToInt32(instDR["dataBits"]);
                            instTransport.direction = Convert.ToByte(instDR["direction"]);
                            instTransport.dtrEnable = Convert.ToBoolean(instDR["dtrEnable"]);
                            instTransport.handShake = (Handshake) Convert.ToInt32(instDR["handShake"]);
                            instTransport.httpApipUrl = Convert.IsDBNull(instDR["httpApiUrl"]) ? string.Empty : Convert.ToString(instDR["httpApiUrl"]);
                            instTransport.isTestModem = Convert.ToBoolean(instDR["isTestModem"]);
                            instTransport.isValid = Convert.ToBoolean(instDR["isValid"]);
                            instTransport.parity = (Parity) Convert.ToByte(instDR["parity"]);
                            instTransport.readTimeOut = Convert.ToInt32(instDR["readTimeOut"]) < 0 ? instTransport.readTimeOut : Convert.ToInt32(instDR["readTimeOut"]);
                            instTransport.rtsEnable = Convert.ToBoolean(instDR["rtsEnable"]);
                            instTransport.stopBits = (StopBits) Convert.ToInt32(instDR["idTransport"]);
                            instTransport.transportName = Convert.ToString(instDR["transportName"]);
                            instTransport.transportType = Convert.ToString(instDR["transportType"]);
                            instTransport.updatedBy = Convert.ToString(instDR["updatedBy"]);
                            instTransport.updatedOn = Convert.ToDateTime(instDR["updatedOn"]);
                            instTransport.writeTimeOut = Convert.ToInt32(instDR["writeTimeOut"]) < 0 ? instTransport.writeTimeOut : Convert.ToInt32(instDR["writeTimeOut"]);

                            if(transportsList == null) {
                                transportsList = new List<Entity.transport>();

                            }

                            transportsList.Add(instTransport);

                        }

                        instDR.Close();
                        instDR.Dispose();

                    }

                    instCmd.Cancel();
                    instCmd.Dispose();

                }

                instConn.Close();
                instConn.Dispose();

            }

            return transportsList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transportName"></param>
        /// <param name="transportType"></param>
        /// <param name="httpApiUrl"></param>
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
        /// <param name="userName"></param>
        /// <returns></returns>
        public static bool insertTransport(string transportName, string transportType, string httpApiUrl, string commPort, long baudRate
                                            , Parity parity, int dataBits, StopBits stopBits, Handshake handShake
                                            , int readTimeOut, int writeTimeOut, bool dtrEnable, bool rtsEnable, byte direction
                                            , bool isTestModem, bool isValid, string userName) {
            
            bool queryStatus = false;
            string sqlInsert = string.Empty;

            using (IDbConnection instConn = dacFactory.getConnection()) {
                sqlInsert = string.Format(transportDAC.sqlInsertTransport
                                            , transportName, transportType, httpApiUrl, commPort, baudRate, parity, dataBits, stopBits, handShake
                                            , readTimeOut, writeTimeOut, dtrEnable, rtsEnable, direction
                                            , isTestModem, isValid, userName, userName);

                using (IDbCommand instCmd = dacFactory.getCommand(sqlInsert, instConn)) {
                    
                    queryStatus = (instCmd.ExecuteNonQuery() >= 1);

                    instCmd.Cancel();
                    instCmd.Dispose();

                }

                instConn.Close();
                instConn.Dispose();

            }

            return queryStatus;
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldTransportName"></param>
        /// <param name="transportName"></param>
        /// <param name="transportType"></param>
        /// <param name="httpApiUrl"></param>
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
        /// <param name="userName"></param>
        /// <returns></returns>
        public static bool updateTransport(long idTransport, string transportName, string transportType, string httpApiUrl, string commPort, long baudRate
                                            , Parity parity, int dataBits, StopBits stopBits, Handshake handShake
                                            , int readTimeOut, int writeTimeOut, bool dtrEnable, bool rtsEnable, byte direction
                                            , bool isTestModem, bool isValid, string userName) {


            bool queryStatus = false;
            string sqlUpdate = string.Empty;

            using (IDbConnection instConn = dacFactory.getConnection()) {
                sqlUpdate = string.Format(transportDAC.sqlUpdateTransport
                                            , transportName, transportType, httpApiUrl, commPort, baudRate
                                            , (int) parity, dataBits, (int) stopBits, (int) handShake
                                            , readTimeOut, writeTimeOut, dtrEnable ? 1 : 0, rtsEnable ? 1 : 0, (int) direction, isTestModem ? 1 : 0, isValid ? 1 : 0
                                            , userName, idTransport);

                using (IDbCommand instCmd = dacFactory.getCommand(sqlUpdate, instConn)) {

                    queryStatus = (instCmd.ExecuteNonQuery() >= 1);

                    instCmd.Cancel();
                    instCmd.Dispose();

                }

                instConn.Close();
                instConn.Dispose();

            }

            return queryStatus;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="transportName"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static bool deleteTransport(long idTransport, string userName) {
            bool queryStatus = false;
            string sqlDelete = string.Empty;
           
            using (IDbConnection instConn = dacFactory.getConnection()) {
                sqlDelete = string.Format(transportDAC.sqlShoftDeleteTransport,
                                            1, userName, idTransport);

                using (IDbCommand instCmd = dacFactory.getCommand(sqlDelete, instConn)) {
                    queryStatus = (instCmd.ExecuteNonQuery() >= 1);

                    instCmd.Cancel();
                    instCmd.Dispose();
                }

                instConn.Close();
                instConn.Dispose();

            }

            return queryStatus;
        }
    }
}
