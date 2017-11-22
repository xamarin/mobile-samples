using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Webkit;

namespace RazorNativeTodo
{
	public class RazorWebViewClient : WebViewClient {

		RazorActivity context;
		public RazorWebViewClient (RazorActivity context){
			this.context = context;
		}
		Speech speech;
		public override bool ShouldOverrideUrlLoading (WebView webView, string url) {
			var scheme = "hybrid:";
			// If the URL is not our own custom scheme, just let the webView load the URL as usual
			if (!url.StartsWith (scheme)) 
				return false;

			// This handler will treat everything between the protocol and "?"
			// as the method name.  The querystring has all of the parameters.
			var resources = url.Substring(scheme.Length).Split('?');
			var method = resources [0];
			var parameters = System.Web.HttpUtility.ParseQueryString(resources[1]);


			if (method == "") {
				var template = new TodoView () { Model = new TodoItem() };
				var page = template.GenerateString ();
				webView.LoadDataWithBaseURL("file:///android_asset/", page, "text/html", "UTF-8", null);
			}
			else if (method == "ViewTask") {
				var id = parameters ["todoid"];
				var model = App.Database.GetItem (Convert.ToInt32 (id));
				var template = new TodoView () { Model = model };
				var page = template.GenerateString ();
				webView.LoadDataWithBaseURL("file:///android_asset/", page, "text/html", "UTF-8", null);
			} else if (method == "TextAll" || method == "TweetAll") {

				var todos = App.Database.GetItemsNotDone ();
				var totext = "";
				foreach (var t in todos)
					totext += t.Name + ",";
				if (totext == "")
					totext = "there are no tasks to share";

				try {
					var intent = new Intent(Intent.ActionSend);
					intent.PutExtra(Intent.ExtraText,totext);
					intent.SetType("text/plain");
					context.StartActivity(Intent.CreateChooser(intent, "Undone Todos"));
				} catch(Exception ex) {
					System.Diagnostics.Debug.WriteLine (ex);
				}

			} else if (method == "TodoView") {
				// the editing form
				var button = parameters ["Button"];
				if (button == "Save") {
					var id = parameters ["id"];
					var name = parameters ["name"];
					var notes = parameters ["notes"];
					var done = parameters ["done"];

					var todo = new TodoItem {
						ID = Convert.ToInt32 (id),
						Name = name,
						Notes = notes,
						Done = (done == "on")
					};

					App.Database.SaveItem (todo);
					context.Finish();

				} else if (button == "Delete") {
					var id = parameters ["id"];

					App.Database.DeleteItem (Convert.ToInt32 (id));
					context.Finish();

				} else if (button == "Cancel") {
					context.Finish();

				} else if (button == "Speak") {
					var name = parameters ["name"];
					var notes = parameters ["notes"];
					if (speech == null)
						speech = new Speech ();
					speech.Speak (context, name + " " + notes);
				}
			}


			return true;
		}
	}
}

