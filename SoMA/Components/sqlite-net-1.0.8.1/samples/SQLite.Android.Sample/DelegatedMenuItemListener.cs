using System;
using Android.Views;

namespace SQLite.Android.Sample
{
	public class DelegatedMenuItemListener
		: Java.Lang.Object, IMenuItemOnMenuItemClickListener
	{
		public DelegatedMenuItemListener (Func<IMenuItem, bool> handler)
		{
			if (handler == null)
				throw new ArgumentNullException ("handler");

			this.handler = handler;
		}

		public bool OnMenuItemClick (IMenuItem item)
		{
			return this.handler (item);
		}

		private readonly Func<IMenuItem, bool> handler;
	}
}