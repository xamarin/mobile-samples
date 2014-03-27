using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.IO;
using Android.Speech.Tts;
using Android.Webkit;
using System.Linq;

namespace RazorTodo
{
	[Activity (Label = "RazorTodo", MainLauncher = true)]
	public class Activity1 : Activity
	{
		// I apologize in advance for this awful hack, I'm sure there's a better way...
		public static Activity1 SpeakingActivityContext; 

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			#region Database setup
			var sqliteFilename = "TodoSQLite.db3";
			string documentsPath = System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal); // Documents folder
			var path = Path.Combine(documentsPath, sqliteFilename);

			// This is where we copy in the prepopulated database
			Console.WriteLine (path);
			if (!File.Exists(path))
			{
				var s = Resources.OpenRawResource(RazorTodo.Resource.Raw.TodoSQLite);  // RESOURCE NAME ###

				// create a write stream
				FileStream writeStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
				// write to the stream
				ReadWriteStream(s, writeStream);
			}

			var plat = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
			var conn = new SQLite.Net.SQLiteConnection(plat, path);

			// Set the database connection string
			App.SetDatabaseConnection (conn);
			#endregion

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			var webView = FindViewById<WebView> (Resource.Id.webView);
			webView.Settings.JavaScriptEnabled = true;

			// Use subclassed WebViewClient to intercept hybrid native calls
			webView.SetWebViewClient (new RazorWebViewClient (this));

			// Render the view from the type generated from RazorView.cshtml
			var model = App.Database.GetItems ().ToList();
			var template = new TodoList () { Model = model };
			var page = template.GenerateString ();

			// Load the rendered HTML into the view with a base URL 
			// that points to the root of the bundled Assets folder
			webView.LoadDataWithBaseURL("file:///android_asset/", page, "text/html", "UTF-8", null);
		}

		/// <summary>
		/// helper method to get the database out of /raw/ and into the user filesystem
		/// </summary>
		void ReadWriteStream(Stream readStream, Stream writeStream)
		{
			int Length = 256;
			Byte[] buffer = new Byte[Length];
			int bytesRead = readStream.Read(buffer, 0, Length);
			// write the required bytes
			while (bytesRead > 0)
			{
				writeStream.Write(buffer, 0, bytesRead);
				bytesRead = readStream.Read(buffer, 0, Length);
			}
			readStream.Close();
			writeStream.Close();
		}
	}
}


