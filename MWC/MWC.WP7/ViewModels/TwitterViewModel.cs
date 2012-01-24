using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
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

        public void BeginUpdate ()
        {
            IsUpdating = true;

            var entries = TwitterFeedManager.GetTweets ();

            TwitterFeedManager.UpdateFinished += HandleUpdateFinished;
            TwitterFeedManager.Update ();

            PopulateData (entries);
        }

        void HandleUpdateFinished (object sender, EventArgs e)
        {
            TwitterFeedManager.UpdateFinished -= HandleUpdateFinished;

            var entries = TwitterFeedManager.GetTweets ();
            PopulateData (entries);
        }

        void PopulateData (IEnumerable<Tweet> entries)
        {
            Items = new ObservableCollection<TweetViewModel> (
                from e in entries
                select new TweetViewModel (e));

            OnPropertyChanged ("Items");

            ListVisibility = Items.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            NoDataVisibility = Items.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
            IsUpdating = false;

            OnPropertyChanged ("ListVisibility");
            OnPropertyChanged ("NoDataVisibility");
            OnPropertyChanged ("IsUpdating");
        }
    }
}
