using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Threading;
using MWC.BL;
using MWC.BL.Managers;

namespace MWC.WP7.ViewModels
{
    public class TwitterViewModel : ViewModelBase
    {
        public ObservableCollection<TweetViewModel> Items { get; private set; }

        public void BeginUpdate ()
        {
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
        }
    }
}
