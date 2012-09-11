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
using Microsoft.Phone.Tasks;

namespace Phoneword
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        string translatedNumber;

        private void Translate_Click(object sender, RoutedEventArgs e)
        {
            if(!String.IsNullOrEmpty(PhoneNumberText.Text))
            {

                // *** SHARED CODE
                translatedNumber = Core.PhonewordTranslator.ToNumber(PhoneNumberText.Text);
                

                CallButton.Content = "Call " + translatedNumber;
                CallButton.IsEnabled = true;
            }
            else
            {
                CallButton.Content = "Call";
                CallButton.IsEnabled = false;
            }
        }

        private void Call_Click (object sender, RoutedEventArgs e)
        {
            var call = new PhoneCallTask();
            call.PhoneNumber = translatedNumber;
            call.DisplayName = PhoneNumberText.Text;
            call.Show();
        }
    }
}