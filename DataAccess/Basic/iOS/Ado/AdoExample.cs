using System;
using System.IO;
using Mono.Data.Sqlite;

namespace DataAccess
{
	public static class AdoExample
	{
		public static SqliteConnection connection;
		public static string DoSomeDataAccess ()
		{
			var output = "";

			string dbPath = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "adodemo.db3");
			
			bool exists = File.Exists (dbPath);
			
			if (!exists) {
				output += "Creating database";
				// Need to create the database and seed it with some data.
				Mono.Data.Sqlite.SqliteConnection.CreateFile (dbPath);
				connection = new SqliteConnection ("Data Source=" + dbPath);
				
				connection.Open ();
				var commands = new[] {
					"CREATE TABLE [Items] (_id ntext, Symbol ntext);"
					,
					"INSERT INTO [Items] ([_id], [Symbol]) VALUES ('1', 'AAPL')"
					,
					"INSERT INTO [Items] ([_id], [Symbol]) VALUES ('2', 'GOOG')"
					,
					"INSERT INTO [Items] ([_id], [Symbol]) VALUES ('3', 'MSFT')"
				};
				foreach (var command in commands) {
					using (var c = connection.CreateCommand ()) {
						c.CommandText = command;
						var i = c.ExecuteNonQuery ();
						output += "\n\tExecuted " + command + " (rows:" + i +")";
					}
				}
			} else {
				output += "Database already exists";
				connection = new SqliteConnection ("Data Source=" + dbPath);
				connection.Open ();
			}
			
			// query the database to prove data was inserted!
			using (var contents = connection.CreateCommand ()) {
				contents.CommandText = "SELECT [_id], [Symbol] from [Items]";
				var r = contents.ExecuteReader ();
				output += "\n=== Reading data;";
				while (r.Read ())
					output += String.Format ("\n\tKey={0}; Value={1}",
					                  r ["_id"].ToString (),
					                  r ["Symbol"].ToString ());
			}
			connection.Close ();

			return output;
		}

		public static string MoreComplexQuery () 
		{
			var output = "";
			output += "\n=== Complex query example: ";
			string dbPath = Path.Combine (
				Environment.GetFolderPath (Environment.SpecialFolder.Personal), "adodemo.db3");
			
			connection = new SqliteConnection ("Data Source=" + dbPath);
			connection.Open ();
			using (var contents = connection.CreateCommand ()) {
				contents.CommandText = "SELECT * FROM [Items] WHERE Symbol = 'MSFT'";
				var r = contents.ExecuteReader ();
				output += "\nReading data"; 
				while (r.Read ())
					output += String.Format ("\n\tKey={0}; Value={1}",
					                         r ["_id"].ToString (),
					                         r ["Symbol"].ToString ());
			}
			connection.Close ();

			return output;
		}

		public static string ScalarQuery () 
		{
			var output = "";
			output += "\n=== Scalar query example: ";
			string dbPath = Path.Combine (
				Environment.GetFolderPath (Environment.SpecialFolder.Personal), "adodemo.db3");
			
			connection = new SqliteConnection ("Data Source=" + dbPath);
			connection.Open ();
			using (var contents = connection.CreateCommand ()) {
				contents.CommandText = "SELECT COUNT(*) FROM [Items] WHERE Symbol <> 'MSFT'";
				var i = contents.ExecuteScalar ();
				output += "\nExecuting a scalar query";
				output += "\n\tResult=" + i;
			}
			connection.Close ();
			
			return output;
		}
	}
}

