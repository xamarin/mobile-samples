using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Tasky.BL.Managers;
using Tasky.DL.SQLite;
using System.IO;

namespace TaskyAndroid {
    [Application]
    public class TaskyApp : Application {
        public static TaskyApp Current { get; private set; }

        public TaskManager TaskMgr { get; set; }
        Connection conn;

        public TaskyApp(IntPtr handle, global::Android.Runtime.JniHandleOwnership transfer)
            : base(handle, transfer) {
                Current = this;
        }

        public override void OnCreate()
        {
            base.OnCreate();

            var sqliteFilename = "TaskDB.db3";
            string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(libraryPath, sqliteFilename);
            conn = new Connection(path);

            TaskMgr = new TaskManager(conn);
        }
    }
}
