#!/bin/sh

# This is a bash script that will compile the iOS and Android applications.

### You shouldn't have to update these variables.
export IPA=./CreditCardValidation.iOS/bin/iPhone/Debug/CreditCardvalidationiOS-1.0.ipa
export APK=./CreditCardValidation.Droid/bin/Release/CreditCardValidation.Droid.apk

### Remove the old directories - a poor man's clean.
rm -rf CreditCardValidation.Droid/obj
rm -rf CreditCardValidation.Droid/bin
rm -rf CreditCardValidation.iOS/obj
rm -rf CreditCardValidation.iOS/bin
rm -rf ./CreditCardValidation.Common/bin
rm -rf ./CreditCardValidation.Common/obj


### iOS : compile a Debug build for the iPhone
/Applications/Xamarin\ Studio.app/Contents/MacOS/mdtool -v build "--configuration:Debug|iPhone" ./CreditCardValidation.sln

### Android: compile a Debug and a Release build for Android. Notice that the Android Share runtime is disabled.
/usr/bin/xbuild /t:Package /p:Configuration=Release /p:AndroidUseSharedRuntime=false /p:EmbedAssembliesIntoApk=true ./CreditCardValidation.Droid/CreditCardValidation.Droid.csproj
/usr/bin/xbuild /t:Package /p:Configuration=Debug /p:AndroidUseSharedRuntime=false /p:EmbedAssembliesIntoApk=true ./CreditCardValidation.Droid/CreditCardValidation.Droid.csproj
