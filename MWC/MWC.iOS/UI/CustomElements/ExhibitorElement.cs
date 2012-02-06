using System;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using System.Text;

namespace MWC.iOS.UI.CustomElements {
	/// <summary>
	/// Exhibitor element for MonoTouch.Dialog
	/// </summary>
	public class ExhibitorElement : Element, IElementSizing {
		MWC.iOS.Screens.iPad.Exhibitors.ExhibitorSplitView splitView;

		/// <summary>
		/// Gets or sets the exhibitor.
		/// </summary>
		/// <value>
		/// The exhibitor that is used to populate the cell.
		/// </value>
		public BL.Exhibitor Exhibitor {
			get { return exhibitor; }
			set { exhibitor = value; }
		}
		protected BL.Exhibitor exhibitor = null;
		
		/// <summary>
		/// Gets the reuse identifier
		/// </summary>
		protected override MonoTouch.Foundation.NSString CellKey
		{
			get { return cellKey; }
		}
		static NSString cellKey = new NSString("ExhibitorCell");
		
		/// <summary>
		/// for iPhone
		/// </summary>
		public ExhibitorElement (BL.Exhibitor exhibitor) : base ("")
		{
			this.exhibitor = exhibitor;
		}
		/// <summary>
		/// for iPad (SplitViewController)
		/// </summary>
		public ExhibitorElement (BL.Exhibitor showExhibitor, MWC.iOS.Screens.iPad.Exhibitors.ExhibitorSplitView exhibitorSplitView) : base ("")
		{
			exhibitor = showExhibitor;
			splitView = exhibitorSplitView;	// could be null, in current implementation
		}
		
		public override MonoTouch.UIKit.UITableViewCell GetCell (MonoTouch.UIKit.UITableView tv)
		{
			// try and dequeue a cell object to reuse. if one doesn't exist, create a new one
			ExhibitorCell cell = tv.DequeueReusableCell (cellKey) as ExhibitorCell;
			if (cell == null) {
				cell = new UI.CustomElements.ExhibitorCell (exhibitor);
			}
			cell.UpdateCell(exhibitor);

			return cell;
		}
		
		public float GetHeight (MonoTouch.UIKit.UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			return 65f;
		}
	
		/// <summary>Implement MT.D search on name and company properties</summary>
		public override bool Matches (string text)
		{
			return (exhibitor.Name).ToLower ().IndexOf (text.ToLower ()) >= 0;
		}

		public override void Selected (DialogViewController dvc, UITableView tableView, MonoTouch.Foundation.NSIndexPath path)
		{
			var eds = new MWC.iOS.Screens.iPhone.Exhibitors.ExhibitorDetailsScreen (exhibitor.ID);
			
			if (splitView != null)
				splitView.ShowExhibitor(exhibitor.ID, eds);
			else
				dvc.ActivateController (eds);
		}	
	}
}

