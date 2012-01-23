using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using MWC.WP7.ViewModels;
using MWC.BL.Managers;

namespace MWC.WP7
{
    public partial class SessionDetails : PhoneApplicationPage
    {
        public SessionDetails ()
        {
            InitializeComponent ();
        }

        protected override void OnNavigatedTo (System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo (e);

            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.New) {
                var vm = new SessionDetailsViewModel ();

                if (NavigationContext.QueryString.ContainsKey ("id")) {
                    var id = int.Parse (NavigationContext.QueryString["id"]);
                    var session = SessionManager.GetSession (id);
                    if (session != null) {
                        vm.Update (session);
                    }
                }

                DataContext = vm;
            }
        }
    }
}
