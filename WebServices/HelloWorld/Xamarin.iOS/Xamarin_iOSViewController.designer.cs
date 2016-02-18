// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace Xamarin.iOS
{
	[Register ("Xamarin_iOSViewController")]
	partial class Xamarin_iOSViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton getHelloWorldDataButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextView getHelloWorldDataText { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton helloWorldButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton sayHelloWorldButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextView sayHelloWorldText { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (getHelloWorldDataButton != null) {
				getHelloWorldDataButton.Dispose ();
				getHelloWorldDataButton = null;
			}

			if (getHelloWorldDataText != null) {
				getHelloWorldDataText.Dispose ();
				getHelloWorldDataText = null;
			}

			if (sayHelloWorldButton != null) {
				sayHelloWorldButton.Dispose ();
				sayHelloWorldButton = null;
			}

			if (sayHelloWorldText != null) {
				sayHelloWorldText.Dispose ();
				sayHelloWorldText = null;
			}

			if (helloWorldButton != null) {
				helloWorldButton.Dispose ();
				helloWorldButton = null;
			}
		}
	}
}
