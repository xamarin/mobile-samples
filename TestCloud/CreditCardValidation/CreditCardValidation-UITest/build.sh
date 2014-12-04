#!/bin/sh
# This is a sample build script in Bash.

### You will have to update these variables for your environment
export XTC_API_KEY=YOUR_API_KEY_HERE
export IOS_DEVICE_ID=YOUR_IOS_DEVICE_ID_HERE
export ANDROID_DEVICE_ID=YOUR_ANDROID_DEVICE_ID_HERE

### This will have to be updated when Xamarin.UITest is updated via NuGet.
export TESTCLOUD=./packages/Xamarin.UITest.0.6.7/tools/test-cloud.exe

### You shouldn't have to update these variables.
export TEST_ASSEMBLIES=./CreditCardValidation.UITests/bin/Debug/
export IPA=./CreditCardValidation.iOS/bin/iPhone/Debug/CreditCardvalidationiOS-1.0.ipa
export APK=./CreditCardValidation.Droid/bin/Release/CreditCardValidation.Droid.apk

### Grab all the nuget packages
/usr/bin/nuget restore CreditCardValidation.sln

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
/usr/bin/mono $TESTCLOUD submit $IPA $XTC_API_KEY --devices $IOS_DEVICE_ID --series "iOS" --locale "en_US" --assembly-dir $TEST_ASSEMBLIES --app-name "Credit Card Validator" --dsym $DSYM

### Android: Build and submit the Android app for testing using the default keystore
/usr/bin/xbuild /t:Package /p:Configuration=Release /p:AndroidUseSharedRuntime=false /p:EmbedAssembliesIntoApk=true ./CreditCardValidation.Droid/CreditCardValidation.Droid.csproj
/usr/bin/xbuild /t:Package /p:Configuration=Debug /p:AndroidUseSharedRuntime=false /p:EmbedAssembliesIntoApk=true ./CreditCardValidation.Droid/CreditCardValidation.Droid.csproj
/usr/bin/mono $TESTCLOUD submit $APK $XTC_API_KEY --devices $ANDROID_DEVICE_ID --series "Android" --locale "en_US" --assembly-dir $TEST_ASSEMBLIES --app-name "Credit Card Validator"
