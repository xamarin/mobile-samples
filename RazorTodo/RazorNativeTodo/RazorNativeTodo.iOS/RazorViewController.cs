using System;
using UIKit;
using RazorNativeTodo;
using System.Collections.Generic;
using Foundation;
using System.Linq;
using Twitter;
using MessageUI;

namespace RazorNativeTodo
{
	public class RazorViewController : UIViewController
	{
		public RazorViewController ()
		{
			Title = "Todo Item";
			webView = new UIWebView (UIScreen.MainScreen.Bounds);
		}
		public UIWebView webView;
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			View.Add (webView);

			// Intercept URL loading to handle native calls from browser
			webView.ShouldStartLoad += HandleShouldStartLoad;

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

			if (method == "") {
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
				PresentViewController (tweetController, true, null);
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
					PresentViewController (message, true, null);
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
					NavigationController.PopToRootViewController (true);

				} else if (button == "Delete") {
					var id = parameters ["id"];

					App.Database.DeleteItem (Convert.ToInt32 (id));
					NavigationController.PopToRootViewController (true);

				} else if (button == "Cancel") {
					NavigationController.PopToRootViewController (true);

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

