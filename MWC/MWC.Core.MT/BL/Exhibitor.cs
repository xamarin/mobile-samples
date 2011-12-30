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
		
		public Exhibitor ()
		{
		}
	}
}

