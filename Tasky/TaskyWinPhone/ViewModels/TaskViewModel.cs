﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Tasky.BL;

namespace TaskyWP7 {
    public class TaskViewModel : ViewModelBase {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public bool Done { get; set; }
    
        public TaskViewModel ()
        {
        }
        public TaskViewModel (TaskItem item)
        {
            Update (item);
        }

        public void Update (TaskItem item)
        {
            ID = item.ID;
            Name = item.Name;
            Notes = item.Notes;
            Done = item.Done;
        }

        public TaskItem GetTask() {
            return new TaskItem {
                ID = this.ID,
                Name = this.Name,
                Notes = this.Notes,
                Done = this.Done
            };
        }
    }
}
