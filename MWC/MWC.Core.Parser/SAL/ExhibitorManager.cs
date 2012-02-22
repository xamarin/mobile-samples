using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MWC.BL;
using MWC.SAL.Helpers;
using Newtonsoft.Json.Linq;

namespace MWC.SAL
{
	public static class ExhibitorManager
	{
		private static string baseUrl = "http://www.mobileworldcongress.com";

		public static List<Exhibitor> GetExhibitorList(bool doPartial)
		{
			List<Exhibitor> results = new List<Exhibitor>();
			// TODO: make this 0
			int page = 0;

			while(true)
			{
				bool isBlankPage = true;
				#region Ajax Args
				// this.eventId = 7;
				//this.pageSize = 20;
				//this.page = isNaN(page) ? 0 : page;
				//this.sortOrder = $("#page_content_ctl00_ddlSort").val();
				//this.filterChar = filterChar;
				//this.searchName = $("#searchName").val();
				//this.searchCompany = $("#searchCompany").val();
				//this.searchCountry = $("#page_content_ctl00_ddlCountries option:selected").text();
				//this.searchInterest = $("#page_content_ctl00_ddlInterest").val(); 
				#endregion
				string url = baseUrl + "/API/WebService.asmx/GetExhibitors";
				string data = String.Concat("{\"eventId\":7,\"pageSize\":20,\"page\":", page, ",\"sortOrder\":0,\"filterChar\":\"\",\"searchName\":\"\",\"searchCompany\":\"\",\"searchCountry\":\"Any\",\"searchInterest\":-1}");
				string json = string.Empty;

				json = WebHelper.HttpPost(url, data);
				var j = JObject.Parse(json);

				HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
				doc.LoadHtml(j["d"]["ReturnText"].ToString());

				HtmlAgilityPack.HtmlNodeCollection nodes = null;
				if((nodes = doc.DocumentNode.SelectNodes("//li[@class='exhibitorListItem Connect']")) != null)
				{
					isBlankPage = false;
					results.AddRange(ParseExhibitors(nodes, true));
				}

				if((nodes = doc.DocumentNode.SelectNodes("//li[@class='exhibitorListItem Basic']")) != null)
				{
					isBlankPage = false;
					results.AddRange(ParseExhibitors(nodes, false));
				}

				if(isBlankPage) { break; }

				page++;

				if(doPartial) break;
			}
			results = GetExtendedData(results);
			return results;
		}

		private static List<Exhibitor> GetExtendedData(List<Exhibitor> exhibitors)
		{
			foreach(var exhibitor in exhibitors)
			{
				string url = baseUrl + exhibitor.DetailUrl;
				string html = WebHelper.HttpGet(url);

				HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
				doc.LoadHtml(html);

				exhibitor.Overview = HtmlDocHelper.GetInnerText(doc, "//*[@id=\"page_content_ctl00_ctl00_divDescription\"]");
				exhibitor.Tags = HtmlDocHelper.GetInnerText(doc, "//*[@id=\"page_content_ctl00_ctl00_ulDetailTags\"]");
				exhibitor.Email = HtmlDocHelper.GetInnerText(doc, "//*[@id=\"page_content_ctl00_ctl00_hlEmail\"]");
				exhibitor.Phone = HtmlDocHelper.GetInnerText(doc, "//*[@id=\"page_content_ctl00_ctl00_divTel\"]").Replace("Tel:", "").Trim();
				exhibitor.Fax = HtmlDocHelper.GetInnerText(doc, "//*[@id=\"page_content_ctl00_ctl00_divFax\"]").Replace("Fax:", "").Trim();
				exhibitor.ImageUrl = HtmlDocHelper.GetAttribute(doc.DocumentNode, "//*[@id=\"page_content_ctl00_ctl00_imgExhibitorDetail\"]", "src");
				
				var nodes = doc.DocumentNode.SelectNodes("//ul[@id=\"page_content_ctl00_ctl00_ulDetailAddress\"]/li");
				if(nodes != null)
				{
					List<string> addressParts = new List<string>();
					foreach(var node in nodes)
					{
						addressParts.Add(node.InnerText);
					}
					exhibitor.Address = string.Join(", ", addressParts);
				}
			}
			return exhibitors;
		}

		private static List<Exhibitor> ParseExhibitors(HtmlAgilityPack.HtmlNodeCollection nodes, bool isFeatured)
		{
			List<Exhibitor> exhibitors = new List<Exhibitor>();
			foreach(HtmlAgilityPack.HtmlNode node in nodes)
			{
				Exhibitor exhibitor = new Exhibitor();
				exhibitor.DetailUrl = node.SelectSingleNode("div[2]/h3/a").Attributes["href"].Value;
				exhibitor.Name = node.SelectSingleNode("div[2]/h3/a").InnerText.Trim();
				exhibitor.IsFeatured = isFeatured;

				string[] cityCountry = node.SelectSingleNode("div[2]/div/h4").InnerText.Split(',');
				if(cityCountry.Length >= 2)
				{
					exhibitor.City = cityCountry[0].Trim();
					exhibitor.Country = cityCountry[1].Trim();
				}

				var locationNodes = node.SelectNodes("div[3]/ul/li");
				if(locationNodes != null)
				{
					List<string> locations = new List<string>();

					foreach(var l in locationNodes)
					{ locations.Add(string.Concat("Hall ", l.InnerText)); }

					exhibitor.Locations = string.Join(", ", locations.ToArray());
				}
				exhibitors.Add(exhibitor);
			}
			return exhibitors;
		}
	}
}
