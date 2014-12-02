using UIKit;
using System;
using Foundation;

using TipCalc.Util;

namespace TipCalcUIiOS
{
	public partial class MainViewController : UIViewController
	{
		UIPopoverController flipsidePopoverController;
		
		TipInfo info = new TipInfo () {
			TipPercent = 15,
		};
		
		public MainViewController (string nibName, NSBundle bundle) : base (nibName, bundle)
		{
			// Custom initialization
			info.TipValueChanged += (sender, e) => {
				TipValue.Text  = info.TipValue.ToString ();
				Total.Text     = (info.TipValue + info.Total).ToString ();
			};
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			ScrollView.ContentSize = new CoreGraphics.CGSize (Total.Frame.Width, Total.Frame.Height + Total.Frame.Top);
			
			Subtotal.EditingDidEnd += (sender, e) => {
				info.Subtotal = Parse (Subtotal);
			};
			ReceiptTotal.EditingDidEnd += (sender, e) => {
				info.Total = Parse (ReceiptTotal);
			};
			TipPercent.EditingDidEnd += (sender, e) => {
				info.TipPercent = Parse (TipPercent);
			};
			TipPercentSlider.ValueChanged += (sender, e) => {
				TipPercent.Text = Math.Truncate(TipPercentSlider.Value).ToString ();
				info.TipPercent = (decimal) TipPercentSlider.Value;
			};
		}
		
		static decimal Parse (UITextField field)
		{
			if (field.Text == "")
				return 0m;
			try {
				return Convert.ToDecimal (field.Text);
			} catch (Exception) {
				field.Text = "";
				return 0m;
			}
		}

		[Obsolete]
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone) {
				return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
			} else {
				return true;
			}
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		[Obsolete]
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			
			// Release any retained subviews of the main view.
			// e.g. this.myOutlet = null;
		}
		
		partial void showInfo (NSObject sender)
		{
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone) {
				var controller = new FlipsideViewController ("FlipsideViewController", null) {
					ModalTransitionStyle = UIModalTransitionStyle.FlipHorizontal,
				};
				controller.Done += delegate {
					this.DismissModalViewController (true);
				};
				this.PresentModalViewController (controller, true);
			} else {
				if (flipsidePopoverController == null) {
					var controller = new FlipsideViewController ("FlipsideViewController", null);
					flipsidePopoverController = new UIPopoverController (controller);
					controller.Done += delegate {
						flipsidePopoverController.Dismiss (true);
					};
				}
				if (flipsidePopoverController.PopoverVisible) {
					flipsidePopoverController.Dismiss (true);
				} else {
					flipsidePopoverController.PresentFromBarButtonItem ((UIBarButtonItem)sender, UIPopoverArrowDirection.Any, true);
				}
			}
		}
	}
}
