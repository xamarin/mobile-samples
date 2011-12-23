using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using MWC.BL;

namespace MWC.iOS.Screens.iPhone.Speakers
{
	/// <summary>
	/// Speakers screen. Derives from MonoTouch.Dialog's DialogViewController to do 
	/// the heavy lifting for table population.
	/// </summary>
	public partial class SpeakersScreen : DialogViewController
	{
		protected SpeakerDetailsScreen _speakerDetailsScreen;

		public SpeakersScreen () : base (UITableViewStyle.Plain, null)
		{
			if(BL.Managers.UpdateManager.IsUpdating)
			{
				Console.WriteLine("Waiting for updates to finish (speakers screen)");
				BL.Managers.UpdateManager.UpdateFinished += (sender, e) => {
					Console.WriteLine("Updates finished, going to populate speakers screen.");
					this.InvokeOnMainThread ( () => { this.PopulatePage(); } );
					//TODO: unsubscribe from static event so GC can clean
				};
			}
			else
			{
				Console.WriteLine("not updating, populating speakers.");
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
			var speakers = BL.Managers.SpeakerManager.GetSpeakers();
			
			// create a root element and a new section (MT.D requires at least one)
			this.Root = new RootElement ("Speakers");
			section = new Section();

			// for each exhibitor, add a custom ExhibitorElement to the elements collection
			foreach ( var sp in speakers )
			{
				var currentSpeaker = sp; //cloj
				speakerElement = new MonoTouch.Dialog.StringElement (currentSpeaker.Name);
				speakerElement.Tapped += () => {
					int speakerID = currentSpeaker.ID;
					this._speakerDetailsScreen = new SpeakerDetailsScreen ( speakerID );
					this.NavigationController.PushViewController ( this._speakerDetailsScreen, true );
				};
				section.Add(speakerElement);
			}
			
			// add the section to the root
			Root.Add(section);
		}		
	}
}