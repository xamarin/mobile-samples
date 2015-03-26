using System;
using System.Collections.Generic;

namespace DataAccess
{
	public class StockRepository
	{
		StockDatabase db = null;
		protected static StockRepository me;	
		static StockRepository ()
		{
			me = new StockRepository();
		}
		protected StockRepository()
		{
			db = new StockDatabase(StockDatabase.DatabaseFilePath);
		}

		public static Stock GetStock(int id)
		{
			return me.db.GetStock(id);
		}
		
		public static IEnumerable<Stock> GetStocks ()
		{
			return me.db.GetStocks();
		}
		
		public static int SaveStock (Stock item)
		{
			return me.db.SaveStock(item);
		}
		
		public static int DeleteStock(Stock item)
		{
			return me.db.DeleteStock(item);
		}
	}
}

