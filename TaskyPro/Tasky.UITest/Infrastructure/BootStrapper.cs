using System;

namespace Tasky.UITest
{
    public static class BootStrapper
    {
        public static readonly string DefaultAndroidHome = "/Users/tom/android-sdk-macosx";

        public static IAppFactory AppFactory { get; private set; }

        public static void Initialize()
        {
            CheckEnvironmentVariables();
            AppFactory = new AppFactory();
        }

        static void CheckEnvironmentVariables()
        {
            var androidHome = Environment.GetEnvironmentVariable("ANDROID_HOME");

            if (string.IsNullOrWhiteSpace(androidHome))
            {
                Environment.SetEnvironmentVariable("ANDROID_HOME", DefaultAndroidHome);
                Console.WriteLine("Explicitly setting $ANDROID_HOME to {0}", DefaultAndroidHome);
            }
        }
    }
}
