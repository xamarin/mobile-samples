AnalogClock
===========

AnalogClock is a classic clock containing hour, minute, and second hands, with a circle of 
tick marks showing minutes and hours. A single solution contains versions for iOS, Android, and Windows Phone. 

**If you open the solution in Xamarin Studio, it will not be able to load the Windows Phone project;
and if you open the solution in Xamarin Studio under Windows, it will not be able to load the iOS project either.**

All three versions share a *ClockModel* class that implements the *INotifyPropertyChanged* interface, 
and provides angle values for the clock hands. The *ClockModel* class also creates a rudimentary timer 
using the *await* operator and the *Task.Delay* method.

All three versions draw the clockface ticks using a circle rendered with a dotted line.
The iOS and Android versions draw the clock hands from code using a graphics path with Bezier curves.
The Windows Phone version defines the clock entirely in XAML and is updated from the *ClockModel*
class defined as a resource.

Author
------

Charles Petzold

Testing 
