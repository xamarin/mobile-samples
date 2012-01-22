using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace MWC.WP7.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            Speakers = new SpeakerListViewModel ();
        }

        public SpeakerListViewModel Speakers { get; private set; }

        public bool IsDataLoaded { get; private set; }

        public void LoadData ()
        {
            Speakers.LoadData ();
        }
    }
}
