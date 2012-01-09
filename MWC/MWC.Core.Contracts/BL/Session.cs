using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MWC.BL
{
	public partial class Session
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
		public List<string> SpeakerKeys { get; set; }
		[XmlIgnore]
		public List<Speaker> SpeakerList { get; set; }
		[XmlIgnore]
		public string DetailUrl { get; set; }

		public Session()
		{
			SpeakerList = new List<Speaker>();
			SpeakerKeys = new List<string>();
		}
	}
}
