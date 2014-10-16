using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Windows.Threading;
using Tasky.BL;
using System.Collections.ObjectModel;
using Tasky.BL.Managers;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace TaskyWinPhone {
    public class TaskListViewModel : ViewModelBase {

        public ObservableCollection<TaskViewModel> Items { get; private set; }

        public bool IsUpdating { get; set; }
        public Visibility ListVisibility { get; set; }
        public Visibility NoDataVisibility { get; set; }

        public Visibility UpdatingVisibility
        {
            get
            {
                return (IsUpdating || Items == null || Items.Count == 0) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        Dispatcher dispatcher;

        public void BeginUpdate(Dispatcher dispatcher) {
            this.dispatcher = dispatcher;

            IsUpdating = true;

            ThreadPool.QueueUserWorkItem(delegate {
                var entries = TaskManager.GetTasks();
                PopulateData(entries);
            });
        }

        void PopulateData(IEnumerable<Task> entries)
        {
            dispatcher.BeginInvoke(delegate {
                //
                // Set all the news items
                //
                Items = new ObservableCollection<TaskViewModel>(
                    from e in entries
                    select new TaskViewModel(e));

                //
                // Update the properties
                //
                OnPropertyChanged("Items");

                ListVisibility = Items.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
                NoDataVisibility = Items.Count == 0 ? Visibility.Visible : Visibility.Collapsed;

                OnPropertyChanged("ListVisibility");
                OnPropertyChanged("NoDataVisibility");
                OnPropertyChanged("IsUpdating");
                OnPropertyChanged("UpdatingVisibility");
            });
        }



    }
}
