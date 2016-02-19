// 
//  Copyright 2011  abhatia
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
using System;
using RestSharp;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using RestSharp.Contrib;
using System.Collections.Generic;
using System.IO;


namespace WebServices.RxNormSample
{
	/// <summary>
	/// Pillbox rest client.
	/// This is a shared instance Rest Client for the Pillbox API.
	/// </summary>
	public class PillboxClient
	{
		//API KEY = WGY7R8OGHU
		public static bool IsInitialized = false;
		
		static RestClient Client;
		static DirectoryInfo MyDocuments = new DirectoryInfo(Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments));
		
		static Func<string, string> DownloadImageAction = DownloadPillboxImage;
		
		public static bool Initialize()
		{
			Client = new RestClient(@"http://pillbox.nlm.nih.gov/PHP/pillboxAPIService.php");
			
			Client.AddDefaultParameter("key", "WGY7R8OGHU");
			Client.AddDefaultParameter("has_image", "1");
			
			IsInitialized = true;
			
			return IsInitialized;
		}
		
		public static void DownloadPillboxImageAsync(string rxcui, Action<string> callback = null)
		{
			var existingPath = Path.Combine(MyDocuments.FullName, rxcui + ".jpg");
			
			if(File.Exists(existingPath)) {
				if(callback != null) {
					callback.BeginInvoke(existingPath, null, null);
				}
				
				return;
			}
			
			DownloadImageAction.BeginInvoke(rxcui, (asyncResult) => {
				
				if(callback != null) {	
					var path = DownloadImageAction.EndInvoke(asyncResult);
					callback(path);
				}
				
			}, null);
		}
		
		public static string DownloadPillboxImage(string rxcui)
		{
			var id = FindPillBoxImage(rxcui);
			if(string.IsNullOrWhiteSpace(id)) {
				return null;
			}
			
			var client = new RestClient("http://pillbox.nlm.nih.gov/assets/medium/");
			var request = new RestRequest(string.Format("{0}md.jpg", id));
			var response = client.DownloadData(request);
			
			if(response == null) {
				return null;	
			}
			
			var path = Path.Combine(MyDocuments.FullName, rxcui + ".jpg");
			File.WriteAllBytes(path, response);
			
			return path;
		}
		
		public static string FindPillBoxImage(string rxcui)
		{
			string imageId = string.Empty;
			
			using(TextReader rdr = new StreamReader("PillBoxImageDataSource.xml")) {
				var doc = XDocument.Load(rdr);
				var result = doc.Root.Elements("pill").Where(x => x.Element("RXCUI").Value == rxcui);
				
				if(result.Any()) {
					imageId = result.First().Element("image_id").Value;
				}
			}
			
			return imageId;
		}
		
		public static IEnumerable<Pill> DeserializePills(string content)
		{
			var doc = XDocument.Parse(content);
			
			var pills = doc.Root.Elements("pill").Select(x => new Pill {
				SplId = x.Element("SPL_ID").Value,
				RxCUI = x.Element("RXCUI").Value,
				RxString = x.Element("RXSTRING").Value,
				Ingredients = x.Element("INGREDIENTS").Value,
				ImageId = x.Element("image_id").Value,
			});
			
			return pills;
		}
		
		
		public static IEnumerable<Pill> LookupByNdcCode(string ndcCode)
		{
			var pills = Enumerable.Empty<Pill>();
			
			try {
				if(string.IsNullOrWhiteSpace(ndcCode))
					return null;
				
				var req = new RestRequest("");
				req.AddParameter("prodcode", ndcCode);
				
				var response = Client.Execute(req);
				
				if(!string.IsNullOrWhiteSpace(response.Content) || response.StatusCode != System.Net.HttpStatusCode.OK)
					return null;
				
				pills = DeserializePills(response.Content.CleanupDirtyXml());
			} 
			catch (Exception ex) {
				Console.WriteLine("Problem getting data for code: {0} -- Exception: {1}", ndcCode, ex.Message + ex.StackTrace);
			}
			
			return pills;	
		}
		
		
		public static IEnumerable<Pill> LookupByIngredient(string ingredient)
		{
			var pills = Enumerable.Empty<Pill>();
			
			try {
				
				if(string.IsNullOrWhiteSpace(ingredient))
					return null;
				
				var request = new RestRequest("");
				request.AddParameter("ingredient", ingredient);
				
				var response = Client.Execute(request);
				
				if(string.IsNullOrWhiteSpace(response.Content) || response.StatusCode != System.Net.HttpStatusCode.OK) {
					return null;
				}
				
				pills = DeserializePills(response.Content.CleanupDirtyXml());
				
			} catch (Exception ex) {
				Console.WriteLine("Problem getting data for ingredient: {0} -- Exception: {1}", ingredient, ex.Message + ex.StackTrace);
			}
			
			return pills;
		}
		
	}
	
	public static class XmlCleanerUpper
	{
		public static string CleanupDirtyXml(this string content)
		{
			return HttpUtility.HtmlDecode(content)
					.Replace(@"&nbsp;", "&#160;")
					.Replace("&", "&amp;");
		}
	}
		
			
}

