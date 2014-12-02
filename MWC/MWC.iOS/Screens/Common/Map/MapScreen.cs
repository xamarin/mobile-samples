using System;
using System.Drawing;
using CoreLocation;
using MapKit;
using UIKit;
using CoreGraphics;

namespace MWC.iOS.Screens.Common.Map {
	/// <summary>
	/// Display a map of Barcelona with a pin at the conference center
	/// </summary>
	public class MapScreen : UIViewController {
		UINavigationBar toolbar;
		MKMapView mapView;
		UISegmentedControl segmentedControl;
		int toolbarHeight = 44;

		public MapScreen () : base ()
		{
			if (UIDevice.CurrentDevice.CheckSystemVersion (7,0)) 
				EdgesForExtendedLayout = UIRectEdge.All;
		}
		
		public override void ViewDidLoad () {
			base.ViewDidLoad ();

			toolbar = new UINavigationBar(new CGRect(0,0,View.Frame.Width,toolbarHeight));
			toolbar.SetItems (new UINavigationItem[]{
					new UINavigationItem("Map")
			}, false);
			toolbar.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
			toolbar.TintColor = AppDelegate.ColorNavBarTint;

			Title = Constants.MapPinSubtitle; // "Fira de Barcelona";
			TabBarItem.Title = "Map";
			
			// create our location and zoom for los angeles
			CLLocationCoordinate2D coords = new CLLocationCoordinate2D (Constants.MapPinLatitude, Constants.MapPinLongitude);
			MKCoordinateSpan span = new MKCoordinateSpan(MilesToLatitudeDegrees (3), MilesToLongitudeDegrees (3, coords.Latitude));
			
			mapView = new MKMapView(new CGRect(0, toolbarHeight, View.Frame.Width, UIScreen.MainScreen.ApplicationFrame.Height - toolbarHeight ));
			mapView.ShowsUserLocation = true;
			mapView.Frame = new CGRect (0, 0, View.Frame.Width, View.Frame.Height);
			mapView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth|UIViewAutoresizing.FlexibleHeight;
			// set the coords and zoom on the map
			mapView.Region = new MKCoordinateRegion (coords, span);

			segmentedControl = new UISegmentedControl();
			segmentedControl.InsertSegment("Map", 0, false);
			segmentedControl.InsertSegment("Satellite", 1, false);
			segmentedControl.InsertSegment("Hybrid", 2, false);
			segmentedControl.SelectedSegment = 0;
			segmentedControl.ControlStyle = UISegmentedControlStyle.Bar;
			segmentedControl.TintColor = UIColor.DarkGray;
			if (AppDelegate.IsPhone) {
				var topOfSegement = View.Frame.Height - 120;
				segmentedControl.Frame = new CGRect(20, topOfSegement, 282, 30);
				//segmentedControl.Frame = new CGRect(20, 340, 282, 30);
			} else {
				// IsPad
				var left = (View.Frame.Width / 2) - (282 / 2);
				segmentedControl.Frame = new CGRect(left, View.Frame.Height - 130, 282, 30);
				segmentedControl.AutoresizingMask = UIViewAutoresizing.FlexibleMargins;
			}
			segmentedControl.ValueChanged += delegate {
				if (segmentedControl.SelectedSegment == 0)
					mapView.MapType = MapKit.MKMapType.Standard;
				else if (segmentedControl.SelectedSegment == 1)
					mapView.MapType = MapKit.MKMapType.Satellite;
				else if (segmentedControl.SelectedSegment == 2)
					mapView.MapType = MapKit.MKMapType.Hybrid;
			};
			
			try {
				// add a basic annotation, got a bug report about these lines of code
				mapView.AddAnnotation (
					new BasicMapAnnotation (coords, Constants.MapPinTitle, Constants.MapPinSubtitle )
				);
			} catch (Exception mapex) {
				ConsoleD.WriteLine ("Not sure if happens " + mapex.Message); 
			}

			View.AddSubview(mapView);
			View.AddSubview(toolbar);
			View.AddSubview(segmentedControl);
		}
		
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			mapView.ShowsUserLocation = true;
		}
		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			mapView.ShowsUserLocation = false; // save battery?
		}

		/// <summary>
		/// Annotation to display the location of the conference
		/// </summary>
		protected class BasicMapAnnotation : MKAnnotation
		{
			/// <summary>
			/// The location of the annotation
			/// </summary>
			public override CLLocationCoordinate2D Coordinate { get; set; }
			
			/// <summary>
			/// Conference title
			/// </summary>
			public override string Title
			{ get { return title; } }
			protected string title;
			
			/// <summary>
			/// Conference address (summarised)
			/// </summary>
			public override string Subtitle
			{ get { return subtitle; } }
			protected string subtitle;
			
			public BasicMapAnnotation (CLLocationCoordinate2D c, string t, string s)
				: base()
			{
				Coordinate = c;
				title = t;
				subtitle = s;
			}
		}
		
		/// <summary>
		/// Converts miles to latitude degrees
		/// </summary>
		public double MilesToLatitudeDegrees(double miles)
		{
			double earthRadius = 3960.0;
			double radiansToDegrees = 180.0/Math.PI;
			return (miles/earthRadius) * radiansToDegrees;
		}

		/// <summary>
		/// Converts miles to longitudinal degrees at a specified latitude
		/// </summary>
		public double MilesToLongitudeDegrees(double miles, double atLatitude)
		{
			double earthRadius = 3960.0;
			double degreesToRadians = Math.PI/180.0;
			double radiansToDegrees = 180.0/Math.PI;

			// derive the earth's radius at that point in latitude
			double radiusAtLatitude = earthRadius * Math.Cos(atLatitude * degreesToRadians);
    		return (miles / radiusAtLatitude) * radiansToDegrees;
		}
	}
}