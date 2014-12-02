using System;

using UIKit;
using CoreLocation;

using Location.iOS.MainScreen;

namespace Location.iOS
{
	public class MainViewController : UIViewController
	{
		#region declarations
		
		MainViewController_iPhone mainViewController_iPhone;
		MainViewController_iPad mainViewController_iPad;
		
		IMainScreen mainScreen = null;

		#endregion
		
		#region constructors
		// Constructor invoked from the NIB loader
		public MainViewController (IntPtr p) : base(p)
		{
		}
		
		public MainViewController () : base()
		{
		}
		#endregion
		
		public override void ViewDidLoad ()
		{
			// call your base
			base.ViewDidLoad ();
			
			// load the appropriate view, based on the device type
			this.LoadViewForDevice ();

			// It is better to handle this with notifications, so that the UI updates
			// resume when the application re-enters the foreground!
			// Manager.LocationUpdated += HandleLocationChanged;

			// screen subscribes to the location changed event
			UIApplication.Notifications.ObserveDidBecomeActive ((sender, args) => {
				AppDelegate.Manager.LocationUpdated += HandleLocationChanged;
			});
			
			// whenever the app enters the background state, we unsubscribe from the event 
			// so we no longer perform foreground updates
			UIApplication.Notifications.ObserveDidEnterBackground ((sender, args) => {
				AppDelegate.Manager.LocationUpdated -= HandleLocationChanged;
			});
			
		}

		
		public void HandleLocationChanged (object sender, LocationUpdatedEventArgs e)
		{
			// handle foreground updates
			CLLocation location = e.Location;
			IMainScreen ms = mainScreen;

			ms.LblAltitude.Text = location.Altitude + " meters";
			ms.LblLongitude.Text = location.Coordinate.Longitude.ToString ();
			ms.LblLatitude.Text = location.Coordinate.Latitude.ToString ();
			ms.LblCourse.Text = location.Course.ToString ();
			ms.LblSpeed.Text = location.Speed.ToString ();

			Console.WriteLine ("foreground updated");
		}
		
		#region protected methods
		
		// Loads either the iPad or iPhone view, based on the current device
		protected void LoadViewForDevice()
		{
			// load the appropriate view based on the device
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				mainViewController_iPad = new MainViewController_iPad ();
				this.View.AddSubview (mainViewController_iPad.View);
				mainScreen = mainViewController_iPad;
			} else {
				mainViewController_iPhone = new MainViewController_iPhone ();
				var b = this.View.Bounds;
				this.View.AddSubview (mainViewController_iPhone.View);
				mainViewController_iPhone.View.Frame = b; // for 4 inch iPhone5 screen
				mainScreen = mainViewController_iPhone;
			}
		}
		
		#endregion
	}
}
