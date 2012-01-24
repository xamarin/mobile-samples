using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;
using MWC.BL;
using MWC.BL.Managers;
using System.Threading;

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
                select new NewsItemViewModel {
                    ID = e.ID,
                    Url = e.Url,
                    Title = e.Title,
                    Published = e.Published,
                    Content = e.Content,
                });

            OnPropertyChanged ("Items");
        }
    }
}
