using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Core;

namespace Droid
{
	public class MainScreenAdapter : BaseAdapter<ShareItem> {
		protected Activity context = null;
		protected IList<ShareItem> items = new List<ShareItem>();

		public MainScreenAdapter (Activity context, IList<ShareItem> items) : base ()
		{
			this.context = context;
			this.items = items;
		}

		public override ShareItem this[int position]
		{
			get { return items[position]; }
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override int Count
		{
			get { return items.Count; }
		}

		public override Android.Views.View GetView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			// Get our object for position
			var item = items[position];			

			var view = (convertView ?? 
			            context.LayoutInflater.Inflate(Resource.Layout.PhotoRow, null)
			            );

			view.FindViewById<TextView>(Resource.Id.Text1).Text = item.Text;
			view.FindViewById<TextView>(Resource.Id.Text2).Text = item.SocialType;

			// TODO: Fix OutOfMemoryException
//			Bitmap b = BitmapFactory.DecodeFile (item.ThumbImagePath);
//			view.FindViewById<ImageView>(Resource.Id.Image).SetImageBitmap (b);
//			b.Dispose ();
//
			//Finally return the view
			return view;
		}
	}
}

