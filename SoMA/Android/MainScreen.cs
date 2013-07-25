using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Core;

namespace Droid
{
	[Activity (Label = "Xamarin.SoMA", MainLauncher = true, Icon = "@drawable/ic_launcher", Theme = "@style/AppTheme")]
	public class MainScreen : Activity
	{
		Button newButton;
		ListView itemListView;
		List<ShareItem> items;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.MainScreen);

			newButton = FindViewById<Button> (Resource.Id.myButton);
			itemListView = FindViewById<ListView> (Resource.Id.listView1);

			// add event handlers for touches (clicks)
			newButton.Click += (sender, e) => {
				StartActivity(typeof(PhotoScreen));
			};

			itemListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
				var photoScreen = new Intent (this, typeof (PhotoScreen));
				photoScreen.PutExtra (PhotoScreen.ShareItemIdExtraName, items[e.Position].Id);
				StartActivity (photoScreen);
			};
		}

		protected override void OnResume ()
		{
			base.OnResume ();

			items = App.Database.GetItems ();
			Console.WriteLine ("items: " + items.Count);
			itemListView.Adapter = new MainScreenAdapter (this, items);
		}
	}
}


