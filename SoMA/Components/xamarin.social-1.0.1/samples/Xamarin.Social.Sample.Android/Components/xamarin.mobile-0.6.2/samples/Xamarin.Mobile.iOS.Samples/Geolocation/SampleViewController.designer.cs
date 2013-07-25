// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace Sample
{
	[Register ("SampleViewController")]
	partial class SampleViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton PositionButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton CancelButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton ToggleListeningButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel PositionStatus { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel PositionLatitude { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel PositionLongitude { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel ListenStatus { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel ListenLongitude { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel ListenLatitude { get; set; }

		[Action ("GetPosition:")]
		partial void GetPosition (MonoTouch.Foundation.NSObject sender);

		[Action ("CancelPosition:")]
		partial void CancelPosition (MonoTouch.Foundation.NSObject sender);

		[Action ("ToggleListening:")]
		partial void ToggleListening (MonoTouch.Foundation.NSObject sender);
	}
}
