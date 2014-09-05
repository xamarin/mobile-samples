#!/bin/sh
# This is the server build script. 

### Grab all the nuget packages
/usr/bin/nuget restore CreditCardValidation.sln

### This will have to be updated when xut-console is updated.
export XUTCONSOLE=./packages/Xamarin.UITest.0.4.8/tools/xut-console.exe

### You shouldn't have to update these variables.
export TEST_ASSEMBLIES=./CreditCardValidation.Tests/bin/Debug/
export IPA=./CreditCardValidation.iOS/bin/iPhone/Debug/CreditCardvalidationiOS-1.0.ipa
export APK=./CreditCardValidation.Droid/bin/Release/CreditCardValidation.Droid.apk

### Uploading the dSYM files is optional - but it can help with troubleshooting 
export DSYM=./CreditCardValidation.iOS/bin/iPhone/Debug/CreditCardValidationiOS.app.dSYM

### iOS : build and submit the iOS app for testing
/Applications/Xamarin\ Studio.app/Contents/MacOS/mdtool -v build "--configuration:Debug|iPhone" ./CreditCardValidation.sln
/usr/bin/mono $XUTCONSOLE submit $IPA $TESTCLOUD_API_KEY --devices $DEVICE_ID_IOS --series "iOS" --locale "en_US" --assembly-dir $TEST_ASSEMBLIES --app-name "Simple Credit Card Validator" --dsym $DSYM

### Android: Build and submit the Android app for testing using the default keystore
/usr/bin/xbuild /t:Package /p:Configuration=Release ./CreditCardValidation.Droid/CreditCardValidation.Droid.csproj
/usr/bin/mono $XUTCONSOLE submit $APK $TESTCLOUD_API_KEY --devices $DEVICE_ID_ANDROID --series "Android" --locale "en_US" --assembly-dir $TEST_ASSEMBLIES --app-name "Simple Credit Card Validator"
