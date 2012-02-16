using System;
using System.Windows;
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
        public Thickness Margin { get; set; }

        public string RealName
        {
            get
            {
                return Author.Substring (Author.IndexOf ("(") + 1, Author.IndexOf (")") - Author.IndexOf ("(") - 1);
            }
        }

        public string Username
        {
            get
            {
                return Author.Substring (0, Author.IndexOf (" "));
            }
        }

        public string PublishedAgo
        {
            get
            {
                TimeSpan diff = DateTime.Now - Published;
                if (diff.TotalMinutes < 1)
                    return "now";
                if (diff.TotalMinutes < 2)
                    return "1 min";
                if (diff.TotalMinutes < 60)
                    return string.Format ("{0:0} mins", diff.TotalMinutes);
                if (diff.TotalMinutes < 120)
                    return "1 hour";
                if (diff.TotalHours < 24)
                    return string.Format ("{0:0} hours", diff.TotalHours);
                if (diff.TotalHours < 48)
                    return "1 day";
                return string.Format ("{0:0} days", diff.TotalDays);
            }
        }

        public TweetViewModel ()
        {
            Margin = new Thickness (0, 12, 0, 0);
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
