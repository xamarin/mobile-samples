using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Tasky.BL;

namespace WinPhone81
{
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
