using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace AsyncAwait
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed class MainPage_old : Page
    {
        public MainPage_old()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
        string localPath;

        delegate void HomepageDownloaded(int length);
        HomepageDownloaded downloaded;
        void HandleTouchUpInside(object sender, EventArgs e)
        {
            ResultLabel.Text = "loading...";
            ResultTextView.Text = "loading...\n";
            DownloadedImageView.Source = null;

            int intResult;
            downloaded += (len) =>
            {
                intResult = len;
                ResultLabel.Text = "Length: " + intResult;
            };
#if OLD
            // it's actually impossible to really show this old-style in a Windows Store app
            // the non-async APIs just don't exist :-)
            DownloadHomepage();
#endif
        }

#if OLD
        // it's actually impossible to really show this old-style in a Windows Store app
        // the non-async APIs just don't exist :-)
        public void DownloadHomepage()
        {
            var webClient = new WebClient(); // not in Windows Store APIs

            webClient.DownloadStringCompleted += (sender, e) =>
            {
                if (e.Cancelled || e.Error != null)
                {
                    // do something with error
                }
                string contents = e.Result;

                int length = contents.Length;
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    ResultTextView.Text += "Downloaded the html and found out the length.\n\n";
                });
                webClient.DownloadDataCompleted += (sender1, e1) =>
                {
                    if (e1.Cancelled || e1.Error != null)
                    {
                        // do something with error
                    }
                    SaveBytesToFile(e1.Result, "team.jpg");

                    Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        ResultTextView.Text += "Downloaded the image.\n";

                        var img = ApplicationData.Current.LocalFolder.GetFileAsync("team.jpg");
                        var i = new BitmapImage(new Uri(img.Path, UriKind.Absolute));
                        DownloadedImageView.Source = i;
                    });

                    if (downloaded != null)
                        downloaded(length);
                };
                webClient.DownloadDataAsync(new Uri("http://xamarin.com/images/about/team.jpg"));
            };

            webClient.DownloadStringAsync(new Uri("http://xamarin.com/"));
        }
#endif

        async Task SaveBytesToFile(byte[] r, string f)
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            try
            {
                StorageFile d = await localFolder.GetFileAsync(f);
                await d.DeleteAsync();
            }
            catch { }
            StorageFile file = await localFolder.CreateFileAsync(f);
            await FileIO.WriteBytesAsync(file, r);
        }

        #region irrelevant
       
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.TextBox ResultTextView;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Image DownloadedImageView;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.TextBlock ResultLabel;

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent()
        {
            global::Windows.UI.Xaml.Application.LoadComponent(this, new global::System.Uri("ms-appx:///MainPage.xaml"), global::Windows.UI.Xaml.Controls.Primitives.ComponentResourceLocation.Application);

            ResultTextView = (global::Windows.UI.Xaml.Controls.TextBox)this.FindName("ResultTextView");
            DownloadedImageView = (global::Windows.UI.Xaml.Controls.Image)this.FindName("DownloadedImageView");
            ResultLabel = (global::Windows.UI.Xaml.Controls.TextBlock)this.FindName("ResultLabel");
        }
        #endregion
    }
}