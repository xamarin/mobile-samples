WeatherApp (Native iOS, Android, and Windows Phone)
===================

WeatherApp (Native) is an example that accompanies [Build apps with native UI using Xamarin in Visual Studio](https://msdn.microsoft.com/library/dn879698.aspx) (MSDN Library).

It consists of a PCL that contains all the business logic. The platform-specific projects for Android, iOS, and Windows each contain native UI layers.

![screenshot](Screenshots/WeatherApp-All-sml.png)


To use this sample, you must first sign up for a free API key at [http://openweathermap.org/appid](http://openweathermap.org/appid). Paste that key in place of *YOUR API KEY HERE* in the following line of **WeatherApp/Core.cs**:

```
string key = "YOUR API KEY HERE";
```

The app will throw an exception to remind you if you forget.

Android Project
---------------
The WeatherApp\WeatherApp.Droid\WeatherApp.Droid.csproj file contains the following line to force use of Android Build tools version 23 instead of 24. This is done according to [Technical Bulletin: Android SDK Build Tools 24](https://releases.xamarin.com/technical-bulletin-android-sdk-build-tools-24/) to prevent "Unsupported major.minor version 52.0" errors when building the Android app.

You will need to make sure you have build tools 23 on your machine. Follow [James Montemagno's blog post](http://motzcod.es/post/149717060272/fix-for-unsupported-majorminor-version-520) for instructions.

Xamarin.Forms Version
---------------------

The equivalent app written with Xamarin.Forms is [Weather for Xamarin.Form](https://github.com/xamarin/xamarin-forms-samples/tree/master/Weather).


Authors
-------

Kraig Brockschmidt, Nicole Haugen
