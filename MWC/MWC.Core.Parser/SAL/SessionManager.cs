using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using MWC.SAL.Helpers;
using MWC.BL;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;

namespace MWC.SAL
{
	public static class SessionManager
	{
		private static string baseUrl = "http://www.mobileworldcongress.com";

		public static List<Session> GetSessionList(bool doPartial)
		{
			List<Session> results = new List<Session>();
			int page = 12;

			while(true)
			{
				string url = baseUrl + "/API/WebService.asmx/GetEventSessionsByTopic";
				//string data = String.Concat("{\"eventID\":7,\"pageSize\":10,\"page\":", page, ",\"topicID\":-1,\"searchName\":\"\"}");
				string data = JsonConvert.SerializeObject(new JsonRequest(page));
				string json = string.Empty;

				json = WebHelper.HttpPost(url, data);
				var j = JObject.Parse(json);

				HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
				doc.LoadHtml(j["d"]["ReturnText"].ToString());

				var nodes = doc.DocumentNode.SelectNodes("//li[@class='eventSessionListItem']");
				if(nodes != null)
				{
					foreach(HtmlAgilityPack.HtmlNode node in nodes)
					{
						Session session = new Session();
						session.Title = node.SelectSingleNode("div[2]/h3/a").InnerText;
						session.DetailUrl = node.SelectSingleNode("div[2]/h3/a").Attributes["href"].Value;
						var date = node.SelectSingleNode("div[3]/div");
						var time = node.SelectSingleNode("div[3]/div[2]");

						var dateTime = ParseDateTime(date.InnerText, time.InnerText);
						session.Start = dateTime[0];
						session.End = dateTime[1];
						session.Room = HtmlDocHelper.GetInnerText(node, "div[2]/div/ul[@class='ulLocations']");
						session.SpeakerNames = HtmlDocHelper.GetInnerText(node, "div[2]/div[@class='eventSessionSpeakers']/ul");

						results.Add(session);
					}
				}
				else
				{
					break;
				}
				page++;

				if(doPartial) break;
			}

			results = GetExtendedSessions(results);

			foreach(var result in results)
			{
				result.SpeakerList = GetExtendedSpeakers(result.SpeakerList);
			}

			return results;
		}

		private static List<Session> GetExtendedSessions(List<Session> sessions)
		{
			foreach(var session in sessions)
			{
				string url = baseUrl + session.DetailUrl;
				string html = WebHelper.HttpGet(url);

				HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
				doc.LoadHtml(html);

				string text = string.Empty;

				var node = doc.DocumentNode.SelectSingleNode("//*[@id=\"page_content_Content4_oModuleEventSessions_5_ctl00_ctl01_divDescription\"]");
				if(node != null)
				{
					var nodes = node.SelectNodes("p");
					if(nodes != null)
					{
						bool wasEmpty = false;
						foreach(var n in nodes)
						{
							string part = HttpUtility.HtmlDecode(n.InnerText.Trim().Replace("\n\t", "\r\n"));
							if(part == "Jointly developed with") { part = ""; }

							if(part.Trim() == string.Empty)
							{
								if(!wasEmpty)
								{
									text += "\r\n";
									wasEmpty = true;
								}
							}
							else
							{
								text += part.Trim() + "\r\n";
								wasEmpty = false;
							}

							//foreach(var item in n.SelectNodes("text()"))
							//{
							//    string part = HttpUtility.HtmlDecode(item.InnerText.Trim().Replace("\n\t", "\r\n"));
							//    text += part + "\r\n";
							//}
						}
					}
				}
				session.Overview = text.Trim();
				session.Overview = session.Overview.Replace("Click here", "Visit the MWC website");

				var speakers = doc.DocumentNode.SelectNodes("//*[@id=\"page_content_Content4_oModuleEventSessions_5_ctl00_ctl02_pnlSpeakers\"]/ul/li");
				if(speakers != null)
				{
					foreach(var s in speakers)
					{
						if(s.InnerText != "No speakers")
						{
							Speaker speaker = new Speaker();
							speaker.Name = HtmlDocHelper.GetInnerText(s, "div[@class='event-list-name']/a");
							speaker.DetailUrl = HtmlDocHelper.GetAttribute(s, "div[@class='event-list-name']/a", "href");
							speaker.Company = HtmlDocHelper.GetInnerText(s, "div[@class='event-list-company']");
							speaker.Title = HtmlDocHelper.GetInnerText(s, "div[@class='event-list-position']");
							session.SpeakerList.Add(speaker);
							session.SpeakerKeys.Add(speaker.Key);
						}
					}
				}
			}
			return sessions;
		}

		private static List<Speaker> GetExtendedSpeakers(List<Speaker> speakers)
		{
			foreach(var speaker in speakers)
			{
				string url = baseUrl + speaker.DetailUrl;
				string html = WebHelper.HttpGet(url);

				HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
				doc.LoadHtml(html);

				speaker.ImageUrl = baseUrl + HtmlDocHelper.GetAttribute(doc, "//div[@class='profile-pic profile-pic-large']/a/img", "src");

				var nodes = doc.DocumentNode.SelectNodes("//div[@class='uiUserField_Name']");
				if(nodes != null)
				{
					foreach(var node in nodes)
					{
						if(node.InnerText == "About Me")
						{
							speaker.Bio = node.NextSibling.NextSibling.InnerText.Trim();
						}
					}
				}
			}
			return speakers;
		}

		private static DateTime[] ParseDateTime(string dt, string ti)
		{
			DateTime[] results = new DateTime[2];
			DateTime startDate = DateTime.Parse(dt + " " + ti.Split('-')[0]);
			DateTime endDate = DateTime.Parse(dt + " " + ti.Split('-')[1]);
			results[0] = startDate;
			results[1] = endDate;
			return results;
		}
	}
}
