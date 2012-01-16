using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Serialization;
using MWC.BL;

namespace MWC.SAL
{
	public class MWCSiteParser
	{
		string _url;
		static MWCSiteParser ()
		{
		}
		public MWCSiteParser (string url)
		{
			_url = url;
		}
		public Conference ConferenceData {get;set;}

		public void GetConference (Action action)
		{			
			var webClient = new WebClient ();
			Debug.WriteLine ("Get remote data for conference");
			webClient.DownloadStringCompleted += (sender, e) =>
			{
				try {
					var r = e.Result;
					var serializer = new XmlSerializer(typeof(Conference));
					var sr = new StringReader(r);
					ConferenceData = (Conference)serializer.Deserialize(sr);
				} catch (Exception ex) {
					Debug.WriteLine ("ERROR deserializing downloaded XML: " + ex);
				}
				action();
			};
			webClient.Encoding = System.Text.Encoding.UTF8;
			webClient.DownloadStringAsync (new Uri (_url));
		}

		public static IList<Exhibitor> GetExhibitors()
		{
			//stub
			return new List<Exhibitor> () { 
				new Exhibitor() { Name = "Test", City = "Somwhere", Country = "USA", Locations = "h1b, e12" }
				, new Exhibitor() { Name = "Ploock Fap", City = "fapville", Country = "USA", Locations = "h1b, e12" }
				, new Exhibitor() { Name = "IrkleSparks", City = "sparks", Country = "USA", Locations = "h1b, e12" }
				, new Exhibitor() { Name = "Shia LeBouf Sucks", City = "hollywood", Country = "USA", Locations = "h1b, e12" }
				, new Exhibitor() { Name = "Sprinkle Turkey", City = "Istanbul", Country = "Turkey", Locations = "h1b, e12" }
				, new Exhibitor() { Name = "Onion Core", City = "Somwhere", Country = "USA", Locations = "h1b, e12" }
				, new Exhibitor() { Name = "Temple Fist", City = "Wherever you are", Country = "USA", Locations = "h1b, e12" }
				, new Exhibitor() { Name = "Mr. Chin Very Angry with you.", City = "THeLongestCityNameInTheWorldIsNotThi", Country = "USA", Locations = "h1b, e12" }
				, new Exhibitor() { Name = "packelPlop", City = "Somwhere", Country = "USA", Locations = "h1b, e12" }
				, new Exhibitor() { Name = "instrin", City = "Somwhere", Country = "USA", Locations = "h1b, e12" }
			};
		}
		
