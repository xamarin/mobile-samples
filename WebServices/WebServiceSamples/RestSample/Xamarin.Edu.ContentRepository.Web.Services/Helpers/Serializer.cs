using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Web.Script.Serialization;

namespace Xamarin.Edu.ContentRepository.Web.Services.Helpers
{
	public static class Serializer
	{
		// default callback wrapper name for Jsonp
		const string defaultCallback = "Callback";

		// xml namespace
		const string xmlNamespace = "http://api.xamarin.edu/";

		// serialization types that we currently handle
		public enum SerializationType
		{
			XML,
			Json,
			Jsonp
		}

		/// <summary>
		/// Single serialization method to handle the serialization types, optional callback parameter to specify jsonp callback name.
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public static string Serialize(object obj, SerializationType type, string jsonpCallback = "")
		{
			// here we'll serialize based on the type.  

			switch(type)
			{
				case SerializationType.Json:
					return SerializeJson(obj);
				case SerializationType.Jsonp:
					if(jsonpCallback == "")
					{
						jsonpCallback = defaultCallback;
					}
					return SerializeJsonp(obj, defaultCallback);
				case SerializationType.XML:
					return SerializeXML(obj);
				default:
					throw new ArgumentNullException();
			}
		}

		/// <summary>
		/// Single method to deserialize based on the deserialization type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="data"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public static T Deserialize<T>(string data, SerializationType type)
		{
			switch(type)
			{
				case SerializationType.Json:
					return DeserializeJson<T>(data);
				case SerializationType.Jsonp:
					return DeserializeJsonp<T>(data);
				case SerializationType.XML:
					return DeserializeXML<T>(data);
				default:
					throw new ArgumentNullException();
			}
		}

		#region Jsonp private serialize/deserialize methods
		private static string SerializeJsonp(object obj, string callback)
		{
			string json = SerializeJson(obj);
			return string.Format("{0}({1})", callback, json);
		}

		private static T DeserializeJsonp<T>(string jsonp)
		{
			int start = jsonp.IndexOf('(') + 1;
			int end = jsonp.LastIndexOf(')');
			string json = jsonp.Substring(start, end - start);

			return DeserializeJson<T>(json);
		}
		#endregion

		#region Json private serialize/deserialize methods
		private static string SerializeJson(object obj)
		{
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			return serializer.Serialize(obj);

			// DataContractJsonSerializer is causing some issues serializing.  For complex types, it is returning xml, wth.

			//using( MemoryStream ms = new MemoryStream() )
			//{
			//    DataContractJsonSerializer jserializer = new DataContractJsonSerializer( obj.GetType() );
			//    jserializer.WriteObject( ms, obj );
			//    ms.Seek( 0, SeekOrigin.Begin );
			//    StreamReader sr = new StreamReader( ms );
			//    string json = sr.ReadToEnd();
			//    sr.Close();
			//    return json;
			//}
		}

		private static T DeserializeJson<T>(string json)
		{
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			return serializer.Deserialize<T>(json);

			//T obj = Activator.CreateInstance<T>();
			//using( MemoryStream ms = new MemoryStream( Encoding.Unicode.GetBytes( json ) ) )
			//{
			//    System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer( obj.GetType() );
			//    obj = (T)serializer.ReadObject( ms );
			//    ms.Close();
			//    ms.Dispose();
			//    return obj;
			//}
		}
		#endregion

		#region XML private serialize/deserialize methods
		private static string SerializeXML(object obj)
		{
			XmlSerializer serializer = new XmlSerializer(obj.GetType(), xmlNamespace);
			using(MemoryStream ms = new MemoryStream())
			{
				XmlWriter writer = XmlWriter.Create(ms);
				serializer.Serialize(writer, obj);
				writer.Flush();
				writer.Close();

				ms.Position = 0;
				StreamReader reader = new StreamReader(ms);

				string xml = reader.ReadToEnd();
				return xml;
			}
		}

		private static T DeserializeXML<T>(string xml)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(T), xmlNamespace);
			using(StringReader stringReader = new StringReader(xml))
			{
				XmlReader reader = XmlReader.Create(stringReader);
				object obj = serializer.Deserialize(reader);
				return (T)obj;
			}
		}
		#endregion

	}

}