using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using MWC.iOS.AL;

namespace MWC.iOS.Screens.iPhone.Exhibitors
{
	public partial class ExhibitorsScreen : DialogViewController
	{
		IList<Exhibitor> _exhibitors;
		
		public ExhibitorsScreen () : base (UITableViewStyle.Grouped, null)
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

			
//			Root = new RootElement ("ExhibitorsScreen") {
//				new Section ("First Section"){
//					new StringElement ("Hello", () => {
//				new UIAlertView ("Hola", "Thanks for tapping!", null, "Continue").Show (); 
//			}),
//					new EntryElement ("Name", "Enter your name", String.Empty)
//				},
//				new Section ("Second Section"){
//				},
//			};
		}
		
		public void PopulatePage()
		{
			var exhs = BL.Managers.ExhibitorManager.GetExhibitors();
			this._exhibitors = new List<Exhibitor>();
			foreach(var ex in exhs) { this._exhibitors.Add(ex as Exhibitor); }
			
			//this._exhibitors = BL.Managers.ExhibitorManager.GetExhibitors();
  			if(this._exhibitors != null)
			{
//				BindingContext bc = new BindingContext(this, this._exhibitors, "Exhibitors");
//				this.Root = bc.Root;
			}
		}
		
	}
}
