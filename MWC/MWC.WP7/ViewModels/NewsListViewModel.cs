using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
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

        public void BeginUpdate ()
        {
            IsUpdating = true;

            var entries = NewsManager.Get ();

            NewsManager.UpdateFinished += HandleUpdateFinished;
            NewsManager.Update ();
            
            PopulateData (entries);
        }

        void HandleUpdateFinished (object sender, EventArgs e)
        {
            NewsManager.UpdateFinished -= HandleUpdateFinished;

            var entries = NewsManager.Get ();            
            PopulateData (entries);
        }

        void PopulateData (IEnumerable<RSSEntry> entries)
        {
            Items = new ObservableCollection<NewsItemViewModel> (
                from e in entries
                select new NewsItemViewModel (e));

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
