using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinComputerVision
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            imgBanner.Source = ImageSource.FromResource("XamarinComputerVision.images.banner.png");
            imgChoosed.Source = ImageSource.FromResource("XamarinComputerVision.images.thumbnail.jpg");
            
        }

        private async void btnPick_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            try
            {
                var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
                });
                if (file == null)
                    return;
                imgChoosed.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
                var result = await GetImageDescription(file.GetStream());
                lblResult.Text = null;
                file.Dispose();
                foreach (string tag in result.Description.Tags)
                {
                    lblResult.Text = lblResult.Text + "\n" + tag;
                }

            }
            catch
            (Exception ex)
            {
                string test = ex.Message;
            }
        }

        public async Task<AnalysisResult> GetImageDescription(Stream imageStream)
        {
            VisionServiceClient visionClient = new VisionServiceClient("a338648c0df347c6b3b9e46ea2022fcd", "https://westcentralus.api.cognitive.microsoft.com/vision/v2.0");
            VisualFeature[] features = { VisualFeature.Tags, VisualFeature.Categories, VisualFeature.Description };
            return await visionClient.AnalyzeImageAsync(imageStream, features.ToList(), null);
        }

        private async void btnTake_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            try
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("No Camera", ":( No camera available.", "OK");
                    return;
                }
                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "Sample",
                    Name = "xamarin.jpg"
                });
                if (file == null)
                    return;
                imgChoosed.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
                var result = await GetImageDescription(file.GetStream());
                file.Dispose();
                lblResult.Text = null;
                foreach (string tag in result.Description.Tags)
                {
                    lblResult.Text = lblResult.Text + "\n" + tag;
                }
            }
            catch(Exception ex)
            {
                string test = ex.Message;
            }
        }
    }
}
