// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Example_StandardControls.Screens.iPhone.Switches
{
    [Register ("Switches_iPhone")]
    partial class Switches_iPhone
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch swchOne { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch swchTwo { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (swchOne != null) {
                swchOne.Dispose ();
                swchOne = null;
            }

            if (swchTwo != null) {
                swchTwo.Dispose ();
                swchTwo = null;
            }
        }
    }
}