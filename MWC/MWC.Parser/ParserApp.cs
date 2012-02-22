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
			{ DoSessions(path); }
			if(parseItems == "exhibitors" || parseItems == "both")
			{ DoExhibitors(path); }
		}

		private void DoSessions(string path)
		{
			bool doPartial = parseMode == "partial";

			Output("Getting sessions/speakers/");
			Conference conf = ConferenceManager.GetConference(doPartial);
			Output(string.Concat(conf.Sessions.Count, " sessions"));
			Output(string.Concat(conf.Speakers.Count, " speakers"));

			Output("Writing output");
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

			Output("Getting exhibitors");
			Conference conf = new Conference();
			conf.Exhibitors = ExhibitorManager.GetExhibitorList(doPartial);
			Output(string.Concat(conf.Exhibitors.Count, " exhibitors"));
			Output("Writing output");
			using(TextWriter tw = File.CreateText(Path.Combine(path, "Exhibitors.xml")))
			{
				XmlSerializer serializer = new XmlSerializer(typeof(Conference));
				serializer.Serialize(tw, conf);
			}

			using(TextWriter tw = File.CreateText(Path.Combine(path, "Exhibitors.html")))
			{
				tw.Write(DateTime.Now.ToString());
			}
		}

		private void Output(string msg)
		{
			Console.WriteLine(msg);
			LogManager.WriteLog(LogLevel.INFO, msg);
		}
	}
}
