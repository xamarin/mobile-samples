// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace XamarinTodoQuickStart
{
    [Register ("TodoListViewController")]
    partial class TodoListViewController
    {
        [Outlet]
        UIKit.UIActivityIndicatorView activityIndicator { get; set; }


        [Outlet]
        UIKit.UITextField itemText { get; set; }


        [Action ("OnAdd:")]
        partial void OnAdd (Foundation.NSObject sender);

        void ReleaseDesignerOutlets ()
        {
            if (activityIndicator != null) {
                activityIndicator.Dispose ();
                activityIndicator = null;
            }

            if (itemText != null) {
                itemText.Dispose ();
                itemText = null;
            }
        }
    }
}