using System;
using Tasky.Portable;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;

namespace Tasky
{
	public class XmlStorage : IXmlStorage
	{
		public XmlStorage ()
		{
		}

		public List<Task> ReadXml (string filename)
		{
			if (File.Exists (filename)) {
				var serializer = new XmlSerializer (typeof(List<Task>));
				using (var stream = new FileStream (filename, FileMode.Open)) {
					return (List<Task>)serializer.Deserialize (stream);
				}
			}
			return new List<Task> ();
		}

		public void WriteXml (List<Task> tasks, string filename)
		{
			var serializer = new XmlSerializer (typeof(List<Task>));
			using (var writer = new StreamWriter (filename)) {
				serializer.Serialize (writer, tasks);
			}
		}
	}
}

