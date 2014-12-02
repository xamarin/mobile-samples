using System;
using UIKit;
using System.Drawing;
using CoreGraphics;

namespace Example_StandardControls.Controls
{
	/// <summary>
	/// A class to show a date picker. To use, create a new UIDatePicker, set the Title, modify 
	/// any settings on the DatePicker property, and call Show(). It will 
	/// automatically dismiss when the user clicks "Done," or you can call Hide() to dismiss it 
	/// manually.
	/// </summary>
	[Foundation.Register("SlideOnDatePicker")]
	public class UIViewDatePicker
	{
		#region -= declarations =-
		
		UIView datePickerView;
		UIButton doneButton = UIButton.FromType (UIButtonType.RoundedRect);
		UIView owner;
		UILabel titleLabel = new UILabel ();
		
		#endregion
		
		#region -= properties =-
		
		/// <summary>
		/// Set any datepicker properties here
		/// </summary>
		public UIDatePicker DatePicker
		{
			get { return datePicker; }
			set { datePicker = value; }
		}
		UIDatePicker datePicker = new UIDatePicker(CGRect.Empty);
		
		/// <summary>
		/// The title that shows up for the date picker
		/// </summary>
		public string Title
		{
			get { return titleLabel.Text; }
			set { titleLabel.Text = value; }
		}
		
		#endregion
		
		#region -= constructor =-
		
		/// <summary>
		/// 
		/// </summary>
		public UIViewDatePicker (UIView owner)
		{
			// save our uiview owner
			this.owner = owner;
	
			// configure the title label
			titleLabel.BackgroundColor = UIColor.Clear;
			titleLabel.TextColor = UIColor.LightTextColor;
			titleLabel.Font = UIFont.BoldSystemFontOfSize (18);
			
			// configure the done button
			doneButton.SetTitle ("done", UIControlState.Normal);
			//doneButton.TouchUpInside += (s, e) => { actionSheet.DismissWithClickedButtonIndex (0, true); };

			doneButton.TouchUpInside += async (sender, e) => { 
				Hide(true);
			};
			
			// create + configure the action sheet
			datePickerView = new UIView () {  };
			//actionSheet.Clicked += (s, e) => { Console.WriteLine ("Clicked on item {0}", e.ButtonIndex); };
	
			// add our controls to the action sheet
			datePickerView.AddSubview (datePicker);
			datePickerView.AddSubview (titleLabel);
			datePickerView.AddSubview (doneButton);
		}
		
		#endregion
		
		#region -= public methods =-
			
		/// <summary>
		/// Shows the action sheet picker from the view that was set as the owner.
		/// </summary>
		public async void Show ()
		{
			// declare vars
			float titleBarHeight = 40;
			CGSize doneButtonSize = new CGSize (71, 30);
			CGSize actionSheetSize = new CGSize (owner.Frame.Width, datePicker.Frame.Height + titleBarHeight);
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
			datePicker.Frame = new CGRect 
				((nfloat)datePicker.Frame.X, titleBarHeight, datePicker.Frame.Width, datePicker.Frame.Height);
			
			// move our label to the top of the action sheet
			titleLabel.Frame = new CGRect (10, 4, owner.Frame.Width - 100, 35);
			
			// move our button
			doneButton.Frame = new CGRect (actionSheetSize.Width - doneButtonSize.Width - 10, 7, doneButtonSize.Width, doneButtonSize.Height);

			// First set the alpha of the datePickerView to 0 to prepare for a fade in animation
			datePickerView.Alpha = 0;
			// Store datePickerView.Frame in the temporary variable to allow it to be animated
			datePickerFrame = datePickerView.Frame;
			await UIView.AnimateAsync (0.2, () => {
				datePickerFrame.Offset(0, -100);
				datePickerView.Frame = datePickerFrame;
				datePickerView.Alpha = 1;
			});

		}
		
		/// <summary>
		/// Dismisses the action sheet date picker
		/// </summary>
		public async void Hide (bool animated)
		{
			CGRect frame = datePickerView.Frame;

			await UIView.AnimateAsync (0.2, () => {
				frame.Offset(0, 100);
				datePickerView.Frame = frame;
				datePickerView.Alpha = 0;
			});
			datePickerView.RemoveFromSuperview ();
		}
		
		#endregion		
	}
}

