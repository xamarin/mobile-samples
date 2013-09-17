using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;

namespace Droid
{
	[Activity (Label = "Droid", MainLauncher = true)]
	public class MainActivity : Activity
	{
		Button GetButton;
		TextView ResultTextView;
		EditText ResultEditText;
		ImageView DownloadedImageView;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			GetButton = FindViewById<Button> (Resource.Id.GetButton);
			ResultTextView = FindViewById<TextView> (Resource.Id.ResultTextView);
			ResultEditText = FindViewById<EditText> (Resource.Id.ResultEditText);
			DownloadedImageView = FindViewById<ImageView> (Resource.Id.DownloadedImageView);

			GetButton.Click += async (sender, e) => {

				Task<int> sizeTask = DownloadHomepageAsync();

				ResultTextView.Text = "loading...";
				ResultEditText.Text = "loading...\n";
				DownloadedImageView.SetImageResource (Android.Resource.Drawable.IcMenuGallery);

				// await! control returns to the caller
				var length = await sizeTask;

				// when the Task<int> returns, the value is available and we can display on the UI
				ResultTextView.Text = "Length: " + length ;

				// effectively returns void
			};

			// Alternate way to call
//			GetButton.Click += HandleClick;
		}

		public async Task<int> DownloadHomepageAsync()
		{
			var httpClient = new HttpClient(); // Xamarin supports HttpClient!

			Task<string> contentsTask = httpClient.GetStringAsync("http://xamarin.com"); // async method!


			// await! control returns to the caller and the task continues to run on another thread
			string contents = await contentsTask;
			ResultEditText.Text += "DownloadHomepage method continues after async call. . . . .\n";

			// After contentTask completes, you can calculate the length of the string.
			int exampleInt = contents.Length;

			ResultEditText.Text += "Downloaded the html and found out the length.\n\n\n";




			byte[] imageBytes  = await httpClient.GetByteArrayAsync("http://xamarin.com/images/about/team.jpg"); // async method!

			string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			string localFilename = "team.jpg";
			string localPath = Path.Combine (documentsPath, localFilename);
			File.WriteAllBytes (localPath, imageBytes); // writes to local storage   

			ResultTextView.Text += "Downloaded the image.\n";

			// ImageView stuff goes here

			ResultEditText.Text += contents; // just dump the entire HTML
			return exampleInt; // Task<TResult> returns an object of type TResult, in this case int
		}


		// Alternate way to call - notice the void return
//		async void HandleClick (object sender, EventArgs e)
//		{
//			ResultTextView.Text = "loading...";
//			ResultEditText.Text = "loading...\n";
//
//			// await! control returns to the caller
//			var intResult = await DownloadHomepage();
//
//			// when the Task<int> returns, the value is available and we can display on the UI
//			ResultTextView.Text = "Length: " + intResult ;
//		}
	}
}


