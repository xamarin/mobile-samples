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
			
			this.PopulateTable();
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}
		
		protected void PopulateTable()
		{
			this._tableSource = new MWC.AL.HomeTableSource(BL.Managers.SessionManager.GetSessions());
			this.SessionTable.Source = this._tableSource;					
		}
	}
}

