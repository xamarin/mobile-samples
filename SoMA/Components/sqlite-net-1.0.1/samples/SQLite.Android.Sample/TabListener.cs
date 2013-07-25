using System;
using Android.App;
using Android.Content;

namespace SQLite.Android.Sample
{
	public class TabListener<T>
		: Java.Lang.Object, ActionBar.ITabListener
		where T : Fragment
	{
		public TabListener (Context listenerContext)
		{
			if (listenerContext == null)
				throw new ArgumentNullException ("context");

			this.context = listenerContext;
			this.fragmentName = typeof(T).Namespace.ToLower() + "." + typeof(T).Name;
		}

		public TabListener (Context listenerContext, string tag, T existingFragment = null)
			: this (listenerContext)
		{
			if (tag == null)
				throw new ArgumentNullException ("tag");

			this.fragment = existingFragment;
			this.tag = tag;
		}

		public T Fragment
		{
			get { return this.fragment; }
		}

		public void OnTabReselected (ActionBar.Tab tab, FragmentTransaction ft)
		{
		}

		public void OnTabSelected (ActionBar.Tab tab, FragmentTransaction ft)
		{
			if (this.fragment == null)
			{
				this.fragment = (T)global::Android.App.Fragment.Instantiate (this.context, this.fragmentName);
				if (this.tag != null)
					ft.Add (global::Android.Resource.Id.Content, this.fragment, this.tag);
				else
					ft.Add (global::Android.Resource.Id.Content, this.fragment);
			}
			else
				ft.Attach (this.fragment);
		}

		public void OnTabUnselected (ActionBar.Tab tab, FragmentTransaction ft)
		{
			if (this.fragment != null)
				ft.Detach (this.fragment);
		}

		private T fragment;
		private readonly Context context;
		private readonly string tag;
		private readonly string fragmentName;

		protected override void Dispose(bool disposing)
		{
			if (disposing)
				this.fragment.Dispose();

			base.Dispose (disposing);
		}
	}
}