using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace SQLite.Android.Sample
{
	public class ListFragment
		: Fragment
	{
		private ListView list;
		private readonly TaskScheduler uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();
		
		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			if (this.list == null) {
				this.list = new ListView (Activity);
				this.list.LayoutParameters = new ViewGroup.LayoutParams (
					ViewGroup.LayoutParams.FillParent, ViewGroup.LayoutParams.WrapContent);
			}

			return this.list;
		}

		public override void OnDestroyView()
		{
			this.list = null;
			base.OnDestroyView();
		}

		public override void OnCreate (Bundle savedInstanceState)
		{
			SetHasOptionsMenu (true);

			base.OnCreate (savedInstanceState);
		}

		public override void OnStart()
		{
			Refresh();

			base.OnStart();
		}

		public override void OnCreateOptionsMenu (IMenu menu, MenuInflater inflater)
		{
			IMenuItem add = menu.Add ("Create Item");
			add.SetShowAsAction (ShowAsAction.Always);
			add.SetOnMenuItemClickListener (new DelegatedMenuItemListener (OnAddItemClicked));
		}

		private bool OnAddItemClicked (IMenuItem menuItem)
		{
			AlertDialog.Builder builder = new AlertDialog.Builder (Activity);
			builder.SetTitle ("Create Item");

			EditText nameInput = (EditText)Activity.LayoutInflater.Inflate (Resource.Layout.NameDialog, null);

			builder.SetView (nameInput);
			builder.SetPositiveButton ("Add", (sender, args) => CreateItem (nameInput.Text));
			builder.SetNegativeButton ("Cancel", (IDialogInterfaceOnClickListener)null);

			AlertDialog dialog = builder.Create();
			dialog.Show();

			return true;
		}

		private class ListItemsAdapter
			: BaseAdapter
		{
			private readonly Activity activity;
			private ListItem[] listItems;
			private readonly TaskScheduler uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();

			public ListItemsAdapter (Activity activity, IEnumerable<ListItem> listItems)
			{
				if (activity == null)
					throw new ArgumentNullException ("activity");
				if (listItems == null)
					throw new ArgumentNullException ("listItems");

				this.activity = activity;
				this.listItems = listItems.ToArray();
			}

			public override Java.Lang.Object GetItem (int position)
			{
				return null;
			}

			public override long GetItemId (int position)
			{
				return position;
			}

			public override View GetView (int position, View convertView, ViewGroup parent)
			{
				ListItem ListItem = this.listItems[position];

				bool existing = (convertView != null);
				CheckBox view = (CheckBox)(convertView ?? this.activity.LayoutInflater.Inflate (Resource.Layout.ListItem, null));
				if (existing)
					view.CheckedChange -= OnChecked;

				view.Text = ListItem.Text;
				view.Checked = ListItem.Completed;
				view.Tag = ListItem.Id;
				view.CheckedChange += OnChecked;

				return view;
			}

			private void OnChecked (object sender, CompoundButton.CheckedChangeEventArgs e)
			{
				CheckBox checkBox = (CheckBox)sender;
				int id = (int)checkBox.Tag;

				ListItem existing = this.listItems.First (i => i.Id == id);
				existing.Completed = e.IsChecked;

				MainActivity.DB.UpdateAsync (existing).ContinueWith (ut => {
					this.listItems = this.listItems.OrderBy (i => i.Completed).ToArray();
					NotifyDataSetChanged();
				}, this.uiScheduler);
			}

			public override int Count
			{
				get { return this.listItems.Length; }
			}
		}

		private void Refresh()
		{
			Activity.SetProgressBarIndeterminateVisibility (true);

			int listId = Int32.Parse (Tag);
			MainActivity.DB.QueryAsync<ListItem> ("SELECT * FROM ListItem WHERE (ListId=?)", listId).ContinueWith (t => {
				Activity.SetProgressBarIndeterminateVisibility (false);

				var activity = Activity;
				if (activity == null)
					return;

				if (this.list == null)
					return;

				this.list.Adapter = new ListItemsAdapter (activity, t.Result);
			}, this.uiScheduler);
		}

		private void CreateItem (string text)
		{
			var item = new ListItem { Text = text, ListId = Int32.Parse (Tag) };
			MainActivity.DB.InsertAsync (item).ContinueWith (t => Refresh(), uiScheduler);
		}
	}
}