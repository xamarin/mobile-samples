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

AsyncAwait : C# old-style 

*/
namespace iOS
{
	public partial class iOSViewController_old : UIViewController
	{
		string localPath;
		public iOSViewController_old (IntPtr handle) : base (handle)
		{
		}


		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			GetButton.TouchUpInside += HandleTouchUpInside;
		}


		delegate void HomepageDownloaded (int length);		
		HomepageDownloaded downloaded;
		void HandleTouchUpInside (object sender, EventArgs e)
		{
			ResultLabel.Text = "loading...";
			ResultTextView.Text = "loading...\n";
			DownloadedImageView.Image = null;

			int intResult;
			downloaded += (len) => {
				intResult = len;
				ResultLabel.Text = "Length: " + intResult ;
			};
			DownloadHomepage();
		}


		public void DownloadHomepage()
		{
			var webClient = new WebClient();

			webClient.DownloadStringCompleted += (sender, e) => {
				if(e.Cancelled || e.Error != null) {
					// do something with error
				}
				string contents = e.Result;

				int length = contents.Length;
				InvokeOnMainThread (() => {
					ResultTextView.Text += "Downloaded the html and found out the length.\n\n";
				});
				webClient.DownloadDataCompleted += (sender1, e1) => {
					if(e1.Cancelled || e1.Error != null) {
						// do something with error
					}
					SaveBytesToFile(e1.Result, "team.jpg");
					
					InvokeOnMainThread (() => {
						ResultTextView.Text += "Downloaded the image.\n";
						DownloadedImageView.Image = UIImage.FromFile (localPath);
					});

					ALAssetsLibrary library = new ALAssetsLibrary();     
					var dict = new NSDictionary();
					library.WriteImageToSavedPhotosAlbum (DownloadedImageView.Image.CGImage, dict, (s2,e2) => {
						InvokeOnMainThread (() => {
							ResultTextView.Text += "Saved to album assetUrl\n";
						});
						if (downloaded != null)
							downloaded(length);
					});
				};
				webClient.DownloadDataAsync(new Uri("http://xamarin.com/images/about/team.jpg"));
			};

			webClient.DownloadStringAsync(new Uri("http://xamarin.com/"));
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
		UIKit.UIImageView DownloadedImageView { get; set; }

		[Outlet]
		UIKit.UIButton GetButton { get; set; }

		[Outlet]
		UIKit.UILabel ResultLabel { get; set; }

		[Outlet]
		UIKit.UITextView ResultTextView { get; set; }

		[Action ("UIButton9_TouchUpInside:")]
		partial void UIButton9_TouchUpInside (UIKit.UIButton sender);
		#endregion
	}
}

