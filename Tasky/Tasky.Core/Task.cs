using System;
using Tasky.Core.SQLite;
using Tasky.Core.Contracts;

namespace Tasky.Core {
	/// <summary>
	/// Task business object, the SQLite code turns this into a 'table'
	/// with three columns (where ID is the primary key, of course)
	/// </summary>
	public class Task : IBusinessEntity {
		public Task ()
		{
		}

		[PrimaryKey, AutoIncrement]
        public int ID { get; set; }
		public string Name { get; set; }
		public string Notes { get; set; }
	}
}