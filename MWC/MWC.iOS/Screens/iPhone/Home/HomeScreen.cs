using System;
using System.Collections.Generic;
using System.Drawing;
using Foundation;
using UIKit;
using MWC.BL;
using CoreGraphics;

namespace MWC.iOS.Screens.iPhone.Home {
	/// <summary>
	/// Home screen contains a masthead graphic/ad
	/// plus (iPad only) "what's on" in the next two 'timeslots'
	/// and the "favorites" list.
	/// </summary>
	public partial class HomeScreen : UIViewController {
		Screens.Common.Session.SessionDayScheduleScreen dayScheduleScreen;
		UI.Controls.LoadingOverlay loadingOverlay;
		NSObject ObserverRotation;

		public HomeScreen () : base (AppDelegate.IsPhone ? "HomeScreen_iPhone" : "HomeScreen_iPad", null)
		{
		}
		
		MWC.iOS.AL.DaysTableSource tableSource = null;

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			BL.Managers.UpdateManager.UpdateFinished += HandleUpdateFinished; 
			
			SessionTable.SeparatorColor = UIColor.Black;
			SessionTable.SeparatorStyle = UITableViewCellSeparatorStyle.SingleLine; 

			if (AppDelegate.IsPhone) {
				if (UIScreen.MainScreen.Bounds.Size.Height > 480) { // 4" iPhone5
					MwcLogoImageView.Image = UIImage.FromBundle("/Images/Home-568h");
				} else {
					MwcLogoImageView.Image = UIImage.FromBundle("/Images/Home");
				}
				MwcLogoImageView.Frame = new CGRect(0,0,320,480);
				// added for iOS6
				if (UIDevice.CurrentDevice.CheckSystemVersion (6,0)) {
					var clearView1 = new UIView();
					clearView1.Frame = new CGRect(0,470, 320, 200);
					clearView1.BackgroundColor = UIColor.Clear;
					SessionTable.BackgroundColor = UIColor.Clear;
					SessionTable.BackgroundView = clearView1;
				}
			} else {
				// IsPad
				MwcLogoImageView.Image = UIImage.FromBundle("/Images/Home-Portrait~ipad");
				
				// style the separators to be black
				SessionTable.SeparatorColor = UIColor.Black;
				SessionTable.SeparatorStyle = UITableViewCellSeparatorStyle.SingleLine; 
				UpNextTable.SeparatorColor = UIColor.Black;
				UpNextTable.SeparatorStyle = UITableViewCellSeparatorStyle.SingleLine; 
				FavoritesTable.SeparatorColor = UIColor.Black;
				FavoritesTable.SeparatorStyle = UITableViewCellSeparatorStyle.SingleLine; 

				// the extra 'clear' Views for background are a bit of a hack
				// and not needed on the phone!
				// http://forums.macrumors.com/showthread.php?t=901706
				var clearView1 = new UIView();
				clearView1.Frame = new CGRect(0,470, 320, 200);
				clearView1.BackgroundColor = UIColor.Clear;
				SessionTable.BackgroundColor = UIColor.Clear;
				SessionTable.BackgroundView = clearView1;
				
				var clearView2 = new UIView();
				clearView2.Frame = new CGRect(0,470+210, 320, 320);
				clearView2.BackgroundColor = UIColor.Clear;
				UpNextTable.BackgroundColor = UIColor.Clear;
				UpNextTable.BackgroundView = clearView2;
				
				var clearView3 = new UIView();
				clearView3.Frame = new CGRect(768-320,470, 320, 420);				
				clearView3.BackgroundColor = UIColor.Clear;
				FavoritesTable.BackgroundColor = UIColor.Clear;
				FavoritesTable.BackgroundView = clearView3;	
			}

			//TODO: Craig, i want to look at encapsulating this at the BL layer, 
			// i don't know if that's a feasible approach, but i think this is 
			// generally a good pattern.
			//
			// if we're in the process of updating, populate the table when it's done
			// alas, if we keep it in the app layer, it gives us an opportunity to 
			// show a spinner over the table with an "updating" message.
			if(BL.Managers.UpdateManager.IsUpdating)
			{
				if (AppDelegate.IsPhone)
					loadingOverlay = new MWC.iOS.UI.Controls.LoadingOverlay (SessionTable.Frame);
				else {	// IsPad (rotates!)
					var overlayFrame = View.Frame;
					overlayFrame.Y = 330;
					overlayFrame.Height = 768 - 330;
					loadingOverlay = new MWC.iOS.UI.Controls.LoadingOverlay (overlayFrame);
					loadingOverlay.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
				}
				View.AddSubview (loadingOverlay);
				
				ConsoleD.WriteLine("UpdateManager.IsUpdating ~ wait for them to finish");
			}
			else { PopulateTable(); }
		}

