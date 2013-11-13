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
	[Register ("AsyncExtrasController")]
	partial class AsyncExtrasController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton CancelButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIProgressView ProgressBar { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextView ProgressTextView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton StartButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextView StatusTextView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ProgressTextView != null) {
				ProgressTextView.Dispose ();
				ProgressTextView = null;
			}

			if (ProgressBar != null) {
				ProgressBar.Dispose ();
				ProgressBar = null;
			}

			if (StatusTextView != null) {
				StatusTextView.Dispose ();
				StatusTextView = null;
			}

			if (CancelButton != null) {
				CancelButton.Dispose ();
				CancelButton = null;
			}

			if (StartButton != null) {
				StartButton.Dispose ();
				StartButton = null;
			}
		}
	}
}
