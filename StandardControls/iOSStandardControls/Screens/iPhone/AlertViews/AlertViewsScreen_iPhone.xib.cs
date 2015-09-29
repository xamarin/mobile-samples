using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;
using Example_StandardControls.Controls;
using System.Threading;

namespace Example_StandardControls.Screens.iPhone.AlertViews
{
	public partial class AlertViewsScreen_iPhone : UIViewController
	{
		class CustomButtonsAlertDelegate : UIAlertViewDelegate
		{
			public CustomButtonsAlertDelegate ()
			{
			}

			public override void Canceled (UIAlertView alertView)
			{
				Console.WriteLine ("Alert Cancelled");
			}

			/// <summary>
			/// Runs when any of the custom buttons on the alert are clicked
			/// </summary>
			public override void Clicked (UIAlertView alertview, nint buttonIndex)
			{
				Console.WriteLine (string.Format ("Button {0} clicked", buttonIndex));
			}

			/// <summary>
			/// Runs right after clicked, and before Dismissed
			/// </summary>
			public override void WillDismiss (UIAlertView alertView, nint buttonIndex)
			{
				Console.WriteLine ("Alert will dismiss, button " + buttonIndex.ToString ());
			}

			/// <summary>
			/// Runs after Clicked
			/// </summary>
			public override void Dismissed (UIAlertView alertView, nint buttonIndex)
			{
				Console.WriteLine ("Alert Dismissed, button " + buttonIndex.ToString ());
			}
		}

		/// <summary>
		/// This is here to keep a reference to an alert after the method that creates it
		/// completes. unlike in windows, the .show() method is not blocking (with thread
		/// magic that still keeps the UI unblocked), so after show() returns, the method
		/// will complete and the reference to the alert (and more importantly, the alert
		/// delegate will get garbage collected).
		/// </summary>
		UIAlertView alert;

		public AlertViewsScreen_iPhone (IntPtr handle)
			: base (handle)
		{
		}

		public AlertViewsScreen_iPhone ()
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Title = "Alert Views";

			btnSimpleAlert.TouchUpInside += HandleBtnSimpleAlertTouchUpInside;
			btnCustomButtons.TouchUpInside += HandleBtnCustomButtonsTouchUpInside;
			btnCustomButtonsWithDelegate.TouchUpInside += HandleBtnCustomButtonsWithDelegateTouchUpInside;
			//Fix to Bug 16893
			//Custom Alert Views have been removed in iOS7 - https://developer.apple.com/library/ios/DOCUMENTATION/UIKit/Reference/UIAlertView_Class/UIAlertView/UIAlertView.html#//apple_ref/doc/uid/TP40006802-CH3-DontLinkElementID_1
			if (UIDevice.CurrentDevice.CheckSystemVersion (7, 0))
				btnCustomAlert.TouchUpInside += DontHandleBtnCustomAlertTouchUpInside;
			else
				btnCustomAlert.TouchUpInside += HandleBtnCustomAlertTouchUpInside;
		}

		#region -= simple alert =-

		/// <summary>
		/// Runs when the simple alert button is clicked. launches a very simple alert
		/// that presents an "OK" button, does not use a delegate
		/// </summary>
		void HandleBtnSimpleAlertTouchUpInside (object sender, EventArgs e)
		{
			UIAlertView alert = new UIAlertView () {
				Title = "alert title", Message = "this is a simple alert"
			};
			alert.AddButton ("OK");
			alert.Show ();
		}

		#endregion

		#region -= custom buttons alert =-

		void HandleBtnCustomButtonsTouchUpInside (object sender, EventArgs e)
		{
			// create an alert and add more buttons
			alert = new UIAlertView () {
				Title = "custom buttons alert",
				Message = "this alert has custom buttons"
			};
			alert.AddButton ("custom button 1");
			alert.AddButton ("custom button 2");
			// last button added is the 'cancel' button (index of '2')
			alert.AddButton ("OK");

			alert.Clicked += (object a, UIButtonEventArgs b) => {
				Console.WriteLine (string.Format("Button {0} clicked", b.ButtonIndex));
			};
			alert.Show ();
		}

		#endregion

		#region -= custom buttons with delegate alert =-

		/// <summary>
		/// Runs when the Custom Buttons alert button is clicked. launches an alert with
		/// additional buttons added
		/// </summary>
		void HandleBtnCustomButtonsWithDelegateTouchUpInside (object sender, EventArgs e)
		{
			string[] otherButtons = { "custom button 1", "custom button 2" };

			alert = new UIAlertView ("custom buttons alert", "this alert has custom buttons",
				new CustomButtonsAlertDelegate (), "ok", otherButtons);
			alert.Show ();
		}

		#endregion

		#region -= custom alert =-

		/// <summary>
		/// runs when the custom alert button is pressed. shows the alert and then
		/// kicks off a secondary thread that spins for 5 seconds and then closes
		/// the alert
		/// </summary>
		void HandleBtnCustomAlertTouchUpInside (object sender, EventArgs e)
		{
			alert = new ActivityIndicatorAlertView {
				Message = "performing stuff"
			};
			alert.Show ();

			Thread longRunningProc = new Thread (() => {
				LongRunningProcess (5);
			});
			longRunningProc.Start ();
		}

		/// <summary>
		/// spins a thread for the specified amount of time and then closes the
		/// custom alert. used to simulate a long-running process
		/// </summary>
		void LongRunningProcess (int seconds)
		{
			Thread.Sleep (seconds * 1000);
			((ActivityIndicatorAlertView)alert).Hide (true);
		}

		void DontHandleBtnCustomAlertTouchUpInside (object sender, EventArgs e)
		{
			UIAlertView alert = new UIAlertView () {
				Title = "You are using iOS7",
				Message = "Custom Alert Views Deprecated in iOS7"
			};
			alert.AddButton ("OK");
			alert.Show ();
		}

		#endregion
	}
}

