using System;
using UIKit;
using Tasky.Shared;
using MonoTouch.Dialog;

namespace Tasky.ApplicationLayer 
{
	/// <summary>
	/// Wrapper class for Task, to use with MonoTouch.Dialog. If it was just iOS platform
	/// we could apply these attributes directly to the Task class, but because we share that
	/// with other platforms this wrapper provides a bridge to MonoTouch.Dialog.
	/// </summary>
	public class TodoItemDialog 
	{
		public TodoItemDialog (TodoItem item)
		{
			Name = item.Name;
			Notes = item.Notes;
			// TODO: ensure the completed property is displayed on the screen
			Done = item.Done;
		}
		
		[Entry("task name")]
		public string Name { get; set; }

		[Entry("other task info")]
		public string Notes { get; set; }

		// TODO: add this user-interface element
		[Entry("Done")]
		public bool Done { get; set; }

		[Section ("")]
		[OnTap ("SaveTask")]
		[Alignment (UITextAlignment.Center)]
    	public string Save;
		
		[Section ("")]
		[OnTap ("DeleteTask")]
		[Alignment (UITextAlignment.Center)]
    	public string Delete;
	}
}