using System;
using Android.OS;
using Android.App;
using Android.Views;
using Android.Widget;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

// TODO:: Add the following using statement
// using Microsoft.WindowsAzure.MobileServices;
using System.Collections.Generic;

namespace XamarinTodoQuickStart
{
	[Activity (MainLauncher = true, 
	           Icon="@drawable/ic_launcher", Label="@string/app_name",
	           Theme="@style/AppTheme")]
	public class TodoActivity : Activity
	{       		
        // TODO:: Uncomment the following two lines of code to use Mobile Services
        private MobileServiceClient client; // Mobile Service Client reference		
        private IMobileServiceTable<TodoItem> todoTable; // Mobile Service Table used to access data     

        // TODO:: Comment out this line to remove the in-memory list
        public List<TodoItem> todoItemList = new List<TodoItem>();

        private TodoItemAdapter adapter; // Adapter to sync the items list with the view            
        private EditText textNewTodo; // EditText containing the "New Todo" text
        private ProgressBar progressBar; // Progress spinner to use for table operations

        // Called when the activity initially gets created
		protected override async void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Activity_To_Do);

            // Initialize the progress bar
			progressBar = FindViewById<ProgressBar>(Resource.Id.loadingProgressBar);
			progressBar.Visibility = ViewStates.Gone;

            // TODO:: Uncomment the following code when using a mobile service
            
			// Create ProgressFilter to handle busy state
			var progressHandler = new ProgressHandler ();
			progressHandler.BusyStateChange += (busy) => {
				if (progressBar != null) 
					progressBar.Visibility = busy ? ViewStates.Visible : ViewStates.Gone;
			};
                     

			try 
            {
                // TODO:: Uncomment the following code to create the mobile services client

				CurrentPlatform.Init ();
				// Create the Mobile Service Client instance, using the provided
				// Mobile Service URL and key
				client = new MobileServiceClient(
					Constants.ApplicationURL,
					Constants.ApplicationKey, progressHandler);

				// Get the Mobile Service Table instance to use
				todoTable = client.GetTable<TodoItem>();

				textNewTodo = FindViewById<EditText>(Resource.Id.textNewTodo);

				// Create an adapter to bind the items with the view
				adapter = new TodoItemAdapter(this, Resource.Layout.Row_List_To_Do);
				var listViewTodo = FindViewById<ListView>(Resource.Id.listViewTodo);
				listViewTodo.Adapter = adapter;

				// Load the items from the Mobile Service
				await RefreshItemsFromTableAsync();

			} 
            catch (Java.Net.MalformedURLException) 
            {
				CreateAndShowDialog(new Exception ("There was an error creating the Mobile Service. Verify the URL"), "Error");
			} 
            catch (Exception e) 
            {
				CreateAndShowDialog(e, "Error");
			}
		}

		// Initializes the activity menu
		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.activity_main, menu);
			return true;
		}

		// Select an option from the menu
		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			if (item.ItemId == Resource.Id.menu_refresh) {
				OnRefreshItemsSelected();
			}
			return true;
		}

		// Called when the refresh menu opion is selected
		async void OnRefreshItemsSelected()
		{
			await RefreshItemsFromTableAsync();
		}

		// Refresh the list with the items in the Mobile Service Table
		async Task RefreshItemsFromTableAsync()
		{
            // TODO:: Uncomment the following code when using a mobile service
            
			try 
            {
				// Get the items that weren't marked as completed and add them in the adapter
				var list = await todoTable.Where(item => item.Complete == false).ToListAsync ();

				adapter.Clear();

				foreach (TodoItem current in list)
					adapter.Add(current);
			} 
            catch (Exception e) 
            {
				CreateAndShowDialog(e, "Error");
			}


			// TODO:: Comment out these lines to remove the in-memory list
            adapter.Clear();
            foreach (var item in todoItemList)
            {
                if (!item.Complete)
                    adapter.Add(item);
            }
            // NOTE:: End of lines to comment out
		}

		public async Task CheckItem(TodoItem item)
		{
			// Set the item as completed and update it in the table
			item.Complete = true;

            // TODO:: Uncomment the following code when using a mobile service
            
			try 
            {
				await todoTable.UpdateAsync(item);
				if (item.Complete)
					adapter.Remove(item);
			} 
            catch (Exception e) 
            {
				CreateAndShowDialog(e, "Error");
			}


            // TODO:: Comment out these lines to remove the in-memory list
            todoItemList.Add(item);
            if (item.Complete)
                adapter.Remove(item);
            // NOTE:: End of lines to comment out
		}

		[Java.Interop.Export()]
		public async void AddItem(View view)
		{
			// Create a new item
			var item = new TodoItem() {
				Text = textNewTodo.Text,
				Complete = false
			};

            // TODO:: Uncomment the following code when using a mobile service
            
			try 
            {
				// Insert the new item
				await todoTable.InsertAsync(item);

				if (!item.Complete) 
					adapter.Add(item);
			} 
            catch (Exception e) 
            {
				CreateAndShowDialog(e, "Error");
			}


            // TODO:: Comment out these lines to remove the in-memory list
            todoItemList.Add(item);
            adapter.Add(item);
            // NOTE:: End of lines to comment out

			textNewTodo.Text = "";
		}

		void CreateAndShowDialog(Exception exception, String title)
		{
			CreateAndShowDialog(exception.Message, title);
		}

		void CreateAndShowDialog(string message, string title)
		{
			AlertDialog.Builder builder = new AlertDialog.Builder(this);

			builder.SetMessage(message);
			builder.SetTitle(title);
			builder.Create().Show();
		}

        // TODO:: Uncomment the following code when using a mobile service
        
		class ProgressHandler : DelegatingHandler
		{
			int busyCount = 0;

			public event Action<bool> BusyStateChange;

			#region implemented abstract members of HttpMessageHandler

			protected override async Task<HttpResponseMessage> SendAsync (HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
			{
				 //assumes always executes on UI thread
				if (busyCount++ == 0 && BusyStateChange != null)
					BusyStateChange (true);

				var response = await base.SendAsync (request, cancellationToken);

				// assumes always executes on UI thread
				if (--busyCount == 0 && BusyStateChange != null)
					BusyStateChange (false);

				return response;
			}

			#endregion

		}
        
	}
}