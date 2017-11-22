using System;
using System.Drawing;

using Foundation;
using UIKit;

namespace PhonewordiOS
{
	public partial class PhonewordiOSViewController : UIViewController
	{
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public PhonewordiOSViewController (IntPtr handle) : base (handle)
		{
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
		}
		
		#region View lifecycle
		string translatedNumber = "";
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			CallButton.Enabled = false;

			PhoneNumberText.ShouldReturn += (textField) => {
				textField.ResignFirstResponder();
				return true;
			};

			PhoneNumberText.ClearsOnBeginEditing = true;

			TranslateButton.TouchUpInside += (sender, e) => {

				// *** SHARED CODE ***
				translatedNumber = Core.PhonewordTranslator.ToNumber(PhoneNumberText.Text);


				if (translatedNumber == "") {
					CallButton.SetTitle ("Call ", UIControlState.Normal);
					CallButton.Enabled = false;
				} else {
					CallButton.SetTitle ("Call " + translatedNumber, UIControlState.Normal);
					CallButton.Enabled = true;
				}
			};
			CallButton.TouchUpInside += (sender, e) => {
				NSUrl url = new NSUrl("tel:" + translatedNumber);
				if (!UIApplication.SharedApplication.OpenUrl(url))
				{
					var av = new UIAlertView("Not supported"
					                         , "Scheme 'tel:' is not supported on this device"
					                         , null
					                         , "OK"
					                         , null);
					av.Show();
				}
			};
		}
		
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			ReleaseDesignerOutlets ();
		}
		
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}
		
		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}
		
		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}
		
		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}
		
		#endregion
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			if (UserInterfaceIdiomIsPhone) {
				return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
			} else {
				return true;
			}
		}
	}
}

