// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace SoMA
{
	[Register ("PhotoScreen")]
	partial class PhotoScreen
	{
		[Outlet]
		UIKit.UILabel LocationText { get; set; }

		[Outlet]
		UIKit.UIImageView PhotoImageView { get; set; }

		[Outlet]
		UIKit.UIButton ShareAppnet { get; set; }

		[Outlet]
		UIKit.UIButton ShareFacebook { get; set; }

		[Outlet]
		UIKit.UIButton ShareFlickr { get; set; }

		[Outlet]
		UIKit.UIButton ShareTwitter { get; set; }

		[Action ("ShareAppnet_TouchUpInside:")]
		partial void ShareAppnet_TouchUpInside (UIKit.UIButton sender);

		[Action ("ShareFacebook_TouchUpInside:")]
		partial void ShareFacebook_TouchUpInside (UIKit.UIButton sender);

		[Action ("ShareFlickr_TouchUpInside:")]
		partial void ShareFlickr_TouchUpInside (UIKit.UIButton sender);

		[Action ("ShareTwitter_TouchUpInside:")]
		partial void ShareTwitter_TouchUpInside (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (LocationText != null) {
				LocationText.Dispose ();
				LocationText = null;
			}

			if (PhotoImageView != null) {
				PhotoImageView.Dispose ();
				PhotoImageView = null;
			}

			if (ShareAppnet != null) {
				ShareAppnet.Dispose ();
				ShareAppnet = null;
			}

			if (ShareFacebook != null) {
				ShareFacebook.Dispose ();
				ShareFacebook = null;
			}

			if (ShareFlickr != null) {
				ShareFlickr.Dispose ();
				ShareFlickr = null;
			}

			if (ShareTwitter != null) {
				ShareTwitter.Dispose ();
				ShareTwitter = null;
			}
		}
	}
}
