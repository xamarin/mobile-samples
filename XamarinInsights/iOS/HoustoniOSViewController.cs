﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using Xamarin;

namespace HoustoniOS
{
    public partial class HoustoniOSViewController : UIViewController
    {
        // accessor to the event routine used for the timers
        public WorkCompletedEvent WorkCompleted { get; set; }

        public HoustoniOSViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();
			
            // Release any cached data, images, etc that aren't in use.
        }

        #region View lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			
            // by default, buttons after iOS 6 are borderless and don't really stand out
            // this little piece of code colors the background and gives the button a border
            foreach (var btn in View.Subviews)
            {
                if (btn is UIButton)
                {
                    var dupBtn = btn as UIButton;
                    dupBtn.VisibleButton();
                }
            }

            // disable all buttons until either Single or Multiple has been selected
            btnCrashApp.Enabled = btnDivByZero.Enabled = btnFileException.Enabled = btnNullRef.Enabled = btnPartialInfo.Enabled = btnStartMultiTracker.Enabled = btnStartTimer.Enabled = false;

            // call the main application
            RunInsights();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
        }

        #endregion

        private void RunInsights()
        {
            // let's identify the user of this software. This can be a single person, or a group of people (perhaps from a group subscription to an app)

            // 1. For a single user
            // The format is unique identifier, key, value

            btnSingle.TouchUpInside += delegate
            {
                Insights.Identify("alan_user@xamarin.com", "Name", "Alan James User");
                btnCrashApp.Enabled = btnDivByZero.Enabled = btnFileException.Enabled = btnNullRef.Enabled = btnPartialInfo.Enabled = btnStartMultiTracker.Enabled = btnStartTimer.Enabled = true;
                btnMultipleId.Enabled = false;
            };

            // 2. For a group of users, a dictionary is used, this should be provided after the unique ID

            btnMultipleId.TouchUpInside += delegate
            {
                var extraInformation = new Dictionary<string,string>
                {
                    { "Email","alan_user@xamarin.com" },
                    { "Name", "Alan James User" },
                };
                Insights.Identify("UniqueUserId", extraInformation);
                btnCrashApp.Enabled = btnDivByZero.Enabled = btnFileException.Enabled = btnNullRef.Enabled = btnPartialInfo.Enabled = btnStartMultiTracker.Enabled = btnStartTimer.Enabled = true;
                btnSingle.Enabled = false;
            };

            // to make the Insights reporting tool report, an exception has to be thrown
            // the reports can be sent via one of three ways
            // the simplest is to just send back the exception

            btnDivByZero.TouchUpInside += delegate
            {
                try
                {
                    int divByZero = 42 / int.Parse("0");
                }
                catch (DivideByZeroException ex)
                {
                    Insights.Report();
                }
            };


            // the next is to send specific information. This can achieved using a Dictionary<string,string>()
            // and may be constructed as part of the report or outside of it

            // 1. as part of the exception

            btnFileException.TouchUpInside += delegate
            {
                try
                {
                    using (var text = File.OpenText("some_file.tardis"))
                    {
                        Console.WriteLine("{0}", text.ReadLine());
                    }
                }
                catch (FileNotFoundException ex)
                {
                    Insights.Report(ex, new Dictionary<string,string>
                        {
                            { "File missing", "some_file.tardis" },
                            { "Source file","MainActivity.cs" },
                            { "Method name", "protected override void OnCreate(Bundle bundle)" }
                        });
                }
            };

            // 2. outside the exception. Essentially, this is the same as the 1st type, but there is additional flexibility
            // Here it calls CreateDictionary which takes a T exception and produces a new dictionary from it
            // This is a very trivial example - data from Reflection would probably be of more use

            btnNullRef.TouchUpInside += delegate
            {
                try
                {
                    List<string> myList = null;
                    myList.Add("Hello");
                }
                catch (NullReferenceException ex)
                {
                    var report = CreateDictionary(ex);
                    Insights.Report(ex, report);
                }
            };

            // 3. Instead of sending over the full exception, just send a piece over

            btnPartialInfo.TouchUpInside += delegate
            {
                try
                {
                    var block = new byte[]{ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                    using (var text = File.OpenWrite(@"/bin/hello.txt"))
                    {
                        text.Write(block, 0, 10);
                    }
                }
				catch (UnauthorizedAccessException ua) 
				{
					ua.Data["MoreData"] = "You can't write to the bin directory!";
					// send the report
					Insights.Report(ua);
					// throw the exception - this exception would need to be caught using another try/catch
					throw ua;
				}
				catch (IOException ex)
                {
                    // see http://msdn.microsoft.com/en-us/library/system.exception.data%28v=vs.110%29.aspx for more details on this
                    ex.Data["MoreData"] = "You can't write to the bin directory!";
                    // send the report
                    Insights.Report(ex);
                    // throw the exception - this exception would need to be caught using another try/catch
                    throw ex;
                }
            };

            // catching an uncaught exception
            btnCrashApp.TouchUpInside += delegate
            {
                var block = new byte[]{ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                block[11] = 10;
            };


            // An important aspect of any form of trace software is the ability to track an event. In the following example.
            // a simple reaction time will be measured.

            btnStartTimer.TouchUpInside += delegate
            {
                btnStartTimer.SetTitle("Stop the clock", UIControlState.Normal);
                var timer = Stopwatch.StartNew(); // from System.Diagnostics
                btnStartTimer.TouchUpInside += delegate
                {
                    using (var react = Insights.TrackTime("reactionTime"))
                    {
                        btnStartTimer.TouchUpInside += async delegate
                        {
                            timer.Stop();
                            var timeSpan = timer.Elapsed;
                            await StoreReactionTime(DateTime.Now.Subtract(timeSpan).Second);
                        };
                    }
                };
            };

            // it is also possible to track a specific thread. There are two ways to do this - with and without additional parameters

            btnStartMultiTracker.TouchUpInside += delegate
            {
                Insights.Track("mySpecificProcess");
                // In a similar way to using Debug.WriteLine, a dictionary can be used to track 
                Insights.Track("setUpForEach", new Dictionary<string,string>{ { "process1","create list" }, { "process2","populate list" } });
                var myList = new List<string>();
                myList.AddRange(new string[]{ "iOS", "Android", "Symbian", "Windows Mobile", "Blackberry" });

                // The next part is to do some work on the list. This will be a mix of TrackTimer and Track
                Insights.Track("doSomeWorkOnList", new Dictionary<string, string>{ { "process3", "encrypt and time data" } });
                var timer = Stopwatch.StartNew();
                using (var handle = Insights.TrackTime("encrypter"))
                {
                    EncryptData(myList);

                    if (WorkCompleted == null)
                        return;

                    WorkCompleted.Change += async delegate(object s, WorkCompletedEventArgs ea)
                    {
                        if (ea.ModuleName == "Encryption")
                        {
                            timer.Stop();
                            var timeSpan = timer.Elapsed;
                            await StoreReactionTime(DateTime.Now.Subtract(timeSpan).Second);
                        }
                    };
                }
                // tell the tracker we're done
                Insights.Track("mySpecificProcess", new Dictionary<string,string>{ { "process4", "processing completed" } });
            };
        }

        private Dictionary<string,string> CreateDictionary<T>(T ex) where T : Exception
        {
            // this is an example of creating the dictionary based on a generic exception
            var myReport = new Dictionary<string,string> { { "Stack trace", ex.StackTrace }, { "Report", DateTime.Now.ToShortDateString() }, { "Message", ex.Message } };
            return myReport;
        }

        private void EncryptData(List<string> data)
        {
            // a very simple MD5 encryption of the data we've fed in. It shouldn't take any time at all really

            var encrypted = new List<string>();
            foreach (var d in data)
                using (var md5Hash = MD5.Create())
                    encrypted.Add(GetMd5Hash(md5Hash, d));

            // broadcast the event for tracking
            WorkCompleted?.BroadcastIt("Encryption");
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {
            var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));
            return sBuilder.ToString();
        }

        private async Task<bool> StoreReactionTime(int time)
        {
            var file = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "testfile.txt");

            using (var output = File.CreateText(file))
            {
                await output.WriteLineAsync(string.Format("Time taken = {0} seconds", time));
                Insights.Track("reactionTime", new Dictionary<string,string>{ { "Time taken", time.ToString() } });
            }

            return true;
        }
    }

    public static class UIPrettifier
    {
        public static UIButton VisibleButton(this UIButton theButton)
        {
            theButton.Layer.BackgroundColor = UIColor.White.CGColor;
            theButton.Layer.CornerRadius = 5f;
            theButton.Layer.BorderColor = UIColor.Cyan.CGColor;
            theButton.Layer.BorderWidth = 1f;
            return theButton;
        }
    }
}

