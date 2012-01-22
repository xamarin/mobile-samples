using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MWC.BL;
using System.Collections;
using MWC.BL.Managers;

namespace MWC.WP7.ViewModels
{
    public class SpeakerListViewModel : ViewModelBase
    {
        public ObservableCollection<SpeakerListGroupViewModel> Groups { get; set; }

        public SpeakerListViewModel ()
        {
        }

        public void LoadData ()
        {
            var speakers = SpeakerManager.GetSpeakers ();

            Console.WriteLine (speakers);

            /*Root = new RootElement ("Speakers") {
					from speaker in _speakers
                    group speaker by (speaker.Index()) into alpha
						orderby alpha.Key
						select new Section (alpha.Key) {
						from eachSpeaker in alpha
						   select (Element) new MWC.iOS.UI.CustomElements.SpeakerElement (eachSpeaker)
			}};*/
        }
    }

    public class SpeakerListGroupViewModel : ViewModelBase, IEnumerable<SpeakerListItemViewModel>
    {
        public string Title { get; set; }

        public ObservableCollection<SpeakerListItemViewModel> Items { get; set; }

        public SpeakerListGroupViewModel ()
        {
            Title = "";
            Items = new ObservableCollection<SpeakerListItemViewModel> ();
        }

        public override bool Equals (object obj)
        {
            var o = obj as SpeakerListGroupViewModel;
            return (o != null) && Title.Equals (o.Title);
        }

        public override int GetHashCode ()
        {
            return Title.GetHashCode ();
        }

        public IEnumerator<SpeakerListItemViewModel> GetEnumerator ()
        {
            return Items.GetEnumerator ();
        }

        IEnumerator IEnumerable.GetEnumerator ()
        {
            return Items.GetEnumerator ();
        }
    }

    public class SpeakerListItemViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public string ImageUrl { get; set; }

        public SpeakerListItemViewModel ()
        {
        }

        public SpeakerListItemViewModel (Speaker speaker)
        {
            ID = speaker.ID;
            Name = speaker.Name;
            Title = speaker.Title;
            Company = speaker.Company;
            ImageUrl = speaker.ImageUrl;
        }
    }
}
