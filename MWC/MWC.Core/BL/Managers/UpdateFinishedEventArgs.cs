using System;

namespace MWC
{
	public enum UpdateType
	{
		SeedData,
		Conference,
		Exhibitors
	}
	
	/// <summary>
	/// When a data update has finished, pass back whether it succeeded or failed
	/// and also the type of data that was updated (so the calling code can do useful stuff)
	/// </summary>
	public class UpdateFinishedEventArgs : EventArgs
	{
		public bool Success {get;set;}

		public UpdateType UpdateType {get;set;}

		public UpdateFinishedEventArgs (UpdateType updateType, bool success)
		{
			this.UpdateType = updateType;
			this.Success = success;
		}
	}
}

