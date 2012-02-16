using System;
using System.Windows;
using MWC.BL;

namespace MWC.WP7.ViewModels
{
    public class NewsItemViewModel : ViewModelBase
    {
        public int ID { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public DateTime Published { get; set; }
        public string Content { get; set; }
        public Thickness Margin { get; set; }

        public NewsItemViewModel ()
        {
            Margin = new Thickness (0, 12, 0, 0);
        }

        public NewsItemViewModel (RSSEntry item)
            : this ()
        {
            Update (item);
        }

        public void Update (RSSEntry item)
        {
            ID = item.ID;
            Url = item.Url;
            Title = item.Title;
            Published = item.Published;
            Content = item.Content;
        }
    }
}
