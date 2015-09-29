using System;
using System.Windows.Input;
using Microsoft.Phone.Controls;

namespace ContactsSample
{
	public partial class MainPage : PhoneApplicationPage
	{
		// Constructor
		public MainPage()
		{
			var vm = new MainPageViewModel();
			vm.SelectedContact += OnSelectedContact;
			DataContext = vm;
			InitializeComponent();
		}

		private void OnSelectedContact (object sender, EventArgs eventArgs)
		{
			this.pivot.SelectedIndex = 1;
		}
	}
}