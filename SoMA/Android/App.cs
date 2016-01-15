using Core;
using System.IO;

namespace Droid
{
	public static class App
	{
		static App () {
			string userPath = System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal); 
			string databasePath = Path.Combine (userPath, "soma.db");
			Database = new SomaDatabase (databasePath);
		}
		public static SomaDatabase Database { get; set; }
	}
}

