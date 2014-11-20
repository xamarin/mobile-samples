CreditCardValidation-UITest
============================

This sample is the companion code for the [Introduction to Xamarin.UITest](http://developer.xamarin.com/guides/testcloud/uitest/intro-to-uitest/) and the [Submitting Tests to Xamarin Test Cloud](http://developer.xamarin.com/guides/testcloud/submitting-to-testcoud) guides. It has some simple examples of tests written using Xamarin.UITest and includes a shell script that automates compiling the application and submitting it to Xamarin Test Cloud.

To get started with this project, it is necessary to log in to test cloud, create a new *test run*, receive your Xamarin Test Cloud API key, and the *device IDs* for the Android and iOS devices you would like to test on. The details on how to do this are discussed in the [Submitting Tests to Tesst Cloud](http://developer.xamarin.com/guides/testcloud/submitting/) guide.

Once you have these values, you can update one of the build scripts (described below) and upload the application and test to Test Cloud at the command line.


## Build Scripts

Creating a build script is a great way to automate and simplify the compilation of the mobile applications and submitting them to Xamarin Test Cloud. This project provides two possible build scripts. One is a bash script, the other is a Rakefile.


### Using the bash Build Script

To build the project and submit to Test Cloud using Bash, edit the file **build.sh** and add the XTC_API_KEY, Android Device ID, and the iOS Device ID where applicable. Then just run the bash script from Terminal:
    
    $ ./build.sh


### Using the Rakefile    

[Rake](https://rubygems.org/gems/rake) is another great tool for building applications. The include `Rakefile` has the following tasks:

	$ rake -T
	rake build  # Compiles the Android and iOS projects
	rake clean  # Cleans the project, removing any artifacts from previous builds
	rake xtc    # Builds the application and then submits the APK and IPA to Xamarin Test Cloud

## Generating the Signing Information File

There are several options available for [signing the Android APK](http://developer.xamarin.com/guides/testcloud/submitting/#Signing_Android_APKs) prior to submitting it to Xamarin Test Cloud. One such option is to use a *signing information file*. This project includes a signing information file and a sample keystore.

The name of the keystore file is `creditcardvalidation-example.keystore` and it has a single key in it called `uitest_sample`. The password for the keystore file is *password1*, while the password for the key is *password2*.

### How the Keystore File was Created

To illustrate how to create the signing information file, the sample keystore has been provided with the following information:

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


**Note:** In a production environment the keystore file should not be included in source code control. It contains confidential information that is used to sign your application and should be protected. The keystore is only included in this example for demonstration purposes.

The bash script `testcloud-submit-example.sh` demonstrates how to compile the APK, sign it, generate a *signing information* file, and then submit the APK using that signing information file. The signing information file, `testserver.si`, is also included in this sample. It is save to include signing information file in source code control.

The signing information file was created with the following command line (the environment variables were initialized in the bash script):
 
    mono text-cloud.exe gen-sign-info $APK_FINAL ./creditcardvalidation-example.keystore $KEYSTORE_PASS uitest_sample $ALIAS_PASS --dir ./

