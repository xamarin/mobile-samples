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
    public class TwitterViewModel : ViewModelBase
    {
        public ObservableCollection<TweetViewModel> Items { get; private set; }

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
                var entries = TwitterFeedManager.GetTweets ();
                PopulateData (entries);

                TwitterFeedManager.UpdateFinished += HandleUpdateFinished;
                TwitterFeedManager.Update ();
            });

            OnPropertyChanged ("IsUpdating");
            OnPropertyChanged ("UpdatingVisibility");
        }

        void HandleUpdateFinished (object sender, EventArgs e)
        {
            TwitterFeedManager.UpdateFinished -= HandleUpdateFinished;
            var entries = TwitterFeedManager.GetTweets ();
            IsUpdating = false;
            PopulateData (entries);
        }

        void PopulateData (IEnumerable<Tweet> entries)
        {
            _dispatcher.BeginInvoke (delegate {
                Items = new ObservableCollection<TweetViewModel> (
                    from e in entries
                    select new TweetViewModel (e));

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
