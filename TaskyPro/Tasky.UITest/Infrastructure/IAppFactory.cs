using System;
using System.IO;
using System.Reflection;
using Xamarin.UITest;

namespace Tasky.UITest
{
    public interface IAppFactory
    {
        IApp CreateApp();
    }

    public class AppFactory: IAppFactory
    {
        public  AppFactory()
        {
            string currentFile = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
            FileInfo fi = new FileInfo(currentFile);
            string dir = fi.Directory.Parent.Parent.Parent.FullName;
            PathToAPK = Path.Combine(dir, "Tasky.Droid", "bin", "Release", "com.xamarin.samples.taskydroid-Signed.apk");
            PathToIPA = Path.Combine(dir, "Tasky.iOS", "bin", "iPhoneSimulator", "Debug", "CreditCardValidationiOS.app");
        }

        public IApp CreateApp()
        {
            IApp _app;
            if (TestEnvironment.Platform.Equals(TestPlatform.TestCloudiOS))
            {
                _app = ConfigureApp.iOS.StartApp();
            }
            else if (TestEnvironment.Platform.Equals(TestPlatform.TestCloudAndroid))
            {
                _app = ConfigureApp.Android.StartApp();
            }
            else
            {
                _app = ConfigureApp.Android.ApkFile(PathToAPK).StartApp();;  
            }
            return _app;
        }

        public string PathToAPK { get; set; }

        public string PathToIPA { get; set; }

    }




}
