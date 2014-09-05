CreditCardValidation-UITest
============================

This sample is the companion code for the [Introduction to Xamarin.UITest](http://developer.xamarin.com/guides/testcloud/uitest/intro-to-uitest/) and the [Submitting Tests to Xamarin Test Cloud](http://developer.xamarin.com/guides/testcloud/submitting-to-testcoud) guides. It has some simple examples of tests written using Xamarin.UITest and includes a shell script that automates compiling the application and submitting it to Xamarin Test Cloud.

Create a test run at [Xamarin Test Cloud](http://testcloud.xamarin.com) and make a note of your API key. Then create a copy of the file `testcloud-submit-example.sh`, (i.e. `testcloud-submit.sh`). Update your copy of the file with your Test Cloud API key. You may optionally update the device ID's as well.

Once you have done that, you may run `./testcloud-submit.sh` and it will compile the Android and iOS applications and submit them to Test Cloud for you.

Generating the Signing Information File
---------------------------------------

To illustrate how to create the Signing Information File, a sample keystore has been provided with the following information:

	$ keytool -genkey -v -keystore creditcardvalidation-example.keystore -alias uitest_sample -keyalg RSA -keysize 2048 -validity 10000
	Enter keystore password:  
	Re-enter new password: 
	What is your first and last name?
	  [Unknown]:  Albert the Second
	What is the name of your organizational unit?
	  [Unknown]:  Documentation
	What is the name of your organization?
	  [Unknown]:  Xamarin
	What is the name of your City or Locality?
	  [Unknown]:  San Francisco
	What is the name of your State or Province?
	  [Unknown]:  CA
	What is the two-letter country code for this unit?
	  [Unknown]:  US
	Is CN=Albert the Second, OU=Documentation, O=Xamarin, L=San Francisco, ST=CA, C=US correct?
	  [no]:  yes

The name of the keystore file is `creditcardvalidation-example.keystore` and it has a single key in it called `uitest_sample`. The password for the keystore file is password1, while the password for the key is password2.

**Note:** The keystore file should not be included in source code control. It contains confidential information that is used to sign your application and should be protected. The keystore is only included in this example for demonstration purposes.

The bash script `testcloud-submit-example.sh` demonstrates how to compile the APK, sign it, generate a *signing information* file, and then submit the APK using that signing information file. The signing information file, `testserver.si`, is also included in this sample. It is save to include signing information file in source code control.

The signing information file was created with the following command line (the environment variables were initialized in the bash script):
 
    mono xut-console.exe gen-sign-info $APK_FINAL ./creditcardvalidation-example.keystore $KEYSTORE_PASS uitest_sample $ALIAS_PASS --dir ./

