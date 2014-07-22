Tasky Pro (Calabash)
====================

This project is an example of how to create cross platform function tests for [Xamarin Test Cloud](http://testcloud.xamarin.com) using the [Calabash framework](http://developer.xamarin.com/guides/testcloud/calabash).You will need have a Test Cloud account and a Test Cloud API key.

This project contains a Rakefile with tasks to perform much of compiling the application and submitting to test cloud. 


Setting up the Build Environment
--------------------------------

It is necessary to have a modern version of Ruby and Rake installed. Ensure that you have up to date gemsets by running `bundle update`. You will also have to create a Test Cloud project for the iOS and Android project 

There are some variable unique to each Test Cloud project, most notably the *API key* and the *device id*. These values can be provided to the rakefile via environment variables or in a text file called `rake_env`. This file has the following format:

    set(:testcloud_api_key, "YOUR_TESTCLOUD_API_KEY_HERE")
    set(:android_device_id, "YOUR_ANDROID_DEVICE_KEY_HERE")
    set(:ios_device_id, "YOUR_IOS_DEVICE_ID")

Building the Applications
-------------------------

Regardless of how you would like to test your application it is necessary to compile the Android or iOS project. The provided `rakefile` will take care of compiling the mobile application for you:

    rake build
    
This will compile the following:

* `./Tasky.Droid/bin/Release/com.xamarin.samples.taskydroid-Signed.apk` - a Release APK for Android. This can be used for either local testing or it can be submitted to Xamarin Test Cloud.

* `Tasky.iOS/bin/iPhone/Debug/TaskyiOS-1.ipa` - this is an ad-hoc IPA for testing.


Running the Testing
-------------------

There are two ways to test the Android application:

  * **Locally** - on an attached device or running emulator. Run `rake test_android`.
  * **Test Cloud** - enqueue the application to Test Cloud. Make sure that you have a `rake_env` text file setup and then run `rake testcloud_android`.
  
To test the iOS application:

  * **Locally** - `cucumber -p ios` - will run the tests in the iOS simulator.
  * **Test Cloud** - `rake testcloud_ios` - will compile the app and upload it to Test Cloud.

Authors
-------

Bryan Costanich, Craig Dunn, Tom Opgenorth
