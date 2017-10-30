// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Example_StandardControls.Screens.iPhone.PagerControl
{
    [Register ("PagerControl_iPhone")]
    partial class PagerControl_iPhone
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIPageControl pgrMain { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView scrlMain { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (pgrMain != null) {
                pgrMain.Dispose ();
                pgrMain = null;
            }

            if (scrlMain != null) {
                scrlMain.Dispose ();
                scrlMain = null;
            }
        }
    }
}