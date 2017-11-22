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

AsyncAwait : C# 

*/
using System.Diagnostics;


namespace iOS
{
	public partial class iOSViewController : UIViewController
	{
		public iOSViewController(IntPtr handle) : base (handle)
		{
		}

		string localPath;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			GetButton.TouchUpInside += async (sender, e) =>
			{
				Task<int> sizeTask = DownloadHomepageAsync();

				ResultLabel.Text = "loading...";
				ResultTextView.Text = "loading...\n";
				DownloadedImageView.Image = null;

				// await! control returns to the caller
				var intResult = await sizeTask;

				// when the Task<int> returns, the value is available and we can display on the UI
				ResultLabel.Text = "Length: " + intResult;

				// effectively returns void
			};

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

		public async Task<int> DownloadHomepageAsync()
		{
			try
			{
				var httpClient = new HttpClient(); // Xamarin supports HttpClient!

				//
				// download HTML string
				Task<string> contentsTask = httpClient.GetStringAsync("https://xamarin.com"); // async method!

				ResultTextView.Text += "DownloadHomepage method continues after Async() call, until await is used\n";

				// await! control returns to the caller and the task continues to run on another thread
				string contents = await contentsTask;

				// After contentTask completes, you can calculate the length of the string.
				int length = contents.Length;
				ResultTextView.Text += "Downloaded the html and found out the length.\n\n";

				//
				// download image bytes
				ResultTextView.Text += "Start downloading image.\n";

				byte[] imageBytes = await httpClient.GetByteArrayAsync("https://xamarin.com/content/images/pages/about/team-h.jpg"); // async method!  
				ResultTextView.Text += "Downloaded the image.\n";
				await SaveBytesToFileAsync(imageBytes, "team.jpg");
				ResultTextView.Text += "Save the image to a file." + Environment.NewLine;
				DownloadedImageView.Image = UIImage.FromFile(localPath);

				//
				// save image to Photo Album using async-ified iOS API
				ALAssetsLibrary library = new ALAssetsLibrary();     
				var dict = new NSDictionary();
				var assetUrl = await library.WriteImageToSavedPhotosAlbumAsync(DownloadedImageView.Image.CGImage, dict);
				ResultTextView.Text += "Saved to album assetUrl = " + assetUrl + "\n";

				//
				// download multiple images
				// http://blogs.msdn.com/b/pfxteam/archive/2012/08/02/processing-tasks-as-they-complete.aspx
				Task<byte[]> task1 = httpClient.GetByteArrayAsync("https://developer.xamarin.com/guides/cross-platform/advanced/async_support_overview/Images/AsyncAwait.png"); // async method!
				Task<byte[]> task2 = httpClient.GetByteArrayAsync("https://blog.xamarin.com/wp-content/uploads/2013/07/monkey_cowboy.jpg"); // async method!
				Task<byte[]> task3 = httpClient.GetByteArrayAsync("https://developer.xamarin.com/image-doesn't-exist.pngXXX"); // ERROR async method!

		
				List<Task<byte[]>> tasks = new List<Task<byte[]>>();
				tasks.Add(task1);
				tasks.Add(task2);
				tasks.Add(task3);

				while (tasks.Count > 0)
				{ 
					var t = await Task.WhenAny(tasks);
					tasks.Remove(t); 

					try
					{ 
						await t; 
						ResultTextView.Text += "** Downloaded " + t.Result.Length + " bytes\n";
					}
					catch (OperationCanceledException)
					{
					}
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
				Console.WriteLine("Centralized exception handling!");
				return -1;
			}
		}

		// HACK: do not try this at home! just a demo of what happens when you DO block the UI thread :)
		partial void Naysync_TouchUpInside(UIButton sender)
		{
			Thread.Sleep(5000);
			ResultTextView.Text = "Non-asynchronous stuff complete.";
		}

		public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation == UIInterfaceOrientation.Portrait);
		}
	}
}

