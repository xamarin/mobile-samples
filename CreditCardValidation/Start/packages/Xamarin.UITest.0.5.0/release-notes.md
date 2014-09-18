# Xamarin.UITest release notes

## 0.5.0

* `xut-console.exe` has been renamed to `test-cloud.exe`
* The `Xamarin.UITest.Console` NuGet package has been removed. Everything is (and has been for a while) contained in the `Xamarin.UITest` NuGet package
* `.DeviceIdentifier(...)` can now accept iOS simulator strings for running on a specific simulator. Entering an invalid text produces and error with all the valid options
* Support for `[TestFixtureSetUp]` has been added for Test Cloud
* Android test start up time has been improved
* Direct HTTP access to the test server is available through `.TestServer`
* `.Parent()`, `.Sibling()`, `.Descendant()` and `.Child()` can all accept an integer index for the element - so `.Child(1)` is equivalent to the current `.Child().Index(1)`
* The query language contains `.WebView()` for cross platform web view selection
* The query language has `.InvokeJS(...)` for evaluating JavaScript in matched web views
* Dynamic wait times has been added - currently used for longer default waits in Test Cloud and shorter default waits in the REPL
* `iOSApp` and `IApp` now also have support for `.Back()`
* Android `.EnterText()` has been converted from old `setText` approach to touching the input field and entering text

## 0.4.10

* Adds `EnterText(text)` to `IApp` and `AndroidApp` for typing into the currently focused view element

## 0.4.9

* Allows direct access to iOS UIA using JavaScript via `InvokeUia(script)` for advanced scenarios and workarounds
* Fixes an issue with proxying HTTP over USB for iOS on physical devices on some systems
* Improves the error message when trying to use `SetLocation` on Android without having the `android.permission.ALLOW_MOCK_LOCATIONS` permission

## 0.4.8

* Both Android and iOS now have support for `.Flash(...)` to highlight view elements matched by the query
* Both Android and iOS now have support for `.SetLocation(...)` - it can be found under `app.Device`
* Invoking methods on Android where the result cannot be serialized by calabash are handled more gracefully
* Running tests on both iOS simulators and physical devices should no longer produce port conflicts
* Deserializing json results have better support for `object` and nullables
* Configuration properties on `.Config` have found a new home in `.Device` for clearer naming
* Android scrolling works again, after adapting to the new method used by calabash-android 
* Uploading iOS tests no longer fail if user does not have the Android SDK installed
* Improved a bunch of error messages in different scenarios
