using System;
using MWC.BL;

namespace MWC.WP7.ViewModels
{
    public class TweetViewModel
    {
        public int ID { get; set; }
   		public string Author { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public string Url { get; set; }
		public string ImageUrl { get; set; }
        public DateTime Published { get; set; }

        public TweetViewModel ()
        {
        }

        public TweetViewModel (Tweet item)
        {
            Update (item);
        }

        public void Update (Tweet item)
        {
            ID = item.ID;
            Author = item.Author;
            Title = item.Title;
            Content = item.Content;
            Url = item.Url;
            ImageUrl = item.ImageUrl;
            Published = item.Published;
        }
    }
}
