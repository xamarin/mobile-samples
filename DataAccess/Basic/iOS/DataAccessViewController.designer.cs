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

namespace DataAccess
{
    [Register ("DataAccessViewController")]
    partial class DataAccessViewController
    {
        [Outlet]
        UIKit.UIButton AdoExampleButton { get; set; }


        [Outlet]
        UIKit.UIButton OrmExampleButton { get; set; }


        [Outlet]
        UIKit.UITextView OutputText { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AdoExampleButton != null) {
                AdoExampleButton.Dispose ();
                AdoExampleButton = null;
            }

            if (OrmExampleButton != null) {
                OrmExampleButton.Dispose ();
                OrmExampleButton = null;
            }

            if (OutputText != null) {
                OutputText.Dispose ();
                OutputText = null;
            }
        }
    }
}