SQLite.NET is an open source, minimal library to allow .NET and Mono applications to store data in [SQLite 3 databases](http://www.sqlite.org). SQLite.NET was designed as a quick and convenient database layer. Its design follows from these *goals*:

* Very easy to integrate with existing projects and with Xamarin projects.
  
* Thin wrapper over SQLite and should be fast and efficient. (The library should not be the performance bottleneck of your queries.)
  
* Very simple methods for executing CRUD operations and queries safely (using parameters) and for retrieving the results of those query in a strongly typed fashion.
  
* Works with your data model without forcing you to change your classes. (Contains a small reflection-driven ORM layer.)

*Non-goals* include:

* Not an ADO.NET implementation. This is not a full SQLite driver. If you need that, use [Mono.Data.SQLite](http://www.mono-project.com/SQLite) or [csharp-sqlite](http://code.google.com/p/csharp-sqlite/).

## Example

```csharp
using SQLite;
// ...

public class Note
{
	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }
	public string Message { get; set; }
}

// Create our connection
string folder = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
var db = new SQLiteConnection (System.IO.Path.Combine (folder, "notes.db"));
db.CreateTable<Note>();

// Insert note into the database
var note = new Note { Message = "Test Note" };
db.Insert (note);

// Show the automatically set ID and message.
Console.WriteLine ("{0}: {1}", note.Id, note.Message);
```