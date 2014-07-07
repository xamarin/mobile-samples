#!/bin/sh
# This shell script is a template for how to script compiling the application and then
# uploading the Xamarin.UITests to Xamarin Test Cloud.
#
# It is assumed that you already have installed the following NuGet packages:
#
#    Xamarin.UITest
#    Xamarin.UITest.Console

# Update these with the API key and device ID from the Test Cloud project.
export TESTCLOUD_ID=YOUR_API_KEY_HERE
export DEVICE_ID=YOUR_DEVICE_ID_HERE

# You shouldn't have to update these variable.
export IPA=./CreditCardValidation.iOS/bin/iPhone/Debug/CreditCardvalidationiOS-1.0.ipa
export TEST_ASSEMBLIES=./CreditCardValidation.Tests/bin/Debug/
export XUTCONSOLE=./packages/Xamarin.UITest.Console.0.4.0/tools/xut-console.exe

# Compile a debug .IPA for an iPhone.
/Applications/Xamarin\ Studio.app/Contents/MacOS/mdtool -v build "--configuration:Debug|iPhone" ./CreditCardValidation.sln

# This is the command line for submitting the C# tests to Test Cloud using xut-console.exe
mono $XUTCONSOLE submit $IPA $TESTCLOUD_ID --devices $DEVICE_ID --series "iOS" --locale "en_US" --assembly-dir $TEST_ASSEMBLIES

# This is the command line for submitting the Ruby tests to Test Cloud using the test-cloud gem.
#test-cloud submit $IPA $ID --devices $DEVICE_ID --series "iOS" --locale "en_US"