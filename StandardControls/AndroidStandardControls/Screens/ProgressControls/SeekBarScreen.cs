using Android.App;
using Android.OS;
using Android.Widget;

namespace StandardControls
{
	[Activity(Label = "SeekBar")]
	public class SeekBarScreen : Activity, SeekBar.IOnSeekBarChangeListener
	{
		SeekBar _seekBar;
		TextView _textView;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.SeekBarScreen);

			_seekBar = FindViewById<SeekBar>(Resource.Id.seekBar1);
			_textView = FindViewById<TextView>(Resource.Id.textView1);

			// The "better" way is to use the event handler
			_seekBar.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) => {
				if (e.FromUser)
				{
					_textView.Text = string.Format("The value of the SeekBar is {0}", e.Progress);
				}
			};
		
			// This is one example of how to implement SeekBar.IOnSeekBarChangeListener. Make
			// sure that you comment out the event handler in the previous line.
			//_seekBar.SetOnSeekBarChangeListener(this);
		}

		public void OnProgressChanged(SeekBar seekBar, int progress, bool fromUser)
		{
			if (fromUser)
			{
				_textView.Text = string.Format("The you adjusted the value of the SeekBar to {0}", seekBar.Progress);
			}
		}

		public void OnStartTrackingTouch(SeekBar seekBar)
		{
			// see http://developer.android.com/reference/android/widget/SeekBar.OnSeekBarChangeListener.html#onStartTrackingTouch(android.widget.SeekBar)
			// for details about this method.
			System.Diagnostics.Debug.WriteLine("Tracking changes.");
		}

		public void OnStopTrackingTouch(SeekBar seekBar)
		{
			// see http://developer.android.com/reference/android/widget/SeekBar.OnSeekBarChangeListener.html#onStopTrackingTouch(android.widget.SeekBar)
			// for details about this method
			System.Diagnostics.Debug.WriteLine("Stopped tracking changes.");
		}
	}
}


