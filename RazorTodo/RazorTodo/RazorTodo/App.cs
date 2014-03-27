using System;

namespace RazorTodo
{
	public static class App
	{
		static SQLite.Net.SQLiteConnection conn;
		static TodoItemDatabase database;
		public static void SetDatabaseConnection (SQLite.Net.SQLiteConnection connection)
		{
			conn = connection;
			database = new TodoItemDatabase (conn);
		}
		public static TodoItemDatabase Database {
			get { return database; }
		}
	}
}

