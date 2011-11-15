using System;
using System.Collections.Generic;
using Android.Widget;
using Tasky.BL;
using Android.App;
using Android;

namespace TaskyAndroid.Adapters
{
	public class TaskListAdapter : BaseAdapter<Task>
	{
		protected Activity _context = null;
		protected IList<Task> _tasks = new List<Task>();
		
		public TaskListAdapter (Activity context, IList<Task> tasks) : base ()
		{
			this._context = context;
			this._tasks = tasks;
		}
		
		public override Task this[int position]
		{
			get { return this._tasks[position]; }
		}
		
		public override long GetItemId (int position)
		{
			return position;
		}
		
		public override int Count
		{
			get { return this._tasks.Count; }
		}
		
		public override Android.Views.View GetView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{

			// Get our object for this position
			var item = this._tasks[position];			

			//Try to reuse convertView if it's not  null, otherwise inflate it from our item layout
			// This gives us some performance gains by not always inflating a new view
			// This will sound familiar to MonoTouch developers with UITableViewCell.DequeueReusableCell()
			var view = (convertView ?? 
					this._context.LayoutInflater.Inflate(
					Resource.Layout.TaskListItem, 
					parent, 
					false)) as LinearLayout;

			// Find references to each subview in the list item's view
			var txtName = view.FindViewById<TextView>(Resource.Id.txtName);
			var txtDescription = view.FindViewById<TextView>(Resource.Id.txtDescription);

			//Assign this item's values to the various subviews
			txtName.SetText (this._tasks[position].Name, TextView.BufferType.Normal);
			txtDescription.SetText (this._tasks[position].Notes, TextView.BufferType.Normal);

			//Finally return the view
			return view;
		}
	}
}

