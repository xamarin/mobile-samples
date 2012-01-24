using System;
using MWC.BL;
using System.Xml;
using System.Xml.Linq;
using System.Linq;

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

        public string Username
        {
            get
            {
                return Author.Substring (0, Author.IndexOf (" "));
            }
        }

        string _cleanContent;

        public string CleanContent
        {
            get
            {
                if (_cleanContent == null) {
                    try {
                        var xml = XDocument.Parse("<body>" + Content + "</body>");
                        _cleanContent = ((XElement)xml.FirstNode).Value;
                    }
                    catch (Exception) {
                        _cleanContent = Content;
                    }
                }
                return _cleanContent;
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
