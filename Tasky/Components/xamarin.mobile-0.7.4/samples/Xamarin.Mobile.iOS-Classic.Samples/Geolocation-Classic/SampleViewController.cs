using MonoTouch.UIKit;
using System.Drawing;
using System;
using MonoTouch.Foundation;
using System.Threading;
using Xamarin.Geolocation;
using System.Threading.Tasks;

namespace Sample
{
	public partial class SampleViewController : UIViewController
	{
		#region Boilerplate
		public SampleViewController () : base ("SampleViewController", null)
		{
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
		#endregion
		
		private Geolocator geolocator;

		private TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
		private CancellationTokenSource cancelSource;
		
		private void Setup()
		{
			if (this.geolocator != null)
				return;

			this.geolocator = new Geolocator { DesiredAccuracy = 50 };
			this.geolocator.PositionError += OnListeningError;
			this.geolocator.PositionChanged += OnPositionChanged;
		}

		partial void GetPosition (NSObject sender)
		{
			Setup();
			
			this.cancelSource = new CancellationTokenSource();
			
			PositionStatus.Text = String.Empty;
			PositionLatitude.Text = String.Empty;
			PositionLongitude.Text = String.Empty;
			
			this.geolocator.GetPositionAsync (timeout: 10000, cancelToken: this.cancelSource.Token, includeHeading: true)
				.ContinueWith (t =>
				{
					if (t.IsFaulted)
						PositionStatus.Text = ((GeolocationException)t.Exception.InnerException).Error.ToString();
					else if (t.IsCanceled)
						PositionStatus.Text = "Canceled";
					else
					{
						PositionStatus.Text = t.Result.Timestamp.ToString("G");
						PositionLatitude.Text = "La: " + t.Result.Latitude.ToString("N4");
						PositionLongitude.Text = "Lo: " + t.Result.Longitude.ToString("N4");
					}
					
				}, scheduler);
		}
		
		partial void CancelPosition (NSObject sender)
		{
			CancellationTokenSource cancel = this.cancelSource;
			if (cancel != null)
				cancel.Cancel();
		}
		
		partial void ToggleListening (NSObject sender)
		{
			Setup();
			
			if (!this.geolocator.IsListening)
			{
				ToggleListeningButton.SetTitle ("Stop listening", UIControlState.Normal);
				
				this.geolocator.StartListening (minTime: 30000, minDistance: 0, includeHeading: true);
			}
			else
			{
				ToggleListeningButton.SetTitle ("Start listening", UIControlState.Normal);
				this.geolocator.StopListening();
			}
		}
		
		private void OnListeningError (object sender, PositionErrorEventArgs e)
		{
			BeginInvokeOnMainThread (() => {
				ListenStatus.Text = e.Error.ToString();
			});
		}
		
		private void OnPositionChanged (object sender, PositionEventArgs e)
		{
			BeginInvokeOnMainThread (() => {
				ListenStatus.Text = e.Position.Timestamp.ToString("G");
				ListenLatitude.Text = "La: " + e.Position.Latitude.ToString("N4");
				ListenLongitude.Text = "Lo: " + e.Position.Longitude.ToString("N4");
			});
		}
	}
}
