using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using MWC.BL;
using MWC.BL.Managers;

namespace MWC.WP7.ViewModels
{
    public class NewsListViewModel : ViewModelBase
    {
        public ObservableCollection<NewsItemViewModel> Items { get; private set; }

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

        Dispatcher _dispatcher;

        public void BeginUpdate (Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;

            IsUpdating = true;

            ThreadPool.QueueUserWorkItem (delegate {
                var entries = NewsManager.Get ();
                PopulateData (entries);

                NewsManager.UpdateFinished += HandleUpdateFinished;
                NewsManager.Update ();
            });

            OnPropertyChanged ("IsUpdating");
            OnPropertyChanged ("UpdatingVisibility");
        }

        void HandleUpdateFinished (object sender, EventArgs e)
        {
            NewsManager.UpdateFinished -= HandleUpdateFinished;
            var entries = NewsManager.Get ();
            IsUpdating = false;
            PopulateData (entries);
        }

        void PopulateData (IEnumerable<RSSEntry> entries)
        {
            _dispatcher.BeginInvoke (delegate {
                Items = new ObservableCollection<NewsItemViewModel> (
                    from e in entries
                    select new NewsItemViewModel (e));

                OnPropertyChanged ("Items");

                ListVisibility = Items.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
                NoDataVisibility = Items.Count == 0 ? Visibility.Visible : Visibility.Collapsed;

                OnPropertyChanged ("ListVisibility");
                OnPropertyChanged ("NoDataVisibility");
                OnPropertyChanged ("IsUpdating");
                OnPropertyChanged ("UpdatingVisibility");
            });
        }
    }
}
