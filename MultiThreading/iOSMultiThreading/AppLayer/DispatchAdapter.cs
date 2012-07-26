using System;
using MonoTouch.Foundation;

namespace MultiThreading.AppLayer
{
	public class DispatchAdapter : IDispatchOnUIThread {
		public readonly NSObject owner;

		public DispatchAdapter (NSObject owner) {
			this.owner = owner;
		}
	    
		public void Invoke (Action action) {
			owner.BeginInvokeOnMainThread(new NSAction(action));
		}
	}
}

