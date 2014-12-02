using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;

/*
 SoMA : Social Mobile Auth
 */
using System.IO;


namespace SoMA
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{

		public static Core.SomaDatabase Database { get; set; }


		public override void FinishedLaunching (UIApplication application)
		{
			string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
			string libraryPath = Path.Combine (documentsPath, "..","Library","soma.db"); // Library folder
			Database = new Core.SomaDatabase (libraryPath);


			#region APPEARANCE
			//http://dribbble.com/shots/1121640-iOS-7-Colors-PS-Swatches/attachments/142910
			UINavigationBar.Appearance.TintColor = new UIColor(88/255f, 86/255f, 214/255f, 1f);
			//UILabel.Appearance.Font = UIFont.FromName (FontLightName, 14f); // Avenir-Heavy, Avenir-Black, Avenir-Medium, ...Oblique
			UINavigationBar.Appearance.SetTitleTextAttributes (FontTitleTextAttributes);
			UIBarButtonItem.Appearance.SetTitleTextAttributes (FontBackTextAttributes, UIControlState.Normal);
			#endregion
		}





		#region iOS6 and iOS5 font support
		// seealso: WebViewControllerBase.cs for CSS specification
		static string FontName {
			get {
				if (UIDevice.CurrentDevice.CheckSystemVersion (6,0)) 
					return "Avenir";
				else
					return "HelveticaNeue";
			}
		}
		static string FontMediumName {
			get {
				if (UIDevice.CurrentDevice.CheckSystemVersion (6,0)) 
					return "Avenir-Medium";
				else
					return "HelveticaNeue-Medium";
			}
		}
		public static string FontLightName {
			get {
				if (UIDevice.CurrentDevice.CheckSystemVersion (6,0)) 
					return "Avenir-Light";
				else
					return "HelveticaNeue-Light";
			}
		}
		public UITextAttributes FontTitleTextAttributes {
			get {
				var fontTitleTextAttributes = new UITextAttributes();
				fontTitleTextAttributes.Font = UIFont.FromName(FontMediumName, 20f);
				return fontTitleTextAttributes;
			}
		}
		public UITextAttributes FontBackTextAttributes {
			get {
				var fontTitleTextAttributes = new UITextAttributes();
				fontTitleTextAttributes.Font = UIFont.FromName(FontLightName, 12f);
				return fontTitleTextAttributes;
			}
		}
		#endregion



		// class-level declarations
		public override UIWindow Window {
			get;
			set;
		}
		// This method is invoked when the application is about to move from active to inactive state.
		// OpenGL applications should use this method to pause.
		public override void OnResignActivation (UIApplication application)
		{
		}
		// This method should be used to release shared resources and it should store the application state.
		// If your application supports background exection this method is called instead of WillTerminate
		// when the user quits.
		public override void DidEnterBackground (UIApplication application)
		{
		}

		/// This method is called as part of the transiton from background to active state.
		public override void WillEnterForeground (UIApplication application)
		{
		}

		/// This method is called when the application is about to terminate. Save data, if needed. 
		public override void WillTerminate (UIApplication application)
		{
		}
	}
}

