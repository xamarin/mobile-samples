// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Example_StandardControls.Screens.iPhone.ProgressBars
{
    [Register ("ProgressBars_iPhone")]
    partial class ProgressBars_iPhone
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIProgressView btnProgress1 { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnProgress1 != null) {
                btnProgress1.Dispose ();
                btnProgress1 = null;
            }
        }
    }
}