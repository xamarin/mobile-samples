using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace PhonewordWin8
{
    public sealed partial class MainPage : Page
    {
        string translatedNumber;
        public ObservableCollection<PhonewordTranslation> Translations;

        private void Translate_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(PhoneNumberText.Text))
            {

                // *** SHARED CODE
                translatedNumber = Core.PhonewordTranslator.ToNumber(PhoneNumberText.Text);
                
                
                CallText.Text = "Call " + translatedNumber;
                // Win8, add to history list
                Translations.Add(new PhonewordTranslation() {Phoneword=PhoneNumberText.Text, Phonenumber=translatedNumber });
            }
            else
            {
                CallText.Text = "";
            }
        }

        private void ListItems_Click(object sender, ItemClickEventArgs e)
        {
            var ci = e.ClickedItem as PhonewordTranslation;
            var dialog = new MessageDialog( String.Format ("{0} translates to {1}", ci.Phoneword, ci.Phonenumber));
            dialog.ShowAsync();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ListItems.ItemsSource = Translations;
        }

        public MainPage()
        {
            this.InitializeComponent();

            Translations = new ObservableCollection<PhonewordTranslation>();

            Translations.Add(new PhonewordTranslation() { Phoneword = "1-800-flowers", Phonenumber = "1-800-3569377" });
            Translations.Add(new PhonewordTranslation() { Phoneword = "1-800-Business", Phonenumber = "1-800-28746377" });
            Translations.Add(new PhonewordTranslation() { Phoneword = "1-800-THRIFTY", Phonenumber = "1-800-8474389" });
        }

    }
}
