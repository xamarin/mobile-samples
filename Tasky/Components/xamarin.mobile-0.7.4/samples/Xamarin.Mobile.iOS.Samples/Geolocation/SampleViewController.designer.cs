// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;

namespace Sample
{
	[Register ("SampleViewController")]
	partial class SampleViewController
	{
		[Outlet]
		UIKit.UIButton PositionButton { get; set; }

		[Outlet]
		UIKit.UIButton CancelButton { get; set; }

		[Outlet]
		UIKit.UIButton ToggleListeningButton { get; set; }

		[Outlet]
		UIKit.UILabel PositionStatus { get; set; }

		[Outlet]
		UIKit.UILabel PositionLatitude { get; set; }

		[Outlet]
		UIKit.UILabel PositionLongitude { get; set; }

		[Outlet]
		UIKit.UILabel ListenStatus { get; set; }

		[Outlet]
		UIKit.UILabel ListenLongitude { get; set; }

		[Outlet]
		UIKit.UILabel ListenLatitude { get; set; }

		[Action ("GetPosition:")]
		partial void GetPosition (Foundation.NSObject sender);

		[Action ("CancelPosition:")]
		partial void CancelPosition (Foundation.NSObject sender);

		[Action ("ToggleListening:")]
		partial void ToggleListening (Foundation.NSObject sender);
	}
}
