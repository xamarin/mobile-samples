using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using System.Diagnostics;
#if SILVERLIGHT
using System.IO.IsolatedStorage;
#endif

namespace MWC.SAL
{
	public abstract class XmlFeedParserBase<T>
	{
#if !SILVERLIGHT
		readonly string _documents = Environment.GetFolderPath (Environment.SpecialFolder.Personal);		
#endif
        string _localPath;
		string _url;

		List<T> _items = new List<T>();

		public XmlFeedParserBase (string url, string filename)
		{
			Debug.WriteLine ("Url: " + url);
			_url = url;

#if SILVERLIGHT
            _localPath = filename;
#else
			_localPath = Path.Combine (_documents, filename);
#endif
			
			if (HasLocalData) {
                var data = OpenLocal ();
                _items = ParseXml (data);
			}
		}

		public List<T> AllItems {
			get { return _items; }
		}

        string OpenLocal ()
        {
#if SILVERLIGHT
            var iso = IsolatedStorageFile.GetUserStoreForApplication ();
            using (var f = new StreamReader (iso.OpenFile (_localPath, FileMode.Open))) {
                return f.ReadToEnd ();
            }
#else
            using (var f = File.OpenText (_localPath)) {
                return f.ReadToEnd ();
            }
#endif
        }

		void SaveLocal (string data)
		{
#if SILVERLIGHT
            var iso = IsolatedStorageFile.GetUserStoreForApplication ();
            using (var f = new StreamWriter (iso.CreateFile (_localPath))) {
                f.Write (data);
            }
#else
            using (var f = new StreamWriter (_localPath)) {
                f.Write (data);
            }
#endif
		}

		public DateTime GetLastRefreshTimeUtc ()
		{
			if (HasLocalData) {
#if SILVERLIGHT
                return IsolatedStorageFile.GetUserStoreForApplication ().GetLastWriteTime (_localPath).UtcDateTime;
#else
                return new FileInfo (_localPath).LastWriteTimeUtc;
#endif
            } else {
				return new DateTime (1990, 1, 1);
			}
		}

		public bool HasLocalData {
			get { 
#if SILVERLIGHT
                return IsolatedStorageFile.GetUserStoreForApplication ().FileExists (_localPath);
#else
                return File.Exists (_localPath); 
#endif
            }
		}

		public void Refresh (Action action)
		{			
			var webClient = new WebClient ();
			Debug.WriteLine ("Get remote xml data");
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
