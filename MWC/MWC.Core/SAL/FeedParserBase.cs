using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using System.Diagnostics;
using System.Xml;


#if SILVERLIGHT
using System.IO.IsolatedStorage;
#endif

namespace MWC.SAL {
	public abstract class FeedParserBase<T> {
#if !SILVERLIGHT
		readonly string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);	
#endif
        string localPath;
		string documentUrl;

		List<T> items = new List<T>();

		public FeedParserBase (string url, string filename)
		{
			Debug.WriteLine ("Url: " + url);
			documentUrl = url;

#if SILVERLIGHT
            localPath = filename;
#else
			
#if __ANDROID__
            string libraryPath = documentsPath;
#else
            // we need to put in /tmp/ on iOS5.1 to meet Apple's iCloud terms (don't want this backed-up)
			string libraryPath = Path.Combine (documentsPath, "../tmp/");
#endif
            localPath = Path.Combine (libraryPath, filename); // iOS or Android
#endif
            Debug.WriteLine("FeedParserBase path:" + localPath);
			
			if (HasLocalData) {
                var data = OpenLocal ();
                items = Parse (data);
			}
		}

		public virtual List<T> AllItems {
			get { return items; }
		}

        string OpenLocal ()
        {
#if SILVERLIGHT
            var iso = IsolatedStorageFile.GetUserStoreForApplication ();
            using (var f = new StreamReader (iso.OpenFile (localPath, FileMode.Open))) {
                return f.ReadToEnd ();
            }
#else
            using (var f = File.OpenText (localPath)) {
                return f.ReadToEnd ();
            }
#endif
        }

		protected void SaveLocal (string data)
		{
#if SILVERLIGHT
            var iso = IsolatedStorageFile.GetUserStoreForApplication ();
            using (var f = new StreamWriter (iso.CreateFile (localPath))) {
                f.Write (data);
            }
#else
            using (var f = new StreamWriter (localPath)) {
                f.Write (data);
            }
#endif
		}

		public DateTime GetLastRefreshTimeUtc ()
		{
			if (HasLocalData) {
#if SILVERLIGHT
                return IsolatedStorageFile.GetUserStoreForApplication ().GetLastWriteTime (localPath).UtcDateTime;
#else
                return new FileInfo (localPath).LastWriteTimeUtc;
#endif
            } else {
				return new DateTime (1990, 1, 1);
			}
		}

		public bool HasLocalData {
			get { 
#if SILVERLIGHT
                return IsolatedStorageFile.GetUserStoreForApplication ().FileExists (localPath);
#else
                return File.Exists (localPath); 
#endif
            }
		}

		public virtual void Refresh (Action action)
		{
			var webClient = new WebClient ();
			Debug.WriteLine ("Get remote data");
			webClient.DownloadStringCompleted += (sender, e) =>
			{
				try {
					SaveLocal (e.Result);
					items = Parse (e.Result);
				} catch (Exception ex) {
					Debug.WriteLine ("ERROR saving downloaded data: " + ex);
				}
				action();
			};
			webClient.Encoding = System.Text.Encoding.UTF8;
			webClient.DownloadString (new Uri(documentUrl));
		}

		protected abstract List<T> Parse (string data);
		
		protected static DateTime ParseDateTime (string date)
		{
			var ps = date.Split (' ');
			var justDt = string.Join (" ", ps.Skip (1).Take (4).ToArray ());
			return DateTime.Parse (justDt);
		}
	}
}
