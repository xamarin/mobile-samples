using MonoTouch.UIKit;
using System.Drawing;
using System;
using MonoTouch.Foundation;
using Xamarin.Media;
using MonoTouch.MediaPlayer;

namespace MediaPickerSample
{
	public partial class MediaPickerSampleViewController : UIViewController
	{
		public MediaPickerSampleViewController () : base ("MediaPickerSampleViewController", null)
		{
		}
		
		MediaPicker picker;
		MPMoviePlayerViewController moviePlayer;
		private UIAlertView errorAlert;
		
		/// <summary>
		/// Event handler when the user clicks the Take a Photo button
		/// </summary>
		/// <param name='sender'>
		/// Sender
		/// </param>
		partial void takePhotoBtnClicked (MonoTouch.Foundation.NSObject sender)
		{
			Console.WriteLine("takePhotoBtnClicked");
			
			picker = new MediaPicker ();
			
			// Check if a camera is available and photos are supported on this device
			if (!picker.IsCameraAvailable || !picker.PhotosSupported)
			{
				ShowUnsupported();
				return;
			}

			// Call TakePhotoAsync, which gives us a Task<MediaFile>
			picker.TakePhotoAsync (new StoreCameraMediaOptions
			{
				Name = "test.jpg",
				Directory = "MediaPickerSample"
			})
			.ContinueWith (t => // Continue when the user has taken a photo
			{
				if (t.IsCanceled) // The user canceled
					return;
					
				// Show the photo the user took
				InvokeOnMainThread( delegate {
					UIImage image = UIImage.FromFile(t.Result.Path);
					this.imageView.Image = image;	
				});
			});
		}
		
		/// <summary>
		/// Event handler when the user clicks the Pick a Photo button
		/// </summary>
		/// <param name='sender'>
		/// Sender
		/// </param>
		partial void pickPhotoBtnClicked (MonoTouch.Foundation.NSObject sender)
		{
			Console.WriteLine("pickPhotoBtnClicked");
			
			picker = new MediaPicker ();
			
			// Check if photos are supported on this device
			if (!picker.PhotosSupported)
			{
				ShowUnsupported();
				return;
			}
			
			// Call PickPhotoAsync, which gives us a Task<MediaFile>
			picker.PickPhotoAsync ()
			.ContinueWith (t => // Continue when the user has picked a photo
			{
				if (t.IsCanceled) // The user canceled
					return;
					
				// Show the photo the user selected
				InvokeOnMainThread( delegate {
					UIImage image = UIImage.FromFile(t.Result.Path);
					this.imageView.Image = image;	
				});
			});
		}
		
		/// <summary>
		/// Event handler when the user clicks the Take a Video button
		/// </summary>
		/// <param name='sender'>
		/// Sender
		/// </param>
		partial void takeVideoBtnClicked (MonoTouch.Foundation.NSObject sender)
		{
			Console.WriteLine("takeVideoBtnClicked");
			
			picker = new MediaPicker ();
			
			// Check if a camera is available and videos are supported on this device
			if (!picker.IsCameraAvailable || !picker.VideosSupported)
			{
				ShowUnsupported();
				return;
			}
			
			// Call TakeVideoAsync, which returns a Task<MediaFile>.
			picker.TakeVideoAsync (new StoreVideoOptions
			{
				Quality = VideoQuality.Medium,
				DesiredLength = new TimeSpan(0, 0, 30)
			})
			.ContinueWith (t => // Continue when the user has finished recording
			{
				if (t.IsCanceled) // The user canceled
					return;
					
				// Play the video the user recorded
				InvokeOnMainThread( delegate {
					moviePlayer = new MPMoviePlayerViewController (NSUrl.FromFilename(t.Result.Path));
					moviePlayer.MoviePlayer.UseApplicationAudioSession = true;
		    		this.PresentMoviePlayerViewController(moviePlayer);
				});
			});
		}
		
		/// <summary>
		/// Event handler when the user clicks the pick a video button 
		/// </summary>
		/// <param name='sender'>
		/// Sender
		/// </param>
		partial void pickVideoBtnClicked (MonoTouch.Foundation.NSObject sender)
		{
			Console.WriteLine("pickVideoBtnClicked");
			
			picker = new MediaPicker ();
			
			// Check that videos are supported on this device
			if (!picker.VideosSupported)
			{
				ShowUnsupported();
				return;
			}
			
			//
			// Call PickideoAsync, which returns a Task<MediaFile>
			picker.PickVideoAsync ()
			.ContinueWith (t => // Continue when the user has picked a video
			{
				if (t.IsCanceled) // The user canceled
					return;
					
				// Play the video the user picked
				InvokeOnMainThread( delegate {
					moviePlayer = new MPMoviePlayerViewController (NSUrl.FromFilename(t.Result.Path));
					moviePlayer.MoviePlayer.UseApplicationAudioSession = true;
		    		this.PresentMoviePlayerViewController(moviePlayer);
				});
			});
		}
		
		private void ShowUnsupported()
		{
			if (this.errorAlert != null)
				this.errorAlert.Dispose();
			
			this.errorAlert = new UIAlertView ("Device unsupported", "Your device does not support this feature",
			                                   new UIAlertViewDelegate(), "OK");
			this.errorAlert.Show();
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			//any additional setup after loading the view, typically from a nib.
		}
		
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			
			// Release any retained subviews of the main view.
			// e.g. myOutlet = null;
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}
	}
}
