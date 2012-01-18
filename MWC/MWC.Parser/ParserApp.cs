using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MWC.BL;
using System.Xml.Serialization;
using System.Configuration;

namespace MWC.Parser
{
	public class ParserApp
	{
		static string parseMode = ConfigurationManager.AppSettings["parseMode"];
		static string parseItems = ConfigurationManager.AppSettings["parseItems"];

		public void WriteConferenceXML(string path)
		{
			if(parseItems == "sessions" || parseItems == "both")
			{
				DoSessions(path);
			}
			if(parseItems == "exhibitors" || parseItems == "both")
			{
				DoExhibitors(path);
			}

		}

		private void DoSessions(string path)
		{
			bool doPartial = parseMode == "partial";

			Console.WriteLine("Getting sessions/speakers/");
			Conference conf = ConferenceManager.GetConference(doPartial);
			Console.WriteLine(string.Concat(conf.Sessions.Count, " sessions"));
			Console.WriteLine(string.Concat(conf.Speakers.Count, " speakers"));

			Console.WriteLine("Writing output");
			using(TextWriter tw = File.CreateText(Path.Combine(path, "Conference.xml")))
			{
				XmlSerializer serializer = new XmlSerializer(typeof(Conference));
				serializer.Serialize(tw, conf);
			}

			using(TextWriter tw = File.CreateText(Path.Combine(path, "Conference.html")))
			{
				tw.Write(DateTime.Now.ToString());
			}
		}

		private void DoExhibitors(string path)
		{
			bool doPartial = parseMode == "partial";

			Console.WriteLine("Getting exhibitors");
			Exhibitors exhibitors = new Exhibitors();
			exhibitors.Items = ExhibitorManager.GetExhibitorList(doPartial);
			Console.WriteLine(string.Concat(exhibitors.Items.Count, " exhibitors"));
			Console.WriteLine("Writing output");
			using(TextWriter tw = File.CreateText(Path.Combine(path, "Exhibitors.xml")))
			{
				XmlSerializer serializer = new XmlSerializer(typeof(Exhibitors));
				serializer.Serialize(tw, exhibitors);
			}

			using(TextWriter tw = File.CreateText(Path.Combine(path, "Exhibitors.html")))
			{
				tw.Write(DateTime.Now.ToString());
			}
		}
	}
}
