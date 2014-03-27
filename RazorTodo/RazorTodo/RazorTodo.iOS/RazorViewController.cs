using System;
using MonoTouch.UIKit;
using RazorTodo;
using System.Collections.Generic;
using MonoTouch.Foundation;
using System.Linq;
using MonoTouch.Twitter;
using MonoTouch.MessageUI;

namespace RazorTodo
{
	public class RazorViewController : UIViewController
	{
		public RazorViewController ()
		{
		}
		UIWebView webView;
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			webView = new UIWebView (UIScreen.MainScreen.Bounds);
			View.Add (webView);

			// Intercept URL loading to handle native calls from browser
			webView.ShouldStartLoad += HandleShouldStartLoad;

			// Render the view from the type generated from RazorView.cshtml

			var model = App.Database.GetItems ().ToList ();
			var template = new TodoList () { Model = model };
			var page = template.GenerateString ();
			webView.LoadHtmlString (page, NSBundle.MainBundle.BundleUrl);
		}

		bool HandleShouldStartLoad (UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navigationType) {
			var scheme = "hybrid:";
			// If the URL is not our own custom scheme, just let the webView load the URL as usual
			if (request.Url.Scheme != scheme.Replace(":", ""))
				return true;

			// This handler will treat everything between the protocol and "?"
			// as the method name.  The querystring has all of the parameters.
			var resources = request.Url.ResourceSpecifier.Split('?');
			var method = resources [0];
			var parameters = System.Web.HttpUtility.ParseQueryString(resources[1]); // breaks if ? not present (ie no params)

			if (method == "ListAll") {
				var model = App.Database.GetItems ().ToList();
				var template = new TodoList () { Model = model };
				var page = template.GenerateString ();
				webView.LoadHtmlString (page, NSBundle.MainBundle.BundleUrl);
			}
			else if (method == "AddTask") {
				var template = new TodoView () { Model = new TodoItem() };
				var page = template.GenerateString ();
				webView.LoadHtmlString (page, NSBundle.MainBundle.BundleUrl);
			}
			else if (method == "ViewTask") {
				var id = parameters ["todoid"];
				var model = App.Database.GetItem (Convert.ToInt32 (id));
				var template = new TodoView () { Model = model };
				var page = template.GenerateString ();
				webView.LoadHtmlString (page, NSBundle.MainBundle.BundleUrl);
			} 
			else if (method == "SpeakAll") {
				var todos = App.Database.GetItemsNotDone ();
				var tospeak = "";
				foreach (var t in todos)
					tospeak += t.Name + " ";
				if (tospeak == "")
					tospeak = "there are no tasks to do";
				Speech.Speak (tospeak);
			} else if (method == "TweetAll") {
				var todos = App.Database.GetItemsNotDone ();
				var totweet = "";
				foreach (var t in todos)
					totweet += t.Name + ",";
				if (totweet == "")
					totweet = "there are no tasks to tweet";
				else 
					totweet = "Still do to:" + totweet;
				var tweetController = new TWTweetComposeViewController ();
				tweetController.SetInitialText (totweet); 
				PresentModalViewController (tweetController, true);
			} else if (method == "TextAll") {
				if (MFMessageComposeViewController.CanSendText) {

					var todos = App.Database.GetItemsNotDone ();
					var totext = "";
					foreach (var t in todos)
						totext += t.Name + ",";
					if (totext == "")
						totext = "there are no tasks to text";

					MFMessageComposeViewController message =
						new MFMessageComposeViewController ();
					message.Finished += (sender, e) => {
						e.Controller.DismissViewController (true, null);
					};
					//message.Recipients = new string[] { receiver };
					message.Body = totext;
					PresentModalViewController (message, true);
				} else {
					new UIAlertView ("Sorry", "Cannot text from this device", null, "OK", null).Show ();
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

					var model = App.Database.GetItems ().ToList ();
					var template = new TodoList () { Model = model };
					var page = template.GenerateString ();
					webView.LoadHtmlString (page, NSBundle.MainBundle.BundleUrl);
				} else if (button == "Delete") {
					var id = parameters ["id"];

					App.Database.DeleteItem (Convert.ToInt32 (id));

					var model = App.Database.GetItems ().ToList ();
					var template = new TodoList () { Model = model };
					var page = template.GenerateString ();
					webView.LoadHtmlString (page, NSBundle.MainBundle.BundleUrl);
				} else if (button == "Cancel") {
					var model = App.Database.GetItems ().ToList ();
					var template = new TodoList () { Model = model };
					var page = template.GenerateString ();
					webView.LoadHtmlString (page, NSBundle.MainBundle.BundleUrl);
				} else if (button == "Speak") {
					var name = parameters ["name"];
					var notes = parameters ["notes"];
					Speech.Speak (name + " " + notes);
				}
			}
			return false;
		}
	}
}

