using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using MWC.WP7.ViewModels;
using MWC.BL.Managers;
using Microsoft.Phone.Tasks;

namespace MWC.WP7
{
    public partial class TweetDetails : PhoneApplicationPage
    {
        public TweetDetails ()
        {
            InitializeComponent ();
        }

        protected override void OnNavigatedTo (System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo (e);

            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.New) {
                var vm = new TweetViewModel ();

                if (NavigationContext.QueryString.ContainsKey ("id")) {
                    var id = int.Parse (NavigationContext.QueryString["id"]);
                    var tweet = TwitterFeedManager.GetTweet (id);
                    if (tweet != null) {
                        vm.Update (tweet);
                    }
                }

                if (!string.IsNullOrEmpty (vm.Content)) {

                    //
                    // Adapt to the theme
                    //
                    var bgColor = "black";
                    var color = "white";
                    if ((Visibility)Application.Current.Resources["PhoneLightThemeVisibility"] == Visibility.Visible) {
                        bgColor = "white";
                        color = "black";
                    }

                    var accentColor = (Color)Application.Current.Resources["PhoneAccentColor"];
                    var linkColor = "#" + accentColor.ToString ().Substring(3);
                    
                    //
                    // Show the text
                    //
                    var html = string.Format ("<html><head><style>body{{background-color:{0};color:{1};}} a{{color:{2};}}</style></head><body>{3}</body></html>",
                        bgColor, color, linkColor, vm.Content);

                    Browser.NavigateToString (html);
                }

                DataContext = vm;
            }
        }

        private void Browser_Navigating (object sender, NavigatingEventArgs e)
        {
            e.Cancel = true;

            var task = new WebBrowserTask {
                Uri = e.Uri,
            };
            task.Show ();
        }

        private void Browser_Navigated (object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            Browser.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
