// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;

namespace XamarinTodoQuickStart
{
	[Register ("TodoListViewController")]
	partial class TodoListViewController
	{
		[Outlet]
		UIKit.UIActivityIndicatorView activityIndicator { get; set; }

		[Outlet]
		UIKit.UITextField itemText { get; set; }

		[Action ("OnAdd:")]
		partial void OnAdd (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (itemText != null) {
				itemText.Dispose ();
				itemText = null;
			}

			if (activityIndicator != null) {
				activityIndicator.Dispose ();
				activityIndicator = null;
			}
		}
	}
}
