using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Dialog;
using Foundation;
using UIKit;
using MWC.BL;
using MWC.iOS.Screens.iPad.Speakers;

namespace MWC.iOS.Screens.iPhone.Speakers {
	/// <summary>
	/// Speakers screen. Derives from Dialog's DialogViewController to do 
	/// the heavy lifting for table population. Also uses ImageLoader in SpeakerCell.cs
	/// </summary>
	public partial class SpeakersScreen : UpdateManagerLoadingDialogViewController {
		protected SpeakerDetailsScreen speakerDetailsScreen;
		IList<Speaker> speakers;
		
		/// <summary>If this is null, on iPhone; otherwise on iPad</summary>
		SpeakerSplitView splitView;
		
		/// <summary>for iPhone</summary>
		public SpeakersScreen () : this (null)
		{
		}
		/// <summary>for iPad</summary>
		public SpeakersScreen (SpeakerSplitView speakerSplitView) : base ()
		{
			splitView = speakerSplitView;
			EnableSearch = true; // requires SpeakerElement to implement Matches()
		}
		
		/// <summary>
		/// Populates the page with exhibitors.
		/// </summary>
		protected override void PopulateTable()
		{
			speakers = BL.Managers.SpeakerManager.GetSpeakers();

			Root = 	new RootElement ("Speakers") {
					from speaker in speakers
                    group speaker by (speaker.Index) into alpha
						orderby alpha.Key
						select new Section (alpha.Key) {
						from eachSpeaker in alpha
						   select (Element) new MWC.iOS.UI.CustomElements.SpeakerElement (eachSpeaker, splitView)
			}};
			// hide search until pull-down
			TableView.ScrollToRow (NSIndexPath.FromRowSection (0,0), UITableViewScrollPosition.Top, false);
		}
		
		public override DialogViewController.Source CreateSizingSource (bool unevenRows)
		{
			return new SpeakersTableSource(this, speakers);
		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
        {
            return true;
        }

	}
	
	/// <summary>
	/// Implement index
	/// </summary>
	public class SpeakersTableSource : DialogViewController.SizingSource {
		IList<Speaker> speakerList;
		public SpeakersTableSource (DialogViewController dvc, IList<Speaker> speakers) : base(dvc)
		{
			speakerList = speakers;
		}

		public override string[] SectionIndexTitles (UITableView tableView)
		{
			var sit = from speaker in speakerList
                    group speaker by (speaker.Index) into alpha
						orderby alpha.Key
						select alpha.Key;
			return sit.ToArray();
		}

		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return 60f;
		}
	}
}