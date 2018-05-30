using System;
using Android.App;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using XamarinTodoQuickStart;

namespace XamarinTodoQuickStart
{
	public class TodoItemAdapter : BaseAdapter<TodoItem>
	{
        // Private Variables
		private Activity activity;
		private int layoutResourceId;
		private List<TodoItem> items = new List<TodoItem>();

        // Constructor
		public TodoItemAdapter(Activity activity, int layoutResourceId)
		{
			this.activity = activity;
			this.layoutResourceId = layoutResourceId;
		}

		// Returns the view for a specific item on the list
		public override View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			var row = convertView;
			var currentItem = this[position];
			CheckBox checkBox;

            if (row == null)
            {
                var inflater = activity.LayoutInflater;
                row = inflater.Inflate(layoutResourceId, parent, false);

                checkBox = row.FindViewById<CheckBox>(Resource.Id.checkTodoItem);

                checkBox.CheckedChange += async (sender, e) => {
                    var cbSender = sender as CheckBox;
                    if (cbSender != null && cbSender.Tag is TodoItemWrapper && cbSender.Checked)
                    {
                        cbSender.Enabled = false;

                        if (activity is TodoActivity)
                            await ((TodoActivity)activity).CheckItem((cbSender.Tag as TodoItemWrapper).TodoItem);
                    }
                };
            } 
            else
            {
                checkBox = row.FindViewById<CheckBox>(Resource.Id.checkTodoItem);
            }

            string displayDate = "missing";
            if (currentItem.CreatedAt.HasValue)
                displayDate = currentItem.CreatedAt.Value.ToShortTimeString();

            // Time created is added to the row/cell
            checkBox.Text = string.Format("{0} - {1}", currentItem.Text, displayDate);
			checkBox.Checked = false;
			checkBox.Enabled = true;
			checkBox.Tag = new TodoItemWrapper(currentItem);

			return row;
		}

		public void Add(TodoItem item)
		{
			items.Add(item);
			NotifyDataSetChanged();
		}

		public void Clear()
		{
			items.Clear();
			NotifyDataSetChanged();
		}

		public void Remove(TodoItem item)
		{
			items.Remove(item);
			NotifyDataSetChanged();
		}

		#region implemented abstract members of BaseAdapter

		public override long GetItemId(int position)
		{
			return position;
		}

		public override int Count 
        { 
            get { return items.Count; } 
        }

		public override TodoItem this[int position] 
        {
			get { return items[position]; }
		}

		#endregion
	}
}

