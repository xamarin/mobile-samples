using System;
using UIKit;
using MonoTouch.Dialog;
using Tasky.Portable;

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
		[MonoTouch.Dialog.Entry("task name")]
		public string Name { get; set; }

		[Localize]
		[MonoTouch.Dialog.Entry("other task info")]
		public string Notes { get; set; }
		
		// new property
		[Localize]
		[MonoTouch.Dialog.Entry("Done")]
		public bool Done { get; set; }

		[Localize]
		[MonoTouch.Dialog.Section("")]
		[MonoTouch.Dialog.OnTap("SaveTask")]
		[MonoTouch.Dialog.Alignment(UITextAlignment.Center)]
    	public string Save;
		
		[Localize]
		[MonoTouch.Dialog.Section("")]
		[MonoTouch.Dialog.OnTap("DeleteTask")]
		[MonoTouch.Dialog.Alignment(UITextAlignment.Center)]
    	public string Delete;
	}
}