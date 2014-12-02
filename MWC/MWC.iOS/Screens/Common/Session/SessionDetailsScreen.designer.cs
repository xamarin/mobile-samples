// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;

namespace MWC.iOS.Screens.Common.Session
{
	[Register ("SessionDetailsScreen")]
	partial class SessionDetailsScreen
	{
		[Outlet]
		UIKit.UILabel TitleLabel { get; set; }

		[Outlet]
		UIKit.UILabel LocationLabel { get; set; }

		[Outlet]
		UIKit.UILabel TimeLabel { get; set; }

		[Outlet]
		UIKit.UILabel SpeakerLabel { get; set; }

		[Outlet]
		UIKit.UITextView OverviewLabel { get; set; }

		[Outlet]
		UIKit.UIButton FavoriteButton { get; set; }

		[Outlet]
		UIKit.UIScrollView ScrollView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (TitleLabel != null) {
				TitleLabel.Dispose ();
				TitleLabel = null;
			}

			if (LocationLabel != null) {
				LocationLabel.Dispose ();
				LocationLabel = null;
			}

			if (TimeLabel != null) {
				TimeLabel.Dispose ();
				TimeLabel = null;
			}

			if (SpeakerLabel != null) {
				SpeakerLabel.Dispose ();
				SpeakerLabel = null;
			}

			if (OverviewLabel != null) {
				OverviewLabel.Dispose ();
				OverviewLabel = null;
			}

			if (FavoriteButton != null) {
				FavoriteButton.Dispose ();
				FavoriteButton = null;
			}

			if (ScrollView != null) {
				ScrollView.Dispose ();
				ScrollView = null;
			}
		}
	}
}
