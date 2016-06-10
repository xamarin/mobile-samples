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
    [Register ("StockDetailViewController")]
    partial class StockDetailViewController
    {
        [Outlet]
        UIKit.UIButton SaveButton { get; set; }


        [Outlet]
        UIKit.UIButton DeleteButton { get; set; }


        [Outlet]
        UIKit.UITextField Code { get; set; }


        [Outlet]
        UIKit.UITextField Name { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Code != null) {
                Code.Dispose ();
                Code = null;
            }

            if (DeleteButton != null) {
                DeleteButton.Dispose ();
                DeleteButton = null;
            }

            if (Name != null) {
                Name.Dispose ();
                Name = null;
            }

            if (SaveButton != null) {
                SaveButton.Dispose ();
                SaveButton = null;
            }
        }
    }
}