// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Example_StandardControls.Screens.iPhone.Sliders
{
    [Register ("Sliders_iPhone")]
    partial class Sliders_iPhone
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISlider sldrWithImages { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (sldrWithImages != null) {
                sldrWithImages.Dispose ();
                sldrWithImages = null;
            }
        }
    }
}