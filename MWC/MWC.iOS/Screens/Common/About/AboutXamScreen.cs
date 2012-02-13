using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace MWC.iOS.Screens.Common.About {
	public partial class AboutXamScreen : UIViewController {
		public AboutXamScreen ()
			: base (AppDelegate.IsPhone ? "AboutXamScreen_iPhone" : "AboutXamScreen_iPad", null)
		{
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Title = "About Xamarin";
			
			if (AppDelegate.IsPhone) {
				ScrollView.Frame = new RectangleF(0, 43, 320, 367);
				ScrollView.ContentSize = new SizeF(320, 600);
	
				XamLogoImageView.Image = UIImage.FromBundle("/Images/About");			
				
				AboutTextView.Frame = new RectangleF(
											AboutTextView.Frame.X,
											AboutTextView.Frame.Y,
											320, 
											320);
			} else {
				// IsPad
				ScrollView.Frame = new RectangleF(0, 0, 768, 1004);
				ScrollView.ContentSize = new SizeF(768, 1024);

				XamLogoImageView.Image = UIImage.FromBundle("/Images/About-Portrait~iPad");			
				XamLogoImageView.Frame = ScrollView.Frame;

				AboutTextView.Frame = new RectangleF(
											15,
											800,
											768, 
											320);
				AboutTextView.AutoresizingMask = UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleWidth;
			}
			AboutTextView.Text = Constants.AboutText; // @"Xamarin was founded in 2011 with the mission to make it fast, easy and fun to build great mobile apps. Xamarin’s products are used by individual developers and companies, including VMware, Target, Rdio, Medtronic and Unity Technologies, to simplify creation and operation of high-performance, cross-platform mobile consumer and corporate applications, targeting phones and tablets running iOS, Android and Windows. For more information, visit http://xamarin.com.";
		}
		
		public bool IsPortrait {
			get {
				return InterfaceOrientation == UIInterfaceOrientation.Portrait 
					|| InterfaceOrientation == UIInterfaceOrientation.PortraitUpsideDown;
			}
		}
	
		protected void OnDeviceRotated(NSNotification notification)
		{
			if (AppDelegate.IsPad) {
				if(IsPortrait) {
					XamLogoImageView.Image = UIImage.FromBundle("/Images/About-Portrait~iPad");
					ScrollView.ContentSize = new SizeF(768, 1004);
				} else {
					// IsLandscape
					XamLogoImageView.Image = UIImage.FromBundle("/Images/About-Landscape~iPad");
					ScrollView.ContentSize = new SizeF(1024, 748);
				}
			}
		}
		
		NSObject ObserverRotation;

		/// <summary>
		/// Is called when the view is about to appear on the screen. We use method to hide the 
		/// navigation bar.
		/// </summary>
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			OnDeviceRotated(null);
			ObserverRotation = NSNotificationCenter.DefaultCenter.AddObserver("UIDeviceOrientationDidChangeNotification", OnDeviceRotated);
			UIDevice.CurrentDevice.BeginGeneratingDeviceOrientationNotifications();
		}
		
		/// <summary>
		/// Is called when the another view will appear and one will be hidden. We use method
		/// to show the navigation bar again.
		/// </summary>
		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			UIDevice.CurrentDevice.EndGeneratingDeviceOrientationNotifications();
			NSNotificationCenter.DefaultCenter.RemoveObserver(ObserverRotation);
		}
	}
}