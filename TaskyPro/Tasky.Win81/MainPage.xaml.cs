using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tasky.BL;
using Tasky.BL.Managers;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TaskyWin8
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            DataContext = new TaskListViewModel();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ((TaskListViewModel)DataContext).BeginUpdate();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var task = new Task();
            task.Name = "<new task>";
            task.Notes = "";
            TaskManager.SaveTask(task);
            ((TaskListViewModel)DataContext).BeginUpdate();
        }

        private void Task_Tap(object sender, TappedRoutedEventArgs e)
        {
            var lb = sender as ListBox;
            var tvm = lb.SelectedItem as TaskViewModel;
            ((TaskListViewModel)DataContext).PopulateTaskViewModel(tvm);
        }

        private void Save_Tap(object sender, TappedRoutedEventArgs e)
        {
            var t = ((TaskListViewModel)DataContext).GetTask();
            if (t != null) {
                TaskManager.SaveTask(t);
                ((TaskListViewModel)DataContext).BeginUpdate();
            }
        }

        private void Delete_Tap(object sender, TappedRoutedEventArgs e)
        {
            var t = ((TaskListViewModel)DataContext).GetTask();
            if (t != null)
            {
                TaskManager.DeleteTask(t.ID);
                ((TaskListViewModel)DataContext).BeginUpdate();
            }
        }
    }
}
