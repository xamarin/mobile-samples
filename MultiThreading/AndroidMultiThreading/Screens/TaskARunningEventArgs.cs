using System;

namespace AndroidMultiThreading.Screens
{
    public class TaskARunningEventArgs : EventArgs
    {
        public TaskARunningEventArgs(bool isRunning)
        {
            IsRunning = isRunning;
        }

        public bool IsRunning { get; }
    }
}
