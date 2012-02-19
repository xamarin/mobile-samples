using System.Xml.Serialization;
using System;

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
		[XmlAttribute("i")]
		public string ImageUrl { get; set; }
		
		public Exhibitor ()
		{
		}

        /// <summary>
        /// Used in the UI to build 'fast scrolling'/'index scrolling'
        /// </summary>
        public string Index {
            get {
                return IsCapitalLetter(Name[0]) ? Name[0].ToString().ToUpper() : "1";
            }
        }
        bool IsCapitalLetter(char startsWith)
        {
            return ((startsWith >= 'A') && (startsWith <= 'Z'))
                || ((startsWith >= 'a') && (startsWith <= 'z'));
        }

        public string FormattedCityCountry {
            get {
                string cityCountry = City;
                if (!String.IsNullOrEmpty(cityCountry)) {
                    if (!String.IsNullOrEmpty(Country))
                        cityCountry += ", " + Country;
                } else
                    cityCountry = Country;
                return cityCountry;
            }
        }
	}
}

