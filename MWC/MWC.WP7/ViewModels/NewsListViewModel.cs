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
    public class NewsListViewModel : ViewModelBase
    {
        public ObservableCollection<NewsItemViewModel> Items { get; private set; }

        Dispatcher _dispatcher;

        public void BeginUpdate (Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;

            var entries = NewsManager.Get ();

            NewsManager.UpdateFinished += HandleUpdateFinished;

            ThreadPool.QueueUserWorkItem (delegate {
                NewsManager.Update ();
            });
            
            PopulateData (entries);
        }

        void HandleUpdateFinished (object sender, EventArgs e)
        {
            var entries = NewsManager.Get ();

            if (_dispatcher != null) {
                _dispatcher.BeginInvoke (delegate {
                    NewsManager.UpdateFinished -= HandleUpdateFinished;
                    PopulateData (entries);
                });
            }
        }

        void PopulateData (IEnumerable<RSSEntry> entries)
        {
            Items = new ObservableCollection<NewsItemViewModel> (
                from e in entries
                select new NewsItemViewModel (e));

            OnPropertyChanged ("Items");
        }
    }
}
