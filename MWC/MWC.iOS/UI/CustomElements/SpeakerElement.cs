using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.BL;

namespace MWC.iOS.UI.CustomElements
{
	/// <summary>
	/// Speaker element.
	/// on iPhone, pushes via MT.D
	/// on iPad, sends view to SplitViewController
	/// </summary>
	public class SpeakerElement : Element 
	{
		static NSString key = new NSString ("SpeakerElement");
		Speaker _speaker;
		MWC.iOS.Screens.iPad.Speakers.SpeakerSplitView _splitView;
		
		/// <summary>
		/// for iPhone
		/// </summary>
		public SpeakerElement (Speaker speaker) : base (speaker.Name)
		{
			this._speaker = speaker;
		}
		/// <summary>
		/// for iPad (SplitViewController)
		/// </summary>
		public SpeakerElement (Speaker speaker, MWC.iOS.Screens.iPad.Speakers.SpeakerSplitView splitView) : base (speaker.Name)
		{
			this._speaker = speaker;
			this._splitView = splitView;
		}
		
		static int count;
		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = tv.DequeueReusableCell (key);
			count++;
			if (cell == null)
			{
				cell = new SpeakerCell (UITableViewCellStyle.Subtitle, key, _speaker);
			}
			else
			{
				((SpeakerCell)cell).UpdateCell (_speaker);
			}
			return cell;
		}

		public override void Selected (DialogViewController dvc, UITableView tableView, MonoTouch.Foundation.NSIndexPath path)
		{
			var sds = new MWC.iOS.Screens.iPhone.Speakers.SpeakerDetailsScreen (_speaker.ID);
			if (_splitView != null)
				_splitView.ShowSpeaker(_speaker.ID, sds);
			else
				dvc.ActivateController (sds);
		}
	}
}