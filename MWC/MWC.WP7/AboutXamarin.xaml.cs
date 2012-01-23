using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace MWC.WP7
{
    public partial class AboutXamarin : PhoneApplicationPage
    {
        public AboutXamarin ()
        {
            InitializeComponent ();
        }

        private void HyperlinkButton_Click (object sender, RoutedEventArgs e)
        {
            var task = new WebBrowserTask {
                Uri = new Uri ("http://xamarin.com"),
            };
            task.Show ();
        }
    }
}
