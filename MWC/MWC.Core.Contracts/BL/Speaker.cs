using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MWC.BL
{
	public partial class Speaker
	{
		[XmlAttribute("k")]
		public string Key { get; set; }
		[XmlAttribute("n")]
		public string Name { get; set; }
		[XmlAttribute("t")]
		public string Title { get; set; }
		[XmlAttribute("c")]
		public string Company { get; set; }
		[XmlAttribute("b")]
		public string Bio { get; set; }
		[XmlAttribute("i")]
		public string ImageUrl { get; set; }
		[XmlIgnore]
		public string DetailUrl { get; set; }
		
		public Speaker()
		{
			Key = Guid.NewGuid().ToString();
		}
	}
}
