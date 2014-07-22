#!/bin/sh
### This is a simple bash script that will compile the APK and IPA and
### then enqueue a test run in Xamarin Test Cloud. 

### API key is a sensitive value and should be protected.
export TESTCLOUD_API_KEY=<YOUR API KEY HERE>

### The Device ID's are not so sensitive - they just tell Test Cloud what
### what devices run the test on.
export DEVICE_ID_IOS=65195772
export DEVICE_ID_ANDROID=f2d04717

### This will have to be updated when xut-console is updated.
export XUTCONSOLE=./packages/Xamarin.UITest.Console.0.4.3/tools/xut-console.exe

### You shouldn't have to update these variables.
export TEST_ASSEMBLIES=./CreditCardValidation.Tests/bin/Debug/
export IPA=./CreditCardValidation.iOS/bin/iPhone/Debug/CreditCardvalidationiOS-1.0.ipa
export APK=./CreditCardValidation.Droid/bin/Release/CreditCardValidation.Droid.apk

### Uploading the dSYM files is optional - but it can help with troubleshooting 
export DSYM=./CreditCardValidation.iOS/bin/iPhone/Debug/CreditCardValidationiOS.app.dSYM

### iOS : build and submit the iOS app for testing
/Applications/Xamarin\ Studio.app/Contents/MacOS/mdtool -v build "--configuration:Debug|iPhone" ./CreditCardValidation.sln
mono $XUTCONSOLE submit $IPA $TESTCLOUD_API_KEY --devices $DEVICE_ID_IOS --series "iOS" --locale "en_US" --assembly-dir $TEST_ASSEMBLIES --app-name "Simple Credit Card Validator" --dsym $DSYM

### Android: Build and submit the Android app for testing using the default keystore
/usr/bin/xbuild /t:Package /p:Configuration=Release ./CreditCardValidation.Droid/CreditCardValidation.Droid.csproj
mono $XUTCONSOLE submit $APK $TESTCLOUD_API_KEY --devices $DEVICE_ID_ANDROID --series "Android" --locale "en_US" --assembly-dir $TEST_ASSEMBLIES --app-name "Simple Credit Card Validator"


### The following lines are an exmaple of how to create and use a signing 
### information file to submit the Android tests
# export APK_SIGNED=./CreditCardValidation.Droid/bin/Release/CreditCardValidation.Droid-Signed.apk
# export APK_FINAL=./CreditCardValidation.Droid/bin/Release/CreditCardValidation.apk
# export KEYSTORE_PASS=password1
# export ALIAS_PASS=password2

### Android: Build and submit the application using the signing information file.
### This assumes that jarsigner and zipalign are in your $PATH
# /usr/bin/xbuild /t:Package /p:Configuration=Release ./CreditCardValidation.Droid/CreditCardValidation.Droid.csproj
# jarsigner -verbose -sigalg SHA1withRSA -digestalg SHA1 -keystore creditcardvalidation-example.keystore -signedjar $APK_SIGNED -storepass $KEYSTORE_PASS -keypass $ALIAS_PASS $APK  uitest_sample
# zipalign -f -v 4 $APK_SIGNED $APK_FINAL

### It is only necessary to create the signing info file when xut-console.exe is updated. This is a sample of how to do so: 
# mono $XUTCONSOLE gen-sign-info $APK_FINAL ./creditcardvalidation-example.keystore $KEYSTORE_PASS uitest_sample $ALIAS_PASS --dir ./
# mono $XUTCONSOLE submit $APK_FINAL $TESTCLOUD_API_KEY --devices $DEVICE_ID_ANDROID  --sign-info ./testserver.si --series "Android" --locale "en_US" --assembly-dir $TEST_ASSEMBLIES --app-name "Simple Credit Card Validator" 
