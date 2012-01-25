using System;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using MWC.BL.Managers;

namespace MWC.WP7.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            News = new NewsListViewModel ();
            Twitter = new TwitterViewModel ();

            Monday = new DateTime (2012, 2, 27);
            Tuesday = new DateTime (2012, 2, 28);
            Wednesday = new DateTime (2012, 2, 29);
            Thursday = new DateTime (2012, 3, 1);
            FavoriteSessionsKey = "favorites";
        }

        public NewsListViewModel News { get; private set; }
        public TwitterViewModel Twitter { get; private set; }

        public DateTime Monday { get; private set; }
        public DateTime Tuesday { get; private set; }
        public DateTime Wednesday { get; private set; }
        public DateTime Thursday { get; private set; }
        public string FavoriteSessionsKey { get; private set; }

        public void BeginUpdate (Dispatcher dispatcher)
        {
            News.BeginUpdate (dispatcher);
            Twitter.BeginUpdate (dispatcher);
        }
    }
}
