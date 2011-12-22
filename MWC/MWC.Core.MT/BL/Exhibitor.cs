using System;
using System.Collections.Generic;

namespace MWC.BL
{
	/// <summary>
	/// Represents an Exhibitor at the conference.
	/// </summary>
	public partial class Exhibitor : Contracts.BusinessEntityBase
	{
		public virtual string Name { get; set; }
		public virtual string City { get; set; }
		public virtual string Country { get; set; }
		public virtual string Locations { get; set; }
		
		public Exhibitor ()
		{
		}
	}
}

