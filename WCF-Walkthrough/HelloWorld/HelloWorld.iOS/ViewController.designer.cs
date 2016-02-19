// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace HelloWorld.iOS
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton getHelloWorldDataButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextView getHelloWorldDataText { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton sayHelloWorldButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextView sayHelloWorldText { get; set; }

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
		}
	}
}
