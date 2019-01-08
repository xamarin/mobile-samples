// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace EmbeddedResources
{
    [Register ("HomeScreen")]
    partial class HomeScreen
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView HaikuTextView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (HaikuTextView != null) {
                HaikuTextView.Dispose ();
                HaikuTextView = null;
            }
        }
    }
}