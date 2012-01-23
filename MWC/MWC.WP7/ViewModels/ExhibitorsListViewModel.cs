using System;
using System.Collections.Generic;
using System.Linq;
using MWC.BL;
using MWC.DAL;

namespace MWC.WP7.ViewModels
{
    public class ExhibitorListViewModel : GroupedListViewModel<Exhibitor, ExhibitorListItemViewModel>
    {
        protected override IEnumerable<IGrouping<string, Exhibitor>> GetGroupedItems ()
        {
            return from s in DataManager.GetExhibitors ()
                   group s by GetGroupKey (s);
        }

        string GetGroupKey (Exhibitor item)
        {
            var name = item.Name.Trim ();
            if (name.Length == 0 || !char.IsLetter (name[0])) {
                return "#";
            }
            else {
                return char.ToLowerInvariant (name[0]).ToString ();
            }
        }

        protected override object GetItemKey (Exhibitor item)
        {
            return item.ID;
        }
    }

    public class ExhibitorListItemViewModel : GroupedListItemViewModel<Exhibitor>
    {
        public int ID { get; set; }

        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Locations { get; set; }
        public string ImageUrl { get; set; }

        public string CityAndCountry
        {
            get
            {
                return City + ", " + Country;
            }
        }

        public override void Update (Exhibitor item)
        {
            ID = item.ID;
            Name = item.Name.Trim ();
            City = item.City;
            Country = item.Country;
            Locations = item.Locations;
            ImageUrl = item.ImageUrl;

            SortKey = Name;
        }
    }
}
