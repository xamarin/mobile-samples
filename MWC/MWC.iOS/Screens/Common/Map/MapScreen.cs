using System;
using System.Drawing;
using MonoTouch.CoreLocation;
using MonoTouch.MapKit;
using MonoTouch.UIKit;

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
		}
		
		public override void ViewDidLoad () {
			base.ViewDidLoad ();
			
			toolbar = new UINavigationBar(new RectangleF(0,0,View.Frame.Width,toolbarHeight));
			toolbar.SetItems (new UINavigationItem[]{
					new UINavigationItem("Map")
			}, false);
			toolbar.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
			toolbar.TintColor = UIColor.DarkGray;

			Title = "Fira de Barcelona";
			TabBarItem.Title = "Map";
			
			// create our location and zoom for los angeles
			CLLocationCoordinate2D coords = new CLLocationCoordinate2D (41.374377, 2.152226);
			MKCoordinateSpan span = new MKCoordinateSpan(MilesToLatitudeDegrees (3), MilesToLongitudeDegrees (3, coords.Latitude));
			
			mapView = new MKMapView(new RectangleF(0, toolbarHeight, View.Frame.Width, UIScreen.MainScreen.ApplicationFrame.Height - toolbarHeight ));
			mapView.ShowsUserLocation = true;
			mapView.Frame = new RectangleF (0, 0, View.Frame.Width, View.Frame.Height);
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
				segmentedControl.Frame = new RectangleF(20, 340, 282, 30);
			} else {
				// IsPad
				var left = (View.Frame.Width / 2) - (282 / 2);
				segmentedControl.Frame = new RectangleF(left, View.Frame.Height - 70, 282, 30);
				segmentedControl.AutoresizingMask = UIViewAutoresizing.FlexibleMargins;
			}
			segmentedControl.ValueChanged += delegate {
				if (segmentedControl.SelectedSegment == 0)
					mapView.MapType = MonoTouch.MapKit.MKMapType.Standard;
				else if (segmentedControl.SelectedSegment == 1)
					mapView.MapType = MonoTouch.MapKit.MKMapType.Satellite;
				else if (segmentedControl.SelectedSegment == 2)
					mapView.MapType = MonoTouch.MapKit.MKMapType.Hybrid;
			};
			
			try {
				// add a basic annotation, got a bug report about these lines of code
				mapView.AddAnnotation (
					new BasicMapAnnotation (coords, "Mobile World Congress 2012", Title )
				);
			} catch (Exception mapex) {
				Console.WriteLine ("Not sure if happens " + mapex.Message); 
			}

			View.AddSubview(mapView);
			View.AddSubview(toolbar);
			View.AddSubview(segmentedControl);
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