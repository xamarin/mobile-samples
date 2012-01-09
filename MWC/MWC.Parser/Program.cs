using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace MWC.Parser
{
	class Program
	{
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
			}

			Console.WriteLine("Parsed that mofo good, yo.");
			Console.ReadLine();
		}
	}
}
