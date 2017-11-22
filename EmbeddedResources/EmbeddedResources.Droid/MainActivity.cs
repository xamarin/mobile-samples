using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Reflection;

namespace EmbeddedResources.Droid
{
	[Activity (Label = "EmbeddedResources.Droid", MainLauncher = true)]
	public class MainActivity : Activity
	{
		protected EditText haikuTextView;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			this.RequestWindowFeature (WindowFeatures.NoTitle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			this.haikuTextView = FindViewById<EditText> (Resource.Id.HaikuTextView);

			this.haikuTextView.Text = ResourceLoader.GetEmbeddedResourceString (Assembly.GetAssembly (typeof(ResourceLoader)), "ALittleStory.txt");

		}
	}
}


