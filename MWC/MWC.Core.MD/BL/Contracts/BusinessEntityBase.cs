using System;
using MWC.DL.SQLite;

namespace MWC.BL.Contracts
{
	/// <summary>
	/// Business entity base class. Provides the ID property.
	/// </summary>
	public abstract class BusinessEntityBase : IBusinessEntity
	{
		public BusinessEntityBase ()
		{
		}
		
		/// <summary>
		/// Gets or sets the Database ID.
		/// </summary>
		/// <value>
		/// The ID.
		/// </value>
		[PrimaryKey, AutoIncrement]
        public int ID { get; set; }

	}
}

