using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MWC.BL
{
	public partial class Conference
	{
		public Conference()
		{
			Speakers = new List<Speaker>();
			Sessions = new List<Session>();
			Exhibitors = new List<Exhibitor>();
		}

		[XmlElement("se")]
		public List<Session> Sessions { get; set; }
		[XmlElement("sp")]
		public List<Speaker> Speakers { get; set; }
		[XmlElement("ex")]
		public List<Exhibitor> Exhibitors { get; set; }
	}
}
