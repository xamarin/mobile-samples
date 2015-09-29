// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace EmbeddedResources
{
	[Register ("HomeScreen")]
	partial class HomeScreen
	{
		[Outlet]
		[GeneratedCodeAttribute ("iOS Designer", "1.0")]
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
