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
    public interface IAppFactory 
    {
        IApp CreateApp();
    }
 
    public class AppFactory: Provider<IApp> 
    {
        public  AppFactory()
        {
            string currentFile = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
            FileInfo fi = new FileInfo(currentFile);
            string dir = fi.Directory.Parent.Parent.Parent.FullName;
            PathToAPK = Path.Combine(dir, "Tasky.Droid", "bin", "Release", "com.xamarin.samples.taskydroid-Signed.apk");
            PathToIPA = Path.Combine(dir, "Tasky.iOS", "bin", "iPhoneSimulator", "Debug", "CreditCardValidationiOS.app");
        }

        protected override IApp CreateInstance(IContext context)
        {
            return CreateApp();
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
                _app = ConfigureApp.Android.ApkFile(PathToAPK).StartApp();  
            }
            return _app;
        }

        public string PathToAPK { get; set; }

        public string PathToIPA { get; set; }

    }


    /// <summary>
    /// Respondsible for 
    /// </summary>
    public class UITestModule: NinjectModule
    {
        public UITestModule(bool isTestCloud)
        {
            IsTestCloud = isTestCloud;
        }

        public override void Load()
        {
            Bind<IApp>().ToProvider(new AppFactory());
        }

        protected bool IsTestCloud { get; private set; }
    }


}
