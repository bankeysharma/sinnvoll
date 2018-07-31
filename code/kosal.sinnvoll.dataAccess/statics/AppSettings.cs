using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

using kosal.common.library;

namespace kosal.sinnvoll.dataAccess.statics {
	public static class AppSettings {
    
        public static string connectionString { get; set; }
        public static string appDatabase { get; set; }
        
        static AppSettings() {
            //AppSettings.connectionString = encryption.Decrypt(ConfigurationManager.ConnectionStrings["sinnvollDb"].ConnectionString);
            AppSettings.connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Jet OLEDB:Database Password=;";
            AppSettings.appDatabase = ConfigurationManager.AppSettings["appDatabase"];

            AppSettings.connectionString = string.Format(AppSettings.connectionString, AppSettings.appDatabase);

            //string str = kosal.common.library.encryption.Encrypt(AppSettings.connectionString);

        }
	}
}
