using System;
using Android.Widget;
using Android.App;
using System.Collections.Generic;

namespace BluetoothLEExplorer.Droid
{
	public abstract class GenericAdapterBase<T> : BaseAdapter<T>
	{
		protected Activity context;
		protected int resource;

		public IList<T> Items {
			get { return this.items; }
		} protected IList<T> items;


		public GenericAdapterBase (Activity context, int resource, IList<T> items) : base()
		{
			this.context = context;
			this.items = items;
			this.resource = resource;
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override T this[int position] 
		{  
			get { return items[position]; }
		}

		public override int Count 
		{
			get { return items.Count; }
		}
	}
}

