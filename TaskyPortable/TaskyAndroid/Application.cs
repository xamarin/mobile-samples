using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Tasky.BL.Managers;
using System.IO;
using SQLite;

namespace TaskyAndroid {
    [Application]
    public class TaskyApp : Application {
        public static TaskyApp Current { get; private set; }

        public TaskItemManager TaskMgr { get; set; }
        SQLiteConnection conn;

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
            conn = new SQLiteConnection(path);

            TaskMgr = new TaskItemManager(conn);
        }
    }
}
