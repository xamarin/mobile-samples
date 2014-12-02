using System;
using System.Drawing;
using MonoTouch.Dialog.Utilities;
using Foundation;
using UIKit;
using CoreGraphics;

namespace MWC.iOS.UI.CustomElements {
	/// <summary>
	/// Custom Exhibitor cell. Used to display exhibitor info.
	/// </summary>
	public class ExhibitorCell : UITableViewCell, IImageUpdated {
		BL.Exhibitor exhibitor;
		// control declarations
		protected UILabel nameLabel;
		protected UILabel cityCountryLabel;
		protected UILabel boothLocationLabel;
		protected UIImageView logoImageView;
		int cellTextLeft = 8 + 44 + 13;
		
		/// <summary>
		/// Gets the reuse identifier.
		/// </summary>
		/// <value>
		/// The reuse identifier.
		/// </value>
		public override string ReuseIdentifier
		{
			get { return cellId; }
		}
		static NSString cellId = new NSString("ExhibitorCell");
		
		/// <summary>
		/// Initializes a new instance of the <see cref="MWC.iOS.UI.CustomElements.ExhibitorCell"/> class.
		/// </summary>
		public ExhibitorCell (BL.Exhibitor showExhibitor) : base (new CGRect (0, 0, 320, 66 ) )
		{
			exhibitor = showExhibitor;

			// create the control and add it to the view
			nameLabel = new UILabel ( new CGRect ( cellTextLeft, 7, 231, 27 ) ); //9->7,23->25
			nameLabel.Font = UIFont.FromName ( "Helvetica-Light", AppDelegate.Font16pt );
			nameLabel.BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f);
			AddSubview (nameLabel);
			
			cityCountryLabel = new UILabel ( new CGRect ( cellTextLeft, 9+23, 231, 16 ) );  // 15->16
			cityCountryLabel.Font = UIFont.FromName ( "Helvetica-LightOblique",  AppDelegate.Font10pt );
			cityCountryLabel.TextColor = UIColor.DarkGray;
			cityCountryLabel.BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f);
			AddSubview ( cityCountryLabel );
			
			boothLocationLabel = new UILabel ( new CGRect ( cellTextLeft, 9+23+16+2, 231, 9 ) ); //15->17, 7->9
			boothLocationLabel.Font = UIFont.FromName ( "Helvetica-Light", AppDelegate.Font7_5pt );
			boothLocationLabel.BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f);
			AddSubview ( boothLocationLabel );
			
			logoImageView = new UIImageView ( new CGRect ( 8, 8, 44, 44 ) );
			AddSubview(logoImageView);
		}

		public void UpdateCell (BL.Exhibitor showExhibitor)
		{
			exhibitor = showExhibitor;
			nameLabel.Text = exhibitor.Name;
			cityCountryLabel.Text = exhibitor.City + ", " + exhibitor.Country;
			boothLocationLabel.Text = exhibitor.Locations;
		
			var u = new Uri(exhibitor.ImageUrl);
			logoImageView.Image = ImageLoader.DefaultRequestImage(u, this);
		}

		public void UpdatedImage (Uri uri)
		{
			logoImageView.Image = ImageLoader.DefaultRequestImage(uri, this);
		}
	}
}