using System;
using System.IO;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Configuration;

namespace CreditCardValidation.Tests
{

    public static class TestHelpers
    {
        public static string GetPathToAPK(this object obj)
        {
            string currentFile = new Uri(obj.GetType().Assembly.CodeBase).LocalPath;
            FileInfo fi = new FileInfo(currentFile);
            string dir = fi.Directory.Parent.Parent.Parent.FullName;

            string pathToAPK = Path.Combine(dir, "CreditCardValidation.Droid", "bin", "Release", "CreditCardValidation.Droid.APK");
            return pathToAPK;
        }

        public static string GetPathToIPA(this object obj)
        {
            string currentFile = new Uri(obj.GetType().Assembly.CodeBase).LocalPath;
            FileInfo fi = new FileInfo(currentFile);
            string dir = fi.Directory.Parent.Parent.Parent.FullName;

            string pathToIPA = Path.Combine(dir, "CreditCardValidation.iOS", "bin", "iPhoneSimulator", "Debug", "CreditCardValidationiOS.app");
            return pathToIPA;
        }

        public static iOSAppConfigurator ConfigureiOSApp(this object obj, string apiKey, string simulatorName)
        {
            var deviceId = GetDeviceID(simulatorName);
            var pathToBundle = obj.GetPathToIPA();
            if (!File.Exists(pathToBundle))
            {
                throw new FileNotFoundException("Cannot file the app bundle " + pathToBundle + ".", pathToBundle);
            }
            iOSAppConfigurator appConfig = ConfigureApp.iOS.DeviceIdentifier(deviceId).ApiKey(apiKey).AppBundle(pathToBundle);
            return appConfig;
        }

        public static AndroidAppConfigurator ConfigureAndroidApp(this object obj, string apiKey)
        {
            var pathToAPK = obj.GetPathToAPK();
            if (!File.Exists(pathToAPK))
            {
                throw new FileNotFoundException("Cannot file the APK " + pathToAPK + ". Make sure that you compile the application.", pathToAPK);
            }
            AndroidAppConfigurator appConfig = ConfigureApp.Android.ApkFile(pathToAPK);
            return appConfig;
        }

        public static string GetDeviceID( string simulatorName)
        {
            if (!TestEnvironment.Platform.Equals(TestPlatform.Local))
            {
                return string.Empty;
            }

            var results = new InstrumentsRunner().GetListOfSimulators();

            var simulator = (from sim in results
                                      where sim.Name.Equals(simulatorName)
                                      select sim).FirstOrDefault();

            if (simulator == null)
            {
                throw new ArgumentException("Could not find a device identifier for '" + simulatorName + "'.", "simulatorName");
            }
            else
            {
                return simulator.GUID;
            }
        }
    }
}

