using System.Threading;
using System.Threading.Tasks;

using Android.App;
using Android.OS;
using Android.Util;

namespace AndroidMultiThreading.Screens
{
    public delegate void TaskARunStatusHandler(object sender, TaskARunningEventArgs e);

    public class BackgroundTaskRunnerFragment : Fragment
    {
        public static readonly string FRAGMENT_TAG = "state_frag";
        public event TaskARunStatusHandler TaskARunStatusChangeHandler;

        readonly string TAG = "ATM:TaskHelperFragment";

        bool isTaskBRunning;
        public bool IsTaskARunning { get; internal set; }

        public async Task StartTaskUpdateUI()
        {
            if (IsTaskARunning)
            {
                Log.Info(TAG, "Task A is already running.");
                return;
            }

            Log.Info(TAG, "Try to start Task A.");
            IsTaskARunning = true;
            TaskARunStatusChangeHandler?.Invoke(this, new TaskARunningEventArgs(IsTaskARunning));

            await Task.Run(() =>
                           {
                               Log.Info(TAG, "Begin task A.");
                               LongRunningProcess(15, "Task A.");
                               Log.Info(TAG, "Done task A.");
                           });
            IsTaskARunning = false;
            TaskARunStatusChangeHandler?.Invoke(this, new TaskARunningEventArgs(IsTaskARunning));
        }

        public async Task StartTaskNoUpdateUI()
        {
            if (isTaskBRunning)
            {
                Log.Info(TAG, "Task B is already running.");
                return;
            }

            Log.Info(TAG, "Task B starting.");

            var taskB1 = new Task(() => { LongRunningProcess(10, "Task B-1."); });
            var taskB2 = new Task(() => { LongRunningProcess(5, "Task B-2."); });

            isTaskBRunning = true;
            taskB1.Start();
            taskB2.Start();

            await Task.WhenAll(taskB1, taskB2);

            Log.Info(TAG, "Task B is finished.");
            isTaskBRunning = false;
        }


        public static BackgroundTaskRunnerFragment NewInstance()
        {
            var f = new BackgroundTaskRunnerFragment();
            return f;
        }


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RetainInstance = true;
        }

        /// <summary>
        ///     Simulation method to sit for a number of seconds.
        /// </summary>
        protected void LongRunningProcess(int seconds, string name)
        {
            Log.Info(TAG, $"Beginning long running process {name} for {seconds} seconds.");
            Thread.Sleep(seconds * 1000);
            Log.Info(TAG, $"Finished long running process {name} for {seconds} seconds.");
        }
    }
}
