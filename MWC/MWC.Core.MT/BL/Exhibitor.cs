using System.Xml.Serialization;

namespace MWC.BL
{
	/// <summary>
	/// Represents an Exhibitor at the conference.
	/// </summary>
	public partial class Exhibitor : Contracts.BusinessEntityBase
	{
		[XmlAttribute("n")]
		public virtual string Name { get; set; }
		[XmlAttribute("ci")]
		public virtual string City { get; set; }
		[XmlAttribute("co")]
		public virtual string Country { get; set; }
		[XmlElement("l")]
		public virtual string Locations { get; set; }
		[XmlAttribute("fe")]
		public virtual bool IsFeatured { get; set; }
		[XmlElement("o")]
		public virtual string Overview { get; set; }
		[XmlAttribute("t")]
		public virtual string Tags { get; set; }
		[XmlAttribute("d")]
		public virtual string Email { get; set; }
		[XmlAttribute("a")]
		public virtual string Address { get; set; }
		[XmlAttribute("p")]
		public virtual string Phone { get; set; }
		[XmlAttribute("fa")]
		public virtual string Fax { get; set; }
		
		public Exhibitor ()
		{
		}
	}
}

