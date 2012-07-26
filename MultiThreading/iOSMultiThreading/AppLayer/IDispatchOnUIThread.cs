using System;

namespace MultiThreading.AppLayer
{
	public interface IDispatchOnUIThread {
	    void Invoke (Action action);
	}
}

