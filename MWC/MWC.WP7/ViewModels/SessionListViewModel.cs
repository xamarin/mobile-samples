using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MWC.BL;
using System.Linq;
using System.Collections.Generic;
using MWC.DAL;

namespace MWC.WP7.ViewModels
{
    public class SessionListViewModel : GroupedListViewModel<Session, SessionListItemViewModel>
    {
        public DayOfWeek? FilterDayOfWeek { get; set; }

        protected override IEnumerable<IGrouping<string, Session>> GetGroupedItems ()
        {
            if (FilterDayOfWeek.HasValue) {
                return from s in DataManager.GetSessions ()
                       where s.Start.DayOfWeek == FilterDayOfWeek.Value
                       group s by s.Start.ToString ("s", System.Globalization.CultureInfo.InvariantCulture);
            }
            else {
                return from s in DataManager.GetSessions ()
                       group s by s.Start.ToString ("s", System.Globalization.CultureInfo.InvariantCulture);
            }
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
