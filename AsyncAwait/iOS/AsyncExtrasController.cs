using System;
using System.Threading.Tasks;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Threading;
using System.Net.Http;
using System.Collections.Generic;

namespace iOS
{
	public partial class AsyncExtrasController : UIViewController
	{
		CancellationTokenSource cancellationTokenSource;

		public AsyncExtrasController(IntPtr handle) : base (handle)
		{
		}

		public override void DidReceiveMemoryWarning()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning();
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			StartButton.TouchUpInside += StartDownloadsHandler;
			CancelButton.TouchUpInside += CancelDownloadsHandler;
		}

		void CancelDownloadsHandler (object sender, EventArgs e)
		{
			if (cancellationTokenSource != null)
			{
				cancellationTokenSource.Cancel();
			}
			StatusTextView.Text = "Cancelling downloads....";
		}


		async Task<int> GetBytes(string url, CancellationToken cancelToken)
		{
			HttpClient client = new HttpClient();
			HttpResponseMessage response = await client.GetAsync(url, cancelToken);
			cancelToken.ThrowIfCancellationRequested();
			byte[] bytes = await response.Content.ReadAsByteArrayAsync();
			return bytes.Length;
		}

		async void StartDownloadsHandler (object sender, EventArgs e)
		{
			cancellationTokenSource = new CancellationTokenSource();

			StatusTextView.Text = "Downloads starting....";
			ProgressTextView.Text = String.Empty;

			HttpClient client = new HttpClient();
			string[] listOfImages = new string[]
			{
				"http://xamarin.com/images/tour/amazing-ide.png",
				"http://xamarin.com/images/how-it-works/chalkboard2.jpg", 
				"http://xamarin.com/images/about/team.jpg",
				"http://xamarin.com/images/prebuilt/rich-feature-set.jpg",
				"http://cdn1.xamarin.com/webimages/images/features/shared-code-2.pngXXX",
				"http://xamarin.com/images/tour/4platforms12.jpg",
				"http://xamarin.com/images/tour/amazing-ide.png",
				"http://xamarin.com/images/enterprise/multiple_platforms.png"
			};

			List<Task<int>> tasks = new List<Task<int>>(listOfImages.Length);
			foreach (var item in listOfImages)
			{
				tasks.Add(GetBytes(item, cancellationTokenSource.Token));
			}


			while (tasks.Count > 0)
			{ 
				var t = await Task.WhenAny(tasks);
				tasks.Remove(t); 
				try
				{ 
					await t; 
					ProgressTextView.Text += "** Downloaded " + t.Result + " bytes" + Environment.NewLine;

				}
				catch (OperationCanceledException)
				{
					ProgressTextView.Text += "## Download was cancelled." + Environment.NewLine;
					break;
				}
				catch (Exception exc)
				{ 
					ProgressTextView.Text += "-- Download ERROR: " + exc.Message + Environment.NewLine;
				} 
			}

			StatusTextView.Text = "Downloads finished.";

		}

	}
}

