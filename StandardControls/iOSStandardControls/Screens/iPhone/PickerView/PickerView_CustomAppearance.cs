
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;

namespace Example_StandardControls.Screens.iPhone.PickerView
{
	public partial class PickerView_CustomAppearance : UIViewController
	{
		PickerDataModel pickerDataModel;
		
		#region Constructors

		// The IntPtr and initWithCoder constructors are required for controllers that need 
		// to be able to be created from a xib rather than from managed code

        public PickerView_CustomAppearance (IntPtr handle) : base(handle)
		{
			Initialize ();
		}

		[Export("initWithCoder:")]
        public PickerView_CustomAppearance (NSCoder coder) : base(coder)
		{
			Initialize ();
		}

        public PickerView_CustomAppearance () : base("PickerView_CustomAppearance", null)
		{
			Initialize ();
		}

		void Initialize ()
		{
		}
		
		#endregion
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			this.Title = "Picker View";
			
			// create our simple picker model
			pickerDataModel = new PickerDataModel ();
			pickerDataModel.Items.Add (UIColor.Red);
			pickerDataModel.Items.Add (UIColor.Blue);
			pickerDataModel.Items.Add (UIColor.Green);
			pickerDataModel.Items.Add (UIColor.Magenta);
			
			// set it on our picker class
			this.pkrMain.Source = pickerDataModel;
			
			// wire up the value change method
			pickerDataModel.ValueChanged += (s, e) => {
				this.lblSelectedItem.Text = pickerDataModel.SelectedItem.ToString();
			};
				
			// set our initial selection on the label
			this.lblSelectedItem.Text = pickerDataModel.SelectedItem.ToString();
		}
		
		/// <summary>
		/// This is our simple picker model. It uses a list of colors as its model.
		/// </summary>
		protected class PickerDataModel : UIPickerViewModel 
		{

			public event EventHandler<EventArgs> ValueChanged;
			
			/// <summary>
			/// The color we wish to display
			/// </summary>
			public List<UIColor> Items
			{
				get { return items; }
				set { items = value; }
			}
			List<UIColor> items = new List<UIColor>();
			
			/// <summary>
			/// The current selected item
			/// </summary>
			public UIColor SelectedItem
			{
				get { return items[selectedIndex]; }
			}
			protected int selectedIndex = 0;
			
			/// <summary>
			/// default constructor
			/// </summary>
			public PickerDataModel ()
			{
			}
		
			/// <summary>
			/// Called by the picker to determine how many rows are in a given spinner item
			/// </summary>
			public override int GetRowsInComponent (UIPickerView picker, int component)
			{
				return items.Count;
			}
			
			/// <summary>
			/// called by the picker to get the number of spinner items
			/// </summary>
			public override int GetComponentCount (UIPickerView picker)
			{
				return 2;
			}
			
			/// <summary>
			/// called when a row is selected in the spinner
			/// </summary>
			public override void Selected (UIPickerView picker, int row, int component)
			{
				selectedIndex = row;
				if (this.ValueChanged != null)
				{
					this.ValueChanged (this, new EventArgs ());
				}	
			}

            /// <summary>
            /// Custom row view. 
            /// 
            /// The <c>view</c> param is the reusable view for the row. It will be null initially.
            /// 
            /// You can add subviews, etc., but prefer to do that in the lazy-initialization block rather
            /// than every time this method is called. 
            /// 
            /// Note that GetTitle() is no longer overridden since we aren't using the default row view
            /// </summary>
            public override UIView GetView(UIPickerView picker, int row, int component, UIView view)
            {
                //Lazy initialize
                if(view == null)
                {
                    SizeF rowSize = picker.RowSizeForComponent(component);
                    view = new UIView(new RectangleF(new PointF(0,0), rowSize));
                }
                //Modify state to reflect data
                view.BackgroundColor = items[row];
                return view;
            }

            /// <summary>
            /// Make the rows in the second component half the size of those in the first
            /// </summary>
            public override float GetRowHeight(UIPickerView picker, int component)
            {
                return 44 / (component % 2 + 1);
            }
		}		
	}
}

