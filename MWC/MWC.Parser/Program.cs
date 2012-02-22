using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using MWC.BL;

namespace MWC.Parser
{
	class Program
	{
		static bool doVerbose = bool.Parse(ConfigurationManager.AppSettings["doVerbose"]);
		static string path = ConfigurationManager.AppSettings["outputPath"];

		static void Main(string[] args)
		{
			ParserApp app = new ParserApp();
			try
			{
				app.WriteConferenceXML(path);
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);
				LogManager.WriteLog(ex);
			}

			if(doVerbose)
			{
				Console.WriteLine("done");
				Console.ReadLine();
			}

		}
	}
}
