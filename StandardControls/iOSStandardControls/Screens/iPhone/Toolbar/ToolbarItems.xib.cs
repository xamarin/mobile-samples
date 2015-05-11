using System;
using Foundation;
using UIKit;

namespace Example_StandardControls.Screens.iPhone.Toolbar
{
	public partial class ToolbarItems : UIViewController
	{
		public ToolbarItems (IntPtr handle)
			: base (handle)
		{
		}

		public ToolbarItems ()
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			Title = "Various toolbar items";
		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return toInterfaceOrientation == UIInterfaceOrientation.LandscapeLeft || toInterfaceOrientation == UIInterfaceOrientation.LandscapeRight;
		}
	}
}