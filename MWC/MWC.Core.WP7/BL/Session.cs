using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MWC.BL
{
	public partial class Session : Contracts.BusinessEntityBase
	{
		[XmlElement("t")]
		public string Title { get; set; }
		[XmlAttribute("st")]
		public DateTime Start { get; set; }
		[XmlAttribute("en")]
		public DateTime End { get; set; }
		[XmlAttribute("r")]
		public string Room { get; set; }
		[XmlAttribute("sn")]
		public string SpeakerNames { get; set; }
		[XmlElement("o")]
		public string Overview { get; set; }

		[XmlElement("sk")]
		[MWC.DL.SQLite.Ignore]
		public List<string> SpeakerKeys { get; set; }

//		[MWC.DL.SQLite.Ignore]
//		public IList<string> Speakers
//		{
//			get { return this.speakers; }
//			set { this.speakers = value; }
//		}
//		protected IList<string> speakers = new List<string>();
		

		public Session ()
		{
		}
	}
}

