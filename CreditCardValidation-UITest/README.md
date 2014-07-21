CreditCardValidation-UITest
---------------------------

This sample is the companion code for the [Introduction to Xamarin.UITest](http://staging-docs.xamarin.com/guides/testcloud/uitest/intro-to-uitest/) guide. It has some simple examples of tests written using Xamarin.UITest and includes a shell script that automates compiling the application and submitting it to Xamarin Test Cloud.

Create a test run at [Xamarin Test Cloud](http://testcloud.xamarin.com) and make a note of your API key. Then create a copy of the file `testcloud-submit-example.sh`, (i.e. `testcloud-submit.sh`). Update your copy of the file with your Test Cloud API key. You may optionally update the device ID's as well.

Once you have done that, you may run `./testcloud-submit.sh` and it will compile the Android and iOS applications and submit them to Test Cloud for you.