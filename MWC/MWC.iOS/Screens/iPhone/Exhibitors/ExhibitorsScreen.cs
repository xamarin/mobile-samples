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
		public ExhibitorsScreen () : base (UITableViewStyle.Plain, null)
		{
			if(BL.Managers.UpdateManager.IsUpdating)
			{
				Console.WriteLine("Waiting for updates to finish (exhibitors screen)");
				BL.Managers.UpdateManager.UpdateFinished += (sender, e) => {
					Console.WriteLine("Updates finished, goign to populate exhibitors screen.");
					this.InvokeOnMainThread ( () => { this.PopulatePage(); } );
					//TODO: unsubscribe from static event so GC can clean
				};
			}
			else
			{
				Console.WriteLine("not updating, populating exhibitors.");
				this.PopulatePage();
			}
		}
		
		/// <summary>
		/// Populates the page with exhibitors.
		/// </summary>
		public void PopulatePage()
		{
			// declare vars
			Section section;
			UI.CustomElements.ExhibitorElement exhibitorElement;

			// get the exhibitors from the database
			var exhibitors = BL.Managers.ExhibitorManager.GetExhibitors();
			
			// create a root element and a new section (MT.D requires at least one)
			this.Root = new RootElement ("Exhibitors");
			section = new Section();

			// for each exhibitor, add a custom ExhibitorElement to the elements collection
			foreach ( var ex in exhibitors )
			{
				exhibitorElement = new UI.CustomElements.ExhibitorElement (ex);
				section.Add(exhibitorElement);
			}
			
			// add the section to the root
			Root.Add(section);
		}		
	}
}