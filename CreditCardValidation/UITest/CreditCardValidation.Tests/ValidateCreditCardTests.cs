using System;
using System.IO;
using System.Linq;
using System.Reflection;

using NUnit.Framework;

using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace CreditCardValidation.Tests
{
    [TestFixture]
    public class ValidateCreditCardTests
    {
        IScreenQueries _queries;
        IApp _app;

        public string PathToIPA { get; private set; }

        public string PathToAPK { get; private set; }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            if (TestEnvironment.IsTestCloud)
            {
                PathToAPK = String.Empty;
                PathToIPA = String.Empty;
            }
            else
            {
                CheckAndroidHomeEnvironmentVariable(); 

                string currentFile = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
                FileInfo fi = new FileInfo(currentFile);
                string dir = fi.Directory.Parent.Parent.Parent.FullName;

                PathToIPA = Path.Combine(dir, "CreditCardValidation.iOS", "bin", "iPhoneSimulator", "Debug", "CreditCardValidationiOS.app");
                PathToAPK = Path.Combine(dir, "CreditCardValidation.Droid", "bin", "Release", "CreditCardValidation.Droid.APK");
            }
        }

        [SetUp]
        public void SetUp()
        {
            if (TestEnvironment.Platform.Equals(TestPlatform.TestCloudiOS))
            {
                _app = ConfigureApp
                    .iOS
                    .StartApp();
                _queries = new iOSQueries();
            }
            else if (TestEnvironment.Platform.Equals(TestPlatform.TestCloudAndroid))
            {
                _queries = new AndroidQueries();
                _app = ConfigureApp
                    .Android
                    .StartApp();
            }
            else if (TestEnvironment.Platform.Equals(TestPlatform.Local))
            {
//                _app = ConfigureApp
//                                                    .iOS
//                                                    .AppBundle(PathToIPA)
//                                                    .StartApp();
//                _queries = new iOSQueries();

                _app = ConfigureApp
                    .Android
                    .ApkFile(PathToAPK)
                    .StartApp();
                _queries = new AndroidQueries();
            }
            else
            {
                throw new NotImplementedException(String.Format("I don't know this platform {0}", TestEnvironment.Platform));
            }
        }

        [Test]
        public void CreditCardNumber_CorrectSize_DisplaySuccessScreen()
        {
            // Arrange - set up our queries for the views
            // Nothing to do here - the queries are already defined.

            /* Act */
            _app.EnterText(_queries.CreditCardNumberView, new string('9', 16));
            _app.Screenshot("Success");
            _app.Tap(_queries.ValidateButtonView);

            /* Assert */
            _app.WaitForElement(_queries.SuccessScreenNavBar, "Valid Credit Card screen did not appear", TimeSpan.FromSeconds(5));
            AppResult[] results = _app.Query(_queries.SuccessMessageView);
            Assert.IsTrue(results.Any(), "The success message was not displayed on the screen");
        }

        [Test]
        public void CreditCardNumber_TooLong_DisplayErrorMessage()
        {
            /* Arrange - set up our queries for the views */
            // Nothing to do here - the queries are already defined.

            /* Act */
            _app.EnterText(_queries.CreditCardNumberView, new string('9', 17));
            _app.Tap(_queries.ValidateButtonView);

            /* Assert */
            AppResult[] result = _app.Query(_queries.LongCreditCardNumberView);
            Assert.IsTrue(result.Any(), "The error message is not being displayed.");
        }

        [Test]
        public void CreditCardNumber_TooShort_DisplayErrorMessage()
        {
            /* Arrange - set up our queries for the views */
            // Nothing to do here - the queries are already defined.

            /* Act */
            _app.EnterText(_queries.CreditCardNumberView, new string('9', 15));
            _app.Tap(_queries.ValidateButtonView);

            /* Assert */
            AppResult[] result = _app.Query(_queries.ShortCreditCardNumberView);
            Assert.IsTrue(result.Any(), "The error message is not being displayed.");
        }

        void CheckAndroidHomeEnvironmentVariable()
        {
            var androidHome = Environment.GetEnvironmentVariable("ANDROID_HOME");
            if (string.IsNullOrWhiteSpace(androidHome))
            {
                Environment.SetEnvironmentVariable("ANDROID_HOME", "~/android-sdk-macosx");
            }
        }
    }
}
