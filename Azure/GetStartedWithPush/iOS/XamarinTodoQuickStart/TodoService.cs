using System;
using System.Collections.Generic;
using Foundation;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using System.Net.Http;

namespace XamarinTodoQuickStart
{
	public class TodoService : DelegatingHandler
	{
		private static TodoService todoServiceInstance = new TodoService();
        public static TodoService DefaultService { get { return todoServiceInstance; } }

        private MobileServiceClient client;
        private IMobileServiceTable<TodoItem> todoTable;        

        public List<TodoItem> Items { get; private set;}
        private int busyCount = 0;

        // Public events
		public event Action<bool> BusyUpdate;

        // Constructor
		protected TodoService()
		{
            Items = new List<TodoItem>();

			CurrentPlatform.Init ();
			// Initialize the Mobile Service client with your URL and key
			client = new MobileServiceClient(Constants.ApplicationURL, this);

			// Create an MSTable instance to allow us to work with the TodoItem table
			todoTable = client.GetTable <TodoItem>();
		}

		async public Task<List<TodoItem>> RefreshDataAsync()
		{
			try 
            {
				// This code refreshes the entries in the list view by querying the TodoItems table.
				// The query excludes completed TodoItems
				Items = await todoTable
					.Where (todoItem => todoItem.Complete == false).ToListAsync();

			} 
            catch (MobileServiceInvalidOperationException e) 
            {
				Console.Error.WriteLine (@"ERROR {0}", e.Message);
				return null;
			}

			return Items;
		}

		public async Task<int> InsertTodoItemAsync(TodoItem todoItem)
		{
			try 
            {
				// This code inserts a new TodoItem into the database. When the operation completes
				// and Mobile Services has assigned an Id, the item is added to the CollectionView
				await todoTable.InsertAsync(todoItem);

				Items.Add(todoItem); 

                return Items.IndexOf(todoItem);

			} 
            catch (MobileServiceInvalidOperationException e) 
            {
				Console.Error.WriteLine(@"ERROR {0}", e.Message);
                return 0;
			}
		}

		public async Task CompleteItemAsync(TodoItem item)
		{
			try 
            {
				// This code takes a freshly completed TodoItem and updates the database. When the MobileService 
				// responds, the item is removed from the list 
				item.Complete = true;
				await todoTable.UpdateAsync(item);
				Items.Remove(item);

			} 
            catch (MobileServiceInvalidOperationException e) 
            {
				Console.Error.WriteLine (@"ERROR {0}", e.Message);
			}
		}

		void Busy(bool busy)
		{
			// assumes always executes on UI thread
			if (busy) 
            {
				if (busyCount++ == 0 && BusyUpdate != null)
					BusyUpdate(true);
			} 
            else 
            {
				if (--busyCount == 0 && BusyUpdate != null)
					BusyUpdate(false);
			}
		}

		#region implemented abstract members of HttpMessageHandler

		protected override async Task<System.Net.Http.HttpResponseMessage> SendAsync (System.Net.Http.HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
		{
			Busy (true);
			var response = await base.SendAsync (request, cancellationToken);

			Busy (false);
			return response;
		}

		#endregion
	}
}