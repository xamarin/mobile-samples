using System;
using CoreGraphics;

using Foundation;
using UIKit;

namespace DataAccess
{
	public partial class DataAccessViewController : UIViewController
	{
		public DataAccessViewController () : base ("DataAccessViewController", null)
		{
		}
	
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.
			OrmExampleButton.TouchUpInside += (sender, e) => {
				OutputText.Text = OrmExample.DoSomeDataAccess ();

				OutputText.Text += OrmExample.MoreComplexQuery ();

				OutputText.Text += OrmExample.ScalarQuery ();

				OutputText.Text += OrmExample.Get ();

				OutputText.Text += OrmExample.Delete ();
			};
			
			AdoExampleButton.TouchUpInside += (sender, e) => {
				OutputText.Text = AdoExample.DoSomeDataAccess ();

				OutputText.Text += AdoExample.MoreComplexQuery ();

				OutputText.Text += AdoExample.ScalarQuery ();
			};
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
		}

		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			
			// Clear any references to subviews of the main view in order to
			// allow the Garbage Collector to collect them sooner.
			//
			// e.g. myOutlet.Dispose (); myOutlet = null;
			
			ReleaseDesignerOutlets ();
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}
	}
}

