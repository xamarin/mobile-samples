using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using System.IO;
using Tasky.Portable;

namespace Tasky {
    [Application]
    public class TaskyApp : Application {

        public static TaskyApp Current { get; private set; }

        public TaskManager TaskMgr { get; set; }

        public TaskyApp(IntPtr handle, global::Android.Runtime.JniHandleOwnership transfer)
            : base(handle, transfer) {
                Current = this;
        }

        public override void OnCreate()
        {
            base.OnCreate();

            var sqliteFilename = "TaskDB.xml";
            string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(libraryPath, sqliteFilename);

			var xmlStorage = new Tasky.XmlStorage ();
			TaskMgr = new TaskManager(path, xmlStorage);

        }
    }
}
