using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using MWC.BL;

namespace MWC.iOS.Screens.iPhone.Exhibitors
{
	/// <summary>
	/// Exhibitors screen. Derives from MonoTouch.Dialog's DialogViewController to do 
	/// the heavy lifting for table population.
	/// </summary>
	public partial class ExhibitorsScreen : DialogViewController
	{
	
		protected ExhibitorDetailsScreen _exhibitorsDetailsScreen;
		IList<Exhibitor> _exhibitors;

		public ExhibitorsScreen () : base (UITableViewStyle.Plain, null)
		{
			if(BL.Managers.UpdateManager.IsUpdating)
			{
				Console.WriteLine("Waiting for updates to finish (exhibitors screen)");
				BL.Managers.UpdateManager.UpdateFinished  += HandleUpdateFinished; 
//				+= (sender, e) => {
//					Console.WriteLine("Updates finished, goign to populate exhibitors screen.");
//					this.InvokeOnMainThread ( () => { this.PopulateTable(); } );
//					//TODO: unsubscribe from static event so GC can clean
//				};
			}
			else
			{
				Console.WriteLine("not updating, populating exhibitors.");
				this.PopulateTable();
			}
		}
		
		/// <summary>
		/// Populates the page with exhibitors.
		/// </summary>
		public void PopulateTable()
		{
			_exhibitors = BL.Managers.ExhibitorManager.GetExhibitors();

			Root = 	new RootElement ("Exhibitors") {
					from exhibitor in _exhibitors
                    group exhibitor by (exhibitor.Index()) into alpha
						orderby alpha.Key
						select new Section (alpha.Key) {
						from eachExhibitor in alpha
						   select (Element) new MWC.iOS.UI.CustomElements.ExhibitorElement (eachExhibitor)
			}};
		}
		
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			BL.Managers.UpdateManager.UpdateFinished -= HandleUpdateFinished; 
		}
		void HandleUpdateFinished(object sender, EventArgs e)
		{
			Console.WriteLine("Updates finished, going to populate table.");
			this.InvokeOnMainThread ( () => {
				this.PopulateTable ();
//				loadingOverlay.Hide ();
			});
		}

		public override DialogViewController.Source CreateSizingSource (bool unevenRows)
		{
			return new ExhibitorsTableSource(this, _exhibitors);
		}
	}

	/// <summary>
	/// Implement index-slider down right side of tableview
	/// </summary>
	public class ExhibitorsTableSource : DialogViewController.SizingSource
	{
		IList<Exhibitor> _exhibitors;
		public ExhibitorsTableSource (DialogViewController dvc, IList<Exhibitor> exhibitors) : base(dvc)
		{
			_exhibitors = exhibitors;
		}

		public override string[] SectionIndexTitles (UITableView tableView)
		{
			var sit = from exhibitor in _exhibitors
                    group exhibitor by (exhibitor.Index()) into alpha
						orderby alpha.Key
						select alpha.Key;
			return sit.ToArray();
		}

//		public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
//		{
//			return 65f;
//		}
	}
	
	/// <summary>
	/// Quick way to incorporate logic into linq
	/// </summary>
	public static class ExhibitorsExtensions
	{
		/// <summary>
		/// anything not A-Z is grouped under the number 1
		/// </summary>
		public static string Index (this Exhibitor exhibitor)
		{
			return IsCapitalLetter(exhibitor.Name[0])?exhibitor.Name[0].ToString().ToUpper():"1";
		}
		static bool IsCapitalLetter (char startsWith)
		{
	    	return ((startsWith >= 'A') && (startsWith <= 'Z'))
				|| ((startsWith >= 'a') && (startsWith <= 'z'));
		}
	}
}