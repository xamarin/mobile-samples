using System;
using System.Drawing;
using Foundation;
using UIKit;
using System.Threading.Tasks;
using MultiThreading.Controls;

namespace MultiThreading {
	public partial class MainScreen_iPhone : UIViewController {

		protected LoadingOverlay _loadPop = null;

		public MainScreen_iPhone () : base ("MainScreen_iPhone", null)
		{
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// this example uses the Task.Factory class to spin up a new task (thread)
			// Factory is nice because it allows you to use the ContinueWith method so 
			// you can specify code to execute after the first task is complete.
			this.StartBackgroundTaskButton.TouchUpInside += (sender, e) => {

				// show the loading overlay on the UI thread
				this._loadPop = new LoadingOverlay (UIScreen.MainScreen.Bounds);
				this.View.Add ( this._loadPop );

				// spin up a new thread to do some long running work using StartNew
				Task.Factory.StartNew (
					// tasks allow you to use the lambda syntax to pass work
					() => {
						Console.WriteLine ( "Hello from taskA." );
						LongRunningProcess(2);
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
						this._loadPop.Hide ();
						Console.WriteLine ( "Finished, hiding our loading overlay from the UI thread." );
					}, TaskScheduler.FromCurrentSynchronizationContext()
				);


				// Output a message from the original thread. note that this executes before
				// the background thread has finished.
				Console.WriteLine("Hello from the calling thread.");
			};

			// the simplest way to start background tasks is to use create a Task, assign
			// some work to it, and call start.
			this.StartBackgroundTaskNoUpdateButton.TouchUpInside += (sender, e) => {
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
			Console.WriteLine ( "Beginning Long Running Process {0}", seconds );
			System.Threading.Thread.Sleep ( seconds * 1000 );
			Console.WriteLine ( "Finished Long Running Process {0}", seconds );
		}

		[Obsolete]
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			ReleaseDesignerOutlets ();
		}

		[Obsolete]
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}
	}
}

