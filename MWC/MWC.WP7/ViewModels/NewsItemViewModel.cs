using System;
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

        public NewsItemViewModel ()
        {
        }

        public NewsItemViewModel (RSSEntry item)
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
