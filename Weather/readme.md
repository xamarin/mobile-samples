WeatherApp (Native iOS, Android, and UWP)
===================

WeatherApp (Native) is an example that accompanies [Build apps with native UI using Xamarin in Visual Studio](https://docs.microsoft.com/visualstudio/cross-platform/build-apps-with-native-ui-using-xamarin-in-visual-studio).

It consists of a .NET Standard Library that contains all the business logic. The platform-specific projects for Android, iOS, and UWP each contain native UI layers.

To use this sample, you must first sign up for a free API key at [http://openweathermap.org/appid](http://openweathermap.org/appid). Paste that key in place of *YOUR API KEY HERE* in the following line of **WeatherApp/Core.cs**:

```
string key = "YOUR API KEY HERE";
```

The app will throw an exception to remind you if you forget.

Xamarin.Forms Version
---------------------

The equivalent app written with Xamarin.Forms is [Weather for Xamarin.Form](https://github.com/xamarin/xamarin-forms-samples/tree/master/Weather).

Authors
-------

Kraig Brockschmidt, Nicole Haugen, Charles Petzold