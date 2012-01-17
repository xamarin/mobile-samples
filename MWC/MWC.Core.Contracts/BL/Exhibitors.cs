using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MWC.BL
{
	public class Exhibitors
	{
		public Exhibitors()
		{
			this.Items = new List<Exhibitor>();
		}

		[XmlElement("ex")]
		public List<Exhibitor> Items { get; set; }
	}
}
