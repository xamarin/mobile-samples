using System;
using System.Linq;
using System.Threading.Tasks;

using MonoTouch.Dialog;
#if __UNIFIED__
using UIKit;
#else
using MonoTouch.UIKit;
#endif

namespace SQLite.iOS.Sample
{
	public class TasksViewController : DialogViewController
	{
		private readonly SQLiteAsyncConnection db;
		private readonly List list;
		private readonly TaskScheduler uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();

		public TasksViewController (SQLiteAsyncConnection connection, List list)
			: base (UITableViewStyle.Plain, null, true)
		{
			if (connection == null)
				throw new ArgumentNullException ("connection");
			if (list == null)
				throw new ArgumentNullException ("list");

			this.db = connection;
			this.list = list;

			var addButton = new UIBarButtonItem (UIBarButtonSystemItem.Add, OnAddItem);
			var refreshButton = new UIBarButtonItem (UIBarButtonSystemItem.Refresh, delegate { Refresh(); });

			// attach a pull-to-refresh
			//RefreshRequested += (s, e) => Refresh();
			// add a refresh button
			NavigationItem.RightBarButtonItems = new []{ addButton, refreshButton };

			Root = new RootElement (list.Name) {
				new Section()
			};

			Refresh();
		}

		// Create an item an add it to the list
		private void CreateItem (string text)
		{
			ListItem listItem = new ListItem {
				Text = text,
				ListId = this.list.Id
			};

			AppDelegate.AddActivity();

			this.db.InsertAsync (listItem).ContinueWith (t => {
				AppDelegate.FinishActivity();
				Refresh();
			});
		}

		// Load the list of items and set up their events
		private void Refresh()
		{
			AppDelegate.AddActivity();

			this.db.QueryAsync<ListItem> ("SELECT * FROM ListItem WHERE (ListId=?)", this.list.Id)
			    .ContinueWith (t => {
					if (t.Exception != null) {
						AppDelegate.FinishActivity();
						ReloadComplete();
						ShowError (t.Exception.Flatten().InnerException);
						return;
					}

					var section = Root.First();
					section.Clear();
				
					section.AddAll (t.Result.Select (item =>
						new StyledStringElement (item.Text, () => OnTap (item)) {
							Accessory = item.Completed ? UITableViewCellAccessory.Checkmark : UITableViewCellAccessory.None
						}
					).Cast<Element>());

					ReloadComplete();

					AppDelegate.FinishActivity();
			    }, uiScheduler);
		}

		// Toggle items as completed when tapped
		private void OnTap (ListItem task)
		{
			AppDelegate.AddActivity();

			task.Completed = !task.Completed;

			this.db.UpdateAsync (task).ContinueWith (t => {
				Refresh();
				AppDelegate.FinishActivity();
			});
		}

		private UIAlertView create;
		private void OnAddItem (object sender, EventArgs e)
		{
			this.create = new UIAlertView ("Create Task", String.Empty, null, "Cancel", "Create") {
				AlertViewStyle = UIAlertViewStyle.PlainTextInput,
			};
			this.create.ShouldEnableFirstOtherButton = a => 
				!String.IsNullOrWhiteSpace (a.GetTextField(0).Text);

			this.create.Clicked += (s, ce) => {
				if (ce.ButtonIndex != 1)
					return;

				CreateItem (this.create.GetTextField (0).Text);
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
