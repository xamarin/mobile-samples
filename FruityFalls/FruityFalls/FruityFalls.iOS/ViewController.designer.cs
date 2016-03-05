//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace FruityFalls.iOS
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		CocosSharp.CCGameView GameView { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (GameView != null)
			{
				GameView.Dispose ();
				GameView = null;
			}
		}
	}
}
