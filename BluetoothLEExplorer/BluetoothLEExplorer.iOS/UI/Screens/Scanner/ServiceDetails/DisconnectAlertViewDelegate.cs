using System;
using UIKit;

namespace BluetoothLEExplorer.iOS
{
	public class DisconnectAlertViewDelegate : UIAlertViewDelegate
	{
		readonly UIViewController parent;

		public DisconnectAlertViewDelegate(UIViewController parent)
		{
			this.parent = parent;
		}

		public override void Clicked (UIAlertView alertview, nint buttonIndex)
		{
			parent.NavigationController.PopViewController(true);
		}

		public override void Canceled (UIAlertView alertView)
		{
			parent.NavigationController.PopViewController(true);
		}
	}
}