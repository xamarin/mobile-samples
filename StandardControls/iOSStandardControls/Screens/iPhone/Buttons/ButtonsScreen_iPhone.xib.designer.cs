// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Example_StandardControls.Screens.iPhone.Buttons
{
    [Register ("ButtonsScreen_iPhone")]
    partial class ButtonsScreen_iPhone
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnOne { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnTwo { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnOne != null) {
                btnOne.Dispose ();
                btnOne = null;
            }

            if (btnTwo != null) {
                btnTwo.Dispose ();
                btnTwo = null;
            }
        }
    }
}