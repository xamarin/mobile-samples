Tasky
=====

Tasky is a simple cross-platform todo/task application sample that allows
you to track todo/task items.

Tasky supports iOS and Android - each with a native UI written in C#.
Common code lives in a [Portable Class Library](http://developer.xamarin.com/guides/cross-platform/application_fundamentals/pcl/), 
which is referenced by each application project.
It uses a local SQLite database to store the tasks, which are saved and read using
the SQLite-PCL NuGet package.

This project type requires Xamarin 3 (Xamarin Studio 5.x) or Visual Studio 2013 with PCL support.
It requires an Indie licence (or start a free Trial) of Xamarin.

![screenshot](https://github.com/xamarin/mobile-samples/raw/master/Tasky/Screenshots/all-small.png "iOS and Android")


Xamarin.Forms Version
---------------------
The equivalent app written with [Xamarin.Forms](http://xamarin.com/forms) is
called [Todo](https://github.com/xamarin/xamarin-forms-samples/tree/master/Todo).

Authors
-------

Bryan Costanich, Craig Dunn