		public static IList<Session> GetSessions()
		{
			//stub
			return new List<Session> () {
				
  new Session() {Title="Opening Keynote 1: Mobile Operator Strategies in Developed Markets", Overview="Opening Keynote 1: Mobile Operator Strategies in Developed Markets", Start=DateTime.Parse("2012-02-27 9:30"), End=DateTime.Parse("2012-02-27 11:0")}
, new Session() {Title="Networking Break", Overview="Networking Break", Start=DateTime.Parse("2012-02-27 11:0"), End=DateTime.Parse("2012-02-27 11:30")}
, new Session() {Title="Keynote 2: The Connected Consumer", Overview="Keynote 2: The Connected Consumer", Start=DateTime.Parse("2012-02-27 11:30"), End=DateTime.Parse("2012-02-27 13:0")}
, new Session() {Title="Congress Lunch", Overview="Congress Lunch", Start=DateTime.Parse("2012-02-27 13:0"), End=DateTime.Parse("2012-02-27 14:0")}
, new Session() {Title="Mobile Applications: Apps for All - How Apps are Changing our Lives ", Overview="Mobile Applications: Apps for All - How Apps are Changing our Lives ", Start=DateTime.Parse("2012-02-27 14:0"), End=DateTime.Parse("2012-02-27 15:30")}
, new Session() {Title="Mobile Health: Getting Mobile into the System - What Weâ€™ve Learnt so Far", Overview="Mobile Health: Getting Mobile into the System - What Weâ€™ve Learnt so Far", Start=DateTime.Parse("2012-02-27 14:0"), End=DateTime.Parse("2012-02-27 15:30")}
, new Session() {Title="Mobile Cloud: Competitive Landscape", Overview="Mobile Cloud: Competitive Landscape", Start=DateTime.Parse("2012-02-27 14:0"), End=DateTime.Parse("2012-02-27 15:30")}
, new Session() {Title="Business Transformation: Operators as Agile Businesses ", Overview="Business Transformation: Operators as Agile Businesses ", Start=DateTime.Parse("2012-02-27 14:0"), End=DateTime.Parse("2012-02-27 15:30")}
, new Session() {Title="Networking Break", Overview="Networking Break", Start=DateTime.Parse("2012-02-27 15:30"), End=DateTime.Parse("2012-02-27 16:0")}
, new Session() {Title="Mobile Applications: The Future of Voice & Messaging", Overview="Mobile Applications: The Future of Voice & Messaging", Start=DateTime.Parse("2012-02-27 16:0"), End=DateTime.Parse("2012-02-27 17:30")}
, new Session() {Title="Mobile Health: mHealth & The User - Building Trust, Creating ", Overview="Mobile Health: mHealth & The User - Building Trust, Creating ", Start=DateTime.Parse("2012-02-27 16:0"), End=DateTime.Parse("2012-02-27 17:30")}
, new Session() {Title="Mobile Cloud: Contending for Content ", Overview="Mobile Cloud: Contending for Content ", Start=DateTime.Parse("2012-02-27 16:0"), End=DateTime.Parse("2012-02-27 17:30")}
, new Session() {Title="Business Transformation: Operators as Intelligent Partners", Overview="Business Transformation: Operators as Intelligent Partners", Start=DateTime.Parse("2012-02-27 16:0"), End=DateTime.Parse("2012-02-27 17:30")}
, new Session() {Title="Networking Break", Overview="Networking Break", Start=DateTime.Parse("2012-02-27 17:30"), End=DateTime.Parse("2012-02-27 18:0")}
, new Session() {Title="Mobile World Live Keynote", Overview="Mobile World Live Keynote", Start=DateTime.Parse("2012-02-27 18:0"), End=DateTime.Parse("2012-02-27 18:45")}


, new Session() {Title="Keynote 3: Mobile Operator Strategies in Developing Markets", Overview="Keynote 3: Mobile Operator Strategies in Developing Markets", Start=DateTime.Parse("2012-02-28 9:0"), End=DateTime.Parse("2012-02-28 10:30")}
, new Session() {Title="Networking Break", Overview="Networking Break", Start=DateTime.Parse("2012-02-28 10:30"), End=DateTime.Parse("2012-02-28 11:0")}
, new Session() {Title="Keynote 4: Exploring the Mobile Cloud ", Overview="Keynote 4: Exploring the Mobile Cloud ", Start=DateTime.Parse("2012-02-28 11:0"), End=DateTime.Parse("2012-02-28 12:30")}
, new Session() {Title="Congress Lunch", Overview="Congress Lunch", Start=DateTime.Parse("2012-02-28 12:30"), End=DateTime.Parse("2012-02-28 14:0")}
, new Session() {Title="Mobile Applications: Building for Tomorrow - The Evolution of App Development", Overview="Mobile Applications: Building for Tomorrow - The Evolution of App Development", Start=DateTime.Parse("2012-02-28 14:0"), End=DateTime.Parse("2012-02-28 15:30")}
, new Session() {Title="Mobile Health: Emerging Markets - Partnerships for Growth", Overview="Mobile Health: Emerging Markets - Partnerships for Growth", Start=DateTime.Parse("2012-02-28 14:0"), End=DateTime.Parse("2012-02-28 15:30")}
, new Session() {Title="Mobile Cloud: Home of the Future Embraces Mobile Cloud", Overview="Mobile Cloud: Home of the Future Embraces Mobile Cloud", Start=DateTime.Parse("2012-02-28 14:0"), End=DateTime.Parse("2012-02-28 15:30")}
, new Session() {Title="Mobile Advertising: Mobile in the Marketing Mix - Does Having an App Count?", Overview="Mobile Advertising: Mobile in the Marketing Mix - Does Having an App Count?", Start=DateTime.Parse("2012-02-28 14:0"), End=DateTime.Parse("2012-02-28 15:30")}
, new Session() {Title="Networking Break", Overview="Networking Break", Start=DateTime.Parse("2012-02-28 15:30"), End=DateTime.Parse("2012-02-28 16:0")}
, new Session() {Title="Mobilising the Retail Business ", Overview="Mobilising the Retail Business ", Start=DateTime.Parse("2012-02-28 16:0"), End=DateTime.Parse("2012-02-28 17:30")}
, new Session() {Title="Mobile Enterprise: Maximising Workforce Productivity in the Age of the Consumerisation", Overview="Mobile Enterprise: Maximising Workforce Productivity in the Age of the Consumerisation", Start=DateTime.Parse("2012-02-28 16:0"), End=DateTime.Parse("2012-02-28 17:30")}
, new Session() {Title="Mobile Cloud: Operators Fight Back - Cloud & Network Intelligence", Overview="Mobile Cloud: Operators Fight Back - Cloud & Network Intelligence", Start=DateTime.Parse("2012-02-28 16:0"), End=DateTime.Parse("2012-02-28 17:30")}
, new Session() {Title="Mobile Advertising: Context is King - Advertising & Marketing with Mobile Social Media ", Overview="Mobile Advertising: Context is King - Advertising & Marketing with Mobile Social Media ", Start=DateTime.Parse("2012-02-28 16:0"), End=DateTime.Parse("2012-02-28 17:30")}
, new Session() {Title="Networking Break", Overview="Networking Break", Start=DateTime.Parse("2012-02-28 17:30"), End=DateTime.Parse("2012-02-28 18:0")}
, new Session() {Title="Mobile World Live Keynote", Overview="Mobile World Live Keynote", Start=DateTime.Parse("2012-02-28 18:0"), End=DateTime.Parse("2012-02-28 18:45")}


, new Session() {Title="Keynote 5: Mobile OS & Applications", Overview="Keynote 5: Mobile OS & Applications", Start=DateTime.Parse("2012-02-29 9:0"), End=DateTime.Parse("2012-02-29 10:30")}
, new Session() {Title="Networking Break", Overview="Networking Break", Start=DateTime.Parse("2012-02-29 10:30"), End=DateTime.Parse("2012-02-29 11:0")}
, new Session() {Title="Keynote 6: Financial Services in a Mobile World", Overview="Keynote 6: Financial Services in a Mobile World", Start=DateTime.Parse("2012-02-29 11:0"), End=DateTime.Parse("2012-02-29 12:30")}
, new Session() {Title="Congress Lunch", Overview="Congress Lunch", Start=DateTime.Parse("2012-02-29 12:30"), End=DateTime.Parse("2012-02-29 13:45")}
, new Session() {Title="Mobile Innovation: A Vision of 2022", Overview="Mobile Innovation: A Vision of 2022", Start=DateTime.Parse("2012-02-29 13:45"), End=DateTime.Parse("2012-02-29 15:15")}
, new Session() {Title="Regional Focus: BRICS & The Challenges of Innovation", Overview="Regional Focus: BRICS & The Challenges of Innovation", Start=DateTime.Parse("2012-02-29 13:45"), End=DateTime.Parse("2012-02-29 15:15")}
, new Session() {Title="Networks: Infrastructure Costs in the Age of Austerity", Overview="Networks: Infrastructure Costs in the Age of Austerity", Start=DateTime.Parse("2012-02-29 13:45"), End=DateTime.Parse("2012-02-29 15:15")}
, new Session() {Title="Consumer Devices:  Riding the Next Wave of Smart Devices ", Overview="Consumer Devices:  Riding the Next Wave of Smart Devices ", Start=DateTime.Parse("2012-02-29 13:45"), End=DateTime.Parse("2012-02-29 15:15")}
, new Session() {Title="Mobile Advertising: The Mobile Advertising Ecosystem - Making it Work", Overview="Mobile Advertising: The Mobile Advertising Ecosystem - Making it Work", Start=DateTime.Parse("2012-02-29 13:45"), End=DateTime.Parse("2012-02-29 15:15")}
, new Session() {Title="Networking Break", Overview="Networking Break", Start=DateTime.Parse("2012-02-29 15:15"), End=DateTime.Parse("2012-02-29 15:45")}
, new Session() {Title="Mobile Money: NFC Services Gain Momentum", Overview="Mobile Money: NFC Services Gain Momentum", Start=DateTime.Parse("2012-02-29 15:45"), End=DateTime.Parse("2012-02-29 17:15")}
, new Session() {Title="Embedded Mobile: State of the Market", Overview="Embedded Mobile: State of the Market", Start=DateTime.Parse("2012-02-29 15:45"), End=DateTime.Parse("2012-02-29 17:15")}
, new Session() {Title="Networks: Delivering Quality of Experience, Despite Capacity Constraints", Overview="Networks: Delivering Quality of Experience, Despite Capacity Constraints", Start=DateTime.Parse("2012-02-29 15:45"), End=DateTime.Parse("2012-02-29 17:15")}
, new Session() {Title="Media & Entertainment:  The Future of Mobile Music", Overview="Media & Entertainment:  The Future of Mobile Music", Start=DateTime.Parse("2012-02-29 15:45"), End=DateTime.Parse("2012-02-29 17:15")}
, new Session() {Title="Mobile Advertising: Emerging Markets ", Overview="Mobile Advertising: Emerging Markets ", Start=DateTime.Parse("2012-02-29 15:45"), End=DateTime.Parse("2012-02-29 17:15")}
, new Session() {Title="Networking Break", Overview="Networking Break", Start=DateTime.Parse("2012-02-29 17:15"), End=DateTime.Parse("2012-02-29 18:0")}
, new Session() {Title="Mobile World Live Keynote", Overview="Mobile World Live Keynote", Start=DateTime.Parse("2012-02-29 18:0"), End=DateTime.Parse("2012-02-29 18:45")}


, new Session() {Title="Keynote 7: Driving the Mobile Technology Evolution", Overview="Keynote 7: Driving the Mobile Technology Evolution", Start=DateTime.Parse("2012-03-01 9:30"), End=DateTime.Parse("2012-03-01 11:0")}
, new Session() {Title="Networking Break", Overview="Networking Break", Start=DateTime.Parse("2012-03-01 11:0"), End=DateTime.Parse("2012-03-01 11:30")}
, new Session() {Title="Mobile Money: Delivering Innovative Mobile Payment Services", Overview="Mobile Money: Delivering Innovative Mobile Payment Services", Start=DateTime.Parse("2012-03-01 11:30"), End=DateTime.Parse("2012-03-01 13:0")}
, new Session() {Title="Embedded Mobile: The Market Opportunity in the Automotive and Utilities Sectors ", Overview="Embedded Mobile: The Market Opportunity in the Automotive and Utilities Sectors ", Start=DateTime.Parse("2012-03-01 11:30"), End=DateTime.Parse("2012-03-01 13:0")}
, new Session() {Title="Technology Evolution: Network Architecture Evolution ", Overview="Technology Evolution: Network Architecture Evolution ", Start=DateTime.Parse("2012-03-01 11:30"), End=DateTime.Parse("2012-03-01 13:0")}
, new Session() {Title="Congress Lunch", Overview="Congress Lunch", Start=DateTime.Parse("2012-03-01 13:0"), End=DateTime.Parse("2012-03-01 14:30")}
, new Session() {Title="Mobile Money: Emerging Markets - Where are the Opportunities for Mobile Money?", Overview="Mobile Money: Emerging Markets - Where are the Opportunities for Mobile Money?", Start=DateTime.Parse("2012-03-01 14:30"), End=DateTime.Parse("2012-03-01 16:0")}
, new Session() {Title="Embedded Mobile: Consumer Electronics - The Case for Embedding Mobile Connectivity", Overview="Embedded Mobile: Consumer Electronics - The Case for Embedding Mobile Connectivity", Start=DateTime.Parse("2012-03-01 14:30"), End=DateTime.Parse("2012-03-01 16:0")}
, new Session() {Title="Technology Evolution: Network Operations Evolution", Overview="Technology Evolution: Network Operations Evolution", Start=DateTime.Parse("2012-03-01 14:30"), End=DateTime.Parse("2012-03-01 16:0")}


			};
		}
		
