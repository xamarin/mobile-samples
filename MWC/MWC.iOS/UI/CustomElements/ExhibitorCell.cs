using System;
using System.Drawing;
using MonoTouch.Dialog.Utilities;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Collections.Generic;
using System.Text;

namespace MWC.iOS.UI.CustomElements
{
	/// <summary>
	/// Custom Exhibitor cell. Used to display exhibitor info.
	/// </summary>
	public class ExhibitorCell : UITableViewCell, IImageUpdated
	{
		BL.Exhibitor _exhibitor;
		// control declarations
		protected UILabel _nameLabel;
		protected UILabel _cityCountryLabel;
		protected UILabel _boothLocationLabel;
		protected UIImageView _logoImageView;
		int cellTextLeft = 8 + 44 + 13;
		
//		/// <summary>
//		/// Sets the name label.
//		/// </summary>
//		public string Name { set { this._nameLabel.Text = value; } }
//		/// <summary>
//		/// Sets the city and country label.
//		/// </summary>
//		public string CityAndCountry { set { this._cityCountryLabel.Text = value; } }
//		/// <summary>
//		/// Sets the booth locations label.
//		/// </summary>
//		public string BoothLocations { set { this._boothLocationLabel.Text = value; } }

		
		/// <summary>
		/// Gets the reuse identifier.
		/// </summary>
		/// <value>
		/// The reuse identifier.
		/// </value>
		public override string ReuseIdentifier
		{
			get { return _reuseId; }
		}
		static NSString _reuseId = new NSString("ExhibitorCell");
		
		/// <summary>
		/// Initializes a new instance of the <see cref="MWC.iOS.UI.CustomElements.ExhibitorCell"/> class.
		/// </summary>
		public ExhibitorCell (BL.Exhibitor exhibitor) : base (new RectangleF (0, 0, 320, 66 ) )
		{
			_exhibitor = exhibitor;

			// create the control and add it to the view
			this._nameLabel = new UILabel ( new RectangleF ( cellTextLeft, 7, 231, 27 ) ); //9->7,23->25
			this._nameLabel.Font = UIFont.FromName ( "Helvetica-Light", AppDelegate.Font16pt );
			this._nameLabel.BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f);
			this.AddSubview (this._nameLabel);
			
			this._cityCountryLabel = new UILabel ( new RectangleF ( cellTextLeft, 9+23, 231, 16 ) );  // 15->16
			this._cityCountryLabel.Font = UIFont.FromName ( "Helvetica-LightOblique",  AppDelegate.Font10pt );
			this._cityCountryLabel.TextColor = UIColor.DarkGray;
			this._cityCountryLabel.BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f);
			this.AddSubview ( this._cityCountryLabel );
			
			this._boothLocationLabel = new UILabel ( new RectangleF ( cellTextLeft, 9+23+16+2, 231, 9 ) ); //15->17, 7->9
			this._boothLocationLabel.Font = UIFont.FromName ( "Helvetica-Light", AppDelegate.Font7_5pt );
			this._boothLocationLabel.BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f);
			this.AddSubview ( this._boothLocationLabel );
			
			this._logoImageView = new UIImageView ( new RectangleF ( 8, 8, 44, 44 ) );
			this.AddSubview(this._logoImageView);
		}

		public void UpdateCell (BL.Exhibitor exhibitor)
		{
			this._exhibitor = exhibitor;
			_nameLabel.Text = this._exhibitor.Name;
			_cityCountryLabel.Text = this._exhibitor.City + ", " + this._exhibitor.Country;
			_boothLocationLabel.Text = this._exhibitor.Locations;
		
			Console.WriteLine("INITIAL:" + _exhibitor.ImageUrl);
			var u = new Uri(_exhibitor.ImageUrl);
			_logoImageView.Image = ImageLoader.DefaultRequestImage(u,this);
		}

		public void UpdatedImage (Uri uri)
		{
			Console.WriteLine("UPDATED:" + uri.AbsoluteUri);
			_logoImageView.Image = ImageLoader.DefaultRequestImage(uri, this);
		}
	}
}

