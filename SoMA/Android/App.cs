using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Core;
using System.IO;

namespace Droid
{
	public static class App
	{
		static App () {
			string userPath = System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal); 
			string databasePath = Path.Combine (userPath, "soma.db");
			Database = new Core.SomaDatabase (databasePath);
		}
		public static SomaDatabase Database { get; set; }
	}
}

