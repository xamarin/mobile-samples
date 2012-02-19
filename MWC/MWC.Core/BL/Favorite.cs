using System;
using MWC.BL.Contracts;
using MWC.DL.SQLite;

namespace MWC.BL
{
	/// <summary>
	/// DataModel for whether a session is a 'favorite' of the user
	/// </summary>
	public class Favorite : BusinessEntityBase
	{
		public Favorite () {}
		
		/// <summary>
		/// Gets or sets the session I.
		/// </summary>
		[Obsolete("Not used, the ID is like quicksand, always moving and changing")]
        public int SessionID { get; set; }

		public string SessionKey { get; set; }
	}
}