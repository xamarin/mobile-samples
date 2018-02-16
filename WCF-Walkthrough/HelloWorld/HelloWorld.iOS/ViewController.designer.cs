// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace HelloWorld.iOS
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton getHelloWorldDataButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView getHelloWorldDataText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton sayHelloWorldButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView sayHelloWorldText { get; set; }

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