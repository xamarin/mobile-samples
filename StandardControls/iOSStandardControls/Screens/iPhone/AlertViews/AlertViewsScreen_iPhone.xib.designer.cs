// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Example_StandardControls.Screens.iPhone.AlertViews
{
    [Register ("AlertViewsScreen_iPhone")]
    partial class AlertViewsScreen_iPhone
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnCustomAlert { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnCustomButtons { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnCustomButtonsWithDelegate { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnSimpleAlert { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnCustomAlert != null) {
                btnCustomAlert.Dispose ();
                btnCustomAlert = null;
            }

            if (btnCustomButtons != null) {
                btnCustomButtons.Dispose ();
                btnCustomButtons = null;
            }

            if (btnCustomButtonsWithDelegate != null) {
                btnCustomButtonsWithDelegate.Dispose ();
                btnCustomButtonsWithDelegate = null;
            }

            if (btnSimpleAlert != null) {
                btnSimpleAlert.Dispose ();
                btnSimpleAlert = null;
            }
        }
    }
}