using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.BL;

namespace MWC.iOS.Screens.iPhone {
	/// <summary>
	/// Base class for loading screens: Home, Speakers, Sessions
	/// </summary>
	/// <remarks>
	/// This ViewController implements the data loading via a virtual
	/// method LoadData(), which must call StopLoadingScreen()
	/// </remarks>
	public partial class UpdateManagerLoadingDialogViewController : DialogViewController {
		UI.Controls.LoadingOverlay loadingOverlay;

		/// <summary>
		/// Set pushing=true so that the UINavCtrl 'back' button is enabled
		/// </summary>
		public UpdateManagerLoadingDialogViewController () : base (UITableViewStyle.Plain, null, true)
		{
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			BL.Managers.UpdateManager.UpdateFinished += HandleUpdateFinished;
		}
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			if(BL.Managers.UpdateManager.IsUpdating) {
				if (loadingOverlay == null) {
					loadingOverlay = new MWC.iOS.UI.Controls.LoadingOverlay (TableView.Frame);
					// because DialogViewController is a UITableViewController,
					// we need to step OVER the UITableView, otherwise the loadingOverlay
					// sits *in* the scrolling area of the table
					View.Superview.Add (loadingOverlay); 
					View.Superview.BringSubviewToFront (loadingOverlay);
				}
				Console.WriteLine("Waiting for updates to finish before displaying table.");
			} else {
				loadingOverlay = null;
				Console.WriteLine("Not updating, populating table.");
				PopulateTable();
			}
		}
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			BL.Managers.UpdateManager.UpdateFinished -= HandleUpdateFinished; 
		}
		void HandleUpdateFinished(object sender, EventArgs e)
		{
			Console.WriteLine("Updates finished, going to populate table.");
			InvokeOnMainThread ( () => {
				PopulateTable ();
				if (loadingOverlay != null)
					loadingOverlay.Hide ();
				loadingOverlay = null;
			});
		}
		
		/// <summary>
		/// Your implementation should get data from the UpdateManager 
		/// and set the Root for the DialogViewController
		/// </summary>
		protected virtual void PopulateTable()
		{
		}
	}
}