using System;
using System.Collections.Generic;

namespace MWC.BL
{
	public class Exhibitor : Contracts.BusinessEntityBase
	{
		public string Name { get; set; }
		public string City { get; set; }
		public string Country { get; set; }
		public IList<string> Locations
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

