namespace SQLite.iOS.Sample
{
	public class ListItem
	{
		[PrimaryKey, AutoIncrement]
		public int Id
		{
			get;
			set;
		}

		[Indexed]
		public int ListId
		{
			get;
			set;
		}

		public bool Completed
		{
			get;
			set;
		}

		public string Text
		{
			get;
			set;
		}
	}
}