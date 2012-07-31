
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AndroidMultiThreading.Screens {

	[Activity (Label = "MultiThreading", MainLauncher = true)]			
	public class MainScreen : Activity {
		Button updateUIButton, noupdateUIButton;
		ProgressDialog progress;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			updateUIButton = FindViewById<Button> (Resource.Id.StartBackgroundTaskUpdateUI);

			updateUIButton.Click += delegate {

				// show the loading overlay on the UI thread
				progress = ProgressDialog.Show(this, "Loading", "Please Wait...", true); 

				// spin up a new thread to do some long running work using StartNew
				Task.Factory.StartNew (
					// tasks allow you to use the lambda syntax to pass work
					() => {
						Console.WriteLine ( "Hello from taskA." );
						LongRunningProcess(4);
					}
				// ContinueWith allows you to specify an action that runs after the previous thread
				// completes
				// 
				// By using TaskScheduler.FromCurrentSyncrhonizationContext, we can make sure that 
				// this task now runs on the original calling thread, in this case the UI thread
				// so that any UI updates are safe. in this example, we want to hide our overlay, 
				// but we don't want to update the UI from a background thread.
				).ContinueWith ( 
					t => {
		                if (progress != null)
		                    progress.Hide();
		                
						Console.WriteLine ( "Finished, hiding our loading overlay from the UI thread." );
					}, TaskScheduler.FromCurrentSynchronizationContext()
				);

				// Output a message from the original thread. note that this executes before
				// the background thread has finished.
				Console.WriteLine("Hello from the calling thread.");
			};


			// the simplest way to start background tasks is to use create a Task, assign
			// some work to it, and call start.
			noupdateUIButton = FindViewById<Button> (Resource.Id.StartBackgroundTaskNoUpdate);
			noupdateUIButton.Click += delegate {
				var TaskA = new Task ( () => { LongRunningProcess (5); } );
				var TaskB = new Task ( () => { LongRunningProcess (4); } );

				TaskA.Start ();
				TaskB.Start ();
			};
		}

		/// <summary>
		/// Simulation method to sit for a number of seconds.
		/// </summary>
		protected void LongRunningProcess (int seconds)
		{
			Console.WriteLine ( "Beginning Long Running Process {0} seconds", seconds );
			System.Threading.Thread.Sleep ( seconds * 1000 );
			Console.WriteLine ( "Finished Long Running Process {0} seconds", seconds );
		}
	}
}