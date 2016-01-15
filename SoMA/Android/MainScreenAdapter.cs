using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using Core;

namespace Droid
{
	public class MainScreenAdapter : BaseAdapter<ShareItem> {
		protected Activity context;
		protected IList<ShareItem> items = new List<ShareItem>();

		public override ShareItem this[int index] {
			get {
				return items[index];
			}
		}

		public override int Count {
			get {
				return items.Count;
			}
		}

		public MainScreenAdapter (Activity context, IList<ShareItem> items)
		{
			this.context = context;
			this.items = items;
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			// Get our object for position
			var item = items[position];

			var view = convertView ?? context.LayoutInflater.Inflate (Resource.Layout.PhotoRow, null);

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

