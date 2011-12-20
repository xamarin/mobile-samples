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
		
		public MapController (RectangleF frame) : base ()
		{
			this._mapView = new MKMapView(frame);
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			Title = "Fira de Barcelona";
			this.TabBarItem.Title = "Map";
			
			// create our location and zoom for los angeles
			CLLocationCoordinate2D coords = new CLLocationCoordinate2D (34.120, -118.188);
			MKCoordinateSpan span = new MKCoordinateSpan(MilesToLatitudeDegrees (20), MilesToLongitudeDegrees (20, coords.Latitude));

			// set the coords and zoom on the map
			this._mapView.Region = new MKCoordinateRegion (coords, span);
			
			// add the map to the screen
			this.View.AddSubview(this._mapView);
			
			// add a basic annotation
			this._mapView.AddAnnotation (new BasicMapAnnotation (new CLLocationCoordinate2D (34.120, -118.188), "Los Angeles", "City of Demons"));
			
		}
		
		/// <summary>
		/// Basic map annotation.
		/// </summary>
		protected class BasicMapAnnotation : MKAnnotation
		{
			/// <summary>
			/// The location of the annotation
			/// </summary>
			public override CLLocationCoordinate2D Coordinate { get; set; }
			
			/// <summary>
			/// The title text
			/// </summary>
			public override string Title
			{ get { return title; } }
			protected string title;
			
			/// <summary>
			/// The subtitle text
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

