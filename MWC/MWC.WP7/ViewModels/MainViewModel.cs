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

            Monday = new DateTime (2012, 2, 27);
            Tuesday = new DateTime (2012, 2, 28);
            Wednesday = new DateTime (2012, 2, 29);
            Thursday = new DateTime (2012, 3, 1);
        }

        public SpeakerListViewModel Speakers { get; private set; }

        public DateTime Monday { get; private set; }
        public DateTime Tuesday { get; private set; }
        public DateTime Wednesday { get; private set; }
        public DateTime Thursday { get; private set; }

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

        public void LoadData (Dispatcher dispatcher)
        {
            //
            // Seed the DB
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
                            UpdateViewModelData ();
                        });
                    };

                    UpdateManager.UpdateConference ();

                });
            }

            //
            // Show whatever data we happen to have at this point
            //
            UpdateViewModelData ();
        }

        void UpdateViewModelData ()
        {
            Debug.WriteLine ("UpdateViewModelData()");
            Speakers.Update ();
        }

        #endregion
    }
}
