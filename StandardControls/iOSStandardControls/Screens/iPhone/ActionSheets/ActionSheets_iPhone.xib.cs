using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;

namespace Example_StandardControls.Screens.iPhone.ActionSheets
{
	public partial class ActionSheets_iPhone : UIViewController
	{
		UIActionSheet actionSheet;

		public ActionSheets_iPhone (IntPtr handle)
			: base (handle)
		{
		}

		public ActionSheets_iPhone ()
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Title = "Action Sheets";

			btnSimpleActionSheet.TouchUpInside += HandleBtnSimpleActionSheetTouchUpInside;
			btnActionSheetWithOtherButtons.TouchUpInside += HandleBtnActionSheetWithOtherButtonsTouchUpInside;
		}

		void HandleBtnSimpleActionSheetTouchUpInside (object sender, EventArgs e)
		{
			// create an action sheet using the qualified constructor
			actionSheet = new UIActionSheet ("simple action sheet", null, "cancel", "delete", null);
			actionSheet.Clicked += OnClicked;
			actionSheet.ShowInView (View);
		}

		void HandleBtnActionSheetWithOtherButtonsTouchUpInside (object sender, EventArgs e)
		{
			actionSheet = new UIActionSheet ("action sheet with other buttons");
			actionSheet.AddButton ("delete");
			actionSheet.AddButton ("cancel");
			actionSheet.AddButton ("a different option!");
			actionSheet.AddButton ("another option");
			actionSheet.DestructiveButtonIndex = 0;
			actionSheet.CancelButtonIndex = 1;
			actionSheet.Clicked += OnClicked;
			actionSheet.ShowInView (View);
		}

		void OnClicked(object sender, UIButtonEventArgs e)
		{
			Console.WriteLine (string.Format("Button {0} clicked", e.ButtonIndex));
		}
	}
}