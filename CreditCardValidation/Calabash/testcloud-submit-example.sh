#!/bin/sh
# This is a simple bash script that will compile the IPA and then
# enqueue a test run in Xamarin Test Cloud. 

# API key is a sensitive value and should be protected.
export TESTCLOUD_API_KEY=<YOUR API KEY HERE>

# The Device ID's are not sensitive - they just tell Test Cloud what devices to 
# run the test on. This device ID is for the following devices:
#		iPhone 5C (7.1.1)
#		iPhone 5S (7.0.4)
#		iPod Touch 5th Gen (7.0.4)
#		iPod Touch 5th Gen (7.1)
export DEVICE_ID_IOS=65195772

# You shouldn't have to update these variables.
export IPA=./CreditCardValidation.iOS/bin/iPhone/Debug/CreditCardvalidationiOS-1.0.ipa

# Uploading the dSYM files is optional - but it can help with troubleshooting.
export DSYM=./CreditCardValidation.iOS/bin/iPhone/Debug/CreditCardValidationiOS.app.dSYM

# iOS : build and submit the iOS app for testing
/Applications/Xamarin\ Studio.app/Contents/MacOS/mdtool -v build "--configuration:Debug|iPhone" ./CreditCardValidation.sln
test-cloud submit $IPA $TESTCLOUD_API_KEY --devices $DEVICE_ID_IOS  --series "iOS" --locale "en_US" --app-name="Simple Credit Card Validator" -y $DSYM