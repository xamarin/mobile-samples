using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Xamarin.Edu.ContentRepository.Web.Services.Models;

namespace Xamarin.Edu.ContentRepository.Web.Services.ConsoleTest
{
	class Program
	{
		static string apiBase = "http://localhost:50150";

		static void Main(string[] args)
		{
			
			Console.WriteLine("XML:");
			GetUser(Helpers.Serializer.SerializationType.XML);

			Console.WriteLine("Json:");
			GetUser(Helpers.Serializer.SerializationType.Json);

			Console.WriteLine("Jsonp:");
			GetUser(Helpers.Serializer.SerializationType.Jsonp);
			
			Console.WriteLine("Done!");
			Console.ReadLine();
		}

		static void GetUser(Xamarin.Edu.ContentRepository.Web.Services.Helpers.Serializer.SerializationType type)
		{
			int id = 1;

			User user = null;
			string url = string.Format("{0}/User/GetUser/{1}?format={2}", apiBase, id.ToString(), Enum.GetName(typeof(Xamarin.Edu.ContentRepository.Web.Services.Helpers.Serializer.SerializationType), type));
			Console.WriteLine(url);

			try
			{
				HttpWebResponse response = GetWebResponse(url);

				if(response != null)
				{
					using(StreamReader sr = new StreamReader(response.GetResponseStream()))
					{
						string result = sr.ReadToEnd();
						Console.WriteLine(result);
						user = Xamarin.Edu.ContentRepository.Web.Services.Helpers.Serializer.Deserialize<User>(result, type);
					}
				}
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			if(user != null)
			{
				Console.WriteLine(user.FirstName);
				Console.WriteLine(user.LastName);
				Console.WriteLine(user.EmailAddress);
			}
			else
			{
				Console.WriteLine("oops, it's null, something went terribly wrong...");
			}

			Console.WriteLine("Call complete\n");
		}

		static HttpWebResponse GetWebResponse(string url)
		{
			//string authInfo = username + ":" + password;
			//authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));

			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
			//request.Headers["Authorization"] = "Basic " + authInfo;

			HttpWebResponse response = null;
			try
			{
				response = (HttpWebResponse)request.GetResponse();
			}
			catch(System.Net.WebException webException)
			{
				Console.WriteLine("ERROR getting response stream from: {0}", url);
				Console.Write(webException.Message);
			}
			return response;
		}
	}
}
