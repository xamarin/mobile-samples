// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Example_StandardControls.Screens.iPhone.DatePicker
{
    [Register ("DatePicker_iPhone")]
    partial class DatePicker_iPhone
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnChooseDate { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblDate { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnChooseDate != null) {
                btnChooseDate.Dispose ();
                btnChooseDate = null;
            }

            if (lblDate != null) {
                lblDate.Dispose ();
                lblDate = null;
            }
        }
    }
}