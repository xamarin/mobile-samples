using System;

namespace Tasky.Portable {
	/// <summary>
	/// Task business object, stored as XML
	/// </summary>
	public class Task : IBusinessEntity {
		public Task ()
		{
		}

        public int ID { get; set; }
		public string Name { get; set; }
		public string Notes { get; set; }
		public bool Done { get; set; }
	}
}