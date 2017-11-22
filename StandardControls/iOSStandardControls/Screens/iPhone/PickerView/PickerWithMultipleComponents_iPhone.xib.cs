using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace Example_StandardControls.Screens.iPhone.PickerView
{
	public partial class PickerWithMultipleComponents_iPhone : UIViewController
	{
		class PickerDataModel : UIPickerViewModel
		{
			public event EventHandler<EventArgs> ValueChanged;

			int selectedIndexLeft;
			int selectedIndexRigth;

			public string SelectionTitle {
				get {
					return string.Format ("{0} {1}", Items [0] [selectedIndexLeft], Items [1] [selectedIndexRigth]);
				}
			}

			/// <summary>
			/// The items to show up in the picker
			/// </summary>
			public Dictionary<int, List<string>> Items { get; private set; }

			public PickerDataModel ()
			{
				Items = new Dictionary<int, List<string>>();
			}

			/// <summary>
			/// Called by the picker to determine how many rows are in a given spinner item
			/// </summary>
			public override nint GetRowsInComponent (UIPickerView picker, nint component)
			{
				return Items [(int)component].Count;
			}

			/// <summary>
			/// called by the picker to get the text for a particular row in a particular
			/// spinner item
			/// </summary>
			public override string GetTitle (UIPickerView picker, nint row, nint component)
			{
				return Items [(int)component] [(int)row];
			}

			/// <summary>
			/// called by the picker to get the number of spinner items
			/// </summary>
			public override nint GetComponentCount (UIPickerView picker)
			{
				return Items.Count;
			}

			/// <summary>
			/// called when a row is selected in the spinner
			/// </summary>
			public override void Selected (UIPickerView picker, nint row, nint component)
			{
				if (component == 0)
					selectedIndexLeft = (int)row;
				else
					selectedIndexRigth = (int)row;

				if (ValueChanged != null) {
					ValueChanged (this, new EventArgs ());
				}
			}
		}

		PickerDataModel pickerDataModel;

		public PickerWithMultipleComponents_iPhone (IntPtr handle)
			: base (handle)
		{
		}

		public PickerWithMultipleComponents_iPhone ()
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Title = "Picker View";

			// create our simple picker modle
			pickerDataModel = new PickerDataModel ();

			var items = new List<string> ();
			items.Add ("1");
			items.Add ("2");
			items.Add ("3");
			pickerDataModel.Items.Add (0, items);

			items = new List<string> ();
			items.Add ("Red");
			items.Add ("Green");
			items.Add ("Blue");
			items.Add ("Alpha");
			pickerDataModel.Items.Add (1, items);

			// set it on our picker class
			pkrMain.Model = pickerDataModel;

			// wire up the item selected method
			pickerDataModel.ValueChanged += (s, e) => {
				lblSelectedItem.Text = pickerDataModel.SelectionTitle;
			};

			// set our initial selection on the label
			lblSelectedItem.Text = pickerDataModel.SelectionTitle;
		}
	}
}