
using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;

namespace Location.iOS.MainScreen
{
	public partial class MainViewController_iPhone : UIViewController, IMainScreen
	{
		#region Constructors

		// The IntPtr and initWithCoder constructors are required for controllers that need 
		// to be able to be created from a xib rather than from managed code

		public MainViewController_iPhone (IntPtr handle) : base(handle)
		{
			Initialize ();
		}

		[Export("initWithCoder:")]
		public MainViewController_iPhone (NSCoder coder) : base(coder)
		{
			Initialize ();
		}

		public MainViewController_iPhone () : base("MainViewController_iPhone", null)
		{
			Initialize ();
		}

		void Initialize ()
		{
		}
		
		#endregion
		
		public UILabel LblAltitude
		{
			get { return this.lblAltitude; }
		}
		public UILabel LblLatitude
		{
			get { return this.lblLatitude; }
		}
		public UILabel LblLongitude
		{
			get { return this.lblLongitude; }
		}
		public UILabel LblCourse
		{
			get { return this.lblCourse; }
		}
		public UILabel LblSpeed
		{
			get { return this.lblSpeed; }
		}
		
		
	}
}
