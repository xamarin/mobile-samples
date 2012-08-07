using System;
using Tasky.BL;

namespace Tasky.AL
{
	public class TaskClickedEventArgs : EventArgs
	{
		public Task Task { get; set; }
		
		public TaskClickedEventArgs (Task task) : base ()
		{
			this.Task = task;
		}
	}
}

