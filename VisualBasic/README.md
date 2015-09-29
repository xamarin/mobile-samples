Visual Basic
============

Xamarin does not support Visual Basic.

It is possible, however, to build Portable Class Libraries (PCLs) in Visual Basic.NET (using Visual Studio).

These PCLs can be consumed by Xamarin.iOS and Xamarin.Android apps. You can even build Xamarin.Forms apps using Visual Basic.NET for the common code. These two samples demonstrate how this can work:

* [TaskyPortableVB](https://github.com/xamarin/mobile-samples/tree/master/VisualBasic/TaskyPortableVB) - shows how to write business logic in VB.NET and consume it in mobile applications written in C# (see this [blog](https://blog.xamarin.com/visual-basic-goes-mobile-with-portable-libraries/) and [doc](http://developer.xamarin.com/guides/cross-platform/application_fundamentals/pcl/portable_visual_basic_net/))

* [XamarinFormsVB](https://github.com/xamarin/mobile-samples/tree/master/VisualBasic/XamarinFormsVB) - demonstrates a Xamarin.Forms application where the common code is all in VB.NET (referencing the Xamarin.Forms Nuget). 

![](https://raw.githubusercontent.com/xamarin/mobile-samples/master/VisualBasic/XamarinFormsVB/Screenshots/demo.png)