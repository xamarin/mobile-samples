using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LiveView
{
	public partial class MainPage : ContentPage
	{
		int clickTotal = 0;

		public MainPage()
		{
			InitializeComponent();
		}

		void OnButtonClicked(object sender, EventArgs e)
		{
			clickTotal += 1;
			label.Text = String.Format("{0} button click{1}",
									   clickTotal, clickTotal == 1 ? "" : "s");
		}
	}
	}
}
