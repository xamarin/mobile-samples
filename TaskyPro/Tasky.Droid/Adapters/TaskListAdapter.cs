using System;
using System.Collections.Generic;
using Android.Widget;
using Tasky.BL;
using Android.App;
using Android;
using Android.Views;

namespace Tasky.Droid.Adapters {
	public class TaskListAdapter : BaseAdapter<Task> {
		protected Activity context = null;
		protected IList<Task> tasks = new List<Task>();
		
		public TaskListAdapter (Activity context, IList<Task> tasks) : base ()
		{
			this.context = context;
			this.tasks = tasks;
		}
		
		public override Task this[int position]
		{
			get { return tasks[position]; }
		}
		
		public override long GetItemId (int position)
		{
			return position;
		}
		
		public override int Count
		{
			get { return tasks.Count; }
		}
		
		public override Android.Views.View GetView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			// Get our object for position
			var item = tasks[position];			
            View view;

            //Try to reuse convertView if it's not  null, otherwise inflate it from our item layout
            // gives us some performance gains by not always inflating a new view
            if (convertView == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.TaskListItem, null);
            }
            else
            {
                view = convertView;
            }

            var nameLabel = view.FindViewById<TextView>(Resource.Id.lblName);
            nameLabel.Text = item.Name;
            var notesLabel = view.FindViewById<TextView>(Resource.Id.lblDescription);
            notesLabel.Text = item.Notes;

            // TODO: set the check.
            var checkMark = view.FindViewById<ImageView>(Resource.Id.checkMark);
            checkMark.Visibility = item.Done ? ViewStates.Visible : ViewStates.Gone;

			//Finally return the view
			return view;
		}
	}
}