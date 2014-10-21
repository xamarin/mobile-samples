#!/bin/sh
# This is the server build script. It expects the following environment variables to be set outside 
# of this build script:
#
#		$XTC_API_KEY
#		$IOS_DEVICE_ID
#		$ANDROID_DEVICE_ID

### Grab all the nuget packages
/usr/bin/nuget restore CreditCardValidation.sln

### This will have to be updated when Xamarin.UITest is updated via NuGet.
export TESTCLOUD=./packages/Xamarin.UITest.0.6.5/tools/test-cloud.exe

### You shouldn't have to update these variables.
export TEST_ASSEMBLIES=./CreditCardValidation.Tests/bin/Debug/
export IPA=./CreditCardValidation.iOS/bin/iPhone/Debug/CreditCardvalidationiOS-1.0.ipa
export APK=./CreditCardValidation.Droid/bin/Release/CreditCardValidation.Droid.apk

### Uploading the dSYM files is optional - but it can help with troubleshooting 
export DSYM=./CreditCardValidation.iOS/bin/iPhone/Debug/CreditCardValidationiOS.app.dSYM

### Clean out any old builds
rm -rf ./CreditCardValidation.iOS/bin
rm -rf ./CreditCardValidation.iOS/obj
rm -rf ./CreditCardValidation.Droid/bin
rm -rf ./CreditCardValidation.Droid/obj
rm -rf ./CreditCardValidation.Common/bin
rm -rf ./CreditCardValidation.Common/obj
rm -rf ./CreditCardValidation.Tests/bin
rm -rf ./CreditCardValidation.Tests/obj


### iOS : build and submit the iOS app for testing
/Applications/Xamarin\ Studio.app/Contents/MacOS/mdtool -v build "--configuration:Debug|iPhone" ./CreditCardValidation.sln
/usr/bin/mono $TESTCLOUD submit $IPA $XTC_API_KEY --devices $IOS_DEVICE_ID --series "iOS" --locale "en_US" --assembly-dir $TEST_ASSEMBLIES --app-name "Simple Credit Card Validator" --dsym $DSYM

### Android: Build and submit the Android app for testing using the default keystore
#/usr/bin/xbuild /t:Package /p:Configuration=Release ./CreditCardValidation.Droid/CreditCardValidation.Droid.csproj
#/usr/bin/mono $XUTCONSOLE submit $APK $XTC_API_KEY --devices $ANDROID_DEVICE_ID --series "Android" --locale "en_US" --assembly-dir $TEST_ASSEMBLIES --app-name "Simple Credit Card Validator"
