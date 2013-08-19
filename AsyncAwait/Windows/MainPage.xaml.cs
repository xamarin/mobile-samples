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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AsyncAwait
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Task<int> sizeTask = DownloadHomepageAsync();

            ResultLabel.Text = "loading...";
            ResultTextView.Text = "loading...\n";
            DownloadedImageView.Source = null;

            // await! control returns to the caller
            var intResult = await sizeTask;

            // when the Task<int> returns, the value is available and we can display on the UI
            ResultLabel.Text = "Length: " + intResult;
        }

        public async Task<int> DownloadHomepageAsync()
        {
            try
            {
                var httpClient = new HttpClient(); // Xamarin supports HttpClient!

                //
                // download HTML string
                Task<string> contentsTask = httpClient.GetStringAsync("http://xamarin.com"); // async method!

                ResultTextView.Text += "DownloadHomepage method continues after Async() call, until await is used\n";

                // await! control returns to the caller and the task continues to run on another thread
                string contents = await contentsTask;

                // After contentTask completes, you can calculate the length of the string.
                int length = contents.Length;
                ResultTextView.Text += "Downloaded the html and found out the length.\n\n";

                //
                // download image bytes
                ResultTextView.Text += "Start downloading image.\n";

                byte[] imageBytes = await httpClient.GetByteArrayAsync("http://xamarin.com/images/about/team.jpg"); // async method!  
                await SaveBytesToFile(imageBytes, "team.jpg");
                ResultTextView.Text += "Downloaded the image.\n";

                //
                // Save and display the image
                var img = await ApplicationData.Current.LocalFolder.GetFileAsync("team.jpg");
                var i = new BitmapImage(new Uri(img.Path, UriKind.Absolute));
                DownloadedImageView.Source = i;

                //
                // download multiple images
                // http://blogs.msdn.com/b/pfxteam/archive/2012/08/02/processing-tasks-as-they-complete.aspx
                Task<byte[]> task1 = httpClient.GetByteArrayAsync("http://xamarin.com/images/tour/amazing-ide.png"); 
                Task<byte[]> task2 = httpClient.GetByteArrayAsync("http://xamarin.com/images/how-it-works/chalkboard2.jpg");
                Task<byte[]> task3 = httpClient.GetByteArrayAsync("http://cdn1.xamarin.com/webimages/images/features/shared-code-2.pngXXX"); // ERROR will fail - bad file extension 404!
                Task<byte[]> task4 = httpClient.GetByteArrayAsync("http://xamarin.com/images/about/team.jpg"); 

                List<Task<byte[]>> tasks = new List<Task<byte[]>>();
                tasks.Add(task1);
                tasks.Add(task2);
                tasks.Add(task3);
                tasks.Add(task4);

                while (tasks.Count > 0)
                {
                    var t = await Task.WhenAny(tasks);
                    tasks.Remove(t);

                    try
                    {
                        await t;
                        ResultTextView.Text += "** Downloaded " + t.Result.Length + " bytes\n";
                    }
                    catch (OperationCanceledException) { }
                    catch (Exception exc)
                    {
                        ResultTextView.Text += "-- Download ERROR: " + exc.Message + "\n";
                    }
                }



                // this doesn't happen until the image has downloaded as well
                ResultTextView.Text += "\n\n\n" + contents; // just dump the entire HTML
                return length; // Task<TResult> returns an object of type TResult, in this case int
            }
            catch (Exception ex)
            {
               // Console.WriteLine("Centralized exception handling!");
                return -1;
            }
        }

        async Task SaveBytesToFile(byte[] r, string f)
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            try {
                StorageFile d = await localFolder.GetFileAsync(f);
                await d.DeleteAsync();
            } catch { }
            StorageFile file = await localFolder.CreateFileAsync(f);
            await FileIO.WriteBytesAsync(file, r); 
        }

        // HACK: demonstrate what happens when you DO block the UI thread!
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // HACK: do not try this at home!
            using (EventWaitHandle tmpEvent = new ManualResetEvent(false))
            {
                tmpEvent.WaitOne(TimeSpan.FromSeconds(5));
            }
        }
    }
}
