using System;
using System.Linq;
using System.Threading.Tasks;
using MonoTouch.Dialog;
using MonoTouch.UIKit;

namespace SQLite.iOS.Sample
{
	public partial class TaskListsViewController : DialogViewController
	{
		private readonly SQLiteAsyncConnection db;
		private readonly TaskScheduler uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();

		public TaskListsViewController (SQLiteAsyncConnection connection)
			: base (UITableViewStyle.Plain, null, true)
		{
			this.db = connection;

			Root = new RootElement ("Lists");
			NavigationItem.RightBarButtonItem = new UIBarButtonItem (UIBarButtonSystemItem.Add, OnCreateList);

			RefreshRequested += (s, e) => RefreshAsync();

			RefreshAsync();
		}

		// Load the list of lists and setup their events
		private Task RefreshAsync()
		{
			AppDelegate.AddActivity();

			return this.db.Table<List>().ToListAsync().ContinueWith (t => {
				if (t.Exception != null) {
					BeginInvokeOnMainThread (ReloadComplete);
					AppDelegate.FinishActivity();
					ShowError (t.Exception.Flatten().InnerException);
					return;
				}

				Section section = new Section();
				section.AddAll (t.Result.Select (l =>
					new StringElement (l.Name, () => {
						var tasks = new TasksViewController (this.db, l);
						NavigationController.PushViewController (tasks, true);
					})
				).Cast<Element>());

				InvokeOnMainThread (() => {
					Root.Clear();
					Root.Add (section);

					ReloadComplete();

					AppDelegate.FinishActivity();
				});
			});
		}

		// Create a new list and then navigate to it
		private void CreateList (string name)
		{
			if (String.IsNullOrWhiteSpace (name))
				return;

			AppDelegate.AddActivity();

			var list = new List { Name = name };
			this.db.InsertAsync (list).ContinueWith (t => {
				AppDelegate.FinishActivity();

				RefreshAsync().ContinueWith (rt => {
					var tasks = new TasksViewController (this.db, list);
					NavigationController.PushViewController (tasks, animated: true);
				}, uiScheduler);
			});
		}

		private UIAlertView create;
		private void OnCreateList (object sender, EventArgs eventArgs)
		{
			this.create = new UIAlertView ("Create List", String.Empty, null, "Cancel", "Create") {
				AlertViewStyle = UIAlertViewStyle.PlainTextInput,
			};
			this.create.ShouldEnableFirstOtherButton = a => 
				!String.IsNullOrWhiteSpace (a.GetTextField(0).Text);

			this.create.Clicked += (s, e) => {
				if (e.ButtonIndex != 1)
					return;

				CreateList (this.create.GetTextField (0).Text);
			};

			this.create.Show();
		}

		private UIAlertView alert;
		private void ShowError (Exception exception)
		{
			BeginInvokeOnMainThread (() => {
				if (this.alert != null)
					this.alert.Dispose();

				this.alert = new UIAlertView ("Error", exception.Message, null, "Close");
				this.alert.Show();
			});
		}
	}
}