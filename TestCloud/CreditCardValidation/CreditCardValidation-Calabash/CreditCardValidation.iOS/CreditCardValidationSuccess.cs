using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace CreditCardValidation.iOS
{
	public partial class CreditCardValidationSuccess : UIViewController
	{
		public CreditCardValidationSuccess () : base ("CreditCardValidationSuccess", null)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			this.Title = "Valid Credit Card";
			
			// Perform any additional setup after loading the view, typically from a nib.
		}
	}
}