		[Obsolete]
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			BL.Managers.UpdateManager.UpdateFinished -= HandleUpdateFinished; 
		}
		void HandleUpdateFinished(object sender, EventArgs e)
		{
			ConsoleD.WriteLine("Updates finished, going to populate table.");
			InvokeOnMainThread ( () => {
				PopulateTable ();
				if (loadingOverlay != null)
					loadingOverlay.Hide ();
			});
		}

		[Obsolete]
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return AppDelegate.IsPad;
		}

		/// <summary>iPad only method</summary>
		void SessionClicked (object sender, MWC.iOS.AL.FavoriteClickedEventArgs args)
		{
			var s = new MWC.iOS.Screens.iPad.SessionPopupScreen(args.SessionClicked, this);

			//HACK: UIPresentationStyle doesn't exist anymore
			//s.UIPresentationStyle = UIModalPresentationStyle.FormSheet;

			PresentViewController (s, true, null);
		}

		/// <summary>iPad only method</summary>
		public void SessionClosed(bool wasDirty) 
		{
			if (wasDirty)
				PopulateiPadTables();
		}

		protected void PopulateTable ()
		{
			tableSource = new MWC.iOS.AL.DaysTableSource();
			SessionTable.Source = tableSource;
			SessionTable.ReloadData();
			tableSource.DayClicked += delegate (object sender, MWC.iOS.AL.DayClickedEventArgs e) {
				LoadSessionDayScreen (e.DayName, e.Day);
			};
			
			if (AppDelegate.IsPad)
				PopulateiPadTables();
		}
		/// <summary>iPad only method: the UpNext and Favorites tables</summary>
		void PopulateiPadTables()
		{
			var uns = new MWC.iOS.AL.UpNextTableSource();
			UpNextTable.Source = uns;
			uns.SessionClicked += SessionClicked;
			UpNextTable.ReloadData();
			
			var fs = new MWC.iOS.AL.FavoritesTableSource();
			FavoritesTable.Source = fs;
			fs.FavoriteClicked += SessionClicked;
			FavoritesTable.ReloadData ();
		}

		/// <summary>
		/// Show the session info, push navctrl for iPhone, in a modal overlay for iPad
		/// </summary>
		protected void LoadSessionDayScreen (string dayName, int day)
		{
			if (AppDelegate.IsPhone) {
				dayScheduleScreen = new MWC.iOS.Screens.Common.Session.SessionDayScheduleScreen (dayName, day, null);
				NavigationController.PushViewController (dayScheduleScreen, true);				
			} else {
				var nvc = ParentViewController;
				var tab = nvc.ParentViewController as MWC.iOS.Screens.Common.TabBarController;
				tab.SelectedIndex = 1;
				tab.ShowSessionDay(day);
			}
		}
		
		public bool IsPortrait {
			get {
				return InterfaceOrientation == UIInterfaceOrientation.Portrait 
					|| InterfaceOrientation == UIInterfaceOrientation.PortraitUpsideDown;
			}
		}

		/// <summary>
		/// Home layout changes on rotation
		/// </summary>
		protected void OnDeviceRotated (NSNotification notification)
		{
			if (AppDelegate.IsPad) {
				if (IsPortrait) {
					MwcLogoImageView.Image = UIImage.FromBundle("/Images/Home-Portrait~ipad");
					SessionTable.Frame   = new CGRect(0,      300, 320, 320);
					UpNextTable.Frame    = new CGRect(0,      640, 320, 300);
					FavoritesTable.Frame = new CGRect(768-400,300, 400, 560);
				}
				else
				{	// IsLandscape
					MwcLogoImageView.Image = UIImage.FromBundle("/Images/Home-Landscape~ipad");
					SessionTable.Frame   = new CGRect(0,   300, 320, 390);
					UpNextTable.Frame    = new CGRect(350, 300, 320, 390);
					FavoritesTable.Frame = new CGRect(704, 300, 320, 390);
				}
			}
		}

		/// <summary>
		/// Is called when the view is about to appear on the screen. We use this method to hide the 
		/// navigation bar.
		/// </summary>
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			NavigationController.SetNavigationBarHidden (true, animated);
			
			if (AppDelegate.IsPad) {
				OnDeviceRotated(null);
				
				// We attempt to re-populate to refresh the 'Favorites' and 'Up Next' lists (which need to change over time)
				if (!BL.Managers.UpdateManager.IsUpdating)
					PopulateiPadTables();
			
				ObserverRotation = NSNotificationCenter.DefaultCenter.AddObserver(
					AppDelegate.NotificationOrientationDidChange, OnDeviceRotated);
				UIDevice.CurrentDevice.BeginGeneratingDeviceOrientationNotifications();
			}
		}
		
		/// <summary>
		/// Is called when the another view will appear and this one will be hidden. We use this method
		/// to show the navigation bar again.
		/// </summary>
		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			NavigationController.SetNavigationBarHidden (false, animated);
	
			if (AppDelegate.IsPad) {
				UIDevice.CurrentDevice.EndGeneratingDeviceOrientationNotifications();
				NSNotificationCenter.DefaultCenter.RemoveObserver(ObserverRotation);
			}
		}
	}
}