		public static IList<Speaker> GetSpeakers()
		{
			return new List<Speaker> () { 

  new Speaker() {Name="Franco BernabÈ", Title="Chairman & CEO", Company="Telecom Italia Group"}
, new Speaker() {Name="Anne Bourverot", Title="Director General", Company="GSMA"}
, new Speaker() {Name="Ben Verwaayen", Title="CEO", Company="Alcatel-Lucent"}
, new Speaker() {Name="Ralph de la Vega", Title="President & CEO AT&T Mobility & Consumer Markets", Company="AT&T"}
, new Speaker() {Name="Brian Dunn", Title="CEO", Company="Best Buy"}
, new Speaker() {Name="Sunil Mittal", Title="Chairman & MD", Company="Bharti Airtel"}
, new Speaker() {Name="Vittorio Colao", Title="Chief Executive", Company="Vodafone"}
, new Speaker() {Name="Vikram Pandit", Title="CEO", Company="Citigroup"}
, new Speaker() {Name="RenÈ Obermann", Title="Chairman & CEO", Company="Deutsche Telecom"}
, new Speaker() {Name="John Riccitiello", Title="CEO", Company="EA"}
, new Speaker() {Name="John Donahoe", Title="CEO", Company="eBay"}
, new Speaker() {Name="Hans Vestberg", Title="President & CEO ", Company="Ericsson"}
, new Speaker() {Name="Eric Scmidt", Title="Executive Chairman ", Company="Google"}
, new Speaker() {Name="Peter Chou", Title="CEO", Company="HTC"}
, new Speaker() {Name="Michael Roth", Title="Chairman & CEO", Company="IPG"}
, new Speaker() {Name="Michael Abbott", Title="CEO", Company="ISIS"}
, new Speaker() {Name="Kevin Johnson", Title="CEO", Company="Juniper Networks"}
, new Speaker() {Name="Stephen Elop", Title="President & CEO", Company="Nokia"}
, new Speaker() {Name="Ryuji Yamada", Title="President & CEO", Company="NTT DOCOMO"}
, new Speaker() {Name="Jon Fredrik Baksaas", Title="President & CEO", Company="Telenor Group"}
, new Speaker() {Name="Jo Lunder", Title="CEO", Company="VimpelCom"}


			};
		}
	}
}