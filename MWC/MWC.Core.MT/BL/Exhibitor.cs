using System;
using System.Collections.Generic;

namespace MWC.BL
{
	public partial class Exhibitor : Contracts.BusinessEntityBase
	{
		public virtual string Name { get; set; }
		public virtual string City { get; set; }
		public virtual string Country { get; set; }

		[MWC.DL.SQLite.Ignore]
		public virtual IList<string> Locations
		{
			get { return this.locations; }
			set { this.locations = value; }
		}
		protected IList<string> locations = new List<string>();
		
		public Exhibitor ()
		{
		}
	}
}

