using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using log4net;

namespace kosal.common.logger {
    public sealed class logger {
        
        private string instanceName { get; set; }

        private ILog instance { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        private logger(ILog logger) {
            this.instance=  logger;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggerName"></param>
        /// <returns></returns>
        public static logger getInstance(string loggerName) {
            logger instLocal = new logger(LogManager.GetLogger(loggerName));

            return instLocal;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void logDebug(string message) {
            this.log(message, logLevel.Debug);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void logError(string message) {
            this.log(message, logLevel.Error);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        public void log(string message, logLevel level) {
            if(this.instance != null) {
                switch (level) {
                    case logLevel.Error:
                        this.instance.Error(message);
                        break;
                    case logLevel.Fatal:
                        this.instance.Fatal(message);
                        break;
                    case logLevel.Information:
                        this.instance.Info(message);
                        break;
                    case logLevel.Warning:
                        this.instance.Warn(message);
                        break;
                    case logLevel.Debug:
                        this.instance.Debug(message);
                        break;
                }
            }
        }
    }
}