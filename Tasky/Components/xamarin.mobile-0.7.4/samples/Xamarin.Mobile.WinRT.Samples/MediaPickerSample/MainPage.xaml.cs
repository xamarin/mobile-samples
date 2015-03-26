using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MediaPickerSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
	        DataContext = new MainPageViewModel();
			((MainPageViewModel)DataContext).PropertyChanged += OnPropertyChanged;
        }

	    private void OnPropertyChanged (object sender, PropertyChangedEventArgs e)
	    {
			if (e.PropertyName != "Video")
				return;

		    this.video.SetSource (((MainPageViewModel)DataContext).Video, "video/mp4");
	    }

	    /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

		private void OnClickPlay (object sender, RoutedEventArgs e)
		{
			
			this.video.Play();
		}

	    private void OnClickPause (object sender, RoutedEventArgs e)
		{
			this.video.Pause();
		}

		private void OnClickStop (object sender, RoutedEventArgs e)
		{
			this.video.Stop();
		}
    }
}
