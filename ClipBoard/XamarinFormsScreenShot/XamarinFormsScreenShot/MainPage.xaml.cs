using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinFormsScreenShot
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            imgBanner.Source = ImageSource.FromResource("XamarinFormsScreenShot.images.banner.png");
            imgCaptured.Source = ImageSource.FromResource("XamarinFormsScreenShot.images.default.jpg");
            
        }

        private async void btnCapture_Clicked(object sender, EventArgs e)
        {
            //Call Dependency Service
            var imageByte = await DependencyService.Get<IScreen>().CaptureScreenAsync();
            imgCaptured.Source = ImageSource.FromStream(() => new MemoryStream(imageByte));
        }
    }
}
