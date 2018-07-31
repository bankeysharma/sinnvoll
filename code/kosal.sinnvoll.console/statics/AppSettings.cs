using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kosal.sinnvoll.console.statics {
	public static class AppSettings {
    
        public static string connectionString { get; set; }
        
        static AppSettings() {
            AppSettings.connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\workspace\sinnvoll\database\kosal.sinnvoll.accdb;Jet OLEDB:Database Password=;";
        }
	}
}
