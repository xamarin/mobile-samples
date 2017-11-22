// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace iOS
{
	[Register ("iOSViewController")]
	partial class iOSViewController
	{
		[Outlet]
		UIKit.UIButton BetterAsyncButton { get; set; }

		[Outlet]
		UIKit.UIImageView DownloadedImageView { get; set; }

		[Outlet]
		UIKit.UIButton GetButton { get; set; }

		[Outlet]
		UIKit.UILabel ResultLabel { get; set; }

		[Outlet]
		UIKit.UITextView ResultTextView { get; set; }

		[Action ("Naysync_TouchUpInside:")]
		partial void Naysync_TouchUpInside (UIKit.UIButton sender);

		[Action ("UIButton14_TouchUpInside:")]
		partial void UIButton14_TouchUpInside (UIKit.UIButton sender);

		[Action ("UIButton9_TouchUpInside:")]
		partial void UIButton9_TouchUpInside (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (DownloadedImageView != null) {
				DownloadedImageView.Dispose ();
				DownloadedImageView = null;
			}

			if (GetButton != null) {
				GetButton.Dispose ();
				GetButton = null;
			}

			if (ResultLabel != null) {
				ResultLabel.Dispose ();
				ResultLabel = null;
			}

			if (ResultTextView != null) {
				ResultTextView.Dispose ();
				ResultTextView = null;
			}

			if (BetterAsyncButton != null) {
				BetterAsyncButton.Dispose ();
				BetterAsyncButton = null;
			}
		}
	}
}
