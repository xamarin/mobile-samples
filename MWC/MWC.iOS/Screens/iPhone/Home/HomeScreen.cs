using MonoTouch.UIKit;
using System.Drawing;
using System;
using MonoTouch.Foundation;

namespace MWC.iOS.Screens.iPhone.Home
{
	public partial class HomeScreen : UIViewController
	{
		public HomeScreen () : base ("HomeScreen", null)
		{
		}
				
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			this.MwcLogoImageView.Image = UIImage.FromBundle("/Images/MWCLogo");
			this.XamLogoImageView.Image = UIImage.FromBundle("/Images/XamLogo");
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}
	}
}

