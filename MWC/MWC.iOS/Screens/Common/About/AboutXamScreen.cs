using MonoTouch.UIKit;
using System.Drawing;
using System;
using MonoTouch.Foundation;

namespace MWC.iOS.Screens.Common.About
{
	public partial class AboutXamScreen : UIViewController
	{
		public AboutXamScreen ()
			: base (AppDelegate.IsPhone ? "AboutXamScreen_iPhone" : "AboutXamScreen_iPad", null)
		{
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			this.Title = "About Xamarin";
			
			if (AppDelegate.IsPhone)
			{
				this.ScrollView.Frame = new RectangleF(0, 43, 320, 367);
				this.ScrollView.ContentSize = new SizeF(320, 600);
	
				this.XamLogoImageView.Image = UIImage.FromBundle("/Images/About");			
				
				this.AboutTextView.Frame = new RectangleF(
											AboutTextView.Frame.X,
											AboutTextView.Frame.Y,
											320, 
											320);
			}
			else
			{	// IsPad
				this.ScrollView.Frame = new RectangleF(0, 0, 768, 1024);
				this.ScrollView.ContentSize = new SizeF(768, 1024);
	
				this.XamLogoImageView.Image = UIImage.FromBundle("/Images/About-Portrait~iPad");			
				this.XamLogoImageView.Frame = this.ScrollView.Frame;

				this.AboutTextView.Frame = new RectangleF(
											15,
											800,
											768, 
											320);
				this.AboutTextView.AutoresizingMask = UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleWidth;
			}
			this.AboutTextView.Text = @"Xamarin was founded in May of 2011 in Boston, Massachusetts. Our mission is to produce the best software development tools in the world, and to make it fast, easy and fun to build great mobile apps.

Xamarin is composed of more than 20 members of the team that built Mono, with ten years of experience working together to create a great developer platform.

http://xamarin.com";


		}
	}
}