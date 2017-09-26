// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Example_StandardControls.Screens.iPhone.Images
{
    [Register ("Images_iPhone")]
    partial class Images_iPhone
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgMain { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (imgMain != null) {
                imgMain.Dispose ();
                imgMain = null;
            }
        }
    }
}