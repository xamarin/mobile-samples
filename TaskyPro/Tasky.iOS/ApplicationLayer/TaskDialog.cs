using System;
using UIKit;
using Tasky.BL;
using MonoTouch.Dialog;

namespace Tasky {
	/// <summary>
	/// Wrapper class for Task, to use with MonoTouch.Dialog. If it was just iOS platform
	/// we could apply these attributes directly to the Task class, but because we share that
	/// with other platforms this wrapper provides a bridge to MonoTouch.Dialog.
	/// </summary>
	public class TaskDialog {
		public TaskDialog (Task task)
		{
			Name = task.Name;
			Notes = task.Notes;
			Done = task.Done;
		}
		
		[Localize]
		[Entry("task name")]
		public string Name { get; set; }

		[Localize]
		[Entry("other task info")]
		public string Notes { get; set; }
		
		// new property
		[Localize]
		[Entry("Done")]
		public bool Done { get; set; }

		[Localize]
		[Section ("")]
		[OnTap ("SaveTask")]
		[Alignment (UITextAlignment.Center)]
    	public string Save;
		
		[Localize]
		[Section ("")]
		[OnTap ("DeleteTask")]
		[Alignment (UITextAlignment.Center)]
    	public string Delete;
	}
}