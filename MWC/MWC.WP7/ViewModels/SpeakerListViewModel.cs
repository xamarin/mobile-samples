using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MWC.BL;
using MWC.BL.Managers;
using System.Diagnostics;

namespace MWC.WP7.ViewModels
{
    public class SpeakerListViewModel : ViewModelBase
    {
        public ObservableCollection<SpeakerListGroupViewModel> Groups { get; set; }

        public SpeakerListViewModel ()
        {
            Groups = new ObservableCollection<SpeakerListGroupViewModel> ();
        }

        public void Update ()
        {
            var speakerGroups = from s in SpeakerManager.GetSpeakers ()
                                let groupTitle = s.Name.Length > 0 ? char.ToLowerInvariant (s.Name[0]) : '?'
                                group s by groupTitle;

            var oldGroups = Groups.ToList ();
            var newGroups = new List<SpeakerListGroupViewModel> ();

            foreach (var sg in (from x in speakerGroups orderby x.Key select x)) {

                var group = oldGroups.FirstOrDefault (g => g.Title[0] == sg.Key);

                if (group == null) {
                    group = new SpeakerListGroupViewModel {
                        Title = sg.Key.ToString (),
                    };
                }

                group.Update (sg);
                newGroups.Add (group);
            }

            Groups = new ObservableCollection<SpeakerListGroupViewModel> (newGroups.OrderBy (x => x.Title));
            OnPropertyChanged ("Groups");
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

        public void Update (IEnumerable<Speaker> items)
        {
            //
            // Find or create ViewModels for each item
            //
            var oldViewModels = Items.ToDictionary (i => i.Key);
            var newItems = new List<SpeakerListItemViewModel> ();
            foreach (var i in items) {
                var vm = default (SpeakerListItemViewModel);
                if (!oldViewModels.TryGetValue (i.Key, out vm)) {
                    vm = new SpeakerListItemViewModel ();
                }
                vm.Update (i);
                newItems.Add (vm);
            }

            //
            // Update the list
            //
            Items = new ObservableCollection<SpeakerListItemViewModel> (newItems.OrderBy (x => x.Name));
            OnPropertyChanged ("Items");
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
        public string Key { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public string ImageUrl { get; set; }

        public string TitleAndCompany
        {
            get
            {
                if (string.IsNullOrEmpty (Title)) {
                    return Company;
                }
                else {
                    return string.Format ("{0}, {1}", Title, Company);
                }
            }
        }

        public SpeakerListItemViewModel ()
        {
        }

        public void Update (Speaker speaker)
        {
            Key = speaker.Key;
            Name = speaker.Name;
            Title = speaker.Title;
            Company = speaker.Company;
            ImageUrl = speaker.ImageUrl;
        }
    }
}
