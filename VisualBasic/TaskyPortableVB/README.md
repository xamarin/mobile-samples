TaskyPortable (with VB and XML)

=============



Sample for the [Portable Visual Basic.NET](http://developer.xamarin.com/guides/cross-platform/application_fundamentals/pcl/portable_visual_basic_net/) document on the [Xamarin Developer Portal](http://developer.xamarin.com).

IMPORTANT
---------

This sample *requires* Visual Studio.

^ if you *really* wanted to try it in Visual Studio, you'll have to remove the Visual Basic project and reference the TaskyPortableVB.dll directly.

NOTES

-----



This simple to-do list app demonstrates using a **Portable Class Library** to share code between Xamarin.iOS, Xamarin.Android and Windows apps.



To keep the code simple we use a single XML file as the data-store - it contains a serialized collection of Task objects that we read & write to when the user edits their to-do items.



There are two implementations of the Portable code:



* C#

* [Visual Basic](https://gist.github.com/conceptdev/bba20899cf8901cc36a2)!



Because Portable Class Library profiles do not allow `System.IO` we expose an interface in the shared code that we implement in our apps (`IXmlStorage`). Thanks to the magic of .NET the two language implementations are interchangeable and application code 'just works' whichever PCL you reference.



If you are browsing the VB code, this 
[Visual Basic Reference](http://msdn.microsoft.com/en-us/library/sh9ywfdk.aspx) might come in handy ;)