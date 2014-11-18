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
                // Uncomment this if Xamarin.UITest cannot locate the Android SDK.
                // Check out the function and update it with the correct path
                // to your Android SDK.
                // CheckAndroidHomeEnvironmentVariable();

                // If you don't provide the API key, then the tests can only be run in the iOS simulator.
                _app = ConfigureApp.iOS
                                   .ApiKey("")
                                   .AppBundle(PathToIPA)
                                   .StartApp();
                _queries = new iOSQueries();

                //                _app = ConfigureApp
                //                    .Android
                //                    .ApkFile(PathToAPK)
                //                    .ApiKey("")
                //                    .StartApp();
                //                _queries = new AndroidQueries();
            }
            else
            {
                throw new NotImplementedException(String.Format("I don't know this platform {0}", TestEnvironment.Platform));
            }
        }

        /// <summary>
        /// This holds the AppQueries that will be used in the test.
        /// </summary>
        IScreenQueries _queries;

        IApp _app;

        public string PathToIPA { get; private set; }

        public string PathToAPK { get; private set; }

        /// <summary>
        /// Before each test is run we calculate the path to the AppBundle and 
        /// the APK.
        /// </summary>
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
                string currentFile = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
                FileInfo fi = new FileInfo(currentFile);
                string dir = fi.Directory.Parent.Parent.Parent.FullName;

                PathToIPA = Path.Combine(dir, "CreditCardValidation.iOS", "bin", "iPhoneSimulator", "Debug", "CreditCardValidationiOS.app");
                PathToAPK = Path.Combine(dir, "CreditCardValidation.Droid", "bin", "Release", "CreditCardValidation.Droid.APK");
            }
        }

        /// <summary>
        ///   This method checks to make sure that UITest can find the Android SDK if it is not in
        ///   a standard location. If you get a message from UITest that it cannot locae the Android
        ///   SDK
        /// </summary>
        void CheckAndroidHomeEnvironmentVariable()
        {
            string androidHome = Environment.GetEnvironmentVariable("ANDROID_HOME");
            if (string.IsNullOrWhiteSpace(androidHome))
            {
                Environment.SetEnvironmentVariable("ANDROID_HOME", "~/android-sdk-macosx");
            }
        }

        [Test]
        public void CreditCardNumber_CorrectSize_DisplaySuccessScreen()
        {
            // Arrange - set up our queries for the views
            // Nothing to do here - the queries are already defined.

            /* Act */
            _app.EnterText(_queries.CreditCardNumberView, new string('9', 16));
            _app.Screenshot("Credit Card Number is correct length.");
            _app.Tap(_queries.ValidateButtonView);

            /* Assert */
            _app.WaitForElement(_queries.SuccessScreenNavBar, "Valid Credit Card screen did not appear", TimeSpan.FromSeconds(5));
            _app.Screenshot("Success screen for credit card number.");

            AppResult[] results = _app.Query(_queries.SuccessMessageView);
            Assert.IsTrue(results.Any(), "The success message was not displayed on the screen");
        }

        [Test]
        public void CreditCardNumber_IsBlank_DisplayErrorMessage()
        {
            // Arrange - set up our queries for the views
            // Nothing to do here - the queries are already defined.

            /* Act */
            _app.EnterText(_queries.CreditCardNumberView, String.Empty);
            _app.Screenshot("Credit Card Number missing");
            _app.Tap(_queries.ValidateButtonView);

            /* Assert */
            AppResult[] result = _app.Query(_queries.LongCreditCardNumberView);
            _app.Screenshot("Error message for long credit card nuymber.");
            Assert.IsTrue(result.Any(), "The error message is not being displayed.");

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
            _app.Screenshot("Credit Card Number is too long.");
            _app.Tap(_queries.ValidateButtonView);

            /* Assert */
            AppResult[] result = _app.Query(_queries.LongCreditCardNumberView);
            _app.Screenshot("Error message for long credit card nuymber.");
            Assert.IsTrue(result.Any(), "The error message is not being displayed.");
        }

        [Test]
        public void CreditCardNumber_TooShort_DisplayErrorMessage()
        {
            /* Arrange - set up our queries for the views */
            // Nothing to do here - the queries are already defined.

            /* Act */
            _app.EnterText(_queries.CreditCardNumberView, new string('9', 15));
            _app.Screenshot("Credit Card Number is too short.");
            _app.Tap(_queries.ValidateButtonView);

            /* Assert */
            AppResult[] result = _app.Query(_queries.ShortCreditCardNumberView);
            _app.Screenshot("Error message for short credit card number.");
            Assert.IsTrue(result.Any(), "The error message is not being displayed.");
        }
    }
}
