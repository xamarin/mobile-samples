using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Dialog;
using Foundation;
using UIKit;
using MWC.BL;
using MWC.iOS.Screens.iPad.Exhibitors;

namespace MWC.iOS.Screens.iPhone.Exhibitors {
	/// <summary>
	/// Exhibitors screen. Derives from Dialog's DialogViewController to do 
	/// the heavy lifting for table population.
	/// </summary>
	/// <remarks>
	/// This class initially inherited from UpdateManagerLoadingDialogViewController
	/// but when we split the data download into two parts, the methods from that
	/// baseclass we duplicated here (due to different eventhandlers)
	/// </remarks>
	public partial class ExhibitorsScreen : DialogViewController {
		protected ExhibitorDetailsScreen exhibitorsDetailsScreen;
		IList<Exhibitor> exhibitors;
		
		/// <summary>
		/// Set pushing=true so that the UINavCtrl 'back' button is enabled
		/// </summary>
		public ExhibitorsScreen () : base (UITableViewStyle.Plain, null, true)
		{
			EnableSearch = true; // requires ExhibitorElement to implement Matches()
		}
		
		ExhibitorSplitView splitView;
		public ExhibitorsScreen (ExhibitorSplitView exhibitorSplitView) : base (UITableViewStyle.Plain, null)
		{
			splitView = exhibitorSplitView;
			EnableSearch = true; // requires ExhibitorElement to implement Matches()
		}

		/// <summary>
		/// Populates the page with exhibitors.
		/// </summary>
		protected void PopulateTable()
		{
			exhibitors = BL.Managers.ExhibitorManager.GetExhibitors();

			Root = 	new RootElement ("Exhibitors") {
					from exhibitor in exhibitors
                    group exhibitor by (exhibitor.Index) into alpha
						orderby alpha.Key
						select new Section (alpha.Key) {
						from eachExhibitor in alpha
						   select (Element) new MWC.iOS.UI.CustomElements.ExhibitorElement (eachExhibitor, splitView)
			}};
			// hide search until pull-down
			TableView.ScrollToRow (NSIndexPath.FromRowSection (0,0), UITableViewScrollPosition.Top, false);
		}

		public override DialogViewController.Source CreateSizingSource (bool unevenRows)
		{
			return new ExhibitorsTableSource (this, exhibitors);
		}

		#region UpdatemanagerLoadingDialogViewController copied here, for Exhibitor-specific behaviour
		UI.Controls.LoadingOverlay loadingOverlay;
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			BL.Managers.UpdateManager.UpdateExhibitorsStarted += HandleUpdateStarted;
			BL.Managers.UpdateManager.UpdateExhibitorsFinished += HandleUpdateFinished;
		}
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			if(BL.Managers.UpdateManager.IsUpdatingExhibitors) {
				if (loadingOverlay == null) {
					loadingOverlay = new MWC.iOS.UI.Controls.LoadingOverlay (View.Frame);
					// because DialogViewController is a UITableViewController,
					// we need to step OVER the UITableView, otherwise the loadingOverlay
					// sits *in* the scrolling area of the table
					if (View.Superview != null) {
						// TODO: see when Superview is null
						View.Superview.Add (loadingOverlay); 
						View.Superview.BringSubviewToFront (loadingOverlay);
					}
				}
				ConsoleD.WriteLine("Waiting for updates to finish before displaying table.");
			} else {
				loadingOverlay = null;
				if (Root == null || Root.Count == 0) {
					ConsoleD.WriteLine("Not updating, populating table.");
					PopulateTable();
				} else ConsoleD.WriteLine ("Exhibitors already populated");
			}
		}

		[Obsolete]
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			BL.Managers.UpdateManager.UpdateExhibitorsFinished -= HandleUpdateFinished; 
		}

		void HandleUpdateStarted(object sender, EventArgs e)
		{
			ConsoleD.WriteLine("Updates starting, need to create overlay.");
			InvokeOnMainThread ( () => {
				if (loadingOverlay == null) {
					loadingOverlay = new MWC.iOS.UI.Controls.LoadingOverlay (TableView.Frame);
					// because DialogViewController is a UITableViewController,
					// we need to step OVER the UITableView, otherwise the loadingOverlay
					// sits *in* the scrolling area of the table
					if (View.Superview != null) { //TODO: see when this is null
						View.Superview.Add (loadingOverlay); 
						View.Superview.BringSubviewToFront (loadingOverlay);
					}
				}
			});
		}
		void HandleUpdateFinished(object sender, EventArgs e)
		{
			ConsoleD.WriteLine("Updates finished, going to populate table.");
			InvokeOnMainThread ( () => {
				PopulateTable ();
				if (loadingOverlay != null)
					loadingOverlay.Hide ();
				loadingOverlay = null;
			});
		}
		#endregion
	}

	/// <summary>
	/// Implement index-slider down right side of tableview
	/// </summary>
	public class ExhibitorsTableSource : DialogViewController.SizingSource {
		IList<Exhibitor> exhibitorList;
		public ExhibitorsTableSource (DialogViewController dvc, IList<Exhibitor> exhibitors) : base(dvc)
		{
			exhibitorList = exhibitors;
		}

		public override string[] SectionIndexTitles (UITableView tableView)
		{
			var sit = from exhibitor in exhibitorList
                    group exhibitor by (exhibitor.Index) into alpha
						orderby alpha.Key
						select alpha.Key;
			return sit.ToArray();
		}

//		public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
//		{
//			return 65f;
//		}
	}
}