using System;
using CoreGraphics;
using Foundation;
using UIKit;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.IO;
using AssetsLibrary;
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

		public iOSViewController2a(IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			GetButton.TouchUpInside += HandleTouchUpInside;
		}

		async void HandleTouchUpInside(object sender, EventArgs e)
		{
			ResultLabel.Text = "loading...";
			ResultTextView.Text = "loading...\n";
			DownloadedImageView.Image = null;

			Task<int> sizeTask = DownloadHomepageAsync();
			var intResult = await sizeTask;
			ResultLabel.Text = "Length: " + intResult;
		}

		public async Task<int> DownloadHomepageAsync()
		{
			try
			{
				var httpClient = new HttpClient();

				Task<string> contentsTask = httpClient.GetStringAsync("http://xamarin.com"); 

				string contents = await contentsTask;

				int length = contents.Length;
				ResultTextView.Text += "Downloaded the html and found out the length.\n\n";

				byte[] imageBytes = await httpClient.GetByteArrayAsync("http://xamarin.com/images/about/team.jpg"); 
				await SaveBytesToFileAsync(imageBytes, "team.jpg");
				ResultTextView.Text += "Downloaded the image.\n";
				ResultTextView.Text += "Save the image to a file." + Environment.NewLine;
				DownloadedImageView.Image = UIImage.FromFile(localPath);

				ALAssetsLibrary library = new ALAssetsLibrary();     
				var dict = new NSDictionary();
				var assetUrl = await library.WriteImageToSavedPhotosAlbumAsync 
													(DownloadedImageView.Image.CGImage, dict);
				ResultTextView.Text += "Saved to album assetUrl = " + assetUrl + "\n";

				ResultTextView.Text += "\n\n\n" + contents; // just dump the entire HTML
				return length; 
			}
			catch
			{
				// do something with error
				return -1;
			}
		}

		async Task SaveBytesToFileAsync(byte[] bytesToSave, string fileName)
		{
			string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string localFilename = fileName;
			localPath = Path.Combine(documentsPath, localFilename);

			if (File.Exists(localPath))
			{
				File.Delete(localPath);
			}

			using (FileStream fs = new FileStream(localPath, FileMode.Create, FileAccess.Write))
			{
				await fs.WriteAsync(bytesToSave, 0, bytesToSave.Length);
			}
		}

		#region irrelevant

		public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation == UIInterfaceOrientation.Portrait);
		}

		[Outlet]
		UIKit.UIImageView DownloadedImageView { get; set; }

		[Outlet]
		UIKit.UIButton GetButton { get; set; }

		[Outlet]
		UIKit.UILabel ResultLabel { get; set; }

		[Outlet]
		UIKit.UITextView ResultTextView { get; set; }

		[Action("UIButton9_TouchUpInside:")]
		partial void UIButton9_TouchUpInside(UIKit.UIButton sender);

		#endregion

	}
}

