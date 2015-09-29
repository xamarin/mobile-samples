// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace MediaPickerSample
{
	[Register ("MediaPickerSampleViewController")]
	partial class MediaPickerSampleViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIImageView imageView { get; set; }

		[Action ("takePhotoBtnClicked:")]
		partial void takePhotoBtnClicked (MonoTouch.Foundation.NSObject sender);

		[Action ("pickPhotoBtnClicked:")]
		partial void pickPhotoBtnClicked (MonoTouch.Foundation.NSObject sender);

		[Action ("takeVideoBtnClicked:")]
		partial void takeVideoBtnClicked (MonoTouch.Foundation.NSObject sender);

		[Action ("pickVideoBtnClicked:")]
		partial void pickVideoBtnClicked (MonoTouch.Foundation.NSObject sender);
	}
}
