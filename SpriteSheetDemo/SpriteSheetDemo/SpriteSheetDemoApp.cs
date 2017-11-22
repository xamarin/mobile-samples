using System;
using System.Collections.Generic;
using Xamarin.Forms;
using CocosSharp;

namespace SpriteSheetDemo
{
	public class SpriteSheetDemoApp : Application
	{
		public SpriteSheetDemoApp ()
		{
			// The root page of your application
			MainPage = new GamePage ();
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

