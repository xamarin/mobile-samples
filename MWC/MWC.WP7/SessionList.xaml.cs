using System;
using Microsoft.Phone.Controls;
using MWC.WP7.ViewModels;
using System.Linq;

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
                else if (NavigationContext.QueryString.ContainsKey ("favorites")) {
                    vm.FilterFavorites = true;
                }

                vm.Update ();
                DataContext = vm;
            }
            else {
                var vm = (SessionListViewModel)DataContext;
                if (vm != null && vm.FilterFavorites) {
                    vm.Update ();
                }
            }
        }

        private void HandleSessionSelectionChanged (object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var item = e.AddedItems
                .OfType<LongListSelector.LongListSelectorItem> ()
                .Where (x => x.ItemType == LongListSelector.LongListSelectorItemType.Item)
                .Select (x => x.Item)
                .OfType<SessionListItemViewModel> ()
                .FirstOrDefault ();

            if (item != null) {
                NavigationService.Navigate (new Uri ("/SessionDetails.xaml?id=" + item.ID, UriKind.RelativeOrAbsolute));
            }
        }
    }
}
