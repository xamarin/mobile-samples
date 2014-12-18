using System;
using System.Linq;

using NUnit.Framework;

using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace CreditCardValidation.Tests
{
    [TestFixture]
    public class ValidateCreditCardTests
    {
        /// <summary>
        ///   In some cases UITest will not be able to resolve the path to the Android SDK.
        ///   Set this to your local Android SDK path.
        /// </summary>
        public static readonly string PathToAndroidSdk = "/Users/tom/android-sdk-macosx";
        IApp _app;
        /// <summary>
        ///   This holds the AppQueries that will be used in the test.
        /// </summary>
        IScreenQueries _queries;
        public string PathToIPA { get; private set; }
        public string PathToAPK { get; private set; }

        /// <summary>
        ///   Before each test is run we calculate the path to the AppBundle and
        ///   the APK.
        /// </summary>
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            PathToIPA = "../../../CreditCardValidation.iOS/bin/iPhoneSimulator/Debug/CreditCardValidationiOS-1.0.ipa";
            PathToAPK = "../../../CreditCardValidation.Droid/bin/Debug/CreditCardValidation.Droid.APK";
        }

        [TestCase(Platform.iOS)]
        [TestCase(Platform.Android)]
        public void CreditCardNumber_CorrectSize_DisplaySuccessScreen(Platform platform)
        {
            // Arrange - set up our queries for the views
            ConfigureTest(platform);

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

        [TestCase(Platform.iOS)]
        [TestCase(Platform.Android)]
        public void CreditCardNumber_IsBlank_DisplayErrorMessage(Platform platform)
        {
            // Arrange - set up our queries for the views
            ConfigureTest(platform);

            /* Act */
            _app.EnterText(_queries.CreditCardNumberView, String.Empty);
            _app.Screenshot("Credit Card Number missing");
            _app.Tap(_queries.ValidateButtonView);

            /* Assert */
            AppResult[] result = _app.Query(_queries.MissingCreditCardNumberView);
            _app.Screenshot("Error message for a missing credit card number.");
            Assert.IsTrue(result.Any(), "The 'missing credit card' error message is not displayed.");
        }

        [TestCase(Platform.iOS)]
        [TestCase(Platform.Android)]
        public void CreditCardNumber_TooLong_DisplayErrorMessage(Platform platform)
        {
            // Arrange - set up our queries for the views
            ConfigureTest(platform);

            /* Act */
            _app.EnterText(_queries.CreditCardNumberView, new string('9', 17));
            _app.Screenshot("Credit Card Number is too long.");
            _app.Tap(_queries.ValidateButtonView);

            /* Assert */
            AppResult[] result = _app.Query(_queries.LongCreditCardNumberView);
            _app.Screenshot("Error message for long credit card nuymber.");
            Assert.IsTrue(result.Any(), "The 'long credit card' error message is not being displayed.");
        }

        [TestCase(Platform.iOS)]
        [TestCase(Platform.Android)]
        public void CreditCardNumber_TooShort_DisplayErrorMessage(Platform platform)
        {
            // Arrange - set up our queries for the views
            ConfigureTest(platform);

            /* Act */
            _app.EnterText(_queries.CreditCardNumberView, new string('9', 15));
            _app.Screenshot("Credit Card Number is too short.");
            _app.Tap(_queries.ValidateButtonView);

            /* Assert */
            AppResult[] result = _app.Query(_queries.ShortCreditCardNumberView);
            _app.Screenshot("Error message for short credit card number.");
            Assert.IsTrue(result.Any(), "The 'short credit card' error message is not being displayed.");
        }

        /// <summary>
        ///   This method checks to make sure that UITest can find the Android SDK if it is not in
        ///   a standard location.
        /// </summary>
        /// <remarks>
        ///   This method is only used if the PathToAndroidSDK is set.
        /// </remarks>
        void CheckAndroidHomeEnvironmentVariable()
        {
            if (string.IsNullOrWhiteSpace(PathToAndroidSdk))
            {
                return;
            }
            string androidHome = Environment.GetEnvironmentVariable("ANDROID_HOME");
            if (string.IsNullOrWhiteSpace(androidHome))
            {
                Environment.SetEnvironmentVariable("ANDROID_HOME", PathToAndroidSdk);
            }
        }

        /// <summary>
        ///   This will initialize the IApp to the Android application.
        /// </summary>
        void ConfigureAndroidApp()
        {
            // If there is a problem finding the Android SDK, this method can help.
            // CheckAndroidHomeEnvironmentVariable();

            if (TestEnvironment.Platform.Equals(TestPlatform.Local))
            {
                _app = ConfigureApp.Android
                                   .ApkFile(PathToAPK)
                                   .EnableLocalScreenshots()
                                   .StartApp();
            }
            else
            {
                _app = ConfigureApp
                    .Android
                    .StartApp();
            }
        }

        /// <summary>
        ///   This will initialize IApp to the iOS application.
        /// </summary>
        void ConfigureiOSApp()
        {
            if (TestEnvironment.Platform.Equals(TestPlatform.Local))
            {
                _app = ConfigureApp.iOS
                                   .EnableLocalScreenshots()
                                   .AppBundle(PathToIPA)
                                   .StartApp();
            }
            else
            {
                _app = ConfigureApp
                    .iOS
                    .StartApp();
            }
        }

        void ConfigureTest(Platform platform)
        {
            switch (platform)
            {
                case Platform.Android:
                    _queries = new AndroidQueries();
                    ConfigureAndroidApp();
                    break;
                case Platform.iOS:
                    _queries = new iOSQueries();
                    ConfigureiOSApp();
                    break;
            }
        }
    }
}
