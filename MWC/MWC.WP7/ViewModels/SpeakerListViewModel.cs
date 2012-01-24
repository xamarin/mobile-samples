using System;
using System.Collections.Generic;
using System.Linq;
using MWC.BL;
using MWC.BL.Managers;

namespace MWC.WP7.ViewModels
{
    public class SpeakerListViewModel : GroupedListViewModel<Speaker, SpeakerListItemViewModel>
    {
        protected override IEnumerable<IGrouping<string, Speaker>> GetGroupedItems ()
        {
            return from s in SpeakerManager.GetSpeakers ()
                   group s by GetGroupKey (s);
        }

        string GetGroupKey (Speaker s)
        {
            return s.Name.Length > 0 ? char.ToLowerInvariant (s.Name[0]).ToString () : "?";
        }

        protected override object GetItemKey (Speaker item)
        {
            return item.Key;
        }
    }

    public class SpeakerListItemViewModel : GroupedListItemViewModel<Speaker>
    {
        public int ID { get; set; }
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

        public override void Update (Speaker speaker)
        {
            SortKey = speaker.Name;

            ID = speaker.ID;
            Name = speaker.Name;
            Title = speaker.Title;
            Company = speaker.Company;
            ImageUrl = speaker.ImageUrl;
        }
    }
}
