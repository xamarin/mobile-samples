// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Example_StandardControls.Screens.iPhone.PickerView
{
    [Register ("PickerWithMultipleComponents_iPhone")]
    partial class PickerWithMultipleComponents_iPhone
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblSelectedItem { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIPickerView pkrMain { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (lblSelectedItem != null) {
                lblSelectedItem.Dispose ();
                lblSelectedItem = null;
            }

            if (pkrMain != null) {
                pkrMain.Dispose ();
                pkrMain = null;
            }
        }
    }
}