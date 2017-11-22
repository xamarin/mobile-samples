using System;
using System.Collections.Generic;

namespace Xamarin.Code
{
	/// <summary>
	/// A group that contains table items
	/// </summary>
	public class NavItemGroup
	{
		public string Name { get; set; }

		public string Footer { get; set; }

		public List<NavItem> Items { get; private set; }

		public NavItemGroup (string name)
		{
			Name = name;
			Items = new List<NavItem> ();
		}
	}
}
