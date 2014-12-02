using System;
using System.Threading.Tasks;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace MediaPickerSample
{
	[Activity]
	public class ImageActivity
		: Activity
	{
		private string path;
		private ImageView image;
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			this.image = new ImageView (this);
			this.image.SetScaleType (ImageView.ScaleType.CenterInside);
			SetContentView (this.image, new ViewGroup.LayoutParams (ViewGroup.LayoutParams.FillParent, ViewGroup.LayoutParams.FillParent));

			this.path = (savedInstanceState ?? Intent.Extras).GetString ("path");
			Title = System.IO.Path.GetFileName (this.path);

			Cleanup();
			DecodeBitmapAsync (path, 400, 400).ContinueWith (t => {
				this.image.SetImageBitmap (this.bitmap = t.Result);
			}, TaskScheduler.FromCurrentSynchronizationContext());
		}

		protected override void OnSaveInstanceState (Bundle outState)
		{
			outState.PutString ("path", this.path);
			base.OnSaveInstanceState (outState);
		}

		private static Task<Bitmap> DecodeBitmapAsync (string path, int desiredWidth, int desiredHeight)
		{
			return Task.Factory.StartNew (() => {
				BitmapFactory.Options options = new BitmapFactory.Options();
				options.InJustDecodeBounds = true;
				BitmapFactory.DecodeFile (path, options);

				int height = options.OutHeight;
				int width = options.OutWidth;

				int sampleSize = 1;
				if (height > desiredHeight || width > desiredWidth) {
					int heightRatio = (int)Math.Round ((float)height / (float)desiredHeight);
					int widthRatio = (int)Math.Round ((float)width / (float)desiredWidth);
					sampleSize = Math.Min (heightRatio, widthRatio);
				}

				options = new BitmapFactory.Options();
				options.InSampleSize = sampleSize;

				return BitmapFactory.DecodeFile (path, options);
			});
		}

		private Bitmap bitmap;
		private void Cleanup()
		{
			if (this.bitmap == null)
				return;

			this.image.SetImageBitmap (null);
			this.bitmap.Dispose();
			this.bitmap = null;
		}
	}
}