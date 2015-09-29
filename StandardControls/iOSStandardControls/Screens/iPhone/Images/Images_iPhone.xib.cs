using System;
using Foundation;
using UIKit;

namespace Example_StandardControls.Screens.iPhone.Images
{
	public partial class Images_iPhone : UIViewController
	{
		public Images_iPhone (IntPtr handle)
			: base (handle)
		{
		}

		public Images_iPhone ()
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			Title = "Images";
		}
	}
}