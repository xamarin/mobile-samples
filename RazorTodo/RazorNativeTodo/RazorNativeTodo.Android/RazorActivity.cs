using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.IO;
using Android.Speech.Tts;
using RazorNativeTodo;
using Android.Webkit;
using System.Linq;

namespace RazorNativeTodo
{
	[Activity (Label = "RazorNativeTodo")]
	public class RazorActivity : Activity
	{
		// I apologize in advance for this awful hack, I'm sure there's a better way...
		public static RazorActivity SpeakingActivityContext; 

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			var webView = FindViewById<WebView> (Resource.Id.webView);
			webView.Settings.JavaScriptEnabled = true;

			// Use subclassed WebViewClient to intercept hybrid native calls
			webView.SetWebViewClient (new RazorWebViewClient (this));

			int todoid = Intent.GetIntExtra("todoid", 0);
			TodoItem model = new TodoItem ();
			if (todoid > 0)
				model = App.Database.GetItem (Convert.ToInt32 (todoid));

			var template = new TodoView () { Model = model };
			var page = template.GenerateString ();
			webView.LoadDataWithBaseURL("file:///android_asset/", page, "text/html", "UTF-8", null);
		}


	}
}


