using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.BL;

namespace MWC.iOS.Screens.iPhone.Speakers
{
	/// <summary>
	/// Speakers screen. Derives from MonoTouch.Dialog's DialogViewController to do 
	/// the heavy lifting for table population. Also uses ImageLoader in SpeakerCell.cs
	/// </summary>
	public partial class SpeakersScreen : DialogViewController
	{
		protected SpeakerDetailsScreen _speakerDetailsScreen;
		IList<Speaker> _speakers;

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
			_speakers = BL.Managers.SpeakerManager.GetSpeakers();

			Root = 	new RootElement ("Speakers") {
					from speaker in _speakers
                    group speaker by (speaker.Index()) into alpha
						orderby alpha.Key
						select new Section (alpha.Key) {
						from eachSpeaker in alpha
						   select (Element) new MWC.iOS.UI.CustomElements.SpeakerElement (eachSpeaker)
			}};

		}

		public override DialogViewController.Source CreateSizingSource (bool unevenRows)
		{
			return new SpeakersTableSource(this, _speakers);
		}
	}
	
	/// <summary>
	/// Implement index
	/// </summary>
	public class SpeakersTableSource : DialogViewController.SizingSource
	{
		IList<Speaker> _speakers;
		public SpeakersTableSource (DialogViewController dvc, IList<Speaker> speakers) : base(dvc)
		{
			_speakers = speakers;
		}

		public override string[] SectionIndexTitles (UITableView tableView)
		{
			var sit = from speaker in _speakers
                    group speaker by (speaker.Index()) into alpha
						orderby alpha.Key
						select alpha.Key;
			return sit.ToArray();
		}

		public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return 60f;
		}
	}

	public static class SpeakersExtensions
	{
		public static string Index (this Speaker speaker)
		{
			return speaker.Name.Length==0?"A":speaker.Name[0].ToString().ToUpper();
		}
	}
}