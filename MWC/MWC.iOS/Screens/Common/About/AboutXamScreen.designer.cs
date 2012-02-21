// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace MWC.iOS.Screens.Common.About
{
	[Register ("AboutXamScreen")]
	partial class AboutXamScreen
	{
		[Outlet]
		MonoTouch.UIKit.UIScrollView ScrollView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView XamLogoImageView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextView AboutTextView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton StackOverflowButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton LinkedInButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton TwitterButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton YouTubeButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton FacebookButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton BlogRssButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ScrollView != null) {
				ScrollView.Dispose ();
				ScrollView = null;
			}

			if (XamLogoImageView != null) {
				XamLogoImageView.Dispose ();
				XamLogoImageView = null;
			}

			if (AboutTextView != null) {
				AboutTextView.Dispose ();
				AboutTextView = null;
			}

			if (StackOverflowButton != null) {
				StackOverflowButton.Dispose ();
				StackOverflowButton = null;
			}

			if (LinkedInButton != null) {
				LinkedInButton.Dispose ();
				LinkedInButton = null;
			}

			if (TwitterButton != null) {
				TwitterButton.Dispose ();
				TwitterButton = null;
			}

			if (YouTubeButton != null) {
				YouTubeButton.Dispose ();
				YouTubeButton = null;
			}

			if (FacebookButton != null) {
				FacebookButton.Dispose ();
				FacebookButton = null;
			}

			if (BlogRssButton != null) {
				BlogRssButton.Dispose ();
				BlogRssButton = null;
			}
		}
	}
}
