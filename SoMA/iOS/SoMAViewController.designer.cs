// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace SoMA
{
	[Register ("SoMAViewController")]
	partial class SoMAViewController
	{
		[Outlet]
		UIKit.UIButton PhotoButton { get; set; }

		[Action ("PhotoButton_TouchUpInside:")]
		partial void PhotoButton_TouchUpInside (UIKit.UIButton sender);

		[Action ("UIButton7_TouchUpInside:")]
		partial void UIButton7_TouchUpInside (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (PhotoButton != null) {
				PhotoButton.Dispose ();
				PhotoButton = null;
			}
		}
	}
}
