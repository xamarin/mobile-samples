using MonoTouch.UIKit;
using System.Drawing;
using System;
using MonoTouch.Foundation;

namespace MWC.iOS.Screens.Common.About
{
	public partial class AboutXamScreen : UIViewController
	{
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public AboutXamScreen ()
			: base (UserInterfaceIdiomIsPhone ? "AboutXamScreen_iPhone" : "AboutXamScreen_iPad", null)
		{
		}
		
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			this.XamLogoImageView.Image = UIImage.FromBundle("/Images/XamLogo");			
		}
		
	}
}

