Please consult the Wiki for, ahem, [complete documentation](https://github.com/praeclarum/sqlite-net/wiki).

The library contains simple attributes that you can use to control the construction of tables. In a simple stock program, you might use:

```csharp
using  SQLite;
// ...

public class Stock
{
	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }
	[MaxLength(8)]
	public string Symbol { get; set; }
}

public class Valuation
{
	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }
	[Indexed]
	public int StockId { get; set; }
	public DateTime Time { get; set; }
	public decimal Price { get; set; }
}
```

Once you've defined the objects in your model you have a choice of APIs. You can use the "synchronous API" where calls
block one at a time, or you can use the "asynchronous API" where calls do not block. You may care to use the asynchronous
API for mobile applications in order to increase reponsiveness.

Both APIs are explained in the two sections below.

## Synchronous API

Once you have defined your entity, you can automatically generate tables in your database by calling `CreateTable`:

```csharp
string folder = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
var conn = new SQLiteConnection (System.IO.Path.Combine (folder, "stocks.db"));
conn.CreateTable<Stock>();
conn.CreateTable<Valuation>();
```

You can insert rows in the database using `Insert`. If the table contains an auto-incremented primary key, then the value for that key will be available to you after the insert:

```csharp
public static void AddStock (SQLiteConnection db, string symbol) {
	var s = new Stock { Symbol = symbol };
	db.Insert (s);
	Console.WriteLine ("{0} == {1}", s.Symbol, s.Id);
}
```

Similar methods exist for `Update` and `Delete`.

The most straightforward way to query for data is using the `Table` method. This can take predicates for constraining via WHERE clauses and/or adding ORDER BY clauses:

```csharp
var query = conn.Table<Stock>().Where (v => v.Symbol.StartsWith("A"));

foreach (var stock in query)
	Console.WriteLine ("Stock: " + stock.Symbol);
```

You can also query the database at a low-level using the `Query` method:

```csharp
public static IEnumerable<Valuation> QueryValuations (SQLiteConnection db, Stock stock)
{
	return db.Query<Valuation> ("select * from Valuation where StockId = ?", stock.Id);
}
```

The generic parameter to the `Query` method specifies the type of object to create for each row. It can be one of your table classes, or any other class whose public properties match the column returned by the query. For instance, we could rewrite the above query as:

```csharp
public class Val {
	public decimal Money { get; set; }
	public DateTime Date { get; set; }
}

public static IEnumerable<Val> QueryVals (SQLiteConnection db, Stock stock)
{
	return db.Query<Val> ("select 'Price' as 'Money', 'Time' as 'Date' from Valuation where StockId = ?", stock.Id);
}
```

You can perform low-level updates of the database using the `Execute` method.

## Asynchronous API

To use the asynchronous API, you'll create a `SQLiteAsyncConnection` instead of `SQLiteConnection`. `SQLiteAsyncConnection` exposes asynchronous versions of the database APIs providing you with `Task` to perform continuations on.

Once you have defined your entity, you can automatically generate tables by calling `CreateTableAsync`:

```csharp
string folder = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
var conn = new SQLiteAsyncConnection (System.IO.Path.Combine (folder, "stocks.db"));
conn.CreateTableAsync<Stock>().ContinueWith (t => {
	Console.WriteLine ("Table created!");
});
```

You can insert rows in the database using `Insert`. If the table contains an auto-incremented primary key, then the value for that key will be available to you after the insert:

```csharp
Stock stock = new Stock { Symbol = "AAPL" };

conn.InsertAsync (stock).ContinueWith (t => {
	Console.WriteLine ("New customer ID: {0}", stock.Id);
});
```

Similar methods exist for `UpdateAsync` and `DeleteAsync`.

Querying for data is most straightforwardly done using the `Table` method. This will return an `AsyncTableQuery` instance back, whereupon
you can add predictates for constraining via WHERE clauses and/or adding ORDER BY. The database is not physically touched until one of the special 
retrieval methods - `ToListAsync`, `FirstAsync`, or `FirstOrDefaultAsync` - is called.

```csharp
var query = conn.Table<Stock>().Where (v => v.Symbol.StartsWith ("A"));

query.ToListAsync().ContinueWith (t => {
	foreach (var stock in t.Result)
		Console.WriteLine ("Stock: " + stock.Symbol);
});
```

There are a number of low-level methods available. You can also query the database directly via the `QueryAsync` method. Over and above the change 
operations provided by `InsertAsync` etc you can issue `ExecuteAsync` methods to change sets of data directly within the database.

Another helpful method is `ExecuteScalarAsync`. This allows you to return a scalar value from the database easily:

```csharp
conn.ExecuteScalarAsync<int> ("select count(*) from Stock", null).ContinueWith (t => {
	Console.WriteLine (string.Format("Found '{0}' stock items.", t.Result));
});
```
