using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;

using kosal.sinnvoll.dataAccess.statics;

namespace kosal.sinnvoll.dataAccess {
    public static class dacFactory {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IDbConnection getConnection() {
            OleDbConnection instConn = new OleDbConnection(AppSettings.connectionString);
            return instConn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="instConn"></param>
        /// <returns></returns>
        public static IDbCommand getCommand(string commandText, IDbConnection instConn) {
            IDbCommand instCmd = new OleDbCommand(commandText);
            instCmd.Connection = instConn;
            instCmd.CommandType = CommandType.Text;
            instCmd.CommandTimeout = 120;
            
            if(instCmd.Connection.State == ConnectionState.Closed) {
                instCmd.Connection.Open();
            }

            return instCmd;
        }
    }
}
