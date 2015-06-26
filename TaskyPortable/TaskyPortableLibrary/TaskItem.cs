using System;
using Tasky.BL.Contracts;
using SQLite;

namespace Tasky.BL
{
	/// <summary>
	/// Represents a Task.
	/// </summary>
	public class TaskItem : IBusinessEntity
	{
		public TaskItem ()
		{
		}

		[PrimaryKey, AutoIncrement]
        public int ID { get; set; }
		public string Name { get; set; }
		public string Notes { get; set; }
		// new property
		public bool Done { get; set; }
	}
}

