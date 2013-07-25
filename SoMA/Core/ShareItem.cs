using System;
using SQLite;

namespace Core
{
	public class ShareItem
	{
		public ShareItem ()
		{
		}

		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public string ImagePath { get; set; }
		public string ThumbImagePath { get; set; }
		public string Location { get; set; }
		public string Text { get; set; }
		public string Link { get; set; }
		public string SocialType { get; set; }
	}
}

