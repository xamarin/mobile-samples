using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Environment = System.Environment;

namespace SQLite.Android.Sample
{
	[Activity(Label = "SQLite.Android.Sample", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		internal static SQLiteAsyncConnection DB;
		private string selectedId;
		private readonly TaskScheduler uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			RequestWindowFeature (WindowFeatures.ActionBar);
			RequestWindowFeature (WindowFeatures.Progress);
			RequestWindowFeature (WindowFeatures.IndeterminateProgress);

			SetProgressBarIndeterminate (true);
			SetProgressBarIndeterminateVisibility (true);

			ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
			ActionBar.Title = "Tasks Example";
			ActionBar.SetDisplayShowTitleEnabled (true);

			if (bundle != null)
				this.selectedId = bundle.GetString ("selected");

			DB = new SQLiteAsyncConnection (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "todo.db"));

			// Create our tables
			DB.CreateTablesAsync<List, ListItem>().ContinueWith (t => LoadLists(), uiScheduler);
		}

		private void LoadLists()
		{
			// Query for all lists and display them in the ActionBar
			SetProgressBarIndeterminateVisibility (true);

			DB.QueryAsync<List> ("SELECT * FROM List").ContinueWith (t => {
				SetProgressBarIndeterminateVisibility (false);

				ActionBar.RemoveAllTabs();
				ActionBar.Tab[] tabs = t.Result.Select (AddListTab).ToArray();

				if (tabs.Length == 0)
					return;

				var tab = tabs[0];
				if (this.selectedId != null)
					tab = tabs.FirstOrDefault (ft => (string)ft.Tag == this.selectedId) ?? tab;

				ActionBar.SelectTab (tab);
			}, uiScheduler);
		}

		public void CreateList (string name)
		{
			SetProgressBarIndeterminateVisibility (true);

			var list = new List { Name = name };
			DB.InsertAsync (list).ContinueWith (t => {
				SetProgressBarIndeterminateVisibility (false);
				AddListTab (list, select: true);
			}, uiScheduler);
		}

		private ActionBar.Tab AddListTab (List list)
		{
			return AddListTab (list, false);
		}

		private ActionBar.Tab AddListTab (List list, bool select)
		{
			var listTab = ActionBar.NewTab();
			listTab.SetText (list.Name);
			listTab.SetTag (list.Id);

			ListFragment existing = (ListFragment)FragmentManager.FindFragmentByTag (list.Id.ToString());
			listTab.SetTabListener (new TabListener<ListFragment> (this, list.Id.ToString(), existing));

			ActionBar.AddTab (listTab, select);

			return listTab;
		}

		// Keep track of what list we had selected
		protected override void OnSaveInstanceState (Bundle outState)
		{
			if (ActionBar.SelectedTab != null)
				outState.PutString ("selected", (string)ActionBar.SelectedTab.Tag);

			base.OnSaveInstanceState (outState);
		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			IMenuItem createItem = menu.Add ("Create List");
			createItem.SetShowAsAction (ShowAsAction.IfRoom);
			createItem.SetOnMenuItemClickListener (new DelegatedMenuItemListener (OnCreateClicked));

			return true;
		}

		private bool OnCreateClicked (IMenuItem menuItem)
		{
			AlertDialog.Builder builder = new AlertDialog.Builder (this);
			builder.SetTitle ("Create List");

			EditText nameInput = (EditText)LayoutInflater.Inflate (Resource.Layout.NameDialog, null);

			builder.SetView (nameInput);
			builder.SetPositiveButton ("Create", (sender, args) => CreateList (nameInput.Text));
			builder.SetNegativeButton ("Cancel", (IDialogInterfaceOnClickListener)null);

			AlertDialog dialog = builder.Create();
			dialog.Show();

			return true;
		}
	}
}

