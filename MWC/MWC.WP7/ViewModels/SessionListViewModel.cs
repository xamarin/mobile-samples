using System;
using System.Collections.Generic;
using System.Linq;
using MWC.BL;
using MWC.BL.Managers;
using System.Windows;

namespace MWC.WP7.ViewModels
{
    public class SessionListViewModel : GroupedListViewModel<Session, SessionListItemViewModel>
    {
        public bool FilterFavorites { get; set; }
        public DayOfWeek? FilterDayOfWeek { get; set; }

        public Visibility NoFavoritesVisibility
        {
            get
            {
                return (FilterFavorites && Groups.Count == 0) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public Visibility ListVisibility
        {
            get
            {
                return (NoFavoritesVisibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        protected override IEnumerable<IGrouping<string, Session>> GetGroupedItems ()
        {
            var allSessions = SessionManager.GetSessions ();

            if (FilterFavorites) {
                var favs = FavoritesManager
                    .GetFavorites ()
                    .Select (f => f.SessionKey)
                    .ToDictionary (x => x);
                return from s in allSessions
                       where favs.ContainsKey (s.Key)
                       group s by GetGroupKey (s);
            }
            else if (FilterDayOfWeek.HasValue) {
                return from s in allSessions
                       where s.Start.DayOfWeek == FilterDayOfWeek.Value
                       group s by GetGroupKey (s);
            }
            else {
                return from s in allSessions
                       group s by GetGroupKey (s);
            }
        }

        string GetGroupKey (Session s)
        {
            return s.Start.ToString ("s", System.Globalization.CultureInfo.InvariantCulture);
        }

        protected override string GetGroupTitle (string groupKey)
        {
            var time = DateTime.Parse (groupKey);

            return string.Format ("{0:dddd} {0:t}", time).ToLower ();
        }

        protected override object GetItemKey (Session item)
        {
            return item.Key;
        }
    }

    public class SessionListItemViewModel : GroupedListItemViewModel<Session>
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public string Room { get; set; }

        public override void Update (Session item)
        {
            ID = item.ID;
            Title = item.Title;
            Room = string.IsNullOrWhiteSpace (item.Room) ? "Unknown Location" : item.Room;
            Start = item.Start;
            SortKey = Start.ToString ("s", System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
