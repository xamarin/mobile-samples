// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace BluetoothLEExplorer.iOS
{
	partial class ServiceDetailsScreen
	{
		[Outlet]
		UIKit.UITableView CharacteristicsTable { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (CharacteristicsTable != null) {
				CharacteristicsTable.Dispose ();
				CharacteristicsTable = null;
			}
		}
	}
}
