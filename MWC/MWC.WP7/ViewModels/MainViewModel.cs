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
            Speakers = new SpeakerListViewModel ();
            Exhibitors = new ExhibitorListViewModel ();
            News = new NewsListViewModel ();
            Twitter = new TwitterViewModel ();

            Monday = new DateTime (2012, 2, 27);
            Tuesday = new DateTime (2012, 2, 28);
            Wednesday = new DateTime (2012, 2, 29);
            Thursday = new DateTime (2012, 3, 1);
            FavoriteSessionsKey = "favorites";
        }

        public SpeakerListViewModel Speakers { get; private set; }
        public ExhibitorListViewModel Exhibitors { get; private set; }
        public NewsListViewModel News { get; private set; }
        public TwitterViewModel Twitter { get; private set; }

        public DateTime Monday { get; private set; }
        public DateTime Tuesday { get; private set; }
        public DateTime Wednesday { get; private set; }
        public DateTime Thursday { get; private set; }
        public string FavoriteSessionsKey { get; private set; }

        #region Loading

        bool IsDataSeeded
        {
            get
            {
                return IsolatedStorageSettings.ApplicationSettings.Contains ("IsDataSeeded") &&
                    (bool)IsolatedStorageSettings.ApplicationSettings["IsDataSeeded"];
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings["IsDataSeeded"] = value;
            }
        }

        DateTime NextUpdateTimeUtc
        {
            get
            {
                return IsolatedStorageSettings.ApplicationSettings.Contains ("NextUpdateTimeUtc") ?
                    (DateTime)IsolatedStorageSettings.ApplicationSettings["NextUpdateTimeUtc"] :
                    DateTime.UtcNow;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings["NextUpdateTimeUtc"] = value;
            }
        }

        public bool HasBeenUpdated { get; private set; }

        public void BeginUpdate (Dispatcher dispatcher)
        {
            HasBeenUpdated = true;

            //
            // Seed the Conference DB
            //
            var isDataSeeded = IsDataSeeded;
            if (!isDataSeeded) {

                using (var s = Application.GetResourceStream (new Uri ("Assets\\SeedData.xml", UriKind.Relative)).Stream) {
                    using (var r = new StreamReader (s)) {
                        UpdateManager.UpdateFromFile (r.ReadToEnd ());
                    }
                }

                IsDataSeeded = true;
            }

            //
            // Update the DB if we only have seed data or if it's time for an update
            //
            if (!isDataSeeded || NextUpdateTimeUtc >= DateTime.UtcNow) {

                ThreadPool.QueueUserWorkItem (delegate {

                    UpdateManager.UpdateFinished += delegate {                        
                        dispatcher.BeginInvoke (delegate {
                            NextUpdateTimeUtc = DateTime.UtcNow.AddHours (1);
                            UpdateConferenceViewModels (dispatcher);
                        });
                    };

                    UpdateManager.UpdateConference ();

                });
            }

            //
            // Show whatever data we happen to have at this point
            //
            UpdateConferenceViewModels (dispatcher);
            News.BeginUpdate (dispatcher);
            Twitter.BeginUpdate (dispatcher);
        }

        void UpdateConferenceViewModels (Dispatcher dispatcher)
        {
            Speakers.BeginUpdate (dispatcher);
            Exhibitors.BeginUpdate (dispatcher);
        }

        #endregion
    }
}
