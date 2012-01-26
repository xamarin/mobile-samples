using System;
using MonoTouch.UIKit;
using MonoTouch.MapKit;
using System.Drawing;
using MonoTouch.CoreLocation;

namespace MWC.iOS.Screens.Common.Map
{
	public class MapController : UIViewController
	{
		MKMapView _mapView;
		UISegmentedControl _segmentedControl;

		public MapController (RectangleF frame) : base ()
		{
			this._mapView = new MKMapView(frame);
			this._mapView.ShowsUserLocation = true;
			this._mapView.Frame = new RectangleF (0, 0, this.View.Frame.Width, this.View.Frame.Height);
			this._mapView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth|UIViewAutoresizing.FlexibleHeight;
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			Title = "Fira de Barcelona";
			this.TabBarItem.Title = "Map";
			
			// create our location and zoom for los angeles
			CLLocationCoordinate2D coords = new CLLocationCoordinate2D (41.374377, 2.152226);
			MKCoordinateSpan span = new MKCoordinateSpan(MilesToLatitudeDegrees (3), MilesToLongitudeDegrees (3, coords.Latitude));

			// set the coords and zoom on the map
			this._mapView.Region = new MKCoordinateRegion (coords, span);
			

			_segmentedControl = new UISegmentedControl();
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
				_segmentedControl.Frame = new RectangleF(20, 340, 282,30);
			else
			{
				_segmentedControl.Frame = new RectangleF(20, this.View.Frame.Height - 120, 282,30);
				_segmentedControl.AutoresizingMask = UIViewAutoresizing.FlexibleTopMargin;
			}
			_segmentedControl.InsertSegment("Map", 0, false);
			_segmentedControl.InsertSegment("Satellite", 1, false);
			_segmentedControl.InsertSegment("Hybrid", 2, false);
			_segmentedControl.SelectedSegment = 0;
			_segmentedControl.ControlStyle = UISegmentedControlStyle.Bar;
			_segmentedControl.TintColor = UIColor.DarkGray;
			
			_segmentedControl.ValueChanged += delegate {
				if (_segmentedControl.SelectedSegment == 0)
					_mapView.MapType = MonoTouch.MapKit.MKMapType.Standard;
				else if (_segmentedControl.SelectedSegment == 1)
					_mapView.MapType = MonoTouch.MapKit.MKMapType.Satellite;
				else if (_segmentedControl.SelectedSegment == 2)
					_mapView.MapType = MonoTouch.MapKit.MKMapType.Hybrid;
			};
			
			try {
				// add a basic annotation, got a bug report about these lines of code
				this._mapView.AddAnnotation (
					new BasicMapAnnotation (coords, "Mobile World Congress 2012", Title )
				);
			} catch (Exception mapex) { Console.WriteLine ("Not sure if this happens " + mapex.Message); }

			// add the map to the screen
			this.View.AddSubview(this._mapView);
			this.View.AddSubview(this._segmentedControl);
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
			
			public BasicMapAnnotation (CLLocationCoordinate2D coordinate, string title, string subTitle)
				: base()
			{
				this.Coordinate = coordinate;
				this.title = title;
				this.subtitle = subTitle;
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