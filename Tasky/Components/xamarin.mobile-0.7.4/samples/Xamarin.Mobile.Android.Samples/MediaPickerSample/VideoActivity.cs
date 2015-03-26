using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace MediaPickerSample
{
	[Activity]
	public class VideoActivity
		: Activity
	{
		private VideoView video;
		private string path;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			this.video = new VideoView (this);
			SetContentView (this.video, new ViewGroup.LayoutParams (ViewGroup.LayoutParams.FillParent, ViewGroup.LayoutParams.FillParent));

			this.path = (savedInstanceState ?? Intent.Extras).GetString ("path");
			Title = System.IO.Path.GetFileName (this.path);

			this.video.SetVideoPath (this.path);
			this.video.Start();
		}

		protected override void OnSaveInstanceState (Bundle outState)
		{
			outState.PutString ("path", this.path);
			base.OnSaveInstanceState (outState);
		}
	}
}