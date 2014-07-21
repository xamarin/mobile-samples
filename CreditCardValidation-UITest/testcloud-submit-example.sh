#!/bin/sh
# This is a simple bash script that will compile the APK and IPA and then
# enqueue a test run in Xamarin Test Cloud. 

# API key is a sensitive value and should be protected.
export TESTCLOUD_API_KEY=<YOUR API KEY HERE>

# The Device ID's are not so sensitive - they just tell Test Cloud what devices to 
# run the test on.
export DEVICE_ID_IOS=65195772
export DEVICE_ID_ANDROID=f2d04717

# This will have to be updated when xut-console is updated.
export XUTCONSOLE=./packages/Xamarin.UITest.Console.0.4.3/tools/xut-console.exe

# You shouldn't have to update these variables.
export TEST_ASSEMBLIES=./CreditCardValidation.Tests/bin/Debug/
export IPA=./CreditCardValidation.iOS/bin/iPhone/Debug/CreditCardvalidationiOS-1.0.ipa
export APK=./CreditCardValidation.Droid/bin/Release/CreditCardValidation.Droid.apk

# Uploading the dSYM files is optional - but it can help with troubleshooting if necessary.
export DSYM=./CreditCardValidation.iOS/bin/iPhone/Debug/CreditCardValidationiOS.app.dSYM

# iOS : build and submit the iOS app for testing
/Applications/Xamarin\ Studio.app/Contents/MacOS/mdtool -v build "--configuration:Debug|iPhone" ./CreditCardValidation.sln
mono $XUTCONSOLE submit $IPA $TESTCLOUD_API_KEY --devices $DEVICE_ID_IOS --series "iOS" --locale "en_US" --assembly-dir $TEST_ASSEMBLIES --app-name "Simple Credit Card Validator" --dsym $DSYM

# Android: Build and submit the Android app for testing
/usr/bin/xbuild /t:Package /p:Configuration=Release ./CreditCardValidation.Droid/CreditCardValidation.Droid.csproj
mono $XUTCONSOLE submit $APK $TESTCLOUD_API_KEY --devices $DEVICE_ID_ANDROID --series "Android" --locale "en_US" --assembly-dir $TEST_ASSEMBLIES --app-name "Simple Credit Card Validator"
