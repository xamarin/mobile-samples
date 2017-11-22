using System;
using System.IO;

using Foundation;
using UIKit;

namespace SoMA {
	[Register ("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate {
		public override UIWindow Window { get; set; }

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
				return UIDevice.CurrentDevice.CheckSystemVersion (6, 0) ? "Avenir" : "HelveticaNeue";
			}
		}
		static string FontMediumName {
			get {
				return UIDevice.CurrentDevice.CheckSystemVersion (6, 0) ? "Avenir-Medium" : "HelveticaNeue-Medium";
			}
		}
		public static string FontLightName {
			get {
				return UIDevice.CurrentDevice.CheckSystemVersion (6, 0) ? "Avenir-Light" : "HelveticaNeue-Light";
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
	}
}

