using System;
using System.Collections.Generic;
using Foundation;
using System.Threading.Tasks;
using UIKit;
using System.Net.Http;

// TODO:: Add the following using statement
 using Microsoft.WindowsAzure.MobileServices;

namespace XamarinTodoQuickStart
{
	// TODO:: inherit from DelegatingHandler
	public class TodoService : DelegatingHandler
	{
		private static TodoService todoServiceInstance = new TodoService();
        public static TodoService DefaultService { get { return todoServiceInstance; } }

        // TODO:: Uncomment these lines to use Mobile Services
        private MobileServiceClient client;
        private IMobileServiceTable<TodoItem> todoTable;        

        public List<TodoItem> Items { get; private set;}
        private int busyCount = 0;

        // Public events
		public event Action<bool> BusyUpdate;

        // Constructor
		protected TodoService()
		{
			CurrentPlatform.Init ();

            Items = new List<TodoItem>();

            // TODO:: Uncomment these lines to use Mobile Services			
			client = new MobileServiceClient (Constants.ApplicationURL, Constants.ApplicationKey, this);	
            todoTable = client.GetTable<TodoItem>(); // Create an MSTable instance to allow us to work with the TodoItem table
		}

		async public Task<List<TodoItem>> RefreshDataAsync()
		{
            // TODO:: Uncomment these lines to use Mobile Services

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
                // TODO:: Uncomment this line out to use Mobile Services
				await todoTable.InsertAsync(todoItem);

				Items.Add(todoItem); 

                return Items.IndexOf(todoItem);
			} 
            catch (Exception e) // TODO:: Optional - catch MobileServiceInvalidOperationException instead of generic Exception
            {
				Console.Error.WriteLine(@"ERROR {0}", e.Message);
                return 0;
			}
		}

		public async Task CompleteItemAsync(TodoItem item)
		{
			try 
            {
				item.Complete = true;

                // This code takes a freshly completed TodoItem and updates the database. When the MobileService 
                // responds, the item is removed from the list 
				// TODO:: Uncomment this line to use Mobile Services
                await todoTable.UpdateAsync(item);

				Items.Remove(item);

			} 
            catch (Exception e) // TODO:: Optional - catch MobileServiceInvalidOperationException instead of generic Exception
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

		// TODO:: Uncomment this code when using Mobile Services and inheriting from IServiceFilter
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