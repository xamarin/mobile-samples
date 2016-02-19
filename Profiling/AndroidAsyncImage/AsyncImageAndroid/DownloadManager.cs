using System;
using System.Net;
using System.Threading.Tasks;
using System.IO;

namespace AsyncImageAndroid
{
	/// <summary>
	/// We introduce this class to incapsulate net downloading technology. For this particular case we will use WebClient.
	/// This class is bad designed because have multiple responsibilities:
	/// 1. Download data from net
	/// 2. Loggin feature – EventHandler method. With this method you can subscribe on all events and log arguments with timestamp
	/// </summary>
	public class DownloadManager
	{
		public event DownloadProgressChangedEventHandler DownloadProgressChanged;

		WebClient webClient;
		// Memory Leak here. If you reuse DownloadManager it will hold downladed data and not dispose.
		// use local variable instead field.
		//TODO comment out the line below to fix memory leak
		byte[] bytes;

		string DocumentsFolder {
			get {
				return System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal);
			}
		}

		public async Task<string> DownloadAsync(string downloadUrl, string fileName)
		{
			webClient = new WebClient ();
			webClient.DownloadProgressChanged += HandleDownloadProgressChanged;

			var url = new Uri (downloadUrl);
 			//TODO uncomment the line below to fix memory leak
			//byte[] bytes = await webClient.DownloadDataTaskAsync(url);

			//TODO comment out the line below to fix memory leak
			bytes = await webClient.DownloadDataTaskAsync(url);

			string localPath = FetchStorePath (fileName);
			await StoreData (bytes, localPath);

			return localPath;
		}

		async Task StoreData(byte[] data, string pathToStore)
		{
			FileStream fs = new FileStream (pathToStore, FileMode.OpenOrCreate);
			await fs.WriteAsync (data, 0, data.Length);
			fs.Close ();
		}

		string FetchStorePath(string fileName)
		{
			return Path.Combine (DocumentsFolder, fileName);
		}

		void HandleDownloadProgressChanged (object sender, DownloadProgressChangedEventArgs e)
		{
			var handler = DownloadProgressChanged;
			if (handler != null)
				handler (sender, e);
		}

		public void EventHandler(object sender, EventArgs args)
		{
			Console.WriteLine ("sender {0} args {1} Timestamp {2}", sender, args, DateTime.Now);
		}
	}
}
