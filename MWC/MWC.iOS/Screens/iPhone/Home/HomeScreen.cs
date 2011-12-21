using MonoTouch.UIKit;
using System.Drawing;
using System;
using MonoTouch.Foundation;
using System.Collections.Generic;

using MWC.BL;

namespace MWC.iOS.Screens.iPhone.Home
{
	/// <summary>
	/// Home screen contains a masthead graphic/ad
	/// plus "what's on" in the next two 'timeslots'
	/// </summary>
	public partial class HomeScreen : UIViewController
	{
		public HomeScreen () : base ("HomeScreen", null)
		{
		}
		
		AL.HomeTableSource _tableSource = null;

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			this.MwcLogoImageView.Image = UIImage.FromBundle("/Images/MWCLogo");
			this.XamLogoImageView.Image = UIImage.FromBundle("/Images/XamLogo");
			
			//TODO: Craig, i want to look at encapsulating this at the BL layer, 
			// i don't know if that's a feasible approach, but i think this is 
			// generally a good pattern.
			//
			// if we're in the process of updateding, populate the table when it's done
			// alas, if we keep it in the app layer, it gives us an opportunity to 
			// show a spinner over the table with an "updating" message.
			if(BL.Managers.UpdateManager.IsUpdating)
			{
				Console.WriteLine("Waiting for updates to finish");
				BL.Managers.UpdateManager.UpdateFinished += (sender, e) => {
					Console.WriteLine("Updates finished, goign to populate table.");
					this.InvokeOnMainThread ( () => { this.PopulateTable(); } );
					//TODO: unsubscribe from static event so GC can clean
				};
			}
			else { this.PopulateTable(); }

		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}
		
		protected void PopulateTable()
		{
			Console.WriteLine ("PopulateTable called()");
			this._tableSource = new MWC.AL.HomeTableSource(BL.Managers.SessionManager.GetSessions());
			this.SessionTable.Source = this._tableSource;	
			this.SessionTable.ReloadData();
		}
	}
}

