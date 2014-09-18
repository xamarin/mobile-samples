#!/bin/sh

# This is a bash script that will compile the iOS and Android applications.

### You shouldn't have to update these variables.
export IPA=./CreditCardValidation.iOS/bin/iPhone/Debug/CreditCardvalidationiOS-1.0.ipa
export APK=./CreditCardValidation.Droid/bin/Release/CreditCardValidation.Droid.apk

### Remove the old directories
rm -rf CreditCardValidation.Droid/obj
rm -rf CreditCardValidation.Droid/bin
rm -rf CreditCardValidation.iOS/obj
rm -rf CreditCardValidation.iOS/bin

### iOS : compile a Debug build for the iPhone
/Applications/Xamarin\ Studio.app/Contents/MacOS/mdtool -v build "--configuration:Debug|iPhone" ./CreditCardValidation.sln

### Android: compile a Release build for Android
/usr/bin/xbuild /t:Package /p:Configuration=Release ./CreditCardValidation.Droid/CreditCardValidation.Droid.csproj