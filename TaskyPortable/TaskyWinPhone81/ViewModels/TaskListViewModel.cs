using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using Tasky.BL;
using System.Collections.ObjectModel;
using Tasky.BL.Managers;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace WinPhone81
{
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

        CoreDispatcher dispatcher;

        public void BeginUpdate(CoreDispatcher dispatcher) {
            this.dispatcher = dispatcher;

            IsUpdating = true;

            ThreadPool.RunAsync(delegate {
                var entries = (App.Current as WinPhone81.App).TaskMgr.GetTasks();
                PopulateData(entries);
            });
        }

        void PopulateData(IEnumerable<TaskItem> entries)
        {
            dispatcher.RunIdleAsync(delegate {
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
