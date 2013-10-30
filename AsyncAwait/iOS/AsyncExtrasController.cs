using System;
using System.Threading.Tasks;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Threading;
using System.Net.Http;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Drawing;

namespace iOS
{
	public class DownloadBytesProgress
	{
		public DownloadBytesProgress(string fileName, int bytesReceived, int totalBytes)
		{
			Filename = fileName;
			BytesReceived = bytesReceived;
			TotalBytes = totalBytes;
		}
		public int TotalBytes { get; private set; }
		public int BytesReceived { get; private set; }
		public float PercentComplete { get { return (float) BytesReceived / TotalBytes; } }
		public string Filename { get; private set; }
		public bool IsFinished { get { return BytesReceived == TotalBytes; } }
	}

	public partial class AsyncExtrasController : UIViewController
	{
		/// <summary>
		/// A list of all the files we want to download.
		/// </summary>
		static readonly string[]  ListOfImages = new string[]
		{
			"http://xamarin.com/images/tour/amazing-ide.png",
			"http://xamarin.com/images/how-it-works/chalkboard2.jpg", 
			"http://xamarin.com/images/about/team.jpg",
			"http://xamarin.com/images/prebuilt/rich-feature-set.jpg",
			"http://cdn1.xamarin.com/webimages/images/features/shared-code-2.pngXXX",
			"http://xamarin.com/images/tour/4platforms12.jpg",
			"http://xamarin.com/images/tour/amazing-ide.png",
			"http://xamarin.com/images/enterprise/multiple_platforms.png",
			"http://blog.xamarin.com/wp-content/uploads/2013/07/monkey_cowboy.jpg",
			"http://docs.xamarin.com/guides/cross-platform/getting_started/introducing_xamarin_studio/Images/19.png"
		};


		readonly List<UIProgressView> progressViews = new List<UIProgressView>();
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
				StatusTextView.Text = "Cancelling downloads....";
				cancellationTokenSource.Cancel();
			}
		}

		async Task<int> GetBytes(string url, CancellationToken cancelToken, IProgress<DownloadBytesProgress> progressIndicator)
		{
			int receivedBytes = 0;
			int totalBytes = -1;
			WebClient client = new WebClient();

			using (var stream = await client.OpenReadTaskAsync(url))
			{
				Int32.TryParse(client.ResponseHeaders[HttpResponseHeader.ContentLength], out totalBytes);

				byte[] buffer = new byte[4096];
				for (;;)
				{
					int len = await stream.ReadAsync(buffer, 0, buffer.Length);
					if (len == 0)
					{
						// Looks like we're out of bytes (done). Time to get out of the loop.
						await Task.Yield();
						break;
					}
					receivedBytes += len;

					// Update the UIProgressView associated with this download - display the bytes we have.
					if (progressIndicator != null)
					{
						// Tell IProgress<T> implementation to update the UI with how things are going.
						DownloadBytesProgress args = new DownloadBytesProgress(url, receivedBytes, totalBytes);
						progressIndicator.Report(args);
					}

					// If the user has clicked the Cancel button, then cancel the download.
					cancelToken.ThrowIfCancellationRequested();
				}
			}
			return receivedBytes;
		}

		async void StartDownloadsHandler (object sender, EventArgs e)
		{
			cancellationTokenSource = new CancellationTokenSource();

			InitializeViews();

			List<Task<int>> tasks = CreateTaskForEachFileToDownload();

			while (tasks.Count > 0)
			{ 
				var t = await Task.WhenAny(tasks);  
				// Take the task that has just finished, remove it from the list of outstanding 
				// tasks, and then update the progress bar that displays the overall progress.
				tasks.Remove(t); 
				ProgressBar.Progress = (float)(ListOfImages.Length - tasks.Count) / ListOfImages.Length;

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
			cancellationTokenSource = null;
		}

		void InitializeViews()
		{
			StatusTextView.Text = "Downloads starting....";
			ProgressTextView.Text = String.Empty;
			ProgressBar.Progress = 0;

			// Remove any existing UIProgressViews in the current View.
			foreach (var item in progressViews)
			{
				item.RemoveFromSuperview();
			}
		}

		List<Task<int>> CreateTaskForEachFileToDownload()
		{
			List<Task<int>> tasks = new List<Task<int>>(ListOfImages.Length);
			progressViews.Clear();

			for (int idx =0; idx< ListOfImages.Length; idx++)
			{
				// Add a UIProgressView for each file that is to be downloaded.
				RectangleF frame = new RectangleF(20f, 276f + idx * 20f, 280f, 2f);
				UIProgressView progressView = new UIProgressView(frame);
				Add(progressView);
				// Keep track of the UIProgressViews so we can remove them later.
				progressViews.Add(progressView);


				// Create an IProgress<T> for each file that is to be downloaded. This will update the 
				// UIProgressView that we created to display the download progress.
				Progress<DownloadBytesProgress> progressReporter = new Progress<DownloadBytesProgress>();
				progressReporter.ProgressChanged += (s, e) => 
				{
					progressView.Progress = e.PercentComplete;
				};

				// Create the task that will do the work.
				Task<int> task = GetBytes(ListOfImages[idx], cancellationTokenSource.Token, progressReporter);
				tasks.Add(task);
			}

			return tasks;
		}
	}
}

