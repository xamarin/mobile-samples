// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Example_StandardControls.Screens.iPhone.Toolbar
{
    [Register ("Toolbar1_iPhone")]
    partial class Toolbar1_iPhone
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem btnFour { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem btnOne { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem btnThree { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem btnTwo { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnFour != null) {
                btnFour.Dispose ();
                btnFour = null;
            }

            if (btnOne != null) {
                btnOne.Dispose ();
                btnOne = null;
            }

            if (btnThree != null) {
                btnThree.Dispose ();
                btnThree = null;
            }

            if (btnTwo != null) {
                btnTwo.Dispose ();
                btnTwo = null;
            }
        }
    }
}