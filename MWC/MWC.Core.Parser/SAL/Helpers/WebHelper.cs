using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace MWC.SAL.Helpers
{
	public static class WebHelper
	{
		public static string HttpGet(string url)
		{
			int tryCount = 0;
			int maxRetries = 3;
			string result = string.Empty;
			while(true)
			{
				try
				{
					HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
					// get the http response
					HttpWebResponse response = (HttpWebResponse)request.GetResponse();
					using(StreamReader sr = new StreamReader(response.GetResponseStream()))
					{
						result = sr.ReadToEnd();
					}
					break;
				}
				catch(WebException webex)
				{
					if(tryCount < maxRetries)
					{ tryCount++; }
					else
					{
						Console.WriteLine(url);
						throw webex;
					}
				}
			}
			return result;
		}

		public static string HttpPost(string url, string data)
		{
			string result = string.Empty;

			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
				request.Method = "POST";
				request.ContentType = "application/json";

				// prepare the bytes to send to the request stream
				byte[] bytes = Encoding.UTF8.GetBytes(data.ToString());
				request.ContentLength = data.ToString().Length;

				// write the data to the request stream
				using(Stream requestStream = request.GetRequestStream())
				{
					requestStream.Write(bytes, 0, bytes.Length);
				}

				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
				using(StreamReader sr = new StreamReader(response.GetResponseStream()))
				{
					result = sr.ReadToEnd();
				}
			}
			catch(WebException web)
			{
				if(web.Response != null)
				{
					Stream errorStream = ((HttpWebResponse)web.Response).GetResponseStream();
					using(StreamReader errorSr = new StreamReader(errorStream))
					{
						string errorResponse = errorSr.ReadToEnd();
						Console.WriteLine(errorResponse);
					}
				}
				throw web;
			}
			return result;

		}

		public static string Scrape(string url)
		{
			WebClient client = new WebClient();
			client.Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 7.0b; Windows NT 6.0)");

			try
			{
				using(Stream data = client.OpenRead(url))
				{
					using(StreamReader sr = new StreamReader(data))
					{
						return sr.ReadToEnd();
					}
				}
			}
			catch(Exception ex)
			{
				return "Could not retrieve file: \n\n" + ex.Message;
			}
		}
	}
}
