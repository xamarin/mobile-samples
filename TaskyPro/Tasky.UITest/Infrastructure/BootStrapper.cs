using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using Ninject;
using Ninject.Modules;
using Xamarin.UITest;
using Ninject.Extensions.Factory;
using Ninject.Activation;

namespace Tasky.UITest
{

    public static class BootStrapper
    {
        public static readonly string DefaultAndroidHome = "/Users/tom/android-sdk-macosx";
        public static StandardKernel Container { get; set; }

        public static void Initialize()
        {
            CheckEnvironmentVariables();
            bool isTestCloud = !TestEnvironment.Platform.Equals(TestPlatform.Local);
            NinjectModule module = new UITestModule(isTestCloud);
            Container = new StandardKernel(module);
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
