using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using System.Diagnostics;

namespace MWC.SAL
{
	public abstract class XmlFeedParserBase<T>
	{
		readonly string _documents = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
		
		string _localPath;
		string _url;

		List<T> _items = new List<T>();

		public XmlFeedParserBase (string url, string filename)
		{
			Debug.WriteLine ("Url: " + url);
			_url = url;
			_localPath = Path.Combine (_documents, filename);
			
			if (HasLocalData) {
				_items = ParseXml (File.ReadAllText (_localPath));
			}
		}

		public List<T> AllItems {
			get { return _items; }
		}

		void SaveLocal (string data)
		{
			File.WriteAllText (_localPath, data);
		}

		public DateTime GetLastRefreshTimeUtc ()
		{
			if (HasLocalData) {
				return new FileInfo (_localPath).LastWriteTimeUtc;
			} else {
				return new DateTime (1990, 1, 1);
			}
		}

		public bool HasLocalData {
			get { return File.Exists (_localPath); }
		}

		public void Refresh (Action action)
		{			
			var webClient = new WebClient ();
			Debug.WriteLine ("Get remote data");
			webClient.DownloadStringCompleted += (sender, e) =>
			{
				try {
					SaveLocal (e.Result);
					_items = ParseXml (e.Result);
				} catch (Exception ex) {
					Debug.WriteLine ("ERROR saving downloaded XML: " + ex);
				}
				action();
			};
			webClient.Encoding = System.Text.Encoding.UTF8;
			webClient.DownloadStringAsync (new Uri (_url));
		}

		protected abstract List<T> ParseXml (string xml);
		
		protected static DateTime ParseDateTime (string date)
		{
			var ps = date.Split (' ');
			var justDt = string.Join (" ", ps.Skip (1).Take (4).ToArray ());
			return DateTime.Parse (justDt);
		}
	}
}
