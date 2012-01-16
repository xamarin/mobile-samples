using System;
using MWC.BL.Contracts;
using MWC.DL.SQLite;

namespace MWC.BL
{
	/// <summary>
	/// DataModel for whether a session is a 'favorite' of the user
	/// </summary>
	/// <remarks>
	/// Currently implemented with 'Session Name' as the unique identifier
	/// since it isn't yet clear how we'll manage Session.ID across 
	/// data updates. If the PK changes, we'd lost favorites.
	/// </remarks>
	public class Favorite : BusinessEntityBase
	{
		public Favorite () {}

        public int SessionID { get; set; }

		public string SessionName { get; set; }
	}
}