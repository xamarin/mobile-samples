using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Tasky.BL.Managers;
using Windows.UI.Xaml.Controls;
using Tasky.BL;

namespace WinPhone81 {
    public partial class TaskDetailsPage : Page {
        public TaskDetailsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == Windows.UI.Xaml.Navigation.NavigationMode.New)
            {
                var vm = new TaskViewModel();
                var task = default(TaskItem);
                int id = (int)e.Parameter;
                task = (App.Current as WinPhone81.App).TaskMgr.GetTask(id);
                if (task != null)
                {
                    vm.Update(task);
                }

                DataContext = vm;
            }
        }  

        private void HandleDelete(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var taskvm = (TaskViewModel)DataContext;
            if (taskvm.ID >= 0)
                (App.Current as WinPhone81.App).TaskMgr.DeleteTask(taskvm.ID);
            this.Frame.GoBack();
        }

        private void HandleSave(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var taskvm = (TaskViewModel)DataContext;
            var task = taskvm.GetTask();
            (App.Current as WinPhone81.App).TaskMgr.SaveTask(task);
            this.Frame.GoBack();
        }
    }
}