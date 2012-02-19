using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MWC.BL {
	public partial class Session : Contracts.BusinessEntityBase {

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
		
		// These are ONLY POPULATED on the client-side, when a single Session is requested
		[XmlElement("sk")]
		[MWC.DL.SQLite.Ignore]
		public List<string> SpeakerKeys { get; set; }
		
		[XmlIgnore]
		[MWC.DL.SQLite.Ignore]
		public List<Speaker> Speakers { get; set; }
		
		/// <summary>Only populated on the client-side, using StartTime+Title</summary>
		public string Key { get; set; }

		public Session ()
		{
            // seems like WP7 likes these initialized, because after deserialization of empty lists they are otherwise null
            SpeakerKeys = new List<string>();
            Speakers = new List<Speaker>();
		}
	}
}