Visual Basic
============

Xamarin does not support Visual Basic.

It is possible, however, to build .NET Standard libraries in Visual Basic.NET (using Visual Studio).

.NET Standard assemblies can be consumed by Xamarin.iOS and Xamarin.Android apps. You can even build Xamarin.Forms apps using Visual Basic.NET for the common code. These two samples demonstrate how this can work:

* [TaskyVB](https://github.com/xamarin/mobile-samples/tree/master/VisualBasic/TaskyVB) - shows how to write business logic in VB.NET and consume it in mobile applications written in C#, with this [doc](https://docs.microsoft.com/en-us/xamarin/cross-platform/platform/visual-basic/native-apps).

* [XamarinFormsVB](https://github.com/xamarin/mobile-samples/tree/master/VisualBasic/XamarinFormsVB) - demonstrates a Xamarin.Forms application where the common code is all in VB.NET (referencing the Xamarin.Forms NuGet). 

![](https://raw.githubusercontent.com/xamarin/mobile-samples/master/VisualBasic/XamarinFormsVB/Screenshots/demo.png)