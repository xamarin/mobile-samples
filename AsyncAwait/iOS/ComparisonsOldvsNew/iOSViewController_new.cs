using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.IO;
using MonoTouch.AssetsLibrary;
using System.Collections.Generic;
using System.Threading;
/*

AsyncAwait : C# 5 style async-await

*/
namespace iOS
{
	public partial class iOSViewController2a : UIViewController
	{
		string localPath;
		public iOSViewController2a (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			GetButton.TouchUpInside += HandleTouchUpInside;
		}

		async void HandleTouchUpInside (object sender, EventArgs e)
		{
			ResultLabel.Text = "loading...";
			ResultTextView.Text = "loading...\n";
			DownloadedImageView.Image = null;

			Task<int> sizeTask = DownloadHomepageAsync();
			var intResult = await sizeTask;
			ResultLabel.Text = "Length: " + intResult ;
		}

		public async Task<int> DownloadHomepageAsync()
		{
			try {
				var httpClient = new HttpClient();

				Task<string> contentsTask = httpClient.GetStringAsync("http://xamarin.com"); 

				string contents = await contentsTask;

				int length = contents.Length;
				ResultTextView.Text += "Downloaded the html and found out the length.\n\n";

				byte[] imageBytes  = await httpClient.GetByteArrayAsync("http://xamarin.com/images/about/team.jpg"); 
				SaveBytesToFile(imageBytes, "team.jpg");
				ResultTextView.Text += "Downloaded the image.\n";
				DownloadedImageView.Image = UIImage.FromFile (localPath);

				ALAssetsLibrary library = new ALAssetsLibrary();     
				var dict = new NSDictionary();
				var assetUrl = await library.WriteImageToSavedPhotosAlbumAsync 
													(DownloadedImageView.Image.CGImage, dict);
				ResultTextView.Text += "Saved to album assetUrl = " + assetUrl + "\n";

				ResultTextView.Text += "\n\n\n" + contents; // just dump the entire HTML
				return length; 
			} catch {
				// do something with error
				return -1;
			}
		}



		void SaveBytesToFile(byte[] r, string f)
		{
			string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string localFilename = f;
			localPath = Path.Combine (documentsPath, localFilename);
			File.WriteAllBytes (localPath, r); // writes to local storage   
		}

		#region irrelevant
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation == UIInterfaceOrientation.Portrait);
		}

		
		[Outlet]
		MonoTouch.UIKit.UIImageView DownloadedImageView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton GetButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel ResultLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextView ResultTextView { get; set; }

		[Action ("UIButton9_TouchUpInside:")]
		partial void UIButton9_TouchUpInside (MonoTouch.UIKit.UIButton sender);
		#endregion
	}
}

