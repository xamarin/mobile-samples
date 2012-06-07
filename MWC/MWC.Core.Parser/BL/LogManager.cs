using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace MWC.BL
{
	public enum LogLevel
	{
		DEBUG,
		ERROR,
		FATAL,
		INFO,
		WARN
	}

	/// <summary>
	/// LogManager handles all logging for the application.
	/// </summary>
	public static class LogManager
	{
		/// <summary>
		/// ctor to configure log4net
		/// </summary>
		static LogManager()
		{
			log4net.Config.XmlConfigurator.Configure();
		}

		private static ILog log = log4net.LogManager.GetLogger(typeof(MWC.BL.LogManager));

		/// <summary>
		/// Write the exception message and stack trace fo the log
		/// </summary>
		/// <param name="ex"></param>
		public static void WriteLog(Exception ex)
		{
			WriteLog(LogLevel.ERROR, string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace));
			if(ex.InnerException != null)
			{
				WriteLog(ex.InnerException);
			}
		}
		/// <summary>
		/// Write the message and loglevel to the log.
		/// </summary>
		/// <param name="logLevel"></param>
		/// <param name="msg"></param>
		public static void WriteLog(LogLevel logLevel, string msg)
		{
			switch(logLevel)
			{
				case LogLevel.DEBUG:
					log.Debug(msg);
					break;
				case LogLevel.ERROR:
					log.Error(msg);
					break;
				case LogLevel.FATAL:
					log.Fatal(msg);
					break;
				case LogLevel.INFO:
					log.Info(msg);
					break;
				case LogLevel.WARN:
					log.Warn(msg);
					break;
				default: throw new NotImplementedException();
			}
		}
	}
}
