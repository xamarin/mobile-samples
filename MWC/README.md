MWC 2012
========

MWC 2012 is an open-source conference schedule application for www.mobileworldcongress.com in Barcelona, Spain (February 2012).

It illustrates proper application architecture layering and uses a common code base for the Business Layer (BL), Data Access Layer (DAL), and Data Layer (DL) layers. It then separates out the User Interface (UI) and Application Layer (AL) into the appropriate device-applications.

There are versions for iPhone/iPad, Android and Windows Phone 7.

You can read more about this app on the Xamarin blog: http://blog.xamarin.com/2012/02/24/mwc_2012/

* NOTE: the Android version was written for Google Maps version 1.0 and requires an API key to enable the map tiles in the MapView. **Google no longer supports this version of Maps, so the map functionality is currently 'disabled'**.
* If you already have a Google Maps version 1.0 API key, you can enable the map by adding a GOOGLEMAPSV1 compiler directive, then see the 
instructions in /Resources/Values/PrivateStringsTemplate.xml and /Resources/Layout/MapScreen.axml for details on where to add the API key. You must also
have installed the Google Maps API Add-On (instructions: http://docs.xamarin.com/android/tutorials/Maps_and_Location/Part_2_-_Maps_API#Google_APIs_Add-On)


Authors
-------

Bryan Costanich, 
Craig Dunn,
Frank Krueger, 
Brian Kim, 
James Clancey