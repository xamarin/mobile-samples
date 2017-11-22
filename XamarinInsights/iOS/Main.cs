using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace HoustoniOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
			// register with Insights
			Xamarin.Insights.Initialize(Constants.InsightsApiKey);

            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}
