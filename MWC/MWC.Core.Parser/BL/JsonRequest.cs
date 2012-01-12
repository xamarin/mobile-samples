using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MWC.BL
{
	public class JsonRequest
	{
		public int eventId { get; set; }
		public int pageSize { get; set; }
		public int page { get; set; }
		public string sortOrder { get; set; }
		public string sortColumn { get; set; }
		public string searchName { get; set; }
		public string searchCompany { get; set; }
		public string searchJobTitle { get; set; }
		public string searchCountry { get; set; }
		public string searchInterest { get; set; }
		public string filterChar { get; set; }
		public string searchSession { get; set; }
		public int topicID { get; set; }

		public JsonRequest(int page)
		{
			this.eventId = 7;
			this.page = page;
			this.sortOrder = "";
			this.sortColumn = "";
			this.searchName = "";
			this.searchCompany = "";
			this.searchJobTitle = "";
			this.searchCountry = "";
			this.searchInterest = "";
			this.filterChar = "";
			this.searchSession = "";
			this.topicID = -1;
		}
	}
}
