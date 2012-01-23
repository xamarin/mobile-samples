using System;
using Microsoft.Phone.Controls;
using MWC.WP7.ViewModels;

namespace MWC.WP7
{
    public partial class SessionList : PhoneApplicationPage
    {
        public SessionList ()
        {
            InitializeComponent ();
        }

        protected override void OnNavigatedTo (System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo (e);

            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.New) {
                var vm = new SessionListViewModel ();

                if (NavigationContext.QueryString.ContainsKey ("dayOfWeek")) {
                    vm.FilterDayOfWeek = (DayOfWeek)Enum.Parse (typeof (DayOfWeek), NavigationContext.QueryString["dayOfWeek"], false);
                }

                vm.Update ();
                DataContext = vm;
            }
        }
    }
}
