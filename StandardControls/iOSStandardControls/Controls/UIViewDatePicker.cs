using System;
using UIKit;
using System.Drawing;
using CoreGraphics;
using System.Threading.Tasks;

namespace Example_StandardControls.Controls
{
	/// <summary>
	/// A class to show a date picker. To use, create a new UIDatePicker, set the Title, modify
	/// any settings on the DatePicker property, and call Show(). It will
	/// automatically dismiss when the user clicks "Done," or you can call Hide() to dismiss it
	/// manually.
	/// </summary>
	[Foundation.Register ("SlideOnDatePicker")]
	public class UIViewDatePicker
	{
		readonly UIView datePickerView;
		readonly UIButton doneButton;
		readonly UILabel titleLabel;
		readonly UIView owner;

		#region -= properties =-

		public UIDatePicker DatePicker { get; private set; }

		/// <summary>
		/// The title that shows up for the date picker
		/// </summary>
		public string Title {
			get { return titleLabel.Text; }
			set { titleLabel.Text = value; }
		}

		#endregion

		public UIViewDatePicker (UIView owner)
		{
			// save our uiview owner
			this.owner = owner;
			DatePicker = new UIDatePicker ();

			titleLabel = new UILabel {
				BackgroundColor = UIColor.Clear,
				TextColor = UIColor.LightTextColor,
				Font = UIFont.BoldSystemFontOfSize (18),
			};

			doneButton = UIButton.FromType (UIButtonType.RoundedRect);
			doneButton.SetTitle ("done", UIControlState.Normal);
			doneButton.TouchUpInside += (sender, e) => {
				Hide (true);
			};

			datePickerView = new UIView {
				DatePicker,
				titleLabel,
				doneButton
			};
		}

		#region -= public methods =-

		/// <summary>
		/// Shows the action sheet picker from the view that was set as the owner.
		/// </summary>
		public void Show ()
		{
			// declare vars
			float titleBarHeight = 40;
			CGSize doneButtonSize = new CGSize (71, 30);
			CGSize actionSheetSize = new CGSize (owner.Frame.Width, DatePicker.Frame.Height + titleBarHeight);
			CGRect datePickerFrame = new CGRect (0, owner.Frame.Height - actionSheetSize.Height
				, actionSheetSize.Width, actionSheetSize.Height);

			// show the action sheet and add the controls to it
			//actionSheet.ShowInView (owner);
			owner.AddSubview (datePickerView);
			// Set the Y offset of the frame 100, so that it can be brought back upwards in a slide animation
			datePickerFrame.Offset (0, 100);
			// resize the date picker frame to fit our other stuff
			datePickerView.Frame = datePickerFrame;

			// move our picker to be at the bottom of the actionsheet (view coords are relative to the action sheet)
			DatePicker.Frame = new CGRect ((nfloat)DatePicker.Frame.X, titleBarHeight, DatePicker.Frame.Width, DatePicker.Frame.Height);

			// move our label to the top of the action sheet
			titleLabel.Frame = new CGRect (10, 4, owner.Frame.Width - 100, 35);

			// move our button
			doneButton.Frame = new CGRect (actionSheetSize.Width - doneButtonSize.Width - 10, 7, doneButtonSize.Width, doneButtonSize.Height);

			// First set the alpha of the datePickerView to 0 to prepare for a fade in animation
			datePickerView.Alpha = 0;
			// Store datePickerView.Frame in the temporary variable to allow it to be animated
			datePickerFrame = datePickerView.Frame;
			UIView.Animate (0.2, () => {
				datePickerFrame.Offset (0, -100);
				datePickerView.Frame = datePickerFrame;
				datePickerView.Alpha = 1;
			});
		}

		/// <summary>
		/// Dismisses the action sheet date picker
		/// </summary>
		public void Hide (bool animated)
		{
			CGRect frame = datePickerView.Frame;

			UIView.Animate (0.2, () => {
				frame.Offset (0, 100);
				datePickerView.Frame = frame;
				datePickerView.Alpha = 0;
			});
			datePickerView.RemoveFromSuperview ();
		}

		#endregion
	}
}

