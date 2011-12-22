using System;
using System.Drawing;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Collections.Generic;
using System.Text;

namespace MWC.iOS.UI.CustomElements
{
	/// <summary>
	/// Custom Exhibitor cell. Used to display exhibitor info.
	/// </summary>
	public class ExhibitorCell : UITableViewCell
	{
		// control declarations
		protected UILabel _nameLabel;
		protected UILabel _cityCountryLabel;
		protected UILabel _boothLocationLabel;
		protected UIImageView _logoImageView;
		
		/// <summary>
		/// Sets the name label.
		/// </summary>
		public string Name { set { this._nameLabel.Text = value; } }
		/// <summary>
		/// Sets the city and country label.
		/// </summary>
		public string CityAndCountry { set { this._cityCountryLabel.Text = value; } }
		/// <summary>
		/// Sets the booth locations label.
		/// </summary>
		public string BoothLocations { set { this._boothLocationLabel.Text = value; } }
		/// <summary>
		/// Sets the logo image.
		/// </summary>
		public UIImage Logo { set { this._logoImageView.Image = value; } }
		
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
		public ExhibitorCell () : base (new RectangleF (0, 0, 320, 66 ) )
		{
			// create the control and add it to the view
			this._nameLabel = new UILabel ( new RectangleF ( 69, 6, 231, 21 ) );
			this._nameLabel.Font = UIFont.SystemFontOfSize ( 17.0f );
			this.AddSubview (this._nameLabel);
			
			this._cityCountryLabel = new UILabel ( new RectangleF ( 69, 24, 231, 21 ) );
			this._cityCountryLabel.Font = UIFont.SystemFontOfSize ( 15.0f );
			this._cityCountryLabel.TextColor = UIColor.DarkGray;
			this.AddSubview ( this._cityCountryLabel );
			
			this._boothLocationLabel = new UILabel ( new RectangleF ( 69, 41, 231, 21 ) );
			this._boothLocationLabel.Font = UIFont.SystemFontOfSize ( 13.0f );
			this.AddSubview ( this._boothLocationLabel );
			
			this._logoImageView = new UIImageView ( new RectangleF ( 5, 2, 60, 60 ) );
			this.AddSubview(this._logoImageView);
		}
	}
}

