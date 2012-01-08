using System;
using MWC.BL.Contracts;

namespace MWC.BL
{
	/// <summary>
	/// Just the two useful parts of a Tweet Atom entry
	/// </summary>
	public class Tweet : BusinessEntityBase
	{
		public Tweet () {}
		
		public string Author
		{
			get;set;
		}
		
		public string Title
		{
			get;set;
		}
		
		public string Content
		{
			get;set;
		}
		
		public string Url
		{
			get;set;	
		}
		
		public string ImageUrl
		{
			get;set;
		}
		
		public DateTime Published
		{
			get;set;
		}

		[MWC.DL.SQLite.Ignore]
		public string FormattedAuthor
		{	
			get{
				return Author.Substring (0, Author.IndexOf (" "));
			}
		}
		[MWC.DL.SQLite.Ignore]
		public string RealName
		{	
			get{
				return Author.Substring (Author.IndexOf ("(") + 1, Author.IndexOf (")") - Author.IndexOf ("(") - 1);
			}
		}
		[MWC.DL.SQLite.Ignore]
		public string FormattedTime
		{
			get {
				TimeSpan diff = DateTime.Now - Published;
				if (diff.TotalMinutes < 1)
					return "now";
				if (diff.TotalMinutes < 2)
					return "1 minute ago";
				if (diff.TotalMinutes < 60)
					return string.Format ("{0:0} minutes ago", diff.TotalMinutes);
				if (diff.TotalMinutes < 120)
					return "1 hour ago";
				if (diff.TotalHours < 24)
					return string.Format ("{0:0} hours ago", diff.TotalHours);
				if (diff.TotalHours < 48)
					return "1 day ago";
				return string.Format ("{0:0} days ago", diff.TotalDays);
			}
		}
	}
}

