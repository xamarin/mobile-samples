using SQLite;

namespace TaskyNetStandard.Core 
{
	/// <summary>
	/// Todo Item business object
	/// </summary>
	public class TodoItem 
	{
		public TodoItem ()
		{
		}

		// SQLite attributes
		[PrimaryKey, AutoIncrement]

        public int ID { get; set; }
		public string Name { get; set; }
		public string Notes { get; set; }
		public bool Done { get; set; }	// TODO: add this field to the user-interface
	}
}