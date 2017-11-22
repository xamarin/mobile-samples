// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Example_StandardControls.Screens.iPad.ActionSheets
{
    [Register ("ActionSheets_iPad")]
    partial class ActionSheets_iPad
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnActionSheetWithOtherButtons { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnSimpleActionSheet { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnActionSheetWithOtherButtons != null) {
                btnActionSheetWithOtherButtons.Dispose ();
                btnActionSheetWithOtherButtons = null;
            }

            if (btnSimpleActionSheet != null) {
                btnSimpleActionSheet.Dispose ();
                btnSimpleActionSheet = null;
            }
        }
    }
}