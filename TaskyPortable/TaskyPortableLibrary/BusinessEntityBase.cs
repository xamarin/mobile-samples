using System;
using Tasky.DL.SQLite;

namespace Tasky.BL.Contracts {
	/// <summary>
	/// Business entity base class. Provides the ID property.
	/// </summary>
	public abstract class BusinessEntityBase : IBusinessEntity {
		public BusinessEntityBase ()
		{
		}
		
		/// <summary>
		/// Gets or sets the Database ID.
		/// </summary>
		[PrimaryKey, AutoIncrement]
        public int ID { get; set; }
	}
}

