using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace Example_StandardControls.Screens.iPhone.PickerView
{
	public partial class PickerView1_iPhone : UIViewController
	{
		PickerDataModel pickerDataModel;

		public PickerView1_iPhone (IntPtr handle)
			: base (handle)
		{
		}

		public PickerView1_iPhone ()
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Title = "Picker View";

			// create our simple picker model
			pickerDataModel = new PickerDataModel ();
			pickerDataModel.Items.Add ("item the first!");
			pickerDataModel.Items.Add ("item the second!");
			pickerDataModel.Items.Add ("item the third!");
			pickerDataModel.Items.Add ("fourth item!");

			// set it on our picker class
			pkrMain.Model = pickerDataModel;

			// wire up the value change method
			pickerDataModel.ValueChanged += (s, e) => {
				lblSelectedItem.Text = pickerDataModel.SelectedItem;
			};

			// set our initial selection on the label
			lblSelectedItem.Text = pickerDataModel.SelectedItem;
		}

		/// <summary>
		/// This is our simple picker model. it uses a list of strings as it's data and exposes
		/// a ValueChanged event when the picker changes.
		/// </summary>
		protected class PickerDataModel : UIPickerViewModel
		{
			public event EventHandler<EventArgs> ValueChanged;

			/// <summary>
			/// The items to show up in the picker
			/// </summary>
			public List<string> Items { get; private set; }

			/// <summary>
			/// The current selected item
			/// </summary>
			public string SelectedItem {
				get { return Items [selectedIndex]; }
			}

			int selectedIndex = 0;

			public PickerDataModel ()
			{
				Items = new List<string> ();
			}

			/// <summary>
			/// Called by the picker to determine how many rows are in a given spinner item
			/// </summary>
			public override nint GetRowsInComponent (UIPickerView picker, nint component)
			{
				return Items.Count;
			}

			/// <summary>
			/// called by the picker to get the text for a particular row in a particular
			/// spinner item
			/// </summary>
			public override string GetTitle (UIPickerView picker, nint row, nint component)
			{
				return Items [(int)row];
			}

			/// <summary>
			/// called by the picker to get the number of spinner items
			/// </summary>
			public override nint GetComponentCount (UIPickerView picker)
			{
				return 1;
			}

			/// <summary>
			/// called when a row is selected in the spinner
			/// </summary>
			public override void Selected (UIPickerView picker, nint row, nint component)
			{
				selectedIndex = (int)row;
				if (ValueChanged != null) {
					ValueChanged (this, new EventArgs ());
				}
			}
		}
	}
}