// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace iOS
{
	[Register ("iOSViewController")]
	partial class iOSViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIImageView DownloadedImageView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton GetButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel ResultLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextView ResultTextView { get; set; }

		[Action ("Naysync_TouchUpInside:")]
		[GeneratedCodeAttribute ("iOS Designer", "1.0")]
		partial void Naysync_TouchUpInside (MonoTouch.UIKit.UIButton sender);

		[Action ("UIButton14_TouchUpInside:")]
		partial void UIButton14_TouchUpInside (MonoTouch.UIKit.UIButton sender);

		[Action ("UIButton9_TouchUpInside:")]
		partial void UIButton9_TouchUpInside (MonoTouch.UIKit.UIButton sender);
		
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
		}
	}
}
