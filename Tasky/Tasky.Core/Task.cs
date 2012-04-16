using System;
using Tasky.Core.SQLite;
using Tasky.Core.Contracts;

namespace Tasky.Core
{
	/// <summary>
	/// Represents a Task.
	/// </summary>
	public class Task : IBusinessEntity
	{
		public Task ()
		{
		}

		[PrimaryKey, AutoIncrement]
        public int ID { get; set; }
		public string Name { get; set; }
		public string Notes { get; set; }
	}
}

