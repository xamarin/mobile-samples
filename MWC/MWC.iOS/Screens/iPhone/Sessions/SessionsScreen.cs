using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using MWC.BL;
using MWC.iOS.Screens.Common.Session;

namespace MWC.iOS.Screens.iPhone.Sessions
{
	/// <summary>
	/// Speakers screen. Derives from MonoTouch.Dialog's DialogViewController to do 
	/// the heavy lifting for table population.
	/// </summary>
	public partial class SessionsScreen : DialogViewController
	{
		protected SessionDetailsScreen _sessionDetailsScreen;

		public SessionsScreen () : base (UITableViewStyle.Plain, null)
		{
			if(BL.Managers.UpdateManager.IsUpdating)
			{
				Console.WriteLine("Waiting for updates to finish (sessions screen)");
				BL.Managers.UpdateManager.UpdateFinished += (sender, e) => {
					Console.WriteLine("Updates finished, going to populate sessions screen.");
					this.InvokeOnMainThread ( () => { this.PopulatePage(); } );
					//TODO: unsubscribe from static event so GC can clean
				};
			}
			else
			{
				Console.WriteLine("not updating, populating sessions.");
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
			MonoTouch.Dialog.StringElement speakerElement;

			// get the exhibitors from the database
			var sessions = BL.Managers.SessionManager.GetSessions ();
			
			// create a root element and a new section (MT.D requires at least one)
			this.Root = new RootElement ("Sessions");
			section = new Section();

			// for each exhibitor, add a custom ExhibitorElement to the elements collection
			foreach ( var se in sessions )
			{
				var currentSession = se; //cloj
				speakerElement = new MonoTouch.Dialog.StringElement (currentSession.Title);
				speakerElement.Tapped += () => {
					int sessionID = currentSession.ID;
					this._sessionDetailsScreen = new SessionDetailsScreen ( sessionID );
					this.NavigationController.PushViewController ( this._sessionDetailsScreen, true );
				};
				section.Add(speakerElement);
			}
			
			// add the section to the root
			Root.Add(section);
		}		
	}
}